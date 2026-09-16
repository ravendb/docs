# RavenDB Python client — core usage

Verified against `ravendb` PyPI package v7.2.3 source (requires Python >= 3.9). Covers store/session lifecycle, CRUD, querying, includes, bulk insert.

## Setup

```python
from ravendb import DocumentStore

store = DocumentStore("http://127.0.0.1:8080", "ConsultingDeals")
store.initialize()
# on shutdown:
store.close()
```

- One `DocumentStore` per app; also usable as a context manager (`with DocumentStore(...) as store:`). `initialize()` is mandatory before any use; conventions are frozen at that point.

## Secured server (certificate)

```python
store = DocumentStore("https://a.myapp.ravendb.cloud", "ConsultingDeals")
store.certificate_pem_path = os.environ["RAVENDB_CERT_PEM"]   # cert + key in ONE .pem
store.trust_store_path = os.environ.get("RAVENDB_CA_PEM")     # only for a self-signed CA
store.initialize()
```

- **PEM only** — the setter raises `ValueError` on a `.pfx` path, and also on a PEM missing either the `BEGIN CERTIFICATE` or the `PRIVATE KEY` block, so the certificate and its key must live in the same file. Convert a pfx once: `openssl pkcs12 -in client.pfx -out client.pem -nodes`.
- Both must be set before `initialize()` (`certificate_pem_path` asserts it). URL must be `https://` — the mismatch is rejected by the client at initialization, not by the server. Never commit the pem — path from the environment.

## Entities: classes vs dicts

- **Class instances**: collection = pluralized class name (`User` → `Users`, via `inflect`). Deserialization calls `YourClass(**fields)` — the `__init__` keyword args must match the stored JSON property names, or define a `from_json` classmethod.
- The identity property is the attribute `Id` (capital I). The client sets it after `store()`; it is not part of the document body.
- To read the id of a document you **queried** rather than stored, use `session.advanced.get_document_id(entity)`. It works on any tracked entity, including a plain `dict` result, which has no `Id` key of its own. Projections are not tracked, so project `id()` in the query instead.
- **Plain dicts**: collection is derived from the pluralized document-key prefix (`session.store({...}, "clients/1")` → collection `clients`). A dict stored *without* an explicit key lands in collection `dicts` — always pass an explicit `"collection/..."` id for dicts.

## Session lifecycle

```python
with store.open_session() as session:
    session.store(User(name="Acme"))            # id auto: "users/1-A" — the prefix is lower-cased
    session.store(user, "users/42")             # explicit id
    session.store(user, "users/")               # "/" suffix: server assigns id tail
    session.save_changes()                      # one batch request; nothing hits the server before this
```

- A session is a unit of work with an identity map. It holds every document it received in full (`load`, an include, a query without `select_fields`), keyed by id, with a copy of the JSON as it arrived. `save_changes()` diffs the tracked documents against those copies and sends the differences, plus anything stored or deleted, in one batch; nothing else reaches the server.
- Not in the map, so not tracked: projections (`select_fields`, `raw_query` with `select`), streams, aggregations, facets. Mutating one and calling `save_changes()` writes nothing and raises nothing; `get_document_id` and `get_metadata_for` cannot see it either. To change documents without loading them in full, patch (`operations.md`).
- Limit: 30 requests per session (`store.conventions.max_number_of_requests_per_session`); exceeding raises. Open a new session instead of raising it. Current count: `session.advanced.number_of_requests`.
- `session.delete(entity)` or `session.delete("users/1-A")`, then `save_changes()`.
- `session.load("users/1-A", User)` returns the entity or `None`; same id twice in one session hits the cache. Load many: `session.load(["users/1-A", "users/2-A"], User)` returns `{id: entity_or_None}`.
- `session.load_starting_with("users/", User, page_size=25)` pages by id prefix.

## Metadata

```python
md = session.advanced.get_metadata_for(entity)   # tracked entity only
md["@collection"]                                # collection name
md["customKey"] = "v"                            # mutate + save_changes() persists
```

## Querying

```python
results = list(
    session.query_collection("Users", User)
    .where_equals("status", "active")
    .where_in("region", ["EU", "US"])
    .where_between("age", 18, 65)
    .order_by("name")            # order_by_descending(...) for descending
    .skip(20).take(10)
)

n = session.query_collection("Users").count()
one = session.query_collection("Users", User).first()   # also .single()
```

