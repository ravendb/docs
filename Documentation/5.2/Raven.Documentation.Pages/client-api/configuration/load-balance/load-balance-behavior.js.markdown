# Load balance behavior

 ---

{NOTE: }

* The `loadBalanceBehavior` configuration  
  allows you to specify which sessions should communicate with the same node.  
 
* Sessions that are assigned the __same context__ will have all their _Read_ & _Write_ requests routed to the __same node__.  
  So load balancing is achieved by assigning a different context to different sessions.  

---

* In this page:
    * [loadBalanceBehavior options](../../../client-api/configuration/load-balance/load-balance-behavior#loadbalancebehavior-options)
    * [Initialize loadBalanceBehavior on the client](../../../client-api/configuration/load-balance/load-balance-behavior#initialize-loadbalancebehavior-on-the-client)
    * [Set loadBalanceBehavior on the server:](../../../client-api/configuration/load-balance/load-balance-behavior#set-loadbalancebehavior-on-the-server)
        * [By operation](../../../client-api/configuration/load-balance/load-balance-behavior#setByOperation)
        * [From Studio](../../../client-api/configuration/load-balance/load-balance-behavior#setFromStudio)
    * [When to use](../../../client-api/configuration/load-balance/load-balance-behavior#when-to-use)
     
{NOTE/}

---

{PANEL: loadBalanceBehavior options }

{NOTE: }

__`None`__ (default option)

* Requests will be handled based on the `readBalanceBehavior` configuration.  
  See the conditional flow described in [Client logic for choosing a node](../../../client-api/configuration/load-balance/overview#client-logic-for-choosing-a-node).  
  * **_Read_** requests:  
    The client will calculate the target node from the configured [readBalanceBehavior Option](../../../client-api/configuration/load-balance/read-balance-behavior#readbalancebehavior-options).  
  * **_Write_** requests:  
    Will be sent to the [preferred node](../../../client-api/configuration/load-balance/overview#the-preferred-node).  
    The data will then be replicated to all the other nodes in the database group.
 
{NOTE/}

{NOTE: }

__`UseSessionContext`__

* __Load-balance__

  * When this option is enabled, the client will calculate the target node from the session-id.  
    The session-id is hashed from a __context string__ and an optional __seed__ given by the user.  
    The context string together with the seed are referred to as __"The session context"__.
  
  * Per session, the client will select a node from the topology list based on this session-context.  
    So sessions that use the __same__ context will target the __same__ node.
  
  * All **_Read & Write_** requests made on the session (i.e a query or a load request, etc.)  
    will address this calculated node.  
    _Read & Write_ requests that are made on the store (i.e. executing an [operation](../../../client-api/operations/what-are-operations))  
    will go to the preferred node.

  * All _Write_ requests will be replicated to all the other nodes in the database group as usual.

* __Failover__  

  * In case of a failure, the client will try to access the next node from the topology nodes list.

{NOTE/}

{PANEL/}

{PANEL: Initialize loadBalanceBehavior on the client }

* The `loadBalanceBehavior` convention can be set __on the client__ when initializing the Document Store.  
  This will set the load balance behavior for the default database that is set on the store.

* This setting can be __overriden__ by setting 'loadBalanceBehavior' on the server, see [below](../../../client-api/configuration/load-balance/load-balance-behavior#set-loadbalancebehavior-on-the-server).

---

__Initialize conventions__:

{CODE:nodejs loadBalance_1@ClientApi\Configuration\LoadBalance\loadBalance.js /}
{CODE:nodejs loadBalance_6@ClientApi\Configuration\LoadBalance\loadBalance.js /}

---

__Session usage__:

{CODE:nodejs loadBalance_2@ClientApi\Configuration\LoadBalance\loadBalance.js /}
{CODE:nodejs loadBalance_3@ClientApi\Configuration\LoadBalance\loadBalance.js /}

{PANEL/}

{PANEL: Set loadBalanceBehavior on the server }

{INFO: }

__Note__:  

* Setting the load balance behavior on the server, either by an __Operation__ or from the __Studio__,  
  only 'enables the feature' and sets the seed.

* For the feature to be in effect, you still need to define the context string itself:  
  * either per session, call `session.advanced.sessionInfo.setContext`  
  * or, on the document store, set a default value for - `loadBalancerPerSessionContextSelector`  

{INFO/}

{NOTE: }

<a id="setByOperation" /> __Set loadBalanceBehavior on the server - by operation__ 

---

* The `loadBalanceBehavior` configuration can be set __on the server__ by sending an [operation](../../../client-api/operations/what-are-operations).

* The operation can modify the default database only, or all databases - see examples below.

* Once configuration on the server has changed, the running client will get updated with the new settings.  
  See [keeping client up-to-date](../../../client-api/configuration/load-balance/overview#keeping-the-client-topology-up-to-date).

{CODE-TABS}
{CODE-TAB:nodejs:Operation_For_Default_Database loadBalance_4@ClientApi\Configuration\LoadBalance\loadBalance.js /}
{CODE-TAB:nodejs:Operation_For_All_Databases loadBalance_5@ClientApi\Configuration\LoadBalance\loadBalance.js /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

<a id="setFromStudio" /> __Set loadBalanceBehavior on the server - from Studio__

---

* The `loadBalanceBehavior` configuration can be set from the Studio's [Client Configuration view](../../../studio/database/settings/client-configuration-per-database).  
  Setting it from the Studio will set this configuration directly __on the server__.

* Once configuration on the server has changed, the running client will get updated with the new settings.  
  See [keeping client up-to-date](../../../client-api/configuration/load-balance/overview#keeping-the-client-topology-up-to-date).

{NOTE/}

{PANEL/}

{PANEL: When to use }

* Distributing _Read & Write_ requests among the cluster nodes can be beneficial  
  when a set of sessions handle a specific set of documents or similar data.  
  Load balancing can be achieved by routing requests from the sessions that handle similar topics to the same node, while routing other sessions to other nodes.  
 
* Another usage example can be setting the session's context to be the current user.  
  Thus spreading the _Read & Write_ requests per user that logs into the application.  

* Once setting the load balance to be per session-context,  
  in the case when detecting that many or all sessions send requests to the same node,  
  a further level of node randomization can be added by changing the seed.  

{PANEL/}

## Related Articles

### Operations

- [What are operations](../../../client-api/operations/what-are-operations)

### Configuration

- [Load balancing client requests - overview](../../../client-api/configuration/load-balance/overview)
- [Read balance behavior](../../../client-api/configuration/load-balance/read-balance-behavior)
- [Conventions](../../../client-api/configuration/conventions)
- [Querying](../../../client-api/configuration/querying)

### Client Configuration in Studio

- [Requests Configuration in Studio](../../../studio/server/client-configuration)
- [Requests Configuration per Database](../../../studio/database/settings/client-configuration-per-database)
- [Database-group-topology](../../../studio/database/settings/manage-database-group#database-group-topology---view)
