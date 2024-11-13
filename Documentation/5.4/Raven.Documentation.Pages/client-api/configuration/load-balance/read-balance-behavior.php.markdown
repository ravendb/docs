# Read balance behavior

 ---

{NOTE: }

* When set, the `ReadBalanceBehavior` configuration will be in effect according to the   
  conditional flow described in [Client logic for choosing a node](../../../client-api/configuration/load-balance/overview#client-logic-for-choosing-a-node).

* Once configuration is in effect then:  
  * **_Read_** requests - will be sent to the node determined by the configured option - see below.
  * **_Write_** requests - are always sent to the preferred node.  
    The data will then be replicated to all the other nodes in the database group. 
  * Upon a node failure, the node to failover to is also determined by the defined option.  

* In this page:
    * [ReadBalanceBehavior options](../../../client-api/configuration/load-balance/read-balance-behavior#readbalancebehavior-options)  
    * [Initialize ReadBalanceBehavior on the client](../../../client-api/configuration/load-balance/read-balance-behavior#initialize-readbalancebehavior-on-the-client)  
    * [Set ReadBalanceBehavior on the server:](../../../client-api/configuration/load-balance/read-balance-behavior#set-readbalancebehavior-on-the-server)  
        * [By operation](../../../client-api/configuration/load-balance/read-balance-behavior#set-readbalancebehavior-on-the-server---by-operation)  
        * [From Studio](../../../client-api/configuration/load-balance/read-balance-behavior#set-readbalancebehavior-on-the-server---from-studio)  
    * [When to use](../../../client-api/configuration/load-balance/read-balance-behavior#when-to-use)
     
{NOTE/}

---

{PANEL: readBalanceBehavior options }

### `None` (default option)

  * **Read-balance**  
    No read balancing will occur.  
    The client will always send _Read_ requests to the [preferred node](../../../client-api/configuration/load-balance/overview#the-preferred-node).
  * **Failover**  
    The client will failover nodes in the order they appear in the [topology nodes list](../../../studio/database/settings/manage-database-group#database-group-topology---actions).

---

### `RoundRobin`

* **Read-balance**
  * Each session opened is assigned an incremental session-id number.  
    **Per session**, the client will select the next node from the topology list based on this internal session-id.  
  * All _Read_ requests made on the session (i.e a query or a load request, etc.)  
    will address the calculated node.  
  * A _Read_ request that is made on the store (i.e. executing an [operation](../../../client-api/operations/what-are-operations))   
    will go to the preferred node.  
* **Failover**  
  In case of a failure, the client will try the next node from the topology nodes list.  

---

### `FastestNode`

  * **Read-balance**  
    All _Read_ requests will go to the fastest node.  
    The fastest node is determined by a [Speed Test](../../../client-api/cluster/speed-test).
  * **Failover**  
    In case of a failure, a speed test will be triggered again,  
    and in the meantime the client will use the preferred node.

{PANEL/}

{PANEL: Initialize ReadBalanceBehavior on the client }

* The `ReadBalanceBehavior` convention can be set **on the client** when initializing the Document Store.  
  This will set the read balance behavior for the default database that is set on the store.  

* This setting can be **overriden** by setting 'ReadBalanceBehavior' on the server, see [below](../../../client-api/configuration/load-balance/read-balance-behavior#set-readbalancebehavior-on-the-server).   

{CODE:php ReadBalance_1@ClientApi\Configuration\LoadBalance\ReadBalance.php /}

{PANEL/}

{PANEL: Set ReadBalanceBehavior on the server }

#### Set ReadBalanceBehavior on the server - by operation:

* The `ReadBalanceBehavior` configuration can be set **on the server** by sending an [operation](../../../client-api/operations/what-are-operations).  

* The operation can modify the default database only, or all databases - see examples below.  

* Once configuration on the server has changed, the running client will get updated with the new settings.  
  See [keeping client up-to-date](../../../client-api/configuration/load-balance/overview#keeping-the-client-topology-up-to-date).  

{CODE-TABS}
{CODE-TAB:php:Operation_For_Default_Database ReadBalance_2@ClientApi\Configuration\LoadBalance\ReadBalance.php /}
{CODE-TAB:php:Operation_For_All_Databases ReadBalance_3@ClientApi\Configuration\LoadBalance\ReadBalance.php /}
{CODE-TABS/}

#### Set ReadBalanceBehavior on the server - from Studio:

* The `ReadBalanceBehavior` configuration can be set from the Studio's [Client Configuration view](../../../studio/database/settings/client-configuration-per-database).  
  Setting it from the Studio will set this configuration directly **on the server**.  

* Once configuration on the server has changed, the running client will get updated with the new settings.  
  See [keeping client up-to-date](../../../client-api/configuration/load-balance/overview#keeping-the-client-topology-up-to-date).  

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
