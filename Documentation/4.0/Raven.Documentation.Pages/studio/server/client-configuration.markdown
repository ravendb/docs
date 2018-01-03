## Client Configuration
---

![Figure 1. Client Configuration](images/client-configuration.jpg "Client Configuration")

### Client Configuration
On this screen we can set load-balancing behavior and restrict the number of requests per session in the client API. 
This is done per database group.

#### Read Balance Behavior
The choice made in the combobox affects two behaviors:  

  * How the client API will decide which node to failover in case of issues
  * How the client API will execute load balancing

The choices:

  * None - the client API will failover nodes in their TAG order, (Node A, then Node B, then Node C and so on). No load balancing will occur.
  * Round Robin - for each request the client API will address the next node in their TAG order. In case of failover, the client will try the next node as well.
  * Fastest Node - each client API request will go to the fastest node (determined by a [speed test](../../../server/scaling-out/clustering/speed-test)). Any topology change would trigger the [speed test](../../../server/scaling-out/clustering/speed-test) again. Failover in this case would select the node with the next TAG. 

{NOTE: Failure to contact all nodes}
If the client has tried to contact all nodes and failed, an `AllTopologyNodesDownException` will be thrown
{NOTE/}
