# Graph Queries Multi-Section Search Patterns  

---

{NOTE: }

You can create an advanced graph query, whose search pattern groups **multiple sections**.  
Each section is an autonomous search pattern that may include data nodes and edges.  

* The different sections are unified into a single search pattern using **operators** 
  like `and` and `or`.  
* By using the correct operator, you can determine how documents or paths located 
  by a section would be used: they can be **excluded** from the overall results, 
  **included** in them unconditionally, or included in case they **intersect** with 
  results of other searches.  

{INFO: }
Sample queries included in this article use only data that is available in the 
[Northwind sample database](../../../studio/database/tasks/create-sample-data#creating-sample-data), 
so you may easily try them out.  
{INFO/}

* In this page:  
   * [Graph Intersection, Inclusion and Exclusion](../../../indexes/querying/graph/graph-queries-multi-section#graph-intersection,-inclusion-and-exclusion)  
   * [The Flow of Multi-Section Queries](../../../indexes/querying/graph/graph-queries-multi-section#the-flow-of-multi-section-queries)  
   * [Exclusion](../../../indexes/querying/graph/graph-queries-multi-section#exclusion)  
   * [Intersection](../../../indexes/querying/graph/graph-queries-multi-section#intersection)  
   * [Using Multiple Destination Clauses](../../../indexes/querying/graph/graph-queries-multi-section#using-multiple-destination-clauses)  
{NOTE/}

---

{PANEL: Graph Intersection, Inclusion and Exclusion}

Here is a **two-sections query**, that discovers paths leading from **orders** to **products**.  
It is an **inclusive** query, that includes in the final results **paths found by both sections**.  

{CODE-BLOCK:plain}  
//destination node
with 
   {from Products 
      where Supplier ='suppliers/15-A'} as prod

//search pattern
match 
   //first section
   (Orders as LondonOrders 
      where ShipTo.City = 'London') -
   [Lines[].Product] ->
   (prod)
or   
   //second section
   (Orders as ParisOrders 
      where ShipTo.City = 'Paris') -
   [Lines[].Product] ->
   (prod)
{CODE-BLOCK/}

* The **first section** finds paths from **orders shipped to London** to the **products they order**.  
  The **second section** finds paths from **orders shipped to Paris** to the **products they order**.  
  The `or` operator determines that paths found by both sections would be included in the query results.  

* Query results:  
  
    ![Inclusion: Using OR To Find Two Series Of Paths](images/MultiSection01_Inclusion_Or.png "Inclusion: Using OR To Find Two Series Of Paths")

---

#### The Flow of Multi-Section Queries

A multi-section query is executed in two main phases.  

1. Each autonomous section produces its own results dataset, i/e/ a series of paths.  
2. The operators between the sections determine which data would remain in the final results.  
   {NOTE: Operators you may use:}
   
   * `or` - **Inclusion**  
     Use `or` to **include** in the results data found by **either section**.  
   * `and` - [Intersection](../../../client-api/session/querying/how-to-use-intersect#session-querying-how-to-use-intersect)  
     Use `and` to include in the results data found **by both sections**.  
   * `and not` - **Exclusion**  
     Use `and not` to **exclude** a dataset from the results.  

  {NOTE/}

---

#### Exclusion  

To exclude from the results a dataset located by a section, use `and not`.  
The following query is identical to the previous example in everything except for its usage of 
`and not` instead of `or`, **excluding** from the results paths of orders shipped to Paris.  
{CODE-BLOCK:plain}  
with 
   {from Products 
      where Supplier ='suppliers/15-A'} as prod

match 
   (Orders as LondonOrders 
      where ShipTo.City = 'London') -
   [Lines[].Product] ->
   (prod)
and not   
   (Orders as ParisOrders 
      where ShipTo.City = 'Paris') -
   [Lines[].Product] ->
   (prod)
{CODE-BLOCK/}
  
![Exclusion: AND NOT](images/MultiSection02_Exclusion_AndNot.png "Exclusion: AND NOT")

* Comparing the results with those of 
  [the previous example](../../../indexes/querying/graph/graph-queries-multi-section#graph-intersection,-inclusion-and-exclusion) 
  reveals that products/69-A and orders connected to it are now missing.  
  They are missing because products/69-A is destination for orders shipped to both London and 
  Paris, the kind of paths the current query leaves out.  

---

#### Intersection

To include in the results only paths common to both compared datasets, aka **intersection**, 
use `and`.  

The following query is identical to the previous two, except that here we place `and` between 
the two sections to leave in the results only paths discovered **by both sections**.  
{CODE-BLOCK:plain}  
with 
   {from Products 
      where Supplier ='suppliers/15-A'} as prod

match 
   (Orders as LondonOrders 
      where ShipTo.City = 'London') -
   [Lines[].Product] ->
   (prod)
and   
   (Orders as ParisOrders 
      where ShipTo.City = 'Paris') -
   [Lines[].Product] ->
   (prod)
{CODE-BLOCK/}
  
![Intersection: AND](images/MultiSection03_Intersection_And.png "Intersection: AND")

* Comparing the results with those of previous examples reveals that only products/69-A and 
  orders connected to it remain, an exact mirror image of the previous (
  [Exclusion](../../../indexes/querying/graph/graph-queries-multi-section#exclusion)
  ) example.  
  products/69-A remains because it is destination for orders shipped to both London **and** Paris, 
  the intersecting paths the current query looks for.  

---

#### Using Multiple Destination Clauses

Previous examples have shown us how to approach the same destination from different sections.  
It is sometimes useful, however, to create a query that approaches **multiple destinations**.  
To accomplish this, we need to give each destination a unique alias.  
{CODE-BLOCK:plain}  
//Destination 1
with 
   {from Orders 
      where ShipTo.City = 'London' } as LondonOrders

//Destination 2
with 
   {from Orders 
      where ShipTo.City = 'Paris' } as ParisOrders
{CODE-BLOCK/} 

Here is a query that uses multiple destinations, to find products that ship to London **and** Paris for **different prices**.  

{CODE-BLOCK:plain}  
with 
   {from Orders 
      where ShipTo.City = 'London' } as LondonOrders

with 
   {from Orders 
      where ShipTo.City = 'Paris' } as ParisOrders

match 
   (LondonOrders) -
   [Lines as LondonLine select Product] ->
   (Products as p)
   <- [Lines as ParisLine select Product]
   - (ParisOrders)

//Compare prices to the different destinations
where 
   LondonLine.PricePerUnit != ParisLine.PricePerUnit
   
select
   id(p) as ProductID,
   LondonLine.PricePerUnit as LondonPricePerUnit,
   LondonLine.ProductName as LondonProductName,
   ParisLine.PricePerUnit as ParisPricePerUnit,
   ParisLine.ProductName as ParisProductName
  
{CODE-BLOCK/} 

![Same Destination, Multiple Aliases](images/MultiSection04_MultiAlias.png "Same Destination, Multiple Aliases")

![Same Destination, Multiple Aliases - Textual Results](images/MultiSection05_MultiAlias.png "Same Destination, Multiple Aliases - Textual Results")

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
[Filtering](../../../indexes/querying/graph/graph-queries-filtering#graph-queries-filtering)  
[Recursive Graph Queries](../../../indexes/querying/graph/graph-queries-recursive#recursive-graph-queries)  
