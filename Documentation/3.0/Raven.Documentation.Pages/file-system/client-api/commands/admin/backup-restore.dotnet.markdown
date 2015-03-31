#Backup and restore

To start or restore backup use `StartBackup` or `StartRestore` operations respectively.


{NOTE: Non blocking operations}
Execution of the `StartBackup` and `StartRestore` methods simply starts the requested actions on the server and returns immediately. 
The `StartBackup` and `StartRestore` methods do not wait for operation to complete.
{NOTE/}

{PANEL:StartBackup}

{CODE start_backup_1@FileSystem\ClientApi\Commands\Admin.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **backupLocation** | string | The path to a directory where the backup will be stored |
| **fileSystemDocument** | [FileSystemDocument](../../../../glossary/file-system-document) | The file system configuration document that will be stored with the backup location as 'Filesystem.Document' file. Pass `null` to use the current one.<br />**WARNING**: The file system configuration document may contain sensitive data which will be decrypted and stored in the backup. |
| **incremental** | bool | Indicates if it should be the incremental backup |
| **fileSystemName** | string | The name of the file system to backup. |

<hr/>

| Return Value | |
| ------------- | ------------- |
| **Task** | A task that represents the asynchronous start operation |

### Example

{CODE start_backup_2@FileSystem\ClientApi\Commands\Admin.cs /}

If you are interested in checking the current backup status you can retrieve it by getting `Raven/Backup/Status` configuration item:

{CODE start_backup_3@FileSystem\ClientApi\Commands\Admin.cs /}

{PANEL/}


{PANEL:StartRestore}

{CODE start_restore_1@FileSystem\ClientApi\Commands\Admin.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **restoreRequest** | [FilesystemRestoreRequest](../../../../glossary/file-system-restore-request) | Restore information |

<hr/>

| Return Value | |
| ------------- | ------------- |
| **Task&lt;long&gt;** | A task that represents the asynchronous restore operation. The task result is the operation identifier. |

## Example

{CODE start_restore_2@FileSystem\ClientApi\Commands\Admin.cs /}

If you needed to wait until the operation finishes, you would have to initialize `DocumentStore` associated with `<system>` database and wait for the operation completion:

{CODE start_restore_3@FileSystem\ClientApi\Commands\Admin.cs /}

{PANEL/}
