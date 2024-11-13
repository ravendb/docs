# Load balance behavior

---

{NOTE: }

* The `loadBalanceBehavior` configuration allows you to specify which sessions should 
  communicate with the same node.  
 
* Sessions that are assigned the **same context** will have all their _Read_ & _Write_ 
  requests routed to the **same node**. Gain load balancing by assigning **different contexts** 
  to **different sessions**.  

---

* In this page:
    * [LoadBalanceBehavior options](../../../client-api/configuration/load-balance/load-balance-behavior#loadbalancebehavior-options)
    * [Initialize LoadBalanceBehavior on the client](../../../client-api/configuration/load-balance/load-balance-behavior#initialize-loadbalancebehavior-on-the-client)
    * [Set LoadBalanceBehavior on the server:](../../../client-api/configuration/load-balance/load-balance-behavior#set-loadbalancebehavior-on-the-server)
        * [By operation](../../../client-api/configuration/load-balance/load-balance-behavior#set-loadbalancebehavior-on-the-server---by-operation)
        * [From Studio](../../../client-api/configuration/load-balance/load-balance-behavior#set-loadbalancebehavior-on-the-server---from-studio)
    * [When to use](../../../client-api/configuration/load-balance/load-balance-behavior#when-to-use)
     
{NOTE/}

---

{PANEL: LoadBalanceBehavior options }

### `None` (default option)

* Requests will be handled based on the `ReadBalanceBehavior` configuration.  
  See the conditional flow described in [Client logic for choosing a node](../../../client-api/configuration/load-balance/overview#client-logic-for-choosing-a-node).  
   * **_Read_** requests:  
     The client will calculate the target node from the configured [ReadBalanceBehavior Option](../../../client-api/configuration/load-balance/read-balance-behavior#readbalancebehavior-options).  
   * **_Write_** requests:  
     Will be sent to the [preferred node](../../../client-api/configuration/load-balance/overview#the-preferred-node).  
     The data will then be replicated to all the other nodes in the database group.
 
---

### `UseSessionContext`

* **Load-balance**

  * When this option is enabled, the client will calculate the target node from the session-id.  
    The session-id is hashed from a **context string** and an optional **seed** given by the user.  
    The context string together with the seed are referred to as **"The session context"**.
  
  * Per session, the client will select a node from the topology list based on this session-context.  
    So sessions that use the **same** context will target the **same** node.
  
  * All **_Read & Write_** requests made on the session (i.e a query or a load request, etc.)  
    will address this calculated node.  
    _Read & Write_ requests that are made on the store (i.e. executing an [operation](../../../client-api/operations/what-are-operations))  
    will go to the preferred node.

  * All _Write_ requests will be replicated to all the other nodes in the database group as usual.

* **Failover**  

  * In case of a failure, the client will try to access the next node from the topology nodes list.

{PANEL/}

{PANEL: Initialize LoadBalanceBehavior on the client }

* The `LoadBalanceBehavior` convention can be set **on the client** when initializing the Document Store.  
  This will set the load balance behavior for the default database that is set on the store.

* This setting can be **overriden** by setting 'LoadBalanceBehavior' on the server, see [below](../../../client-api/configuration/load-balance/load-balance-behavior#set-loadbalancebehavior-on-the-server).

---

**Initialize conventions**:

{CODE:php LoadBalance_1@ClientApi\Configuration\LoadBalance\LoadBalance.php /}
{CODE:php LoadBalance_6@ClientApi\Configuration\LoadBalance\LoadBalance.php /}

---

**Session usage**:

{CODE:php LoadBalance_2@ClientApi\Configuration\LoadBalance\LoadBalance.php /}
{CODE:php LoadBalance_3@ClientApi\Configuration\LoadBalance\LoadBalance.php /}

{PANEL/}

{PANEL: Set LoadBalanceBehavior on the server }

{NOTE: }

**Note**:  

* Setting the load balance behavior on the server, either by an **Operation** or from the **Studio**,  
  only 'enables the feature' and sets the seed.

* For the feature to be in effect, you still need to define the context string itself:  
  * either, per session, call the advanced `setContext` method  
  * or, set a default document store value using `setLoadBalancerPerSessionContextSelector`  

{NOTE/}

---

#### Set LoadBalanceBehavior on the server - by operation:

* The `LoadBalanceBehavior` configuration can be set **on the server** by sending an [operation](../../../client-api/operations/what-are-operations).

* The operation can modify the default database only, or all databases - see examples below.

* Once configuration on the server has changed, the running client will get updated with the new settings.  
  See [keeping client up-to-date](../../../client-api/configuration/load-balance/overview#keeping-the-client-topology-up-to-date).

{CODE-TABS}
{CODE-TAB:php:Operation_For_Default_Database LoadBalance_4@ClientApi\Configuration\LoadBalance\LoadBalance.php /}
{CODE-TAB:php:Operation_For_All_Databases LoadBalance_5@ClientApi\Configuration\LoadBalance\LoadBalance.php /}
{CODE-TABS/}

---

#### Set LoadBalanceBehavior on the server - from Studio:

* The `LoadBalanceBehavior` configuration can be set from the Studio's [Client Configuration view](../../../studio/database/settings/client-configuration-per-database).  
  Setting it from the Studio will set this configuration directly **on the server**.

* Once configuration on the server has changed, the running client will get updated with the new settings.  
  See [keeping client up-to-date](../../../client-api/configuration/load-balance/overview#keeping-the-client-topology-up-to-date).

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