- `session.query()` takes only `source` and `object_type`. There is no `collection_name` or `index_name` keyword on it, and passing one raises `TypeError: DocumentSession.query() got an unexpected keyword argument`. Name a collection with `query_collection`, an index with `query_index`.
- `query_collection(name, Type)` is a dynamic query (auto index); `query_index("Users/ByName", Type)` targets a static index; `session.query(object_type=User)` infers collection from the class; `session.advanced.raw_query("from users where age > $a", User).add_parameter("a", 18)` runs raw RQL.
- Execute by iterating (`list(query)`), or `.first()` / `.single()` / `.count()`.
- **Sorting on a number or a date needs the ordering type as a second argument.** `order_by`/`order_by_descending` name the field as a string and default to `OrderingType.STRING`, so `.order_by_descending("amount")` puts `99952` above `243889170`. Pass `OrderingType.FLOAT` for numbers (the member is `FLOAT`, there is no `DOUBLE`), `OrderingType.LONG`, or `OrderingType.ALPHA_NUMERIC`: `.order_by_descending("amount", OrderingType.FLOAT)`. Import it from `ravendb`. This returns wrong rows with no error. Do **not** pass the ordering as a plain string: the second argument is `sorter_name_or_ordering_type`, so `.order_by_descending("amount", "Double")` (the form that is correct in the Node client) is read as the name of a custom sorter and the server answers `NotSupportedInCoraxException: Corax doesn't support Custom OrderBy`.
- Query stats: `.statistics(lambda st: ...)` takes a callback, not an out-parameter; the `QueryStatistics` it hands you has `.total_results` (how many the query matched overall, regardless of `skip`/`take`) and `.is_stale`. Import `QueryStatistics` from `ravendb`.
- `raw_query` needs `object_type` whenever the RQL projects (`select ...`). Without it the client raises `AttributeError: 'NoneType' object has no attribute '__dict__'` from inside its deserializer, which names neither the query nor the missing argument. `session.advanced.raw_query(rql, dict)` is the general form.
- Large result sets: `session.advanced.stream(query)` returns a plain **iterator**, not a context manager, so iterate it directly rather than with `with`. Each item is a `StreamResult` read by attribute (`item.document`, `item.key`, `item.metadata`), not by subscript. Results are not tracked.
- Default operator between `where_*` clauses is AND; `.using_default_operator(QueryOperator.OR)` must come before any condition. Explicit: `.and_also()` / `.or_else()` / `.not_()` / `.open_subclause()...close_subclause()`.
- `where(name="John", age=3)` sugar exists but joins the kwargs with OR — prefer explicit `where_equals` chains.
- Full-text: `.search("description", "term1 term2")` (terms OR-ed; `SearchOperator.AND` as third arg, imported from `ravendb.documents.queries.misc`).
- Projection: `.select_fields(UserProjection, "name", "age")` — fields default to the projection class's `__init__` fields when omitted.
- Also available: `.distinct()`, `.where_starts_with`, `.where_greater_than(_or_equal)`, `.where_less_than(_or_equal)`, `.where_exists`, `.contains_any` / `.contains_all`, `.group_by(...)`, `.of_type(Cls)`, `.lazily()`.
- Field names in queries are the literal JSON property names — the client does no case conversion; use exactly what you stored.

## include — avoid N+1

```python
clients = session.include("deal_ids").load("clients/1-A", Client)   # returns {id: entity}
client = clients["clients/1-A"]
deal = session.load(client.deal_ids[0], Deal)   # already cached, no extra request
```

Query-side: `session.query_collection("Deals", Deal).include("client_id")`.

Include paths are resolved on what the query returns. On a projected query the include is silently dropped unless the field it walks is in the projection: `select DealName, AccountId include AccountId` includes the accounts, `select DealName include AccountId` includes nothing and every later load is a request.

## Bulk insert

```python
with store.bulk_insert() as bulk:
    for row in rows:
        bulk.store(entity)                 # returns the assigned key
        bulk.store_as(entity, "deals/7")   # explicit key
```

Streams to the server; far faster than session `store()` loops for seeding. Exiting the `with` block flushes and finishes the operation.

## Staleness

Queries run on indexes that update asynchronously; a query right after `save_changes()` may return stale results. Details and wait patterns: `indexes.md`. Test-suite discipline: `../testing.md`; the driver and per-test database call forms: `testing.md`.

## Gotchas

- Forgetting `store.initialize()` → errors on first operation. Nothing persists without `save_changes()` (or leaving the bulk-insert block).
- Entity classes whose `__init__` params don't match document fields fail to deserialize (`Utils.initialize_object` calls `Type(**fields)`).
- Dicts without an explicit `"collection/..."` id land in the `dicts` collection.
- 30-requests-per-session cap.
- Without `object_type`, both `load` and queries return a `DynamicStructure`, whose fields are read as **attributes** (`d.Amount`); it is not a dict and subscripting it raises `TypeError: 'DynamicStructure' object is not subscriptable`. Pass `dict` explicitly to get plain dicts, or a class to get instances.
