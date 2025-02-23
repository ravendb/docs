# Filter Query Results
---

{NOTE: }

* One of the most basic functionalities of querying is the ability to filter out data and return records that match a given condition.  

* Index queries can be executed using -  
   * [query_index_type](../../client-api/session/querying/how-to-query) from the basic `session` API  
   * [RQL](../../client-api/session/querying/what-is-rql) - Raven Query Language  

* The examples in this page demonstrate how filtering is applied by each of these methods.  

* In this page:
   * [`whereEquals` - Where equals](../../indexes/querying/filtering#whereequals---where-equals)
   * [`whereGreaterThan` - Numeric property](../../indexes/querying/filtering#wheregreaterthan---numeric-property)
   * [`whereGreaterThan` - Nested property](../../indexes/querying/filtering#wheregreaterthan---nested-property)
   * [`whereIn` - Single -vs- Multiple values](../../indexes/querying/filtering#wherein---single--vs--multiple-values)
   * [`containsAny` - Any value from specified collection](../../indexes/querying/filtering#containsany---any-value-from-specified-collection)
   * [`containsAll` - All values from specified collection](../../indexes/querying/filtering#containsall---all-values-from-specified-collection)
   * [`whereStartsWith` - All records with given prefix](../../indexes/querying/filtering#wherestartswith---all-records-with-given-prefix)
   * [`whereEndsWith` - All records with given suffix](../../indexes/querying/filtering#whereendswith---all-records-with-given-suffix)
   * [Where - Identifier Property](../../indexes/querying/filtering#where---identifier-property)
   * [`whereExists` - Where exists or doesn't exist](../../indexes/querying/filtering#whereexists---where-exists-or-doesn)

{NOTE/}

{PANEL: `whereEquals` - Where equals }

{CODE-TABS}
{CODE-TAB:php:Query filtering_0_1@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:documentQuery filtering_0_2@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:Index filtering_0_4@Indexes\Querying\Filtering.php  /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastName'
where FirstName = 'Robert' and LastName = 'King'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: `whereGreaterThan` - Numeric property}

{CODE-TABS}
{CODE-TAB:php:Query filtering_1_1@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:documentQuery filtering_1_2@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:Index filtering_1_4@Indexes\Querying\Filtering.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Products/ByUnitsInStock'
where UnitsInStock > 50
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: `whereGreaterThan` - Nested property}

{CODE-TABS}
{CODE-TAB:php:Query filtering_10_1@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:documentQuery filtering_10_2@Indexes\Querying\Filtering.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
where ShipTo.City = 'Albuquerque'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:php:Query filtering_2_1@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:documentQuery filtering_2_2@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:Index filtering_2_4@Indexes\Querying\Filtering.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Order/ByOrderLinesCount'
where Lines.Count > 50
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: `whereIn` - Single -vs- Multiple values}

When you want to check a single value against multiple values, `whereIn` can become handy.  
To retrieve all employees where `FirstName` is either `Robert` or `Nancy`, we can issue the following query:

{CODE-TABS}
{CODE-TAB:php:Query filtering_4_1@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:documentQuery filtering_4_2@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:Index filtering_0_4@Indexes\Querying\Filtering.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastName'
where FirstName IN ('Robert', 'Nancy')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: `containsAny` - Any value from specified collection}

To check if enumeration contains **any** of the values from a specified collection, 
use the `containsAny` method.

For example, if you want to return all `BlogPosts` that contain any of the specified `Tags`:

{CODE-TABS}
{CODE-TAB:php:Query filtering_5_1@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:documentQuery filtering_5_2@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:Index filtering_5_4@Indexes\Querying\Filtering.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'BlogPosts/ByTags'
where Tags IN ('Development', 'Research')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: `containsAll` - All values from specified collection}

To check if an enumeration contains **all** of the values from a specified collection, 
use the `containsAll` method.

For example, if you want to return all the `BlogPosts` that contain all of the specified `Tags`:

{CODE-TABS}
{CODE-TAB:php:Query filtering_6_1@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:documentQuery filtering_6_2@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:Index filtering_5_4@Indexes\Querying\Filtering.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'BlogPosts/ByTags'
where Tags ALL IN ('Development', 'Research')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: `whereStartsWith` - All records with given prefix}

{CODE-TABS}
{CODE-TAB:php:Query filtering_8_1@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:documentQuery filtering_8_2@Indexes\Querying\Filtering.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Products 
where startsWith(Name, 'ch')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: `whereEndsWith` - All records with given suffix}

{CODE-TABS}
{CODE-TAB:php:Query filtering_9_1@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:documentQuery filtering_9_2@Indexes\Querying\Filtering.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Products 
where endsWith(Name, 'ra')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Where - Identifier Property}

Once a property used in the `whereEquals` clause is recognized as an identity property of a given entity type 
(according to [`FindIdentityProperty` convention](../../client-api/configuration/identifier-generation/global#findidentityproperty))
and there aren't any other fields involved in the query, then it is called a "collection query". 
Simple collection queries that ask about documents with given IDs or where identifiers start with a given prefix
and don't require any additional handling like ordering, full-text searching, etc, are handled directly by the storage engine. 
It means that querying by ID doesn't create an auto-index and has no extra cost. In terms of efficiency, it is the same as
loading documents using [`session->load`](../../client-api/session/loading-entities).  


{CODE-TABS}
{CODE-TAB:php:Query filtering_11_1@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:documentQuery filtering_11_2@Indexes\Querying\Filtering.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
where id() = 'orders/1-A'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:php:Query filtering_12_1@Indexes\Querying\Filtering.php /}
{CODE-TAB:php:documentQuery filtering_12_2@Indexes\Querying\Filtering.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
where startsWith(id(), 'orders/1')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: `whereExists` - Where exists or doesn't exist}

To find all documents in a collection that have a specified field, see [How to Filter by Field Presence](../../client-api/session/querying/how-to-filter-by-field).  

To find all documents in a collection that don't have a specified field, see [How to Filter by Non-Existing Field](../../client-api/session/querying/how-to-filter-by-non-existing-field).

{PANEL/}

## Related Articles

### Client API

- [Query Overview](../../client-api/session/querying/how-to-query)

### Indexes

- [Indexing Basics](../../indexes/indexing-basics)

### Querying

- [Query an Index](../../indexes/querying/query-index)
- [Paging](../../indexes/querying/paging)
- [Sorting](../../indexes/querying/sorting)
