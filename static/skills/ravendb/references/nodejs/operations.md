# RavenDB Node.js client — operations beyond CRUD

Verified against `ravendb` npm package v7.2.0 source. Covers self-provisioning databases, subscriptions, patching, attachments/counters/time series/revisions, cluster-wide transactions, and the call form for advanced queries.

All named types below are imported from the top-level package: `const { CreateDatabaseOperation, ... } = require("ravendb")`.

## Database management (self-provision)

```js
const { CreateDatabaseOperation, DeleteDatabasesOperation, GetDatabaseNamesOperation } = require("ravendb");

// ensure-exists: create only if missing (idempotent app startup)
const names = await store.maintenance.server.send(new GetDatabaseNamesOperation(0, 100));
if (names.includes("ConsultingDeals") === false) {
    await store.maintenance.server.send(new CreateDatabaseOperation({ databaseName: "ConsultingDeals" }, 1));
}

// delete (hardDelete: true also erases data files, not just the record)
await store.maintenance.server.send(new DeleteDatabasesOperation({
    databaseNames: ["ConsultingDeals"], hardDelete: true
}));
```

- `store.maintenance.server.send(...)` targets the cluster; `store.maintenance.send(...)` (no `.server`) targets the store's database. Server-level ops (create/delete DB) MUST go through `.server`.
- `CreateDatabaseOperation(record, replicationFactor)` — the record is a `DatabaseRecord`; only `databaseName` is required. Second overload takes a builder: `new CreateDatabaseOperation(b => b.regular("Name"))`.
- Sending an op against a nonexistent DB throws `DatabaseDoesNotExistException` (catch by `err.name`).
- `GetDatabaseRecordOperation(name)` returns the full record (`null` if absent) — heavier than the names list; use the names list for a plain existence check.

## Data subscriptions

Server-side query defines what to stream; a worker consumes batches and acks per batch. A single worker per subscription needs no license; **concurrent** workers on one subscription are license-gated (`HasConcurrentDataSubscriptions` — verify via `GET /license/status`).

```js
const subscriptionName = await store.subscriptions.create({
    query: "from Deals where value > 10000"
});

const worker = store.subscriptions.getSubscriptionWorker({
    subscriptionName,
    maxDocsPerBatch: 20,        // default hands you everything matching in one batch
    closeWhenNoDocsLeft: false  // true: drain, then end with SubscriptionClosedException — for a one-shot job
});

worker.on("batch", (batch, callback) => {
    try {
        for (const item of batch.items) {
            // item.id, item.result (typed doc), item.rawResult, item.rawMetadata, item.changeVector
            process(item.result);
        }
        callback();          // ack -> server sends the next batch
    } catch (err) {
        callback(err);       // nack -> batch is retried
    }
});

worker.on("error", err => { /* connection/processing errors */ });
// other events: "end", "unexpectedSubscriptionError", "afterAcknowledgment", "connectionRetry"
// "end" fires with no argument on a clean dispose and after a fatal error alike, so it tells you
// nothing about why; a wrong-address failure ends the worker instead of entering "connectionRetry",
// so a supervisor that only watches "connectionRetry" waits forever.

// worker.dispose() to stop consuming; delete server-side task:
// await store.subscriptions.delete(subscriptionName);
```
**A named subscription is create-once, not upsert.** The example above lets the server generate the name. Pass your own `name` and the call is rejected on the second run (the name is "already in use in a subscription with different Id"), so bootstrap code that reruns must list first and create only when missing.


- `create()` returns the generated name; pass `{ name, query }` to name it yourself. Options also accept a document type for typed items. There is no create-if-missing overload: on a named subscription check `getSubscriptions(0, 100)` (each returned state carries the name as `subscriptionName`) or `getSubscriptionState(name)` first, and use `update({ name, query })` to change an existing one — a re-`create()` on a taken name is not idempotent. Unlicensed servers cap subscriptions per database — read `MaxNumberOfSubscriptionsPerDatabase` from `GET /license/status`.
- The subscription connects over the server's **TCP** port, not HTTP: the client reads `GET /info/tcp` and dials what the server advertises there. A server whose TCP port is published on a different host port than it binds (a Docker remap) sends the client to the wrong place, and the error names a missing database rather than a wrong port — `../install.md` has the fix.
- Progress is server-tracked per subscription: a new worker resumes after the last acked document. Exactly-once-ish — a crash before ack replays the batch, so make `process()` idempotent.
- One active worker per subscription by default; a second connection waits or fails depending on the subscription's strategy.

