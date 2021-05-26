# Indexes: Rolling Index Deployment
---

{NOTE: }

* When **Rolling Index Deployment** is enabled, indexing a database is performed by one node at a time.  
* Rolling index deployment prevents parallel indexing by multiple nodes, to ensure that most cluster resources 
  would always be available for data and task procession.  
* Indexing operations are assigned to nodes by the cluster.  
* The cluster will assign a node with indexing, only when the node it had previously assigned 
  with indexing confirms that it finished indexing.  


* In this page:  
  * [Why Rolling Index Deployment](../indexes/rolling-index-deployment#why-rolling-index-deployment)  
  * [How Does It Work](../indexes/rolling-index-deployment#how-does-it-work)  
     * [The Rolling Procedure](../indexes/rolling-index-deployment#the-rolling-procedure)  
     * [Deployment Scope and Course](../indexes/rolling-index-deployment#deployment-scope-and-course)  
  * [Setting Indexing Deployment Mode](../indexes/rolling-index-deployment#setting-indexing-deployment-mode)  
     * [System-Wide Deployment Mode](../indexes/rolling-index-deployment#system-wide-deployment-mode)  
     * [Deployment Mode in an Index Definition](../indexes/rolling-index-deployment#deployment-mode-in-an-index-definition)  

{NOTE/}

---

{PANEL: Why Rolling Index Deployment}

When heavy-duty indexing is performed in parallel by all cluster nodes, the cluster's 
performance and availability may be reduced.  
The extent of the reduction in cluster conduct depends upon its nodes' resources, 
the scope of the required indexing, and the number of nodes concurrently indexing.  

* **On Site**, dedicating much of all nodes' resources to indexing rather than to processing 
  data and tasks may reduce the cluster's performance.  
* **On the Cloud**, parallel indexing may exhaust the credits available to multiple nodes 
  at the same time and degrade the cluster's availability.  

**Rolling index deployment** ensures that indexes will be created and updated while 
the cluster remains fully available and performant.  

{INFO: }
Parallel indexing may be a better option when there is minor or no database activity.  
{INFO/}

{PANEL/}

{PANEL: How Does It Work}

---

### The Rolling Procedure

Nodes are assigned with the indexing of each database in a linear order, one node at a time.  

1. The cluster assigns indexing to one of its nodes.  
2. When the assigned node finishes indexing, it send the cluster leader a confirmation 
   that indexing is done.  
3. The cluster leader assigns indexing to the next node.  
   {WARNING: }
   If the delivery of an **indexing completion confirmation** fails, e.g. because the 
   sending (indexing) or receiving (cluster leader) node has been forcefully disconnected 
   from the cluster, **indexing will pend** for all nodes until the cluster recovers or 
   indexing is initiated manually.  
   {WARNING/}

---

### Deployment Scope and Course

* **Rolling Scope**  
  The cluster maintains an **independent indexing deployment course** for each databases.  
* **Rolling Order**  
  Rolling order is determined by the cluster (node `A` **not** necessarily always being the first).  

The implication of this design is that different nodes **can** index different databases concurrently.  

* E.g. -  
   * The indexing deployment course the cluster chose for the **"Integration"** database, is:  
     indexing by node `B`, then by node `C`, and finally by node `A`.  
   * The indexing deployment course the cluster chose for the **"Production"** database, is:  
     indexing by node `C`, then by node `A`, and finally by node `B`.  
   * Assuming that the cluster has started indexing the two databases at the same time,  
     node `B` will be indexing "Integration" while node `C` indexes "Production".  

{PANEL/}

{PANEL: Setting Indexing Deployment Mode}

---

### System-Wide Deployment Mode

 Deployment mode can be chosen system-wide using [configuration options](../server/configuration/configuration-options#json).  

* [auto Indexes](../indexes/creating-and-deploying#auto-indexes) Deployment Mode  
  Choose a deployment mode for indexes created automatically using the `Indexing.Auto.DeploymentMode` configuration option.  
  `"Indexing.Auto.DeploymentMode": "Rolling"`  
  `"Indexing.Auto.DeploymentMode": "Parallel"`  

* [static Indexes](../indexes/creating-and-deploying#static-indexes) Deployment Mode  
  Choose a deployment mode for static indexes using the `Indexing.Static.DeploymentMode` configuration option.  
  `"Indexing.Static.DeploymentMode": "Rolling"`  
  `"Indexing.Static.DeploymentMode": "Parallel"`  
    
---

### Deployment Mode in an Index Definition

Enable or disable rolling for a specific index using the index-definition `DeploymentMode` property.  
Setting this property overrides default and configuration-option settings.  
  
  * `DeploymentMode = IndexDeploymentMode.Rolling`  
  * `DeploymentMode = IndexDeploymentMode.Parallel`  
  
    {INFO: }
    The deployment mode can be chosen for a specific index when, for example, parallel indexing 
    is preferred in general but rolling is a better option for a particularly "weighty" index.  
    {INFO/}

  {CODE-BLOCK:csharp}
  private class MyRollingIndex : AbstractIndexCreationTask<Order>
  {
      public MyRollingIndex()
      {
          Map = orders => from order in orders
          select new
          {
              order.Company,
          };
          DeploymentMode = IndexDeploymentMode.Rolling;
      }
  }
  {CODE-BLOCK/}

{PANEL/}

## Related Articles

### Indexes
- [auto Indexes](../indexes/creating-and-deploying#auto-indexes)  
- [static Indexes](../indexes/creating-and-deploying#static-indexes)  

### Server
- [configuration options](../server/configuration/configuration-options#json)  
