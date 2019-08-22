# Basic Graph Queries  

---

{NOTE: }

* While preparing graph queries, you can define elements explicitly to make the queries 
  more readable, or let the server define them for you.  
* You can minimize network usage by projecting a subset of the query results.  

* In this page:  
   * [Explicit and Implicit queries](../../../indexes/querying/graph/graph-queries-basic#explicit-and-implicit-queries)  
   * [Projection](../../../indexes/querying/graph/graph-queries-basic#projection)  
   * [Operators: OR and AND](../../../indexes/querying/graph/graph-queries-basic#operators:-or-and-and)  
   * [Path graph query](../../../indexes/querying/graph/graph-queries-basic#path-graph-query)  
   * [Graph queries and Indexes](../../../indexes/querying/graph/graph-queries-basic#graph-queries-and-indexes)  
   * [Graph Queries Flow](../../../indexes/querying/graph/graph-queries-basic#graph-queries-flow)  
   * [Narrowing Down Results](../../../indexes/querying/graph/graph-queries-basic#narrowing-down-results)  
      * [Filtering results in an edge clause](../../../indexes/querying/graph/graph-queries-basic#filtering-results-in-an-edge-clause)  
      * [Filtering results in data-node clauses](../../../indexes/querying/graph/graph-queries-basic#filtering-results-in-data-node-clauses)  

{NOTE/}

---

{PANEL: Explicit and Implicit queries}  

Every data node or edge that is to participate in a query is chosen using a `with` clause.  
After choosing all data elements this way, a `match` statement uses them to define the search pattern.  
Only data that matches this pattern is retrieved.  

It is allowed to omit the `with` clauses from your code. RavenDB will notice unchosen data 
elements in your `match` pattern and choose them for you with nearly no performance toll.  

* **Not including `with` clauses** in your queries improves code conciseness 
  and automates a procedure that doesn't need your attention.  
* **Defining `with` clauses yourself** may create more readable code.  

---

####Implicitly defining nodes and edges  

To use an implicit query:  

* Use a `match` clause to define and run a search pattern.  

Here are two simple examples of implicit queries that search a tiny three-documents collection.  

* `match (Houses as houses)`  
  Use this search pattern to retrieve all documents of the "Houses" collection.  
   * Graphical results view  
     ![Retrieve All - Graphical view](images/BasicQuery_GraphicalView1.png)  
   * Textual results view  
     ![Retrieve All - Textual view](images/BasicQuery_TextualView1.png)  

* `match (Houses as forSale where Status = "ForSale")`  
  Retrieve from the Houses collection only documents whose "Status" field contains "ForSale".  
  The Houses collection currently holds only one document that matches this condition:  
  {CODE-BLOCK:JSON}
  Document: Houses/Malfoy_Manor
  {
    "House": "Malfoy Manor",
    "Owner": "BeepAgency",
    "Status": "ForSale",
    "@metadata": {
        "@collection": "Houses"
    }
  }
  {CODE-BLOCK/}
  So results would be:  
   * Graphical results view  
     ![Retrieve Selected Documents - Graphical view](images/BasicQuery_GraphicalView2.png)  
   * Textual results view  
     ![Retrieve Selected Documents - Textual view](images/BasicQuery_TextualView2.png)  

---

####Explicitly defining data elements  

* To define an explicit query:  
   * Choose data elements that would participate your query.
      * Choose each Data Node for your query using `from` within a `with` clause.  
        E.g. `with {from Houses} as houses`  
        **Houses** is a collection chosen to function as a data node.  
        `as houses` is an alias for the data node. Define it outside the `with` clause.  
      * Choose each Edge using a `with edges` clause.  
        `with edges (Owner) as ownedBy`  
        **Owner** is the edge. It is the name of a text field that contains a reference to a document.  
   * Providing your data elements with aliases is **mandatory** in explicit queries.  
   * Use your aliases in a `match` statement to define and run the search pattern.  

* Here are samples for explicit queries and their implicit equivalents -  

    |Explicit|Implicit|
    |---------------|---------------|
    |{CODE-BLOCK:JSON}
with {from Houses} as houses  
match (houses)
    {CODE-BLOCK/}|{CODE-BLOCK:JSON} match (Houses as houses) {CODE-BLOCK/}|
    |{CODE-BLOCK:JSON}
//Use WHERE  
with {from Houses where Status = "ForSale"} as forSale  
match (forSale)
    {CODE-BLOCK/}|{CODE-BLOCK:JSON} match (Houses as forSale where Status = "ForSale") {CODE-BLOCK/}|
    |{CODE-BLOCK:JSON}
with {from Houses} as houses
//Set an Edge
with edges (Owner) as ownedBy
with {from Owners} as owner
match(houses)-[ownedBy]->(owner)
    {CODE-BLOCK/}|{CODE-BLOCK:JSON}
    match ((Houses as houses)-[Owner as ownedBy]->(Owners as owners))
    {CODE-BLOCK/}|
    
{PANEL/}

---

{PANEL: Projection}

As with non-graph queries, you can [project](../../../indexes/querying/projections#what-are-projections-and-when-to-use-them) 
a subset of graph query results using the `select` keyword to put results in order and minimize server data transfers.  

* This projection for example - 
  {CODE-BLOCK:JSON}
  with {from Houses} as houses
match(houses)
select houses.Owner
  {CODE-BLOCK/}
* With these results -  
  ![Textual View](images/BasicQuery_ProjectionResults.png)  

{NOTE: }
You can use `select` in an edge clause, but not in a data-node clause.  

* You can do this -  
  `match (Orders as orders)-[Lines as cheap where Discount >= 0.25 select Product]->(Products as products)`  
  And this -  
  `match (Orders as orders where Freight > 5)`  
  But not this -  
  `match (Orders as orders where Freight > 5 select Order)`  
{NOTE/}

{PANEL/}

---

{PANEL: Operators: OR and AND}  

You can use the `or` and `and` operators to condition your graph queries, as you would in a regular RQL expression.  

* **Example 1**: Use `and` and `or` in a query.  
You can create this query on your own to examine it if you like, its data is included in RavenDB's [sample data](../../../studio/database/tasks/create-sample-data).  

    {CODE-TABS}
    {CODE-TAB-BLOCK:sql:Implicit} 
    match
(Orders as highFreightOrders where Freight > 700)-[Company as company]->(Companies as companies)
or
(Orders as pricyOrders where (Lines[].PricePerUnit > 250) and (Lines[].Quantity > 60))-[Employee as employee]->(Employees as employees)
    {CODE-TAB-BLOCK/}
    {CODE-TAB-BLOCK:sql:Explicit}
    with {from Orders where (Lines[].PricePerUnit > 250) and (Lines[].Quantity > 60)} as pricyOrders
with edges (Employee) as employee
with {from Employees} as employees

with {from Orders where Freight > 700} as highFreightOrders
with edges (Company) as company
with {from Companies} as companies

match
(highFreightOrders)-[company]->(companies)
or
(pricyOrders)-[employee]->(employees)
    {CODE-TAB-BLOCK/}
    {CODE-TABS/}

* **Example 2**: Use `or` to retrieve the profiles of occupied houses **or** of ones up for sale.  
  {CODE-BLOCK:JSON}
  match 
(Houses as forSale where Status = "ForSale")
or
((Houses as houses)-[Owner as ownedBy]->(Owners as owners))
  {CODE-BLOCK/}
   * Graphical results view:  
     ![Graphical View](images/BasicQuery_UsingOr_1_GraphicalView.png)  
   * Textual results view:  
     ![Textual View](images/BasicQuery_UsingOr_1_TextualView.png)  

* **Example 3**: Use `or` to reveal more relations between nodes:  
  {CODE-BLOCK:JSON}
match  
(Organisms as eater)-[Hunts as hunts]->(Organisms as food)  
or (eater)-[Eats as eats]->(food)
  {CODE-BLOCK/}
   * Graphical results view  
     ![Graphical View](images/BasicQuery_UsingOr_2_GraphicalView.png)  

---

{PANEL/}

---

{PANEL: Path graph query}  

A `path` is a document object in which a chain of sub-elements is embedded.  
You can include a path as an edge in your query, and RavenDB will regard it 
a multi-edge, forming a relation between your node and each path sub-element.  

* Take the document `orders/830-A` for example.  
   * The order includes a path named `Lines`. Each line in the path includes specifications regarding a product that's been included in the order.  
      {CODE-BLOCK:JSON}
      "Lines": [
        {
            "Discount": 0.2,
            "PricePerUnit": 19,
            "Product": "products/2-A",
            "ProductName": "Chang",
            "Quantity": 24
        },
        {
            "Discount": 0,
            "PricePerUnit": 10,
            "Product": "products/3-A",
            "ProductName": "Aniseed Syrup",
            "Quantity": 4
        },
        {
            "Discount": 0,
            "PricePerUnit": 22,
            "Product": "products/4-A",
            "ProductName": "Chef Anton's Cajun Seasoning",
            "Quantity": 1
        }
    ]
    {CODE-BLOCK/}
   * You can form a simple query that uses each line in the path as an edge, linking the order to a product.  
     Use this syntax:  
     `match (Orders as orders)-[Lines[].Product as lines]->(Products as products)`
      * With these results:  
        ![Textual View](images/PathQuery_GraphicalView_1.png)  
        ![Textual View](images/PathQuery_GraphicalView_2.png)  
{PANEL/}

---

{PANEL: Graph queries and Indexes}

####How Can Indexes Be Queried

You can graph-query an index as you would using a non-graph query.  
Querying indexes is currently supported with [explicit](../../../indexes/querying/graph/graph-queries-basic#explicitly-defining-data-elements) 
queries only.  

{CODE-BLOCK:JSON}
with {from index 'Orders/ByCompany'} as byCompany
match(byCompany)-[Company as company]->(Companies as companies)
{CODE-BLOCK/}

---

####How Are Graph Queries Indexed

Any non-trivial graph query actually creates several types of queries, each with its own measure of indexing and resources.  

* `match (Houses as houses)`  
  An expression like this creates **no index**, since no searching is required in order to simply retrieve all documents of a collection.  
* `match (Houses as forSale where Status = "ForSale")`  
  The node clause shown here will trigger RavenDB to create an auto index for queried houses, as it would with non-graph queries.  
* `match ((Houses as houses)-[Owner as ownedBy]->(Owners as owners))`  
  Queries with edges are handled by the graph queries engine, to fathom the relations between data nodes and hand them to clients.  
     > **Graph Queries and Map Reduce**  
     > To minimize the amount of data the graph engine processes in memory and maximize its usage of 
     > indexed data, it is recommended to create a static [Map Reduce](../../../studio/database/indexes/create-map-reduce-index) 
     > definition for every datasets you plan to include in an edge clause.  

* More complex queries can be combinations of the methods described above, like in the following examples.  
  {CODE-BLOCK:JSON}
  match
  (Houses as forSale where Status = "ForSale")
  and 
  (Houses as wanted where Address = "Virginia")  
  {CODE-BLOCK/}
  Here, RavenDB would first execute a _document query_ to create a dataset for each node.  
  Then it would apply the query's `and` condition to create the results set, fetching only 
  the houses it finds in both datasets.  
  {CODE-BLOCK:JSON}
  match
  (Houses as wanted where Status = "ForSale" and Address = "Virginia")
  or
  ((Houses as houses)-[Owner as ownedBy]->(Owners as owners))
  {CODE-BLOCK/}
  The edge in this query is sent for further processing by the graph engine.  

{PANEL/}

{PANEL: Narrowing Down Results}

As we've seen in [various earlier examples](../../../indexes/querying/graph/graph-queries-basic#explicitly-defining-data-elements), 
you can filter query results using `where`.  
`where` can be used both in data-node and edge clauses.  

---

####Filtering results in an edge clause  

{CODE-BLOCK:plain}  
match(Orders as orders) -  
[Lines as crabMeat  
where ProductName = "Boston Crab Meat"  
select Product] ->  
(Products as products)  
{CODE-BLOCK/}
![Filtering edges](images/BasicQuery_NarrowingResults4.png "Filtering edges") |

---

####Filtering results in data-node clauses  

Here, we limit the results to products sent to Brazil:  
{CODE-BLOCK:plain}  
match(Orders as orders where (ShipTo.Country = "Brazil")) -
[Lines as lines select Product] ->  
(Products as products)
{CODE-BLOCK/}
![Filtering nodes 1](images/BasicQuery_NarrowingResults1.png "Filtering nodeos 1") |

And here we filter these results further, to products sent to Brazil from France:  
{CODE-BLOCK:plain}  
match(Orders as orders where (ShipTo.Country = "Brazil")) -
[Lines as lines select Product] ->  
(Products as products) -
[Supplier as supplier] -> 
(Suppliers as suppliers where Address.Country = "France")
{CODE-BLOCK/}
![Filtering nodes 2](images/BasicQuery_NarrowingResults2.png "Filtering nodeos 2") |

{PANEL/}

## Related Articles

**Querying**:  
[RQL](../../../indexes/querying/what-is-rql#querying-rql---raven-query-language)  
[Indexes](../../../indexes/what-are-indexes#what-indexes-are)  

##Graph Querying**
[Overview](../../../indexes/querying/graph/graph-queries-overview#graph-querying-overview)  
[Basic Graph Queries](../../../indexes/querying/graph/graph-queries-basic#basic-graph-queries)  
[Recursive Graph Queries](../../../indexes/querying/graph/graph-queries-recursive#recursive-graph-queries)  
