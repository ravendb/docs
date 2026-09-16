# Structuring a RavenDB codebase

How the pieces wire together, regardless of language. Syntax for each call lives in your language's `client.md` / `indexes.md` — this file is only the shape.

## One store, created once

`DocumentStore` is the single entry point and is thread-safe. Construct and `initialize()` it **once per process**, hold it for the app's lifetime, and open every session from it. Never `new` a store per request/query — that leaks the request executor and connection pool.

- C#: register the initialized store as a DI singleton and the session as scoped, so each request gets one unit of work — the wiring is in `dotnet/client.md`.
- Node/Python: a module-level instance created at boot, imported where needed.
- Conventions (id conventions, serializers, and Node's `findCollectionNameForObjectLiteral`) are frozen at initialization — set them before.

## Startup does the provisioning, in order

One startup path takes a bare server to a ready app, idempotently:

1. Create + `initialize()` the store.
2. Ensure the database exists (`CreateDatabase`, swallow "already exists").
3. **Deploy every static index** — the client's bulk helper (`IndexCreation.CreateIndexes` / `createIndexes` / `create_indexes`) or each task's own execute call; exact form in your language's `indexes.md`. Idempotent: an unchanged definition is a no-op, a changed one rebuilds side-by-side. This is the only correct place to register indexes; do not deploy them lazily from a query path.
4. Enable any server-side features the app relies on (Refresh, ETL, expiration, AI connection strings/agents).
5. Seed idempotently — check for existing data and skip. Bulk insert assigns a fresh auto-id per call and never upserts, so a second run silently doubles the collection rather than overwriting it. Use explicit ids when a re-run must be a no-op.
6. Wait once for indexes to be non-stale (poll `GetStatisticsOperation`) **after** deploy and seed — never before. A wait placed ahead of the deploy returns immediately (the index doesn't exist yet) and the first queries hit a stale index.

A static index only exists for the queries that name it. Query a collection instead (`query(collection)`, raw `from Collection where …`) and the server quietly builds a *separate* auto-index while the static one sits unused; name an index that was never deployed and the query falls back to an auto-index or errors. Deploy it at startup **and** query it by name, or don't deploy it.

## Indexes are defined code, in one place

Every static index is a class (`AbstractIndexCreationTask` / `AbstractJavaScriptIndexCreationTask` / C#-syntax task), kept in a dedicated indexes module (`indexes.ts`, `app/indexes.py`, index classes in the assembly) — not inline string literals scattered across handlers. The class name *is* the index name (`_` → `/`). One definition, deployed once, queried by name everywhere.

## Session per unit of work

Open a session at the start of a unit of work (typically one request/operation), do the loads/queries/changes, save once, dispose. Sessions are cheap and short-lived — never share one across requests or hold one open for the process.

- Scope it (`using` / `with` / try-finally) so it always disposes.
- Reads and writes for one logical operation share the session so `include()` and the identity map avoid N+1 and repeat loads.
- The 30-request-per-session cap is a smell detector: hitting it means the unit of work is too big, not that the cap should be raised.

## Where query code lives

Queries run inside the session, in the data-access/handler layer — targeting a **deployed static index** for anything beyond a trivial filter, a dynamic `query(collection)` only for simple ad-hoc equality/range. Keep RQL/query-builder code out of view/render code; a route/handler opens a session, queries, projects to the shape the caller needs, and returns it.

## Smells

- A `DocumentStore` constructed anywhere but startup, or more than one.
- Index definitions as inline strings in a request handler, or created on first query instead of at startup.
- `query(indexName: "X")` with no `X` class deployed anywhere — or the inverse, a deployed index no query ever names.
- A session opened at module scope, reused across requests, or never disposed.
- Query/RQL strings embedded in rendering/UI code.
- A document collection whose fields disagree on casing/shape between the seeder and the write path (one writes `Qty`, the other `Quantity`) — the API must own one canonical wire shape and normalize at the boundary, not leak raw docs whose field names drift by origin.
