# Encrypted Backup and Restore  

---

{NOTE: }

* With RavenDB 4.0 and 4.1, you can only encrypt a Snapshot - providing that the database has been encrypted.  
  Starting with RavenDB 4.2, you can encrypt a logical backup and restore an encrypted one.  

* In this page:  
  * [Introduction](../../../../client-api/operations/maintenance/backup/encrypted-backup-42#introduction)  
     * [RavenDB's Security Approach](../../../../client-api/operations/maintenance/backup/encrypted-backup-42#ravendbs-security-approach)  
     * [Enable Secure Communication](../../../../client-api/operations/maintenance/backup/encrypted-backup-42#enable-secure-communication)  
  * [Snapshot procedures](../../../../client-api/operations/maintenance/backup/encrypted-backup-42#snapshot-procedures)  
     * [Snapshot Encryption](../../../../client-api/operations/maintenance/backup/encrypted-backup-42#snapshot-encryption)  
     * [Restoring an encrypted Snapshot](../../../../client-api/operations/maintenance/backup/encrypted-backup-42#restoring-an-encrypted-snapshot)  
  * [Logical-backup procedures](../../../../client-api/operations/maintenance/backup/encrypted-backup-42#snapshot-procedures)  
     * [Logical-backup Encryption](../../../../client-api/operations/maintenance/backup/encrypted-backup-42#logical-backup-encryption)  
     * [Restoring an encrypted Logical-backup](../../../../client-api/operations/maintenance/backup/encrypted-backup-42#restoring-an-encrypted-logical-backup)  
{NOTE/}

---

{PANEL: Introduction}

####RavenDB's security approach

Encrypting backup files is just **one respect** of RavenDB's comprehensive security approach.  
Other respects are implemented in -

* [Database encryption](../../../../server/security/encryption/database-encryption)  
* Securing server-client communication using [Authentication and certification](../../../../server/security/authentication/certificate-configuration).  

---

####Enable Secure Communication

RavenDB emphasizes the importance of overall security, by allowing backup-encryption only 
when server-client communication is [authenticated and certified](../../../../server/security/overview).  

* **Enabling authentication and certification**  
  Enable secure client-server communication during the server setup, either [manually](../../../../server/security/authentication/certificate-configuration) or [using the setup-wizard](../../../../start/installation/setup-wizard).  

* **Client authentication procedure**  
  When authentication is enabled, clients are required to certify themselves in order to connect the server.  
  Here's a code sample for this procedure:  
{CODE encrypted_database@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

{PANEL/}


{PANEL: Snapshot Procedures}

####Snapshot Encryption

As explained in the [overview](../../../../client-api/operations/maintenance/backup/overview#encryption) section, 
a Snapshot is an exact duplication of the database files.  
While the snapshot is created, there is **no** encryption or decryption of files. if the database you've created is encrypted, its snapshot will be encrypted in the snapshot as well. If it hasn't been encrypted, the snapshot won't be either.  

* Embed the [client authentication procedure](../../../../client-api/operations/maintenance/backup/encrypted-backup-42#enable-secure-communication) in your code, before actually creating the backup task.  
* Create a snapshot [as is normally done](../../../../client-api/operations/maintenance/backup/backup#backup-types).  

---

####Restoring an encrypted Snapshot

Restoring an encrypted snapshot is almost identical to restoring an unencrypted one.  

* Embed the [client authentication procedure](../../../../client-api/operations/maintenance/backup/encrypted-backup-42#enable-secure-communication) in your code.  
* Pass RestoreBackupOperation an encryption key, using `restoreConfiguration.EncryptionKey`.  
   Use **the same authentication key used to encrypt the database**.
* Code sample:
{CODE restore_encrypted_database@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

{PANEL/}

{PANEL: Logical-backup Procedures}

Creating and restoring a logical backup is nearly similar when it's encrypted and when it isn't.  
There are only two additional steps you need to add when the backup is to be encrypted:  

* Include the [client authentication procedure](../../../../client-api/operations/maintenance/backup/encrypted-backup-42#enable-secure-communication) in your code, before actually creating or restoring the backup.  
* Provide an encryption key.  

---

####The Encryption Key

When the database is encrypted, RavenDB allows you to use the DB's encryption key to encrypt the backup as well.  
You can also encrypt the backup with a key of your own, whether the database is encrypted or not.  

| Method | DB Encryption Key | New Encryption Key |
|:-------------:|:-------------:|:-----:|
| `RestoreBackupOperation` | `RestoreBackupConfiguration.EncryptionKey` | `RestoreBackupConfiguration.BackupEncryptionSettings` |
| `UpdatePeriodicBackupOperation` | `PeriodicBackupConfiguration.EncryptionKey` | `PeriodicBackupConfiguration.BackupEncryptionSettings` |

---

####Logical-backup encryption

* **The encryption key can be the database's key.**  
  If the database is encrypted, you can use **the database's key** to encrypt your backup.  
   * Pass `UpdatePeriodicBackupOperation` the database's key
   * Use `PeriodicBackupConfiguration.EncryptionKey`  
   * Code sample:  
{CODE encrypting_logical_backup_with_new_key@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

* **You can also create the backup with your own encryption key.**  
   * Pass your own key to `UpdatePeriodicBackupOperation`
   * Use `PeriodicBackupConfiguration.BackupEncryptionSettings`.  
   * Code sample:  
{CODE encrypted_logical_full_backup@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

---

####Restoring an encrypted logical-backup

* **If you backed the database up using the database encryption key**  
   * Pass `RestoreBackupOperation` the database's key
   * Use `RestoreBackupConfiguration.EncryptionKey`  
   * Code sample:  
{CODE restore_encrypting_logical_backup_with_database_key@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

* **If you used a new encryption key to back the database up**  
   * Pass your own key to `RestoreBackupOperation`
   * Use `RestoreBackupConfiguration.BackupEncryptionSettings`.  
   * Code sample:  
{CODE encrypting_logical_backup_with_new_key@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

{PANEL/}

## Related Articles
