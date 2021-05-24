# Indexes: Rolling Index Deployment
---

{NOTE: }

* Rolling Index Deployment is the deployment of indexing gradually, one cluster node at a time, 
  to ensure the cluster's availability and performance.  
* While a node performs indexing, the cluster may reassign its tasks to other nodes if needed.  

* In this page:  
  * [](../indexes/using-analyzers#understanding-analyzers)  
  * [](../indexes/using-analyzers#ravendb)  
  * [](../indexes/using-analyzers#full-text-search)  

{NOTE/}

---

{PANEL: Why Rolling Index Deployment?}

Indexing is meant to improve performance. But heavy-duty indexing may become an obsticle to 
the cluster's performance and availability, if all its nodes are busy indexing at the same time.  

* On Site, the cost of concurrent indexing in memory and CPU usage may decrease cluster performance.  
* On the Cloud, concurrent indexing may exhaust all nodes’ credits at the same time and harm cluster 
  availability.  

Exempting most cluster nodes from indexing ensures that indexing **will** be performedm, but not 
on the expense of performance or availability.  

{PANEL/}

{PANEL: }
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
