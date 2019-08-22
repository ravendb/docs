# Recursive Graph Queries  

* Many tasks that had previously required recursive queries, can now be handled by graph queries.  
  If you often turn to recursion out of habit, you should probably check each time whether it really 
  is your best course of action.  

* A recursive query **is** probably better when you wish to continuously loop through a search, using 
  each round's result for the next round.  
  As a recursive function repeatedly calls itself until its objective is fulfilled, a recursive query 
  repeats a search pattern until the search is concluded.  
  
* You can use recursive queries to easily locate a system's smallest component, find an organization's 
  full "chain of command", map a structure, and for countless other tasks.  

---

{NOTE: }

Use a recursive graph query to repeat a search pattern within a chosen collection or 
throughout your database, until a condition is met.  

* In this page:  
   * [Recursive Query Syntax](../../../indexes/querying/graph/graph-queries-recursive#recursive-graph-queries)  
      * [Recursion Parameters](../../../indexes/querying/graph/graph-queries-recursive#recursion-parameters)  
   * [One-Collection Recursion](../../../indexes/querying/graph/graph-queries-recursive#one-collection-recursion)  
   * [Cross-Collection Recursion](../../../indexes/querying/graph/graph-queries-recursive#cross-collection-recursion)  

{NOTE/}

---

{PANEL: Recursive Graph Queries}

##Recursive Query Syntax

To define a graph query use the `recursive` keyword, and surround the edge and destination data node by curly braces 
to set the recursive pattern. When the query is executed, the edge is repeatedly reinstated between newly-found nodes, 
until the search pattern is fulfilled.  

* `match (Employees as employee where id() = 'employees/9-A') - recursive as chainOfManagers {[ReportsTo as reportsTo]->(Employees as manager)}`  
    * Define where the query starts:  
      `match (Employees as employee where id() = 'employees/9-A')`  
    * Define that the search pattern is recursive:  
      `- recursive as chainOfManagers`  
    * Surround the edge and destination-data-node clauses with curly brackets:  
      `{ [ReportsTo as reportsTo] -> (Employees as manager) }`  


---

####Recursion Parameters

Adjust your queries using this syntax: `- recursive as chainOfManagers(longest)`  
You can choose a minimum hops number, a maximum hops number, multiple-paths configuration, or a combination of them all.  

| Parameters | Type | Description | Example |
| ------------- | ------------- | ----- |
| **Min** | int | **Set a minimum limit for the number of hops between data nodes**.  If the minimum is not reached, the results are discarded. | `- recursive as chainOfManagers(1)` |
| **Max** | int | **Set a maximum limit for the number of hops between data nodes**.  If the maximum is reached, the search stops. | `- recursive as chainOfManagers(1,2)`  The first number is the minimum, the second is the maximum. |
| **RecursiveMatchType** | string | If multiple paths are found, choose how to handle them. **Default Value**: 'Lazy' | `- recursive as chainOfManagers(shortest)` |

* **Allowed `RecursiveMatchType` values**:  
   * `all` - query for all the paths that match the search pattern.  
   * `longest` - Query for the longest matching path.  
     E.g. for the largest number of archeological sites to visit between two locations.  
   * `shortest` - Query for the shortest matching path.  
     E.g. for the shortest flight plan.  
   * `lazy` - Query for the first matching path (which may not be the longest or shortest path).


* **Combining parameters**:  
  You can combine parameters, like so: `- recursive as chainOfManagers(1, 2, all)`  
  In this example, you discard the results if they are of less than 1 hop, 
  limit the maximum number of hops to 2, and retrieve all possible paths.  

##One-Collection Recursion

To recurse through documents of a certain collection, include its name in the destination data node.  
Here are a few examples:  

* `match (Employees as employee) - recursive as chainOfManagers(all) {[ReportsTo as reportsTo]->(Employees as manager)}`
  Note that the starting location is unspecified.  
  It is recommended to specify your starting point, e.g. -  `Employees as employee where id() = "employees/9-A")`  
  {INFO: }
  The data  for this example is included in the [Northwind database](../../../studio/database/tasks/create-sample-data#creating-sample-data), you can install and try it.  
  {INFO/}

* `match(People as people where id() = "People/John") - recursive as path(all) {[Likes as likes] -> (People as liked)}`  
  This query recurses through a database of single-document profiles and tracks the path of **likes** from a selected 
  person on.  
  A person likes others if their IDs are in its **Likes** field.  
  The recursion checks a person's "Likes" field, retrieves profiles whose IDs it contains, checks their "Likes" field, and so on.  

     |         Query part         | Role |
     | ------------------------ | ------------------------ |
     | `match(People as people where id() = "People/John") ` | The query will start with this document. |
     | `- recursive as path` | a recursive edge can be given an alias like any other edge. This one is called "path". |
     | `(all)` | recurse through [all possible paths](../../../indexes/querying/graph/graph-queries-recursive#recursion-parameters).  (You can also choose to find the `longest` path, the `shortest` one, or be `lazy` and simply go for the first.) |
     | `{}` | The recursive edge repeatedly reinstates itself for newly found data nodes.  The edge and the data-node are therefore enveloped by curly braces, to indicate they are both in the recursion scope. |
     | `[Likes as likes]` | The field to be checked in retrieved documents. |
     | `-> (People as liked)` | The destination data-node's collection. |

    * "People" documents in the sample database we used include a **Likes** field with IDs of others they like.  
      E.g.
  {CODE-BLOCK:JSON}
Document: People/John
{
    "Likes": [
        "People/Mathilda",
        "People/Bojangles"
    ],
    "@metadata": {
        "@collection": "People",
    }
}
    {CODE-BLOCK/}

    * Here are several runs of this query, with changing starting points.  
       * `match (People as people where id() = "People/John")-recursive as path(all) {[Likes as likes]->(People as liked)}`  
         ![Recursive Query 1](images/Recursive_SingleCollectionQuery1.png "Recursive Query 1")  
         A lot can be learned even from a miniature dataset such as this. Consider for example 
         the role Fred plays in linking two otherwise unrelated network segments. The large-data-volume 
         equivalent of such a link is the discovery of relevant marketing leads, people in need of technical support, etc.  
       * `match (People as people where id() = "People/Fred")-recursive as path(all) {[Likes as likes]->(People as liked)}`  
         ![Recursive Query 2](images/Recursive_SingleCollectionQuery2.png "Recursive Query 2")  
       * `match (People as people where id() = "People/Kim")-recursive as path(all) {[Likes as likes]->(People as liked)}`  
         ![Recursive Query 3](images/Recursive_SingleCollectionQuery3.png "Recursive Query 3")  
         The small bodyless arrowhead next to the data node means that Kim likes itself.  

##Cross-Collection Recursion

To recurse through multiple collections, simply **leave the destination data-node clause empty: `()`**  
E.g. -  

* `match(_ as startingPoint where id() = "Collection1/Person1") - recursive as LikesTrail(all) {[Likes as likes]->()}`  
  This query recurses through a database of single-document profiles, and tracks the path of **likes** from a selected person on.  
  A person likes others if their IDs are in its **Likes** field.  
  The recursion checks a person's "Likes" field, retrieves profile whose IDs it contains, checks their "Likes" field, and so on.  

     |         Query part         | Role |
     | ------------------------ | ------------------------ |
     | `_ as startingPoint where id() = "Collection1/Person1"` | The query will start with this document. |
     | `- recursive as LikesTrail` | a recursive edge can be given an alias like any other edge. This one is called "LikesTrail". |
     | `(all)` | recurse through [all possible paths](../../../indexes/querying/graph/graph-queries-recursive#recursion-parameters).  (You can also choose to find the `longest` path, the `shortest` one, or be `lazy` and simply go for the first.) |
     | `{}` | The recursive edge repeatedly reinstates itself for newly found data nodes.  The edge and the data-node are therefore enveloped by curly braces, to indicate they are both in the recursion scope.|
     | `[Likes as likes]` | The field to be checked in retrieved documents. |
     | `->()` | Omitting the name of the destination data-node collection indicates it may be in any collection. |

    * A map of the "like" paths in the sample database we used:  
      ![Sample DB map](images/Recursive_CrossCollectionQuery1.png "Sample DB map")  
      Arrows point from persons to persons they like.  

    * Here's the results of trying this query with a few starting points.  

         | Query    | Results Graph |
         |:-------------:|:-------------:|
         | {CODE-BLOCK:plain} 
           match
(_ as startingPoint where id() = 
"Collection1/Person1") 
- recursive as LikesTrail(all) 
{[Likes as likes]->()} 
           {CODE-BLOCK/} | ![Recursive Query 1](images/Recursive_CrossCollectionQuery2.png "Recursive Query 1") |
         | {CODE-BLOCK:plain} 
           match
(_ as startingPoint where id() = 
"Collection2/Person4") 
- recursive as LikesTrail(all) 
{[Likes as likes]->()} 
           {CODE-BLOCK/} | ![Recursive Query 2](images/Recursive_CrossCollectionQuery3.png "Recursive Query 2") |
         | {CODE-BLOCK:plain} 
           match
(_ as startingPoint where id() = 
"Collection4/Person3") 
- recursive as LikesTrail(all) 
{[Likes as likes]->()} 
           {CODE-BLOCK/} | ![Recursive Query 3](images/Recursive_CrossCollectionQuery4.png "Recursive Query 3") |

{PANEL/}

## Related Articles

**Querying**:  
[RQL](../../../indexes/querying/what-is-rql#querying-rql---raven-query-language)  
[Indexes](../../../indexes/what-are-indexes#what-indexes-are)  

##Graph Querying**
[Overview](../../../indexes/querying/graph/graph-queries-overview#graph-querying-overview)  
[Basic Graph Queries](../../../indexes/querying/graph/graph-queries-basic#basic-graph-queries)  
[Recursive Graph Queries](../../../indexes/querying/graph/graph-queries-recursive#recursive-graph-queries)  
