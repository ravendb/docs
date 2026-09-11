# RavenDB Python client — operations beyond CRUD/query

Verified against `ravendb` PyPI package v7.2.3.post1 source (requires Python >= 3.9, `requests`-based). Covers database management, subscriptions, patching, attachments/counters/time-series/revisions, cluster-wide transactions, and advanced-query call forms.

## Two dispatch surfaces

| Executor | Reached via | Scope |
|---|---|---|
| `store.maintenance.send(op)` | database-level maintenance ops (indexes, stats, revisions config) | current (or `.for_database(name)`) db |
| `store.maintenance.server.send(op)` | server/cluster ops (create/delete db, db names) | whole server |
| `store.operations.send(op)` | data ops (patch, compare-exchange, attachments) | current db |

`store.operations.send_async(op)` returns an `Operation`; call `.wait_for_completion()` for long-running ops (patch-by-query, delete-by-query).

## Database management from the client

```python
from ravendb import CreateDatabaseOperation, GetDatabaseNamesOperation
from ravendb.serverwide.database_record import DatabaseRecord
from ravendb.serverwide.operations.common import DeleteDatabaseOperation

record = DatabaseRecord("ConsultingDeals")
store.maintenance.server.send(CreateDatabaseOperation(record, replication_factor=1))

# ensure-exists:
if "ConsultingDeals" not in store.maintenance.server.send(GetDatabaseNamesOperation(0, 100)):
    store.maintenance.server.send(CreateDatabaseOperation(DatabaseRecord("ConsultingDeals")))

store.maintenance.server.send(DeleteDatabaseOperation("ConsultingDeals", hard_delete=True))
```

- `DatabaseRecord(database_name)` — plain object; set `.disabled`, `.settings`, `.encrypted` etc. before create. Neither `DatabaseRecord` nor `DeleteDatabaseOperation` is re-exported at package root (`CreateDatabaseOperation` and `GetDatabaseNamesOperation` are) — import them from `ravendb.serverwide.database_record` and `ravendb.serverwide.operations.common`, or the import fails.
- There is no separate `DeleteDatabasesOperation`. To delete several, pass `DeleteDatabaseOperation(parameters=DeleteDatabaseOperation.Parameters(database_names=[...], hard_delete=True))`.
- `hard_delete=True` wipes files; `False` leaves them on disk. `from_node`/`from_nodes` remove from specific nodes only.

## Data subscriptions

Server-side query defines the stream; a worker consumes acknowledged batches. A single worker per subscription needs no license; **concurrent** workers on one subscription are license-gated (`HasConcurrentDataSubscriptions` — verify via `GET /license/status`).

A subscription worker connects over the server's **TCP** port, not HTTP: the client reads `GET /info/tcp` and dials the address the server advertises there, so a server whose TCP port is published on a different host port than it binds sends the worker to the wrong place — the fix is in `../install.md`. Unlicensed servers cap subscriptions per database — read `MaxNumberOfSubscriptionsPerDatabase` from `GET /license/status`.

```python
from ravendb.documents.subscriptions.options import SubscriptionCreationOptions, SubscriptionWorkerOptions

name = store.subscriptions.create_for_class(User)                       # all Users
name = store.subscriptions.create_for_options(
    SubscriptionCreationOptions(query="from Users where age > 18"))     # RQL filter/projection

worker = store.subscriptions.get_subscription_worker(SubscriptionWorkerOptions(name), User)
future = worker.run(lambda batch: [handle(item.result) for item in batch.items])
future.result()          # blocks; batch loop runs on a background thread until closed
worker.close()
```

- `create_for_class(cls, options=None)` / `create_for_options(options)` / `create_for_class` return the subscription name (str). Options `query` is RQL; `None` query on `create_for_options` raises.
- `worker.run(process_batch)` returns a `concurrent.futures.Future`; the callback receives a `SubscriptionBatch` — iterate `batch.items`, each `item.result` is the deserialized entity (`item.key`, `item.change_vector`, `item.metadata` also available). Accessing `item.result` re-raises a per-item processing error.
- `worker.add_after_acknowledgment(handler)` fires once a batch is acked. Server tracks progress by change vector, so a new worker resumes where the last left off.
- **A named subscription is create-once, not upsert.** Calling `create_for_options` again with a name that already exists is rejected by the server (`RachisApplyException`, the name is "already in use in a subscription with different Id"), so bootstrap code that reruns must list first and create only when missing.
- Other: `get_subscription_worker_by_name(name, cls)`, `get_subscription_worker_for_revisions(...)`, `store.subscriptions.get_subscriptions(start, take)`, `drop_connection(name)`, `delete(name)`. `SubscriptionCreationOptions`/`SubscriptionWorkerOptions` import from `ravendb.documents.subscriptions.options` (not top-level). `SubscriptionCreationOptions(name=…)` names a subscription deterministically, which is what makes the create-if-missing check above possible.

