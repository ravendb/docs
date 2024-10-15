# Sort Query Results

---

{NOTE: }

* When making a query, the server will return the results **sorted** only if explicitly requested by the query.  
  If no sorting method is specified when issuing the query then results will not be sorted.
  
    * Note: An exception to the above rule is when [Boosting](../../../indexes/boosting) is involved in the query.  
      Learn more in [Automatic score-based ordering](../../../indexes/boosting#automatic-score-based-ordering).  

* Sorting is applied by the server after the query filtering stage.  
  Applying filtering is recommended as it reduces the number of results RavenDB needs to sort  
  when querying a large dataset.

* Multiple sorting actions can be chained.

* This article provides examples of sorting query results when making a **dynamic-query**.  
  For sorting results when querying a **static-index** see [sort index query results](../../../indexes/querying/sorting).

* In this page:
    * [Order by field value](../../../client-api/session/querying/sort-query-results#order-by-field-value) 
  
    * [Order by score](../../../client-api/session/querying/sort-query-results#order-by-score)  
        * [Get resulting score](../../../client-api/session/querying/sort-query-results#get-resulting-score)
     
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

* Use `orderBy` or `orderByDescending` (see below) to order the results by the specified document field.

{CODE-TABS}
{CODE-TAB:php:Query sort_1@ClientApi\Session\Querying\SortQueryResults.php /}
{CODE-TAB:php:DocumentQuery sort_3@ClientApi\Session\Querying\SortQueryResults.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where UnitsInStock > 10
order by UnitsInStock as long
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{INFO: }

**Ordering Type**:

* By default, the `orderBy` methods will determine the `OrderingType` from the property path expression  
  and specify that ordering type in the generated RQL that is sent to the server.  

* E.g. in the above example, ordering by `UnitsInStock` will result in `OrderingType::long`  
  because this property's data type is integer.

* Different ordering can be forced - see [Force ordering type](../../../client-api/session/querying/sort-query-results#force-ordering-type) below.

{INFO/}

{PANEL/}

{PANEL: Order by score}

* When querying with some filtering conditions, a basic score is calculated for each item in the results  
  by the underlying indexing engine. (Read more about Lucene scoring [here](https://lucene.apache.org/core/3_3_0/scoring.html)).

* The higher the score value the better the match.  

* Use `orderByScore` to order the query results by this score.

{CODE-TABS}
{CODE-TAB:php:Query sort_4@ClientApi\Session\Querying\SortQueryResults.php /}
{CODE-TAB:php:DocumentQuery sort_6@ClientApi\Session\Querying\SortQueryResults.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where UnitsInStock < 5 or Discontinued == true
order by score()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{INFO: }

#### Get resulting score:

---

The score details can be retrieved by either:  
 
  * **Request to include explanations**:  
    You can get the score details and see how it was calculated by requesting to include explanations in the query. 
    Currently, this is only available when using Lucene as the underlying indexing engine.  
    Learn more in [Include query explanations](../../../client-api/session/querying/debugging/include-explanations).
   
  * **Get score from metadata**:  
    The score is available in the `INDEX_SCORE` metadata property within each result.  
    The following example shows how to get the score from the metadata of the resulting entities that were loaded to the session:

    {CODE:php get_score_from_metadata@ClientApi\Session\Querying\SortQueryResults.php /}

{INFO/}

{PANEL/}

{PANEL: Order by random}

* Use `randomOrdering` to randomize the order of the query results.

* An optional seed parameter can be passed.

{CODE-TABS}
{CODE-TAB:php:Query sort_7@ClientApi\Session\Querying\SortQueryResults.php /}
{CODE-TAB:php:DocumentQuery sort_9@ClientApi\Session\Querying\SortQueryResults.php /}
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
{CODE-TAB:php:Query sort_10@ClientApi\Session\Querying\SortQueryResults.php /}
{CODE-TAB:php:DocumentQuery sort_12@ClientApi\Session\Querying\SortQueryResults.php /}
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
{CODE-TAB:php:Query sort_13@ClientApi\Session\Querying\SortQueryResults.php /}
{CODE-TAB:php:DocumentQuery sort_15@ClientApi\Session\Querying\SortQueryResults.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
group by Category
order by Sum as long
select key() as 'Category', sum(UnitsInStock) as Sum
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Force ordering type}

* By default, the `orderBy` methods will determine the `OrderingType` from the property path expression  
  and specify that ordering type in the generated RQL that is sent to the server.

* A different ordering can be forced by passing the ordering type explicitly to `orderBy` or `orderByDescending`.

* The following ordering types are available:

    * `OrderingType::long`
    * `OrderingType::double`
    * `OrderingType::alphaNumeric`
    * `OrderingType::string` (lexicographic ordering)

* When using RQL directly, if no ordering type is specified, then the server defaults to lexicographic ordering.

{NOTE: }

**Using alphanumeric ordering example**:

* When ordering mixed-character strings by the default lexicographical ordering  
  then comparison is done character by character based on the Unicode values.  
  For example, "Abc9" will come after "Abc10" since 9 is greater than 1.

* If you want the digit characters to be ordered as numbers then use alphanumeric ordering  
  where "Abc10" will result after "Abc9".

{CODE-TABS}
{CODE-TAB:php:Query sort_16@ClientApi\Session\Querying\SortQueryResults.php /}
{CODE-TAB:php:DocumentQuery sort_18@ClientApi\Session\Querying\SortQueryResults.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
order by QuantityPerUnit as alphanumeric
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Chain ordering}

* It is possible to chain multiple orderings in the query.  
  Any combination of secondary sorting is possible as the fields are indexed independently of one another.

* There is no limit on the number of sorting actions that can be chained.
  
{CODE-TABS}
{CODE-TAB:php:Query sort_19@ClientApi\Session\Querying\SortQueryResults.php /}
{CODE-TAB:php:DocumentQuery sort_21@ClientApi\Session\Querying\SortQueryResults.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where UnitsInStock > 10
order by UnitsInStock as long desc, score(), Name
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Custom sorters }

* The Lucene indexing engine allows you to create your own custom sorters.  
  Custom sorters can be deployed to the server by either:  

     * Sending the [Put Sorters Operation](../../../client-api/operations/maintenance/sorters/put-sorter) from your code.
  
     * Uploading a custom sorter from Studio, see [Custom Sorters View](../../../studio/database/settings/custom-sorters).

* Once the custom sorter is deployed, you can sort the query results with it.

{CODE-TABS}
{CODE-TAB:php:Query sort_22@ClientApi\Session\Querying\SortQueryResults.php /}
{CODE-TAB:php:DocumentQuery sort_24@ClientApi\Session\Querying\SortQueryResults.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where UnitsInStock > 10
order by custom(UnitsInStock, "MySorter")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{code:php syntax@ClientApi\Session\Querying\SortQueryResults.php /}

| Parameter  | Type     | Description          |
|------------|----------|----------------------|
| **$field** | `string` | The field to sort by |
| **$sorterNameOrOrdering** | `string` | The ordering type to sort the results by:<br>`OrderingType::long`<br>`OrderingType::double`<br>`OrderingType::alphaNumeric`<br>`OrderingType::string` (default) |

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
