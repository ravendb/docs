# RQL — complete function/method reference

Authoritative list of RQL methods the server's parser recognizes. Names are case-insensitive. For prose and version deltas fetch ravendb.net/docs; for focused syntax see the topic resources linked below.

## Operators
| Kind | Operators |
|---|---|
| Comparison | `=` `==` · `!=` `<>` · `<` `<=` `>` `>=` |
| Logical | `and` · `or` · `not` (cannot start a `where`) · grouping `( )` |
| Range | `field between x and y` (inclusive) |
| Set | `field in (a,b,…)` (any) · `field all in (a,b,…)` (array field contains every listed value) |
| Array/path | `Address.City` (nested) · `Lines[].Sku` (array element) |

## Filtering / where methods
| Method | Purpose | Example |
|---|---|---|
| `id()` | The document id field. `where` accepts an alias argument; `select` never does and always returns the source document's id, so `select id(d)` and `select id(a)` both fail with `id(doc) must be called with an object argument`. A loaded document's id is not projectable: use the field you loaded through | `where id() = 'users/1-A'` · `where id(d) = 'users/1-A'` · `select id() as Id` |
| `startsWith(f,'x')` | Prefix match (index-efficient) | `where startsWith(Name,'Jo')` |
| `endsWith(f,'x')` | Suffix match (full scan) | `where endsWith(Email,'@acme.com')` |
| `exists(f)` | Field is present | `where exists(MiddleName)` |
| `regex(f,'pat')` | Regex match | `where regex(Sku,'^A-\\d+$')` |
| `search(f,'terms'[,and])` | Full-text (see `rql-fulltext.md`) | `where search(Bio,'raven db')` |
| `exact(expr)` | Case-sensitive, non-analyzed | `where exact(Name == 'Bob')` |
| `boost(expr,n)` | Weight for relevance | `where boost(Name='Bob',10)` |
| `lucene(f,'q')` | Raw Lucene (Lucene engine) | `where lucene(Name,'Jo*')` |
| `fuzzy(expr,f)` | Fuzzy, factor 0..1 | `where fuzzy(Name='Bob',0.7)` |
| `proximity(search,n)` | Terms within n words | `where proximity(search(Bio,'a b'),3)` |
| `intersect(a,b,…)` | Subquery intersection (meaningful on fanout static indexes; on a plain dynamic query it's just `and`) | `where intersect(A=1,B=2)` |
| `cmpxchg('key')` | Compare-exchange value ref | `where Ref = cmpxchg('email/a@b.com')` |

## Aggregation
Dynamic `group by` supports **only** `count()` and `sum()` — `avg`/`min`/`max` error in a dynamic group-by select.
| Method | Where usable |
|---|---|
| `count()` | dynamic `group by`, facets, time series |
| `sum(f)` | dynamic `group by`, facets, time series |
| `avg(f)`, `min(f)`, `max(f)` | facets, time series, map-reduce indexes, JS projections — NOT dynamic group-by |
| `key()` | the group key in a `group by` select |
| `array(f)` | group on whole array: `group by array(Tags)` |

For avg/min/max over a collection use a facet (`rql-facets.md`), a static map-reduce index, or a JS `select` projection.

## Ordering functions (see `rql-cheatsheet.md`)
`order by score()` (relevance) · `random()` / `random(seed)` · `spatial.distance(...)` · `custom(f,'Sorter')` (Lucene only). Casts: `as string | long | double | alphaNumeric`.

## Projection / include extensions
| Method | Purpose |
|---|---|
| `highlight(f,frag,count[,opts])` | Highlighted fragments (see `rql-advanced.md`) |
| `explanations()` | Per-result scoring explanation |
| `timings()` | Server-side timing breakdown |
| `counters(name?)` | Include counter values |
| `revisions(...)` | Include document revisions |
| `timeseries(...)` | Include/project time series (see `rql-timeseries.md`) |

## Spatial (see `rql-spatial.md`)
`spatial.point(lat,lng)`, `spatial.circle(r,lat,lng[,units])`, `spatial.wkt('…')`,
`spatial.within|contains|intersects|disjoint(field,shape)`, `spatial.distance(field,point[,roundFactor])` (order by; units come from the shape/index config, default kilometers). Circle units: `kilometers` (default) | `miles`.

## Relevance / similarity / vector (see `rql-advanced.md`)
`moreLikeThis(seed,'{opts}')` · `suggest(f,term,'{opts}')` · `vector.search(field|embedding.*, query[,minSim][,candidates])` (7.0+).

## Time series helpers
`first <n> <unit>` / `last <n> <unit>` — earliest/most-recent window; range **clauses** right after `from` inside a `timeseries(...)` block (not functions, mutually exclusive with `between`). Aggregations: `min max sum avg first last count percentile(n) stddev slope`. Units: `ms|second|minute|hour|day|month|quarter|year`.

## Worked examples
```
# filter + project + page
from Orders where Freight > 500 and ShippedAt > '1998-01-01'
order by Freight as double desc
select Company, Freight limit 0, 25

# aggregate pipeline value by stage
from Deals group by Stage select Stage, count() as N, sum(Amount) as Total

# join related doc into projection
from Orders as o load o.Company as c select { Company: c.Name, Shipped: o.ShippedAt }
```

## Version notes
- `vector.search(...)` / `embedding.*` require server 7.0+.
- Engine matters: Lucene vs Corax differ on `lucene()`, order-by chaining (Corax ≤16), and analyzer behavior.
