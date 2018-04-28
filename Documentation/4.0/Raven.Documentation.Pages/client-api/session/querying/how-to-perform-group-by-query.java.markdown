#Session : Querying : How to Perform Dynamic Group By Query

Since RavenDB 4.0, the query optimizer supports dynamic group by queries and automatically creates auto map-reduce indexes.

You can create a dynamic query that does an aggregation by using the `groupBy()` method.

The supported aggregation operations are:

- `count`
- `sum`

<br />

{PANEL: Group By Single Field}

{CODE-TABS}
{CODE-TAB:java:Java group_by_1@ClientApi\Session\Querying\HowToPerformGroupByQuery.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
group by shipTo.city
select shipTo.city as country, sum(lines[].quantity) as totalQuantity
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Group By Multiple Fields}

{CODE-TABS}
{CODE-TAB:java:Java group_by_2@ClientApi\Session\Querying\HowToPerformGroupByQuery.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
group by employee, company
select employee as employeeIdentifier, company, count() as count
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Select Composite GroupBy Key}

{CODE-TABS}
{CODE-TAB:java:Java group_by_3@ClientApi\Session\Querying\HowToPerformGroupByQuery.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by employee, company
select key() as employeeCompanyPair, count() as count
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Group By Array}

### By Array Values

In order to group by values of array, you need to use `groupBy(array(...))`. The following query will group by `product` field from `lines` collection 
and calculate the count per ordered products. Underneath a [fanout](../../../indexes/fanout-indexes), an auto map-reduce index will be created to handle such query. 

{CODE-TABS}
{CODE-TAB:java:Java group_by_4@ClientApi\Session\Querying\HowToPerformGroupByQuery.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by lines[].product
select lines[].product, count() as count
{CODE-TAB-BLOCK/}
{CODE-TABS/}

Inside a single group by statement you can mix collection values and value of another property. That's supported by `DocumentQuery` only:

{CODE-TABS}
{CODE-TAB:java:Java group_by_5@ClientApi\Session\Querying\HowToPerformGroupByQuery.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by lines[].product, shipTo.country 
select lines[].product as product, shipTo.country as country, count() as count
{CODE-TAB-BLOCK/}
{CODE-TABS/}

Grouping by multiple values from **the same** collection is supported as well:

{CODE-TABS}
{CODE-TAB:java:Java group_by_6@ClientApi\Session\Querying\HowToPerformGroupByQuery.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by lines[].product, lines[].quantity 
select lines[].product as product, lines[].quantity as quantity, count() as count
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### By Array Content

Another option is to group by array content. The reduction key will be calculated based on all values of a collection specified in `GroupBy`.
The client API exposes the `GroupByArrayContent` extension method for that purpose.

{CODE-TABS}
{CODE-TAB:java:Java group_by_7@ClientApi\Session\Querying\HowToPerformGroupByQuery.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
group by array(lines[].product)
select key() as products, count() as count
{CODE-TAB-BLOCK/}
{CODE-TABS/}

Grouping by array content and a value of another property is supported by `DocumentQuery`:

{CODE-TABS}
{CODE-TAB:java:Java group_by_8@ClientApi\Session\Querying\HowToPerformGroupByQuery.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by array(lines[].product), shipTo.country 
select lines[].product as products, shipTo.country as country, count() as count
{CODE-TAB-BLOCK/}
{CODE-TABS/}

Grouping by multiple values from **the same** collection is also supported by `DocumentQuery`:

{CODE-TABS}
{CODE-TAB:java:Java group_by_9@ClientApi\Session\Querying\HowToPerformGroupByQuery.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by array(lines[].product), array(lines[].quantity) 
select lines[].product as products, lines[].quantity as quantities, count() as count
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE: Note}
In order to use the above methods you need to add the following **static import** statement:

{CODE:java group_by_using@ClientApi\Session\Querying\HowToPerformGroupByQuery.java /}
{NOTE/}

{PANEL/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Indexes

- [Map-Reduce Indexes](../../../indexes/map-reduce-indexes)
