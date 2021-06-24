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
     * [Deployment Concurrency and Order](../indexes/rolling-index-deployment#deployment-concurrency-and-order)  
  * [Setting Indexing Deployment Mode](../indexes/rolling-index-deployment#setting-indexing-deployment-mode)  
     * [Server-Wide Deployment Mode](../indexes/rolling-index-deployment#server-wide-deployment-mode)  
     * [Database Deployment Mode](../indexes/rolling-index-deployment#database-deployment-mode)  
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
2. When the assigned node finishes indexing, it sends a cluster-wide confirmation command
   that indexing is done.  
3. The cluster assigns indexing to the next node.  
   {WARNING: }
   If the delivery of an **indexing completion confirmation** fails when the current node 
   finishes indexing, no other node will be able to start indexing until the confirmation 
   succeeds or indexing is initiated manually.  
   Confirmation delivery may fail, for example, due to forceful disconnection of the indexing 
   node or cluster leader node during indexation.  
   {WARNING/}

---

### Deployment Concurrency and Order

* **Deployment Concurrency**  
   * A node is assigned to index a database, only if no other node currently indexes this database.  
   * Multiple nodes **can** be assigned to concurrently index **different databases**.  
     E.g., node `A` can index the "Integration" database, while node `B` indexes the "Production" database.  

* **Deployment Order**  
  * Deployment order is determined by the cluster.  
  * Indexing is deployed in the reverse order of nodes membership in the database group.  
    Nodes that are currently in [Rehab or Promotable state](../server/clustering/distribution/distributed-database#database-topology) 
    are given a lower priority.  

{PANEL/}

{PANEL: Setting Indexing Deployment Mode}

---

### Server-Wide Deployment Mode

 Deployment mode can be set server-wide using [configuration options](../server/configuration/configuration-options#json).  
 Setting the server-wide configuration option will apply to all databases on all nodes.  

* [Auto Indexes](../indexes/creating-and-deploying#auto-indexes) Deployment Mode  
  Select a deployment mode for indexes created automatically using the `Indexing.Auto.DeploymentMode` configuration option.  
  `"Indexing.Auto.DeploymentMode": "Rolling"`  
  `"Indexing.Auto.DeploymentMode": "Parallel"`  

* [Static Indexes](../indexes/creating-and-deploying#static-indexes) Deployment Mode  
  Select a deployment mode for static indexes using the `Indexing.Static.DeploymentMode` configuration option.  
  `"Indexing.Static.DeploymentMode": "Rolling"`  
  `"Indexing.Static.DeploymentMode": "Parallel"`  
    
---
### Database Deployment Mode

Enable or disable rolling for a specific database using database configuration keys.  
Setting these properties overrides the 
[Server-Wide](../indexes/rolling-index-deployment#server-wide-deployment-mode) default.  
 
* From Studio:  

    ![Database Configuration Keys](images/rolling-index-deployment-01.png "Database Configuration Keys")

      1. Open **Settings** > **Database Settings** view.  
      2. **Filter Keys** - Enter a search string to locate the configuration keys.  
      3. **Edit** - Click to edit values (see next image for details).  
      4. **Configuration Keys** -  
         `Indexing.Auto.DeploymentMode` - Deployment mode configuration key for Auto Indexes.  
         `Indexing.Static.DeploymentMode` - Deployment mode configuration key for Static Indexes.  
      5. **Effective Value** - The current configuration.  
      6. **Origin** - The origin of the current configuration.  
         Can be - Default | Database  

    ![Edit Values](images/rolling-index-deployment-02.png "Edit Values")

      1. **Override** - Toggle to override the server-wide configuation.  
      2. **Edit Value** - Select Parallel or Rolling indexing deployment mode.  
      3. **Set Default** - Click 'Set Default' to select the server-wide default value.  
      4. **Save** - Apply changes.  
         {WARNING: }
         An edited configuration key value will become effective only after the database is reloaded.  
         {WARNING/}
    
---

### Deployment Mode in an Index Definition

Enable or disable rolling for a specific index using the index-definition `DeploymentMode` property.  
Setting this property overrides [server-wide](../indexes/rolling-index-deployment#server-wide-deployment-mode) 
and [database](../indexes/rolling-index-deployment#database-deployment-mode) settings configuration.  
  
  * `DeploymentMode = IndexDeploymentMode.Rolling`  
  * `DeploymentMode = IndexDeploymentMode.Parallel`  
  
    {INFO: }
    The deployment mode can be set for a specific index when, for example, parallel indexing 
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
