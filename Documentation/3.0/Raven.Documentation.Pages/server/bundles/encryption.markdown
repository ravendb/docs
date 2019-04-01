# Bundle: Encryption

The encryption bundle introduces data encryption to RavenDB. By default it uses AES-128 encryption algorithm, but this can be easily changed if needed. Encryption is fully transparent for the end-user, it applies to all documents and in default configuration to all indexes as well.

## Configuration

If you want to setup new database with encryption bundle using the Studio, then please refer to [this](../../studio/walkthroughs/how-to-setup-encryption) page.

Four possible configuration options are:   
* **Raven/Encryption/Algorithm** with [AssemblyQualifiedName](https://msdn.microsoft.com/en-us/library/system.type.assemblyqualifiedname.aspx) as a value. Additionally provided type must be a subclass of [SymmetricAlgorithm](https://msdn.microsoft.com/en-us/library/system.security.cryptography.symmetricalgorithm.aspx) from `System.Security.Cryptography` namespace and must not be an abstract class    
* **Raven/Encryption/Key** a key used for encryption purposes with minimum length of 8 characters, base64 encoded    
* **Raven/Encryption/KeyBitsPreference** the preferred encryption key size in bits 
* **Raven/Encryption/EncryptIndexes** Boolean value indicating if the indexes should be encrypted. Default: true   

{WARNING For security reasons all `Raven/Encryption/*` settings should be placed in `SecuredSettings` configuration section. /}

### Global configuration

All configuration settings can be setup server-wide by adding them to server configuration file.

### Database configuration

All settings can be overridden per database during the database creation process.

{CODE encryption_1@Server\Bundles\Encryption.cs /}

Above example demonstrates how to create `EncryptedDB` with active encryption and with non-default encryption algorithm.

{NOTE All encryption settings can only be provided when a database is being created. Changing them later will cause DB malfunction. /}

## Encryption key management

In RavenDB, we have two types of configuration: server-wide configuration, which is usually located at the `App.config` file and a database specific configuration, which is located at the System database. For the `App.config` file, we provide support for encrypting the file using [DPAPI](https://en.wikipedia.org/wiki/Data_Protection_API), using the standard .NET config file encryption system. For the database specific values, we provide our own support for encrypting the values using DPAPI.

So, as the consequences of the above:    
*	Your documents and indexes are encrypted when they are on a disk using strong encryption.    
*	You can use a a server wide or database specific key for the encryption.   
*	Your encryption key is guarded using DPAPI.   
*	The data is safely encrypted on a disk, and the OS guarantees that no one can access the encryption key.   

{NOTE It is your responsibility to backup the encryption key, as there is no way to recover data without it. /}

## Encryption & Backups

{DANGER:Important}

By default, backup of an encrypted database **contains the encryption key (`Raven/Encryption/Key`) as a plain text** in `Database.Document` file found in backup. This is required to make RavenDB able to restore the backup on a different machine. 

To not include any confidential database settings, please issue a backup request manually with filled database document.

### Backup & Restore

1. Issue a [backup request](../../../http/client-api/commands/how-to/start-backup-restore-operations#startbackup) with empty `SecuredSettings` (where encryption configuration is placed) in [DatabaseDocument](../../glossary/database-document) specified:

{CODE-BLOCK:json}
curl -X POST "http://localhost:8080/databases/Northwind/admin/backup?incremental=false" \
 -d "{\"BackupLocation\":\"c:\\temp\\backup\\Northwind\",\"DatabaseDocument\":{\"SecuredSettings\":{},\"Settings\":{\"Raven/ActiveBundles\": \"Encryption\"},\"Disabled\":false,\"Id\":null}}"
{CODE-BLOCK/}

2. Notice that `Database.Document` found in `c:\temp\backup\Northwind\` contains exactly the same information that you send in your request, which means that your database cannot be restored until you specify `Raven/Encryption/Key` in this document.

3. After filling `Raven/Encryption/Key`, `Raven/Encryption/Algorithm`, `Raven/Encryption/KeyBitsPreference` and `Raven/Encryption/EncryptIndexes` in your `Database.Document` you can issue a [restore request](../../../http/client-api/commands/how-to/start-backup-restore-operations#startrestore):

{CODE-BLOCK:json}
curl -X POST "http://localhost:8080/admin/restore" \
 -d "{\"DatabaseName\":\"NewNorthwind\",\"BackupLocation\":\"c:\\temp\\backup\\Northwind\",\"IndexesLocation\":null,\"RestoreStartTimeout\":null,\"DatabaseLocation\":\"~\\Databases\\NewNorthwind\\\",\"Defrag\":false,\"JournalsLocation\":null}"
{CODE-BLOCK/}

{DANGER/}
