# Sharding: API Administration
---

{NOTE: }

* This page explains how to manage a sharded database using RavenDB's API.  
* Learn [here](../../sharding/administration/studio-admin) how to manage 
  a sharded database using Studio.  

* In this page:  
  * [Creating a Sharded Database](../../sharding/administration/api-admin#creating-a-sharded-database)  
  * [Orchestrator Administration](../../sharding/administration/api-admin#orchestrator-administration)  
     * [Adding an Orchestrator](../../sharding/administration/api-admin#adding-an-orchestrator)  
     * [Removing an Orchestrator](../../sharding/administration/api-admin#removing-an-orchestrator)  
  * [Shard Administration](../../sharding/administration/api-admin#shard-administration)  
     * [Adding a Shard](../../sharding/administration/api-admin#adding-a-shard)  
     * [Adding a Shard Replica](../../sharding/administration/api-admin#adding-a-shard-replica)  
     * [Promoting a Shard Replica](../../sharding/administration/api-admin#promoting-a-shard-replica)  
     * [Removing a Shard](../../sharding/administration/api-admin#removing-a-shard)  

{NOTE/}

---

{PANEL: Creating a Sharded Database}

To create a sharded database:  

* Use [CreateDatabaseOperation](../../client-api/operations/server-wide/create-database) 
  to create the database.  
* Define `ShardingConfiguration` in the database record.  
   * The initial configuration can define just the database topologies for as many shards 
     as needed.  
   * Orchestrators and shards can be added and removed later on, after the database is created.  

### `ShardingConfiguration`
{CODE ShardingConfiguration_definition@Sharding\ShardingAdministration.cs /}

### Example
{CODE createShardedDatabase@Sharding\ShardingAdministration.cs /}

{PANEL/}

{PANEL: Orchestrator Administration}

Prior to granting a cluster node an orchestrator functionality, we should 
make sure that the node is up for the task, with no other tasks contesting 
the orchestrator for system resources. E.g., it may be better to use as 
orchestrators nodes that host no shards.  

## Adding an Orchestrator

* To add an orchestrator pass the database name and the node to be added 
  as orchestrator to the `AddNodeToOrchestratorTopologyOperation` operation.  
  {CODE AddNodeToOrchestratorTopologyOperation_Definition@Sharding\ShardingAdministration.cs /}

* Parameters:  

    | Parameter | Type | Description |
    |:-------------:|:-------------:|-------------|
    | `databaseName` | string | Database Name |
    | `node ` | string | Node tag for the node to be made orchestrator |

* Return value: `ModifyOrchestratorTopologyResult`  
  {CODE ModifyOrchestratorTopologyResult@Sharding\ShardingAdministration.cs /}

## Removing an Orchestrator

* To stop a node from functioning as an orchestrator pass the database name 
  and the node tag to the `RemoveNodeFromOrchestratorTopologyOperation` 
  operation.  
  {CODE RemoveNodeFromOrchestratorTopologyOperation_Definition@Sharding\ShardingAdministration.cs /}

* Parameters:  

    | Parameter | Type | Description |
    |:-------------:|:-------------:|-------------|
    | `databaseName` | string | Database Name |
    | `node ` | string | The node to be removed as orchestrator |

* Return value: `ModifyOrchestratorTopologyResult`  
  {CODE ModifyOrchestratorTopologyResult@Sharding\ShardingAdministration.cs /}

{PANEL/}

{PANEL: Shard Administration}

## Adding a Shard 

* To add a new shard, use one of the `AddDatabaseShardOperation` operation overloads.  
  {CODE AddDatabaseShardOperation_Overload-1@Sharding\ShardingAdministration.cs /}
  {CODE AddDatabaseShardOperation_Overload-2@Sharding\ShardingAdministration.cs /}
  {CODE AddDatabaseShardOperation_Overload-3@Sharding\ShardingAdministration.cs /}

* Parameters:  

    | Parameter | Type | Description |
    |:-------------:|:-------------:|-------------|
    | `databaseName` | string | Database Name |
    | `shardNumber ` | int? | Shard number <br> If a shard number is not explicitly provided, the shard number will be the biggest existing shard number + 1 |
    | `replicationFactor` | int? | The new shard's replication factor (**see comment below**) |
    | `nodes` | string[] | A list of nodes to replicate the shard to. <br> If provided, the replication factor will be set by the number of nodes. |

    {NOTE: }
    `replicationFactor`, the new shard's replication factor, is determined as follows:  
    
    * If `replicationFactor` is **not** provided explicitly, and a list of nodes is provided, 
      the replication factor will be set by the number of nodes.  
    * If **neither** `replicationFactor` and a nodes list are provided, the replication factor 
      will be set as that of the first shard.  
    * If **both** `replicationFactor` and a nodes list are provided:  
       * If there are **less** nodes than set by `replicationFactor`, the new shard will be replicated 
         on these nodes.  
       * If there are **more** nodes than set by `replicationFactor`, only as many replications as 
         set by `replicationFactor` will be carried out.

    {NOTE/}


* Return value: `AddDatabaseShardResult`  
  {CODE AddDatabaseShardResult@Sharding\ShardingAdministration.cs /}

## Adding a Shard Replica

* To add a replica to an existing shard pass the database name and a shard 
  number to the `AddDatabaseNodeOperation` operation.  

    {NOTE: }
    The replication factor is updated automatically as replicas are added, 
    there is no need to update it explicitly.  
    {NOTE/}

    {CODE AddDatabaseNodeOperation_Definition@Sharding\ShardingAdministration.cs /}

* Parameters:  

    | Parameter | Type | Description |
    |:-------------:|:-------------:|-------------|
    | `databaseName` | string | Database Name |
    | `shardNumber` | string | Shard Number |
    | `node ` | string | The node that the replica will be set on (optional). <br> If not provided, RavenDB will select an available node. |

* Return value: `DatabasePutResult`  
  {CODE DatabasePutResult@Sharding\ShardingAdministration.cs /}


## Promoting a Shard Replica

* Shard replicas can be [promoted](../../client-api/operations/server-wide/promote-database-node) 
  as non-sharded databases can.  

    To promote a shard, pass the database name, shard number and 
    node tag to the `PromoteDatabaseNodeOperation` operation.  
    This will help locate the  exact shard instance we want to 
    promote, leading to the database, then to the specific shard, 
    and finally to the specific replica of that shard.  
    {CODE PromoteDatabaseNodeOperation_Definition@Sharding\ShardingAdministration.cs /}

* Parameters:  

    | Parameter | Type | Description |
    |:-------------:|:-------------:|-------------|
    | `databaseName` | string | Database Name |
    | `shardNumber` | int | Shard number |
    | `node` | string | Node tag |

* Return value: `DatabasePutResult`  
  {CODE DatabasePutResult@Sharding\ShardingAdministration.cs /}


## Removing a Shard

* A shard is removed when all its replicas have been deleted.  
* RavenDB will remove a shard only after verifying that its database 
  is empty. If any buckets remain in the database the operation will 
  be aborted.  
* To remove a shard use the designated `DeleteDatabasesOperation` overload.  
  {CODE DeleteDatabasesOperation_Definition@Sharding\ShardingAdministration.cs /}

* Parameters:  

    | Parameter | Type | Description |
    |:-------------:|:-------------:|-------------|
    | `databaseName` |  string | Database Name |
    | `shardNumber` | int | Shard number: number of the shard replica to be removed |
    | `hardDelete` | bool | If `true`, Hard Delete: stop replication to this node and **delete the replica's database**. <br> If `false`, Soft Delete: stop replication to this node but do not delete the replica's database. |
    | `fromNode` | string | The node we want to remove the replica from |
    | `timeToWaitForConfirmation` | TimeSpan? |  |

* Return value: `DeleteDatabaseResult`  
  {CODE DeleteDatabaseResult@Sharding\ShardingAdministration.cs /}

{PANEL/}


## Related articles

### Sharding

- [Sharding Overview](../../sharding/overview)  
- [Sharding Studio API](../../sharding/administration/studio-admin)  

### Client-API

- [Create a Database](../../client-api/operations/server-wide/create-database) 
