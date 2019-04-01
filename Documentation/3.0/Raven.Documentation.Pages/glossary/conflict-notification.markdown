# Glossary: ConflictNotification

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **File** | string | The file name |
| **SourceFileSystemUrl** | string | URL of a source file system |
| **Status** | [ConflictStatus](../glossary/conflict-notification#conflictstatus-enum) | The conflict status type enum |
| **RemoteFileHeader** | [FileHeader](./file-header) | The file header of a remote file that was attempted to be synchronized |

<hr />

# ConflictStatus (enum)

### Members

| Name | Value |
| ---- | ----- |
| **Detected** | `0` |
| **Resolved** | `1` |
