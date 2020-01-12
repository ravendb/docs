# Graph Queries Filtering  

---

{NOTE: }

You can use [RQL](../../../indexes/querying/what-is-rql) keywords and 
operators within a graph query node/edge clause, to narrow query results down.  

* `where` can be used within **data node** and **edge** clauses, to condition the query.  
* `select` can be used within **edge** clauses, to choose wanted details of filtered results .  
* Operators like `and` and `or` can complement `where` within data-node and edge clauses.  
  (You can place these operators **between query sections** as well. Learn more about it here: 
  [Multi-Section Search Patterns](../../../indexes/querying/graph/graph-queries-multi-section#graph-queries-multi-section-search-patterns))  

{INFO: }
Sample queries included in this article use only data that is available in the 
[Northwind sample database](../../../studio/database/tasks/create-sample-data#creating-sample-data), 
so you may easily try them out.  
{INFO/}

* In this page:  
   * [Using `where` To Filter Results](../../../indexes/querying/graph/graph-queries-filtering#using--to-filter-results)  
   * [Using `select` To Choose Filtered Results](../../../indexes/querying/graph/graph-queries-filtering#using--to-choose-filtered-results)  
   * [Using `where` With `or`](../../../indexes/querying/graph/graph-queries-filtering#using--with-)  
   * [Using `where` With `and`](../../../indexes/querying/graph/graph-queries-filtering#using--with--1)  
   * [Additional Filtering Examples](../../../indexes/querying/graph/graph-queries-filtering#additional-filtering-examples)  
{NOTE/}

---

{PANEL: Filtering}

You can filter data within data-node and edge clauses, using RQL.  

---

#### Using `where` To Filter Results

`where` can be used within both **data-node** and **edge** clauses.  
Here, for example, we retrieve only big orders from the Orders collection:  
{CODE-BLOCK:JSON}
match 
    (Orders as orders 
       where Freight > 5)
{CODE-BLOCK/}

---

#### Using `select` To Choose Filtered Results

`select` can be used alongside `where`, within an **edge clause**, to select details of filtered data.  

In the following example, we're looking for paths between orders and products with a discount:
{CODE-BLOCK:JSON}
match 
    (Orders as orders)-  
    [Lines as cheap 
       where Discount >= 0.25 
           select Product]->
    (Products as products)
{CODE-BLOCK/}

* **Query Flow**:  
   * The edge clause uses `where` to search each order's 
     [Lines construct](../../../indexes/querying/graph/graph-queries-the-search-pattern#complex-edges).  
     Only edges of products with a high-enough discount will be used.  
   * Then, `select` is used to pick the actual edge - the ID of a product profile.  

{INFO: You **cannot** use `select` within a data-node clause.  }
![SELECT cannot be used in Node Clauses](images/Filtering_NoSelectInDataNode.png "SELECT cannot be used in Node Clauses")
{INFO/}

{NOTE: }
Note: Do not confuse the current usage of `select` with the implementation 
of the same keyword to [project](../../../indexes/querying/graph/graph-queries-overview#projecting-graph-results) data.  
{NOTE/}

---

#### Using `where` With `or`

The following query uses `where` along with `or`, to find products of two different categories.  
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

![Retrieved using OR](images/Filtering_Operators_1_GraphicalView.png "Retrieved using OR")

---

#### Using `where` With `and`

The following query uses `where` along with `and`, to find products in a chosen price range.  
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

* Filtering the **products** (the destination nodes) 
  also determine which **orders** (the origin nodes) 
  would be included in the results.  

![Retrieved using AND: Graphical View](images/Filtering_Operators_2_GraphicalView.png "Retrieved using AND: Graphical View")

![Retrieved using AND: Textual View](images/Filtering_Operators_2_TextualView.png "Retrieved using AND: Textual View")

---

#### Additional Filtering Examples

Let's look at a few more examples to filtering within query clauses.  

* Here, we query the relations between orders, products and suppliers.  
We look for products whose **discount is low** and **price is high**, because we want to renegotiate such deals.  
We limit the results to suppliers whose **contact is also the owner**, so we'd have who to negotiate with.  
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

select
    id(orders) as Pricy, 
    products.QuantityPerUnit as Negotiate, 
    salesrep.Name as Supplier,
    salesrep.Contact.Name as Contact
    {CODE-BLOCK/}

    ![Locate Negotiable Products: Graphical View](images/Filtering_Operators_3_GraphicalView.png "Locate Negotiable Products: Graphical View")

    ![Locate Negotiable Products: Textual View](images/Filtering_Operators_3_TextualView.png "Locate Negotiable Products: Textual View")

* Here is an example for the gradual development of a modular query, and filters included in it.  
   * First, we limit the retrieval of orders to ones ordered from Brazil.  
     {CODE-BLOCK:plain}  
match
    (Orders as orders 
        where (ShipTo.Country = "Brazil"))-
    [Lines as lines 
        select Product]->  
    (Products as products)
     {CODE-BLOCK/}

     ![Filtering Nodes](images/Filtering_NarrowingResults1.png "Filtering Nodes")

   * Then we add another data layer by including Suppliers in our query, 
     and create another filter to include in the results only french suppliers.  
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

![Filtering Nodes](images/Filtering_NarrowingResults2.png "Filtering Nodes")

{PANEL/}

## Related Articles

**Querying**:  
[RQL](../../../indexes/querying/what-is-rql#querying-rql---raven-query-language)  
[Indexes](../../../indexes/what-are-indexes#what-indexes-are)  

**Graph Querying**  
[Overview](../../../indexes/querying/graph/graph-queries-overview#graph-querying-overview)  
[The Search Pattern](../../../indexes/querying/graph/graph-queries-the-search-pattern#the-search-pattern)  
[Expanded Search Patterns](../../../indexes/querying/graph/graph-queries-expanded-search-patterns#graph-queries-expanded-search-patterns)  
[Explicit and Implicit Syntax](../../../indexes/querying/graph/graph-queries-explicit-and-implicit#explicit-and-implicit-syntax)  
[Graph Queries and Indexes](../../../indexes/querying/graph/graph-queries-and-indexes#graph-queries-and-indexes)  
[Multi-Section Search Patterns](../../../indexes/querying/graph/graph-queries-multi-section#graph-queries-multi-section-search-patterns)  
[Recursive Graph Queries](../../../indexes/querying/graph/graph-queries-recursive#recursive-graph-queries)  
