# What is a Document Query?

---

{NOTE: }

* Queries in RavenDB can be written using `Query`, `DocumentQuery` or directly with `RQL`.  
  Learn more in [Query Overview](../../../../client-api/session/querying/how-to-query).

* Unlike the `Query` method, the `DocumentQuery` method does Not support LINQ.  
  However, it gives you more flexibility and control over the process of building a query,  
  as it provides low-level querying capabilities. See [Query -vs- DocumentQuery](../../../../client-api/session/querying/document-query/query-vs-document-query) for all differences.

* In this page:
  * [DocumentQuery examples](../../../../client-api/session/querying/document-query/what-is-document-query#documentquery-examples)
  * [Convert between DocumentQuery and Query](../../../../client-api/session/querying/document-query/what-is-document-query#convert-between-documentquery-and-query)
  * [Custom Methods and Extensions](../../../../client-api/session/querying/document-query/what-is-document-query#custom-methods-and-extensions)
  * [Syntax](../../../../client-api/session/querying/document-query/what-is-document-query#syntax)

{NOTE/}

---

{PANEL: DocumentQuery examples}

{NOTE: }

__Query collection - no filtering__

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery documentQuery_1@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /})
{CODE-TAB:csharp:DocumentQuery_async documentQuery_1_async@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /})
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Query collection - by ID__

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery documentQuery_2@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /})
{CODE-TAB:csharp:DocumentQuery_async documentQuery_2_async@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /})
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" where id() == "employees/1-A"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Query collection - with filtering__

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery documentQuery_3@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /})
{CODE-TAB:csharp:DocumentQuery_async documentQuery_3_async@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /})
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" where FirstName == "Robert"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Query collection - with paging__

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery documentQuery_4@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /})
{CODE-TAB:csharp:DocumentQuery_async documentQuery_4_async@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /})
{CODE-TAB-BLOCK:sql:RQL}
from "Products" limit 5, 10 // skip 5, take 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Query an index__

* Please refer to [Querying an index](../../../../indexes/querying/query-index#session.advanced.documentquery) for examples of querying an index using a DocumentQuery.

{NOTE/}

{PANEL/}

{PANEL: Convert between DocumentQuery and Query}

{NOTE: }

__DocumentQuery to Query__

---

* A `DocumentQuery` can be converted to a `Query`.  
  This enables you to take advantage of all available LINQ extensions provided by RavenDB.  

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery documentQuery_5@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /})
{CODE-TAB:csharp:DocumentQuery_async documentQuery_5_async@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /})
{CODE-TAB-BLOCK:sql:RQL}
from "Orders" where Freight > 25
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* Convert `DocumentQuery` to `Query` when you need to project data from a related document  
  in a dynamic query.

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery documentQuery_6@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /})
{CODE-TAB-BLOCK:sql:RQL}
from "Orders" as o
where o.Freight > 25
load o.Company as c
select {
    Freight: o.Freight,
    CompanyName: c.Name
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Query to DocumentQuery__

---

* A `Query` can be converted to a `DocumentQuery`.  
  This enables you to take advantage of the API available only for _DocumentQuery_.  

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery documentQuery_7@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /})
{CODE-TAB:csharp:DocumentQuery_async documentQuery_7_async@ClientApi\Session\Querying\DocumentQuery\WhatIsDocumentQuery.cs /})
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
where Freight > 25
include explanations()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Custom Methods and Extensions}

{NOTE: }

Several methods share the same functionality as their `Query` counterparts.  
Refer to the corresponding documentation articles, marked with links starting with "[Query]" in the list below.

{NOTE/}

Available custom methods and extensions:   

