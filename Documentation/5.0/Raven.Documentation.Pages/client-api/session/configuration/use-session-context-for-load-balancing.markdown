# Use Session Context for Load Balancing

{NOTE: }

* The `LoadBalanceBehavior` convention determines whether the nodes 
  topology used to handle client requests, can be chosen by a client session.  
* When enabled, a session can select its handling topology by `tag`.  
  Client requests that use the same tag, are served by the same topology.  
* Using pre-set topologies enables clients and administrators to load-balance requests 
  as they please. E.g., frequent employee-related and product-related requests can be 
  handled by an "**employees_top**" and a "**products_top**" topologies, respectively.  

* In this page:  
   * [LoadBalanceBehavior Options](../../../client-api/session/configuration/use-session-context-for-load-balancing#loadbalancebehavior-options)  

{NOTE/}

{PANEL: LoadBalanceBehavior Options}

  * `None`  
    Nodes are chosen to serve read requests by the 
    [ReadBalanceBehavior convention](../../../client-api/configuration/load-balance-and-failover#readbalancebehavior-options), 
    and to server write requests by the [Preferred Node](../../../client-api/configuration/load-balance-and-failover#preferred-node) 
    calculated by the client.  

  * `UseSessionContext`
     * A client session can select the topology that would handle its requests.  
       The topology is selected by `tag`.  
       Requests that use the same tag, are served by the same topology.  
       The same nodes are used for **Read** and **Write** requests.  
     * Administrators can override client selections, selecting the 
       topology that would handle client requests by its hash seed.  
     * Using this option to choose client request-handling topology 
       influences only the client, causing no change in replication 
       or other server functions.  

  {WARNING: }
  Enabling this feature increases the chance for conflicts, 
  as multiple sessions may approach a shared document using different tags.  
  {WARNING/}

##Example I
In this example, a client session chooses its topology by tag.  
{CODE LoadBalanceBehavior@ClientApi\Session\Configuration\ChooseTopology.cs /}

##Example II
In this example a client configuration, including the topology 
selection, is placed on the server using 
[PutClientConfigurationOperation](../../../client-api/operations/maintenance/configuration/put-client-configuration).  
{CODE PutClientConfigurationOperation@ClientApi\Session\Configuration\ChooseTopology.cs /}

##Example III
In this example a server-wide client configuration, including topology 
selection, is placed on the server using 
[PutServerWideClientConfigurationOperation](../../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration).  
{CODE PutServerWideClientConfigurationOperation@ClientApi\Session\Configuration\ChooseTopology.cs /}

{PANEL/}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Opening a Session](../../../client-api/session/opening-a-session)
- [Storing Entities](../../../client-api/session/storing-entities)
- [Loading Entities](../../../client-api/session/loading-entities)
- [Saving Changes](../../../client-api/session/saving-changes)
- [Load Balance & Failover](../../../client-api/configuration/load-balance-and-failover)
