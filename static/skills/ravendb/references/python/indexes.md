# RavenDB indexes from the Python client

Verified against `ravendb` PyPI package v7.2.3 source. Covers auto vs static indexes, defining/deploying, map-reduce, full-text, staleness.

## Auto vs static

| | Auto index | Static index |
|---|---|---|
| Created by | server, on first dynamic query (`query_collection(...)` with a filter/order) | you, deployed from code |
| Named | `Auto/Collection/By...` | class name, `_` → `/` (`Deals_ByClient` → `Deals/ByClient`) |
| Capabilities | equality/range/basic search over stored fields | computed fields, map-reduce, analyzers, stored fields, vector fields |

Dynamic queries are fine for simple filters — the server builds and reuses auto indexes. Write a static index when you need full-text analyzers, aggregation, or computed fields.

## Defining a static index

Subclass `AbstractIndexCreationTask` and set `self.map` in `__init__`. The map/reduce are **C# LINQ syntax strings** shipped to the server — the Python client has no Python-syntax index definitions.

```python
from ravendb import AbstractIndexCreationTask
from ravendb.documents.indexes.definitions import FieldIndexing, FieldStorage

class Deals_ByClientName(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = "from d in docs.Deals select new { d.clientName, d.value }"
        self._index("clientName", FieldIndexing.SEARCH)   # full-text
        self._store("value", FieldStorage.YES)            # retrievable from index w/o doc load
```

**Reading a computed field takes two things — store it, and project it.** A field the map computes does not exist on the source document, so `self._store("total", FieldStorage.YES)` plus an explicit `.select_fields(...)` is what makes the server answer from the index; without the store the projection is `None` on every row, silently. Filtering and sorting on the computed field work either way, which is why this reads as a client bug rather than a missing store. Map-reduce indexes are exempt: reduce results always come from the index, stored or not.

A stored numeric field comes back from the index as a **string** (`"650.0"`, not `650.0`) — the store keeps the indexed term, not the JSON type. Cast in the projection class (`float(total)`); formatting it with `:.2f` or doing arithmetic on it raises instead. Map-reduce results keep their numeric type.

- Field config: `self._index(field, FieldIndexing.SEARCH | EXACT | NO | DEFAULT)`, `self._store(field, FieldStorage.YES)`, `self._analyze(field, "StandardAnalyzer")`, `self._index_suggestions.add(field)`, `self._vector(field, VectorOptions(...))`.
- Multi-map: `AbstractMultiMapIndexCreationTask` with `self._add_map("...")` per collection.
- JS-syntax indexes: `AbstractJavaScriptIndexCreationTask` (set `self.maps` to a list of JS map strings).

## Deploying

```python
store.execute_index(Deals_ByClientName())
# or all at once:
store.execute_indexes([Deals_ByClientName(), Deals_ByStatus()])
# one round trip for the whole set, falling back to per-index execute:
# IndexCreation.create_indexes([...], store)   from ravendb.documents.indexes.index_creation
```

Idempotent: re-deploying an unchanged definition is a no-op; a changed one rebuilds side-by-side.

## Querying a static index

```python
hot = list(
    session.query_index("Deals/ByClientName", Deal)
    .search("clientName", "acme consulting")
)
# or by class: session.query_index_type(Deals_ByClientName, Deal)
```

Only fields emitted by the map are queryable. `search()` on a field requires `FieldIndexing.SEARCH` on it; equality on a Search field matches analyzed terms, not the exact string.

## Map-reduce

```python
class Users_CountByName(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = "from u in docs.Users select new { u.name, count = 1 }"
        self.reduce = (
            "from result in results "
            "group result by result.name into g "
            "select new { name = g.Key, count = g.Sum(x => x.count) }"
        )

class NameCount:
    def __init__(self, name: str = None, count: int = None):
        self.name = name
        self.count = count

rows = list(session.query_index("Users/CountByName", NameCount).order_by_descending("count"))
# rows are aggregates, not documents
```

Map output shape and reduce output shape must match. Optional: set `self._output_reduce_to_collection = "NameCounts"` to materialize results as documents.

Dynamic aggregation without a static index also works: `session.query(object_type=User).group_by("name").select_key().select_count().of_type(NameCount)`.

## Vector / semantic search

RavenDB 7.0+. Define a vector field in a (multi-)map static index; the app sends raw text and RavenDB embeds it with its built-in model (no external embedding service).

```python
from ravendb.documents.indexes.vector.options import VectorOptions
from ravendb.documents.indexes.vector.embedding import VectorEmbeddingType
from ravendb.documents.indexes.definitions import SearchEngineType

class Books_And_Authors_Vector_Search(AbstractMultiMapIndexCreationTask):
    def __init__(self):
        super().__init__()
        self._add_map("from b in docs.Books select new { NameVector = CreateVector(b.Title), Name = b.Title }")
        self._add_map('from a in docs.Authors select new { NameVector = CreateVector(a.FirstName + " " + a.LastName), Name = a.FirstName + " " + a.LastName }')

        # CreateVector(text) in the map + _vector(...) marks a server-side text-embedding field.
        self._vector("NameVector", VectorOptions(source_embedding_type=VectorEmbeddingType.TEXT))

        # Vector fields require the Corax engine (Lucene unsupported); scoped to this index only.
        self.search_engine_type = SearchEngineType.CORAX
```

Query — target the static-index field by name:

```python
hits = list(
    session.query_index("Books/And/Authors/Vector/Search", IndexEntry)
    .vector_search_with_text_field("NameVector", query, minimum_similarity=0.2)  # → vector.search(NameVector, $p0, 0.2, null)
    .take(20)
)
```

`vector_search_with_text_field(field, text, ...)` sends raw text against the static-index field; RavenDB embeds it server-side with the model used at index time (the static-index `vector.search(Field, $p)` form, not the auto-index `embedding.text(...)` form). RQL background: `../rql-advanced.md`.

Client gotcha: `AbstractMultiMapIndexCreationTask.create_index_definition()` does not propagate `_vector_indexes_strings`; override it to set `definition.fields[field].vector` from each entry.

## Staleness

Indexes (auto and static) update asynchronously after writes. A query immediately following `save_changes()` may return results from before the write — the response is marked stale, not delayed. Tests that write-then-query MUST wait:

```python
from datetime import timedelta

list(
    session.query_collection("Deals", Deal)
    .wait_for_non_stale_results(timedelta(seconds=5))   # per-query
    .where_equals("status", "won")
)
```

Global wait (after bulk seeding or index deploy):

```python
import time
from ravendb import GetStatisticsOperation
from ravendb.documents.indexes.definitions import IndexState

def wait_for_indexing(store):
    while True:
        stats = store.maintenance.send(GetStatisticsOperation())
        indexes = [i for i in stats.indexes if i.state != str(IndexState.DISABLED)]
        if all(not i.stale for i in indexes):
            return
        time.sleep(0.1)
```

Never use `wait_for_non_stale_results` on hot production paths — it trades latency for consistency; design UIs to tolerate slightly stale query results instead.
