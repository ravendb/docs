# RavenDB .NET client — operations beyond CRUD/query

Verified against `Raven.Client` v7.2 source. Covers database management, data subscriptions, patching, attachments/counters/time series/revisions, cluster-wide transactions & compare-exchange, and advanced-query call forms.

## Database management from client

```csharp
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Raven.Client.Exceptions.Database;

store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord("Reports")));

store.Maintenance.Server.Send(new DeleteDatabasesOperation("Reports", hardDelete: true));
```

- `Maintenance.Server.Send(...)` runs server-level ops; `Maintenance.Send(...)` runs db-level ops (against `store.Database`).
- `CreateDatabaseOperation(DatabaseRecord)` or `CreateDatabaseOperation(DatabaseRecord, int replicationFactor)`. `DatabaseRecord(string name)` sets the name; leave defaults for a single-node local db.
- `DeleteDatabasesOperation(string databaseName, bool hardDelete, string fromNode = null, TimeSpan? timeToWaitForConfirmation = null)`, or pass a `DeleteDatabasesOperation.Parameters { DatabaseNames = [...], HardDelete = true }` for many.
- `GetDatabaseNamesOperation(start, pageSize)` returns `string[]` of every database on the server — the way to find leftovers to clean up, or to check existence without pulling a whole record.
- Ensure-exists / self-provision from app code — check the record, create if absent, swallow the create race:

```csharp
var record = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(store.Database));
if (record == null)
{
    try { store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(store.Database))); }
    catch (Exception) { /* concurrent create; record now exists */ }
}
```

`GetDatabaseRecordOperation(name)` returns `null` when the db does not exist. Operations against a missing db throw `DatabaseDoesNotExistException`.

## Data subscriptions

Server-side query defines a stream; a worker pulls acknowledged batches with at-least-once delivery and automatic resume from the last acknowledged position. A single worker per subscription needs no license; **concurrent** workers on one subscription are license-gated (`HasConcurrentDataSubscriptions` — verify via `GET /license/status`).

A subscription worker connects over the server's **TCP** port, not HTTP: the client reads `GET /info/tcp` and dials the address the server advertises there, so a server whose TCP port is published on a different host port than it binds sends the worker to the wrong place — the fix is in `../install.md`. Unlicensed servers cap subscriptions per database — read `MaxNumberOfSubscriptionsPerDatabase` from `GET /license/status`.

```csharp
using Raven.Client.Documents.Subscriptions;

string name = store.Subscriptions.Create<Order>(x => x.Total > 1000);
// or full control: store.Subscriptions.Create(new SubscriptionCreationOptions { Name = "big-orders", Query = "from Orders where Total > 1000" });

using var worker = store.Subscriptions.GetSubscriptionWorker<Order>(name);
await worker.Run(batch =>
{
    foreach (SubscriptionBatch<Order>.Item item in batch.Items)
    {
        Order order = item.Result;   // item.Id, item.ChangeVector also available
        Process(order);
    }
    // ack is automatic when the delegate returns; open batch.OpenSession() to write in the same unit
});
```

- `Create<T>(Expression<Func<T,bool>> predicate, ...)`, `Create<T>(SubscriptionCreationOptions<T>)`, or `Create(SubscriptionCreationOptions)` (string `Query`). Returns the subscription name.
- **A named subscription is create-once, not upsert.** `Create` with a `Name` that already exists is rejected by the server (the name is "already in use in a subscription with different Id"), so startup code that reruns must check `GetSubscriptions` first and create only when missing. An unnamed `Create` sidesteps this by letting the server generate the name, at the cost of not being able to find it again.
- `GetSubscriptionWorker<T>(name)` or `GetSubscriptionWorker<T>(SubscriptionWorkerOptions)`. `worker.Run(Action<SubscriptionBatch<T>>)` or `Run(Func<SubscriptionBatch<T>, Task>)`; both return a `Task` that completes only on a terminal error.
- `batch.OpenSession()` gives a session already scoped to the batch — writes there commit together with the ack.
- Reconnection: the worker reconnects automatically on transient drops (`SubscriptionWorkerOptions.TimeToWaitBeforeConnectionRetry`). The returned `Task` faults on terminal conditions — `SubscriptionClosedException`, `SubscriptionInvalidStateException`, `SubscriptionDoesNotExistException`; loop-and-recreate only for those. Only one worker processes a subscription at a time unless `SubscriptionWorkerOptions.Strategy` allows concurrency.

