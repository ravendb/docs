# Basic Graph Queries  

---

{NOTE: }

* You can explicitly choose elements that participate graph queries, or allow RavenDB to do it for you.  
* You can project selected data.  
* You can use edges defined in document fields as well as edge arrays.  
* You can filter query results to get more accurate results using `AND`, `OR`, `WHERE` and `SELECT`.  
* You can query indexes.
* Different queries may use different amounts of resources.  

{INFO: }
The data used for all sample queries included here can be found in the 
[Northwind database](../../../studio/database/tasks/create-sample-data#creating-sample-data).  
{INFO/}

* In this page:  
   * [Explicit and Implicit queries](../../../indexes/querying/graph/graph-queries-basic#explicit-and-implicit-queries)  
   * [Using `SELECT` and `WHERE`](../../../indexes/querying/graph/graph-queries-basic#using-select-and-where)  
   * [Using Operators: OR and AND](../../../indexes/querying/graph/graph-queries-basic#using-operators:-or-and-and)  
   * [Edge Arrays](../../../indexes/querying/graph/graph-queries-basic#edge-arrays)  
   * [Graph queries and Indexes](../../../indexes/querying/graph/graph-queries-basic#graph-queries-and-indexes)  
      * [Querying Indexes](../../../indexes/querying/graph/graph-queries-basic#querying-indexes)  
      * [How Are Graph Queries Indexes](../../../indexes/querying/graph/graph-queries-basic#how-are-graph-queries-indexed)  
   * [Narrowing Down Results](../../../indexes/querying/graph/graph-queries-basic#narrowing-down-results)  

{NOTE/}

---

{PANEL: Explicit and Implicit queries}  

To [create a graph query](../../../indexes/querying/graph/graph-queries-overview#graph-representations), 
follow the `match` keyword with your search pattern.  

[Data elements](../../../indexes/querying/graph/graph-queries-overview#basic-terms-syntax-and-vocabulary) 
(nodes and edges) need to be **chosen** in order to participate a query.  
You can choose them explicitly if you prefer, it may clarify your queries.  
You can also ommit choosing them, and RavenDB will do it automatically for you.  

* To explicitly choose a data element, use a `with` clause.  
  {CODE-BLOCK:JSON}
with {from Houses} as houses  

match
    (houses)
{CODE-BLOCK/}

* To let RavenDB choose data elements, simply ommit `with` clauses.  
  RavenDB will notice their absence and choose your data elements for you.  
  {CODE-BLOCK:JSON}
match 
    (Houses as houses)
{CODE-BLOCK/}

{INFO: }
Explicitly or implicitly choosing data elements is a readability preference, with no performance implications.  

* Choosing data elements yourself may clarify your queries.
* Allowing RavenDB do it for you makes your code more concise.
{INFO/}

---

####Explicitly Choosing Data Elements: Syntax  

* A **data node** is chosen using `from` within a `with` clause.  
    {CODE-BLOCK:JSON}
with {from Products} as products
    
match 
    (products)
    {CODE-BLOCK/}
    **Products** is a collection from which data nodes are retrieved.  
    **as products** is an [alias](../../../indexes/querying/graph/graph-queries-overview#basic-terms-syntax-and-vocabulary).  
  
    This query simply retrieves documents from the Products collection.  
    ![Retrieve All - Graphical view](images/explicitQuery_GraphicalView1.png "Retrieve All - Graphical view")  

* An **edge** is chosen using a `with edges` clause.  
  {CODE-BLOCK:JSON}
with {from Products} as products

with {from Suppliers} as suppliers
with edges (Supplier) as supplier

match 
     (products)-
     [supplier]->
     (suppliers)
  {CODE-BLOCK/}
  **Supplier** is the edge. It is the name of a text field that contains a reference to a document.  
  This query shows the edges that relate products to their suppliers.  
  ![Retrieve All - Graphical view](images/explicitQuery_GraphicalView2.png "Retrieve All - Graphical view")  

{NOTE: }
Providing data elements with aliases is **mandatory** in explicit queries.  
{NOTE/}

    
{PANEL/}

---

####Implicitly Choosing Data Elements: Syntax

To let RavenDB choose your data elements for you, simply include them in your `match` search pattern.  

`match (Orders as orders)`  
RavenDB will automatically choose `Orders` as a data node and run the query.  
![Retrieve All - Graphical view](images/BasicQuery_GraphicalView1.png "Retrieve All - Graphical view")  

---

{PANEL: Using `SELECT` and `WHERE`}

* You can use SELECT to [project](../../../indexes/querying/projections#what-are-projections-and-when-to-use-them) 
  graph query results.  
  To do so, place a `select` clause after your query.  
  {CODE-BLOCK:JSON}
  with {from Houses} as houses
match 
    (houses)
select houses.Owner
  {CODE-BLOCK/}
     With these results -  
  ![Textual View](images/BasicQuery_ProjectionResults.png "Textual View")  

* You can use `select` and `WHERE` within edge clauses.  
  {CODE-BLOCK:JSON}
match 
    (Orders as orders where Freight > 5)
  {CODE-BLOCK/}

    {CODE-BLOCK:JSON}
match 
    (Orders as orders)-  
    [Lines as cheap 
        where Discount >= 0.25 select Product]->
    (Products as products)
  {CODE-BLOCK/}

* You can use `WHERE` but **not** `select` within data-node clauses.  
 {CODE-BLOCK:JSON}
//This won't work
match 
    (Orders as orders 
        where Freight > 5 
            select Order)
{CODE-BLOCK/}
  
{PANEL/}

---

{PANEL: Using Operators: OR and AND}  

You can use the `or` and `and` operators in your graph queries.  

* **Example 1**: Use `or` to locate and present products of two different categories  

    {CODE-TABS}
    {CODE-TAB-BLOCK:sql:Implicit} 
match
    (Products as products)-
    [Category as category]->
    (Categories as categories 
        where Name="Confections" 
            or Name="Condiments")
{CODE-TAB-BLOCK/}
    {CODE-TAB-BLOCK:sql:Explicit}
with {from Products} as products
with edges (Category) as category
with {from Categories 
    where Name="Confections" 
        or Name="Condiments"} as categories
match 
    (products)-
    [category]->
    (categories)
{CODE-TAB-BLOCK/}
    {CODE-TABS/}
     ![Graphical View](images/BasicQuery_Operators_1_GraphicalView.png "Graphical View")  

* **Example 2**: Use `and` to locate orders of products in a chosen price range.  

    {CODE-BLOCK:JSON}
match
    (Orders as orders) -
    [Lines as lines 
        select Product]->  
    (Products as products 
        where (PricePerUnit < 20 
            and PricePerUnit > 15))
    select orders.Company as company, products.Name as name
    {CODE-BLOCK/}
     ![Graphical View](images/BasicQuery_Operators_2_GraphicalView.png "Graphical View")  
     ![Textual View](images/BasicQuery_Operators_2_TextualView.png "Textual View")  

* **Example 3**: query the relations between orders, products and suppliers.  
Find products whose discount is low and price is high, so we'd like to negotiate about them.  
Find suppliers whose contact is also the owner, so we believe we have who to negotiate with.  
    {CODE-BLOCK:JSON}
match
    (Orders as orders 
        where (Lines.PricePerUnit > 50 
            and Lines.Discount < 0.10))-
    [Lines as negotiateThisProduct 
        select Product]->
    (Products as products)-
    [Supplier as contactIsOwner]->
    (Suppliers as salesrep 
        where Contact.Title = "Owner")
    {CODE-BLOCK/}
    ![Graphical View](images/BasicQuery_Operators_3_GraphicalView.png "Graphical View")  
{PANEL/}

---

{PANEL: Edge Arrays}  

You can include an array of edges in your query, and RavenDB will form a relation using each array member.  

* `orders/830-A` for example includes a `Lines` array.  
   * Each line includes specifications regarding a product included in the order.  
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
   * You can form a simple query that uses each line as an edge and links the order to a product.  
     {CODE-BLOCK:JSON}
match 
    (Orders as orders)-
    [Lines[].Product as lines]->
    (Products as products)
{CODE-BLOCK/}
      * With these results:  
        ![Textual View](images/PathQuery_GraphicalView_1.png "Textual View")  
        ![Textual View](images/PathQuery_GraphicalView_2.png "Textual View")  
{PANEL/}

---

{PANEL: Graph queries and Indexes}

####Querying Indexes

You can graph-query an index as you can with a non-graph query.  

{NOTE: }
Querying indexes is currently supported only when you 
[explicitly](../../../indexes/querying/graph/graph-queries-basic#explicitly-choosing-data-elements-syntax) 
choose your data elements.  
{NOTE/}

{CODE-BLOCK:JSON}
with {from index 'Orders/ByCompany'} as byCompany

match
    (byCompany)-
    [Company as company]->
    (Companies as companies)
{CODE-BLOCK/}

---

####How Are Graph Queries Indexed

Any graph query over the bare minimum is interpreted to several types of queries, 
each with its own measure of indexing and resources usage.  
Complex queries may combine methods mentioned here.  

* {CODE-BLOCK:JSON}
match 
    (Orders as orders) 
{CODE-BLOCK/}
  This query creates **no index**, since no searching is required in order to retrieve a whole collection.  

* {CODE-BLOCK:JSON}
match 
    (Orders as orders 
        where ShipTo.Region = "Nueva Esparta")
{CODE-BLOCK/}
  The node clause shown here will trigger RavenDB to create an auto index for queried orders, as is done with non-graph queries.  

* {CODE-BLOCK:JSON}
match 
    (Orders as orders)-
    [ShipVia as shipvia]->
    (Shippers as shippers)
{CODE-BLOCK/}
  Queries with edges are handled by the graph queries engine before handing them to clients, 
  to fathom the relations between data nodes.  

    {INFO: Graph Queries and Map Reduce}
  To minimize the amount of data the graph engine processes in memory and maximize its usage of 
  indexed data, it is recommended to create a static [Map Reduce](../../../studio/database/indexes/create-map-reduce-index) 
  definition for every dataset you plan to include in an edge clause.  
  {INFO/}

{PANEL/}

{PANEL: Narrowing Down Results}

As [demonstrated above](../../../indexes/querying/graph/graph-queries-basic#using-select-and-where), 
query results can be filtered using `where`.  
`where` can be used both in data-node and edge clauses.  

---

Here's an example of **filtering results in an edge clause**:  

{CODE-BLOCK:plain}  
match
    (Orders as orders)-  
    [Lines as crabMeat 
        where ProductName = "Boston Crab Meat" 
            select Product]->  
    (Products as products)  
{CODE-BLOCK/}
![Filtering edges](images/BasicQuery_NarrowingResults4.png "Filtering edges") |

---

And here's one for **filtering results in a data-node clause**, limiting the results to products sent to Brazil.  
{CODE-BLOCK:plain}  
match
    (Orders as orders 
        where (ShipTo.Country = "Brazil"))-
    [Lines as lines 
        select Product]->  
    (Products as products)
{CODE-BLOCK/}
![Filtering nodes 1](images/BasicQuery_NarrowingResults1.png "Filtering nodeos 1") |

---

We can then filter the results even further, to products sent to Brazil from France:  
{CODE-BLOCK:plain}  
match
    (Orders as orders 
        where (ShipTo.Country = "Brazil"))-
    [Lines as lines 
        select Product]->  
    (Products as products)-
    [Supplier as supplier]-> 
    (Suppliers as suppliers 
        where Address.Country = "France")
{CODE-BLOCK/}
![Filtering nodes 2](images/BasicQuery_NarrowingResults2.png "Filtering nodeos 2") |

{PANEL/}

## Related Articles

**Querying**:  
[RQL](../../../indexes/querying/what-is-rql#querying-rql---raven-query-language)  
[Indexes](../../../indexes/what-are-indexes#what-indexes-are)  

**Graph Querying**  
[Overview](../../../indexes/querying/graph/graph-queries-overview#graph-querying-overview)  
[Basic Graph Queries](../../../indexes/querying/graph/graph-queries-basic#basic-graph-queries)  
[Recursive Graph Queries](../../../indexes/querying/graph/graph-queries-recursive#recursive-graph-queries)  
