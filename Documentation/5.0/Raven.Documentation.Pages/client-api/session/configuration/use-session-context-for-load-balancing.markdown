# Use Session Context for Load Balancing

{NOTE: }

* The `LoadBalanceBehavior` convention determines whether a client session 
  is allowed to select the topology that would handle its requests.  
  When enabled, a session can select a topology by `tag`. Client requests 
  sent by sessions that use the same tag, are handled by the same topology.  
* Clients and administrators can use tags to load-balance traffic, e.g. by 
  using the topology tagged "users/1-A" for requests sent by this user.  

* In this page:  
   * [LoadBalanceBehavior Options](../../../client-api/session/configuration/use-session-context-for-load-balancing#loadbalancebehavior-options)  

{NOTE/}

{PANEL: LoadBalanceBehavior Options}

  * `None`  
    Read requests are handled based on the 
    [ReadBalanceBehavior convention](../../../client-api/configuration/load-balance-and-failover#readbalancebehavior-options).  
    Write requests are handled by the [Preferred Node](../../../client-api/configuration/load-balance-and-failover#preferred-node) 
    calculated by the client.  

  * `UseSessionContext`
     * A client session can select the topology that would handle its requests.  
       The topology is selected by `tag`.  
       Requests that use the same tag, are served by the same topology.  
       The same nodes are used for **Read** and **Write** requests.  
     * Administrators can disable this feature, or add a hash to randomize 
       the topology that would be selected for the client when it uses a tag.  
     * Using this option to choose client request-handling topology 
       influences only the client, causing no change in replication 
       or other server functions.  

  {NOTE: }
  Conflicts may rarely happen, when multiple sessions that use different tags 
  modify a shared document concurrently. 
  {NOTE/}

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
