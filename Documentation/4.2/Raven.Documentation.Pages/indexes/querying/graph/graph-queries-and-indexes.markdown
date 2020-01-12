# Graph Queries And Indexes  

---

{NOTE: }

* You can **query indexes** using graph queries.  
* Graph queries use **various indexing methods**, depending on their characteristics.  
* You can create **Map Reduce** definitions that would lift some of the workload from the graph engine.  
* In this page:  
   * [Querying Indexes](../../../indexes/querying/graph/graph-queries-and-indexes#querying-indexes)  
   * [How Are Graph Queries Indexes](../../../indexes/querying/graph/graph-queries-and-indexes#how-are-graph-queries-indexed)  
   * [Graph Queries and Map Reduce](../../../indexes/querying/graph/graph-queries-and-indexes#graph-queries-and-map-reduce)  

{NOTE/}

---

{PANEL: Graph queries and Indexes}

####Querying Indexes

To query an index, define it as a **data node** using the 
[explicit syntax](../../../indexes/querying/graph/graph-queries-explicit-and-implicit#explicitly-declaring-data-elements).  
Use `from index` rather than the ordinary "from", with the index name as a parameter:  
`with {from index 'Orders/ByCompany'} as orders`  
  
The following query for example uses the `Orders/ByCompany` 
index while searching for big orders made by German companies.
{CODE-BLOCK:JSON}
with {from index 'Orders/ByCompany' where Count > 10} as bigOrders

match
    (bigOrders) -
    [Company as company] ->
    (Companies as companies where Address.Country = "Germany")
{CODE-BLOCK/}

---

####How Are Graph Queries Indexed

Any graph query over the bare minimum is interpreted to several types of queries, each with its 
own measure of indexing and resource usage. Complex queries may combine methods mentioned here.  

* **No Indexing**  
  This query creates **no index**, since retrieving a whole collection requires no searching.  
  {CODE-BLOCK:JSON}
match 
    (Orders as orders) 
  {CODE-BLOCK/}

* **Auto Indexing**  
  The node clause shown here will trigger RavenDB to create an auto index for queried orders, 
  as done with non-graph queries.  
  {CODE-BLOCK:JSON}
match 
    (Orders as orders 
        where ShipTo.Region = "Nueva Esparta")
  {CODE-BLOCK/}

* **Handled by the graph queries engine**  
  Queries with edges are handled by the graph queries engine before handing them to clients, 
  to fathom the relations between data nodes.  
  {CODE-BLOCK:JSON}
match 
    (Orders as orders)-
    [ShipVia as shipvia]->
    (Shippers as shippers)
  {CODE-BLOCK/}

---

####Graph Queries and Map Reduce

It is sometimes "cheaper" to transfer work load from the graph engine to Map Reduce 
for the "heavy lifting", and let the graph engine handle just the final results.  
To do this, create static [Map Reduce](../../../studio/database/indexes/create-map-reduce-index) 
definitions for datasets you intend to include in edge clauses.  

{PANEL/}

## Related Articles

**Querying**:  
[RQL](../../../indexes/querying/what-is-rql#querying-rql---raven-query-language)  
[Indexes](../../../indexes/what-are-indexes#what-indexes-are)  

**Graph Querying**  
[Overview](../../../indexes/querying/graph/graph-queries-overview#graph-querying-overview)  
[The Search Pattern](../../../indexes/querying/graph/graph-queries-the-search-pattern#the-search-pattern)  
[Expanded Search Patterns](../../../indexes/querying/graph/graph-queries-expanded-search-patterns#graph-queries-expanded-search-patterns)  
[Explicit and Implicit Syntax](../../../indexes/querying/graph/graph-queries-explicit-and-implicit#explicit-and-implicit-syntax)  
[Filtering](../../../indexes/querying/graph/graph-queries-filtering#graph-queries-filtering)  
[Multi-Section Search Patterns](../../../indexes/querying/graph/graph-queries-multi-section#graph-queries-multi-section-search-patterns)  
[Recursive Graph Queries](../../../indexes/querying/graph/graph-queries-recursive#recursive-graph-queries)  
