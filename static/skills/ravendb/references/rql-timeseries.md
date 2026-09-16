# RQL — time series

Query time series attached to documents, inline or via a named declaration. Version-correct details: ravendb.net/docs.

## Two entry forms
Inline projection:
```
from Employees as e
where Birthday < '1994-01-01'
select timeseries( from HeartRates )
```
Named declaration (reusable, with range/aggregation):
```
declare timeseries ts(jogger) {
  from jogger.HeartRates
  between '2020-05-27T00:00:00Z' and '2020-06-23T00:00:00Z'
  group by '1 hour'
  select min(), max(), avg(), first(), last()
}
from Users as jogger
where Age > 30
select ts(jogger)
```

## Inner time-series query language
| Part | Purpose |
|---|---|
| `from <SeriesName>` | the series source (`from doc.Series` inside a declared block) |
| `between <ts> and <ts>` | ISO-8601 range; or params `$from` / `$to` |
| `first <n> <unit>` / `last <n> <unit>` | earliest / most-recent window (e.g. `last 30 minutes`); mutually exclusive with `between` |
| `where Values[n] <op> x` | filter by the n-th value in each entry |
| `where Tag == '…'` | filter by entry tag |
| `group by '<n> <unit>'` | time buckets; secondary group by tag: `group by '1 hour', tag` |
| `select <aggregations>` | per-bucket aggregation |
| `scale <double>` | multiply every value |
| `offset <timespan>` | shift timestamps to a timezone (client-exposed on some versions) |

Bucket units: `ms`/`milliseconds` · `second` · `minute` · `hour` · `day` · `month` · `quarter` · `year` (e.g. `'7 days'`, `'1 hour'`).

## Aggregations
`min() max() sum() avg() first() last() count() percentile(<n>) stddev() slope()`

**The result field names are not the function names.** Each bucket comes back as `{From, To, Key, Count, Min, Max, Average, First, Last}` — `avg()` reads back as **`Average`**, and every aggregate is an **array**, one element per value column, so a single-value series is `Average[0]`. Indexing `Avg` or `avg` gives `undefined`/`None`, which looks like a broken query rather than a wrong field name.

## Filtering documents by series or counter values

`where` on the document cannot see time-series or counter values, and `include timeseries(...)` / `include counters(...)` only side-load them onto results already selected. To *filter or sort documents* by those values, index them: a time-series index (`AbstractTimeSeriesIndexCreationTask` and its JS/multi-map variants) maps over series entries, and `AbstractCountersIndexCreationTask` over counter values; both exist in the .NET, Node, and Python clients. Query that index like any static index.

## Worked examples
Filter by a value column, weekly max/min:
```
from Companies as c
select timeseries(
  from StockPrices
  where Values[4] > 500000
  group by '7 days'
  select max(), min()
)
```
Most-recent window with the `last` clause (a range clause right after `from` — NOT a function, NOT in `where`):
```
declare timeseries recent(u) {
  from u.HeartRates
  last 30 minutes                   # or: first 7 days
  select avg(), max()
}
from Users as u select recent(u)
```

## Fetch one document's raw series
When you want a single document's series (not a query projection), prefer `get_document_data` with `include=TimeSeries` + `timeSeriesName` (and optional `from`/`to`).