## Patching (server-side, no load)

```csharp
session.Advanced.Patch<Order, string>("orders/1-A", x => x.Status, "shipped");
session.Advanced.Increment<Order, int>("orders/1-A", x => x.Version, 1);
session.SaveChanges();   // patches batch with the rest of the unit of work
```

- Signatures: `Patch<T,U>(string id | T entity, Expression<Func<T,U>> path, U value)`; array form `Patch<T,U>(id, Expression<Func<T,IEnumerable<U>>> path, Expression<Func<JavaScriptArray<U>,object>> arrayAdder)`. `Increment<T,U>(string id | T entity, Expression<Func<T,U>> path, U valToAdd)`. Also `AddOrPatch<T,TU>(id, entity, path, value)` / `AddOrIncrement<T,TU>(id, entity, path, valToAdd)` for upsert semantics.
- Bulk conditional update across a collection — run a JS patch script server-side (no documents travel to the client):

```csharp
using Raven.Client.Documents.Operations;

var op = store.Operations.Send(new PatchByQueryOperation(
    "from Orders as o where o.Status = 'pending' update { o.Status = 'stale'; }"));
op.WaitForCompletion();   // PatchByQueryOperation returns an Operation; await/WaitForCompletion for the result
```

`PatchByQueryOperation(string queryToUpdate)` or `PatchByQueryOperation(IndexQuery, QueryOperationOptions)`. The `update { ... }` clause is JavaScript; parameterize via `IndexQuery.QueryParameters` rather than string-concatenating values.

## Attachments, counters, time series, revisions

All accessed off `session.Advanced` (or directly on the session for counters/time series); changes flush on `SaveChanges()`.

```csharp
// Attachments — binary streams attached to a document
session.Advanced.Attachments.Store("orders/1-A", "invoice.pdf", stream, "application/pdf");
AttachmentResult a = session.Advanced.Attachments.Get("orders/1-A", "invoice.pdf");   // a.Stream, a.Details
bool has = session.Advanced.Attachments.Exists("orders/1-A", "invoice.pdf");

// Counters — distributed, conflict-free numeric values
session.CountersFor("orders/1-A").Increment("views", 1);
long? views = session.CountersFor("orders/1-A").Get("views");
Dictionary<string, long?> all = session.CountersFor("orders/1-A").GetAll();

// Time series — timestamped numeric measurements per named series
session.TimeSeriesFor("orders/1-A", "Heartrate").Append(DateTime.UtcNow, 72, tag: "watch/1");
TimeSeriesEntry[] entries = session.TimeSeriesFor("orders/1-A", "Heartrate").Get(from: DateTime.UtcNow.AddHours(-1), to: DateTime.UtcNow);

// Revisions — read-only history (requires revisions configured for the collection)
List<Order> history = session.Advanced.Revisions.GetFor<Order>("orders/1-A", start: 0, pageSize: 25);
Order at = session.Advanced.Revisions.Get<Order>("orders/1-A", DateTime.UtcNow.AddDays(-1));
List<IMetadataDictionary> meta = session.Advanced.Revisions.GetMetadataFor("orders/1-A");
```

