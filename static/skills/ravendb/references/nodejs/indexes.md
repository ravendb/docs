# RavenDB indexes from the Node.js client

Verified against `ravendb` npm package v7.2.0 source. Covers auto vs static indexes, defining/deploying JS indexes, map-reduce, related documents, full-text, staleness.

## Auto vs static

| | Auto index | Static index |
|---|---|---|
| Created by | server, on first dynamic query (`query({ collection })` with a filter/order) | you, deployed from code |
| Named | `Auto/Collection/By...` | class name, `_` → `/` (`Deals_ByClient` → `Deals/ByClient`) |
| Capabilities | equality/range/basic search over stored fields | computed fields, map-reduce, LoadDocument, analyzers, stored fields |

Dynamic queries are fine for simple filters — the server builds and reuses auto indexes. Write a static index when you need full-text analyzers, aggregation, or indexing across related documents.

## Defining a static index

`AbstractJavaScriptIndexCreationTask` (in the `ravendb` package). Subclass, call `this.map(collection, fn)` in the constructor. The map function is stringified and shipped to the server: it must be self-contained (no closure captures).

```js
const { AbstractJavaScriptIndexCreationTask } = require("ravendb");

class Deals_ByClientName extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();
        this.map("Deals", d => ({
            clientName: d.clientName,
            value: d.value
        }));
        this.index("clientName", "Search");     // full-text
        this.store("value", "Yes");             // retrievable from index w/o doc load
    }
}
```

- `this.index(field, "Search" | "Exact" | "No" | "Default")`, `this.store(field, "Yes" | "No")`, `this.analyze(field, "StandardAnalyzer")`.

**Reading a computed field takes two things — store it, and project it.** A field the map computes does not exist on the source document, so `this.store("total", "Yes")` plus an explicit `.selectFields([...])` is what makes the server answer from the index; without the store the projection is `null` on every row, silently. Filtering and sorting on the computed field work either way, which is why this reads as a client bug rather than a missing store. Map-reduce indexes are exempt: reduce results always come from the index, stored or not.

A stored numeric field comes back from the index as a **string** (`"650"`, not `650`) — the store keeps the indexed term, not the JSON type. It prints identically, so the bug surfaces later as `"650" + 10 === "65010"`; wrap it in `Number(...)` before any arithmetic or comparison. Map-reduce results keep their numeric type.

- Multi-map: `AbstractJavaScriptMultiMapIndexCreationTask` (multiple `this.map` calls).
- Raw C#-syntax maps exist via `AbstractCsharpIndexCreationTask` (`this.map = "from d in docs.Deals select new {...}"`) — prefer the JS task in a Node app.

## Deploying

```js
await store.executeIndex(new Deals_ByClientName());
// or all at once:
const { IndexCreation } = require("ravendb");
await IndexCreation.createIndexes([new Deals_ByClientName(), new Deals_ByStatus()], store);
```

Idempotent: re-deploying an unchanged definition is a no-op; a changed one rebuilds side-by-side.

## Querying a static index

```js
const hot = await session.query({ indexName: "Deals/ByClientName" })
    .search("clientName", "acme consulting")
    .all();
// or by class: session.query({ index: Deals_ByClientName })
```

Only fields emitted by the map are queryable. `search()` on a field requires `indexing: "Search"` on it; equality on a Search field matches analyzed terms, not the exact string.

## Map-reduce

```js
class Deals_CountByStatus extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();
        this.map("Deals", d => ({ status: d.status, count: 1, total: d.value }));
        this.reduce(r => r.groupBy(x => x.status).aggregate(g => ({
            status: g.key,
            count: g.values.reduce((c, v) => c + v.count, 0),
            total: g.values.reduce((t, v) => t + v.total, 0)
        })));
    }
}

const rows = await session.query({ indexName: "Deals/CountByStatus" })
    .whereEquals("status", "won")
    .all();   // rows are { status, count, total } aggregates, not documents
```

