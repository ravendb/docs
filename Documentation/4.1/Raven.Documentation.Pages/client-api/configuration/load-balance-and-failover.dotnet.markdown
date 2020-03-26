# Conventions: Load Balance & Failover

{NOTE: }

###ReadBalanceBehavior

* This convention gets or sets the default load-balancing behavior of ***Read*** requests from the client to RavenDB cluster.  

* The method selected in the convention determines which node the client's `RequestExecutor` will send the ***Read*** requests to,  
  and which node to *failover* to in case of issues with the ***Read*** request.  

* Note: ***Write*** requests will always access the [Preferred Node](../../client-api/configuration/load-balance-and-failover#preferred-node) calculated by the client.  

{CODE ReadBalanceBehavior@ClientApi\Configuration\Cluster.cs /}

* In this page:  
  * [ReadBalanceBehavior Options](../../client-api/configuration/load-balance-and-failover#readbalancebehavior-options)  
  * [Preferred Node](../../client-api/configuration/load-balance-and-failover#preferred-node) 
  * [Session usage](../../client-api/configuration/load-balance-and-failover#session-usage) 
{NOTE/}

---

{PANEL: ReadBalanceBehavior Options}

  * `None`  
    * **Load-balance**: No load balancing will occur. The client will always select the _preferred node_.  
    * **Failover**: The client will failover nodes in the order they appear in the _topology nodes list_.  
      Note: The list can be reordered, see [Database Group Actions](../../studio/database/settings/manage-database-group#database-group-topology---actions).  

  * `RoundRobin`  
    * **Load-balance**: For each _Read_ request, the client will address the next node from the _topology nodes list_.  
    * **Failover**: In case of a failure, the client will try the next node in the round robin order. 

  * `FastestNode`  
    * **Load-balance**: _Read_ request will go to the fastest node.  
      The fastest node is determined by a [Speed Test](../../client-api/cluster/speed-test).  
    * **Failover**: In case of a failure, a speed test will be triggered again and in the meantime the client will use the _preferred node_.  
{PANEL/}

{PANEL: Preferred Node}

* The preferred node is selected by simply going over the _topology nodes list_ and returning the first node that has not had any errors.  

* If all nodes are in a failure state then the _first_ node in the list is returned. The user would get an error or recover if the error was transient.  
{PANEL/}

{PANEL: Session Usage}

* When using `RoundRobin` or `FastestNode` it might happen that the next [session](../../client-api/session/opening-a-session) you open will access a different node.  

* A short delay in replicating changes to all nodes in the cluster is acceptable in most cases.  
  If you need to ensure that the next request will be able to _immediately_ read what you just wrote, 
  you need to use [Write Assurance](../../client-api/session/saving-changes#waiting-for-replication---write-assurance).  
{PANEL/}

## Related Articles

### Configuration

- [Conventions](../../client-api/configuration/conventions)
- [Querying](../../client-api/configuration/querying)
- [Serialization](../../client-api/configuration/serialization)

### Client Configuration in Studio

- [Requests Configuration in Studio](../../studio/server/client-configuration)
- [Requests Configuration per Database](../../studio/database/settings/client-configuration-per-database)
