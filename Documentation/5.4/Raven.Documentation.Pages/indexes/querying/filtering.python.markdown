# Filter Query Results
---

{NOTE: }

* One of the most basic functionalities of querying is the ability to filter out data and return records that match a given condition.  

* Index queries can be executed using -  
   * [query_index_type](../../client-api/session/querying/how-to-query) from the basic `session` API  
   * [RQL](../../client-api/session/querying/what-is-rql) - Raven Query Language  

* The examples in this page demonstrate how filtering is applied by each of these methods.  

* In this page:
   * [`where_equals` - Where equals](../../indexes/querying/filtering#where_equals---where-equals)
   * [`where_greater_than` - Numeric property](../../indexes/querying/filtering#where_greater_than---numeric-property)
   * [`where_greater_than` - Nested property](../../indexes/querying/filtering#where_greater_than---nested-property)
   * [`where_in` - Single -vs- Multiple values](../../indexes/querying/filtering#where_in---single--vs--multiple-values)
   * [`contains_any` - Any value from specified collection](../../indexes/querying/filtering#contains_any---any-value-from-specified-collection)
   * [`contains_all` - All values from specified collection](../../indexes/querying/filtering#contains_all---all-values-from-specified-collection)
   * [`where_starts_with` - All records with given prefix](../../indexes/querying/filtering#where_starts_with---all-records-with-given-prefix)
   * [`where_ends_with` - All records with given suffix](../../indexes/querying/filtering#where_ends_with---all-records-with-given-suffix)
   * [Where - Identifier Property](../../indexes/querying/filtering#where---identifier-property)
   * [`where_exists` - Where exists or doesn't exist](../../indexes/querying/filtering#where_exists---where-exists-or-doesn)

{NOTE/}

{PANEL: `where_equals` - Where equals }

{CODE-TABS}
{CODE-TAB:python:Query filtering_0_1@Indexes\Querying\Filtering.py /}
{CODE-TAB:python:Index filtering_0_4@Indexes\Querying\Filtering.py  /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastName'
where FirstName = 'Robert' and LastName = 'King'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: `where_greater_than` - Numeric property}

{CODE-TABS}
{CODE-TAB:python:Query filtering_1_1@Indexes\Querying\Filtering.py /}
{CODE-TAB:python:Index filtering_1_4@Indexes\Querying\Filtering.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Products/ByUnitsInStock'
where UnitsInStock > 50
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: `where_greater_than` - Nested property}

{CODE-TABS}
{CODE-TAB:python:Query filtering_10_1@Indexes\Querying\Filtering.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
where ShipTo.City = 'Albuquerque'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:python:Query filtering_2_1@Indexes\Querying\Filtering.py /}
{CODE-TAB:python:Index filtering_2_4@Indexes\Querying\Filtering.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Order/ByOrderLinesCount'
where Lines.Count > 50
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: `where_in` - Single -vs- Multiple values}

When you want to check a single value against multiple values, `where_in` can become handy.  
To retrieve all employees where `FirstName` is either `Robert` or `Nancy`, we can issue the following query:

{CODE-TABS}
{CODE-TAB:python:Query filtering_4_1@Indexes\Querying\Filtering.py /}
{CODE-TAB:python:Index filtering_0_4@Indexes\Querying\Filtering.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastName'
where FirstName IN ('Robert', 'Nancy')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: `contains_any` - Any value from specified collection}

To check if enumeration contains **any** of the values from a specified collection, 
use the `contains_any` method.

For example, if you want to return all `BlogPosts` that contain any of the specified `Tags`:

{CODE-TABS}
{CODE-TAB:python:Query filtering_5_1@Indexes\Querying\Filtering.py /}
{CODE-TAB:python:Index filtering_5_4@Indexes\Querying\Filtering.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'BlogPosts/ByTags'
where Tags IN ('Development', 'Research')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: `contains_all` - All values from specified collection}

To check if an enumeration contains **all** of the values from a specified collection, 
use the `contains_all` method.

For example, if you want to return all the `BlogPosts` that contain all of the specified `Tags`:

{CODE-TABS}
{CODE-TAB:python:Query filtering_6_1@Indexes\Querying\Filtering.py /}
{CODE-TAB:python:Index filtering_5_4@Indexes\Querying\Filtering.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'BlogPosts/ByTags'
where Tags ALL IN ('Development', 'Research')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: `where_starts_with` - All records with given prefix}

{CODE-TABS}
{CODE-TAB:python:Query filtering_8_1@Indexes\Querying\Filtering.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Products 
where startsWith(Name, 'ch')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: `where_ends_with` - All records with given suffix}

{CODE-TABS}
{CODE-TAB:python:Query filtering_9_1@Indexes\Querying\Filtering.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Products 
where endsWith(Name, 'ra')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Where - Identifier Property}

Once a property used in the `where_equals` clause is recognized as an identity property of a given entity type 
(according to [`FindIdentityProperty` convention](../../client-api/configuration/identifier-generation/global#findidentityproperty))
and there aren't any other fields involved in the query, then it is called a "collection query". 
Simple collection queries that ask about documents with given IDs or where identifiers start with a given prefix
and don't require any additional handling like ordering, full-text searching, etc, are handled directly by the storage engine. 
It means that querying by ID doesn't create an auto-index and has no extra cost. In terms of efficiency, it is the same as
loading documents using [`session.load`](../../client-api/session/loading-entities).  


{CODE-TABS}
{CODE-TAB:python:Query filtering_11_1@Indexes\Querying\Filtering.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
where id() = 'orders/1-A'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:python:Query filtering_12_1@Indexes\Querying\Filtering.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
where startsWith(id(), 'orders/1')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: `where_exists` - Where exists or doesn't exist}

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
