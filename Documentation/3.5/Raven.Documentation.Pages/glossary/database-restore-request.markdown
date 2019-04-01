# Glossary: DatabaseRestoreRequest

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **DatabaseName** | string | Database name |
| **DatabaseLocation** | string | Database location |
| **DisableReplicationDestinations** | bool | Indicates if `Raven/Replication/Destinations` should be disabled after database is restored (will load database) |
| **GenerateNewDatabaseId** | bool | Indicates if Database Id should be generated during restore (will load database) |
| **BackupLocation** | string | Backup location |
| **JournalsLocation** | string | Journals location |
| **IndexesLocation** | string | Indexes location |
| **Defrag** | bool | Whether database should be defragmented after restore operation |
| **RestoreStartTimeout** | int? | Restore start timeout |
