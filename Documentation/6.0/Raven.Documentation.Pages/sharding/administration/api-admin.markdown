# Sharding: API Administration
---

{NOTE: }

* This page explains how to manage a sharded database using RavenDB's API.  
* Learn [here](../../sharding/administration/api-admin) how to manage 
  a sharded database using Studio.  
* A sharded database can currently be created only 
  [via Studio](../../sharding/administration/studio-admin#creating-a-sharded-database).  
  All other database operations are available via API as well.  

* In this page:  
  * [Orchestrator Administration](../../)  
     * [Adding an Orchestrator](../../)  
     * [Removing an Orchestrator](../../)  
  * [Shard Administration](../../)  
     * [Adding a New Shard](../../)  
     * [Removing a Shard](../../)  

{NOTE/}

---

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
    | `node ` | string | The node to be made orchestrator |

* Return value: `ModifyOrchestratorTopologyResult`  
  {CODE ModifyOrchestratorTopologyResult@Sharding\ShardingAdministration.cs /}

## Removing an Orchestrator

* To stop a node from functioning as an orchestrator pass the database name 
  and the node identifier to the `RemoveNodeFromOrchestratorTopologyOperation` 
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

## Add a Shard 

* To add a new shard, use one of the `AddDatabaseShardOperation` operation overloads.  
  {CODE AddDatabaseShardOperation_Overload-1@Sharding\ShardingAdministration.cs /}
  {CODE AddDatabaseShardOperation_Overload-2@Sharding\ShardingAdministration.cs /}
  {CODE AddDatabaseShardOperation_Overload-3@Sharding\ShardingAdministration.cs /}

* Parameters:  

    | Parameter | Type | Description |
    |:-------------:|:-------------:|-------------|
    | `databaseName` | string | Database Name |
    | `shardNumber ` | int? | Shard number |
    | `replicationFactor` | int? | The new shard's replication factor. <br> if not provided, the replication factor will be set as that of the previous shard to be added. |
    | `nodes` | string[] | A list of nodes to replicate the shard to. <br> The replication factor will be set by the number of nodes provided here. |

* Return value: `AddDatabaseShardResult`  
  {CODE AddDatabaseShardResult@Sharding\ShardingAdministration.cs /}

## Add a Shard Replica

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
    | `node ` | string | The node that the shard will be set on (optional). <br> If not provided, RavenDB will select an available node. |

* Return value: `DatabasePutResult`  
  {CODE DatabasePutResult@Sharding\ShardingAdministration.cs /}


## Promote Shard Replica Immediately

* Shards can be promoted as non-sharded databases can.  

    To promote a shard, pass the database name, shard number and 
    node identifier to the `PromoteDatabaseNodeOperation` operation.  
    {CODE PromoteDatabaseNodeOperation_Definition@Sharding\ShardingAdministration.cs /}

* Parameters:  

    | Parameter | Type | Description |
    |:-------------:|:-------------:|-------------|
    | `databaseName` | string | Database Name |
    | `shard ` | int | Shard number |
    | `node ` | string | Node identifier |

* Return value: `DatabasePutResult`  
  {CODE DatabasePutResult@Sharding\ShardingAdministration.cs /}


## Removing a Shard

* A shard is removed when of all its replicas have been deleted.  
* RavenDB will remove a shard only after verifying that its database 
  is empty. If any buckets remain in the database the operation will 
  be aborted.  
* The replication factor is updated automatically as replicas are 
  removed, there is no need to update it explicitly.  
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

**Client API**  
[Create Database](../../client-api/operations/server-wide/create-database)  
[Smuggler](../../client-api/smuggler/what-is-smuggler)  
[Server-Wide Backup](../../client-api/operations/maintenance/backup/backup#server-wide-backup)  

**Server**  
[Backup Overview](../../server/ongoing-tasks/backup-overview)  
[External Replication](../../server/ongoing-tasks/external-replication)  
[Responsible Node](../../server/clustering/distribution/highly-available-tasks#responsible-node)  

**Studio**  
[Export Data](../../studio/database/tasks/export-database)  
[Import Data](../../studio/database/tasks/import-data/import-data-file)  
[One-Time Backup](../../studio/database/tasks/backup-task#manually-creating-one-time-backups)  