## Live change notifications (Changes API)

`store.changes()` is an ephemeral pub/sub feed of document/index changes — no server-side task, no license gate, no resume-after-restart (contrast the durable subscriptions above). It is the right tool for pushing live updates to connected clients (SSE, WebSocket); use a data subscription instead when you need durable, acked, exactly-once-ish processing.

```js
const changes = store.changes();
const sub = changes
    .forDocumentsInCollection("Products")     // or forDocument(id) / forDocumentsStartingWith(prefix) / forAllDocuments() / forIndex(name)
    .on("data", change => {
        // change.id, change.type ("Put" | "Delete"), change.collectionName
        pushToClient(change);
    });
changes.on("error", err => { /* connection-level errors */ });
await changes.ensureConnectedNow();           // optional: await the initial connection
```

Disposal is mandatory and is the part that is easy to get wrong:

```js
// When the consumer goes away (e.g. SSE request closes):
sub.dispose();        // stop THIS observable
// dispose the whole connection only when no observable on it is still needed:
// changes.dispose();
```

- Keep the handle returned by `.on(...)` and call `sub.dispose()`. Calling `emitter.off("data", () => {})` with a **fresh** function removes nothing — the original listener (and the underlying connection observer) leaks. Every connect/disconnect cycle that fails to dispose leaks a subscription.
- One `store.changes()` multiplexes many observables over a single connection; dispose each observable you created. In an SSE handler, register the `dispose()` calls on the request's `close` event.

## Patching (server-side, no document load)

Session-level shorthands batch into `saveChanges()`:

```js
session.advanced.increment("users/1", "age", 1);        // atomic numeric delta
session.advanced.patch("users/1", "underAge", false);   // set a single field
await session.saveChanges();
```

Script patch (single doc) and conditional bulk patch (by query) via operations:

```js
const { PatchOperation, PatchRequest, PatchByQueryOperation } = require("ravendb");

// one document, arbitrary JS script
await store.operations.send(new PatchOperation(
    "users/1", null, PatchRequest.forScript(`this.name = "Patched"`)));

// bulk conditional update — RQL update clause, runs server-side over matches
const op = await store.operations.send(
    new PatchByQueryOperation(`from Users where age < 18 update { this.underAge = true }`));
await op.waitForCompletion();   // background operation; await it in tests
```

- `PatchOperation(id, changeVector, patchRequest)` — pass a change vector for optimistic concurrency, `null` to skip.
- `PatchByQueryOperation` runs asynchronously on the server and returns an operation handle; `waitForCompletion()` blocks until done and surfaces script errors. It resolves to `undefined`, not a count of patched documents — verify with a fresh query if you need the number.

## Attachments / counters / time series / revisions

Attachments (binary streams tied to a document):

```js
session.advanced.attachments.store(doc, "photo.png", fileStream, "image/png"); // or (docId, ...)
await session.saveChanges();
const att = await session.advanced.attachments.get("users/1", "photo.png");    // att.data is a Readable
await session.advanced.attachments.exists("users/1", "photo.png");
session.advanced.attachments.delete("users/1", "photo.png");                    // then saveChanges()
```

Counters (distributed atomic numbers):

```js
session.countersFor("users/1").increment("Likes", 10);   // delta defaults to 1
await session.saveChanges();
const likes = await session.countersFor("users/1").get("Likes");   // getAll() for the map
session.countersFor("users/1").delete("Likes");
```

Time series (append `(timestamp, value(s), tag?)`):

```js
const tsf = session.timeSeriesFor("users/1", "heartbeat");
tsf.append(new Date(), 120, "watches/fitbit");   // value may be a number or number[]
await session.saveChanges();
const entries = await tsf.get();                 // get(from, to) to bound the range
```

Revisions (read-only history; enable via config first):

```js
const revisions = await session.advanced.revisions.getFor("users/1");   // newest-first array
const meta = await session.advanced.revisions.getMetadataFor("users/1");
```

Revisions only appear for documents that landed in a real collection — an object literal stored without `findCollectionNameForObjectLiteral` (see `client.md`) goes to `@empty`, and `getFor` then returns an empty array with no error to explain it.

Enable revisions per database (maintenance op, not per-session):

