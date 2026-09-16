# RavenDB indexes from the .NET client

Verified against `Raven.Client` v7.2 source. Covers auto vs static indexes, defining/deploying strongly-typed indexes, map-reduce, related documents, full-text, staleness.

## Auto vs static

| | Auto index | Static index |
|---|---|---|
| Created by | server, on first dynamic query (`session.Query<T>()` with a filter/order) | you, deployed from code |
| Named | `Auto/Collection/By...` | class name, `_` → `/` (`Deals_ByClient` → `Deals/ByClient`) |
| Capabilities | equality/range/basic search over stored fields | computed fields, map-reduce, `LoadDocument`, analyzers, stored fields |

Dynamic queries are fine for simple filters — the server builds and reuses auto indexes. Write a static index when you need full-text analyzers, aggregation, or indexing across related documents.

## Defining a static index

Subclass `AbstractIndexCreationTask<TDocument>`; set `Map` in the constructor. The expression is translated and shipped to the server: it must be self-contained (no closure captures, no local method calls).

```csharp
using Raven.Client.Documents.Indexes;

public class Deals_ByClientName : AbstractIndexCreationTask<Deal>
{
    public Deals_ByClientName()
    {
        Map = deals => from d in deals
                       select new { d.ClientName, d.Value };

        Index(x => x.ClientName, FieldIndexing.Search);   // full-text
        Store(x => x.Value, FieldStorage.Yes);            // retrievable from index w/o doc load
        Analyze(x => x.ClientName, "StandardAnalyzer");   // optional; Search implies a default analyzer
    }
}
```

**Reading a computed field takes two things — store it, and project it.** A field the map computes (`Total = Sum(...)`) does not exist on the source document, and a bare `Query<TResult, TIndex>()` **loads the document** and binds `TResult` by name — so `Total` comes back `0`/`null` for every row. Add `Store(x => x.Total, FieldStorage.Yes)` in the index *and* an explicit `.Select(x => new Result {...})` or `.ProjectInto<Result>()`, which is what makes the server answer from stored index fields. Filtering and sorting on the computed field work either way, which is why this reads as a client bug rather than a missing projection. Map-reduce indexes are exempt: reduce results always come from the index, stored or not.

The stored value on the wire is a string (`"2479.0"`); a typed `decimal`/`int` property converts it silently, so this only bites when the projection target is `object`, `dynamic`, or a `string` property that then fails to parse. Give computed fields their real numeric type.

Multi-map: subclass `AbstractMultiMapIndexCreationTask<TReduceResult>` and call `AddMap<TSource>(...)` once per collection; all maps must emit the same shape.

## Deploying

```csharp
await store.ExecuteIndexAsync(new Deals_ByClientName());
// or all at once:
await IndexCreation.CreateIndexesAsync(new IAbstractIndexCreationTask[]
    { new Deals_ByClientName(), new Deals_ByStatus() }, store);
// or every index class in an assembly:
await IndexCreation.CreateIndexesAsync(typeof(Deals_ByClientName).Assembly, store);
```

Idempotent: re-deploying an unchanged definition is a no-op; a changed one rebuilds side-by-side. Sync variants exist (`store.ExecuteIndex`, `IndexCreation.CreateIndexes`).

## Querying a static index

```csharp
var hot = await session.Query<Deal, Deals_ByClientName>()
    .Search(x => x.ClientName, "acme consulting")
    .ToListAsync();
// or by name: session.Query<Deal>("Deals/ByClientName")
```

Only fields emitted by the map are queryable. `Search()` on a field requires `FieldIndexing.Search` on it; equality on a Search field matches analyzed terms, not the exact string.

## Map-reduce

```csharp
public class Deals_CountByStatus : AbstractIndexCreationTask<Deal, Deals_CountByStatus.Result>
{
    public class Result
    {
        public string Status { get; set; }
        public int Count { get; set; }
        public decimal Total { get; set; }
    }

    public Deals_CountByStatus()
    {
        Map = deals => from d in deals
                       select new Result { Status = d.Status, Count = 1, Total = d.Value };

        Reduce = results => from r in results
                            group r by r.Status into g
                            select new Result
                            {
                                Status = g.Key,
                                Count = g.Sum(x => x.Count),
                                Total = g.Sum(x => x.Total)
                            };
    }
}

var won = await session.Query<Deals_CountByStatus.Result, Deals_CountByStatus>()
    .Where(x => x.Status == "won")
    .ToListAsync();   // rows are aggregates, not documents
```

