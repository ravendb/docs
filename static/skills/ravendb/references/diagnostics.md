# Diagnosing a RavenDB problem

Symptom → the one thing to look at. Every endpoint here is a plain `GET` on the server URL unless marked; `{db}` is the database name. Studio exposes all of it visually, but the endpoints work headlessly and in CI.

## "The query returns nothing / the wrong rows"

| Question | Check |
|---|---|
| Which index served it, and why that one? | `GET /databases/{db}/queries?query=<rql>&debug=explain` — returns the candidate indexes with the reason each was picked or rejected. **Dynamic queries only**: on `from index 'X'` it throws `InvalidOperationException: Explain can only work on dynamic indexes`, and the index is already named anyway |
| What did the index actually store for these documents? | `GET /databases/{db}/queries?query=<rql>&debug=entries` — the raw index entries, not the documents. **Drop the `where` clause first**: entries are filtered by the same predicate, so the query that returns nothing also returns no entries. Fields you expected but do not see in the unfiltered entries were never indexed |
| Which terms exist for a field? | `GET /databases/{db}/indexes/terms?name=<index>&field=<field>` — the answer to most "why doesn't `where` match" questions on an analyzed field |
| Where did the time go? | Client-side `.Timings(out var timings)` (or `include timings()` in RQL) breaks the query into optimizer/retriever/query stages |
| Is the result stale? | Every query response carries `IsStale` and `IndexTimestamp`; `GET /databases/{db}/indexes/staleness?name=<index>` asks directly |

Field names are the literal stored JSON property names, and an analyzed (`Search`) field matches analyzed terms, not the exact string — those two account for most empty result sets. `debug=entries` distinguishes them in one call.

The same field can be analyzed in one index and exact in another, so `where Field = 'Some Value'` legitimately returns nothing on a static index while the identical predicate works as a dynamic query against the collection (the auto-index indexes it exact unless full-text was asked for). That is not a contradiction and not a bug: use `search(Field, '…')` against the analyzed one. `indexes/terms` on each index shows which is which.

## "The index is broken / never finishes"

| Check | Endpoint |
|---|---|
| Errors per index (map/reduce exceptions, with document ids) | `GET /databases/{db}/indexes/errors` |
| State, priority, indexed/errored counts, last indexing time | `GET /databases/{db}/indexes/stats` |
| How far behind, and on which collection | `GET /databases/{db}/indexes/progress` |
| Per-batch timings — which stage is slow | `GET /databases/{db}/indexes/performance` (`/performance/live` streams) |
| The definition the server actually has | `GET /databases/{db}/indexes?name=<index>`, or `/indexes/source` for the compiled source |
| Did my deploy change anything? | `POST /databases/{db}/indexes/has-changed` with the definition — a no-op deploy is not the problem |

`ErrorsCount` and `State` are separate questions: a map that throws on a few documents leaves the index `Normal` and still indexing everything else. It flips to `Error` only when the map failure rate passes **15%** over at least 6 attempts, or when every attempt so far has failed — so a handful of poison documents in a large collection shows up as a rising `ErrorsCount`, never as a state change. Check both. An index in state `Error` stops indexing entirely; fix the map and redeploy, which rebuilds side-by-side. A single poison document usually shows up as a repeated error with the same id — patch or delete it. `GET /databases/{db}/indexes/suggest-index-merge` proposes consolidating near-duplicate indexes, which is the fix for "too many auto indexes".

## "It's slow" / "it stopped responding"