```js
const { ConfigureRevisionsOperation, RevisionsConfiguration, RevisionsCollectionConfiguration } = require("ravendb");
const configuration = Object.assign(new RevisionsConfiguration(), {
    defaultConfig: Object.assign(new RevisionsCollectionConfiguration(), { disabled: false, minimumRevisionsToKeep: 2 }),
    collections: new Map([["Users", Object.assign(new RevisionsCollectionConfiguration(), { disabled: false })]])
});
await store.maintenance.send(new ConfigureRevisionsOperation(configuration));
```

`minimumRevisionsToKeep` above the licensed cap is refused with HTTP 402 and a message ending "exceeds the licensed one", and the unlicensed cap is low enough that any round number trips it — read `MaxNumberOfRevisionsToKeep` from `GET /license/status` and stay inside it. There is no `LicenseLimitException` in this client's error names, so there is nothing to match on by type: check the cap up front rather than catching. A `defaultConfig` is refused outright on an unlicensed server whatever the number (`CanSetupDefaultRevisionsConfiguration` is `false` there) — configure per collection instead.

Reading a specific revision, and reverting:

```js
const revisions = await session.advanced.revisions.getFor("contracts/1");        // newest first
const metadata  = await session.advanced.revisions.getMetadataFor("contracts/1"); // change vectors live here
const asOf      = await session.advanced.revisions.get(changeVector);             // or .get(id, date)
const bytes     = await session.advanced.attachments.getRevision("contracts/1", "contract.pdf", changeVector);
```

There is no revert operation in the client: reverting means loading the revision, copying its fields onto the live document, and calling `saveChanges()`. Deleting a document deletes its live attachments and adds a `DeleteRevision`-flagged tombstone revision carrying none, while earlier content revisions keep their own attachment copies — `attachments.getRevision` still retrieves those bytes.

## Cluster-wide transactions & concurrency

```js
const session = store.openSession({ transactionMode: "ClusterWide" });

// compare-exchange: cluster-consistent key/value, ideal for uniqueness/reservations
const cmp = session.advanced.clusterTransaction.createCompareExchangeValue("emails/john@x.com", "users/1");
await session.store({ name: "John" }, "users/1");
await session.saveChanges();   // whole tx is Raft-committed atomically across the cluster

const existing = await session.advanced.clusterTransaction.getCompareExchangeValue("emails/john@x.com"); // .value, .index
// session.advanced.clusterTransaction.deleteCompareExchangeValue(existing); // or (key, index)
```

Optimistic concurrency (single-node, change-vector based):

```js
session.advanced.useOptimisticConcurrency = true;   // whole session; saveChanges throws ConcurrencyException on stale write
```

- Cluster-wide sessions are the only place compare-exchange values participate in a document transaction; a normal session can still read/write them via `store.operations` compare-exchange ops.
- Optimistic concurrency compares the loaded change vector on save; conflicts throw rather than overwrite. `PatchOperation`/`session.delete` can take an explicit change vector for the same guarantee on a single call. The boolean above is the only form in the Node client — the `OptimisticConcurrencyMode` enum the .NET and Python clients gained in 7.2.2 has no Node equivalent, so do not port that shape across.

## Advanced queries — call form only

RQL syntax for each lives in the shared files; only the client call shape is shown here.

Faceted / aggregation — see `../rql-facets.md`:

```js
const result = await session.query({ index: Orders_All })
    .aggregateBy(f => f.byField("region").sumOn("total").maxOn("total"))
    .execute();                       // result["region"].values -> [{ range, count, sum, max }]
// or from a stored FacetSetup document: .aggregateUsing("facets/orders")
```

Suggestions — see `../rql-advanced.md`:

```js
const suggestions = await session.query({ index: Users_ByName })
    .suggestUsing(b => b.byField("name", "Oren").withOptions(opts))
    .execute();
```

More-like-this — see `../rql-advanced.md`:

```js
await session.query({ index: Articles_ByBody })
    .moreLikeThis(f => f.usingDocument(x => x.whereEquals("id()", "articles/1")).withOptions(opts))
    .all();
```

Spatial — see `../rql-spatial.md`:

```js
const { PointField, WktField } = require("ravendb");
await session.query({ index: Events_ByLocation })
    .spatial(new PointField("latitude", "longitude"), c => c.withinRadius(20, 32.56, 34.95))
    .all();
// dynamic field form: .spatial("location", c => c.withinRadius(...))
```

Projections/aggregation over query results: `.selectFields(...)` (see `client.md`); grouped aggregation is map-reduce index territory (see `indexes.md`).
