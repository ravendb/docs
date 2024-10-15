# Perform Dynamic Group By Query

Since RavenDB 4.0, the query optimizer supports dynamic group by queries and automatically creates auto map-reduce indexes.

You can create a dynamic query that does an aggregation using the `groupBy` method.

Supported aggregation operations include:

- `selectKey`  
- `selectSum`  
- `selectCount`  

{PANEL: Group By Single Field}

{CODE-TABS}
{CODE-TAB:php group_by_1@ClientApi\Session\Querying\HowToPerformGroupByQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
group by ShipTo.City
select ShipTo.City as Country, sum(Lines[].Quantity) as TotalQuantity
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Group By Multiple Fields}

{CODE-TABS}
{CODE-TAB:php group_by_2@ClientApi\Session\Querying\HowToPerformGroupByQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
group by Employee, Company
select Employee as EmployeeIdentifier, Company, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Select Composite GroupBy Key}

{CODE-TABS}
{CODE-TAB:php group_by_3@ClientApi\Session\Querying\HowToPerformGroupByQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by Employee, Company
select key() as EmployeeCompanyPair, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Group By Array}

### By Array Values

The following query groups by the `Product` property of the `Lines` collection, 
and calculates the count per ordered products.  
Underneath a fanout, an auto map-reduce index will be created to handle such a query. 

{CODE-TABS}
{CODE-TAB:php group_by_4@ClientApi\Session\Querying\HowToPerformGroupByQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by Lines[].Product
select Lines[].Product, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

Inside a single group by statement you can mix collection values and value of another property.  

{CODE-TABS}
{CODE-TAB:php group_by_5@ClientApi\Session\Querying\HowToPerformGroupByQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by Lines[].Product, ShipTo.Country 
select Lines[].Product as Product, ShipTo.Country as Country, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

Grouping by multiple values from **the same** collection is supported as well:

{CODE-TABS}
{CODE-TAB:php group_by_6@ClientApi\Session\Querying\HowToPerformGroupByQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by Lines[].Product, Lines[].Quantity 
select Lines[].Product as Product, Lines[].Quantity as Quantity, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### By Array Content

Another option is to group by array content.  
The reduction key will be calculated based on all values of a collection specified in `groupBy`.

{CODE-TABS}
{CODE-TAB:php group_by_7@ClientApi\Session\Querying\HowToPerformGroupByQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
group by array(Lines[].Product)
select key() as Products, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

Grouping by array content and a value of another property is supported by `documentQuery`:

{CODE-TABS}
{CODE-TAB:php group_by_8@ClientApi\Session\Querying\HowToPerformGroupByQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by array(Lines[].Product), ShipTo.Country 
select Lines[].Product as Products, ShipTo.Country as Country, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

Grouping by multiple values from **the same** collection is also supported by `documentQuery`:

{CODE-TABS}
{CODE-TAB:php group_by_9@ClientApi\Session\Querying\HowToPerformGroupByQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by array(Lines[].Product), array(Lines[].Quantity) 
select Lines[].Product as Products, Lines[].Quantity as Quantities, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Indexes

- [Map-Reduce Indexes](../../../indexes/map-reduce-indexes)
