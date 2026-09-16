# RQL reference — concepts

How querying works across supported versions. Syntax: `rql-cheatsheet.md`; every method: `rql-functions.md`; version-correct details: ravendb.net/docs.

## Query anatomy
Full clause order:
```
declare <fn>  from <source> [as alias]  [group by …]  where …  order by …  [load …]  [filter …]  [select …]  [include …]  [limit …]
```
Every query executes against an **index**. The client query builders (LINQ and `DocumentQuery` in .NET, the query API in Node and Python) compile to RQL; every client also has a raw-query call that sends RQL directly — see your language's `client.md`.

## Dynamic vs index queries
- **Dynamic** — `from Collection where …`: the server auto-creates/reuses an `Auto/…` index. First run of a new field combination may build the index (watch `IsStale`).
- **Index** — `from index 'Name' …`: queries a named static index directly. Use for map/reduce, computed/stored fields, full-text with a custom analyzer, facets, spatial, more-like-this.

## Auto-indexes
- Filtering, sorting, and aggregation on new field combinations create/extend auto-indexes; aggregations produce `Auto/<Coll>/By…ReducedBy…`.
- Auto-indexes merge over time — a feature, not a leak. A one-off exotic query still pays the build cost.

## Staleness
- Indexing is asynchronous. Results carry `IsStale`; `true` means the index hasn't caught up to the latest writes.
- `IndexName` in the result tells you which index (auto or static) served the query.

## Projections
- `select f, …` returns only those fields (smaller payloads); applied **last**, after filter/sort/paging.
- JS object projections `select { … }` and `declare function` run server-side JS: `.map/.filter/.reduce/.length`, string `.substr`, `Date`/`Date.parse`, `getMetadata(doc)`.
- Projected results are **not** session-tracked (read shapes, not entities).
- RQL has no scalar date/math operators in `where`; do scalar transforms in projections. To *filter or sort* on a derived value, compute it in a static index map (`DiscountedPrice = p.Price * 0.9`) and query that index — a projection is evaluated after filtering, so it cannot feed a `where`.

## Includes vs load (avoid N+1)
- `load o.Ref as x` — resolves a referenced id for use inside `select`.
- `include Ref` / `include Lines[].ProductId` — side-loads related documents in one round-trip; a later `Load()` of those ids hits the session cache. Also `include counters()`, `timeseries()`, `revisions()`, cmpxchg. Streaming queries do not support includes.

## Parameters
Prefer `$p` params over string interpolation: `where Name = $name`. Safer and lets the server cache the query plan. Values follow the query as a JSON object.

## Paging
`limit <take>` · `limit <skip>,<take>` · `limit <take> offset <skip>` · `offset <skip>`.

## Common gotchas
- Strings use single quotes; quote names with special chars (`from "Order Lines"`). Comments: `// line`, `/* block */`.
- Numeric/date ordering needs a cast: `order by Amount as double` (lexical otherwise).
- `search()` needs an analyzed field — not `startsWith`/`=`.
- `in` matches any; `all in` requires every array element to match.
- Dynamic `group by` supports only `count()`/`sum()` — avg/min/max need a facet, map-reduce index, or JS projection.
- `not` cannot start a `where`; anchor optional fields with `exists(field)`.
- Engine matters: Corax caps `order by` at 16 clauses; custom sorters are Lucene-only.

## Discovery loop (write good queries)
1. `from <Coll> limit 1` → learn the real field names/shapes; never guess casing.
2. List existing static indexes (Studio, or `GET /databases/<db>/indexes`) — query them directly instead of forcing a new auto-index.
3. Build the real query; page with `limit`/`offset`; pass values as `$params`.
