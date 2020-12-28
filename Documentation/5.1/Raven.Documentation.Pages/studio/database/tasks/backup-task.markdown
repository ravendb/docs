# Backups
---

{NOTE: }

* Backups save your data at a specific point in time, and allow you to 
[restore](../../../studio/server/databases/create-new-database/from-backup#create-a-database-from-backup) 
your database from that point. Learn more in [Backup Overview](../../../server/ongoing-tasks/backup-overview).  

* This view allows you to create ongoing **periodic backup tasks**, as well as one-time 
**manual backups**, for a particular database.  

* It lists manual and periodic backups for the database, and also the **server-wide backups** 
for this server. See [Studio: Server-Wide Backups](../../../studio/server/server-wide-backup) 
to learn more.  

* A backup is _not_ equivalent to replicating your data, as explained below in 
[Backup -vs- Replication](../../../studio/database/tasks/backup-task#backup-task--vs--replication-task)  

* Configure the following when creating a backup task:
  * **Backup Type** - Select Backup/Snapshot  
  * **Backup Time** - Schedule the task time  
  * **Backup Content** - Select Full/Incremental  
  * **Backup Retention Policy** - Schedule deletion of backups
  * **Backup Destination** - Select the backup destination  

* In this page:  
  * [Backup List View](../../../studio/database/tasks/backup-task#backup-list-view)  
  * [Backup Creation](../../../studio/database/tasks/backup-task#backup-creation)  
      * [Content & Scheduling](../../../studio/database/tasks/backup-task#content--scheduling)  
      * [Retention Policy](../../../studio/database/tasks/backup-task#retention-policy)  
      * [Destination](../../../studio/database/tasks/backup-task#destination)  
      * [Creating Manual Backups](../../../studio/database/tasks/backup-task#creating-manual-backups)  
  * [Periodic Backup Details](../../../studio/database/tasks/backup-task#periodic-backup-details)  
  * [When the Cluster or Node are Down](../../../studio/database/tasks/backup-task#when-the-cluster-or-node-are-down)  
  * [Backup Task -vs- Replication Task](../../../studio/database/tasks/backup-task#backup-task--vs--replication-task)  

{NOTE/}

---

{PANEL: Backup List View}

![](images/backup-task-view.png "'Backups' View")

{WARNING: }
1. Create a **manual backup**, see the creation view [below](../../../studio/database/tasks/backup-task#creating-manual-backups).  
2. Refresh the _Recent Backup_ clock for a manual backup, so that it displays the correct 
amount of time that has passed since this backup was created.  
3. Create a **periodic backup task**, see the creation view [below](../../../studio/database/tasks/backup-task#backup-creation).  
4. View details for this task, see detail view [below](../../../studio/database/tasks/backup-task#periodic-backup-details).  
5. Edit this database backup task. To edit server-wide backup tasks, see button #7.  
6. Delete this periodic backup task.  
7. Go to the [server-wide backups view](../../../studio/server/server-wide-backup).  
8. [Restore a database from a backup](../../../studio/server/databases/create-new-database/from-backup#create-a-database-from-backup) 
(this creates a new database, it doesn't modify this database).  
{WARNING/}

{INFO: }
1. The periodic server-wide backup for this server.  
2. The periodic database backup, which is assigned to this node.  
{INFO/}

{PANEL/}

{PANEL: Backup Creation}

![Figure 1. Backup Task Definition](images/backup-task-1.png "Create New Backup Task")

{WARNING: }
1) **Task Name** (Optional)  

  * Choose a name of your choice  
  * If no name is given then RavenDB server will create one for you based on the defined destination  

2) **Backup Task Type**:  

  * ***Backup***  
      * Backed Up Data: The database data in a JSON format, including documents, indexes (definitions only) & [identities](../../../server/kb/document-identifier-generation#identity)  
       (same as exported database format)  
      * Size of backup data: Smaller  
      * Backup Speed: Faster  
      * Restoring: Slower, Indexes have to be rebuilt from their definitions  
  * ***Snapshot***  
      * Backed Up Data: The raw database data including the indexes (definitions and data)
      * Size of backup data: Larger  
      * Backup Speed: Slower  
      * Restoring: Faster, Indexes do not have to be rebuilt  

3) **Preferred Node** (Optional)  

  * Select a preferred mentor node from the [Database Group](../../../studio/database/settings/manage-database-group) to be the responsible node for this Backup Task  
  * If no node is selected, then the cluster will assign a responsible node (see [Members Duties](../../../studio/database/settings/manage-database-group#database-group-topology---members-duties))  
{WARNING/}

---

### Content & Scheduling

![Figure 2. Backup Task Schedule and Content](images/backup-task-2.png "Backup Task Schedule & Content")

{WARNING: }

Select the content to back up. Note: Both types can be scheduled.  

  1. **Full Backup** 
     Full Backup will back up _all_ the database data every time the task is scheduled to work  

  2. **Incremental Backup** 
     Incremental Backup will only back up the delta of the data since the last Backup that has occurred  

Schedule the Backup Task to occur at regular intervals (daily, monthly, hourly) at specific times.  
{WARNING/}

Notes:  
  1. If _only_ **Incremental Backup** is set, then a **Full Backup** will occur only in the _first_ time that the Task is triggered,  
     followed by Incremental Backups according to the scheduled time.  
     The Full Backup that is done the first time will be either a 'Backup' or a 'Snapshot', depending on the type selected.  

  2. Data that is backed up in **Incremental Backup** is _always_ of type 'Backup' - even if the Backup Task Type is 'Snapshot' !  
     A Snapshot is only  occurring when scheduling 'Full'.  

In the example above:  
  * A Full Backup is scheduled every day at 02:00 AM - and in addition to that -  
  * An Incremental Backup is scheduled every 6 hours  

---

### Retention Policy

![Figure 3. Backup Retention Policy](images/backup-task-2_5.png "Retention Policy")

{WARNING: }
1. Enable / disable the retention policy. If disabled, the backups are stored indefinitely. 
If enabled, deletion can be scheduled.  
2. Select the retention period. Once a backup is older than the specified amount of time, 
it will be deleted during the next scheduled backup task.  
{WARNING/}

---

### Destination

![Figure 4. Backup Task Destinations](images/backup-task-3.png "Backup Destinations")

* Select backup destinations and enter your credentials for each  

* Note: More than one can be selected  

* Available destinations:  

  * Local - Set a folder of your choice (any directory that can be accessed from your machine)  
  * [Amazon S3](https://aws.amazon.com/s3/)  
  * [Microsoft Azure](https://azure.microsoft.com/en-us/services/storage/)  
  * [Google Cloud](https://cloud.google.com/)  
  * [Amazon Glacier](https://aws.amazon.com/glacier/)  
  * FTP - Set your FTP protocol & server address  

---

### Creating Manual Backups

![](images/manual-backup.png "Manual backup creation")

Manual backups have only some of the properties of periodic backups. The backup can 
be _full_ or _snapshot_. The backup can be encrypted or not. The backup must have a 
destination, chosen from the same options as a periodic backup.  

Manual backups do not have names, they are be identified by the time they were 
created. Manual backups are not scheduled, they occur exactly once: when the 
**Backup Now** button is pressed. The backup cannot be modified afterwards from the 
Studio.  

Manual backups do not have retention policies, in other words they are not set to 
be automatically deleted.  

{PANEL/}

{PANEL: Periodic Backup Details}

![](images/backup-task-details.png "'Backups' View")

{INFO: }
1. **Backup Task Details**:  

  *  Task Status - Active / Not Active / Not on Node  
  *  Destinations - List of all backup destinations defined  
  *  Last Full Backup - The last time a Full Backup was done 
     (Snapshot / Backup type - depending on task definition)  
  *  Last Incremental Backup - The last time an Incremental Backup was done  
  *  Next Estimated Backup - Time for next backup 
     (Full Backup / Incremental Backup / Snapshot - depending on task definition)  
  *  Last Executed on Node - The cluster node that most recently executed this periodic 
     backup. This can be different from the "responsible node" chosen when the task was 
     first defined, because when the responsible node is down, another node will be 
     assigned the task.  
{INFO/}

{WARNING: }
1. **Backup Now**:  
   In addition to the scheduled time defined, you can backup your data now.  
   The scheduled backup will still be triggered as defined.  
2. **Refresh**:  
   Click to refresh this panel viewed details  
{WARNING/}
{PANEL/}

{PANEL: When the Cluster or Node are Down}

* **When the cluster is down** (and there is no leader):  

  * Creating a _new_ Ongoing Task is a Cluster-Wide operation,  
    thus, a new Ongoing Backup Task ***cannot*** be scheduled.  

  * If a Backup Task was _already_ defined and active when the cluster went down,  
    then the Backup Task will still ***continue to execute*** on its defined schedule (on its [responsible node](../../../server/clustering/distribution/highly-available-tasks#responsible-node)).  
    But, it will fail to be reported to the cluster and may be run again after the cluster has recovered.  

* **When the responsible node is down**  

  * If the responsible node for the Backup Task is down during the scheduled time,  
    then another node from the Database Group will take ownership of the task so that there are no gaps in your backups.  
{PANEL/}

{PANEL: Backup Task -vs- Replication Task}

* RavenDB's [External Replication](../../../studio/database/tasks/ongoing-tasks/external-replication-task) provides you with an off-site live replica/copy of the data.  
  ('live' meaning that any changes in the database will be reflected in the replica once they occur).  
  This is quite useful if you need to shift operations to a secondary data center.  

*  But a replica isn't a backup... It doesn't present good solutions to many backup scenarios.  
   For example, it doesn't protect you from an accidental collection delete,  
   or tell you the state of the system at, say, 9:03 AM last Friday.  

* A backup keeps an exact state of the database at a specific point in time and can be restored  
  * A new database can be [created from a Backup](../../../studio/server/databases/create-new-database/from-backup)  
  * This can be done for both 'Backup' & 'Snapshot' types  
{PANEL/}

## Related Articles  

### Studio  
- [Server-Wide Backups](../../../studio/server/server-wide-backup)
- [Create a Database : From Backup](../../../studio/server/databases/create-new-database/from-backup)  
- [Create a Database : General Flow](../../../studio/server/databases/create-new-database/general-flow)  
- [Create a Database : Encrypted](../../../studio/server/databases/create-new-database/encrypted)  

### Client API  
- [Restore](../../../client-api/operations/maintenance/backup/restore)  
- [Operations: How to Restore a Database from Backup](../../../client-api/operations/server-wide/restore-backup)  
- [What Is Smuggler](../../../client-api/smuggler/what-is-smuggler)  
- [Backup](../../../client-api/operations/maintenance/backup/backup)  

### Server  
- [Backup Overview](../../../server/ongoing-tasks/backup-overview)  

### Migration  
- [Migration](../../../migration/server/data-migration)  



