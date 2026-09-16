# Testing code that talks to RavenDB

Test against a **real server**. RavenDB's semantics that break code — id generation, change vectors, index staleness, cluster transactions, RQL — are exactly the parts a mock cannot reproduce, so a mocked `IDocumentStore`/`session` proves nothing. Write state-based tests: seed documents, run the code, assert on what the database holds.

## Pick a route, then read your language's file

| Route | What it is | Available in |
|---|---|---|
| **1 — test driver** | Boots an embedded server on first use and hands each test its own fresh database; teardown is automatic. The default choice where it exists. Needs a matching .NET runtime on the machine. | .NET, Python |
| **2 — one server, database per test** | A server you run once for the suite (`install.md`), each test creating and hard-deleting its own database. No runtime dependency, and the only route for Node.js. | any language |
| **3 — embedded server, driven directly** | The same embedded server as Route 1, but you own its lifetime, data directory, and settings. | .NET, Python |

The call forms, the driver's own gotchas, and the runtime requirement in each language: `dotnet/testing.md`, `python/testing.md`, `nodejs/testing.md`. Read one.

Whichever route: give every test its own database — the drivers name and delete one per test for you, and a hand-rolled Route 2 names it (`test-<suite>-<n>`) — a shared database makes tests order-dependent, which is the classic RavenDB test flake — hard-delete on teardown, and delete leftovers at suite start, because a crashed run leaves databases behind.

## Staleness is the #1 test flake

A query issued right after the session saves may legitimately see pre-write results: indexes update asynchronously and the response is flagged stale rather than delayed. Every write-then-query test must wait — the drivers expose a wait-for-indexing helper, otherwise the query's wait-for-non-stale-results call (per-language spelling in each `client.md`). A test that passes locally and fails in CI under load is almost always a missing wait, not a real bug.

Loading a document by id needs no wait — document reads are immediately consistent. Only queries go through indexes.

A test that must *prove* the stale window rather than avoid it cannot rely on losing the race: on a local server the index often catches up before the query. Pause indexing for the database, write, query (now deterministically stale), then resume and wait — `StopIndexingOperation` / `StartIndexingOperation`, sent as database-level maintenance operations, exist under that name in all three clients.

## What to test, and what not to

| Worth a test | Skip it |
|---|---|
| Static index output: seed the edge cases, assert the aggregate/projection | That the client can round-trip a document — that's the client's own test suite |
| RQL that filters or projects non-trivially, including the paging boundary | Getter/setter mapping |
| Cluster-transaction / compare-exchange uniqueness under a real conflict | Mocked session interactions (they assert your mock, not RavenDB) |
| Subscription handlers — feed a batch, assert the effect | |
| Optimistic-concurrency paths: two sessions, same document, expect a concurrency exception | |

CI: run the server as a service container, set `RAVENDB_URL` from the environment, and keep it unsecured on localhost — certificates in a test harness buy nothing unless the certificate handling is what you are testing.
