# The Search Pattern  

---

{NOTE: }

A graph query's search pattern follows the `match` keyword with **data node** and **edge** clauses, 
aliases that clarify query logic and enable filtering and projection, dedicated graph syntax, and 
integrated RQL commands.  

{INFO: }
Sample queries included in this article use only data that is available in the 
[Northwind sample database](../../../studio/database/tasks/create-sample-data#creating-sample-data), 
so you may easily try them out.  
{INFO/}

* In this page:  
  * [Data Nodes](../../../indexes/querying/graph/graph-queries-the-search-pattern#data-nodes)  
  * [Edges](../../../indexes/querying/graph/graph-queries-the-search-pattern#edges)  
     * [Edges Are Directional](../../../indexes/querying/graph/graph-queries-the-search-pattern#edges-are-directional)  
     * [Complex Edges](../../../indexes/querying/graph/graph-queries-the-search-pattern#complex-edges)  
  * [Aliases](../../../indexes/querying/graph/graph-queries-the-search-pattern#aliases)  
     * [What Are Aliases For?](../../../indexes/querying/graph/graph-queries-the-search-pattern#what-are-aliases-for)  
     * [Aliases Scope](../../../indexes/querying/graph/graph-queries-the-search-pattern#aliases-scope)  
   
{NOTE/}

---

{PANEL: Data Nodes}

{INFO: }
We use the term **data nodes** to distinguish these graph elements from 
RavenDB's distributed servers 
([cluster nodes](../../../glossary/cluster-node#glossary-ravendb-cluster-node)).  
{INFO/}

In a search pattern, a data-node clause is surrounded by parentheses:  
`(Orders)`  

The simplest pattern would include just a data-node clause, retrieving a collection's 
contents unconditionally.  
Here's a query that retrieves all documents of the `Orders` collection, just like 
the non-graph query [from orders](../../../indexes/querying/what-is-rql#from) would:  
`match (Orders)`  

Resulting in this pile of orders:  

![Simple Query: Graphical View](images/SearchPattern_001_SimplestQuery.png "Simple Query: Graphical View")

* A data node is a document.  
  Graph queries can locate nodes in 
  a [documents collection](../../../indexes/querying/graph/graph-queries-explicit-and-implicit#implicit-and-explicit-queries), 
  in the results of an [index query](../../../indexes/querying/graph/graph-queries-and-indexes#querying-indexes), 
  or in a [selected subset](../../../indexes/querying/graph/graph-queries-filtering#filtering) of either.  

* A data node clause can include RQL statements.  
  Here is the same query, `match (Orders)`, with an added RQL filter that leaves us with just one document:  
  {CODE-BLOCK:JSON}
  match 
     (Orders 
       where id() = 'Orders/1-A')  
  {CODE-BLOCK/}
  
    And its results:  
  
    ![Simple Query: Graphical View](images/SearchPattern_002_RestrictUsingRQL_GraphicalView.png "Simple Query: Graphical View")
  
    ![Simple Query: Textual View](images/SearchPattern_002_RestrictUsingRQL_TextualView.png "Simple Query: Textual View")

{PANEL/}

{PANEL: Edges}

A RavenDB edge is a simple string field (aka "edge specifier") that holds a document's ID.  
In your search pattern, an edge clause is surrounded by square brackets:  
`[ShipVia]`  

In the following query, we find **who very heavy orders are shipped by**.  

{CODE-BLOCK:JSON}
match 
   (Orders where Freight > 800) -  
   [ShipVia] ->  
   (Shippers)  
{CODE-BLOCK/}

* `(Orders where Freight > 800)`  
  This is the **origin data-node clause**.  
  In this case, origin data nodes are orders from the **Orders** collection whose freight exceeds 800.  
* `[ShipVia]`  
  This is the **edge clause**.  
  In this case, the edge specifier is a string field named **ShipVia**.  
  The query will open the ShipVia property of each origin node, pull the ID stored 
  in it, and look for a document with this ID in the destination data-node clause.  
* `(Shippers)`  
  This is the **destination data-node clause**.  
  In this case, destination data nodes are looked for in the **Shippers** collection.  

This query produces the following results.  

![ShipVia Edge](images/SearchPattern_003_ShipVia_Edge.png "ShipVia Edge")

**1.** This origin data node is **orders/293-A** of the **Orders** collection.  
**2.** This edge is **"ShipVia": "shippers/3-A"**, a string field of the origin node.  
**3.** This destination data node is **shippers/3-A** of the **Shippers** collection.  

---

#### Edges Are Directional.  
An edge always has [a direction](../../../indexes/querying/graph/graph-queries-overview#the-structure-of-a-basic-query).  

In the following query for example, each **origin node** (an order in the `Orders` collection), 
points at a **destination node** (the ordering company's profile, limited in this query to one specific company).  
{CODE-BLOCK:JSON}
match 
    (Orders)-
    [Company]->
    (Companies where Name = "LILA-Supermercado")
{CODE-BLOCK/}

* The **edge specifier** is `Company`, a string property of each origin node.  
  We use the **direction identifiers** `-`, `->` and `<-` to tell the query which way 
  the origin is and which way is the destination.  
* The hyphen `-` that connects `(Orders)` with the edge clause, identified (Orders) as the **origin data-node clause**.  
* The arrow `->` that connects `(Companies)` with the edge clause, identifies (Companies) as the **destination data-node clause**.  

These are this query's results:  

![Edges Are Directional](images/SearchPattern_004_DirectionalEdge.png "Edges Are Directional")

{NOTE: Queries can be directed both to the right and to the left.}
You can use `->` or `<-` to direct your query either to the right or to the left.  

Take these two queries for example:  
`(Orders)-[Company]->(Companies)`  
-and-  
`(Companies)<-[Company]-(Orders)`  

In both cases, `Orders` is the origin, `Company` the edge and `Companies` the destination.  
The syntax of both queries is valid, their functionality is equivalent, and their results similar.  
{NOTE/}

{INFO: Expanded Queries can be Bi-Directional.}
An [expanded query](../../../indexes/querying/graph/graph-queries-expanded-search-patterns#graph-queries-expanded-search-patterns) 
may include some sections directed to the right, and other sections directed to the left.  
This syntax may create 
[concise, effective queries](../../../indexes/querying/graph/graph-queries-expanded-search-patterns#bi-directional-queries). 
{INFO/}

---

#### Complex Edges  
A complex edge is a **nested JSON structure** whose properties include **edge specifiers**.  
A query can use a node's complex edge, to connect it to multiple other nodes.  

* To address a complex edge in a query, use this syntax:  
  `Lines[].Product`  
   * The nested JSON structure, `Lines` in this case, is identified using square brackets.  
   * The edge specifier, `Product` here, follows the dot delimiter.  

* Here is an implementation of a complex edge in a document:  

    ![Complex Edge in document orders/125-A](images/SearchPattern_005_ComplexEdge.png "Complex Edge in document orders/125-A")

* This query uses orders' `Lines` structure as a complex edge, to follow the multiple products handled by each order.  

    {CODE-BLOCK:JSON}
match 
    (Orders as orders)- 
    [Lines[].Product as line]-> 
    (Products as products)
{CODE-BLOCK/}  

    With these results:  
        
     ![Retrieved Using Complex Edges](images/SearchPattern_006_Lines.png "Retrieved Using Complex Edges")

## Aliases

To give an element an alias, use the `as` keyword.  

| Document | Query |
|:-------------:|:-------------:|
| {CODE-BLOCK:JSON}
match 
    (Orders)- 
    [Product as product]-> 
    (Products)
{CODE-BLOCK/} | ![Aliases](images/SearchPattern_007_Alias.png "Aliases") |  

* **Use unique aliases.**  
  Each alias has to be unique.  

* **Use `_` to exclude an element from the textual results.**  
  If you want to prevent RavenDB from including an element in the textual graph results, 
  give it `_` for an alias.  
  E.g. `match (Orders as _)`  
  `_`, `__`, `___` and so on have the same functionality, so you can exclude multiple 
  elements from the results and keep each alias unique.  

* **Explicit syntax always includes an alias.**  
  When you declare a data element using the 
  [explicit syntax](../../../indexes/querying/graph/graph-queries-explicit-and-implicit#explicitly-declaring-data-elements), 
  providing an alias is always required.  
  `with {from Orders} as orders`

---

#### What Are Aliases For?  
Aliases have a few important roles in graph queries.  

* **Readability**  
  Most commonly, aliases are used to improve query readability and clarify 
  each element's role in it.  

* **Abstraction**  
    It is sometimes useful to instate the same data element, e.g. the `Employees` collection, 
    in multiple parts of a query.  
    To do this, we need to give the reoccuring element a **unique alias** in each query part it participates.  

    The following query for example finds who Sales Representatives report to.  
    {CODE-BLOCK:JSON}
    match 
      (Employees as employed 
         where Title = "Sales Representative") - 
      [ReportsTo as reportsTo]-> 
      (Employees as incharge)
    {CODE-BLOCK/}

    The `Employees` collection is instated twice: as `employed` in the origin node clause, 
    and as `incharge` in the destination node clause.  
    During execution, the abstracting aliases are used to retrieve and connect profiles of 
    `employed` sales representatives and the employees `incharge` of them, and looking for 
    both in the same collection poses no problem.  
    
    ![Employees](images/SearchPattern_008_Abstraction.png "Employees")
    
    {INFO: }
    These aliases are operational, not just informative: they relay to the graph interpreter 
    how it should handle this query.  
    Reinstating an element **without** providing a unique alias for each of its occurrences, 
    may produce unreliable results.  
    {INFO/}

* **Projection**  
  To [project](../../../indexes/querying/graph/graph-queries-overview#projecting-graph-results) 
  details of a query's results, we follow the query with a `SELECT`clause.  
  Within this clause, we refer to the query's data elements **by their aliases**. No aliases, no projection.  

---

#### Aliases Scope  

When you set an alias, be aware of its scope.  

* In the following explicit declaration of a data-node, `pd` is 
  an in-clause alias that **cannot** be used outside its scope.  
  `with {from Products as pd where pd.PricePerUnit > 18} as PricyProduct`
  
* `PricyProduct` on the other hand is set for the whole clause and 
  **can** be approached and selected for projection.  
  {CODE-BLOCK:JSON}
with {from Products as pd where pd.PricePerUnit > 18} as PricyProduct

match 
   (Orders as LondonOrders 
      where ShipTo.City = 'London') -
   [Lines[].Product] -> 
   (PricyProduct)

select
   PricyProduct as Pricy
  {CODE-BLOCK/}  

{PANEL/}  

## Related Articles

**Querying**:  
[RQL](../../../indexes/querying/what-is-rql#querying-rql---raven-query-language)  
[Indexes](../../../indexes/what-are-indexes#what-indexes-are)  

**Graph Querying**  
[Expanded Search Patterns](../../../indexes/querying/graph/graph-queries-expanded-search-patterns#graph-queries-expanded-search-patterns)  
[Explicit and Implicit Syntax](../../../indexes/querying/graph/graph-queries-explicit-and-implicit#explicit-and-implicit-syntax)  
[Graph Queries and Indexes](../../../indexes/querying/graph/graph-queries-and-indexes#graph-queries-and-indexes)  
[Filtering](../../../indexes/querying/graph/graph-queries-filtering#graph-queries-filtering)  
[Multi-Section Search Patterns](../../../indexes/querying/graph/graph-queries-multi-section#graph-queries-multi-section-search-patterns)  
[Recursive Graph Queries](../../../indexes/querying/graph/graph-queries-recursive#recursive-graph-queries)  
