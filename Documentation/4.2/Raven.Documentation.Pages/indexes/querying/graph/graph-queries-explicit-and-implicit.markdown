# Explicit and Implicit Syntax  

---

{NOTE: }

* You can declare your data nodes and edges explicitly, or let RavenDB do it for you by simply 
  including nodes and edges in your search pattern as if they have already been declared.  
  Using explicit or implicit syntax is a **readability preference** with no performance implications.  
* The **explicit syntax** may be preferable when you need to **express non-trivial logic**.    
* The **implicit syntax** may be preferable when you want to **be simple or more concise**.  
* Querying indexes is currently available only when you use the explicit syntax.    

{INFO: }
Sample queries included in this article use only data that is available in the 
[Northwind sample database](../../../studio/database/tasks/create-sample-data#creating-sample-data), 
so you may easily try them out.  
{INFO/}

* In this page:  
   * [Implicit and Explicit Queries](../../../indexes/querying/graph/graph-queries-explicit-and-implicit#implicit-and-explicit-queries)  
   * [Explicitly Declaring Data Elements](../../../indexes/querying/graph/graph-queries-explicit-and-implicit#explicitly-declaring-data-elements)  
   * [Mixing Explicit and Implicit Declarations](../../../indexes/querying/graph/graph-queries-explicit-and-implicit#mixing-explicit-and-implicit-declarations)  
{NOTE/}

---

{PANEL: Implicit and Explicit Queries}  

Here is a simple query that retrieves all documents from the **Orders** collection.  

{CODE-BLOCK:JSON}
//Declare a data node
with {from Orders} as orders
    
//Create the search pattern
match 
    (orders)
{CODE-BLOCK/}

It uses an `explicit syntax`: nodes and edges are explicitly declared using a `with` clause before including them in the 
search pattern.  

{INFO: }
`with` clauses allow you to define arbitrary restrictions on matches for selected data nodes or edges.  
{INFO/}

And here is a second query, exactly equivalent to the first, that uses an `implicit syntax`.  
{CODE-BLOCK:JSON}
//Create the search pattern
match
    (Orders as orders)  
{CODE-BLOCK/}

Implicit queries allow you to include nodes and edges in your search pattern without explicitly declaring them first, 
inviting RavenDB to declare them automatically.  

The two queries produce the exact same results:  
![Retrieve All Orders](images/BasicQuery_GraphicalView1.png "Retrieve All Orders")  

---

####Explicitly Declaring Data Elements

* **Declaring Data Nodes**  
  Declare a data node using a `with` clause with curly brackets.  
  Provide each node with a **unique alias**.  

    The following query retrieves all documents from the **Products** collection.  

    {CODE-BLOCK:JSON}
//Declare a data node
with {from Products} as products
    
match 
    (products)
  {CODE-BLOCK/}

    ![Data Nodes: Products](images/explicitQuery_GraphicalView1.png "Data Nodes: Products")

* **Declaring Edges**  
  Declare an edge using a `with edges` clause with parentheses.  
  Provide each edge with a **unique alias**.  

    The following query shows the relations between products and their suppliers.  

    {CODE-BLOCK:JSON}
//Declare data nodes
with {from Products} as products
with {from Suppliers} as suppliers

//Declare an edge
with edges (Supplier) as supplier

match 
     (products)-
     [supplier]->
     (suppliers)
  {CODE-BLOCK/}
    Documents are retrieved from the Products collection, and each product is related to its supplier's profile 
    by the edge: an ID string in the product's Supplier property.  

    ![Edges between Products and Suppliers](images/explicitQuery_GraphicalView2.png "Edges between Products and Suppliers")

---

####Mixing Explicit and Implicit Declarations  

You can freely mix and match the explicit and implicit syntaxes.  
E.g. -  
{CODE-BLOCK:JSON}
//Declare a data node
with {from Products} as products
    
//Create the search pattern with declared and undeclared elements
match 
     (products)-
     [Supplier as supplier]->
     (Suppliers as suppliers)
{CODE-BLOCK/}

{PANEL/}

## Related Articles

**Querying**:  
[RQL](../../../indexes/querying/what-is-rql#querying-rql---raven-query-language)  
[Indexes](../../../indexes/what-are-indexes#what-indexes-are)  

**Graph Querying**  
[Overview](../../../indexes/querying/graph/graph-queries-overview#graph-querying-overview)  
[The Search Pattern](../../../indexes/querying/graph/graph-queries-the-search-pattern#the-search-pattern)  
[Expanded Search Patterns](../../../indexes/querying/graph/graph-queries-expanded-search-patterns#graph-queries-expanded-search-patterns)  
[Graph Queries and Indexes](../../../indexes/querying/graph/graph-queries-and-indexes#graph-queries-and-indexes)  
[Filtering](../../../indexes/querying/graph/graph-queries-filtering#graph-queries-filtering)  
[Multi-Section Search Patterns](../../../indexes/querying/graph/graph-queries-multi-section#graph-queries-multi-section-search-patterns)  
[Recursive Graph Queries](../../../indexes/querying/graph/graph-queries-recursive#recursive-graph-queries)  