| Check | Endpoint |
|---|---|
| Queries running right now (with their RQL and duration) | `GET /databases/{db}/debug/queries/running` — kill one with `POST /databases/{db}/debug/queries/kill?id=<id>` using the id from that listing |
| Database counts, index count, size on disk | `GET /databases/{db}/stats` (`/stats/essential` is cheaper, `/stats/detailed` adds identity and compare-exchange counts). Per-collection document counts and sizes are a different endpoint: `GET /databases/{db}/collections/stats/detailed` |
| Disk I/O behavior over time | `GET /databases/{db}/debug/io-metrics` |
| Storage layout and space per tree | `GET /databases/{db}/debug/storage/report` |
| Oversized documents driving the cost | `GET /databases/{db}/debug/documents/huge` — documents seen over `PerformanceHints.Documents.HugeDocumentSizeInMb` (default 5 MB). It lists only what has been *read* since the server started, so it is empty on a fresh process. The threshold is the **stored** size, which for repetitive text is far below the JSON you sent — check one document with `GET /databases/{db}/docs/size?id=<id>` (`ActualSize`) before concluding the endpoint is broken |
| Everything at once, for a support ticket | `GET /databases/{db}/debug/info-package` — a zip of the whole diagnostic set; attach this rather than pasting fragments |

Cluster-level health lives at `GET /cluster/topology` and `GET /admin/cluster/log`; a node that is `Passive` or a cluster with no leader fails writes while reads keep working.

## Exceptions and what they actually mean

| Exception | Meaning | Response |
|---|---|---|
| `ConcurrencyException` | change-vector mismatch under optimistic concurrency, or a duplicate id on `store` | reload and retry, or drop optimistic concurrency for that path |
| `ClusterTransactionConcurrencyException` | compare-exchange index moved under a cluster-wide transaction | retry the whole transaction; it is the expected outcome of a lost race |
| `DocumentDoesNotExistException` | a patch/attachment op targeted a missing document (`Load` returns `null` instead) | check the id shape — `clients/1-A`, not `clients/1` |
| `IndexDoesNotExistException` | query names an index that was never deployed | deploy indexes at startup, not on demand |
| `InvalidQueryException` | the RQL is rejected by the server | read the message; it names the clause |
| `DatabaseDoesNotExistException` | the store's `Database` does not exist on that server | create it explicitly, don't assume the server auto-creates |
| `AllTopologyNodesDownException` | no node reachable — wrong URL/port, server down, or TLS refused | probe the URL as in `install.md`; with a certificate check the scheme is `https` |
| `AuthorizationException` / 403 | the certificate is valid but not authorized for that database | check the certificate's database permissions |
| `LicenseLimitException` | the feature or resource is beyond the license | `GET /license/status` and see `install.md` |
| `IndexCompilationException` | the index definition does not compile server-side | the message carries the compiler error |
| `NotSupportedInCoraxException` | the index uses a Lucene-only capability | rewrite it, or pin that index to Lucene |
| `RavenTimeoutException` | server did not answer within the configured timeout | usually a slow query or a stale-wait, not a network fault |
| `BulkInsertAbortedException` | the server ended the bulk-insert stream | look at the inner exception; the real error is there |

Per-language shapes: .NET throws these as classes under `Raven.Client.Exceptions` (all deriving from `RavenException`). Python raises them from `ravendb.exceptions.exceptions`, and the missing-document one is spelled `DocumentDoesNotExistsException`. **Node.js has no exception classes** — every failure is a plain `Error` whose `name` is set to the string above, so branch on `err.name === "ConcurrencyException"`; `instanceof` cannot work.

## Server-side logs

Logs go to the configured `Logs.Path` (Docker image: `/var/log/ravendb/logs`; `docker logs <container>` for startup failures). Raise verbosity without a restart:

```bash
curl -s -X POST http://localhost:8080/admin/logs/configuration \
  -H 'Content-Type: application/json' -d '{"Logs":{"MinLevel":"Debug"},"Persist":false}'
```

Lower it again afterwards — debug logging is expensive. `Persist: true` survives a restart. `GET /admin/logs/configuration` reads the current setting, `GET /admin/logs/watch` is a websocket tail, `GET /admin/logs/download` a zip. Startup problems (port in use, bad certificate, unaccepted EULA) only ever appear in stdout/`docker logs`, never in the HTTP API, because the server never got far enough to serve it.
