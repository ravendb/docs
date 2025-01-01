# Client Integration with the Cluster
---

{NOTE: }

* In this page:
  * [Failover behavior](../../client-api/cluster/how-client-integrates-with-replication-and-cluster#failover-behavior)
  * [Cluster topology in the client](../../client-api/cluster/how-client-integrates-with-replication-and-cluster#cluster-topology-in-the-client)
  * [Topology discovery](../../client-api/cluster/how-client-integrates-with-replication-and-cluster#topology-discovery)
  * [Configuring topology nodes](../../client-api/cluster/how-client-integrates-with-replication-and-cluster#configuring-topology-nodes)
  * [Write assurance and database groups](../../client-api/cluster/how-client-integrates-with-replication-and-cluster#write-assurance-and-database-groups)

{NOTE/}

---

{PANEL: Failover behavior}

* In RavenDB, the replication is _not_ a bundle and is always enabled if there are two nodes or more in the cluster. 
  This means that the failover mechanism is always turned on by default.  

* The client contains a list of cluster nodes per database group.  
  Each time the client needs to do a request to a database, it will choose a node that contains this database from this list to send the request to. 
  If the node is down and the request fails, it will select another node from this list.  

* The choice of which node to select depends on the `ReadBalanceBehavior` and `LoadBalanceBehavior` configuration values.
  For more information about the different values and the node selection process, see [Load balancing client requests](../../client-api/configuration/load-balance/overview). 
      
     {NOTE: }
     Each failure to connect to a node spawns a health check for that node.  
     For more information see [Cluster Node Health Check](health-check).
     {NOTE/}

{PANEL/}

{PANEL: Cluster topology in the client}

When the client is initialized, it fetches the topologies and populates the nodes list for the load-balancing and failover functionality.
During the lifetime of a RavenDB Client object, it periodically receives the cluster and the databases topologies from the server.  

---

The **topology** is updated with the following logic:

* Each topology has an etag, which is a number
* Each time the topology has changed, the etag is incremented
* For each request, the client adds the latest topology etag it has to the header
* If the current topology etag at the server is higher than the one in the client, the server adds `"Refresh-Topology: true"` to the response header
* If a client detects the `"Refresh-Topology: true"` header in the response, the client will fetch the updated topology from the server.
  Note: if `ReadBalanceBehavior` `FastestNode` is selected, the client will schedule a speed test to determine the fastest node.
* In addition, every 5 minutes, the client fetches the current topology from the server if no requests are made within that time frame.

---

The **client configuration** is handled in a similar way:

* Each client configuration has an etag attached
* Each time the configuration has changed at the server-side, the server adds `"Refresh-Client-Configuration"` to the response
* When the client detects the aforementioned header in the response, it schedules fetching the new configuration

{PANEL/}

{PANEL: Topology discovery}

In RavenDB, the cluster topology has an etag that increments with each topology change.

#### How and when the topology is updated:

* The first time any request is sent to RavenDB server, the client fetches cluster topology
* Each subsequent request happens with a fetched topology etag in the HTTP headers, under the key `Topology-Etag`
* If the response contains the `Refresh-Topology: true` header, then a thread responsible for updating the topology will be spawned

{PANEL/}

{PANEL: Configuring topology nodes}

Listing any node in the initialization of the cluster in the client is enough to be able to properly connect to the specified database. 
Each node in the cluster contains the full topology of all databases and all nodes that are in the cluster.
Nevertheless, it is possible to specify multiple node urls at the initialization. But why list multiple nodes in the cluster, if url of any cluster node will do?

By listing multiple nodes in the cluster, we can ensure that if a single node is down and we bring a new client up, we'll still be able to get the initial topology.
If the cluster sizes are small (three to five nodes), we'll typically list all the nodes in the cluster.
But for larger clusters, we'll usually just list enough nodes that having them all go down at once will mean that you have more pressing concerns then a new client coming up.

{CODE:nodejs InitializationSample@ClientApi\Cluster\howClientApiIntegratesWithReplicationAndCluster.js /}

{PANEL/}

{PANEL: Write assurance and database groups}

In RavenDB clusters, the databases are hosted in database groups. 
Since there is a master-master replication configured between database group members, a write to one of the nodes will be replicated to all other instances of the group.
If there are some writes that are important, it is possible to make the client wait until the transaction data gets replicated to multiple nodes.
It is called a 'write assurance', and it is available with the `waitForReplicationAfterSaveChanges()` method.

{CODE:nodejs WriteAssuranceSample@ClientApi\Cluster\howClientApiIntegratesWithReplicationAndCluster.js /}

{PANEL/}

## Related articles

### Cluster

- [Clustering Overview](../../server/clustering/overview)
- [Client Speed Test](../../client-api/cluster/speed-test)
- [Cluster Node Health Check](../../client-api/cluster/health-check)

### Configuration

- [Load Balance & Failover](../../client-api/configuration/load-balance/overview)
