# Encrypted Backup and Restore  

---

{NOTE: }

* With RavenDB 4.0 and 4.1, you can only encrypt a Snapshot - providing that the database has been encrypted.  
* With RavenDB 4.2, you can encrypt a logical-backup as well.  

* In this page:  
  * [Introduction](../../../../client-api/operations/maintenance/backup/encrypted-backup#introduction)  
     * [RavenDB's Security Approach](../../../../client-api/operations/maintenance/backup/encrypted-backup#ravendbs-security-approach)  
     * [Enable Secure Communication](../../../../client-api/operations/maintenance/backup/encrypted-backup#enable-secure-communication)  
  * [Logical-Backup Encryption](../../../../client-api/operations/maintenance/backup/encrypted-backup#logical-backup-encryption)  
  * [Snapshot Encryption](../../../../client-api/operations/maintenance/backup/encrypted-backup#snapshot-encryption)  
     * [Creating an Encrypted Snapshot](../../../../client-api/operations/maintenance/backup/encrypted-backup#creating-an-encrypted-snapshot)  
     * [Restoring an Encrypted Snapshot](../../../../client-api/operations/maintenance/backup/encrypted-backup#restoring-an-encrypted-snapshot)  
{NOTE/}

---

{PANEL: Introduction}

####RavenDB's Security Approach

Encrypting backup files is just **one respect** of RavenDB's comprehensive security approach.  
Other respects are implemented in -

* [Database encryption](../../../../server/security/encryption/database-encryption)  
* Securing server-client communication using [Authentication and certification](../../../../server/security/authentication/certificate-configuration).  

---

####Enable Secure Communication

RavenDB emphasizes the importance of overall security, by allowing encryption of the database only when 
server-client communication is [authenticated and certified](../../../../server/security/overview).  

* **Enabling authentication and certification**  
  Enable secure client-server communication during the server setup, either [manually](../../../../server/security/authentication/certificate-configuration) or [using the setup-wizard](../../../../start/installation/setup-wizard).  

* **Client authentication procedure**  
  When authentication is enabled, clients are required to certify themselves in order to connect the server.  
  Here's a code sample for this procedure:  
{CODE encrypted_database@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

{PANEL/}

{PANEL: Logical-backup Encryption}

{NOTE: Logical-backup encryption is supported by RavenDB version 4.2 and on.  }
{NOTE/}

{PANEL/}

{PANEL: Snapshot Encryption}

####Creating an Encrypted Snapshot

A [snapshot](../../../../client-api/operations/maintenance/backup/backup#snapshot) is an exact copy of the database files. 
If the database is encrypted, so would be its snapshot. If the database is **not** encrypted, the snapshot wouldn't be either.  

* If you want your snapshot to be encrypted, take the snapshot of an encrypted database.  
* Include the [client authentication procedure](../../../../client-api/operations/maintenance/backup/encrypted-backup#enable-secure-communication) in your code.  
* Create a snapshot [as you normally would](../../../../client-api/operations/maintenance/backup/backup#backup-types).  

---

####Restoring an Encrypted Snapshot

Restoring an encrypted snapshot is almost identical to restoring an unencrypted one.  

* Embed the [client authentication procedure](../../../../client-api/operations/maintenance/backup/encrypted-backup#enable-secure-communication) in your code.  
* Pass RestoreBackupOperation an encryption key, using `restoreConfiguration.EncryptionKey`.  
   Use **the same authentication key used to encrypt the database**.
* Code sample:
{CODE restore_encrypted_database@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

{PANEL/}

## Related Articles
**Client Articles**:  
[Backup & Restore Overview](../../../../client-api/operations/maintenance/backup/overview)  
[Backup](../../../../client-api/operations/maintenance/backup/backup)  
[Restore](../../../../client-api/operations/maintenance/backup/restore)  

**Studio Articles**:  
[The Backup Task](../../../../studio/database/tasks/ongoing-tasks/backup-task)  
[Create Database from Backup](../../../../studio/server/databases/create-new-database/from-backup)  

**Security**:  
[Database Encryption](../../../../server/security/encryption/database-encryption)  
[Security Overview](../../../../server/security/overview)  
[Authentication and Certification](../../../../server/security/authentication/certificate-configuration)  
