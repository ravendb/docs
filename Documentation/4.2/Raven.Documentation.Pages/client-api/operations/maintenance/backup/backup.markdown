# Backup
---

{NOTE: }

* Create a backup of your data to secure it or to preserve a copy of it in its current state for future reference.  

* RavenDB's Backup task is an [Ongoing-Task](../../../../studio/database/tasks/ongoing-tasks/general-info) 
  designed to run periodically on a pre-defined schedule.  
  You can run it as a one-time operation as well, by using [Export](../../../../client-api/smuggler/what-is-smuggler#export) 
  or executing a backup-task [immediately](../../../../client-api/operations/maintenance/backup/backup#initiate-immediate-backup-execution).  

* In this page:  
  * [Backup Types](../../../../client-api/operations/maintenance/backup/backup#backup-types)  
      * [Logical-Backup](../../../../client-api/operations/maintenance/backup/backup#logical-backup)  
      * [Snapshot](../../../../client-api/operations/maintenance/backup/backup#snapshot)  
  * [Backup Scope](../../../../client-api/operations/maintenance/backup/backup#backup-scope)  
      * [Full Backup](../../../../client-api/operations/maintenance/backup/backup#full-backup)  
      * [Incremental Backup](../../../../client-api/operations/maintenance/backup/backup#incremental-backup)  
  * [Backup to Local and Remote Destinations](../../../../client-api/operations/maintenance/backup/backup#backup-to-local-and-remote-destinations)  
  * [Backup Retention Policy](../../../../client-api/operations/maintenance/backup/backup#backup-retention-policy)  
  * [Server-Wide Backup](../../../../client-api/operations/maintenance/backup/backup#server-wide-backup)  
  * [Initiate Immediate Backup Execution](../../../../client-api/operations/maintenance/backup/backup#initiate-immediate-backup-execution)  
  * [Recommended Precautions](../../../../client-api/operations/maintenance/backup/backup#recommended-precautions)  

{NOTE/}

---

{PANEL: Backup Types}

####Logical-Backup  

* Data is backed-up in [compressed](../../../../server/ongoing-tasks/backup-overview#compression) JSON files.  

* During the restoration, RavenDB -  
   * Re-inserts all data into the database.  
   * Inserts the saved index definitions. To save space, Logical Backup stores index definitions only.  
     After restoration, the dataset is scanned and indexed according to the definitions.

* Restoration time is therefore **slower** than when restoring from a Snapshot.  

* Backup file size is **significantly smaller** than that of a Snapshot.

* In addition to full data backup, Logical Backups can be defined as **incremental**, 
  saving any changes made since the previous backup.

* The following code sample defines a full-backup task that would be executed every 3 hours:  
  {CODE logical_full_backup_every_3_hours@ClientApi\Operations\Maintenance\Backup\Backup.cs /}
  Note the usage of [Cron scheduling](https://en.wikipedia.org/wiki/Cron) when setting backup frequency.  

---

####Snapshot

* A Snapshot is a compressed binary duplication of the full database structure. 
  This includes the data file and the journals at a given point in time.  
  Therefore it includes fully built indexes and ongoing tasks.  
  See [file structure](../../../../server/storage/directory-structure#storage--directory-structure) for more info.

* Snapshot backups are available only for **Enterprise subscribers**.  

* During restoration -
   * Re-inserting data into the database is not required.  
   * Re-indexing is not required.  

* Restoration is typically **faster** than that of a logical backup.  

* Snapshot size is typically **larger** than that of a logical backup.  

* If Incremental backups are created for a snapshot-type backup: 
   * The first backup will be a full Snapshot.
   * The following backups will be incremental. 
   * The [incremental backups](../../../../client-api/operations/maintenance/backup/backup#incremental-backup) 
     will be kept in [JSON format](../../../../client-api/operations/maintenance/backup/backup#incremental-backup).


* Code Sample:  
  {CODE backup_type_snapshot@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

---

####Basic Comparison Between a Logical-Backup and a Snapshot:

  | Backup Type | Stored Format | Restoration speed | Size
  | ------ | ------ | ------ |
  | Snapshot | Compressed Binary Image | Fast | Larger than a logical-backup
  | Logical backup |  Compressed Textual Data - JSON | Slow | Smaller than a Snapshot

{NOTE: Make sure your server has access to the local backup path.}
Verify that RavenDB is allowed to store files in the path set in `LocalSettings.FolderPath`.
{NOTE/}



{PANEL/}

{PANEL: Backup Scope}

As described in [the overview](../../../../server/ongoing-tasks/backup-overview#backing-up-and-restoring-a-database), a backup task can create **full** and **incremental** backups.  

* A Backup Task can be defined to create either a full data backup or an incremental backup.  
  In both cases, the backup task adds a single new backup file to the backup folder each time it runs,
  leaving the existing backup files untouched.  

---

####Full-Backup


* **File Format**  
  A full-backup is a **compressed JSON file** if it is a logical 
  backup, or a **compressed binary file** if it is a snapshot.  

* **Task Ownership**  
  There are no preliminary conditions for creating a full-backup. 
  Any node can perform this task.  

* **To run a full-backup**  
  Set `FullBackupFrequency`.
  {CODE backup_full_backup@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

---

####Incremental-Backup

* **File Format and Notes About Contents**  
  * An incremental-backup file is **always in JSON format**. 
    It is so even when the full-backup it is associated with is a binary snapshot.  
  * An incremental backup stores index definitions (not full indexes).  
    After the backup is restored, the dataset is re-indexed according to the index definitions.
    {NOTE: }
    This initial re-indexing can be time-consuming on large datasets.
    {NOTE/}
  * An incremental backup doesn't store [change vectors](../../../../server/clustering/replication/change-vector).

* **Task Ownership**  
  The ownership of an incremental-backup task is granted dynamically by the cluster.  
  An incremental-backup can be executed only by the same node that currently owns the backup task.  
  A node can run an incremental-backup, only after running full-backup at least once.  

* **To run an incremental-backup**  
  Set `IncrementalBackupFrequency`.
  {CODE backup_incremental_backup@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

{PANEL/}

{PANEL: Backup to Local and Remote Destinations}

* Backups can be made **locally**, as well as to a set of **remote locations** including -  
   * A network path
   * An FTP/SFTP target
   * Azure Storage 
   * Amazon S3 
   * Amazon Glacier 
   * Google Cloud

* RavenDB will store data in a local folder first, and transfer it to the remote 
  destination from the local one.  
   * If a local folder hasn't been specified, RavenDB will use the 
     temp folder defined in its Storage.TempPath setting.  
     If Storage.TempPath is not defined, the temporary files 
     will be created at the same location as the data file.  
     In either case, the folder will be used as temporary storage 
     and the local files deleted from it when the transfer is completed.
   * If a local folder **has** been specified, RavenDB will use it both 
     for the transfer and as its permanent local backup location.  

* Local and Remote Destinations Settings Code Sample:  
  {CODE backup_remote_destinations@ClientApi\Operations\Maintenance\Backup\Backup.cs /}
 
 {INFO: Tip}
    Use AWS [IAM](https://docs.aws.amazon.com/IAM/latest/UserGuide/introduction_access-management.html) (Identity and Access Management) 
    to restrict users access while they create backups.  
    E.g. -  
    {CODE-BLOCK:json}
        {
            "Version": "2012-10-17",
            "Statement": [
                {
                    "Sid": "VisualEditor0",
                    "Effect": "Allow",
                    "Action": "s3:PutObject",
                    "Resource": "arn:aws:s3:::BUCKET_NAME/*"
                },
                {
                    "Sid": "VisualEditor1",
                    "Effect": "Allow",
                    "Action": [
                        "s3:ListBucket",
                        "s3:GetBucketAcl",
                        "s3:GetBucketLocation"
                    ],
                    "Resource": "arn:aws:s3:::BUCKET_NAME"
                }
            ]
        }
    {CODE-BLOCK/}
 {INFO/}
{PANEL/}

{PANEL: Backup Retention Policy}

By default, backups are stored indefinitely. The backup retention policy sets 
a retention period, at the end of which backups are deleted. Deletion occurs 
during the next scheduled backup task after the end of the retention period.  

Full backups and their corresponding incremental backups are deleted together. 
Before a full backup can be deleted, all of its incremental backups must be older 
than the retention period as well.  

The retention policy is a property of `PeriodicBackupConfiguration`:  

{CODE-BLOCK:csharp }
public class RetentionPolicy
{
    public bool Disabled { get; set; }
    public TimeSpan? MinimumBackupAgeToKeep { get; set; }
}
{CODE-BLOCK/}

| Parameter | Type | Description |
| - | - | - |
| **Disabled** | `bool` | If set to `true`, backups will be retained indefinitely, and not deleted. Default: false |
| **MinimumBackupAgeToKeep** | `TimeSpan` | The minimum amount of time to retain a backup. Once a backup is older than this time span, it will be deleted during the next scheduled backup task. |

#### Example

{CODE:csharp backup_retentionpolicy@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

{PANEL/}

{PANEL: Server-Wide Backup}

You can create a Server-Wide Backup task to back-up **all the databases in your cluster** at a scheduled time.

* Backups can be made locally, as well as to a [set of remote locations](../../../../client-api/operations/maintenance/backup/backup#backup-to-local-and-remote-destinations).  

* Server-wide Backup Code Sample:
  {CODE server_wide_backup_configuration@ClientApi\Operations\Maintenance\Backup\ServerWideBackup.cs /}
 
{PANEL/}

{PANEL: Initiate Immediate Backup Execution}

The Backup task is [executed periodically](../../../../server/ongoing-tasks/backup-overview#backup--restore-overview) on its predefined schedule.  
If needed, it can also be executed immediately.  

* To execute an existing backup task immediately, use the `StartBackupOperation` method.  
   {CODE initiate_immediate_backup_execution@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

  * Definition:
    {CODE start_backup_operation@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

  * Parameters:
    
        | Parameter | Type | Functionality |
        | ------ | ------ | ------ |
        | isFullBackup | bool | true: full-backup <br> false: incremental-backup |
        | taskId |  long | The existing backup task ID |


* To verify the execution results, use the `GetPeriodicBackupStatusOperation` method.  
  {CODE get_backup_execution_results@ClientApi\Operations\Maintenance\Backup\Backup.cs /}
   * Return Value:  
     The **PeriodicBackupStatus** object returned from **GetPeriodicBackupStatusOperation** is filled with the previously configured backup parameters and with the execution results.  
     {CODE periodic_backup_status@ClientApi\Operations\Maintenance\Backup\Backup.cs /}
{PANEL/}

{PANEL: Recommended Precautions}
{WARNING: }

* **Don't substitute RavenDB's backup procedures with simply copying the database folder yourself**.  
  The official backup procedure satisfies needs that simply copying the database folder does not. E.g. -  
   * A reliable point-in-time freeze of backed up data.  
   * An ACIDity of backed-up data, to keep its independence during restoration.  
     
* **Remove old backup files regularly**.  
  RavenDB does **not** remove old backup files automatically. 
  As these files continue to aggregate, it is important that you take care of their regular removal.  
  You can use services like crontab (a Linux scheduling procedure) to create an old-backups-removal routine.  

* **Store backup files in a location other than your database's**.  
  Note that backup files are always stored in a local folder first (even when the final backup destination is remote).  
  Make sure that this local folder is not where your database is stored, as a precaution to keep vacant database storage space.  
     
{WARNING/}
{PANEL/}

## Related Articles  
###Server  
- [Backup Overview](../../../../server/ongoing-tasks/backup-overview)

###Client API  
- [Restore](../../../../client-api/operations/maintenance/backup/restore)  
- [Encrypted-Backup : Create & Restore](../../../../client-api/operations/maintenance/backup/encrypted-backup)  
- [Backup FAQ](../../../../client-api/operations/maintenance/backup/faq)  
- [What Is Smuggler](../../../../client-api/smuggler/what-is-smuggler)  

###Studio  
- [The Backup Task](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
- [Create Database: from Backup](../../../../studio/server/databases/create-new-database/from-backup)  
- [Create a Database: General Flow](../../../../studio/server/databases/create-new-database/general-flow)  
- [Create a Database: Encrypted](../../../../studio/server/databases/create-new-database/encrypted)  

###Security  
- [Database Encryption](../../../../server/security/encryption/database-encryption)  
- [Security Overview](../../../../server/security/overview)  
- [Authentication and Certification](../../../../server/security/authentication/certificate-configuration)  

###Migration  
- [Migration](../../../../migration/server/data-migration)   
