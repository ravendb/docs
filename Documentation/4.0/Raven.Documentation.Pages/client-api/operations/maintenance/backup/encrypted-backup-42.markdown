# Encrypted Backup and Restore  

---

{NOTE: }

* With RavenDB 4.0 and 4.1, you can only encrypt a Snapshot - providing that the database has been encrypted.  
* With RavenDB 4.2, you can encrypt a Logical backup as well.  

* In this page:  
  * [Introduction](../../../../client-api/operations/maintenance/backup/encrypted-backup#introduction)  
     * [RavenDB's comprehensive security approach](../../../../client-api/operations/maintenance/backup/encrypted-backup#ravendbs-comprehensive-security-approach)  
     * [Snapshot Encryption](../../../../client-api/operations/maintenance/backup/encrypted-backup#snapshot-encryption)  
     * [Logical Backup encryption](../../../../client-api/operations/maintenance/backup/encrypted-backup#logical-backup-encryption)  
  * [Encrypting Backups](../../../../client-api/operations/maintenance/backup/encrypted-backup#encrypting-backups)  
     * [Authentication](../../../../client-api/operations/maintenance/backup/encrypted-backup#authentication)  
     * [Encrypting a Snapshot](../../../../client-api/operations/maintenance/backup/encrypted-backup#encrypting-a-snapshot)  
     * [Restoring an encrypted Snapshot](../../../../client-api/operations/maintenance/backup/encrypted-backup#restoring-an-encrypted-snapshot)  
{NOTE/}

---

{PANEL: Introduction}

####RavenDB's comprehensive security approach  

Encrypting backup files is just one aspect of RavenDB's comprehensive security approach.  
Other aspects of this approach are implemented in [database encryption](../../../../server/security/encryption/database-encryption) and in [authentication of server-client communication](../../../../server/security/authentication/certificate-configuration).  

{NOTE: You need to enable authentication.}
RavenDB emphasizes the importance of overall security, by allowing backup-encryption only when [server-client communication is authenticated](../../../../server/security/authentication/certificate-configuration#authentication--manual-certificate-configuration).  

* You can enable authentication during the server setup, either [manually](../../../../server/security/authentication/certificate-configuration) or [using the setup-wizard](../../../../start/installation/setup-wizard).  
* Be aware that after enabling authentication, you will to [authenticate](../../../../client-api/operations/maintenance/backup/encrypted-backup#authentication) client-server communication.  

{NOTE/}

{PANEL/}


{PANEL: Encryption Procedures}

####Authentication

As explained in the [introduction](../../../../client-api/operations/maintenance/backup/encrypted-backup#introduction), 
backup-encryption is available only when client-server authentication is enabled.  
Before actually backing up or restoring the database, authenticate your connection.  

* Code sample:
{CODE encrypted_database@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

####Encrypting a Snapshot

As explained in the [overview](../../../../client-api/operations/maintenance/backup/overview#encryption), 
a Snapshot is a duplication of the database files. Therefore the snapshot of an encrypted database is encrypted as well, and the snapshot of an unencrypted database is not encrypted.  
Therefore, assuming your database is encrypted, use a periodic-backup task to create snapshots [as you normally do](../../../../client-api/operations/maintenance/backup/backup#backup-types). The created Snapshot file **will** be encrypted.  

---

####Restoring an encrypted Snapshot

An encrypted Snapshot is restored using **the same authentication key used to encrypt the database**.  
The restoration is in most part similar to restoring an unencrypted snapshot.  
The only difference (besides taking care of [authentication](../../../../client-api/operations/maintenance/backup/encrypted-backup#authentication)), is that you need to pass RestoreBackupOperation the encryption key. Do this using `restoreConfiguration.EncryptionKey`.  

* Code sample:
{CODE restore_encrypted_database@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

---

####Encrypting a Logical backup

You can encrypt a Logical backup whether the database is encrypted or not.  

* When the database **is** encrypted, you can use either the database's encryption key for your backup, or encrypt the backup using your own key.  
  Use `EncryptionKey` to use the database's encryption key.  
  Code sample:
{CODE encrypting_logical_backup_with_database_key@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

* When the database is **not** encrypted, you can use your own key.  
  Use `BackupEncryptionSettings` for a key of your own.   
  Code sample:
{CODE encrypting_logical_backup_with_new_key@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

####Restoring an encrypted Logical backup


{PANEL/}

## Related Articles
