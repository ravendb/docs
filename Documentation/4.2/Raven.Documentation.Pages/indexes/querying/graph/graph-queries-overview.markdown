# Graph Querying: Overview  

---

{NOTE: }

RavenDB's [experimental](../../../indexes/querying/graph/graph-queries-overview#enabling-graph-querying) **graph** support 
allows you to query your database as if it had a graphical structure, gaining extreme efficiency and speed in recognizing 
relations between data elements and organizing them into searchable patterns. Intricate relationships that would render 
a relational databases useless, become the asset they are meant to be.  

* **No need for preliminary preparations**  
  You do not need to alter your database's structure or contents in order to start using graph queries. 
  Your existing collections and documents are used as graph elements, and their relations are inferred from documents' contents.  

* **Simple and effective syntax**  
  We've integrated graph support into [RQL](), to make its learning and usage accessible and intuitive for any user, 
  especially those already familiar with our query language.  

* **Comprehensive support**  
  Queries can be constructed by either clients using API methods or manually using the Studio, and executed 
  by your distributed server. Results can be used by your clients, or shown textually and graphically by the Studio.  

* In this page:  
   * [Introduction to graph modelling](../../../indexes/querying/graph/graph-queries-overview#introduction-to-graph-modelling)  
     * [Enabling Graph Querying](../../../indexes/querying/graph/graph-queries-overview#enabling-graph-querying)  
   * [Designing Graph Queries](../../../indexes/querying/graph/graph-queries-overview#designing-graph-queries)  
     * [Graph Representations](../../../indexes/querying/graph/graph-queries-overview#graph-representations)  
     * [Basic Terms, Syntax and Vocabulary](../../../indexes/querying/graph/graph-queries-overview#basic-terms-syntax-and-vocabulary)  
   * [FAQ](../../../indexes/querying/graph/graph-queries-overview#faq)  
     * [When should or shouldn't I use graph queries?](../../../indexes/querying/graph/graph-queries-overview#q-when-should-or-shouldnt-i-use-graph-queries)  
   
{NOTE/}

---

{PANEL: Introduction to graph modelling}  

* **In The Beginning..**  
  One of the best known founding moments of graph theory is [Leonhard Euler](https://en.wikipedia.org/wiki/Leonhard_Euler)'s 
  attempt at solving the [Königsberg Bridges](https://en.wikipedia.org/wiki/Seven_Bridges_of_K%C3%B6nigsberg) riddle, 
  eventually tackling the problem by representing the scenery and its elements in a graph.  
  Euler's search for an optimal path is a great referal to the practicality of graph theory, leading all the way to its 
  immense present-day effectiveness in managing large and complex data volumes.  
* **..and large data volumes**   
  [Large, complex data volumes](https://www.datamation.com/big-data/big-data-companies.html) represent 
  an important step in the evolution of data management, and are evidently here to stay and develop.  
  As relational databases and the data model they enable are inefficient in (and often incapable of) 
  searching and managing big-data volumes with intricate relations, various applications take part 
  in complementing or replacing them.  
  Databases capable of running graph queries are a major contribution in this regard, though 
  not limited to the  management of large data volumes and often as comfortable and efficient 
  in handling smaller ones.  
* **..and Multi Model**  
  It is common to find graph querying as one of the features of a multi-model database, based upon 
  or cooperating with other database features.  
  RavenDB's graph capabilities are founded upon a capable document store, and data already deposited in 
  the store can participate graph querying with no preceding arrangements, easing user administration and 
  improving internal logic and data management.  

{NOTE: }

###Enabling Graph Querying

Graph Querying is an **Experimental feature**, still under development. We are happy to provide it, and would be grateful 
to hear from you regarding your experiences with it.  
As other experimental fatures, it is disabled by default. You can enable it following theis simple procedure:  

* Open the RavenDB server folder, e.g. C:\Users\Dave\Downloads\RavenDB-4.1.1-windows-x64\Server  
* Open settings.json for editing  
* Enable the Experimental Features.  
   * Verify that the json file contains the following line:  
     "Features.Availability": "Experimental"  
   * Save settings.json and restart RavenDB Server.  

{NOTE/}

{PANEL/}

{PANEL: Designing Graph Queries}  

####Graph representations

Graph querying enhances RQL with simple vocabulary and syntax that allow you to approach your existing data 
as if it had been designed graphically. Here's a basic query that shows relations between employees, using documents 
taken from the Northwind database RavenDB lets you install as sample data.  

* Graph query:  
{CODE-BLOCK:JSON}
match(employees as employee)-[ReportsTo as reportsTo]->(employees as incharge)  
{CODE-BLOCK/}
* Query results are provided by the Studio both graphically and textually.  

 ![Illustrative graph reqults](images/Overview_GraphicalView_1.png)  
 ![Textual graph results](images/Overview_TextualView_1.png)  

---

####Basic Terms, Syntax and Vocabulary

* **Graph Elements**  
  Data elements ("Nodes") and their relations ("Edges") are represented in a graph as equally important.  
  * **Data Nodes\***  
    A "data node" can be a documents collection, or a subset of selected documents.  
    **\*** _We use the term "data nodes" to make it easier for you to distinguish between the data elements 
    we talk about here, and servers of a cluster (that are also called "nodes")._  
  * **Edges**  
    An "edge" is a link between nodes, that joins them in a relation of some sort.  
    A RavenDB edge is simply a string-field within a document, that refers to the unique identifier of a document.  
    RavenDB's edges are always **directional**, pointing from one data node to another.  
     
* **Graph results**  
  Here are the results of a very simple query.  
  ![Figure 1. Simple Relation](images/Overview_Elements.png)  

  1. The first data node is **Dogs/Ruffus**, a document named **Ruffus** in the **Dogs** collection.  
    This is how the document may look like:
    {CODE-BLOCK:JSON}
    Document: Dogs/Ruffus
    {
       "Owner": "Owners/John",
       "Name": "...",
       "@metadata": {
       "@collection": "Dogs",
       "@id": "Dogs/Ruffus"}
    }
    {CODE-BLOCK/}

  2. The arrow titled **ownedBy** is the edge, indicating that Ruffus belongs to John.  
     You can find its definition in the **Ruffus** document as the "Owner" field containing John's ID.  

  3. The second data node is **Owners/John**, a document named **John** in the **Owners** collection.
    {CODE-BLOCK:JSON}
    Document: Owners/John
    {
       "Name": "...",
       "@metadata": {
       "@collection": "Owners",
       "@id": "Owners/John"
       }
    }
    {CODE-BLOCK/}

* **Graph query**  
  Here's a query that could have produced the results shown above:  
  `match(Dogs)-[Owner as ownedBy]->(Owners)`  
  Let's go through its parts and syntax.  
   * The `match` keyword instructs the retrieval of documents that match specified conditions.  
     In this case, the conditions specify document of one collection, connected by ownership to documents of another collection.  
   * The **data nodes** are indicated by surrounding parantheses: `(Dogs)` and `(Owners)`.  
     In this example, they are the **Dogs** and **Owners** document collections.  
   * The **edge** is placed within brackets: `[Owner as ownedBy]`.  
     It has a specific **direction**, pointing from Ruffus to John.  
     A hyphen connects it to the node it emerges from: `-`  
     An "arrow" combined of a hyphen and a bigger-than symbol connects it to the node it points at: `->`  
   * You can tag graph elements (nodes and edges) with whatever **alias** you choose.  
     Use the `as` keyword to do so, like in -  
     `match(Dogs as dogs)-[Owner as ownedBy]->(Owners as owner)`  
     Giving elements aliases isn't obligatory when they are defined [implicitly](../../../indexes/querying/graph/graph-queries-basic#implicitly-defining-nodes-and-edges), and **is** required when defining them [explicitly](../../../indexes/querying/graph/graph-queries-basic#explicitly-defining-data-elements).  
     It is, however, often recommended and sometimes essential.  
      * In our sample query, the Owner relation between a dog and its owner is given the alias "ownedBy", 
        because we wanted to emphasize this aspect of the relations. Another query may emphasize a different 
        aspect by using the alias "occupant", "patient" or something else.  
      * The same node or edge may appear multiple times in a query, sometimes in very different roles.  
        Using different aliases may be technically needed in such cases.  
      * To eliminate an entity from the results, use a sequence of `_` symbols as an alias (i.e. `_`, `__`, `___`..)  
      * Each alias needs to be unique.  
        Note that this is true for `_` aliases as well: use each `_` sequence (`_`, `__`, `___`..) only once.  
* **Graph Queries Flow**
   * Lucene indexing  
     When a graph query is executed, the first thing RavenDB does is index each node clause using Lucene.  
     The result is a group of indexed tables that the graph engine can easily play with.  
   * Handling relations  
     If the query comprises edges, the graph engine uses them now while going through the table prepared during the first phase 
     and fathoming the relations between table elements.  
     [Be aware](../../../indexes/querying/graph/graph-queries-basic#graph-queries-and-indexes) that this part of a query is performed in memory and is not indexed, so reruns actually mean 
     re-running it.  

{PANEL/}

{PANEL: FAQ}

####Q: When should or shouldn't I use graph queries?  

A: There are configurations and situations for which graph querying is an optimal solution, and other 
circumstances that require different approaches. You may find this list helpful in determining whether
to give it a go.  

* **Use graph querying when -**  
   * **Relations between data elements are a concern**.  
     If your data continuously grows in quantity and intricity, queries become increasingly complicated and results 
     arrive after longer and longer periods of time, graph queries are likely to be the solution you're craving for.  
   * **You look for optimized paths**.  
     As their [history](../../../indexes/querying/graph/graph-queries-overview#introduction-to-graph-modelling) suggestss, 
     graph queries are awesom in finding **optimal paths** between related nodes. Graph-using applications may find the fastest 
     way to a suitable host, the quickest publicity route to a destination audience, or the cheapest way to get a specified product.  
   * **You want to collect data from a web of relations**.  
     You can dynamically build user profiles, product pages, vendor data sheets and so on, 
     using graph queries that collect data related to them in the first degree, second degree, third degree and so on.  

* Graph querying may **not** be an ideal solution for you if -  
   * Your documents are isolated from each other by structure or preference.  
   * Your data is pre-arranged and pre-indexed, requiring no ongoing relation queries to refurnish its contents.  
   * A different model has a clear advantage, e.g. key/value store for key/value customer lists, relational database 
     for fixed tables, etc.   
   * Your queries starts with a broad search.  
     Graph queries work best when the search starts with a definite starting point and lays out a path from there on.  

{PANEL/}

## Related Articles
**Client Articles**:  
[Query](../../../../server/ongoing-tasks/backup-overview)  
[Graph Query](../../../../client-api/operations/maintenance/backup/backup)  
[Recursion](../../../../client-api/operations/maintenance/backup/restore)  

**Studio Articles**:  
[Creating a query](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
[Seeing query results](../../../../studio/server/databases/create-new-database/from-backup)  