- Attachments: `Store(documentId|entity, name, Stream, contentType = null)`; `Get(...)` returns `AttachmentResult` (dispose it — it holds the stream); `GetRange`, `GetRevision`, `Exists` also available.
- Counters `Increment(counter, delta = 1)` / `Delete(counter)`; deltas merge across nodes.
- Time series `Append(DateTime, double | IEnumerable<double>, tag = null)`, `Delete(from, to)` / `Delete(at)`; `Get(from, to, start, pageSize)` returns `TimeSeriesEntry[]`. Typed variants via `TimeSeriesFor<T>`.
- Revision retention is license-capped: a configuration above `MaxNumberOfRevisionsToKeep` / `MaxNumberOfRevisionAgeToKeepInDays` is refused with HTTP 402 and `Raven.Client.Exceptions.Commercial.LicenseLimitException` — read both from `GET /license/status` before choosing numbers. A database-wide `Default` configuration is refused outright on an unlicensed server whatever the numbers (`CanSetupDefaultRevisionsConfiguration` is `false` there); configure per collection instead.
- Revisions `GetFor<T>` defaults to `pageSize: 25`; `Get<T>(changeVector)` or `Get<T>(id, DateTime)` fetch one point-in-time version.

## Cluster-wide transactions & concurrency

```csharp
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Operations.CompareExchange;

using var session = store.OpenSession(new SessionOptions { TransactionMode = TransactionMode.ClusterWide });

// atomic reservation — succeeds only if the key does not already exist cluster-wide
CompareExchangeValue<string> reserved =
    session.Advanced.ClusterTransaction.CreateCompareExchangeValue("emails/jane@acme.com", "users/1-A");

session.Store(new User { Email = "jane@acme.com" }, "users/1-A");
session.SaveChanges();   // throws ClusterTransactionConcurrencyException if the key was taken
```

- `TransactionMode.ClusterWide` routes the batch through Raft consensus — all-or-nothing across nodes, at the cost of latency. `TransactionMode.SingleNode` is the default.
- Compare-exchange (the cluster's concurrency primitive): `CreateCompareExchangeValue<T>(key, value)`, `GetCompareExchangeValueAsync<T>(key)`, `DeleteCompareExchangeValue(key, long index)` / `DeleteCompareExchangeValue<T>(CompareExchangeValue<T>)`. `CompareExchangeValue<T>` exposes `Key`, `Value`, and `Index` (the version — 0 means "must not exist yet").
- **Optimistic concurrency** (single-node mode) makes `SaveChanges()` fail on a stale change vector — `ConcurrencyException`. Which API you get depends on the **client package version**, so read it off the manifest first:
  - `session.Advanced.OptimisticConcurrencyMode = OptimisticConcurrencyMode.Writes` — the current API, **(client 7.2.2+)**. Also `WritesAndReads` (tracks loaded entities too); default `None`.
  - `session.Advanced.UseOptimisticConcurrency = true` — use only on a client older than 7.2.2. From 7.2.2 it still compiles but is `[Obsolete]` and fails the build under `TreatWarningsAsErrors` (`CS0618`), and setting both on one session throws.

  Neither combines with `ClusterWide` (compare-exchange is the cluster-wide equivalent).

## Advanced queries — call form only

RQL semantics and full syntax live in the shared files; from C# these are LINQ extensions on `IQueryable<T>` / `IDocumentQuery<T>`:

| Feature | Call form | RQL |
|---|---|---|
| Facets / aggregation | `query.AggregateBy(builder)` or `.AggregateUsing(facetSetupId)` → `.Execute()` returns `Dictionary<string, FacetResult>` | `../rql-facets.md` |
| Suggestions | `query.SuggestUsing(builder)` → `ISuggestionQuery<T>` | `../rql-advanced.md` |
| More-like-this | `query.MoreLikeThis(builder)` → `IRavenQueryable<T>` | `../rql-advanced.md` |
| Spatial | `query.Spatial(x => x.Location, c => c.WithinRadius(km, lat, lng))` | `../rql-spatial.md` |

`AggregateBy` chains `AndAggregateBy(...)` for multiple facets; both facet paths terminate in `Execute()`/`ExecuteAsync()`. Do not hand-write RQL for these when the strongly-typed builder covers the case.
