# Backup Encryption  

---

{NOTE: }

* When a database is **encrypted**, you can generate the following backup types for it:  
   * An *encrypted Snapshot* (using the database encryption key)  
   * An *encrypted Logical-Backup* (using the database encryption key, or any key of your choice)  
   * An *un-encrypted Logical-Backup*  

* When a database is **not encrypted**, you can generate the following backup types for it:  
   * An *un-encrypted Snapshot*  
   * An *encrypted Logical-Backup* (providing an encryption key of your choice)  
   * An *un-encrypted* Logical-Backup  

* **Incremental backups** of encrypted logical-backups and snapshots are encrypted as well,
  using the same encryption key provided for the full backup.  

* In this page:  
  * [RavenDB's Security Approach](../../../../client-api/operations/maintenance/backup/encrypted-backup#ravendb)  
     * [Secure Client-Server Communication](../../../../client-api/operations/maintenance/backup/encrypted-backup#secure-client-server-communication)  
     * [Database Encryption](../../../../client-api/operations/maintenance/backup/encrypted-backup#database-encryption)  
  * [Backup-Encryption Overview](../../../../client-api/operations/maintenance/backup/encrypted-backup#backup-encryption-overview)  
     * [Prerequisites to Encrypting Backups](../../../../client-api/operations/maintenance/backup/encrypted-backup#prerequisites-to-encrypting-backups)  
     * [Choosing Encryption Mode & Key](../../../../client-api/operations/maintenance/backup/encrypted-backup#choosing-encryption-mode--key)  
  * [Creating an Encrypted Logical-Backup](../../../../client-api/operations/maintenance/backup/encrypted-backup#creating-an-encrypted-logical-backup)  
     * [For a Non-Encrypted Database](../../../../client-api/operations/maintenance/backup/encrypted-backup#for-a-non-encrypted-database)  
     * [For an Encrypted Database](../../../../client-api/operations/maintenance/backup/encrypted-backup#for-an-encrypted-database)  
  * [Creating an Encrypted Snapshot](../../../../client-api/operations/maintenance/backup/encrypted-backup#creating-an-encrypted-snapshot)  
  * [Restoring an Encrypted Backup](../../../../client-api/operations/maintenance/backup/encrypted-backup#restoring-an-encrypted-backup)  
     * [Restoring an encrypted Logical-Backup](../../../../client-api/operations/maintenance/backup/encrypted-backup#restoring-an-encrypted-logical-backup)  
     * [Restoring a Snapshot](../../../../client-api/operations/maintenance/backup/encrypted-backup#restoring-a-snapshot)  
{NOTE/}

---

{PANEL: RavenDB's Security Approach}

RavenDB's comprehensive security approach includes -  

* **Authentication** and **Certification**  
  to secure your data while it is **transferred between client and server**.  
* **Database Encryption**  
  to secure your data while **stored in the database**.  
* **Backup-Files Encryption**  
  to secure your data while **stored for safe-keeping**.  

---

####Secure Client-Server Communication

To prevent unauthorized access to your data during transfer, apply the following:  

* **Enable secure communication** in advance, during the server setup.  
  Secure communication can be enabled either [manually](../../../../server/security/authentication/certificate-configuration) 
  or [using the setup-wizard](../../../../start/installation/setup-wizard).  
* **Authenticate with the server**.  
  Secure communication requires clients to **certify themselves** in order to access RavenDB.  
  Client authentication code sample:
  {CODE encrypted_database@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  

---

####Database Encryption

Secure the data stored on the server by 
[encrypting your database](../../../../server/security/encryption/database-encryption).  

* **Secure communication to enable database encryption.**  
  An encrypted database can only be created when the 
  [client-server communication is secure](../../../../client-api/operations/maintenance/backup/encrypted-backup#secure-client-server-communication).  

{PANEL/}

{PANEL: Backup-Encryption Overview}

####Prerequisites to Encrypting Backups

* **Logical-Backup**  
  There are no prerequisites to encrypting a Logical-Backup.  
  An encrypted logical-backup can be generated for an **encrypted database** and 
  for a **non-encrypted database**.  
  The encryption key used to generate an encrypted logical-backup of an encrypted database 
  can be different than the original database encryption key.

* **Snapshot**  
  A [snapshot](../../../../client-api/operations/maintenance/backup/backup#snapshot) is an exact image of your database.  
  If the database is **not encrypted**, its snapshot wouldn't be either.  
  If the database is **encrypted**, its snapshot would also be encrypted using the database encryption key.  
  If you want your snapshot to be encrypted, simply take the snapshot of an 
  [encrypted database](../../../../server/security/encryption/database-encryption#creating-an-encrypted-database-using-the-rest-api-and-the-client-api).  

---

####Choosing Encryption Mode & Key

Use the same [Backup](../../../../client-api/operations/maintenance/backup/backup#backup) and [Restore](../../../../client-api/operations/maintenance/backup/restore) methods that are used to create and restore **un**-encrypted backups.  
Specify whether encryption is used, and with which encryption key, 
in the **BackupEncryptionSettings** structure defined within the 
[PeriodicBackupConfiguration](../../../../client-api/operations/maintenance/backup/backup#backup-to-local-and-remote-destinations) object.  

* `BackupEncryptionSettings` definition:  
  {CODE BackupEncryptionSettings_definition@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  
    BackupEncryptionSettings properties:  

    | Property | Type | Functionality |
    | ------------- | ------------- | ----- |
    | **EncryptionMode** | enum | Set the encryption mode. <br> `None` - Use **no encryption** (default mode). <br> `UseDatabaseKey` - Use **the same key the DB is encrypted with** (Logical-Backups & Snapshots). <br> `UseProvidedKey` - Provide **your own encryption key** (Logical-Backups only). |
    | **Key** | string | Pass **your own encryption key** using this parameter (Logical-Backup only). <br> {CODE use_provided_encryption_key@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /} <br> **Note**: When Key is provided and `EncryptionMode` is set to `useDatabaseKey`, the **database key** is used (and not the provided key). |
    `EncryptionMode` definition:  
    {CODE EncryptionMode_definition@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  

{PANEL/}

{PANEL: Creating an Encrypted Logical-Backup}

An encrypted logical-backup can be created for both **encrypted** and **non-encrypted** databases.  

---

####For a Non-Encrypted Database

1. To create a **non-encrypted logical-backup** -  
   **Set** `EncryptionMode = EncryptionMode.None`  
   Or  
   **Don't set** EncryptionMode & Key at all - Default value is: `EncryptionMode.None`  

2. To create an **encrypted logical-backup**, set:
   {CODE-BLOCK:plain}
   EncryptionMode = EncryptionMode.UseProvidedKey,
   Key = "a_key_of_your_choice"
   {CODE-BLOCK/}

---

####For an Encrypted Database

1. To create a non-encrypted logical-backup -  
   Set `EncryptionMode = EncryptionMode.None`  

2. To create an encrypted logical-backup using the database key:  
   **Set** `EncryptionMode = EncryptionMode.UseDatabaseKey`  
   Or  
   **Don't set** EncryptionMode & Key at all - Default value is: `EncryptionMode.UseDatabaseKey`  
    {CODE use_database_encryption_key_full_sample@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  

3. To create an encrypted logical-backup using your own key, set:
   {CODE-BLOCK:plain}
   EncryptionMode = EncryptionMode.UseProvidedKey,
   Key = "a_key_of_your_choice"
   {CODE-BLOCK/}

{PANEL/}

{PANEL: Creating an Encrypted Snapshot}

An encrypted Snapshot can only be created for an encrypted database.  

* To create a **Non-Encrypted Snapshot** (for a non-encrypted database) -  
  **Set** `EncryptionMode = EncryptionMode.None`  
  Or  
  **Don't set** EncryptionMode & Key at all - Default value is: `EncryptionMode.None`  

* To create an **Encrypted Snapshot** (For an encrypted database) -  
  **Set** `EncryptionMode = EncryptionMode.UseDatabaseKey`  
  Or  
  **Don't set** EncryptionMode & Key at all - Default value is: `EncryptionMode.UseDatabaseKey`  
  {CODE encrypted_snapshot@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  

{PANEL/}

{PANEL: Restoring an Encrypted Backup}

To [restore](../../../../client-api/operations/maintenance/backup/restore#configuration-and-execution) 
an encrypted backup you must provide the **key** that was used to encrypt it.  
Pass the key to `RestoreBackupOperation` via `restoreConfiguration.BackupEncryptionSettings`.  
{CODE restore_encrypted_backup@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}

---

####Restoring an encrypted Logical-Backup

A database is [restored](../../../../client-api/operations/maintenance/backup/encrypted-backup#restoring-an-encrypted-backup) from a logical-backup 
to its **unencrypted** form.  
To restore a database and encrypt its contents, you have to address it explicitly.  

* **To encrypt the restored database**:  
  To encrypt the database, pass `RestoreBackupOperation` an encryption key via `restoreConfiguration.EncryptionKey`.  
  Note: This key can be different than the key that was used to encrypt the logical-backup.  
  {CODE restore_encrypted_database@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  

* To restore an **unencrypted** logical-backup:  
  Either provide **no encryption key** to activate the default value (`EncryptionMode.None`), or -  
  Set `EncryptionMode.None` Explicitly.  
  {CODE restore_unencrypted_database@ClientApi\Operations\Maintenance\Backup\EncryptedBackup.cs /}  

---

####Restoring a Snapshot

Restore a snapshot as specified in [Restoring an Encrypted Database](../../../../client-api/operations/maintenance/backup/encrypted-backup#restoring-an-encrypted-backup).  

* The database of an un-encrypted snapshot is restored to its un-encrypted form.  
* The database of an encrypted snapshot is restored to its encrypted form.  
  You must provide the database key that was used to encrypt the snapshot.  

{PANEL/}

## Related Articles  
###Server  
- [Backup Overview](../../../../server/ongoing-tasks/backup-overview)

###Client API  
- [Backup](../../../../client-api/operations/maintenance/backup/backup)  
- [Restore](../../../../client-api/operations/maintenance/backup/restore)  
- [Backup FAQ](../../../../client-api/operations/maintenance/backup/faq)  
- [What Is Smuggler](../../../../client-api/smuggler/what-is-smuggler)  

###Studio  
- [The Backup Task](../../../../studio/database/tasks/backup-task)  
- [Create Database: from Backup](../../../../studio/server/databases/create-new-database/from-backup)  
- [Create a Database: General Flow](../../../../studio/server/databases/create-new-database/general-flow)  
- [Create a Database: Encrypted](../../../../studio/server/databases/create-new-database/encrypted)  

###Security  
- [Database Encryption](../../../../server/security/encryption/database-encryption)  
- [Security Overview](../../../../server/security/overview)  
- [Authentication and Certification](../../../../server/security/authentication/certificate-configuration)  

###Migration  
- [Migration](../../../../migration/server/data-migration)   
