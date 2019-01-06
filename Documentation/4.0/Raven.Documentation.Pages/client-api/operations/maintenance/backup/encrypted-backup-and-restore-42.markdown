# Encrypted Backup and Restore (4.1 and down)   [ignore - to be added to the 4.2 branch]

---

{NOTE: }

* With RavenDB 4.0 and 4.1, you can only encrypt a Snapshot - providing that the database has been encrypted.  
* With RavenDB 4.2, you can encrypt a Logical backup as well.  

* In this page:  
  * [Encrypting Backups](../../../../client-api/operations/maintenance/backup/backup#introduction)  
     * [Backup-Encryption's part in RavenDB's security](../../../../client-api/operations/maintenance/backup/encrypted-backup-and-restore#backup-encryptions-part-in-ravendbs-security)  
     * [Snapshot Encryption](../../../../client-api/operations/maintenance/backup/encrypted-backup-and-restore#snapshot-encryption)  
     * [Logical Backup encryption](../../../../client-api/operations/maintenance/backup/encrypted-backup-and-restore#logical-backup-encryption)  
  * [Encrypting Backups](../../../../client-api/operations/maintenance/backup/encrypted-backup-and-restore#encrypting-backups)  
     * [Encrypting a Snapshot](../../../../client-api/operations/maintenance/backup/encrypted-backup-and-restore#encrypting-a-snapshot)  
     * [Restoring an encrypted Snapshot](../../../../client-api/operations/maintenance/backup/encrypted-backup-and-restore#restoring-an-encrypted-snapshot)  
{NOTE/}

---

{PANEL: Introduction}

####Backup-Encryption's part in RavenDB's security  
* Encrypting backup files is just one "front" of RavenDB's comprehensive security approach.  
   * Other security-fronts are -  
      * Encrypting the database.  
      * Authenticating server-client communication.  
* To emphasizes the need in overall security, RavenDB allows backup encryption only when [server-client communication is authenticated](../../../../server/security/authentication/certificate-configuration#authentication--manual-certificate-configuration).  
   * You can enable authentication while installing the server using [the Setup Wizard](../../../../start/installation/setup-wizard) or [manually](../../../../server/security/authentication/certificate-configuration).  

---

####Snapshot encryption
* Snapshot encryption is automatic.  
   * When the database is **encrypted**, the Snapshot is **encrypted** as well.  
   * When the database is **un-encrypted**, the Snapshot is **un-encrypted** as well.  
* To restore an encrypted Snapshot, use the same encryption key you used to encrypt the database.  

---

####Logical-backup encryption  
* A logical backup can be encrypted using RavenDB version 4.2 and on.  
  Earlier versions do not suport logical backup encryption.  

{PANEL/}


{PANEL: Encrypting Snapshots}

####Encrypting a Snapshot

* Default behavior:  
   * As explained [in the introduction](../../../../client-api/operations/maintenance/backup/encrypted-backup-and-restore#introduction), Snapshots are automatically encrypted if the database is encrypted, and vice versa.  
      * Other than taking care of authentication, there's no need to address it in the code.  
      * Authentication-handling code sample:

{CODE encrypted_database@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

* You **can** encrypt the Snapshot using an encryption key of your choice.  
   * Pass your key to `EncryptionSettings` in the backup configuration structure.  
   * Sample:
    
{CODE encryption_key@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  



####Restoring an encrypted Snapshot

* In order to restore an encrypted snapshot, provide the encryption key you've encrypted your database with.  
   * `restoreConfiguration.EncryptionKey = "1F0K2R/KkcwbkK7n4kYlv5eqisy/pMnSuJvZ2sJ/EKo=";`  
   * Code sample:
{CODE restore_encrypted_database@ClientApi\Operations\Maintenance\Backup\Backup.cs /}  

{PANEL/}


## Related Articles
