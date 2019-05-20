# RavenDB Graph Queries -vs- Other Graph Queries  

---

{NOTE: }

{NOTE: }

xxx.  

* **xxx**  
  xxx.  
* **xxx**  
  xxx.  
* **xxx**  
  xxx.  

* In this page:  
   * [RQL Graph-queries and RQL Document-queries](../../../indexes/querying/graph/graph-queries-vs-other-queries#rql-graph-queries-and-rql-document-queries)  
   * [RQL Graph-queries and Cypher](../../../indexes/querying/graph/graph-queries-vs-other-queries#rql-graph-queries-and-cypher)  
   * [RQL Graph-queries and OrientDB](../../../indexes/querying/graph/graph-queries-vs-other-queries#rql-graph-queries-and-orientdb)  
{NOTE/}

---

{PANEL: RQL graph queries -vs- other graph queries}

####RQL Graph-queries and RQL Document-queries
RavenDB determines automatically whether this is a graph query or not.  
If it is, RavenDB presents a "Graph" tab in the Studio query-results window.

* e,g,  
  `match (Orders as o)` **will**  produce a graph.  
  `from Orders as o select o` will **not** produce a graph.  

####RQL Graph-queries and Cypher

####RQL Graph-queries and OrientDB

{PANEL/}

--

## Related Articles
**Client Articles**:  
[Query](../../../../server/ongoing-tasks/backup-overview)  
[Graph Query](../../../../client-api/operations/maintenance/backup/backup)  
[Recursion](../../../../client-api/operations/maintenance/backup/restore)  

**Studio Articles**:  
[Creating a query](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
[Seeing query results](../../../../studio/server/databases/create-new-database/from-backup)  


