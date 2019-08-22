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
  We've integrated graph support into [RQL](../../../indexes/querying/what-is-rql), to make its learning 
  and usage accessible and intuitive for any user, especially those already familiar with our query language.  

* **Comprehensive support**  
  Queries can be constructed either by clients using API methods or manually using the Studio, and executed 
  by your distributed database. Results can be used by your clients or shown textually and graphically by the Studio.  

* In this page:  
   * [Introduction to graph modeling](../../../indexes/querying/graph/graph-queries-overview#introduction-to-graph-modeling)  
     * [Enabling Graph Querying](../../../indexes/querying/graph/graph-queries-overview#enabling-graph-querying)  
   * [Designing Graph Queries](../../../indexes/querying/graph/graph-queries-overview#designing-graph-queries)  
     * [Graph Representations](../../../indexes/querying/graph/graph-queries-overview#graph-representations)  
     * [Basic Terms, Syntax and Vocabulary](../../../indexes/querying/graph/graph-queries-overview#basic-terms-syntax-and-vocabulary)  
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
  [Large, complex data volumes](https://www.datamation.com/big-data/big-data-companies.html) represent 
  an important step in the evolution of data management and are evidently here to stay and develop.  
  As relational databases and the data model they enable are inefficient in (and often incapable of) 
  searching and managing big-data volumes with intricate relations, various applications take part 
  in complementing or replacing them.  
  Databases capable of running graph queries are a major contribution in this regard, though not limited 
  to the management of large data volumes and often as comfortable and efficient in handling smaller ones.  
* **Graph modeling in a multi-model database**  
  It is common to find graph querying as one of the features of a multi-model database, based upon 
  or cooperating with other database features.  
  RavenDB's graph capabilities are founded upon a capable document store, and data already deposited in 
  the store can participate graph querying with no preceding arrangements, easing user administration and 
  improving internal logic and data management.  

{NOTE: }

###Enabling Graph Querying

Graph Querying is an experimental feature, and is disabled by default.  
To enable it you need to edit RavenDB's [configuration](../../../server/configuration/configuration-options#json) 
and enable [experimental features](../../../server/configuration/core-configuration#features.availability).  

{NOTE/}

{PANEL/}

{PANEL: Designing Graph Queries}  

Graph queries enhance RavenDB's [RQL](../../../indexes/querying/what-is-rql), and you can 
use the Studio to build and test them as you would with non-graph RQL.  
The Studio will identify your queries as graph queries by their syntax, and automatically 
represent their results both graphically and textually.

![Graph Queries - Graphical View](images/Overview_GraphicalView.png)  

---

![Graph Queries - Textual View](images/Overview_TextualView.png)  

##Graph representations

Graph querying enhances RQL with simple vocabulary and syntax that allow you to approach your existing data 
as if it had been designed graphically.  

* **Graph query**  
  This basic query shows relations between employees.  
  It uses documents taken from the Northwind database that RavenDB lets you install as sample data.  
  ![Graph query](images/Overview_GraphQuery.png)  
* **Results**  
  Results are provided by the Studio both graphically and textually.  
  ![Graph reqults: Illustration](images/Overview_GraphicalView_1.png)  
  ![Graph results: Textual](images/Overview_TextualView_1.png)  

##Basic Terms, Syntax and Vocabulary

* **match**  
  A graph query is defined using the `match` keyword, with a search pattern of at least one data node and possibly 
  edges leading to additional nodes.  
  The graph query **match(orders)** for example returns the whole **orders** collection, 
  like the non-graph query "[from orders](../../../indexes/querying/what-is-rql#from)".  
* **Graph Elements**  
  Documents ("Nodes") and their relations ("Edges") are represented in a graph as equally important. 
  * **Data Nodes\***  
    "Data nodes" can be a [documents collection](../../../indexes/querying/graph/graph-queries-basic#explicit-and-implicit-queries), 
    [index query](../../../indexes/querying/graph/graph-queries-basic#how-can-indexes-be-queried) results, 
    or a [selected subset](../../../indexes/querying/graph/graph-queries-basic#projection) of either.  
      {INFO: }
      A data node is defined within a clause surrounded by parentheses.  
      E.g. `(orders)`  
      {INFO/}
    You can narrow the results down using RQL, within data nodes' clauses. E.g. `match(orders where id() = 'Orders/1-A')`.  
    **\*** _We use the term "data nodes" to distinguish them from cluster servers ("cluster nodes")._  

  * **Edges**  
    "Edges" are links between nodes, that indicate nodes' relations with each other (e.g. "sold by", "in charge of", etc).  
      {INFO: }
      An edge is defined within a clause surrounded by square brackets.  
      E.g. `[Product]`  
      {INFO/}
    RavenDB edges are fields within documents. They are always **directional**, pointing from one data node to another.  
      * Use a hyphen to connect an edge clause to the node the edge points **from**.  
        E.g. `match(orders)-[Product]`  
        In this example the edge indicates the name of a field, `Product`, within the data node (a document of the `orders` collection).  
      * Use an arrow (`<-` or `->`) to connect an edge clause to the data node it points **at**.  
        E.g. `[Product]->(Products)`  
        In this example, the value of the `Product` field is the name of a document in the `Products` collection.  
      * An edge can be a simple string field, that contains a document's ID.  
        e.g. a document named 45-A in the orders collection, may include this field: **"Product": "products/20-A"**.  
        The **Product** field points orders/45-A to the document 20-A of the **products** collection.  
        
            | Document | query: **match(orders)-[Product]->(Products)** |
            |:-------------:|:-------------:|
            | ![Figure 1. Simple Relation](images/Overview_001_Field.png) | ![Figure 1. Simple Relation](images/Overview_001_Elements.png) |  
            **1** - The first data node is **orders/45-A**, a document named **45-A** in the **orders** collection.  
            **2** - The edge is **"Product": "products/20-A"**, a string field in the first node.  
            **1** - The second data node is **products/20-A**, a document named **20-A** in the **products** collection.  

      * An edge can also be a [nested JSON structure](../../../indexes/querying/graph/graph-queries-basic#path-graph-query) 
        with multiple string fields that point at multiple documents.  
        
            | Document | query: **match(orders)-[Lines.Product]->(Products)** |
            |:-------------:|:-------------:|
            | ![Figure 2. multiple Relations](images/Overview_002_Field.png) | ![Figure 2. multiple Relations](images/Overview_002_Elements.png) |

  * **Aliases**  
    Graph elements are given **aliases** to make them more coherent both for human readers and for RavenDB's interpreter.  
    Using aliases is mandatory for [explicit](../../../indexes/querying/graph/graph-queries-basic#explicitly-defining-data-elements) 
    queries, and is recommended and sometimes required with 
    [implicit](../../../indexes/querying/graph/graph-queries-basic#implicitly-defining-nodes-and-edges) queries.  
      {INFO: }
      Use `as` to tag an element with an alias.  
      E.g. `match(orders as myOrders)`  
      {INFO/}
    
        | Document | query: **match(orders)-[Product as product]->(Products)** |
        |:-------------:|:-------------:|
        | ![Figure 3. Aliases](images/Overview_001_Field.png) | ![Figure 3. Aliases](images/Overview_003_Elements.png) |  
    
      * The same node or edge may appear multiple times in a query, sometimes in very different roles.  
        Using different aliases may be technically needed in such cases.  
      * To eliminate an entity from the results, use a sequence of `_` symbols as its alias (i.e. `_`, `__`, `___`..)  
      * Each alias needs to be unique.  
        Note that this is true for `_` aliases as well: use each `_` sequence (`_`, `__`, `___`..) only once.  
      
* **Graph Queries Flow**
   * Indexing  
     When a graph query is executed, RavenDB first queries data nodes.  
     Resulting Lucene and Collection queries are indexed, and the indexes are grouped in tables the graph engine may use.  
   * Handling relations  
     If the query comprises edges, the graph engine uses them now while going through the table prepared during the first 
     phase and fathoming the relations between table elements.  
     [Be aware](../../../indexes/querying/graph/graph-queries-basic#graph-queries-and-indexes) that this part of a query is performed in memory and is not indexed, so reruns actually mean 
     re-running it.  

{PANEL/}

{PANEL: FAQ}

####Q: When should I use graph queries?  

A: There are configurations and situations for which graph querying is an optimal solution, and circumstances 
that invite different approaches.  

* **Use graph querying when -**  
   * **Relations between your data entities can reveal valuable information.**.  
     Intricate, layered data is often mismanaged so not only most of its potential is never revealed 
     but it also becomes a huge nuisance.  
     For graph querying, webs of relations are not a wasteland but a natural habitat. Give it a try 
     if it seems likely that your gathering data has much more to offer than can currently be seen.  
   * **You look for optimized paths**.  
     As their [history](../../../indexes/querying/graph/graph-queries-overview#introduction-to-graph-modeling) suggestss, 
     graph queries are awesome in finding **optimal paths** between related nodes. Graph-using applications may find the 
     fastest way to a suitable host, the quickest publicity route to a destination audience, or the cheapest way to get 
     a specified product.  
   * **You want to collect data from a web of relations**.  
     You can dynamically build user profiles, product pages, vendor data sheets and so on, 
     using graph queries that collect data related to them in the first degree, second degree, third degree and so on.  

* Graph querying may **not** be an ideal solution for you if -  
   * Your documents are isolated from each other by structure or preference.  
   * Your data is pre-arranged and pre-indexed, requiring no ongoing relation queries to refurnish its contents.  
   * A different model has a clear advantage, e.g. key/value store for key/value customer lists, relational database 
     for fixed tables, etc.   
   * Your query starts with a broad search.  
     Graph queries work best when the search starts with a definite starting point and lays out a path from there on.  

{PANEL/}

## Related Articles

**Querying**:  
[RQL](../../../indexes/querying/what-is-rql#querying-rql---raven-query-language)  
[Indexes](../../../indexes/what-are-indexes#what-indexes-are)  

##Graph Querying**
[Overview](../../../indexes/querying/graph/graph-queries-overview#graph-querying-overview)  
[Basic Graph Queries](../../../indexes/querying/graph/graph-queries-basic#basic-graph-queries)  
[Recursive Graph Queries](../../../indexes/querying/graph/graph-queries-recursive#recursive-graph-queries)  
