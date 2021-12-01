# Backups
---

{NOTE: }

* Backups save your data at a specific point in time, and allow you to 
[restore](../../../studio/server/databases/create-new-database/from-backup#create-a-database-from-backup) 
your database from that point. Learn more in [Backup Overview](../../../server/ongoing-tasks/backup-overview).  

* This Studio view enables you to create ongoing [**periodic backup tasks**](../../../studio/database/tasks/backup-task#backup-creation),  
 as well as one-time 
[**manual backups**](../../../studio/database/tasks/backup-task#creating-one-time-manual-backups), for a particular database.  

* It also enables **server-wide backups**.  
 See [Studio: Server-Wide Backups](../../../studio/server/server-wide-backup) to learn more.  

* A backup is _not_ equivalent to replicating your data, as explained below in 
[Backup -vs- Replication](../../../studio/database/tasks/backup-task#backup-task--vs--replication-task)  

* Configure the following when creating a backup task:
  * **Backup Type** - Select Backup/Snapshot  
  * **Backup Time** - Schedule the task time  
  * **Backup Content** - Select Full/Incremental  
  * **Backup Retention Policy** - Schedule deletion of backups
  * **Backup Destination** - Select the backup destination  

* In this page:  
  * [Backups View](../../../studio/database/tasks/backup-task#backups-view)  
  * [Backup Creation](../../../studio/database/tasks/backup-task#backup-creation)  
      * [Content & Scheduling](../../../studio/database/tasks/backup-task#content--scheduling)  
      * [Retention Policy](../../../studio/database/tasks/backup-task#retention-policy)  
      * [Destination](../../../studio/database/tasks/backup-task#destination)  
  * [Periodic Backup Details](../../../studio/database/tasks/backup-task#periodic-backup-details)  
  * [When the Cluster or Node is Down](../../../studio/database/tasks/backup-task#when-the-cluster-or-node-is-down)  
  * [Backup Task -vs- Replication Task](../../../studio/database/tasks/backup-task#backup-task--vs--replication-task)  
  * [Creating One-time Manual Backups](../../../studio/database/tasks/backup-task#creating-one-time-manual-backups)  


{NOTE/}

---

{PANEL: Backups View}



{WARNING: Manual Backup Tasks View}

![Manual Backup Task View](images/manual-backup-task-view.png "Manual Backups View")

1. [Restore a database from a backup](../../../studio/server/databases/create-new-database/from-backup#create-a-database-from-backup) 
(this creates a new database, it doesn't modify this database).  
2. Create a one-time [manual backup](../../../studio/database/tasks/backup-task#creating-one-time-manual-backups). This can be vital before upgrading or whenever you want an unscheduled backup. This can also be done in the [periodic backup details view](../../../studio/database/tasks/backup-task#periodic-backup-details)  
3. Refresh the _Recent Backup_ clock for a manual backup, so that it displays the correct 
amount of time that has passed since this backup was created.  

{WARNING/}

{WARNING: Periodic Backup Tasks View}

![Periodic Backup Task View](images/Periodic-backup-task-view.png "Periodic Backups View")

1. Create a [periodic backup task](../../../studio/database/tasks/backup-task#backup-creation).  
2. [View Details](../../../studio/database/tasks/backup-task#periodic-backup-details) of periodic backup tasks.  
  * **Backup Now** (eg. before software updates) can be triggered in the 'View Details' interface.
3. **Edit** this database backup task. To edit server-wide backup tasks, see button #5.  
4. **Delete** this periodic backup task.  
5. Go to the [server-wide backups view](../../../studio/server/server-wide-backup).  

{WARNING/}


{INFO: Backups Topology Info}

![Backups Topology Info](images/topology-backup-info-view.png "Backups Topology Info")

1. The periodic server-wide backup for this server.  
2. The periodic database backup, which is assigned to this node.  

{INFO/}

{PANEL/}

{PANEL: Backup Creation}


{NOTE: Scheduling two backups}

To save on transfer costs you can schedule frequent incremental backups,  
while backing up your indexes with infrequent full-snapshot type backups. 

{NOTE/}


![Figure 1. Backup Task Definition](images/backup-definition-51up-fullandincremental.png "Create New Backup Task")

{NOTE: }

1. **Task Name** (Optional)  

   * Choose a name of your choice  
   * If no name is given then RavenDB server will create one for you based on the defined destination  


2. **Backup Task Type**:  

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


3. **Preferred Node** (Optional)  

   * Select a preferred mentor node from the [Database Group](../../../studio/database/settings/manage-database-group) to be the responsible node for this Backup Task  
   * If no node is selected, then the cluster will assign a responsible node (see [Members Duties](../../../studio/database/settings/manage-database-group#database-group-topology---members-duties))  

{NOTE/}

---

### Content & Scheduling

{NOTE: }

Select the content to back up. Note: Both types can be scheduled.  

  4) **Full Backup**  
     Full Backup will back up _all_ the database data every time the task is scheduled to work.  
  
  
  5) **Incremental Backup**  
     Incremental Backup will only back up the delta (changes made) of the data since the last backup that has occurred.  

Schedule the Backup Task to occur at regular intervals (daily, monthly, hourly) at specific times.  
{NOTE/}

Notes:  
  1. If _only_ **Incremental Backup** is set, then a **Full Backup** will occur only in the _first_ time that the Task is triggered,  
     followed by Incremental Backups according to the scheduled time.  
     The Full Backup that is done the first time will be either a 'Backup' or a 'Snapshot', depending on the type selected.  

  2. Data that is backed up in **Incremental Backup** is _always_ of type 'Backup' - even if the Backup Task Type is 'Snapshot'.  
     A Snapshot can only occur when scheduling 'Full'.  

---

### Retention Policy

![Figure 3. Backup Retention Policy](images/backup-task-2_5.png "Retention Policy")

{NOTE: }

1. Enable / disable the retention policy. If disabled, the backups are stored indefinitely, which may use a lot of storage. If enabled, deletion can be scheduled. 
2. Select the retention period. Once a backup is older than the specified amount of time, 
it will be deleted during the next scheduled backup task.  

{NOTE/}

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
  
{NOTE: To achieve a robust data protection strategy}
 
 The [3-2-1 rule](https://www.nakivo.com/blog/3-2-1-backup-rule-efficient-data-protection-strategy/) can be implemented by creating a periodic backup onto a cloud storage and another on an onsite machine.

{NOTE/}



---



###Periodic Backup Details

![NoSQL Database Backup](images/backup-task-details.png "Backups View")

{INFO: }
1. **Backup Task Details**:  

  *  **Task Status**  
   Active / Not Active / Not on Node  
  *  **Destinations**  
   List of all backup destinations defined  
  *  **Last Full Backup**  
   The last time a Full Backup was done 
     (Snapshot / Backup type - depending on task definition)  
  *  **Last Incremental Backup**  
   The last time an Incremental Backup was done  
  *  **Next Estimated Backup**  
   Time for next backup  
     (Full Backup / Incremental Backup / Snapshot - depending on task definition)  
  *  **Retention Policy**  
   Backups older than this period will be deleted during next backup.
   
{INFO/}

{WARNING: }

1. **Backup Now**:  
   In addition to the scheduled time defined, you can backup your data now.  
   This is useful before software updates or any other action where data may be lost.  
   The scheduled backup will still be triggered as defined.  
2. **Refresh**:  
   Click to refresh this panel viewed details  

{WARNING/}
<br/>

### When the Cluster or Node is Down

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

* RavenDB's [External Replication](../../../../studio/database/tasks/ongoing-tasks/external-replication-task) provides you with an off-site live replica/copy of the data 
  ('live' meaning that any changes in the database will be reflected in the replica once they occur).  
  If one database is down, the replica can continue its work, thus greatly improving **availability**.  
  This is also quite useful if you need to:  
    * **shift operations** to a secondary data center  
    * **share the workload** across more than one server.  

*  But a replica isn't a backup. It doesn't present good solutions for many **backup scenarios**. For example, backups can:  
    * protect you from an accidental collection delete  
    * tell you the state of the system at, say, 9:03 AM last Friday  
    * protect you from various cyber attacks  

* A backup keeps an exact state of the database at a specific point in time and can be restored.  
  * A new database can be [created from a Backup](../../../studio/server/databases/create-new-database/from-backup)  
  * This can be done with both 'Backup' & 'Snapshot' types  
  {PANEL/}

{PANEL: Creating One-time Manual Backups}

![NoSQL Database Manual Backup Creation](images/manual-backup.png "Manual Backup Creation")

1. Select **Backup Type**.
    * Snapshot saves entire database and it's full indexes.
    * Backup saves the data, but only index definitions.
2. Select **Encryption** or un-encrypted.
3. Select **Destination** for backup to be stored. 
4. Click **Backup Now** to finish the process.

Manual backups have only some of the properties of periodic backups.  

* The backup can be _full_ or _snapshot_.  
* The backup can be encrypted or not.  
* The backup must have a [destination](../../../studio/database/tasks/backup-task#destination).  

Some of the differences include:  

* Manual backups do not have names, they are identified by the time they were 
created.  
* Manual backups are not scheduled, they occur exactly once: when the **Backup Now** button is pressed.  
* The backup cannot be modified afterwards from the Studio, but you can create a new one-time backup whenever you need to.  
* Manual backups do not have retention policies. In other words, they are not set to 
be automatically deleted.  

{PANEL/}

## Related Articles  

### Studio  
- [Server-Wide Backups](../../../studio/server/server-wide-backup)
- [Create a Database : From Backup](../../../studio/server/databases/create-new-database/from-backup)  
- [Create a Database : General Flow](../../../studio/server/databases/create-new-database/general-flow)  
- [Create a Database : Encrypted](../../../studio/server/databases/create-new-database/encrypted)  
- [External Replication](../../../studio/database/tasks/ongoing-tasks/external-replication-task)  

### Client API  
- [Restore](../../../client-api/operations/maintenance/backup/restore)  
- [Operations: How to Restore a Database from Backup](../../../client-api/operations/server-wide/restore-backup)  
- [What Is Smuggler](../../../client-api/smuggler/what-is-smuggler)  
- [Backup](../../../client-api/operations/maintenance/backup/backup)  

### Server  
- [Backup Overview](../../../server/ongoing-tasks/backup-overview)  

### Migration  
- [Migration](../../../migration/server/data-migration)  



