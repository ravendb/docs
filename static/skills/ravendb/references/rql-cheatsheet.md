# RQL cheat-sheet

Dense syntax reference. Method surface: `rql-functions.md`. Concepts: `rql-reference.md`. Version-correct details: ravendb.net/docs.

## Clause order
```
declare <fn>
from <source> [as alias]
[ group by <fields> ]
[ where <predicate> ]
[ order by <field> [as <type>] [asc|desc] ]
[ load <ref> as x ]
[ filter <predicate> ]
[ select <projection> ]
[ include <related> ]
[ limit <skip>, <take> | limit <take> offset <skip> ]
[ filter_limit <n> ]
```
Each clause appears **at most once**. There is no second `where`: `where` runs against the index, before `load` resolves anything, so a condition on a loaded document goes in `filter`, not in another `where`.

Comments: `// line`, `/* block */`. Strings: `'…'` or `"…"`. Literals: `true false null`, numbers. Params: `$name`. Quote names with special chars: `from "Order Lines"`.

## from
```
from Users                     # collection
from "Order Lines"             # quoted name
from @all_docs                 # every document
from index 'Orders/Totals'     # named static index
from Users as u                # alias (needed for load / JS projections)
```

## where
```
where Age > 30 and IsActive = true          # = == , != <>, < <= > >=
where Address.City = 'Albuquerque'          # nested path via dot
where Age between 18 and 65
where Country in ('US','UK')                 # any listed value
where Lines[].Sku all in ('A','B')           # array field: all listed present
where startsWith(Name,'Jo')  where endsWith(Email,'@acme.com')
where exists(MiddleName)                      # anchor optional fields
where regex(Sku,'^A-\d+$')
where search(Bio,'raven database')            # full-text; see rql-fulltext.md
where id() = 'users/1-A'
where boost(Name = 'Bob', 10)
```
`not` cannot START a where clause — follow an expression: `exists(X) and not X in (…)`. Full method list: `rql-functions.md`.

`id()` is asymmetric across clauses. In `where` it takes an alias or nothing, both fine. In `select` it is **always argument-less** and always returns the id of the *source* document: `select id(d)` and `select id(a)` both fail with `id(doc) must be called with an object argument`, whether the alias came from `from … as d` or from `load … as a`.

A **loaded** document's id cannot be projected at all. Use the field you loaded through, which already holds it: after `load AccountId as a`, the id is `AccountId`, not `id(a)`. Do not reach for `a.Id`: it parses and returns 200 with the field silently missing from the results.

## order by
```
order by Age                     # default asc
order by Amount as double desc   # types: string | long | double | alphaNumeric
order by UnitsInStock as long desc, score(), Name   # chained (Corax: max 16)
order by score()  |  random()  |  random(1234)       # relevance / random / seeded
order by spatial.distance(...)                        # see rql-spatial.md
order by custom(Field,'MySorter')                     # Lucene engine only
```

## select (projection) — applied last, results not session-tracked
```
select Name, Address.City as City         # fields + aliases
select id() as Id                         # ALWAYS argument-less; the SOURCE doc's id
select AccountId                          # a loaded doc's id: the field you loaded through
select distinct Country
select { Full: x.FirstName + ' ' + x.LastName,
         Total: x.Lines.map(l => l.Price*l.Qty).reduce((a,b)=>a+b,0) }   # JS object
select { Meta: getMetadata(x) }            # metadata access
```
- `include` after `select` is resolved on the projected result: `select DealName, AccountId include AccountId` includes the accounts, `select DealName include AccountId` silently includes nothing. Project the field the include walks, or do not project.

## group by / aggregation

**`group by` goes before `where`, the reverse of SQL.** Write `from Deals group by Stage where Stage = 'Closed Won'`. The SQL order is a parse error that does not name the fix: `Expected end of query but got: group`. That `where` may only reference the group key; any other condition, including one on an aggregate (`where count() = 3`), fails with `Field '...' is neither an aggregation operation nor part of the group by key` (the Python client shows only the status 500). To count distinct values under a filter on another field, `from Deals where Stage = 'Closed Won' select distinct AccountId` and count the rows; to filter groups by an aggregate, fetch all groups (`select key(), count()`) and filter client-side, or use a map-reduce index.

```
from Orders
group by ShipTo.City
select ShipTo.City as City, count() as N, sum(Amount) as Total
```
Dynamic `group by` supports **only `count()` and `sum()`** (avg/min/max error — use a facet, map-reduce index, or JS projection). `group by array(Tags)` keys on whole array; `key()` returns the group key. Sort aggregates: `order by count() as long`. Aggregation auto-creates an `Auto/…` index.

## include / load (avoid N+1)
```
load o.CustomerId as c select o.Id, c.Name
include CustomerId, Lines[].ProductId       # related docs, one round-trip
include counters(o,'Views')
include timeseries('HeartRate', $from, $to)
include revisions('2026-02-23T07:40:54Z')
```

