# Encrypted Backup and Restore
---

{NOTE: }

* RavenDB 4.0 and 4.1 support the encryption of a Snapshot, when the database is encrypted to begin with.  
  Starting with version 4.2, RavenDB also supports the encryption of a standard backup, whether the database is encrypted or not.  

* In this page:  
  * [Encrypted Backup](../../../../client-api/operations/maintenance/backup/backup#introduction)  
     * [Encrypted Standard-Backup](../../../../client-api/operations/maintenance/backup/backup#introduction)  
     * [Encrypted Snapshot](../../../../client-api/operations/maintenance/backup/backup#introduction)  
     * [Incremental Backups Encryption](../../../../client-api/operations/maintenance/backup/backup#introduction)  
  * [Restoring an encrypted backup](../../../../client-api/operations/maintenance/backup/backup#introduction)  
     * [Restoring an encrypted standard-Backup](../../../../client-api/operations/maintenance/backup/backup#introduction)  
     * [Restoring an encrypted Snapshot](../../../../client-api/operations/maintenance/backup/backup#introduction)  
     * [Restoring encrypted Incremental Backups (if there is such a thing)](../../../../client-api/operations/maintenance/backup/backup#introduction)  
{NOTE/}

---

{PANEL: Encrypted Backup}

####Encrypted Standard-Backup  

* RavenDB 4.0 doesn't support standard-backup encryption.  
  If you wish to encrypt your backup, you can -  
   * Upgrade to RavenDB 4.2, which does support backup encryption.
   * Encrypt your database and back it up using a Snapshot.  

* Code Sample (for RavenDB 4.2)  

####Encrypted Snapshot  

* When the database is encrypted, snapshots are encrypted as well.  
* When the database in not encrypted, snapshots aren't encrypted too.  

* Code Sample

####Incremental Backups Encryption  

* How are Incremental backups stored? (in plain-text? encrypted?)
   * When they supplement an encrypted database/Snapshot
   * When they supplement an encrypted standard-backup

   * Code Sample

{PANEL/}

{PANEL: Restoring an encrypted backup}

####Restoring an encrypted standard-Backup  


####Restoring an encrypted Snapshot  

* An encrypted Snapshot IS supported by earlier (4.0, 4.1) RavenDB versions.  
  Creating a Snapshot when the database is encrypted, produces an encrypted snapshot.  
   * Provide the encryption key.  
     Restoring an encrypted snapshot requires you to provide the original database encryption key.  

####Restoring encrypted Incremental Backups (if there is such a thing)  

* 

{PANEL/}

{PANEL: FAQ}

 * Q:  
  * A:  
{PANEL/}

## Related Articles

[Backup using code](../../../../client-api/operations/maintenance/backup/backup)
[Restore using code](../../../../client-api/operations/maintenance/backup/restore)
[Backup using the Studio](../../../../studio/database/tasks/ongoing-tasks/backup-task)
[Restore using the Studio](../../../../studio/server/databases/create-new-database/from-backup)
