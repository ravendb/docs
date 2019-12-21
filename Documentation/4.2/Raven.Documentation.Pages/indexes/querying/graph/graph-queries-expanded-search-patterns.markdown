# Graph Queries Expanded Search Patterns  

---

{NOTE: }

Graph queries are modular structures that can be easily expanded from data-node to 
edge and from edge to node in order to reveal potential layers of meaningful data.  
This article focuses on simple patterns, including **directional** and **bi-directional** queries.  
Other types of patterns include 
[multi-section](../../../indexes/querying/graph/graph-queries-multi-section) 
and [recursive](../../../indexes/querying/graph/graph-queries-recursive) 
queries.  

* In this page:  
   * [Expanding the Search Pattern](../../../indexes/querying/graph/graph-queries-expanded-search-patterns#expanding-the-search-pattern)  
   * [Bi-Directional Queries](../../../indexes/querying/graph/graph-queries-expanded-search-patterns#bi-directional-queries)  
   
{NOTE/}

---

{PANEL: Expanding The Search Pattern}

You can expand an existing graph query simply by adding it an edge-and-node pair, as follows.  

* **Initial query**:  
  Here we find all the orders made by a company we want to investigate, and all the products that each order includes.  
  {CODE-BLOCK:JSON}
match
    (Orders as orders 
       where Company = "companies/34-A")-
    [Lines[].Product as product]->
    (Products as products)
{CODE-BLOCK/}

    ![Initial Query Results](images/Overview_ExpandingTheQuery_1.png "Initial Query Results")

* **Expanded query**:  
  And here we expand the query to also find each product's supplier.  
{CODE-BLOCK:JSON}
match
    (Orders as orders 
       where Company = "companies/34-A")-
    [Lines[].Product as product]->
    (Products as products)-
    [Supplier as supplier]->
    (Suppliers as suppliers)
{CODE-BLOCK/}

    ![Expanded Query Results](images/Overview_ExpandingTheQuery_2.png "Expanded Query Results")

    The expanded search pattern locates a new layer of relations and data nodes, by querying retrieved **products** 
    and finding which **suppliers** they lead to.  

{PANEL/}

{PANEL: Bi-Directional Queries}
You can direct a graph query to the left and/or to the right.  

* **Directional Queries**  
  In each section of the query, an origin data-node clause is directed to a destination 
  data-node clause to its right **or** to its left.  
  The following two queries for example, searching for orders to France, are equivalent.  

    | To The Right | To The Left |
    | --- | --- |
    | {CODE-BLOCK:JSON}
match
     (Orders as orders 
        where ShipTo.Country = "France")-
     [Company as company]->
     (Companies as companies)
  {CODE-BLOCK/} | {CODE-BLOCK:JSON}
match 
     (Companies as companies)
     <-[Company as company]
     -(Orders as orders 
         where ShipTo.Country = "France") 
  {CODE-BLOCK/}|

* **Bi-Directional Queries**  
  A query may be combined of sections that point to the right **and** to the left.  
  Here is one:
  {CODE-BLOCK:JSON}
match 
     (Employees as employees)
     <-[Employee as handledBy]
     -(Orders as orders)-
     [Company as orderedBy]->
     (Companies as companies)
  {CODE-BLOCK/}  

   * The first part **heads left**, finding **orders and the employees that handle them**.  
     {CODE-BLOCK:JSON}
     (Employees as employees)
     <-[Employee as handledBy]
     -(Orders as orders)
     {CODE-BLOCK/}  
   * The second part **heads right**, finding **orders and the companies that make these orders**.  
     {CODE-BLOCK:JSON}
     (Orders as orders)-
     [Company as orderedBy]->
     (Companies as companies)
     {CODE-BLOCK/}  
   * **Orders**, common to both parts, appears only once.  
     It is origin for both sections.  

    The query produces these results:  

    ![Bi-Directional Query Results](images/Overview_BiDirectionalQuery.png "Bi-Directional Query Results")

* It is equivalent to the following less concise multi-segment query.  
  
    | Multi-Segment (Intersection) | Bi-Directional |
    | --- | --- |
    | {CODE-BLOCK:JSON}
match 
     //First Segment
     (Orders as orders)- 
       [Employee as employee]-> 
         (Employees as employees)
    and
     //Second Segment
     (orders)- 
       [Company as company]-> 
         (Companies as companies)
  {CODE-BLOCK/} | {CODE-BLOCK:JSON} match 
     (Employees as employees)
     <-[Employee as handledBy]
     -(Orders as orders)-
     [Company as orderedBy]->
     (Companies as companies)
     {CODE-BLOCK/}|

   {INFO: }
   Formatting a query as a bi-directional or a multi-segment structure 
   is a **readability preference**.  
   Operation-wise the two formats are equivalent, since **bi-directional** queries 
   are interpreted to **single-direction multi-segment queries** before execution.  
   {INFO/}

{PANEL/}

## Related Articles

**Querying**:  
[RQL](../../../indexes/querying/what-is-rql#querying-rql---raven-query-language)  
[Indexes](../../../indexes/what-are-indexes#what-indexes-are)  

**Graph Querying**  
[Overview](../../../indexes/querying/graph/graph-queries-overview#graph-querying-overview)  
[The Search Pattern](../../../indexes/querying/graph/graph-queries-the-search-pattern#the-search-pattern)  
[Explicit and Implicit Syntax](../../../indexes/querying/graph/graph-queries-explicit-and-implicit#explicit-and-implicit-syntax)  
[Graph Queries and Indexes](../../../indexes/querying/graph/graph-queries-and-indexes#graph-queries-and-indexes)  
[Filtering](../../../indexes/querying/graph/graph-queries-filtering#graph-queries-filtering)  
[Multi-Section Search Patterns](../../../indexes/querying/graph/graph-queries-multi-section#graph-queries-multi-section-search-patterns)  
[Recursive Graph Queries](../../../indexes/querying/graph/graph-queries-recursive#recursive-graph-queries)  