- AddOrder
- [Query] [AfterQueryExecuted](../../../../client-api/session/querying/how-to-customize-query#afterqueryexecuted)
- [Query] [AfterStreamExecuted](../../../../client-api/session/querying/how-to-customize-query#afterstreamexecuted)
- [Query] [AggregateBy](../../../../client-api/session/querying/how-to-perform-a-faceted-search)
- [Query] [AggregateUsing](../../../../client-api/session/querying/how-to-perform-a-faceted-search)
- AndAlso
- [Query] [BeforeQueryExecuted](../../../../client-api/session/querying/how-to-customize-query#beforequeryexecuted)
- Boost
- CloseSubclause
- CmpXchg
- ContainsAll
- ContainsAny
- Count
- CountLazily
- Distinct
- ExplainScores
- First
- FirstOrDefault
- Fuzzy
- GetIndexQuery
- GetQueryResult
- [GroupBy](../../../../client-api/session/querying/how-to-perform-group-by-query)
- [GroupByArrayValues](../../../../client-api/session/querying/how-to-perform-group-by-query#by-array-values)
- [GroupByArrayContent](../../../../client-api/session/querying/how-to-perform-group-by-query#by-array-content)
- [Query] [Highlight](../../../../client-api/session/querying/text-search/highlight-query-results)
- Include
- IncludeExplanations
- Intersect
- InvokeAfterQueryExecuted
- InvokeAfterStreamExecuted
- [Query] [Lazily](../../../../client-api/session/querying/how-to-perform-queries-lazily)
- LongCount
- MoreLikeThis
- NegateNext
- [Not](../../../../client-api/session/querying/document-query/how-to-use-not-operator)
- [Query] [NoCaching](../../../../client-api/session/querying/how-to-customize-query#nocaching)
- [Query] [NoTracking](../../../../client-api/session/querying/how-to-customize-query#notracking)
- OfType
- OpenSubclause
- OrderBy
- OrderByDescending
- [Query] [OrderByDistance](../../../../client-api/session/querying/how-to-make-a-spatial-query#orderByDistance)
- [Query] [OrderByDistanceDescending](../../../../client-api/session/querying/how-to-make-a-spatial-query#orderByDistanceDesc)
- OrderByScore
- OrderByScoreDescending
- OrElse
- [Query] [Projection](../../../../client-api/session/querying/how-to-customize-query#projection)
- Proximity
- [Query] [RandomOrdering](../../../../client-api/session/querying/how-to-customize-query#randomordering)
- [Query] [RelatesToShape](../../../../client-api/session/querying/how-to-make-a-spatial-query#search-by-shape)
- Search
- SelectFields
- SelectTimeSeries
- Single
- SingleOrDefault
- Skip
- [Query] [Spatial](../../../../client-api/session/querying/how-to-make-a-spatial-query)
- Statistics
- [SuggestUsing](../../../../client-api/session/querying/how-to-work-with-suggestions)
- Take
- [Query] [Timings](../../../../client-api/session/querying/how-to-customize-query#timings)
- UsingDefaultOperator
- [Query] [WaitForNonStaleResults](../../../../client-api/session/querying/how-to-customize-query#waitfornonstaleresults)
- Where
- WhereBetween
- WhereEndsWith
- WhereEquals
- [WhereExists](../../../../client-api/session/querying/how-to-filter-by-field)
- WhereGreaterThan
- WhereGreaterThanOrEqual
- WhereIn
- WhereLessThan
- WhereLessThanOrEqual
- [WhereLucene](../../../../client-api/session/querying/document-query/how-to-use-lucene)
- WhereNotEquals
- [WhereRegex](../../../../client-api/session/querying/text-search/using-regex)
- [WhereStartsWith](../../../../client-api/session/querying/text-search/starts-with-search)
- WithinRadiusOf

{PANEL/}

{PANEL: Syntax}

The available `DocumentQuery` overloads are listed in this [Syntax section](../../../../client-api/session/querying/how-to-query#syntax) in the [Query Overview](../../../../client-api/session/querying/how-to-query).

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [How to Use Lucene](../../../../client-api/session/querying/document-query/how-to-use-lucene)

### Querying 

- [Query vs DocumentQuery](../../../../client-api/session/querying/document-query/query-vs-document-query)
- [Projections](../../../../indexes/querying/projections)
