# Encrypted Backup and Restore  

---

{NOTE: }

* With RavenDB 4.0 and 4.1, you can only encrypt a Snapshot - providing that the database has been encrypted.  
  Starting with RavenDB 4.2, you can encrypt a logical backup and restore an encrypted one.  

* In this page:  
  * [Introduction](../../../../client-api/operations/maintenance/backup/encrypted-backup#introduction)  
     * [RavenDB's Security Approach](../../../../client-api/operations/maintenance/backup/encrypted-backup#ravendbs-security-approach)  
     * [Enable Secure Communication](../../../../client-api/operations/maintenance/backup/encrypted-backup#enable-secure-communication)  
     * [The Encryption Key](../../../../client-api/operations/maintenance/backup/encrypted-backup#the-encryption-key)  
     * [Encryption mode](../../../../client-api/operations/maintenance/backup/encrypted-backup#encryption-mode)  
  * [Creating Encrypted Backups](../../../../client-api/operations/maintenance/backup/encrypted-backup#creating-encrypted-backups)  
     * [Creating an Encrypted Logical-backup](../../../../client-api/operations/maintenance/backup/encrypted-backup#creating-an-encrypted-logical-backup)  
     * [Creating an Encrypted Snapshot](../../../../client-api/operations/maintenance/backup/encrypted-backup#creating-an-encrypted-snapshot)  
  * [Restoring Encrypted Backups](../../../../client-api/operations/maintenance/backup/encrypted-backup#restoring-encrypted-backups)  
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
when server-client communication is authenticated and certified.  

* **Enabling authentication and certification**  
  Enable secure client-server communication during the server setup, either [manually](../../../../server/security/authentication/certificate-configuration) or [using the setup-wizard](../../../../start/installation/setup-wizard).  

* **Client authentication procedure**  
  When authentication is enabled, clients are required to certify themselves in order to connect the server.  
  Here's a code sample for this procedure:  
{CODE encrypted_database@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

---

####The Encryption Key

When you encrypt your backup, you can use either **the database's** encryption key or **a new key** of your choice.  

* **To use the database's Key**  
  If your database is encrypted, you can use its encryption key for your backup as well.  
   * This option is relevant for both logical-backups **and** snapshots.  
   * Sample:  
     {CODE use_database_encryption_key@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  
   * When your database is encrypted and your backup type is **Snapshot**, using the database's encryption key is **mandatory**.  
     This is because creating an encrypted snapshot doesn't actually encrypt any file, but simply duplicates files already encrypted.  

* **To use a key of your choice**  
  You can encrypt a logical-backup using an encryption key of your choice, regardless of the database's key (if any).  
   * This option is relevant for logical-backups **only**.  
   * Sample:  
     {CODE use_provided_encryption_key@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

---

####Encryption mode

Use `BackupEncryptionSettings` to determine whether to use encryption at all, and if so - with [which key](../../../../client-api/operations/maintenance/backup/encrypted-backup#the-encryption-key).  

* `BackupEncryptionSettings` Parameters:  

    | Parameter | Value | Functionality |
    | ------------- | ------------- | ----- |
    | **EncryptionMode** | enum | `none` - No encryption <br> `UseProvidedKey` - Choose your own key <br> `UseDatabaseKey` - Use the same key the DB is encrypted with |
    | **Key** | string | to use your own key, provide it here |

   * Sample of usage when creating a backup:  
     {CODE demonstrate_BackupEncryptionSettings_for_backup@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

   * Sample of usage when restoring:  
     {CODE demonstrate_BackupEncryptionSettings_for_restore@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  


{PANEL/}

{PANEL: Creating Encrypted Backups}

####Creating an Encrypted Logical-backup  

Creating and restoring encrypted logical-backups is nearly similar to [creating](../../../../client-api/operations/maintenance/backup/backup) and [restoring](../../../../client-api/operations/maintenance/backup/restore) non-encrypted ones.  
The additional steps you need to take for an encrypted backup are -  

* Include the [client authentication procedure](../../../../client-api/operations/maintenance/backup/encrypted-backup#enable-secure-communication) in your code.  
* Provide an [encryption key](../../../../client-api/operations/maintenance/backup/encrypted-backup#the-encryption-key).  

* **To create the backup using the database encryption key:**  
  Pass `UpdatePeriodicBackupOperation` the database's key using `PeriodicBackupConfiguration.BackupEncryptionSettings`.  
  E.g. -  
  {CODE use_database_encryption_key_full_sample@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

* **To create the backup using your own encryption key:**  
  Pass `UpdatePeriodicBackupOperation` your key using `PeriodicBackupConfiguration.BackupEncryptionSettings`.  
  E.g. -  
 {CODE use_provided_encryption_key_full_sample@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

---

####Creating an Encrypted Snapshot

A [snapshot](../../../../client-api/operations/maintenance/backup/backup#snapshot) is an exact copy of the database files. 
If the database is encrypted, so would be its snapshot. If the database is not, the snapshot won't be either.  

* If you want your snapshot to be encrypted, take the snapshot of an encrypted database.  
* Include the [client authentication procedure](../../../../client-api/operations/maintenance/backup/encrypted-backup#enable-secure-communication) in your code.  
* Create a snapshot [as you normally would](../../../../client-api/operations/maintenance/backup/backup#backup-types).  

{NOTE: If the database is encrypted, the snapshot is encrypte with the same encryption key.}
{NOTE/}

{PANEL/}

{PANEL: Restoring Encrypted Backups}

Restoring logical-backups is similar to restoring snapshots.  
Restoring a backup created using **the same key as the DB**, is similar to restoring a backup created using **a distinctive key**.  
In both cases, it matters just that you use the same key you used while creating the backup.  

* Include the [client authentication procedure](../../../../client-api/operations/maintenance/backup/encrypted-backup#enable-secure-communication) in your code.  
* Pass your encryption key to `RestoreBackupOperation` using `restoreConfiguration.BackupEncryptionSettings`.  
* Code sample:
{CODE restore_encrypted_database@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

{PANEL/}

## Related Articles
