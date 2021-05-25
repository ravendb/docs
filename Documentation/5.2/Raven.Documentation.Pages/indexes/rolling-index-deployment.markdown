# Indexes: Rolling Index Deployment
---

{NOTE: }

* Rolling Index Deployment is the gradual deployment of indexing, one cluster node at a time, 
  to ensure the cluster's availability and performance.  
* While a node performs indexing, the cluster may reassign its tasks to other nodes.  

* In this page:  
  * [](../indexes/using-analyzers#understanding-analyzers)  
  * [](../indexes/using-analyzers#ravendb)  
  * [](../indexes/using-analyzers#full-text-search)  

{NOTE/}

---

{PANEL: Why Rolling Index Deployment}

Parallel heavy-duty indexing by all cluster nodes may reduce the cluster's performance and availability.  

* **On Site**, the cluster's ability to process data and tasks is reduced when multiple nodes dedicate 
  their resources to indexing.  
* **On the Cloud**, parallel indexing may exhaust the credits of multiple nodes at the same time, and 
  degrade the cluster's availability.  

Deploying indexing one node at a time ensures that indexing will be performed, but not on the expense 
of overall cluster conduct.  


{PANEL/}

{PANEL: How Does It Work}

When **Rolling Index Deployment** is enabled, indexing operations are performed by one node at a time.  

### The Rolling Procedure

1. The cluster assigns indexing to one of its nodes.  
2. When the assigned node finishes indexing, it informs the cluster leader that indexing is **Done**.  
   {Warning: }
   If the assignee's confirmation fails to reach the cluster leader, indexing will be stopped for all nodes 
   until the cluster recovers or indexing is restarted manually.  
   This may happen, for example, when the assignee or the cluster leader are disconnected from the cluster 
   while indexing takes place.  
   {Warning/}
3. The cluster leader assigns indexing to the next node.  
4. And so on, until all nodes finish indexing.  

{INFO: Rolling is performed **Per-Database**.}

* indexing is performed one node at a time for each database.  
  I.e., if indexes of the "Integration" database need to be updated, they will be updated as described above, one node at a time.  
* Indexing **can** be performed in parallel for different databases.  
  Node `A`, for example, may create indexes for the "Integration" database, while node `B` creates indexes for the "Production" database.  
{INFO/}

* Rolling Index Delopyment can be enabled or disabled for [Auto Indexes]().  
* Rolling Index Delopyment can be enabled or disabled for [Static Indexes]().  

  {INFO: }
  Times in which you may consider using concurrent indexing and not rolling index deployment, 
  include time in which there is minor or no database activity, e.g. in time of backup restore, 
  when concurrent indexing poses no problem to clients.  
  {INFO/}

{PANEL/}

{Rolling Index Deployment Settings}

* settings.json option:  
        Indexing.Static.DeploymentMode
        Indexing.Auto.DeploymentMode

* API Samples  
   * Adding an index that uses rolling  
{CODE:csharp:}
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
{CODE/}

   * Adding an index that uses parallel indexing  

* Studio Configuration]()  

{PANEL/}


* Studio --> List of Indexes
  change an index
  view deployment
* Studio --> Database Record -> indexes -> "Rolling:": true




3-parts picture, in each one node inxes and the others don't

When shouldn't it be used

how to configure it
sample

link to configurations (settings.json)


more points:
the order of rolling is undetermined
how much indexing is done before the queue is rolled to the next node?
{PANEL}

## Related Articles

### Indexes

- [Boosting](../indexes/boosting)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Dynamic Fields](../indexes/using-dynamic-fields)

### Studio
- [Custom Analyzers](../studio/database/settings/custom-analyzers)  
- [Create Map Index](../studio/database/indexes/create-map-index)  
