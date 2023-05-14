# Sharding: Studio Administration
---

{NOTE: }

* This page explains how to create and manage a sharded database using Studio.  
* Learn [here](../../sharding/administration/api-admin) how to manage 
  a sharded database using API commands.  

* In this page:  
  * [Creating a sharded database](../../sharding/administration/studio-admin#creating-a-sharded-database)  
  * [Database View](../../sharding/administration/studio-admin#database-view)  
  * [Database Group](../../sharding/administration/studio-admin#database-group)  

{NOTE/}

---

{PANEL: Creating a sharded database}

![Create New Database](images/studio-admin_create-new-database.png "Create New Database")

1. **Database View**  
   Click to create, view or edit database properties.  
2. **Create new database**  
   This button is available when no database has been created yet.  
   Click it to create a new database.  
2. **New database**  
   Click to create a new database.  

---

Initiating the creation of a new database as shown above will open 
the following view:  

![New Database Settings](images/studio-admin_new-database.png "New Database Settings")

1. **Name**  
   Enter the database name.  
2. **Replication factor**  
   Decide the number of replicas the database will have.  
   If set to 1, there will be no replication.  
3. **Enable sharding**  
   Click to enable or disable sharding on this database.  
4. **Number of shards**  
   Set the number of shards the database will be comprised of.  
5. **Set nodes layout manually**  
   The layout determines which nodes host each shard, and whether 
   nodes can function as [orchestrators](../../sharding/overview#client-server-communication).  
   Enable this option to set the layout manually.  
   Disable to let RavenDB set it for you.  
6. **Orchestrator nodes**  
   Select which nodes can function as orchestrators.  
7. **Shards replication**  
   Select the nodes that host shard replicas.  
   The number of nodes available for each shard is determined 
   using the "Replication factor" box at the top.  
8. **Create**  
   Click to create the database.  
   
{PANEL/}

{PANEL: Database View}

After creating a database as explained above, the database will 
be available here:  

![Database View](images/studio-admin_database-view_01.png "Database View")

1. **Expand/Collapse distribution details**  
   Click to display or hide per-shard database details.  

     ![Expanded Details](images/studio-admin_database-view_expanded-details.png "Expanded Details")

2. **Encryption status**  
   Informative icon: notify whether the database is encrypted or not.  
   
3. **Shard storage report**  
   Click to watch the shard's storage usage.  
     
      ![Select Shard](images/studio-admin_database-view_02_select-shard.png "Select Shard")

      Select the shard number and its replica node.    
      The storage report view will open.  

      ![Storage Report](images/studio-admin_database-view_03_storage-report.png "Storage Report")

4. **Documents view**  
   Click to view and edit documents.  
      
     ![Documents View](images/studio-admin_database-view_04_docs-view.png "Documents View")

5. **Indexes view**  
   Click to open the [List of Indexes](../../studio/database/indexes/indexes-list-view) view.  
6. **Backups view**  
   Click to manage [backup tasks](../../studio/database/tasks/backup-task) and restore existing backups.  
7. **Manage group**  
   Click to manage the database group ([see below](../../sharding/administration/studio-admin#database-group)).  

{PANEL/}

{PANEL: Database Group}

The Database Group view allows you to appoint and dismiss 
[orchestrators](../../sharding/overview#client-server-communication) 
and add or remove [shards](../../sharding/overview#shards) and 
shard [replicas](../../sharding/overview#shard-replication).  

![Database Group](images/studio-admin_database-group.png "Database Group")

1. **Add Shard**  
   Click to add a shard.  

      ![Add Shard](images/database-group_add-shard.png "Add Shard")
   
      Set the shard's replication factor and click **Add shard** (or **Cancel**).  
      The new shard will be added to the database group view.  

2. **Manage Orchestrators**  
   Add or remove orchestrator functionality to cluster nodes.  

    * **Add Orchestrator**  
      This option will be available only if there are still nodes that 
      an orchestrator hasn't been assigned for.  

        ![Add Orchestrator 1](images/database-group_add-orchestrator-01.png "Add Orchestrator 1")

        Click **Add node** to add an orchestrator.  

        ![Add Orchestrator 2](images/database-group_add-orchestrator-02.png "Add Orchestrator 2")

        Select an available node for the orchestrator and click **Add orchestrator** (or **Cancel**).  

    * **Remove Orchestrator**  

        ![Remove Orchestrator](images/database-group_remove-orchestrator.png "Remove Orchestrator")
      
        Click **Remove** to remove the orchestrator functionality from this node.  

3. **Manage Shards**  

    * **Add shard replica**  
      This option will be available only if there are still available 
      nodes for replicas of the selected shard.  
      
        ![Add Shard Replica 1](images/database-group_add-shard-replica-01.png "Add Shard Replica 1")
      
        Click **Add node** to add a shard replica.  
      
        ![Add Shard Replica 2](images/database-group_add-shard-replica-02.png "Add Shard Replica 2")

        Select a node for the replica.  
        If you want, you can also set a mentor node that will replicate 
        the data to the new replica.  
        Click "Add node" to create the new replica (or **Cancel**).  

    * **Remove shard replica**  
      
        ![Remove Shard Replica 1](images/database-group_remove-shard-replica-01.png "Remove Shard Replica 1")
      
        Click **Delete from group** to remove the shard replica from the database group.  
      
        ![Remove Shard Replica 2](images/database-group_remove-shard-replica-02.png "Remove Shard Replica 2")
      
        Click **Soft Delete** to stop replication to this node but keep the database files on it.  
        Click **Hard Delete** to stop replication to this node and **delete database files** from it.  
        {WARNING: }
        Removing a shard is done by removing all its replicas.  
        Please be careful not to remove files that have no backup and may still be needed.  
        {WARNING/}

{PANEL/}

## Related Articles

### Sharding

- [Sharding Overview](../../sharding/overview)  
- [Sharding Admin API](../../sharding/administration/api-admin)  

