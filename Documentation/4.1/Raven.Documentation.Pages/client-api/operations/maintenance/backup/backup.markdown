# Backup
---

{NOTE: }

* Create a backup of your data to secure it or to preserve a copy of it in its current state for future reference.  

* RavenDB's ongoing Backup task is designed to run [continuously](../../../../server/ongoing-tasks/backup-overview#backup--restore-overview).  
  You can run it as a one-time operation as well, using [export](../../../../client-api/smuggler/what-is-smuggler#export) or executing a backup-task [immediately](../../../../client-api/operations/maintenance/backup/backup#initiate-immediate-backup-execution).  

* In this page:  
  * [Backup Types](../../../../client-api/operations/maintenance/backup/backup#backup-types)  
      * [Logical-Backup](../../../../client-api/operations/maintenance/backup/backup#logical-backup-or-simply-backup)  
      * [Snapshot](../../../../client-api/operations/maintenance/backup/backup#snapshot)  
  * [Backup Scope](../../../../client-api/operations/maintenance/backup/backup#backup-scope)  
      * [Full Backup](../../../../client-api/operations/maintenance/backup/backup#full-backup)  
      * [Incremental Backup](../../../../client-api/operations/maintenance/backup/backup#incremental-backup)  
  * [Backup to Remote Destinations](../../../../client-api/operations/maintenance/backup/backup#backup-to-remote-destinations)  
  * [Initiate Immediate Backup Execution](../../../../client-api/operations/maintenance/backup/backup#initiate-immediate-backup-execution)  
  * [Recommended Precautions](../../../../client-api/operations/maintenance/backup/backup#recommended-precautions)  

{NOTE/}

---

{PANEL: Backup Types}

####Logical-Backup  

* Data is backed-up in [compressed](../../../../server/ongoing-tasks/backup-overview#compression) JSON files.  

* During restoration, RavenDB -  
   * Re-inserts all data into the database.  
   * Re-indexes the data.  

* Restoration Time is therefore **slower** than that required when restoring a Snapshot.  

* Backup size is **significantly smaller** than that of a Snapshot.

* Code Sample:
  {CODE logical_full_backup_every_3_hours@ClientApi\Operations\Maintenance\Backup\Backup.cs /}
  Note the usage of [Cron scheduling](https://en.wikipedia.org/wiki/Cron) when setting backup frequency.  

---

####Snapshot

* **Snapshot** backups are available for **Enterprise subscribers only**.  
    A SnapShot is a compressed binary duplication of the [database and journals](../../../../server/storage/directory-structure#storage--directory-structure) file structure at a given point-in-time.  

* During restoration -
   * Re-indexing is not required.  
   * Re-inserting data into the database is not required.  

* Restoration is typically **faster** than that of a logical backup.  

* Snapshot size is typically **larger** than that of a logical backup.  

* Code Sample:  
  {CODE backup_type_snapshot@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

---

####Basic Comparison Between a Logical-Backup and a Snapshot:

  | Backup Type | Stored Format | Restoration speed | Task characteristics | Size
  | ------ | ------ | ------ | ------ |
  | Snapshot | Compressed Binary Image | Fast | Ongoing Background Task | Larger than logical-backup's
  | Logical backup |  Compressed Textual Data | Slow | Ongoing Background Task | Smaller than Snapshot's

{NOTE: Make sure your server has access to the local backup path.}
Verify that RavenDB is allowed to store backup files in the path set in `LocalSettings.FolderPath`.
{NOTE/}



{PANEL/}

{PANEL: Backup Scope}

As described in [the overview](../../../../server/ongoing-tasks/backup-overview#backing-up-and-restoring-a-database), a backup task can create **full** and **incremental** backups.  

* Both backup operations add a single new backup file to the backup folder each time they run. leaving existing backup files untouched.  
* Both are operated by an **ongoing task**.  

####Full Backup

* There are no preliminary conditions to creating a full backup. Any node can perform this task.  

* To run a full backup, set `FullBackupFrequency`.
  {CODE backup_full_backup@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

---

####Incremental Backup

* An incremental-backup file is **always in JSON format**. 
  It is so even when the full backup it supplements is a binary snapshot.  
* Incremental Backup ownership
   * An incremental backup can be created only by the node that currently owns the backup task.  
   * The ownership is granted dynamically by the cluster.  
   * A node can run the incremental backup task, only after creating a full backup at least once.  

* To run an incremental backup, set `IncrementalBackupFrequency`.
  {CODE backup_incremental_backup@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

{PANEL/}

{PANEL: Backup to Remote Destinations}

* Backups can be made locally, as well as to a set of remote locations including -  
   * A network path
   * An FTP/SFTP target
   * Amazon S3 
   * Azure Storage 
   * Amazon Glacier 

* RavenDB will store data in a local folder first, and transfer it to the remote destination from the local one.  

* Remote Backup Destinations Code Sample:
  {CODE backup_remote_destinations@ClientApi\Operations\Maintenance\Backup\Backup.cs /}
 
 {INFO: Tip}
    You can change youer Access Management in Amazon S3 so the user doing backup don't have full access, 
    read more [here](https://docs.aws.amazon.com/IAM/latest/UserGuide/introduction_access-management.html).   
    for example:
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


{PANEL: Initiate Immediate Backup Execution}

The Backup task [runs continuously](../../../../server/ongoing-tasks/backup-overview#backup--restore-overview) as an ongoing task, but you can also operate the task immediately if you wish to.  

* To execute an existing backup task immediately, use the `StartBackupOperation` method.  
   {CODE initiate_immediate_backup_execution@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

  * Definition:
    {CODE start_backup_operation@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

  * Parameters:
    
        | Parameter | Type | Functionality |
        | ------ | ------ | ------ |
        | isFullBackup | bool | true: full backup <br> false: incremental backup |
        | taskId |  long | The task ID returned by RavenDB |


* To verify the execution results, use the `GetPeriodicBackupStatusOperation` method.  
  {CODE get_backup_execution_results@ClientApi\Operations\Maintenance\Backup\Backup.cs /}
   * Return Value:  
     The status structure that **GetPeriodicBackupStatusOperation** returns, is filled with backup parameters you previously configured and with the execution results.  
     {CODE periodic_backup_status@ClientApi\Operations\Maintenance\Backup\Backup.cs /}
{PANEL/}

{PANEL: Recommended Precautions}
{WARNING: }

* **Don't substitute RavenDB's backup procedures with simply copying the database folder yourself**.  
  The official backup procedure satisfies needs that simply copying the database folder does not. E.g. -  
   * A reliable point-in-time freeze of backed up data.  
   * An ACIDity of backed-up data, to keep its independence during restoration.  
     
* **Regularly remove backup files**.  
  Note that RavenDB does **not** automatically remove backup files. As they continue to aggregate, it is important that you take care of their regular removal.  
  You can use services like crontab (a linux scheduling procedure) to create an old-backups-removal routine.  

* **Locate backup files in a location other than your database's**.  
  Note that backup files are always stored in a local folder first (even when the final backup destination is remote).  
  Make sure that this local folder is not where your database is stored, as a precaution to keep vacant database storage space.  
     
{WARNING/}
{PANEL/}

 
## Related Articles

**Studio Articles**:   
[Create a Database : From Backup](../../../../studio/server/databases/create-new-database/from-backup)   
[Create a Database : General Flow](../../../../studio/server/databases/create-new-database/general-flow)        
[Create a Database : Encrypted](../../../../studio/server/databases/create-new-database/encrypted)      
[The Backup Task](../../../../studio/database/tasks/ongoing-tasks/backup-task)    

**Client Articles**:  
[Restore](../../../../client-api/operations/maintenance/backup/restore)   
[Operations: How to Restore a Database from Backup](../../../../client-api/operations/server-wide/restore-backup)    
[What Is Smuggler](../../../../client-api/smuggler/what-is-smuggler)  
[Encrypted-Backup backup & restore](../../../../client-api/operations/maintenance/backup/encrypted-backup)   

**Server Articles**:  
[Backup Overview](../../../../server/ongoing-tasks/backup-overview)

**Migration Articles**:  
[Migration](../../../../migration/server/data-migration)   