## Patching

Session-level sugar defers a patch into the next `save_changes()`:

```python
session.advanced.increment("users/1", "visits", 1)              # atomic numeric add
session.advanced.patch("users/1", "name", "Patched")            # set a field
session.advanced.patch_array("users/1", "tags", lambda a: a.add("vip"))
session.advanced.patch_object("users/1", "props", lambda m: m.put("k", "v"))
session.save_changes()
```

Operations-level, arbitrary JS, no session:

```python
from ravendb import PatchOperation, PatchByQueryOperation, PatchRequest

store.operations.send(PatchOperation("users/1", None, PatchRequest.for_script('this.name = "Patched"')))

op = store.operations.send_async(PatchByQueryOperation('from Users where age < 18 update { this.minor = true }'))
op.wait_for_completion()
```

- `PatchRequest.for_script(js)` or set `.script` + `.values` (dict, referenced as `args.<key>` in the script). `PatchByQueryOperation` accepts a raw RQL `update {}` string or an `IndexQuery`; pass `QueryOperationOptions(allow_stale=False, retrieve_details=True)` as second arg. Server-side JS runs on the server, not in Python.

## Attachments, counters, time series, revisions

All present. Attachments hang off `session.advanced.attachments`; the others are direct session methods.

```python
# attachments
session.advanced.attachments.store("users/1", "profile.png", stream, "image/png")
result = session.advanced.attachments.get("users/1", "profile.png")   # .details + .data stream
names = session.advanced.attachments.get_names(user)                  # from tracked entity
exists = session.advanced.attachments.exists("users/1", "profile.png")
session.advanced.attachments.delete("users/1", "profile.png")

# counters
session.counters_for("orders/1-A").increment("downloads", 100)
val = session.counters_for("orders/1-A").get("downloads")             # int or None
allc = session.counters_for("orders/1-A").get_all()                   # {name: value}
session.counters_for("orders/1-A").delete("downloads")

# time series
tsf = session.time_series_for("users/1", "Heartrate")
tsf.append_single(timestamp, 72.0, "watches/fitbit")                  # one value
tsf.append(timestamp, [72.0, 0.5], "watches/fitbit")                  # multi-value entry
entries = tsf.get(from_date, to_date)                                 # None,None = all
tsf.delete(from_date, to_date)                                        # also delete_at / delete_all

# revisions (requires revisions enabled for the collection)
revs = session.advanced.revisions.get_for("users/1", User)            # newest first
meta = session.advanced.revisions.get_metadata_for("users/1")
```

- `counters_for_entity(entity)` / `time_series_for_entity(entity, name)` take a tracked entity instead of an id. All of the above are staged in the session and flushed by `save_changes()` (attachment `store` streams the bytes on save).
- Revisions must be enabled first via `ConfigureRevisionsOperation` (import from `ravendb`) sent to `store.maintenance`:

```python
from datetime import timedelta
from ravendb import ConfigureRevisionsOperation
from ravendb.documents.operations.revisions import RevisionsConfiguration, RevisionsCollectionConfiguration

configuration = RevisionsConfiguration(collections={
    "Orders": RevisionsCollectionConfiguration(
        minimum_revisions_to_keep=2, minimum_revisions_age_to_keep=timedelta(days=45))
})
store.maintenance.send(ConfigureRevisionsOperation(configuration))
```

- `RevisionsConfiguration` / `RevisionsCollectionConfiguration` import from `ravendb.documents.operations.revisions`, **not** the package root.
- Revision retention is license-capped: a configuration above `MaxNumberOfRevisionsToKeep` / `MaxNumberOfRevisionAgeToKeepInDays` is refused with HTTP 402 — read both from `GET /license/status` before choosing numbers. There is no `LicenseLimitException` symbol in this package: what you catch is `RavenException` (from `ravendb.exceptions.raven_exceptions`) whose message carries the server's `LicenseLimitException: … exceeds the licensed one '2'`, so match on the message or, better, check the cap up front.
- `default_config` (a database-wide configuration rather than per-collection) is refused outright on an unlicensed server whatever the numbers — `CanSetupDefaultRevisionsConfiguration` is `false` there. Configure per collection instead.

