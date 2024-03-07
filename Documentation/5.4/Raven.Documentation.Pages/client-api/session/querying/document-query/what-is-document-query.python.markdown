# What is a Document Query?

---

{NOTE: }

* Queries in RavenDB can be written using `query`, `document_query` or directly with `RQL`.  
  Learn more in [Query Overview](../../../../client-api/session/querying/how-to-query).

* In this page:
  * [`document_query` Examples](../../../../client-api/session/querying/document-query/what-is-document-query#document_query-examples)
  * [Convert between `document_query` and `query`](../../../../client-api/session/querying/document-query/what-is-document-query#convert-between-document_query-and-query)
  * [Available Custom Methods and Extensions](../../../../client-api/session/querying/document-query/what-is-document-query#convert-between-document_query-and-query)

{NOTE/}

---

{PANEL: `document_query` Examples}

#### Query collection - no filtering

{CODE-TABS}
{CODE-TAB:python:document_query documentQuery_1@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.py /})
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Query collection - by ID

{CODE-TABS}
{CODE-TAB:python:document_query documentQuery_2@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.py /})
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" where id() == "employees/1-A"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Query collection - with paging

{CODE-TABS}
{CODE-TAB:python:document_query documentQuery_4@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.py /})
{CODE-TAB-BLOCK:sql:RQL}
from "Products" limit 5, 10 // skip 5, take 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Query an index

Please refer to [Querying an index](../../../../indexes/querying/query-index#session.advanced.documentquery) for examples of querying an index using document_query.

{PANEL/}

{PANEL: Convert between `document_query` and `query`}

A `document_query` can be converted to a `query`.  

{CODE-TABS}
{CODE-TAB:python:document_query documentQuery_5@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.py /})
{CODE-TAB-BLOCK:sql:RQL}
from "Orders" where Freight > 25
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Available Custom Methods and Extensions}

- add_or_der
- [query] [after_query_executed](../../../../client-api/session/querying/how-to-customize-query#afterqueryexecuted)
- [query] [after_stream_executed](../../../../client-api/session/querying/how-to-customize-query#afterstreamexecuted)
- [query] [aggregate_by](../../../../client-api/session/querying/how-to-perform-a-faceted-search)
- [query] [aggregate_using](../../../../client-api/session/querying/how-to-perform-a-faceted-search)
- and_also
- [query] [before_query_executed](../../../../client-api/session/querying/how-to-customize-query#beforequeryexecuted)
- [boost](../../../../client-api/session/querying/text-search/boost-search-results)
- close_subclause
- cmp_xchg
- contains_all
- contains_any
- [count](../../../../client-api/session/querying/how-to-count-query-results)
- [count_lazily](../../../../client-api/session/querying/how-to-perform-queries-lazily#lazy-count-query)
- distinct
- explain_scores
- first
- first_or_default
- fuzzy
- get_index_query
- get_query_result
- [group_by](../../../../client-api/session/querying/how-to-perform-group-by-query)
- [group_by_array_values](../../../../client-api/session/querying/how-to-perform-group-by-query#by-array-values)
- [group_by_array_content](../../../../client-api/session/querying/how-to-perform-group-by-query#by-array-content)
- [query] [highlight](../../../../client-api/session/querying/text-search/highlight-query-results)
- include
- include_explanations
- intersect
- invoke_after_query_executed
- invoke_after_stream_eExecuted
- [query] [lazily](../../../../client-api/session/querying/how-to-perform-queries-lazily)
- [long_count](../../../../client-api/session/querying/how-to-count-query-results)
- more_like_this
- negate_next
- [not](../../../../client-api/session/querying/document-query/how-to-use-not-operator)
- [query] [no_caching](../../../../client-api/session/querying/how-to-customize-query#nocaching)
- [query] [no_tracking](../../../../client-api/session/querying/how-to-customize-query#notracking)
- of_type
- open_subclause
- [order_by](../../../../client-api/session/querying/sort-query-results)
- [order_by_descending](../../../../client-api/session/querying/sort-query-results)
- [query] [order_by_distance](../../../../client-api/session/querying/how-to-make-a-spatial-query#orderByDistance)
- [query] [order_by_distance_descending](../../../../client-api/session/querying/how-to-make-a-spatial-query#orderByDistanceDesc)
- [order_by_score](../../../../client-api/session/querying/sort-query-results#order-by-score)
- [order_by_score_descending](../../../../client-api/session/querying/sort-query-results#order-by-score)
- or_else
- [query] [projection](../../../../client-api/session/querying/how-to-customize-query#projection)
- proximity
- [query] [random_ordering](../../../../client-api/session/querying/how-to-customize-query#randomordering)
- [query] [relates_to_shape](../../../../client-api/session/querying/how-to-make-a-spatial-query#search-by-shape)
- [search](../../../../client-api/session/querying/text-search/full-text-search)
- select_fields
- select_time_series
- single
- single_or_default
- skip
- [query] [spatial](../../../../client-api/session/querying/how-to-make-a-spatial-query)
- statistics
- [suggest_using](../../../../client-api/session/querying/how-to-work-with-suggestions)
- take
- [query] [timings](../../../../client-api/session/querying/how-to-customize-query#timings)
- using_default_operator
- [query] [wait_for_non_stale_results](../../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresults)
- where
- WhereBetween
- [where_ends_with](../../../../client-api/session/querying/text-search/ends-with-query)
- where_equals
- [where_exists](../../../../client-api/session/querying/how-to-filter-by-field)
- where_greater_than
- where_greater_than_or_equal
- where_in
- where_less_than
- where_less_than_or_equal
- [where_lucene](../../../../client-api/session/querying/document-query/how-to-use-lucene)
- where_not_equals
- [where_regex](../../../../client-api/session/querying/text-search/using-regex)
- [where_starts_with](../../../../client-api/session/querying/text-search/starts-with-query)
- within_radius_of

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [How to Use Lucene](../../../../client-api/session/querying/document-query/how-to-use-lucene)

### Querying 

- [query vs document_query](../../../../client-api/session/querying/document-query/query-vs-document-query)
- [Projections](../../../../indexes/querying/projections)
