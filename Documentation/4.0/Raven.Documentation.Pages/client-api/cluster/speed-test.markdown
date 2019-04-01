# Cluster: Speed Test
---

{NOTE: }

* In RavenDB Client API, if the [Read Balance Behavior](../../client-api/configuration/load-balance-and-failover) is configured for the _Fastest Node_, 
  then under certain conditions, the client executes a `Speed Test` for each node in the cluster so that the fastest node can be accessed for ***Read*** requests.  

* When doing a `Speed Test`, the client checks the response time from all the nodes in the topology.  
  This is done per 'Read' request that is executed.  

* Once the Speed Test is finished, the client stores the fastest node found.  
  After that, the speed test will be repeated every minute.  
{NOTE/}

---

{PANEL: When does the Speed Test Trigger?}

The Speed Test is triggered in the following cases:

* When the client configuration has changed to `FastestNode`  
  Once the client configuration is updated on the server, the next response from the server to the client will include the following header: `Refresh-Client-Configuration`.  
  When the client sees such a header for the first time, it will start the Speed Test - if indeed configuration is set to _FastestNode_.  

* Every 5 minutes the client checks the server for the current nodes' topology.  
   At this periodic check, the Speed Test will be triggered if _FastestNode_ is set.  

* Any time when the nodes' topology changes, and again - only if _FastestNode_ is set.  
{PANEL/}

## Related articles

### Cluster

- [Clustering Overview](../../server/clustering/overview)
- [How a Client Integrates with Replication and the Cluster](../../client-api/cluster/how-client-integrates-with-replication-and-cluster)
- [Cluster Node Health Check](../../client-api/cluster/health-check)

### Configuration

- [Load Balance & Failover](../../client-api/configuration/load-balance-and-failover)
- [Requests Configuration in Studio](../../studio/server/client-configuration)
