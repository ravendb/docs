# Backup Encryption  

---

{NOTE: }

* The snapshot of an encrypted database is encrypted as well.  
  The snapshot of an unencrypted database is not uncrypted.  
* Encrypting logical backups **is** supported by RavenDB 4.2 and on.  

* In this page:  
  * [RavenDB's Security Approach](../../../../client-api/operations/maintenance/backup/encrypted-backup#ravendb)  
     * [Secure Server-Client Communication](../../../../client-api/operations/maintenance/backup/encrypted-backup#secure-server-client-communication)  
     * [Database Encryption](../../../../client-api/operations/maintenance/backup/encrypted-backup#database-encryption)  
  * [Backup-Encryption Overview](../../../../client-api/operations/maintenance/backup/encrypted-backup#backup-encryption-overview)  
     * [Prerequisites to Encrypting Backups](../../../../client-api/operations/maintenance/backup/encrypted-backup#prerequisites-to-encrypting-backups)  
     * [Choosing Encryption Mode](../../../../client-api/operations/maintenance/backup/encrypted-backup#choosing-encryption-mode)  
     * [The Encryption Key](../../../../client-api/operations/maintenance/backup/encrypted-backup#the-encryption-key)  
  * [Creating an Encrypted Backup](../../../../client-api/operations/maintenance/backup/encrypted-backup#creating-an-encrypted-backup)  
     * [Creating an Encrypted Logical-Backup](../../../../client-api/operations/maintenance/backup/encrypted-backup#creating-an-encrypted-logical-backup)  
     * [Creating an Encrypted Snapshot](../../../../client-api/operations/maintenance/backup/encrypted-backup#creating-an-encrypted-snapshot)  
  * [Restoring an Encrypted Backup](../../../../client-api/operations/maintenance/backup/encrypted-backup#restoring-an-encrypted-backup)  
     * [Restoring a Logical-Backup](../../../../client-api/operations/maintenance/backup/encrypted-backup#restoring-a-logical-backup)  
     * [Restoring a Snapshot](../../../../client-api/operations/maintenance/backup/encrypted-backup#restoring-a-snapshot)  
{NOTE/}

---

{PANEL: RavenDB's Security Approach}

The encryption of backup files secures data **while stored for safe-keeping**.  
This is just **one respect** of RavenDB's comprehensive security approach.  
Other respects are -

* Securing data **while transferred between server and client**
* Securing data **while stored in the database**.  

These subjects are briefly introduced here, to help you put backup-encryption in context.  

---

####Secure Server-Client Communication

You can prevent unauthorized access to your data during transfer, by enabling [authentication and Certification](../../../../server/security/authentication/certificate-configuration) 
of server-client communication.  

* **Enable secure communication** in advance, during the server setup.  
  You can enable it while installing the server either [manually](../../../../server/security/authentication/certificate-configuration) or [using the setup-wizard](../../../../start/installation/setup-wizard).  
* **Authenticate with the server**.  
  Be aware that enabling secure communication will require clients to **certify themselves** in order to access RavenDB.  
  Client authentication code sample:
  {CODE encrypted_database@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  

---

####Database Encryption

Database encryption prevents unauthorized access to your data by [encrypting it on the server](../../../../server/security/encryption/database-encryption).  

* **Secure communication to enable database encryption.**  
  RavenDB emphasizes the importance of overall security, by allowing the creation of an encrypted database only 
when [server-client communication is secure](../../../../client-api/operations/maintenance/backup/encrypted-backup#secure-server-client-communication). 
If you want to encrypt your database, you need to enable authentication and certification first.  

{PANEL/}

{PANEL: Backup-Encryption Overview}

####Prerequisites to Encrypting Backups

* There are **no prerequisites** to encrypting a **logical-backup**.  
  An encrypted logical-backup can be generated for an encrypted database **and** for a non-encrypted database.  
  You can also generate an encrypted logical backup of an encrypted database using a _different_ encryption key.  
* If you want your **snapshot** to be encrypted, simply take the snapshot of an [encrypted database](../../../../server/security/encryption/database-encryption#creating-an-encrypted-database-using-the-rest-api-and-the-client-api).  

  A [snapshot](../../../../client-api/operations/maintenance/backup/backup#snapshot) is an exact image of the database. 
  If the database is encrypted, so would be its snapshot. If the DB is not encrypted, a snapshot of it wouldn't be either.  

---

####Choosing Encryption Mode

Use the same [Backup](../../../../client-api/operations/maintenance/backup/backup#backup) and [Restore](../../../../client-api/operations/maintenance/backup/restore) methods you use to create and restore **un**encrypted backups.  
Pass them a **BackupEncryptionSettings** structure to determine whether encryption is used, and if so - with which [encryption key](../../../../client-api/operations/maintenance/backup/encrypted-backup#the-encryption-key).  

* `BackupEncryptionSettings` definition:  
  {CODE BackupEncryptionSettings_definition@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  
    BackupEncryptionSettings parameters:  

    | Parameter | Type | Functionality |
    | ------------- | ------------- | ----- |
    | **EncryptionMode** | enum | Set the encryption mode. <br> `none` - Use **no encryption** (this is the default mode). <br> `UseProvidedKey` - Provide **your own encryption key**. <br> `UseDatabaseKey` - Use **the same key the DB is encrypted with**. |
    | **Key** | string | Pass your own encryption key using this parameter |
    `EncryptionMode` definition:  
    {CODE EncryptionMode_definition@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  

---

####The Encryption Key

To encrypt your backup, either **use the database's encryption key** or **choose a new key**.  

* **Using the database's Key**  
  If your database is encrypted, you can utilize its existing encryption key for your backup as well.  
     {CODE use_database_encryption_key@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  
   
    {NOTE: Both logical-backups and snapshots can use the same key as the database.}
    {NOTE/}

* **Using a key of your choice**  
  You can choose a key other than the database's to encrypt your logical-backup with.  
     {CODE use_provided_encryption_key@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  

    {NOTE: Only a logical-backup can use an encryption-key different than the database's.}
    {NOTE/}

    {NOTE: Use this key when creating an encrypted backup of an unencrypted database.}
    {NOTE/}


{PANEL/}

{PANEL: Creating an Encrypted Backup}

####Creating an Encrypted Logical-Backup  

Creating an encrypted logical-backup is nearly similar to [creating a non-encrypted one](../../../../client-api/operations/maintenance/backup/backup#logical-backup-or-simply-backup).  

* In addition to the regular backup procedure, take these steps:  
   * If secure-communication is enabled on your server, Include the [client authentication procedure](../../../../client-api/operations/maintenance/backup/encrypted-backup#secure-server-client-communication) in your code.  
   * Choose the [encryption mode](../../../../client-api/operations/maintenance/backup/encrypted-backup#choosing-encryption-mode).  
   * Provide an [encryption key](../../../../client-api/operations/maintenance/backup/encrypted-backup#the-encryption-key) if needed.  

* Code sample: encrypting a backup **using the database encryption key**.  
  {CODE use_database_encryption_key_full_sample@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  

* Code sample: encrypting a backup **with an encryption key of your choice** -  
 {CODE use_provided_encryption_key_full_sample@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  

* To create an **unencrypted** backup:  
   * Either let the default setting (`EncryptionMode = EncryptionMode.None`) do the work for you, or -
   * Explicitly choose `EncryptionMode.None`.
     {CODE explicitly_choose_no_encryption@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  

{NOTE: You CAN encrypt the backup of an unencrypted database.}
Pass the encryption key using `EncryptionMode.UseProvidedKey`.  
{CODE use_provided_encryption_key@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}
{NOTE/}

{NOTE: Default encryption-key}

If you don't explicitly state the encryption mode and key, their default value will be -

* If the database is unencrypted, the backup is unencrypted as well.  
* If the database is encrypted, the backup is encrypted using the database's key.  
{NOTE/}

---

####Creating an Encrypted Snapshot

* If you want your snapshot to be encrypted, [take the snapshot of an encrypted database](../../../../client-api/operations/maintenance/backup/encrypted-backup#prerequisites-to-encrypting-backups).  
* If [secure-communication](../../../../client-api/operations/maintenance/backup/encrypted-backup#secure-server-client-communication) is enabled on your server, Include the **client authentication procedure** in your code.  
* Create a snapshot [as you normally would](../../../../client-api/operations/maintenance/backup/backup#backup-types).  

{NOTE: Default encryption-key}

The snapshot of an encrypted database has the same encryption key as the database's.  
The snapshot of an unencrypted database is not encrypted.  
{NOTE/}

{NOTE: The incremental backups of an encrypted snapshot are encrypted as well, using the same key.}
{NOTE/}

{PANEL/}

{PANEL: Restoring an Encrypted Backup}

To restore your encrypted backup, pass [RestoreBackupOperation](../../../../client-api/operations/maintenance/backup/restore#configuration-and-execution) the **key** used to encrypt it.  
Pass the key using `restoreConfiguration.BackupEncryptionSettings`.  
{CODE restore_encrypted_backup@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}

---

####Restoring a Logical-Backup

A database is [restored](../../../../client-api/operations/maintenance/backup/encrypted-backup#restoring-an-encrypted-backup) from a logical-backup 
to its **unencrypted** form.  
If you want to encrypt the restored database, you have to address it explicitly.  

* **To encrypt the restored database**:  
  Pass `RestoreBackupOperation` an encryption key using `restoreConfiguration.EncryptionKey`.  
  {CODE restore_encrypted_database@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  

To restore an **unencrypted** logical-backup -

* Either provide **no encryption key** to activate the default value (`EncryptionMode.None`), or -
* Choose `EncryptionMode.None` Explicitly.  
  {CODE restore_unencrypted_database@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  

---

####Restoring a Snapshot

There are no special requirements to [restoring](../../../../client-api/operations/maintenance/backup/encrypted-backup#restoring-an-encrypted-backup) snapshots.

* The snapshot of an encrypted database is encrypted with the database's key.  
* The database of an encrypted snapshot is restored to its encrypted form.  

{PANEL/}

## Related Articles
**Client Articles**:  
[Backup Overview](../../../../server/ongoing-tasks/backup-overview)  
[Backup](../../../../client-api/operations/maintenance/backup/backup)  
[Restore](../../../../client-api/operations/maintenance/backup/restore)  

**Studio Articles**:  
[The Backup Task](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
[Create Database from Backup](../../../../studio/server/databases/create-new-database/from-backup)  

**Security**:  
[Database Encryption](../../../../server/security/encryption/database-encryption)  
[Security Overview](../../../../server/security/overview)  
[Authentication and Certification](../../../../server/security/authentication/certificate-configuration)  