Map output shape and reduce output shape must match (`TReduceResult`). Optional: set `OutputReduceToCollection` to materialize results as documents.

## Indexing related documents (LoadDocument)

```csharp
public class Deals_ByClientRegion : AbstractIndexCreationTask<Deal>
{
    public Deals_ByClientRegion()
    {
        Map = deals => from d in deals
                       select new
                       {
                           Region = LoadDocument<Client>(d.ClientId).Region,
                           d.Value
                       };
    }
}
```

`LoadDocument<T>(id)` inside the map creates a reference: when the Client changes, affected Deal entries re-index automatically.

## Full-text field, end to end

```csharp
Map = deals => from d in deals select new { d.Notes };
Index(x => x.Notes, FieldIndexing.Search);
```

```csharp
var risky = await session.Query<Deal, Deals_ByNotes>()
    .Search(x => x.Notes, "renewal risk")   // any term matches
    .ToListAsync();
```

## Vector / semantic search

RavenDB 7.0+. Define a vector field in a (multi-)map static index; the app sends raw text and RavenDB embeds it with its built-in model (no external embedding service).

```csharp
using Raven.Client.Documents.Indexes.Vector;
using Raven.Client.Documents.Queries.Vector;

public class Books_And_Authors_Vector_Search
    : AbstractMultiMapIndexCreationTask<Books_And_Authors_Vector_Search.IndexEntry>
{
    public class IndexEntry { public object NameVector { get; set; } public string Name { get; set; } }

    public Books_And_Authors_Vector_Search()
    {
        AddMap<Book>(books => from b in books
            select new IndexEntry { NameVector = CreateVector(b.Title), Name = b.Title });
        AddMap<Author>(authors => from a in authors
            select new IndexEntry { NameVector = CreateVector(a.FirstName + " " + a.LastName), Name = a.FirstName + " " + a.LastName });

        // CreateVector(text) in the map + Vector(...) marks a server-side text-embedding field.
        Vector("NameVector", f => f.SourceEmbedding(VectorEmbeddingType.Text));

        // Vector fields require the Corax engine (Lucene unsupported); scoped to this index only.
        SearchEngineType = SearchEngineType.Corax;
    }
}
```

Query — target the static-index field with `WithField`:

```csharp
var hits = await session
    .Query<Books_And_Authors_Vector_Search.IndexEntry, Books_And_Authors_Vector_Search>()
    .VectorSearch(
        f => f.WithField(x => x.NameVector),                              // → RQL vector.search(NameVector, $p)
        v => ((IVectorEmbeddingTextFieldValueFactory)v).ByText(query),    // embeds the query server-side
        minimumSimilarity: 0.2f)                                          // cosine, 0..1
    .Take(20)
    .ToListAsync();
```

`WithField` → static-index `vector.search(Field, $p)`; `WithText` → the auto-index `embedding.text(...)` form — use `WithField` against a static index. RQL background: `../rql-advanced.md`.

Client gotcha (7.2.0): `AbstractMultiMapIndexCreationTask<T>` does not propagate `VectorIndexesStrings` to the `IndexDefinition`; override `CreateIndexDefinition()` to copy each entry into `def.Fields[key].Vector`.

## Staleness

Indexes (auto and static) update asynchronously after writes. A query immediately following `SaveChanges()` may return results from before the write — the response is marked stale (`stats.IsStale`), not delayed. Tests that write-then-query MUST wait:

```csharp
var won = await session.Query<Deal>()
    .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
    .Where(x => x.Status == "won")
    .ToListAsync();
```

Global wait: tests built on `Raven.TestDriver` inherit `WaitForIndexing(store)` from `RavenTestDriver`. Without the test driver, poll:

```csharp
using Raven.Client.Documents.Operations;

async Task WaitForIndexingAsync(IDocumentStore store)
{
    while (true)
    {
        var stats = await store.Maintenance.SendAsync(new GetStatisticsOperation());
        if (stats.Indexes.All(i => i.State == IndexState.Disabled || i.IsStale == false))
            return;
        await Task.Delay(100);
    }
}
```

Never use `WaitForNonStaleResults` on hot production paths — it trades latency for consistency; design UIs to tolerate slightly stale query results instead.
