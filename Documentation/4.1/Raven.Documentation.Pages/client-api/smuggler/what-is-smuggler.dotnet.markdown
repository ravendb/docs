# What is Smuggler

Smuggler gives you the ability to export or import data from or to a database using JSON format. It is exposed via the `DocumentStore.Smuggler` property.

{PANEL:ForDatabase}

By default, the `DocumentStore.Smuggler` works on the default document store database from the `DocumentStore.Database` property. 

In order to switch it to a different database use the `.ForDatabase` method.

{CODE for_database@ClientApi\Smuggler\WhatIsSmuggler.cs /}

{PANEL/}

{PANEL:Export}

### Syntax

{CODE export_syntax@ClientApi\Smuggler\WhatIsSmuggler.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | `DatabaseSmugglerExportOptions` | Options that will be used during the export. Read more [here](../../client-api/smuggler/what-is-smuggler#databasesmugglerexportoptions). |
| **toDatabase** | `DatabaseSmuggler` | `DatabaseSmuggler` instance used as a destination |
| **toFile** | `string` | Path to a file where exported data will be written |
| **token** | `CancellationToken` | Token used to cancel the operation |

| Return Value | | 
| ------------- | ----- |
| `Operation` | Instance of Operation class which gives you an ability to wait for the operation to complete and subscribe to operation progress events |

### DatabaseSmugglerExportOptions

| Parameters | | |
| ------------- | ------------- | ----- |
| **Collections** | `List<string>` | List of specific collections to export. If empty, then all collections will be exported. Default: `empty` |
| **OperateOnTypes** | `DatabaseItemType` | Indicates what should be exported. Default: `Indexes`, `Documents`, `RevisionDocuments`, `Conflicts`, `DatabaseRecord`, `Identities`, `CompareExchange` |
| **IncludeExpired** | `bool` | Should expired documents be included in the export. Default: `true` |
| **RemoveAnalyzers** | `bool` | Should analyzers be removed from Indexes. Default: `false` |
| **TransformScript** | `string` | JavaScript-based script applied to every exported document. Read more [here](../../client-api/smuggler/what-is-smuggler#transformscript). |
| **MaxStepsForTransformScript** | `int` | Maximum number of steps that transform script can process before failing. Default: 10000 |

### Example

{CODE export_example@ClientApi\Smuggler\WhatIsSmuggler.cs /}

{PANEL/}

{PANEL:Import}

### Syntax

{CODE import_syntax@ClientApi\Smuggler\WhatIsSmuggler.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | `DatabaseSmugglerImportOptions` | Options that will be used during the import. Read more [here](../../client-api/smuggler/what-is-smuggler#databasesmugglerimportoptions). |
| **stream** | `Stream` | Stream with data to import |
| **fromFile** | `string` | Path to a file from which data will be imported |
| **token** | `CancellationToken` | Token used to cancel the operation |

| Return Value | | 
| ------------- | ----- |
| `Operation` | Instance of Operation-class which gives you an ability to wait for the operation to complete and subscribe to operation progress events |

### DatabaseSmugglerImportOptions

| Parameters | | |
| ------------- | ------------- | ----- |
| **Collections** | `List<string>` | List specific of collections to import. If empty then all collections will be imported. Default: `empty` |
| **OperateOnTypes** | `DatabaseItemType` | Indicates what should be imported. Default: `Indexes`, `Documents`, `RevisionDocuments`, `Conflicts`, `DatabaseRecord`, `Identities`, `CompareExchange` |
| **IncludeExpired** | `bool` | Should expired documents be imported. Default: `true` |
| **RemoveAnalyzers** | `bool` | Should analyzers be removed from Indexes. Default: `false` |
| **TransformScript** | `string` | JavaScript-based script applied to every imported document. Read more [here](../../client-api/smuggler/what-is-smuggler#transformscript). |
| **MaxStepsForTransformScript** | `int` | Maximum number of steps that transform script can process before failing. Default: 10000 |
| **SkipRevisionCreation** | `bool`| Smuggler is configured to avoid creating new revisions during import. |

### Example

{CODE import_example@ClientApi\Smuggler\WhatIsSmuggler.cs /}

{PANEL/}

{PANEL:TransformScript}

`TransformScript` exposes the ability to modify or even filter-out the document during the import and export process using the provided JavaScript. 

Underneath the JavaScript engine is exactly the same as used for [patching operations](../../client-api/operations/patching/single-document) giving you identical syntax and capabilities with additional **ability to filter out documents by throwing a 'skip' exception**.

{CODE-BLOCK:javascript}
var id = this['@metadata']['@id'];
if (id === 'orders/999-A')
    throw 'skip'; // filter-out

this.Freight = 15.3;
{CODE-BLOCK/}

{PANEL/}

## Related Articles

**Studio Articles**:   
[Create a Database : From Backup](../../studio/server/databases/create-new-database/from-backup)   
[Create a Database : General Flow](../../studio/server/databases/create-new-database/general-flow)        
[Create a Database : Encrypted](../../studio/server/databases/create-new-database/encrypted)      
[The Backup Task](../../studio/database/tasks/ongoing-tasks/backup-task)   

**Client Articles**:  
[Restore](../../client-api/operations/maintenance/backup/restore)   
[Operations: How to Restore a Database from Backup](../../client-api/operations/server-wide/restore-backup)    
[Backup](../../client-api/operations/maintenance/backup/backup)

**Server Articles**:  
[Backup Overview](../../server/ongoing-tasks/backup-overview)

**Migration Articles**:  
[Migration](../../migration/server/data-migration) 
