# Querying: Distinct
---

{NOTE: }

The `distinct()` method allows you to remove duplicates from the result.  
Items are compared based on the fields listed in the `select` section of the query. 

* In this page:
   * [Sample Query with Distinct](../../indexes/querying/distinct#sample-query-with-distinct)
   * [Paging with Distinct](../../indexes/querying/distinct#paging-with-distinct)
   * [Count with Distinct](../../indexes/querying/distinct#count-with-distinct)
      * [Performance Cost and an Alternative Approach](../../indexes/querying/distinct#performance-cost-and-an-alternative-approach)

{NOTE/}

---

### Sample Query with Distinct

{CODE-TABS}
{CODE-TAB:nodejs:Node.js distinct_1_1@indexes\querying\distinct.js /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
select distinct ShipTo.Country 
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Paging with Distinct

A special approach must be used when calling `distinct()` while paging.  
Please read the dedicated article about [paging through tampered results](../../indexes/querying/paging#paging-through-tampered-results).  

## Count with Distinct

Use `count()` in combination with `distinct()` to get the number of unique items.

{CODE:nodejs distinct_2_1@indexes\querying\distinct.js /}

## Performance Cost and an Alternative Approach

Please keep in mind that using `count()` with `distinct()` might not be efficient for large sets of data due to the need to scan all of the index results in order to find all the unique values.

* Getting the distinct items' count can also be achieved by creating a [Map-Reduce](../../indexes/map-reduce-indexes) index 
  that will aggregate data by the field for which distinct count results are needed.
* This is more efficient since computations are done during indexing time and not at query time.  
  The entire dataset is [Indexed](../../indexes/creating-and-deploying) 
  once, whereafter the aggregated value is always kept up to date as indexing will occur only for new/modified data.  
  Queries, on the other hand, process all of the data assigned to them each time they are activated.

### Map-Reduce Index Sample:

Index definition:

{CODE:nodejs distinct_3_1@indexes\querying\distinct.js /}

Query the index:

{CODE:nodejs distinct_3_2@indexes\querying\distinct.js /}

### Combining Faceted Queries with Map-Reduce

Faceted queries can be used together with a map-reduce index as another alternative approach.  
See a C# example for [Implementing a count(distinct) query in RavenDB](https://ravendb.net/articles/implementing-a-countdistinct-query-in-ravendb).


## Related Articles

### Querying

- [Paging](../../indexes/querying/paging)

### Indexing

- [Map-Reduce Indexes](../../indexes/map-reduce-indexes)

---

### Code Walkthrough

- [Map-Reduce Index](https://demo.ravendb.net/demos/csharp/static-indexes/map-reduce-index)
- [Paging Query Results](https://demo.ravendb.net/demos/csharp/queries/paging-query-results)
