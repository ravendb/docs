# Replication : How client integrates with replication and the cluster?

{PANEL:**Failover behavior**}

In contrast to previous versions, in RavenDB 4.x replication is not a bundle and it is always enabled, if there are two nodes or more in the cluster. 
This means that the failover mechanism is always turned on by default.

The client contains a list of the cluster nodes per database group, and each time the client needs to do a request, it will choose a  that contains the database node to send the request to from the topology.  The choice of node depends on a value of `ReadBalanceBehavior`, which is taken from the current conventions. You can read more about the different values and node selection process [here](../configuration/conventions/replication).

{PANEL/}

{PANEL:**Cluster Topology In The Client**}

RavenDB client periodically receives cluster and database topology from the server. 
When the client initialized it fetches the topologies and populates node list for load-balancing and failover functionality.

During the lifetime of a client object, the topology is updated with the following logic:
 * Each topology has an etag, which is a number. Each time the topology is changed, the etag is incremented.
 * For each request, the client adds in a header, latest topology etag it has.
 * If current topology etag at the server is higher than the one in the client, the server adds `"Refresh-Topology:true"` header to the response.
 * If a client detects `"Refresh-Topology:true"` header in the response, the client will fetch the updated topology from the server. Also, if ReadBalanceBehavior.FastestNode is selected, the client will schedule a speed test to determine the fastest node.

Client configuration is handled in the same way - each client configuration has an etag attached. Each time the configuration is changed at the server-side, the server adds `"Refresh-Client-Configuration"` to the response. When the client detects the aforementioned header in the response, it schedules fetching the new configuration.

{PANEL/}
