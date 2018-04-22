# Client Configuration
---

{NOTE: }

* Configure the RavenDB client behavior for **all** databases in the cluster  
* These default values can be overwritten per databases in [Database Client Configuration](../../../../todo-update-me-later)  
{NOTE/}

---

{PANEL: Client Configuration}

![Figure 1. Client Configuration](images/client-configuration.jpg "Client Configuration")

**1. Read balance behavior**  

* Set the client API load-balancing behavior  
* Effects the client API decision of which node to failover in case of issues  
* Options:  
  * **None**  
      * The client API will failover nodes in their TAG order. (Node A, then Node B, then Node C and so on).  
      * No load balancing will occur.  
  * **Round Robin**  
      * For each request, the client API will address the next node in their TAG order.  
      * In case of a failover, the client will try the next node as well.  
  * **Fastest Node**  
      * Each client API request will go to the fastest node (determined by a [speed test](../../client-api/cluster/speed-test)).  
      * Any topology change would trigger the [speed test](../../client-api/cluster/speed-test) again.  
      * Failover in this case would select the node with the next TAG.  

**2. Max number of requests per session**  

* Set this number to restrict the number of requests per session in the client API.  
* The default value is 30.  

{NOTE: Failure to contact all nodes}
If the client has tried to contact all nodes and failed, an `AllTopologyNodesDownException` will be thrown
{NOTE/}

{PANEL/}
