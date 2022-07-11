# Querying: Distinct
---

{NOTE: }

The `distinct()` method allows you to remove duplicates from the result. Items are compared based on the fields listed in the `select` section of the query. 

* In this page:
   * [Sample Query with Distinct Method](../../indexes/querying/distinct#sample-query-with-the-distinct-method)
   * [Paging with the Distinct Method](../../indexes/querying/distinct#paging-with-the-distinct-method)
   * [Counting](../../indexes/querying/distinct#counting)
   * [Performance Cost and an Alternative Approach](../../indexes/querying/distinct#performance-cost-and-an-alternative-approach)

{NOTE/}

---

### Sample Query with the Distinct Method

{CODE-TABS}
{CODE-TAB:nodejs:Node.js distinct_1_1@indexes\querying\distinct.js /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
select distinct ShipTo.Country 
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Paging with the Distinct Keyword

A special approach must be used when calling the `distinct()` method while paging.  
Please read the dedicated article about [paging through tampered results](../../indexes/querying/paging#paging-through-tampered-results).  

## Counting

RavenDB supports returning counts when the distinct operation is used.

{CODE:nodejs distinct_2_1@indexes\querying\distinct.js /}

{INFO: }

### Performance Cost and an Alternative Approach

Please keep in mind that this operation might not be efficient for large sets of data due to the need to scan all of the index results in order to find all the unique values.

The same result might be achieved by creating a [Map-Reduce](../../indexes/map-reduce-indexes) index that aggregates data by the field where you want a distinct value. 
[Indexes](../../indexes/creating-and-deploying) need to process entire datasets just once, after which they only process any new data. 
Queries process all of the data assigned to them each time they are activated.

Learn how to use the alternative approach efficiently in the article [Implementing a count(distinct) query in RavenDB](https://ravendb.net/articles/implementing-a-countdistinct-query-in-ravendb).

#### Map-Reduce Index Sample:

Index definition:

{CODE:nodejs distinct_3_1@indexes\querying\distinct.js /}

Query the index:

{CODE:nodejs distinct_3_2@indexes\querying\distinct.js /}

{INFO/}

## Related Articles

### Querying

- [Paging](../../indexes/querying/paging)

### Indexing

- [Map-Reduce Indexes](../../indexes/map-reduce-indexes)

---

### Code Walkthrough

- [Map-Reduce Index](https://demo.ravendb.net/demos/csharp/static-indexes/map-reduce-index)
- [Paging Query Results](https://demo.ravendb.net/demos/csharp/queries/paging-query-results)