Map output shape and reduce output shape must match. Optional: `this.outputReduceToCollection = "StatusCounts"` materializes results as documents.

## Indexing related documents (LoadDocument)

```js
class Deals_ByClientRegion extends AbstractJavaScriptIndexCreationTask {
    constructor() {
        super();
        const { load } = this.mapUtils();
        this.map("Deals", d => ({
            region: load(d.clientId, "Clients").region,
            value: d.value
        }));
    }
}
```

`load(id, collection)` inside the map creates a reference: when the Client changes, affected Deal entries re-index automatically. `this.mapUtils()` only exposes stubs so the stringified function resolves; the real `load` runs server-side.

## Full-text field, end to end

```js
this.map("Deals", d => ({ notes: d.notes }));
this.index("notes", "Search");
this.analyze("notes", "StandardAnalyzer");   // optional; Search implies a default analyzer
```

```js
await session.query({ indexName: "Deals/ByNotes" })
    .search("notes", "renewal risk")      // any term matches
    .all();
```

## Vector / semantic search

RavenDB 7.0+. Define a vector field in a (multi-)map static index; the app sends raw text and RavenDB embeds it with its built-in model (no external embedding service). The map wraps the source in a vector function — `CreateVector(...)` in a C#-syntax task, `createVector(...)` from the JS map utilities in a JavaScript task (same utilities that carry `load`, `createSpatialField`, `loadVector`). Either task type works; the C#-syntax form below is the one the samples use.

```js
const { AbstractCsharpMultiMapIndexCreationTask } = require("ravendb");

class Books_And_Authors_Vector_Search extends AbstractCsharpMultiMapIndexCreationTask {
    constructor() {
        super();
        this.addMap(`from b in docs.Books select new { NameVector = CreateVector(b.Title), Name = b.Title }`);
        this.addMap(`from a in docs.Authors select new { NameVector = CreateVector(a.FirstName + " " + a.LastName), Name = a.FirstName + " " + a.LastName }`);

        // CreateVector(text) in the map + vectorField(...) marks a server-side text-embedding field.
        this.vectorField("NameVector", { sourceEmbeddingType: "Text", destinationEmbeddingType: "Single" });

        // Vector fields require the Corax engine (Lucene unsupported); the SDK auto-sets it when a vector field is present.
        this.searchEngineType = "Corax";
    }
}
```

Query — target the static-index field with `withField`:

```js
const hits = await session
    .query({ indexName: "Books/And/Authors/Vector/Search" })
    .vectorSearch(f => f.withField("NameVector"), query, { similarity: 0.2 })   // → vector.search(NameVector, $p0)
    .take(20)
    .selectFields(["Name", "Type", "DocId"])
    .all();
```

`withField` → static-index `vector.search(Field, $p)`; `withText` → the auto-index `embedding.text(...)` form — use `withField` against a static index. Passing the query string as the value factory sets `$p` to the raw text; RavenDB embeds it server-side. RQL background: `../rql-advanced.md`.

No client workaround needed — the Node SDK's `AbstractCsharpMultiMapIndexCreationTask` propagates vector field options correctly (contrast the .NET/Python 7.2 multi-map gotcha).

## Staleness

Indexes (auto and static) update asynchronously after writes. A query immediately following `saveChanges()` may return results from before the write — the response is marked stale, not delayed. Tests that write-then-query MUST wait:

```js
await session.query({ collection: "Deals" })
    .waitForNonStaleResults(5000)   // per-query, timeout in ms
    .whereEquals("status", "won")
    .all();
```

Global wait (after bulk seeding or index deploy):

```js
const { GetStatisticsOperation } = require("ravendb");
async function waitForIndexing(store) {
    while (true) {
        const stats = await store.maintenance.send(new GetStatisticsOperation());
        if (stats.indexes.every(i => i.state === "Disabled" || !i.isStale)) return;
        await new Promise(r => setTimeout(r, 100));
    }
}
```

Never use `waitForNonStaleResults` on hot production paths — it trades latency for consistency; design UIs to tolerate slightly stale query results instead.
