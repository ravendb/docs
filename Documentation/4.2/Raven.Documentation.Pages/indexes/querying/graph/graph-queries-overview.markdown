# Graph Querying: Overview  

---

{NOTE: }

RavenDB's [experimental](../../../indexes/querying/graph/graph-queries-overview#enabling-graph-querying) graph support 
allows you to query your database as if it had a graphical structure, gaining extreme efficiency and speed in recognizing 
relations between data elements and organizing them into searchable patterns. Intricate relationships that would render a 
relational database useless become the asset they are meant to be.  

* **No need for preliminary preparations**  
  You do not need to alter your database's structure or contents in order to start using graph queries. 
  Your existing collections and documents are used as graph elements, and their relations are inferred from documents' contents.  

* **Simple and effective syntax**  
  We've integrated graph support into [RQL](../../../indexes/querying/what-is-rql) to make its learning 
  and usage accessible and intuitive for any user, especially those already familiar with our query language.  

* **Comprehensive support**  
  Queries can be constructed either by clients using API methods or manually using the Studio, and executed 
  by your distributed database. Results can be retrieved by your clients or shown textually and graphically by the Studio.  

{INFO: }
The data used for all sample queries included here can be found in the 
[Northwind database](../../../studio/database/tasks/create-sample-data#creating-sample-data).  
{INFO/}

* In this page:  
   * [Introduction to graph modeling](../../../indexes/querying/graph/graph-queries-overview#introduction-to-graph-modeling)  
     * [Enabling Graph Querying](../../../indexes/querying/graph/graph-queries-overview#enabling-graph-querying)  
   * [Designing Graph Queries](../../../indexes/querying/graph/graph-queries-overview#designing-graph-queries)  
     * [Graph Representations](../../../indexes/querying/graph/graph-queries-overview#graph-representations)  
     * [Basic Terms, Syntax and Vocabulary](../../../indexes/querying/graph/graph-queries-overview#basic-terms-syntax-and-vocabulary)  
     * [Graph Queries Flow](../../../indexes/querying/graph/graph-queries-overview#graph-queries-flow)  
     * [Multi-Part Queries](../../../indexes/querying/graph/graph-queries-overview#multi-part-queries)  
     * [Bi-Directional Queries](../../../indexes/querying/graph/graph-queries-overview#bi-directional-queries)  
   * [FAQ](../../../indexes/querying/graph/graph-queries-overview#faq)  
   
{NOTE/}

---

{PANEL: Introduction to graph modeling}  

* **In The Beginning..**  
  One of the best known founding moments of graph theory is [Leonhard Euler](https://en.wikipedia.org/wiki/Leonhard_Euler)'s 
  attempt at solving the [Königsberg Bridges](https://en.wikipedia.org/wiki/Seven_Bridges_of_K%C3%B6nigsberg) riddle, 
  eventually tackling the problem by representing the scenery and its elements in a graph.  
  Euler's search for an optimal path is a great referral to the practicality of graph theory, 
  leading all the way to its present-day effectiveness in managing large and complex data volumes.  
* **Graph modeling and large data volumes**   
  Large, complex data volumes take an important role in the evolution of data management, and are evidently 
  here to stay and develop.  
  As relational databases and their data model are inefficient in (and often incapable of) searching and managing 
  large data volumes with intricate relations, various applications take part in complementing or replacing them.  
  Databases capable of running graph queries are a major contribution in this regard, though not limited 
  to the management of large data volumes and often as comfortable and efficient in handling smaller ones.  
* **Graph modeling in a multi-model database**  
  It is common to find graph querying as one of the features of a multi-model database, based upon 
  or cooperating with other database features.  
  RavenDB's graph capabilities are founded upon a capable document store, and data already deposited in 
  your store can participate graph querying with no preceding arrangements, easing user administration and 
  improving internal logic and data management.  

{NOTE: }

###Enabling Graph Querying

Graph Querying is an experimental feature, and is disabled by default.  
To activate it you need to edit RavenDB's [configuration file](../../../server/configuration/configuration-options#json) 
and enable [experimental features](../../../server/configuration/core-configuration#features.availability).  

{NOTE/}

{PANEL/}

{PANEL: Designing Graph Queries}  

Graph querying is an expansion of RavenDB's [RQL](../../../indexes/querying/what-is-rql). 
In order to build and test graph queries, you can use the Studio as you would with non-graph RQL.  
The Studio will identify your queries as graph queries by their syntax, and automatically show 
their results both graphically and textually.

![Graph Queries - Graphical View](images/Overview_GraphicalView.png "Graph Queries - Graphical View")  

---

![Graph Queries - Textual View](images/Overview_TextualView.png "Graph Queries - Textual View")  

##Graph representations

Graph querying enhances RQL with simple vocabulary and syntax that allow you to approach your existing data 
as if it had been designed graphically.  

* **Graph query**  
  This basic query shows relations between employees.  
  ![Graph query](images/Overview_GraphQuery.png "Graph query")  
* **Results**  
  The various elements are shown in your graphical and textual views.  
  ![Graph Reqults: Graphical View](images/Overview_GraphicalView_1.png "Graph Reqults: Graphical View")  
  ![Graph results: Textual View](images/Overview_TextualView_1.png "Graph results: Textual View")  

##Basic Terms, Syntax and Vocabulary

* **match**  
  A graph query is defined using the `match` keyword, followed by a search pattern.  
  The search pattern consists of **at least one data node**, and possibly **edges** leading to **additional nodes**.  
  The graph query `match (Orders)` for example, returns the whole `Orders` collection just like the non-graph query 
  "[from orders](../../../indexes/querying/what-is-rql#from)" would.  

* **Graph Elements**  
  The representations of documents ("Nodes") and their relations ("Edges") in a graph, are equally important. 
  * **Data Nodes\***  
    "Data nodes" can be a [documents collection](../../../indexes/querying/graph/graph-queries-basic#explicit-and-implicit-queries), 
    [index query](../../../indexes/querying/graph/graph-queries-basic#querying-indexes) results, 
    or a [selected subset](../../../indexes/querying/graph/graph-queries-basic#using-select-and-where) of either.  
    **\*** _We use the term "data nodes" to distinguish them from cluster servers ("cluster nodes")._  
        {INFO: }
        A data node is defined within a clause surrounded by parentheses:  
        `(orders)`  
        {INFO/}

        You can include RQL statements inside data nodes' clauses:  
        `match (Orders where id() = 'Orders/1-A')`  

  * **Edges**  
    An edge is a connection between data nodes, that indicates a relationship they have with each other (like "sold by" or "in charge of").  
      {INFO: }
      An edge is defined within a clause surrounded by square brackets:  
      `[Product]`  
      {INFO/}
    RavenDB edges are fields within documents. They are always **directional**, pointing from one data node to another.  
      * Use a hyphen to connect an edge clause to the node the edge points **from**:  
        `match (Orders) - [Product]`  
        In this example the edge indicates the name of a field, `Product`, within the data node (a document of the `orders` collection).  
      * Use an arrow (`<-` or `->`) to connect an edge clause to the data node it points **at**:  
        `[Product] -> (Products)`  
        In this example, the value of the `Product` field is the name of a document in the `Products` collection.  
      * **Edge Field**  
        An edge can be a simple string field that contains a document's ID.  
        You may use **orders/45-A**'s **Product** field for example as an edge that relates it to **products/20-A**.  
        
            | Document | Query |
            |:-------------:|:-------------:|
            | ![Simple Relation](images/Overview_001_Field.png "Simple Relation") | {CODE-BLOCK:JSON}
             match 
    (Orders)- 
    [Product]-> 
    (Products)
             {CODE-BLOCK/} ![Simple Relation - Elements](images/Overview_001_Elements.png "Simple Relation - Elements") |  
            **1** - The first data node is **orders/45-A** in the **Orders** collection.  
            **2** - The edge is **"Product": "products/20-A"**, a string field in the first node.  
            **3** - The second data node is **products/20-A** in the **Products** collection.  

      * **Edges Array**  
        Edges can also be included in a [nested JSON structure](../../../indexes/querying/graph/graph-queries-basic#edge-arrays) 
        with multiple string fields that point at multiple documents.  
        
            | Document | Query |
            |:-------------:|:-------------:|
            | ![Multiple Relations](images/Overview_002_Field.png "Multiple Relations") | {CODE-BLOCK:JSON}
             match 
    (Orders)- 
    [Lines[].Product]-> 
    (Products)
             {CODE-BLOCK/} ![Multiple Relations](images/Overview_002_Elements.png "Multiple Relations") |

  * **Aliases**  
    Graph elements are given **aliases** to make them more coherent both for human readers and for RavenDB's interpreter.  
    Using aliases is mandatory for [explicit](../../../indexes/querying/graph/graph-queries-basic#explicitly-choosing-data-elements-syntax) 
    queries, and is recommended and sometimes required with 
    [implicit](../../../indexes/querying/graph/graph-queries-basic#implicitly-choosing-data-elements-syntax) queries.  
      {INFO: }
      To give an element an alias, use the `as` keyword:  
      `match (Orders as myOrders)`  
      {INFO/}
    
        | Document | Query |
        |:-------------:|:-------------:|
        | ![Aliases](images/Overview_001_Field.png "Aliases") |  {CODE-BLOCK:JSON}
         match 
    (Orders)- 
    [Product as product]-> 
    (Products)
             {CODE-BLOCK/} ![Aliases](images/Overview_003_Elements.png "Aliases") |  
    
      * The same element may be given multiple aliases, to allow it to play different roles in different parts of the same query.  
      * The alias `_` indicates that the element that carries it will not appear in the query results.  
        The same applies to `__`, `___`, etc.
      * Each alias needs to be unique.  

* **Paths**  
    A **hop** is a direct connection between two data nodes, including the two nodes and the edge between them.  
    A **path** is the sequence of hops between two nodes.  

    In the following graph the sequence between employees/7-A and employees/5-A is a hop, 
    and the whole path is two hops long.  
    ![Path](images/Path.png "Path")  
    
    Graph queries and their results are often evaluated and related to in terms of paths.  
    E.g. a graph query created to locate the shortest path between two nodes, or a query 
    that succeeded to yield many helpful paths to new customers.  
    


##Graph Queries Flow  

* **Phase 1: Indexing**  
  When a graph query is executed, RavenDB queries its data nodes first.  
  Resulting Lucene and Collection queries are indexed, and the indexes are used by the graph engine.  
* **Phase 2: Handling Relations**  
  If the query includes edges the graph engine uses them now, to fathom relations between elements while 
  going through the indexes prepared during the first phase.  

##Multi-Part Queries  

You can expand a graph query by simply adding it an edge clause and a data-node clause.  

* **Initial query**:
  {CODE-BLOCK:JSON}
match
    (Orders as orders 
       where Company = "companies/34-A")-
    [Lines[].Product as product]->
    (Products as products)
{CODE-BLOCK/}
* **Expanded query**:
{CODE-BLOCK:JSON}
match
    (Orders as orders 
       where Company = "companies/34-A")-
    [Lines[].Product as product]->
    (Products as products)-
    [Supplier as supplier]->
    (Suppliers as suppliers)
{CODE-BLOCK/}

##Bi-Directional Queries  
You can direct a graph query to the left and/or to the right.  

* These two queries are similar:
  {CODE-BLOCK:JSON}
match 
     (Orders as orders)-
     [Company as company]->
     (Companies as companies)
  {CODE-BLOCK/}
  {CODE-BLOCK:JSON}
match 
     (Companies as companies)
     <-[Company as company]
     -(Orders as orders)
  {CODE-BLOCK/}

* And this query sends edges from **orders** both to **employees** that take care of them and to **companies** the orders are from.  
  {CODE-BLOCK:JSON}
match 
     (Employees as employees)
     <-[Employee as employee]
     -(Orders as orders)-
     [Company as company]->
     (Companies as companies)
  {CODE-BLOCK/}  

{PANEL/}

{PANEL: FAQ}

####Q: When should I use graph queries?  

A: There are configurations and situations for which graph querying is an optimal solution, and circumstances 
that invite different approaches.  

* **Use graph querying when -**  
   * **Your intricate, layered data is mismanaged**, so most of its potential is never revealed **and** 
     it becomes a huge nuisance.  
     For graph querying, webs of relations are not a wasteland but a natural habitat. Try them 
     if it's likely that your gathering data has much more to offer than can currently be seen.  
     You can, for example, collect data from a web of relations: dynamically build user profiles, 
     product pages, vendor data sheets and so on, using graph queries that collect data related 
     to them in the first degree, second degree, third degree and so on.  

   * **You look for optimized paths**.  
     As their [history](../../../indexes/querying/graph/graph-queries-overview#introduction-to-graph-modeling) suggests, 
     graph queries are awesome in finding **optimal paths** between related nodes. Graph-using applications may find the 
     fastest way to a suitable host, the quickest publicity route to a destination audience, or the cheapest way to get 
     a specified product.  

* Graph querying may **not** be an ideal solution for you if -  
   * Your documents are isolated from each other by structure or preference.  
   * Your data is pre-arranged and pre-indexed, requiring no ongoing relation queries to refurnish its contents.  
   * A different model has a clear advantage, e.g. key/value store for key/value customer lists or a relational 
     database for fixed tables.   
   * Your query starts with a broad search.  
     Graph queries work best when the search begins with a definite starting point and lays out a path from there on.  

{PANEL/}

## Related Articles

**Querying**:  
[RQL](../../../indexes/querying/what-is-rql#querying-rql---raven-query-language)  
[Indexes](../../../indexes/what-are-indexes#what-indexes-are)  

**Graph Querying**  
[Overview](../../../indexes/querying/graph/graph-queries-overview#graph-querying-overview)  
[Basic Graph Queries](../../../indexes/querying/graph/graph-queries-basic#basic-graph-queries)  
[Recursive Graph Queries](../../../indexes/querying/graph/graph-queries-recursive#recursive-graph-queries)  
