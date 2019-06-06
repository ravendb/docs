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

{NOTE/}

---

{PANEL: Explicit and Implicit queries}  

Every data node and edge that is to participate a query, is chosen using a 'with' clause.  
After choosing all data elements this way, a 'match' statement uses them to define the search pattern.  
Only data that matches this pattern is retrieved.  

It is perfectly fine to omit 'with' clauses from your code. RavenDB will notice unchosen data 
elements in your 'match' pattern and choose them for you with nearly no performance toll.  

* **Not including 'with' clauses** in your queries can improve your code's conciseness 
  and automates a task that doesn't need your attention.  
* **Defining 'with' clauses yourself** may help you create a more readable code.  

---

####Implicitly defining nodes and edges  

To use an implicit query:  

* Use a 'match' clause to define and run a search pattern.  

Here are two simple examples for implicit queries that search a tiny three-documents collection.  

* `match (Houses as houses)`  
  Use this search pattern to retrieve all documents of the "Houses" collection.  
   * Graphical results view  
     ![](images/BasicQuery_GraphicalView1.png)  
   * Textual results view  
     ![](images/BasicQuery_TextualView1.png)  

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
     ![Graphical View](images/BasicQuery_GraphicalView2.png)  
   * Textual results view  
     ![Textual View](images/BasicQuery_TextualView2.png)  

---

####Explicitly defining data elements  

* To define an explicit query:  
   * Choose data elements that wold participate your query.
      * Choose each Data Node for your query using 'from' within a 'with' clause.  
        E.g. `with {from Houses} as houses`  
        'Houses' is a collection chosen to function as a data node.  
        'as houses' is an alias for the data node. Define it outside the 'with' clause.  
      * Choose each Edge using a 'with edges' clause.  
        `with edges (Owner) as ownedBy`  
        'Owner' is the edge. It is the name of a text field that contains a reference to a document.  
   * {NOTE: Providing your data elements with aliases is **mandatory** in explicit queries. /NOTE}  
   * Use your aliases in a `match` statement to define and run the search pattern.  

* Here are samples for explicit queries and their implicit equivalents.  

    |Explicit|Implicit|
    |---------------|---------------|
    |{CODE-BLOCK:JSON}
    with {from Houses} as houses
    match (houses)
    {CODE-BLOCK/}|{CODE-BLOCK:JSON} match (Houses as houses) {CODE-BLOCK/}|
    |{CODE-BLOCK:JSON}
    with {from Houses where Status = "ForSale"} as forSale
    match (forSale)
    {CODE-BLOCK/}|{CODE-BLOCK:JSON} match (Houses as forSale where Status = "ForSale") {CODE-BLOCK/}|
    |{CODE-BLOCK:JSON}
    with {from Houses} as houses
    with edges (Owner) as ownedBy
    with {from Owners} as owner
    match(houses)-[ownedBy]->(owner)
    {CODE-BLOCK/}|{CODE-BLOCK:JSON}
    match ((Houses as houses)-[Owner as ownedBy]->(Owners as owners))
    {CODE-BLOCK/}|

* And an additional sample, this time for a query that includes edges.  

    |Explicit|Implicit|
    |---------------|---------------|
    |{CODE-BLOCK:JSON}
    with {from Houses} as houses
    with edges (Owner) as ownedBy
    with {from Owners} as owner
    match(houses)-[ownedBy]->(owner)
    {CODE-BLOCK/}|{CODE-BLOCK:JSON}
    match ((Houses as houses)-[Owner as ownedBy]->(Owners as owners))
    {CODE-BLOCK/}|
    
    Both produce the same results of course:  
     * Graphical results view  
       ![Graphical View](images/BasicQuery_GraphicalView3.png)  
     * Textual results view  
       ![Textual View](images/BasicQuery_TextualView3.png)  

{PANEL/}

---

{PANEL: Projection}

As with non-graph queries, you can use the `select` keyword to project a subset 
of graph query results, putting results in order and minimizing server data transfers.  

* For example 
   * {This projection
     CODE-BLOCK:JSON}
     with {from Houses} as houses
     match(houses)
     select houses.Owner
     {CODE-BLOCK/}
   * With these results
     Producing these results:  
     ![Textual View](images/BasicQuery_ProjectionResults.png)  

* You can project from within an edge clause, but not from a node clause.  
   * You can do this  
     `match (Orders as orders)-[Lines as cheap where Discount >= 0.25 select Product]->(Products as products)`  
   * And this 
     `match (Orders as orders where Freight > 5)`  
   * But not this 
     `match (Orders as orders where Freight > 5 select Order)`  

{PANEL/}

---

{PANEL: Operators: OR and AND}  

You can use the `or` and `and` operators to condition your graph queries, as you would in a regular RQL expression.  
Here's a sample query that uses 'or' to retrieve the profiles of owned houses as well as of ones that are up for sale.  

* Here's a query showing the usage of 'or':  
  {CODE-BLOCK:JSON}
  match 
  (Houses as forSale where Status = "ForSale")
  or
  ((Houses as houses)-[Owner as ownedBy]->(Owners as owners))
  {CODE-BLOCK/}
  Producing these results:  
   * Graphical results view  
     ![Graphical View](images/BasicQuery_UsingOr_1_GraphicalView.png)  
   * Textual results view  
     ![Textual View](images/BasicQuery_UsingOr_1_TextualView.png)  
