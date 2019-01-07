# Backup
---

{NOTE: }

* Backup "freezes" ACID captures of your data in chosen points of time.  

* Backup [runs as an ongoing task](../../../../client-api/operations/maintenance/backup/overview#backup--restore-overview).  
   * If you need or prefer to manually backup data, you can use the [export](../../../../client-api/smuggler/what-is-smuggler#export) feature.  

* In this page:  
  * [Backup Types](../../../../client-api/operations/maintenance/backup/backup#backup-types)  
      * [Logical Backup](../../../../client-api/operations/maintenance/backup/backup#logical-backup-or-simply-backup)  
      * [Snapshot](../../../../client-api/operations/maintenance/backup/backup#snapshot)  
  * [Backup Scope](../../../../client-api/operations/maintenance/backup/backup#backup-scope)  
      * [Full Backup](../../../../client-api/operations/maintenance/backup/backup#full-backup)  
      * [Incremental Backup](../../../../client-api/operations/maintenance/backup/backup#incremental-backup)  
      * [Point-in-Time Backup](../../../../client-api/operations/maintenance/backup/backup#point-in-time-backup)  
  * [Backup to Remote Destinations](../../../../client-api/operations/maintenance/backup/backup#backup-to-remote-destinations)  
  * [Recommended Cautions](../../../../client-api/operations/maintenance/backup/backup#recommended-cautions)  

{NOTE/}

---

{PANEL: Backup Types}

####Logical Backup (or simply "Backup")

* Data is backed-up in [compressed](../../../../client-api/operations/maintenance/backup/overview#compression) JSON files.  

* During restoration, RavenDB -  
   * Re-inserts all data into the database.  
   * Re-indexes the data.  

* Restoration Time is therefore **slower** than that required when restoring a Snapshot.  

* Needed storage space is **smaller** than that required to store a Snapshot image.  

* Code Sample:
  {CODE logical_full_backup_every_3_hours@ClientApi\Operations\Maintenance\Backup\Backup.cs /}
  Note the usage of [Cron scheduling](https://en.wikipedia.org/wiki/Cron) when setting backup frequency.  

####Snapshot

* **Snapshot** backups are available for **Enterprise subscribers only**.  
    A SnapShot is a binary duplication of the [database and journals](../../../../server/storage/directory-structure#storage--directory-structure) file structure at a given point-in-time.  

* During restoration -
   * Re-indexing is not required.  
   * Re-inserting data into the database is not required.  

* Restoration Time is typically **faster** than that needed when restoring a logical backup.  

* Needed storage space is **larger** than that required for a logical backup.  

* Code Sample:  
  {CODE backup_type_snapshot@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

####Basic comparison between a logical backup and a snapshot:

  | Backup Type | Storage size | Stored Format | Restoration speed | Task characteristics |
  | ------ | ------ | ------ | ------ | ------ |
  | Snapshot | Large  | Compressed Binary Image | Fast | Ongoing Background Task |
  | Backup | Small | Compressed Textual Data | Slow | Ongoing Background Task |

{PANEL/}

{PANEL: Backup scope}

####Full Backup

* A full backup stores all your data from the dawn of time until moment of backup in a single file.  

* There are no preliminary conditions for creating a full backup, It can be done by any node.  

* Full Backup Code Sample:
  {CODE backup_full_backup@ClientApi\Operations\Maintenance\Backup\Backup.cs /}


####Incremental Backup

* An incremental backup is used to save the changes that occured in your database since the last backup.  

* An incremental-backup file is always a dump of JSON-files.  
  It is so even when the full backup has been a binary snapshot.  

* Incremental Backup files are typically very small.  

* Incremental Backup ownership
   * An incremental backup can be created only by the node that currently owns the incremental-backup task.  
   * The ownership is granted dynamically by the cluster.  
   * A node can be apointed to own the incremental backup task, only after creating a full backup at least once.  

* Incremental Backup Code Sample:
  {CODE backup_incremental_backup@ClientApi\Operations\Maintenance\Backup\Backup.cs /}


####Point-In-Time Backup  

* An elegant way to keep many restoration points without consuming too much storage space, is to use the **Revisions** feature.  
  * [Enable the Revisions feature](../../../../server/extensions/revisions#configuration).  
  * Routinely create [incremental backup](../../../../client-api/operations/maintenance/backup/backup#incremental-backup).  
  * To preserve free storage, erase revisions after backup.  
  * To restore data as it was in a certain moment -  
     - Locate and restore the correct incremental backup.  
     - Restore the correct [data revision](../../../../client-api/session/revisions/loading#revisions--loading-revisions) within this backup.  
{PANEL/}

{PANEL: Backup to Remote Destinations}

* Backups can be made locally, as well as to a set of remote locations including -  
   * A network path
   * an FTP/SFTP target
   * Amazon S3 
   * Azure Storage 
   * Amazon Glacier 

* RavenDB will store data in a local folder first, and transfer it to the remote destination from the local one.  

* Remote Backup Destinations Code Sample:
  {CODE backup_remote_destinations@ClientApi\Operations\Maintenance\Backup\Backup.cs /}

{PANEL/}

{PANEL: Recommended Cautions}
{WARNING: }

* Do **not** substitute RavenDB's backup procedures, with simply copying the database folder yourself.  
  Learn the backup procedure and implement it as recommended.  
  Among other things, the official backup procedure provides -   
   * A reliable point-in-time freeze of backed up data.  
   * An ACIDity of backed-up data, so it wouldn't be dependent upon various files and connections when restored.  
     
* Remove backup files  
   * RavenDB does **not** automatically remove aging backup files. Make sure yourself they are regularly removed.  
     You can use services like crontab (a linux scheduling procedure) to create an old-backups-removal routine.  
   * Remotely backed-up files are stored in a local folder first.  
     Make sure this local folder is not where your database is, to prevent the backup procedure from consuming database storage space.  
     
{WARNING/}
{PANEL/}

## Related Articles (to be revised, ignore)

####Client
[Restore using code](../../../../client-api/operations/maintenance/backup/restore)  

####Studio
[Backup using the Studio](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
[Restore using the Studio](../../../../studio/server/databases/create-new-database/from-backup)  
