---
name: ravendb
description: Read this before writing any RavenDB query, client call or /databases/ request. Covers server setup, document modelling, the Node.js, .NET and Python clients, RQL, indexes, vector search and AI agents. Code written from recall tends to run and return the wrong rows, because RQL resembles SQL and the clients take field names as plain strings.
license: MIT
metadata:
  author: ravendb
  version: "1.0.0"
---

# RavenDB

RavenDB is a transactional (ACID) document database: JSON documents in collections, queries in RQL served by indexes that are always maintained, with a client library per language.

**Your recall of RavenDB is stale. Do not write RQL, client code, or endpoint shapes from memory.**

**Before you write any query, do both of these:**

1. **Read `references/rql-cheatsheet.md`.** Every query, every time, including ones that look obvious. RQL resembles SQL closely enough that a query written from memory usually parses and then returns the wrong rows.
2. **If the query names a static index, read that index off the server first:** `GET /databases/<db>/indexes?name=<index>`. `?name=` is a query string, so `/indexes/<index>` is not a route and returns `RouteNotFoundException`. The definition names the fields the index holds and which are analyzed, which decides whether `=` or `search()` matches. An index cannot answer on a field it does not hold.

Then route from the tables below to the one further file your task needs and follow its syntax exactly. Read only what the tables route you to; unrouted files waste context. Use ravendb.net/docs only if no file covers it.

**Find out what the database is before you write to it.** `GET /databases` reports an `Environment` per database:
`Production`, `Testing`, `Development` or `None`. A server-wide default can be set separately, readable at
`GET /configuration/studio` (404 when unset); a database does not inherit it, so check both. `None` is not an answer:
it means nobody declared it, or that the server cannot declare it at all, since setting this is licensed and every
database on a server whose `GET /license/status` reports `HasStudioConfiguration: false` reads `None` whatever it
actually holds. Ask the user which it is, and never infer it from the database name.

**Treat every write as a production write until you have an answer that says otherwise.** Reading is fine. Everything
else needs the user to agree to that specific operation on that specific database. The test is not whether an
operation appears in a list, it is whether the effect outlives your task: does it change state that was there before
you started, or change behaviour for anyone but you? Either one, and you name the operation and the database it lands
on, then stop.

In RavenDB that catches more than it first looks like. Writing to or deleting a document that already exists. `update`
or `delete` by query. Deleting or resetting an index. Hard-deleting a database. Expiration and refresh, which delete
documents on a schedule. A revisions configuration, which rewrites history that is already there. A data subscription,
and any other edit to the database record, which every client of that database sees and which can consume a licensed
slot. Being told to build a feature is not agreement to run it against real data, and a connection string is not
agreement either. To check your own work, write your own document or your own database and use that, never an existing
one.

## Getting a server

**Before you write any connection code, do these three:**

1. **Find the server the user already has.** Never assume `localhost:8080`, and never install RavenDB unasked. If more
   than one server is running, ask which to use.
2. **Confirm it answers and identify it.** `GET /build/version` for the version, `GET /databases` for the databases and each one's `Environment`.
3. **Check the licence.** `GET /license/status`. Warn the user if unlicensed: such a server rejects some operations
   outright and reports `HasStudioConfiguration: false`, which makes every database read back as `Environment: None`.

`references/install.md` has the full discovery protocol and the version and licensing steps.

| You need to… | Read |
|---|---|
| Find an existing server, or install one after asking: Docker, health check, create a database, server/client version check, license check, free Developer license | `references/install.md` |
| Obtain and register a license: the free-key request form, activation, verification. Only **after** `install.md`'s license check has run and the user has agreed | `references/license.md` |
| Install without Docker: download, extract, run as a service | `references/install-native.md` |
| **Anything on Windows**: PowerShell probes, Docker Desktop, `rvn windows-service`, WSL2 port rules, firewall | `references/windows.md` |
| Connect to a secured or Cloud server (`https://`, client certificate) | the "Secured server" section of your language's `client.md` |

## Client code

