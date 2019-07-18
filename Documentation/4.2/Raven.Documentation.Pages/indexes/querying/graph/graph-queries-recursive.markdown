# Recursive Graph Queries  
---

{NOTE: }

Use a recursive graph query to repeat a search pattern within a chosen collection or 
throughout your collections, until a condition is met.  

* In this page:  
   * [Recursive Graph Queries](../../../indexes/querying/graph/graph-queries-recursive#recursive-graph-queries)  
      * [Inter-Collection Recursion](../../../indexes/querying/graph/graph-queries-recursive#inter-collection-recursion)  
      * [Cross-Collection Recursion](../../../indexes/querying/graph/graph-queries-recursive#cross-collection-recursion)  

{NOTE/}

---

{PANEL: Recursive Graph Queries}

As a recursive function repeatedly calls itself until its objective is fulfilled, 
a recursive query can repeat a search pattern until the search is concluded.  
You can use recursive queries to ease tasks like locating a system's smallest component 
or an organization's full "chain of command", mapping a stucture, and countless others.  

####Parameters

* `all` - query all paths to produce the full list of paths that match it.  
* `longest` - query the longest matching path, e.g. to locate the farthest retailer from a manufacturer.  
* `shortest` - query the shortest matching path, e.g. [a useful example]  
* `lazy` - query for the first matching path  

####Inter-Collection Recursion
A recursive query's syntax resembles that of a non recursive one, except for the recursive condition 
defined in its edge clause. When the query is executed, the edge is repeatedly reinstated between newly 
founded nodes until the queried pattern is fulfilled.  

The following query remains the same, except for its changing initial node.  
It's interesting to see the effect each participant has on the entire network.  
Without Mimi in the net for example, many participants that like nobody or only themselves, are disconnected.  
And then there's Kim, who even when disconnected from all others still has himself to like.  
Locating patterns like these may help in various implementations, from marketing to research.  

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

####Cross-Collection Recursion

You can implement recursive queries through multiple collections.  
The following path for example traces users by **like**, across four collections.  
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

## Related Articles
**Client Articles**:  
[Query](../../../../server/ongoing-tasks/backup-overview)  
[Graph Query](../../../../client-api/operations/maintenance/backup/backup)  
[Recursion](../../../../client-api/operations/maintenance/backup/restore)  

**Studio Articles**:  
[Creating a query](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
[Seeing query results](../../../../studio/server/databases/create-new-database/from-backup)  


