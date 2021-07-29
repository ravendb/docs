# What is Smuggler

Smuggler gives you the ability to export or import data from or to a database using JSON format. It is exposed via the `DocumentStore.Smuggler` property.

{PANEL:ForDatabase}

By default, the `IDocumentStore.smuggler` works on the default document store database from the `IDocumentStore.database` property. 

In order to switch it to a different database use the `.forDatabase` method.

{CODE:java for_database@ClientApi\Smuggler\WhatIsSmuggler.java /}

{PANEL/}

{PANEL:Export}

### Syntax

{CODE:java export_syntax@ClientApi\Smuggler\WhatIsSmuggler.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | `DatabaseSmugglerExportOptions` | Options that will be used during the export. Read more [here](../../client-api/smuggler/what-is-smuggler#databasesmugglerexportoptions). |
| **toDatabase** | `DatabaseSmuggler` | `DatabaseSmuggler` instance used as a destination |
| **toFile** | `String` | Path to a file where exported data will be written |

| Return Value | | 
| ------------- | ----- |
| `Operation` | Instance of Operation class which gives you an ability to wait for the operation to complete and subscribe to operation progress events |

### DatabaseSmugglerExportOptions

| Parameters | | |
| ------------- | ------------- | ----- |
| **Collections** | `List<String>` | List of specific collections to export. If empty, then all collections will be exported. Default: `empty` |
| **OperateOnTypes** | `DatabaseItemType` | Indicates what should be exported. Default: `Indexes`, `Documents`, `RevisionDocuments`, `Conflicts`, `DatabaseRecord`, `Identities`, `CompareExchange`, `Subscriptions` |
| **OperateOnDatabaseRecordTypes** | `DatabaseRecordItemType` | Indicates what should be exported from database record. Default: `Client`, `ConflictSolverConfig`, `Expiration`, `ExternalReplications`, `PeriodicBackups`, `RavenConnectionStrings`, `RavenEtls`, `Revisions`, `SqlConnectionStrings`, `Sorters`, `SqlEtls`, `HubPullReplications`, `SinkPullReplications` |
| **IncludeExpired** | `boolean` | Should expired documents be included in the export. Default: `true` |
| **RemoveAnalyzers** | `boolean` | Should analyzers be removed from Indexes. Default: `false` |
| **TransformScript** | `String` | JavaScript-based script applied to every exported document. Read more [here](../../client-api/smuggler/what-is-smuggler#transformscript). |
| **MaxStepsForTransformScript** | `int` | Maximum number of steps that transform script can process before failing. Default: 10000 |

### Example

{CODE:java export_example@ClientApi\Smuggler\WhatIsSmuggler.java /}

{PANEL/}

{PANEL:Import}

### Syntax

{CODE:java import_syntax@ClientApi\Smuggler\WhatIsSmuggler.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | `DatabaseSmugglerImportOptions` | Options that will be used during the import. Read more [here](../../client-api/smuggler/what-is-smuggler#databasesmugglerimportoptions). |
| **stream** | `Stream` | Stream with data to import |
| **fromFile** | `String` | Path to a file from which data will be imported |

| Return Value | | 
| ------------- | ----- |
| `Operation` | Instance of Operation-class which gives you an ability to wait for the operation to complete and subscribe to operation progress events |

### DatabaseSmugglerImportOptions

| Parameters | | |
| ------------- | ------------- | ----- |
| **Collections** | `List<String>` | List specific of collections to import. If empty then all collections will be imported. Default: `empty` |
| **OperateOnTypes** | `DatabaseItemType` | Indicates what should be imported. Default: `Indexes`, `Documents`, `RevisionDocuments`, `Conflicts`, `DatabaseRecord`, `Identities`, `CompareExchange`, `Subscriptions` |
| **OperateOnDatabaseRecordTypes** | `DatabaseRecordItemType` | Indicates what should be exported from database record. Default: `Client`, `ConflictSolverConfig`, `Expiration`, `ExternalReplications`, `PeriodicBackups`, `RavenConnectionStrings`, `RavenEtls`, `Revisions`, `SqlConnectionStrings`, `Sorters`, `SqlEtls`, `HubPullReplications`, `SinkPullReplications` |
| **IncludeExpired** | `boolean` | Should expired documents be imported. Default: `true` |
| **RemoveAnalyzers** | `boolean` | Should analyzers be removed from Indexes. Default: `false` |
| **TransformScript** | `String` | JavaScript-based script applied to every imported document. Read more [here](../../client-api/smuggler/what-is-smuggler#transformscript). |
| **MaxStepsForTransformScript** | `int` | Maximum number of steps that transform script can process before failing. Default: 10000 |

### Example

{CODE:java import_example@ClientApi\Smuggler\WhatIsSmuggler.java /}

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

## Related articles

### Studio

- [Backup Task](../../studio/database/tasks/ongoing-tasks/backup-task)
