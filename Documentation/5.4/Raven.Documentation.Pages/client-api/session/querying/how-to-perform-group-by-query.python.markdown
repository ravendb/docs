# Perform Dynamic Group By Query

---

{NOTE: }

* RavenDB's query optimizer supports dynamic grouping by query and automatically creates 
  auto map-reduce indexes.  

* To run a dynamic query that aggregates data use the `group_by()` method.  

* Data can be grouped by a single or by multiple fields and further aggregated by sum, type, or count.  

* In This Page:  
   * [Group By a Single Field](../../../client-api/session/querying/how-to-perform-group-by-query#group-by-a-single-field)  
   * [Group By Multiple Fields](../../../client-api/session/querying/how-to-perform-group-by-query#group-by-multiple-fields)  
   * [Select Composite GroupBy Key](../../../client-api/session/querying/how-to-perform-group-by-query#select-composite-groupby-key)  
   * [Group By Array](../../../client-api/session/querying/how-to-perform-group-by-query#group-by-array)  
      * [By Array Values](../../../client-api/session/querying/how-to-perform-group-by-query#by-array-values)  
      * [By Array Content](../../../client-api/session/querying/how-to-perform-group-by-query#by-array-content)  
   * [Sorting](../../../client-api/session/querying/how-to-perform-group-by-query#sorting)  
      * [By Count](../../../client-api/session/querying/how-to-perform-group-by-query#by-count)  
      * [By Sum](../../../client-api/session/querying/how-to-perform-group-by-query#by-sum)  

{NOTE/}

---

{PANEL: Group By a Single Field}

{CODE-TABS}
{CODE-TAB:python group_by_1@ClientApi\Session\Querying\HowToPerformGroupByQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
group by ShipTo.City
select ShipTo.City as Country, sum(Lines[].Quantity) as TotalQuantity
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Group By Multiple Fields}

{CODE-TABS}
{CODE-TAB:python group_by_2@ClientApi\Session\Querying\HowToPerformGroupByQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
group by Employee, Company
select Employee as EmployeeIdentifier, Company, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Select Composite GroupBy Key}

{CODE-TABS}
{CODE-TAB:python group_by_3@ClientApi\Session\Querying\HowToPerformGroupByQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by Employee, Company
select key() as EmployeeCompanyPair, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Group By Array}

### By Array Values

The following query will group by the `lines[]` array `product` property 
and calculate the count per product.  
Behind the scenes, an auto map-reduce index is created to handle the query.  
{CODE-TABS}
{CODE-TAB:python group_by_4@ClientApi\Session\Querying\HowToPerformGroupByQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by Lines[].Product
select Lines[].Product, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

It is possible to group by the values of an array field **and** by the value of an additional property.  
{CODE-TABS}
{CODE-TAB:python group_by_5@ClientApi\Session\Querying\HowToPerformGroupByQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by Lines[].Product, ShipTo.Country 
select Lines[].Product as Product, ShipTo.Country as Country, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

Grouping by values of multiple fields of **the same** array is supported as well.  
{CODE-TABS}
{CODE-TAB:python group_by_6@ClientApi\Session\Querying\HowToPerformGroupByQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by Lines[].Product, Lines[].Quantity 
select Lines[].Product as Product, Lines[].Quantity as Quantity, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### By Array Content

Another option is to group by array **content**.  
The creation of the reduction key will be based on the content of the array field specified by `group_by`.  
{CODE-TABS}
{CODE-TAB:python group_by_7@ClientApi\Session\Querying\HowToPerformGroupByQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
group by array(Lines[].Product)
select key() as Products, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

It is possible to group by the content of an array field **and** by that of a field of an additional property.  
{CODE-TABS}
{CODE-TAB:python group_by_8@ClientApi\Session\Querying\HowToPerformGroupByQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by array(Lines[].Product), ShipTo.Country 
select Lines[].Product as Products, ShipTo.Country as Country, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

Grouping by multiple fields of **the same** array is also supported.  
{CODE-TABS}
{CODE-TAB:python group_by_9@ClientApi\Session\Querying\HowToPerformGroupByQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by array(Lines[].Product), array(Lines[].Quantity) 
select Lines[].Product as Products, Lines[].Quantity as Quantities, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Sorting}

The results of a dynamic group_by query can be sorted by an aggregation function used in the query.  
The results can be ordered by the aggregation operations `count` and `sum`.  

#### By Count

{CODE-TABS}
{CODE-TAB:python order_by_count@ClientApi\Session\Querying\HowToPerformGroupByQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by Employee 
order by count() as long 
select Employee, count()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

#### By Sum

{CODE-TABS}
{CODE-TAB:python order_by_sum@ClientApi\Session\Querying\HowToPerformGroupByQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
group by Employee 
order by sum(Freight) as double 
select key() as Employee, sum(Freight) as Sum
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Indexes

- [Map-Reduce Indexes](../../../indexes/map-reduce-indexes)
