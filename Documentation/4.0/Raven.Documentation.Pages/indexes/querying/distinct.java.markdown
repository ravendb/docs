# Query for distinct results
---

{NOTE: }

* The `distinct` method allows you to remove duplicates from query results.  
  Items are compared based on the fields listed in the `select` section of the query. 

* In this page:
   * [Sample query with distinct](../../indexes/querying/distinct#sample-query-with-distinct)
   * [Paging with distinct](../../indexes/querying/distinct#paging-with-distinct)
   * [Count with distinct](../../indexes/querying/distinct#count-with-distinct)
      * [Performance cost and an alternative approach](../../indexes/querying/distinct#performance-cost-and-an-alternative-approach)

{NOTE/}

---

{PANEL: Sample query with Distinct}

{CODE-TABS}
{CODE-TAB:java:Query distinct_1_1@Indexes\Querying\Distinct.java /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
order by ShipTo.Country
select distinct ShipTo.Country
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/} 

{PANEL:  Paging with distinct}

A special approach must be used when calling `distinct()` while paging.  
Please read the dedicated article about [paging through tampered results](../../indexes/querying/paging#paging-through-tampered-results).  

{PANEL/}

{PANEL: Count with distinct}

Use `count()` in combination with `distinct()` to get the number of unique items.  
Similar to _toList()_, _count()_ triggers query execution on the server-side.

{CODE-TABS}
{CODE-TAB:java:Query distinct_2_1@Indexes\Querying\Distinct.java /}
{CODE-TAB-BLOCK:sql:RQL}
// This RQL is intended for use when issuing a Raw Query from the client API.
// Running directly from the Studio will not display the number of results.
// ========================================================================

from "Orders"
select distinct ShipTo.Country
limit 0, 0
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Performance cost and an alternative approach

* Please keep in mind that using `count()` with `distinct()` might not be efficient for large sets of data  
  as it requires scanning all index results to find unique values.

* Getting the distinct items' count can also be achieved by creating a [Map-Reduce](../../indexes/map-reduce-indexes) index 
  that will aggregate data by the field for which distinct count results are needed.

* Using a Map-Reduce index is more efficient since computations are done during indexing time and not at query time. 
  The entire dataset is [indexed](../../indexes/creating-and-deploying) once,
  whereafter the aggregated value is always kept up to date as indexing will occur only for new/modified data.

#### Map-Reduce index example:

Index definition:

{CODE:java distinct_3_1@Indexes\Querying\Distinct.java /}

Query the index:

{CODE-TABS}
{CODE-TAB:java:Query distinct_3_2@Indexes\Querying\Distinct.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Orders/ByShipToCountry"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Combining faceted queries with Map-Reduce:

Faceted queries can be used together with a map-reduce index as another alternative approach.  
See a C# example for [Implementing a count(distinct) query in RavenDB](https://ravendb.net/articles/implementing-a-countdistinct-query-in-ravendb).

{PANEL/} 

## Related Articles

### Querying

- [Paging](../../indexes/querying/paging)

### Indexing

- [Map-Reduce Indexes](../../indexes/map-reduce-indexes)

---

### Code Walkthrough

- [Map-Reduce Index](https://demo.ravendb.net/demos/csharp/static-indexes/map-reduce-index)
- [Paging Query Results](https://demo.ravendb.net/demos/csharp/queries/paging-query-results)