## filter (post-index scan)
`filter <predicate>` runs AFTER the index query, scanning retrieved results server-side, for exact checks the index can't answer. `where` is the primary filter. Cap the scan with `filter_limit <n>`, which goes **last in the query**: `filter … select … filter_limit 2000` parses, `filter … filter_limit 2000 select …` does not.

`TotalResults` is unusable on a filtered query: on an index query it reports the pre-filter index total; on a dynamic query it reports only the filtered rows found in the window scanned so far, so with `limit 5` it is 5 whatever the real total. There is no cheap total under `filter`; count the rows of a full (or `filter_limit`-bounded) result.

## paging
```
limit 25            # take 25
limit 50, 25        # skip 50, take 25
limit 25 offset 50  # take 25, skip 50
```

## Coming from SQL

RQL reads like SQL and then diverges. These have no RQL equivalent; most fail with a parse error that does not name the replacement.

| SQL | RQL |
|---|---|
| `select *` | omit `select` entirely (`select *` is also accepted) |
| `count(*)` | `count()`, and only inside a `group by` |
| `where` before `group by` | reversed: `group by` comes **first**, `from Deals group by Stage where Stage = 'Closed Won'`. The SQL order fails with `Expected end of query but got: group` |
| `sum(x)` on its own | needs `group by`, a map-reduce index, or a facet |
| `like '%x%'` | `startsWith` / `endsWith`, or `search()` on an analyzed field |
| `is null` / `is not null` | `where x = null` / `where exists(x)` |
| `join` | `load` (projection) or `include` (one round-trip, no join) |
| `update t set c = v` | `from t where ... update { ... }`, a write, on a different endpoint |

`group by` works on a collection, never on a named index: `from index 'X' group by …` is rejected outright, because an index that aggregates has to already be map-reduce. Counting has four routes and picking the wrong one fails silently. A plain index or collection query: `limit 0` and read `TotalResults`. A `group by`: `count()`. **A query with `filter`: count the rows in `Results`.** On a filtered query `limit 0` still reports the pre-filter match count and returns no rows to notice it by, so the documented `limit 0` route gives the wrong number without erroring. A facet: the bucket already carries `Count`, so read it. `select count()` is rejected everywhere outside `group by`, **including inside `facet(...)`**, where adding it turns a working aggregate into an error. A `select distinct` query: count the rows; `TotalResults` is the count before deduplication (8 distinct stages, `TotalResults` 1200).

To aggregate over a named index, use a facet. The aggregates are **arguments to `facet(...)`, not a block**:
```
from index 'Deals/ByAccount' where Stage = 'Closed Won' select facet(Stage, sum(Amount), avg(Amount))
```
`select facet() { sum(Amount) }` does not parse, and a bare `select sum(Amount)` is rejected as `sum may only be used in group by queries`. Buckets come back as lowercased index terms. Options and range facets: `rql-facets.md`.

Running that RQL **through a client** needs the aggregation entry point, not the normal result path: `ExecuteAggregation()` in .NET, `executeAggregation()` in Node, `execute_aggregation()` in Python. Calling the ordinary one fails: the server answers `Raw query with aggregation by facet should be called by executeAggregation method`, and in .NET it does not even compile, because `IRawDocumentQuery<T>` has no `ToFacets`.

Case runs the opposite way to most SQL. Field names are case-sensitive, so `type` against an index that stored `Type` is a hard error, while values are not, so `where Stage = 'closed won'` matches `'Closed Won'`.

Two more that return wrong rows rather than failing: quoting a number makes the comparison lexical, so `where Amount > '10000'` matches almost everything; and `=` against a field the index stored as analyzed (`Indexing: Search`) matches single tokens, not the whole string, so `where Name = 'Acme Data Group'` finds nothing while `search(Name, 'Acme Data Group')` ORs the terms and floods. `diagnostics.md` has the `debug=entries` call that tells the two apart.

## gotchas (these cause errors, not just bad results)
- **Clause order is fixed:** `from → group by → where → order by → load → filter → select → include → limit → filter_limit`. `order by` **before** `load` and `select`, never after.
- **`facet(...)` and `morelikethis(...)` need a static index** — `from index 'Name' select facet(...)`, never `from Collection`. `suggest()` and `highlight()` also work on dynamic queries (highlighting not with a dynamic `group by`).
- Numeric/date `order by` needs a cast (`as double`/`as long`), else lexical order.

## Next
Concepts (dynamic vs index, staleness): `rql-reference.md`. Exact signatures for every method: `rql-functions.md`. One file per area — full-text, spatial, time series, facets, suggestions/MLT/vector — routed from `SKILL.md`. Official docs at ravendb.net/docs are the last resort.
