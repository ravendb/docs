# Indexes: Rolling Index Deployment
---

{NOTE: }

* When **Rolling Index Deployment** is enabled, databases are indexed by one node at a time.  
* Indexing operations are assigned to nodes by the cluster.  
* The cluster assigns a new node with indexing, only when the previous indexing node confirmed 
  that it finished indexing.  
* Rolling index deployment prevents parallel heavy-duty indexing by multiple nodes, which may 
  reduce the cluster's availability and performance.  

* In this page:  
  * [Why Rolling Index Deployment](../indexes/rolling-index-deployment#why-rolling-index-deployment)  
  * [How Does It Work](../indexes/rolling-index-deployment#how-does-it-work)  
  * [Set Rolling Index Deployment Mode](../indexes/rolling-index-deployment#set-rolling-index-deployment-mode)  

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

### The Rolling Procedure

Nodes are assigned with indexing tasks one node at a time.  

1. The cluster assigns indexing to one of its nodes.  
2. When the assigned node finishes indexing, it send the cluster leader a confirmation 
   that indexing is **Done**.  
   {WARNING: }
   If confirmation delivery fails, e.g. because the indexing node or the cluster leader node 
   has been disconnected from the cluster, indexing will pend for all nodes until the cluster 
   recovers or indexing is initiated manually.  
   {WARNING/}
3. The cluster leader assigns indexing to the next node.  
4. And so on, until all nodes finish indexing.  

### Rolling Index Deployment Scope

Rolling index deployment is performed **per database**.  
If the "Integration" database, for example, needs indexing, it may be indexed by 
node `C`, then by node `A`, and finally by node `B`.  

{INFO: }
Rolling order is determined by the cluster.  
(Node `A` is **not** necessarily always the first.)  
{INFO/}

{INFO: }
Indexing **can** be performed in parallel **for different databases** even 
when Rolling Index Deployment is enabled.  
The "Integration" database, for example, can be indexed by node `A`, 
while the "Production" database is indexed by node `B`.  
{INFO/}

{PANEL/}

{PANEL: Set Rolling Index Deployment Mode}

Rolling index deployment can be **enabled** or **disabled** for 
[Auto](../indexes/creating-and-deploying#auto-indexes) and 
[Static](../indexes/creating-and-deploying#static-indexes) indexes.  

* **Auto Indexes Deployment Mode**  
  Enable or Disable auto Indexes rolling deployment using the `Indexing.Auto.DeploymentMode` 
  [configuration option](../server/configuration/configuration-options#json).  
  * To **Enable** rolling: "Indexing.Auto.DeploymentMode": "Rolling"  
  * To **Disable** rolling: "Indexing.Auto.DeploymentMode": "Parallel"  
* **Static Indexes Deployment Mode**  
  Enable or Disable static indexes rolling deployment using the `Indexing.Auto.DeploymentMode` 
  [configuration option](../server/configuration/configuration-options#json).  
  * To **Enable** rolling: "Indexing.Static.DeploymentMode": "Rolling"
  * To **Disable** rolling: "Indexing.Static.DeploymentMode": "Parallel"
* **Override Static Index Deployment Mode Configuration**  
  To choose a deployment mode for a specific index that will override 
  the `Indexing.Static.DeploymentMode` configuration option, use 
  `DeploymentMode` in the index definition.  
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

- [Boosting](../indexes/boosting)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Dynamic Fields](../indexes/using-dynamic-fields)

### Studio
- [Custom Analyzers](../studio/database/settings/custom-analyzers)  
- [Create Map Index](../studio/database/indexes/create-map-index)  