* And another example, showing the usage of 'or' in a way that reveals more relations between nodes:  
  {CODE-BLOCK:JSON}
  match  
  (Creatures as eater)-[Hunts as hunts]->(Creatures as food)  
  or (eater)-[Eats as eats]->(food)  
  {CODE-BLOCK/}
   * Query results:  
   * Graphical results view  
     ![Graphical View](images/BasicQuery_UsingOr_2_GraphicalView.png)  
   * Textual results view  
     ![Textual View](images/BasicQuery_UsingOr_2_TextualView.png)  

---

{PANEL/}

---

{PANEL: Path graph query}  

A `path` is a document object in which a chain of sub-elements is embedded.  
You can include a path as an edge in your query, and RavenDB will regard it a multi-edge, forming a relation between 
your node and each path sub-element.  

* Take the document `orders/830-A` for example.  
   * The order includes a path named `Lines`. Each line in the path, includes specifications regarding a product that's been included in the order.  
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
     `match (Orders as orders)-[Lines.Product as lines]->(Products as products)`
      * With these results:  
        ![Textual View](images/PathQuery_TextualView.png)  
   * **note: a bug prevents graphic view of one-to-many relations, a path drawing can't be shown here. add when fixed.**  
{PANEL/}

---

{PANEL: Graph queries and Indexes}
Any non-trivial graph query actually creates several types of queries, each with its own measure of indexing and resources.  

* `match (Houses as houses)`  
  An expression like this creates **no index**, since no searching is required in order to simply retrieve all documents of a collection.  
* `match (Houses as forSale where Status = "ForSale")`  
  A node clause like the one shown here triggers RavenDB to create a simple document (Lucene) index for queried houses, 
  as it would with non-graph queries.  
* `match ((Houses as houses)-[Owner as ownedBy]->(Owners as owners))`  
  A query with edges like this one is handled by the graph queries engine, to fathom the relations between data nodes 
  and hand them to clients.  
   > **Graph Queries and Map Reduce**  
   > To minimize the amount of data the graph engine processes in memory and maximize its usage of 
   > indexed data, it is recommended to create a static [Map Reduce](../../../studio/database/indexes/create-map-reduce-index) 
   > definition for every datasets you plan to include in an edge clause.  

* More complex queries can be combinations of the methods described above, like in the following examples.  
   * {CODE-BLOCK:JSON}
match
(Houses as wanted where Status = "ForSale")
and 
(Houses as wanted where Address = "Virginia")  
     {CODE-BLOCK/}  
     Here, RavenDB would first execute a _document query_ to create a dataset for each node.  
     Then it would apply the query's `and` condition to create the results set, fetching only 
     the houses it finds in both datasets.  
     
   * {CODE-BLOCK:JSON}
match
(Houses as wanted where Status = "ForSale" and Address = "Virginia")
or
((Houses as houses)-[Owner as ownedBy]->(Owners as owners))
     {CODE-BLOCK/}  
     Since this query has an edge, the graph engine has a bigger role in running it.  
     Comparing the indexed tables and concluding the relations between them require memory, 
     and do not speed up each time the query is executed.  

{PANEL/}

{PANEL: Narrowing Down Results}

As we've already seen in earlier examples, e.g. `match (Houses as forSale where Status = "ForSale")`, 
you can use `where` to filter query results.  
The query in the following example remains unchanged, except for an increasing number of 'where' 
filters that trim the number of mathces to retrieve only those results that actually match the user's needs.  

     | Query    | Results Graph |
     |:-------------:|:-------------:|
     | {CODE-BLOCK:plain}  
       match(  
       (Products as products) -  
       [Supplier as supplier] ->  
       (Suppliers as suppliers) -  
       [ServedBefore as servedbefore] ->  
       (Customers as customers)  
       )
       {CODE-BLOCK/} | ![Narrowing down results 1](images/BasicQuery_NarrowingResults1.png "Narrowing down results 1") |
     | {CODE-BLOCK:plain}  
       match(  
       (Products as products where Price < 50) -   
       [Supplier as supplier] ->  
       (Suppliers as suppliers where Proximity = "Near") -  
       [ServedBefore as servedbefore] ->  
       (Customers as customers)  
       ) {CODE-BLOCK/} | ![Narrowing down results 2](images/BasicQuery_NarrowingResults2.png "Narrowing down results 2") |
     | {CODE-BLOCK:plain}  
       match(  
       (Products as products where Price < 50) -   
       [Supplier as supplier] ->  
       (Suppliers as suppliers where Proximity = "Near") -  
       [ServedBefore as servedbefore] ->  
       (Customers as customers where ProximityPreference = "Near")  
       ) {CODE-BLOCK/} | ![Narrowing down results 3](images/BasicQuery_NarrowingResults3.png "Narrowing down results 3") |

{PANEL/}

## Related Articles
**Client Articles**:  
[Query](../../../../server/ongoing-tasks/backup-overview)  
[Graph Query](../../../../client-api/operations/maintenance/backup/backup)  
[Recursion](../../../../client-api/operations/maintenance/backup/restore)  

**Studio Articles**:  
[Creating a query](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
[Seeing query results](../../../../studio/server/databases/create-new-database/from-backup)  