## Cluster-wide transactions & concurrency

```python
from ravendb import SessionOptions, TransactionMode, PutCompareExchangeValueOperation

opts = SessionOptions(transaction_mode=TransactionMode.CLUSTER_WIDE)
with store.open_session(session_options=opts) as session:
    session.advanced.cluster_transaction.create_compare_exchange_value("usernames/john", "users/1")
    session.store(User(name="John"))
    session.save_changes()          # atomic across the cluster (Raft)

with store.open_session(session_options=opts) as session:
    cev = session.advanced.cluster_transaction.get_compare_exchange_value("usernames/john", str)
    session.advanced.cluster_transaction.delete_compare_exchange_value(cev)   # or (key, index)
```

- Compare-exchange = cluster-wide atomic key/value, ideal for uniqueness constraints. Session form requires `CLUSTER_WIDE`; operations form works anywhere: `store.operations.send(PutCompareExchangeValueOperation(key, value, index))` (index `0` = create), plus `GetCompareExchangeValueOperation`, `DeleteCompareExchangeValueOperation`.
- Optimistic concurrency: `session.advanced.use_optimistic_concurrency = True` makes every `save_changes()` in that session fail on a change-vector mismatch (`ConcurrencyException`, imported from `ravendb.exceptions.raven_exceptions` — not re-exported at package root). Off by default, and available on every supported client. `store.conventions.use_optimistic_concurrency` sets the global default, but only **before** `store.initialize()` — conventions freeze there, and a store handed to you already initialized (the test driver's, for one) raises `RuntimeError: Conventions has been frozen`, so use the per-session flag instead. On conventions the boolean and the enum are mutually exclusive: setting both raises. **(client 7.2.2+)** adds `session.advanced.optimistic_concurrency_mode = OptimisticConcurrencyMode.WRITES` (`from ravendb.documents.session.misc import OptimisticConcurrencyMode`; also `WRITES_AND_READS`, default `NONE`) — the boolean stays as a derived view of it, not deprecated.
- Cluster-wide sessions cannot use optimistic-concurrency change vectors on documents (they use the Raft log instead).

## Advanced queries — call form only

RQL semantics and full examples live in the shared RQL packs; here is only how the fluent builder reaches them. All four exist on the query object.

```python
# facets/aggregation → ../rql-facets.md
agg = session.query_index("Products/ByCategory", Product).aggregate_by(
    lambda b: b.by_field("category").sum_on("price")).execute()       # {field: FacetResult}
# every FacetValue field ends in an underscore: count_ sum_ min_ max_ average_ range_
for v in agg["category"].values: print(v.range_, v.count_, v.sum_)

# suggestions → ../rql-advanced.md
sug = session.query_index("Users/ByName", User).suggest_using(
    lambda b: b.by_field("name", "johnn")).execute()

# more-like-this → ../rql-advanced.md
similar = list(session.query_index("Articles/Search", Article).more_like_this(
    lambda b: b.using_document('{"body":"..."}').with_options(MoreLikeThisOptions())))

# spatial → ../rql-spatial.md
near = list(session.query_index("Events/BySpatial", Event).spatial(
    "location", lambda c: c.within_radius(10, 32.07, 34.77)))
```

- Every field on a returned `FacetValue` ends in an underscore: `count_`, `sum_`, `min_`, `max_`, `average_`, `range_`. The bare names do not exist, so `value.average` raises `AttributeError: 'FacetValue' object has no attribute 'average'`.
- `aggregate_by` / `suggest_using` take either a builder lambda or a prebuilt `FacetBase`/`SuggestionBase`; both return a specialized query whose `.execute()` yields the result dict. `spatial(field, clause)` and `within_radius_of(...)` build geo filters via `SpatialCriteriaFactory` (`within_radius`, `relates_to_shape`, `intersects`, `contains`, `disjoint`).
- Field names are literal stored JSON property names — no case conversion.

## Absent / gotchas

- No `UpdateDatabaseOperation`, no distinct `DeleteDatabasesOperation` name (use `DeleteDatabaseOperation.Parameters`), no smuggler (`DatabaseSmuggler`) surface — listed as TODO, not implemented.
- Subscription batch loop runs on a background thread; the `Future` from `worker.run` only completes on error/close — keep the worker alive.
- Long-running server ops (`send_async`) need explicit `.wait_for_completion()`; nothing waits for you.
