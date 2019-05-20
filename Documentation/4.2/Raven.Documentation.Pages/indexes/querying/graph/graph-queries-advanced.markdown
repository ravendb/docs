# Graph Querying: Overview
---

{NOTE: }

* Use a recursive graph query to repeat a search pattern within a chosen collection or 
  throughout your collections, until a condition is met.  
* Use an intersection to retrieve data only when a whole set of conditions is met.

* In this page:  
   * [Recursive graph queries](../../../indexes/querying/graph/graph-queries-advanced#recursive-graph-queries)  
      * [Recursion within a single collection](../../../indexes/querying/graph/graph-queries-advanced#recursion-within-a-single-collection)  
      * [Cross-collection recursion](../../../indexes/querying/graph/graph-queries-advanced#cross-collection-recursion)  
   * [Intersection](../../../indexes/querying/graph/graph-queries-advanced#intersection)  

{NOTE/}

---

{PANEL: Recursive graph queries}

As a recursive function repeatedly calls itself until its objective is fulfilled, 
a recursive query repeats a search pattern until the search is concluded.  
You can use recursive queries to ease tasks like locating a system's smallest component, 
finding an organization's full "chain of command", mapping a stucture, and countless others.  

Recursive queries can search through your documents and across your collections.  

examples:

This query shows everybody in the customers database:
match(Customers as fromCustomer)-[Customers as sellsTo]->(Customers as toCustomer)

This query shows Tim and his two customers, Po and Harry:
match(Customers as fromCustomer where id() = "Customers/Tim")-[Customers as sellsTo]->(Customers as toCustomer)

This query starts with Tim and shows everything hereafter:
match(Customers as fromCustomer where id() = "Customers/Tim")-[Customers as sellsTo]->(Customers as toCustomer)

This query shows the shortest paths available:
match(Customers as fromCustomer)
-recursive as pathOfCustomers(shortest)
{[Customers as sellsTo]
->(Customers as toCustomer)}
select {tryThis : pathOfCustomers.map(x => x.sellsTo) }

this one (from michael's page) does work:
match (Employees as employee where id() = 'employees/6-A')
    -recursive as chainOfManagers
        { [ReportsTo as reportsTo]  
            ->(Employees as manager)} 
select { managerIdsAsPath : chainOfManagers.map(x => x.reportsTo) }

####Syntax

####Parameters

* `all` - query all paths to produce the full list of paths that match it.  
* `longest` - query the longest matching path, e.g. to locate the farthest retailer from a manufacturer.  
* `shortest` - query the shortest matching path, e.g. [a useful example]  
* `lazy` - query for the first matching path  

####recursion within a single collection
You can recurse through a collection's documents in search of a pattern.  
The syntax of a recursive search resembles that of a non recursive one,  
except that its edge defines the recursive condition.  
When such a query is executed, the edge is repeatedly reinstated between 
newly founded nodes, until the query pattern is fulfilled.  

The following query remains the same, except for its changing initial node.  
It's interesting to see the effect each participant has on the entire network.  
Without Mimi in the net for example, many participants that like nobody or only themselves, are disconnected.  
And there are participants like Kim, that even when disconnected from all others still have themselves to like.  
Besides saddening and amusing testimonies regarding human nature, understanding and being able to locate 
such patterns may be of great help for all kinds of implementations, from marketing to research.  

     | Query    | Results Graph |
     |:-------------:|:-------------:|
     | {CODE-BLOCK:plain} 
       match(People as people where id() = "People/John")
       -recursive as path(all) 
       {[Likes as likes]
       ->(People as liked)} 
       {CODE-BLOCK/} | ![Recursive Query 1](images/Recursive_SingleCollectionQuery1.png "Recursive Query 1") |
     | {CODE-BLOCK:plain} 
       match(People as people where id() = "People/Fred")
       -recursive as path(all) 
       {[Likes as likes]
       ->(People as liked)}
       {CODE-BLOCK/} | ![Recursive Query 2](images/Recursive_SingleCollectionQuery2.png "Recursive Query 2") |
     | {CODE-BLOCK:plain} 
       match(People as people where id() = "People/Kim")
       -recursive as path(all) 
       {[Likes as likes]
       ->(People as liked)}
       {CODE-BLOCK/} | ![Recursive Query 3](images/Recursive_SingleCollectionQuery3.png "Recursive Query 3") |

####Cross-collection recursion

You can implement recursive queries through multiple collections.  
E.g., this is the course of users that like each other within a multi-collection database:  
![Cross-collection recursive query plan](images/Recursive_CrossCollectionQuery1.png)  

To follow this course, we can use a query such as this:  
`match(_ as @all_docs where id() = "Y/4")-recursive as rec(all) {[Likes as likes]->()}`  
  
`_ as @all_docs` - recurse through docs that match the pattern  
`where id() = "Y/4"` - id() fetches the current document's ID, and hands it to a standard `where` evaluation.  
`-recursive as rec` - the edge is defined as recursion and is given a name as edges normally do  
`(all)` - recurse through all paths continuing the source node  
`{}` - Place the group to be recursed through within curly brackets  
`[Likes as likes]` - in this case the query recurses through documents with IDs in their "Likes" field  
`->()` - "()" means "all collections", since The query is meant to recurse through various collections  

Here's the same query, where only the starting node changes.  

     | Query    | Results Graph |
     |:-------------:|:-------------:|
     | {CODE-BLOCK:plain} 
       match(_ as @all_docs where id() = "x/1")
       -recursive as rec(all) 
       {[Likes as likes]->()} 
       {CODE-BLOCK/} | ![Recursive Query 1](images/Recursive_CrossCollectionQuery2.png "Recursive Query 1") |
     | {CODE-BLOCK:plain} 
       match(_ as @all_docs where id() = "y/4")
       -recursive as rec(all) 
       {[Likes as likes]->()} 
       {CODE-BLOCK/} | ![Recursive Query 2](images/Recursive_CrossCollectionQuery3.png "Recursive Query 2") |
     | {CODE-BLOCK:plain} 
       match(_ as @all_docs where id() = "h/3")
       -recursive as rec(all) 
       {[Likes as likes]->()} 
       {CODE-BLOCK/} | ![Recursive Query 3](images/Recursive_CrossCollectionQuery4.png "Recursive Query 3") |

{PANEL/}

{PANEL: Intersection}
An intersection is where multiple datasets match the same search pattern.  
A query may for instance include two parts separated by a condition: an OR or an AND.  
Each section is handled as if it were a search on its own, and the found datasets are indexed.  
When the two datasets are prepared, the search is concluded using the given condition.  
Whatever data remains, is the intersection.  

For a search after shoes with **color="green"** AND **proximity<20"** AND **price<40** for example, 
a 20$ green pair of shoes sold at a store 10 miles away will be in the intersection of the three datasets.  

There are in general three types of queries, all implemented within graph queries.
a lucene query - which may remain the only query (when it is performed over data that requires no indexing)
 example - the first () query part
a query over indexed data (that's been queried using lucene and then indexed)
a query implemented by the graph mechanism (not assisted by lucene)
 example - in an EDGE clause

The `intersect` keyword
you can use `intersect` as you would other RQL words, to match an entire set of parameters.  
This is equivalent to using a series of AND or OR conditions.  
Implement it [as you would using non-graph queries](../../../indexes/querying/intersection), providing the list of conditions 
in a `where intersect` clause.  

E.g., a query that retrieves the profiles of devices that **resemble Iphone 6** and **are black**, 
and displays their relations to the devices they are similar to.  
{CODE-BLOCK:plain}
match 
(Devices as devices where intersect(SimilarTo = "Devices/Iphone6", Color = "Black"))-[SimilarTo as similarTo]->(Devices as similar)
{CODE-BLOCK/}
![Cross-collection recursive query plan](images/Intersection.png)  

{PANEL/}

## Related Articles
**Client Articles**:  
[Query](../../../../server/ongoing-tasks/backup-overview)  
[Graph Query](../../../../client-api/operations/maintenance/backup/backup)  
[Recursion](../../../../client-api/operations/maintenance/backup/restore)  

**Studio Articles**:  
[Creating a query](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
[Seeing query results](../../../../studio/server/databases/create-new-database/from-backup)  


