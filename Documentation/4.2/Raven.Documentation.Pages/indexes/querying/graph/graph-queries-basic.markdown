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
Project selected documents using the `select` keyword and other RQL commands.  

* Here's a very simple projection example:  
  {CODE-BLOCK:JSON}
with {from Houses} as houses
match(houses)
select houses.Owner
  {CODE-BLOCK/}
  Producing these results:  
  ![Textual View](images/BasicQuery_ProjectionResults.png)  

{PANEL/}

---

{PANEL: Operators: OR and AND}  

You can use the `or` and `and` operators to condition your graph queries, as you would in a regular RQL expression.  
Here's a sample query that uses 'or' to retrieve the profiles of owned houses as well as of ones that are up for sale.  

* Here's our query:  
  {CODE-BLOCK:JSON}
  match 
  (Houses as forSale where Status = "ForSale")
  or
  ((Houses as houses)-[Owner as ownedBy]->(Owners as owners))
  {CODE-BLOCK/}
  Producing these results:  
   * Graphical results view  
     ![Graphical View](images/BasicQuery_UsingOr_GraphicalView.png)  
   * Textual results view  
     ![Textual View](images/BasicQuery_UsingOr_TextualView.png)  

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

   * participating documents:  

        {CODE-BLOCK:JSON}  
        {
            "Name": "...",
            "Proximity": "Near",
            "Morality": "Green",
            "ServedBefore": [
                "Customers/John",
                "Customers/Simcha",
                "Customers/Rebecca"
            ],
            "@metadata": {
                "@collection": "Suppliers",
                "@id": "Suppliers/Gomgom",
            }
        }

        {
            "Name": "...",
            "Supplier": "Suppliers/Gomgom",
            "Price": 40,
            "Quality": "High",
            "@metadata": {
                "@collection": "Products",
                "@id": "Products/Paper",
            }
        }

        {
            "Name": "...",
            "Age": "62",
            "ProximityPreference": "Near",
            "PricePreference": "Low",
            "@metadata": {
                "@collection": "Customers",
                "@id": "Customers/John",
            }
        }

        {
            "Name": "...",
            "Age": "90",
            "ProximityPreference": "Near",
            "PricePreference": "Low",
            "@metadata": {
                "@collection": "Customers",
                "@id": "Customers/Simcha",
            }
        }
        {CODE-BLOCK/}

{PANEL/}

## Related Articles
**Client Articles**:  
[Query](../../../../server/ongoing-tasks/backup-overview)  
[Graph Query](../../../../client-api/operations/maintenance/backup/backup)  
[Recursion](../../../../client-api/operations/maintenance/backup/restore)  

**Studio Articles**:  
[Creating a query](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
[Seeing query results](../../../../studio/server/databases/create-new-database/from-backup)  
