# Graph Queries Frequently Asked Questions  

---

{NOTE: }

* In this page:  
   * [When should or shouldn't I use graph queries?](../../../indexes/querying/graph/graph-queries-FAQ#q-when-should-or-shouldnt-i-use-graph-queries)  
   * [What Are Hops And Paths?](../../../indexes/querying/graph/graph-queries-FAQ#q-what-are-hops-and-paths)  
   * [What Is The Flow Of Graph Queries?](../../../indexes/querying/graph/graph-queries-FAQ#q-what-is-the-flow-of-graph-queries)  
   
{NOTE/}

---

{PANEL: FAQ}

####Q: When should or shouldn't I use graph queries?  

There are configurations and situations for which graph querying is an optimal solution, and circumstances 
that invite different approaches.  

* **Use graph querying when -**  
   * **The potential of your intricate and layered data remains hidden**, and it may even become a nuisance.  
     For graph querying, webs of relations are not a wasteland but a natural habitat. Try them 
     if it's likely that your gathering data has much more to offer than can currently be seen.  
     You can, for example, collect data from a web of relations: dynamically build user profiles, 
     product pages, vendor data sheets and so on, using graph queries that collect data related 
     to them in the first degree, second degree, third degree and so on.  

   * **You look for optimized paths**.  
     As their [history](../../../indexes/querying/graph/graph-queries-overview#introduction-to-graph-modeling) suggests, 
     graph queries are awesome in finding **optimal paths** between related nodes. Graph-using applications may also find 
     the fastest way to a suitable host, or the quickest publicity route to a destination audience.  

* Graph querying may **not** be an ideal solution when -  
   * **You can achieve the same results with standard RQL.**  
     Non-graph RQL uses less resources than graph queries. 
     Prefer standard queries over graph queries if using graph queries give you no clear advantage over it.  
   * **Your documents are isolated from each other** by structure or preference.  
     Graph queries are great in harnessing the relations between documents to your aid.  
     If there are no such relations, there may be no point in using graph queries.  
   * **Your data is pre-arranged and pre-indexed**, requiring no ongoing relation queries to refurnish its contents.  
   * **A different model has a clear advantage**, e.g. key/value store for key/value customer lists or SQL for fixed tables.  
   * **Your query starts with a broad search.**  
     Graph queries work best when the search begins with a definite starting point and lays out a path from there on.  

---

####Q: What Are Hops And Paths?  

In the context of graphs, the terms "hop" and "path" are often used to describe 
the way and distance between data nodes. A **hop** is a direct jump from one node to 
another, while a **path** is a sequence of hops between two nodes.  

![a two-hops Path](images/Overview_Path.png "a two-hops Path")

Graph queries and their results are often planned and evaluated in terms of paths. 
A [recursive query](../../../indexes/querying/graph/graph-queries-recursive#paths) for example, 
may locate paths to user profiles by various criteria.  

---

####Q: What Is The Flow Of Graph Queries?  

There are two main phases to the flow of a graph query:  

* **Phase 1: Indexing**  
  When a graph query is executed, RavenDB queries its data nodes first.  
  Resulting Lucene and Collection queries are indexed, and the indexes are used by the graph engine.  
* **Phase 2: Handling Relations**  
  If the query includes edges, the graph engine uses them now to fathom relations between elements while 
  it goes through the indexes prepared during the first phase.  

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
[Multi-Section Search Patterns](../../../indexes/querying/graph/graph-queries-multi-section#graph-queries-multi-section-search-patterns)  
[Recursive Graph Queries](../../../indexes/querying/graph/graph-queries-recursive#recursive-graph-queries)  
