# Read balance behavior

 ---

{NOTE: }

* When set, the `readBalanceBehavior` configuration will be in effect according to the   
  conditional flow described in [Client logic for choosing a node](../../../client-api/configuration/load-balance/overview#client-logic-for-choosing-a-node).

* Once configuration is in effect then:  
  * **_Read_** requests - will be sent to the node determined by the configured option - see below.
  * **_Write_** requests - are always sent to the preferred node.  
    The data will then be replicated to all the other nodes in the database group. 
  * Upon a node failure, the node to failover to is also determined by the defined option.  

* In this page:
    * [readBalanceBehavior options](../../../client-api/configuration/load-balance/read-balance-behavior#readbalancebehavior-options)  
    * [Initialize readBalanceBehavior on the client](../../../client-api/configuration/load-balance/read-balance-behavior#initialize-readbalancebehavior-on-the-client)  
    * [Set readBalanceBehavior on the server:](../../../client-api/configuration/load-balance/read-balance-behavior#set-readbalancebehavior-on-the-server)  
        * [By operation](../../../client-api/configuration/load-balance/read-balance-behavior#setByOperation)  
        * [From Studio](../../../client-api/configuration/load-balance/read-balance-behavior#setFromStudio)  
    * [When to use](../../../client-api/configuration/load-balance/read-balance-behavior#when-to-use)
     
{NOTE/}

---

{PANEL: readBalanceBehavior options }

{NOTE: }

__`None`__ (default option)

  * __Read-balance__  
    No read balancing will occur.  
    The client will always send _Read_ requests to the [preferred node](../../../client-api/configuration/load-balance/overview#the-preferred-node).
  * __Failover__  
    The client will failover nodes in the order they appear in the [topology nodes list](../../../studio/database/settings/manage-database-group#database-group-topology---actions).

{NOTE/}

{NOTE: }

__`RoundRobin`__

* __Read-balance__
  * Each session opened is assigned an incremental session-id number.  
    __Per session__, the client will select the next node from the topology list based on this internal session-id.  
  * All _Read_ requests made on the session (i.e a query or a load request, etc.)  
    will address the calculated node.  
  * A _Read_ request that is made on the store (i.e. executing an [operation](../../../client-api/operations/what-are-operations))   
    will go to the preferred node.  
* __Failover__  
  In case of a failure, the client will try the next node from the topology nodes list.  

{NOTE/}

{NOTE: }

__`FastestNode`__  

  * __Read-balance__  
    All _Read_ requests will go to the fastest node.  
    The fastest node is determined by a [Speed Test](../../../client-api/cluster/speed-test).
  * __Failover__  
    In case of a failure, a speed test will be triggered again,  
    and in the meantime the client will use the preferred node.

{NOTE/}

{PANEL/}

{PANEL: Initialize readBalanceBehavior on the client }

{NOTE: }

* The `readBalanceBehavior` convention can be set __on the client__ when initializing the Document Store.  
  This will set the read balance behavior for the default database that is set on the store.  

* This setting can be __overriden__ by setting 'readBalanceBehavior' on the server, see [below](../../../client-api/configuration/load-balance/read-balance-behavior#set-readbalancebehavior-on-the-server).   

{CODE:nodejs readBalance_1@ClientApi\Configuration\LoadBalance\readBalance.js /}

{NOTE/}

{PANEL/}

{PANEL: Set readBalanceBehavior on the server }

{NOTE: }

<a id="setByOperation" /> __Set readBalanceBehavior on the server - by operation__ 

---

* The `readBalanceBehavior` configuration can be set __on the server__ by sending an [operation](../../../client-api/operations/what-are-operations).  

* The operation can modify the default database only, or all databases - see examples below.  

* Once configuration on the server has changed, the running client will get updated with the new settings.  
  See [keeping client up-to-date](../../../client-api/configuration/load-balance/overview#keeping-the-client-topology-up-to-date).  

{CODE-TABS}
{CODE-TAB:nodejs:Operation_For_Default_Database readBalance_2@ClientApi\Configuration\LoadBalance\readBalance.js /}
{CODE-TAB:nodejs:Operation_For_All_Databases readBalance_3@ClientApi\Configuration\LoadBalance\readBalance.js /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

<a id="setFromStudio" /> __Set readBalanceBehavior on the server - from Studio__

---

* The `readBalanceBehavior` configuration can be set from the Studio's [Client Configuration view](../../../studio/database/settings/client-configuration-per-database).  
  Setting it from the Studio will set this configuration directly __on the server__.  

* Once configuration on the server has changed, the running client will get updated with the new settings.  
  See [keeping client up-to-date](../../../client-api/configuration/load-balance/overview#keeping-the-client-topology-up-to-date).  

{NOTE/}

{PANEL/}

{PANEL: When to use }

* Setting the read balance behavior is beneficial when you only care about distributing the _Read_ requests among the cluster nodes,
  and when all _Write_ requests can go to the same node.

* Using the 'FastestNode' option is beneficial when some nodes in the system are known to be faster than others,
  thus letting the fastest node serve each read request.

{PANEL/}

## Related Articles

### Operations

- [What are operations](../../../client-api/operations/what-are-operations)

### Configuration

- [Load balancing client requests - overview](../../../client-api/configuration/load-balance/overview)
- [Load balance behavior](../../../client-api/configuration/load-balance/load-balance-behavior)
- [Conventions](../../../client-api/configuration/conventions)
- [Querying](../../../client-api/configuration/querying)

### Client Configuration in Studio

- [Requests Configuration in Studio](../../../studio/server/client-configuration)
- [Requests Configuration per Database](../../../studio/database/settings/client-configuration-per-database)
- [Database-group-topology](../../../studio/database/settings/manage-database-group#database-group-topology---view)
