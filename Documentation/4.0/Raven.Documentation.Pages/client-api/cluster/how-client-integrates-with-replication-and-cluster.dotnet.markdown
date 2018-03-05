# Replication : How client integrates with replication and the cluster?

{PANEL:**Failover Behavior**}

* In RavenDB 4.x, in contrast to previous versions, replication is _not_ a bundle and is always enabled if there are two nodes or more in the cluster. 
  This means that the failover mechanism is always turned on by default.  

* The client contains a list of cluster nodes per database group.  
  Each time the client needs to do a request to a database, it will choose a node that contains this database from this list to send the request to. 
  If the node is down and the request fails, it will select another node from this list.  

* The choice of which node to select depends on the value of `ReadBalanceBehavior`, which is taken from the current conventions. 
  For more information about the different values and the node selection process see [Related Cluster Conventions](../configuration/cluster). 
  
{NOTE Each failure to connect to a node, spawns a health check for that node. For more information see [Cluster Node Health Check](health-check)./}

{PANEL/}

{PANEL:**Cluster Topology In The Client**}

When the client is initialized, it fetches the topologies and populates the nodes list for the load-balancing and failover functionality.
During the lifetime of a RavenDB Client object, it periodically receives the cluster and the databases topologies from the server.  

The topology is updated with the following logic:

* Each topology has an etag, which is a number. 
* Each time the topology has changed, the etag is incremented.  
* For each request, the client adds the latest topology etag it has to the header.  
* If the current topology etag at the server is higher than the one in the client, the server adds `"Refresh-Topology:true"` the response header.  
* If a client detects `"Refresh-Topology:true"` header in the response, the client will fetch the updated topology from the server.  
  Note: if `ReadBalanceBehavior.FastestNode` is selected, the client will schedule a speed test to determine the fastest node.  

The client configuration is handled in a similar way:

* Each client configuration has an etag attached.  
* Each time the configuration has changed at the server-side, the server adds `"Refresh-Client-Configuration"` to the response.  
* When the client detects the aforementioned header in the response, it schedules fetching the new configuration.
{PANEL/}

{PANEL:**Topology Discovery**}
In RavenDB 4.x, cluster topology has an etag which increments after each topology change.

### How and when the topology is updated?
* First time any request is sent to RavenDB server, the ClientAPI fetches cluster topology. 
* Each subsequent requests happen with a fetched topology etag in the http headers, under the key 'Topology-Etag'
* If in the response headers there is a flag under the key 'Refresh-Topology' and its value is true, a thread that will update the topology will be spawned.

{PANEL/}

{PANEL:**Configuring Topology Nodes**}

The initialization of `DocumentStore` allows specifying cluster node urls

{CODE Sample@ClientApi\Cluster\HowClientApiIntegratesWithReplicationAndCluster.cs /}

{PANEL/}
