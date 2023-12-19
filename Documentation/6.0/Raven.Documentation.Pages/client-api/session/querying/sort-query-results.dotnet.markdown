# Sort Query Results

---

{NOTE: }

* When making a query, the server will return the results __sorted__ only if explicitly requested by the query.  
  If no sorting method is specified when issuing the query then results will not be sorted.

* Sorting is applied by the server after the query filtering stage.  
  Applying filtering is recommended as it reduces the number of results RavenDB needs to sort  
  when querying a large dataset.

* Multiple sorting actions can be chained.

* This article provides examples of sorting query results when making a __dynamic-query__.  
  For sorting results when querying a __static-index__ see [sort index query results](../../../indexes/querying/sorting).

* In this page:
    * [Order by field value](../../../client-api/session/querying/sort-query-results#order-by-field-value)
 
    * [Order by score](../../../client-api/session/querying/sort-query-results#order-by-score)
  
    * [Order by random](../../../client-api/session/querying/sort-query-results#order-by-random)
   
    * [Order by spatial](../../../client-api/session/querying/sort-query-results#order-by-spatial)
     
    * [Order by count (aggregation query)](../../../client-api/session/querying/sort-query-results#order-by-count-(aggregation-query))
  
    * [Order by sum (aggregation query)](../../../client-api/session/querying/sort-query-results#order-by-sum-(aggregation-query))

    * [Force ordering type](../../../client-api/session/querying/sort-query-results#force-ordering-type)

    * [Chain ordering](../../../client-api/session/querying/sort-query-results#chain-ordering)

    * [Custom sorters](../../../client-api/session/querying/sort-query-results#custom-sorters) 

    * [Syntax](../../../client-api/session/querying/sort-query-results#syntax)

{NOTE/}

---

{PANEL: Order by field value}

* Use `OrderBy` or `OrderByDescending` to order the results by the specified document-field.

{CODE-TABS}
{CODE-TAB:csharp:Query sort_1@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB:csharp:Query_async sort_2@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB:csharp:DocumentQuery sort_3@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where UnitsInStock > 10
order by UnitsInStock as long
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{INFO: }

__Ordering Type__:

* By default, the `OrderBy` methods will determine the `OrderingType` from the property path expression  
  and specify that ordering type in the generated RQL that is sent to the server.  

* E.g. in the above example, ordering by `x => x.UnitsInStock` will result in `OrderingType.Long`  
  because that property data type is an integer.

* Different ordering can be forced - see [Force ordering type](../../../client-api/session/querying/sort-query-results#force-ordering-type) below.

{INFO/}

{PANEL/}

{PANEL: Order by score}

* When querying with some filtering conditions, a basic score is calculated for each item in the results  
  by the underlying indexing engine. (Read more about Lucene scoring [here](https://lucene.apache.org/core/3_3_0/scoring.html)).

* The higher the score value the better the match.  

* Use `OrderByScore` or `OrderByScoreDescending` to order by this score.

{CODE-TABS}
{CODE-TAB:csharp:Query sort_4@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB:csharp:Query_async sort_5@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB:csharp:DocumentQuery sort_6@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where UnitsInStock < 5 or Discontinued == true
order by score()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{INFO: }

__Get resulting score__:

---

* __@Index-score metadata property__:  
  The score is available in the `@index-score` metadata property within each result.  
  Learn how to retrieve the resulting score from the metadata in [Get resulting score](../../../client-api/session/querying/text-search/boost-search-results#get-resulting-score).  
  <br>
  Note the following difference between the underlying indexing engines:

    * When using __Lucene__:  
      This metadata property is always available in the results.
      Read more about Lucene scoring [here](https://lucene.apache.org/core/3_3_0/scoring.html).

    * When using __Corax__:  
      In order to enhance performance, this property is not included in the results by default.  
      To get this metadata property you must set the `Indexing.Corax.IncludeDocumentScore` configuration value to _true_.
      Learn how to set configuration values in this [Configuration overview](../../../server/configuration/configuration-options).

* __Include explanations__:  
  Another option to get the score details and see how it was calculated is to request to include explanations in the query.
  Currently, this is only available when using Lucene.  
  See [Include query explanations](../../../client-api/session/querying/debugging/include-explanations).

{INFO/}

{PANEL/}

{PANEL: Order by random}

* Use `RandomOrdering` to randomize the order of the query results.

* An optional seed parameter can be passed.

{CODE-TABS}
{CODE-TAB:csharp:Query sort_7@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB:csharp:Query_async sort_8@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB:csharp:DocumentQuery sort_9@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where UnitsInStock > 10
order by random()
// order by random(someSeed)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Order by spatial}

* If your data contains geographical locations,  
  spatial query results can be sorted based on their distance from a specific point.

* See detailed explanation in [Spatial Sorting](../../../client-api/session/querying/how-to-make-a-spatial-query#spatial-sorting).

{PANEL/}

{PANEL: Order by count (aggregation query)}

* The results of a [group-by query](../../../client-api/session/querying/how-to-perform-group-by-query) can be sorted by the `Count` aggregation operation used in the query.

{CODE-TABS}
{CODE-TAB:csharp:Query sort_10@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB:csharp:Query_async sort_11@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB:csharp:DocumentQuery sort_12@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
group by Category
order by count() as long
select key() as "Category", count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Order by sum (aggregation query)}

* The results of a [group-by query](../../../client-api/session/querying/how-to-perform-group-by-query) can be sorted by the `Sum` aggregation operation used in the query.

{CODE-TABS}
{CODE-TAB:csharp:Query sort_13@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB:csharp:Query_async sort_14@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB:csharp:DocumentQuery sort_15@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
group by Category
order by Sum as long
select key() as 'Category', sum(UnitsInStock) as Sum
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Force ordering type}

* By default, the `OrderBy` methods will determine the `OrderingType` from the property path expression  
  and specify that ordering type in the generated RQL that is sent to the server.

* A different ordering can be forced by passing the ordering type explicitly to `OrderBy` or `OrderByDescending`.

* The following ordering types are available:

    * `OrderingType.Long`
    * `OrderingType.Double`
    * `OrderingType.AlphaNumeric`
    * `OrderingType.String` (lexicographic ordering)

* When using RQL directly, if no ordering type is specified, then the server defaults to lexicographic ordering.

{NOTE: }

__Using alphanumeric ordering example__:

* When ordering mixed-character strings by the default lexicographical ordering  
  then comparison is done character by character based on the Unicode values.  
  For example, "Abc9" will come after "Abc10" since 9 is greater than 1.

* If you want the digit characters to be ordered as numbers then use alphanumeric ordering  
  where "Abc10" will result after "Abc9".

{CODE-TABS}
{CODE-TAB:csharp:Query sort_16@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB:csharp:Query_async sort_17@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB:csharp:DocumentQuery sort_18@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
order by QuantityPerUnit as alphanumeric
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE sort_16_results@ClientApi\Session\Querying\SortQueryResults.cs /}

{NOTE/}

{PANEL/}

{PANEL: Chain ordering}

* It is possible to chain multiple orderings in the query.  
  Any combination of secondary sorting is possible as the fields are indexed independently of one another.

* There is no limit on the number of sorting actions that can be chained.
  
* This is achieved by using the `ThenBy` (`ThenByDescending`) and `ThenByScore` (`ThenByScoreDescending`) methods.

{CODE-TABS}
{CODE-TAB:csharp:Query sort_19@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB:csharp:Query_async sort_20@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB:csharp:DocumentQuery sort_21@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where UnitsInStock > 10
order by UnitsInStock as long desc, score(), Name
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Custom sorters }

* The Lucene indexing engine allows you to create your own custom sorters.  
  Custom sorters are not supported by [Corax](../../../indexes/search-engine/corax).  
 
* Custom sorters can be deployed to the server by either:  

     * Sending the [Put Sorters Operation](../../../client-api/operations/maintenance/sorters/put-sorter) from your code.
  
     * Uploading a custom sorter from Studio, see [Custom Sorters View](../../../studio/database/settings/custom-sorters).

* Once the custom sorter is deployed, you can sort the query results with it.

{CODE-TABS}
{CODE-TAB:csharp:Query sort_22@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB:csharp:Query_async sort_23@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB:csharp:DocumentQuery sort_24@ClientApi\Session\Querying\SortQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where UnitsInStock > 10
order by custom(UnitsInStock, "MySorter")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax@ClientApi\Session\Querying\SortQueryResults.cs /}

| Parameter      | Type                          | Description                                                                                                                                                                |
|----------------|-------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __path__       | `string`                      | The name of the field to sort by                                                                                                                                           |
| __path__       | `Expression<Func<T, object>>` | A lambda expression to the field by which to sort                                                                                                                          |
| __ordering__   | `QueryStatistics`             | The ordering type that will be used to sort the results:<br>`OrderingType.Long`<br>`OrderingType.Double`<br>`OrderingType.AlphaNumeric`<br>`OrderingType.String` (default) |
| __sorterName__ | `string`                      | The name of your custom sorter class                                                                                                                                       |

{PANEL/}

## Related Articles

#### Client API

- [Query Overview](../../../client-api/session/querying/how-to-query)

### Querying

- [Customize query](../../../client-api/session/querying/how-to-customize-query)
- [Group by query](../../../client-api/session/querying/how-to-perform-group-by-query)
- [Spatial query](../../../client-api/session/querying/how-to-make-a-spatial-query)
- [Full-text search](../../../client-api/session/querying/text-search/full-text-search)

### Indexes

- [Sort index query results](../../../indexes/querying/sorting)