**Pick one language directory, then open exactly one file from it. Do not read the others.**

| Target language | Directory |
|---|---|
| JavaScript / Node.js (`ravendb` npm) | `references/nodejs/` |
| C# / .NET (`RavenDB.Client` NuGet) | `references/dotnet/` |
| Python (`ravendb` pip) | `references/python/` |

Every directory holds the same five files:

| File | When your change involves… |
|---|---|
| `client.md` | store setup, sessions, CRUD, basic querying, includes, bulk insert |
| `indexes.md` | static indexes, map-reduce, `LoadDocument`, full-text, vector search, waiting out staleness |
| `operations.md` | creating/deleting a database in code, subscriptions, patching, attachments/counters/time-series/revisions, optimistic concurrency and cluster-wide transactions, facet/suggest/spatial query calls |
| `testing.md` | test driver or database-per-test suite in that language, and the runtime it needs. Read `references/testing.md` first to pick the route |
| `ai-agents.md` | the client's AI-agent wrapper |

Writing a query is different: it always starts at the cheatsheet below, whichever language you are in.

## Writing a query

**Always start at `references/rql-cheatsheet.md`:** clause order, `where`/`select`/`group by`/paging, and the SQL
habits that silently return wrong rows. Open one of these after it, only if your query needs it:

| Your query needs… | Read |
|---|---|
| How querying works: dynamic vs named index, auto-indexes, staleness, parameters | `references/rql-reference.md` |
| The exact signature of one method (`id` `exists` `regex` `startsWith` …) | `references/rql-functions.md` |
| Full-text: `search` `boost` `fuzzy` `proximity` | `references/rql-fulltext.md` |
| Facets, `avg`/`min`/`max` over a collection | `references/rql-facets.md` |
| `suggest` `morelikethis` `highlight` `vector.search` (7.0+) | `references/rql-advanced.md` |
| Spatial: `spatial.within` `circle` `wkt` distance | `references/rql-spatial.md` |
| Time series query syntax | `references/rql-timeseries.md` |

**Aggregation routes three ways, so pick one before reading.** Ad-hoc aggregate: `group by`, in `rql-cheatsheet.md`.
Faceted breakdown of a filtered result set: `rql-facets.md`. Precomputed or large: map-reduce, in your language's
`indexes.md`. `group by` is rejected on a named index; use a collection, or an index that is already map-reduce.

## Everything else

| You want… | Read |
|---|---|
| Wiring a codebase: store singleton, deploy indexes at startup, session per unit of work, where query code lives | `references/structure.md` |
| Document shape: collections, ids, embed vs reference, `@metadata`, change vector | `references/modeling.md` |
| AI agents (7.1+, license): concepts and the conversation HTTP surface | `references/ai-agents.md`, then your language's `ai-agents.md` |
| Raw HTTP: `GET /docs`, `POST /queries`, `POST /bulk_docs`, index definitions, create database | `references/http-api.md` |
| Can this deployment run a marked feature: server version, client package version, license | `references/version-gate.md` |
| Testing: which route, staleness flakes, what to test, CI. Read before your language's `testing.md` | `references/testing.md` |
| Something is wrong: empty or wrong results, broken or lagging index, slow server, an unfamiliar exception, logs | `references/diagnostics.md` |
| A working app to copy: official sample apps per feature, minimal starters, client test suites, playground servers | `references/samples.md` |

Files inside `references/` cross-link to each other with relative paths. Follow those links; they are part of the routing.

Not covered here, so go to ravendb.net/docs or ask the user: sharding, backup/restore, ETL and external replication, production monitoring and SNMP, encryption at rest, cluster topology changes.

## Version & license gate

Baseline server is **6.2+**; older versions are out of support, so ignore them. Features above the baseline carry an
inline mark where they are documented: `(7.0+)` or `(7.1+)` is a **server** requirement, `(client 7.2.2+)` names a
**client package** version, and `(license)` means the license decides. Before writing code against a marked feature,
run the matching check in `references/version-gate.md`.
