# Encrypted Backup and Restore  

---

{NOTE: }

* With RavenDB 4.0 and 4.1, you can only encrypt a Snapshot - providing that the database has been encrypted.  
* With RavenDB 4.2, you can encrypt a logical backup as well.  

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

---

####Snapshot encryption

Snapshot encryption is automatic.  

* When the database is **encrypted**, the Snapshot is **encrypted** as well.  
* When the database is **un-encrypted**, the Snapshot is **un-encrypted** as well.  

To restore an encrypted Snapshot, use the same encryption key used for the database encryption.  

---

####Logical-backup encryption  

{NOTE: Logical-backup encryption is supported by RavenDB version 4.2 and on.  }
{NOTE/}

{PANEL/}


{PANEL: Encrypting Backups}

####Authentication

As explained in the [introduction](../../../../client-api/operations/maintenance/backup/encrypted-backup#introduction), 
backup-encryption is available only when client-server authentication is enabled.  
Before actually backing up or restoring the database, authenticate your connection.  

* Code sample:
{CODE encrypted_database@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

####Encrypting a Snapshot

As explained in the [overview](../../../../client-api/operations/maintenance/backup/overview#encryption), a Snapshot is **automatically** encrypted if the database is encrypted and unencrypted if the database is not.  
So assuming your database is encrypted, use a periodic-backup task to create snapshots [as you normally do](../../../../client-api/operations/maintenance/backup/backup#backup-types). The created Snapshot file **will** be encrypted.  

---

####Restoring an encrypted Snapshot

An encrypted Snapshot is restored using **the same authentication key used to encrypt the database**.  
The restoration is in most part similar to restoring an unencrypted snapshot.  
The only difference (besides taking care of [authentication](../../../../client-api/operations/maintenance/backup/encrypted-backup#authentication)), is that you need to pass RestoreBackupOperation the encryption key. Do this using `restoreConfiguration.EncryptionKey`.  

* Code sample:
{CODE restore_encrypted_database@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

{PANEL/}

## Related Articles
