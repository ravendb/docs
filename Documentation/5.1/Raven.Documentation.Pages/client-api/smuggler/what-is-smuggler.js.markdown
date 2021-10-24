# What is Smuggler

Smuggler gives you the ability to export or import data from or to a database using JSON format. It is exposed via the `DocumentStore.smuggler`.

{PANEL:ForDatabase}

By default, the `DocumentStore.smuggler` works on the default document store database from the `DocumentStore.database` . 

In order to switch it to a different database use the `.forDatabase` method.

{CODE:nodejs for_database@client-api\smuggler\WhatIsSmuggler.js /}

{PANEL/}

{PANEL:Export}

### Usage

{CODE:nodejs export_syntax@client-api\smuggler\WhatIsSmuggler.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | `DatabaseSmugglerExportOptions` | Options that will be used during the export. Read more [here](../../client-api/smuggler/what-is-smuggler#databasesmugglerexportoptions). |
| **toDatabase** | `DatabaseSmuggler` | `DatabaseSmuggler` instance used as a destination |
| **toFile** | `string` | Path to a file where exported data will be written |

| Return Value | | 
| ------------- | ----- |
| `Operation` | Instance of Operation class which gives you an ability to wait for the operation to complete and subscribe to operation progress events |

### DatabaseSmugglerExportOptions

| Parameters | | |
| ------------- | ------------- | ----- |
| **collections** | ` string[]` | List of specific collections to export. If empty, then all collections will be exported. Default: `empty` |
| **operateOnTypes** | `DatabaseItemType[]` | Indicates what should be exported. Default: `Indexes`, `Documents`, `RevisionDocuments`, `Conflicts`, `DatabaseRecord`, `Identities`, `CompareExchange`, `Subscriptions` |
| **operateOnDatabaseRecordTypes** | `DatabaseRecordItemType[]` | Indicates what should be exported from database record. Default: `Client`, `ConflictSolverConfig`, `Expiration`, `ExternalReplications`, `PeriodicBackups`, `RavenConnectionStrings`, `RavenEtls`, `Revisions`, `SqlConnectionStrings`, `Sorters`, `SqlEtls`, `HubPullReplications`, `SinkPullReplications` |
| **includeExpired** | `boolean` | Should expired documents be included in the export. Default: `true` |
| **includeArtificial** | `boolean` | ? |
| **removeAnalyzers** | `boolean` | Should analyzers be removed from Indexes. Default: `false` |
| **transformScript** | `string` | JavaScript-based script applied to every exported document. Read more [here](../../client-api/smuggler/what-is-smuggler#transformscript). |
| **maxStepsForTransformScript** | `number` | Maximum number of steps that transform script can process before failing. Default: 10000 |
| **skipRevisionCreation** | `boolean` | skip revision craetion |
| **encryptionKey** | `string` | Encryption key used for restore |

### Example

{CODE:nodejs export_example@client-api\smuggler\WhatIsSmuggler.js /}

{PANEL/}

{PANEL:Import}

### Usage

{CODE:nodejs import_syntax@client-api\smuggler\WhatIsSmuggler.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | `DatabaseSmugglerImportOptions` | Options that will be used during the import. Read more [here](../../client-api/smuggler/what-is-smuggler#databasesmugglerimportoptions). |
| **stream** | `Stream` | Stream with data to import |
| **fromFile** | `string` | Path to a file from which data will be imported |

| Return Value | | 
| ------------- | ----- |
| `Operation` | Instance of Operation-class which gives you an ability to wait for the operation to complete and subscribe to operation progress events |

### DatabaseSmugglerImportOptions

| Parameters | | |
| ------------- | ------------- | ----- |
| **operateOnTypes** | `DatabaseItemType[]` | Indicates what should be exported. Default: `Indexes`, `Documents`, `RevisionDocuments`, `Conflicts`, `DatabaseRecord`, `Identities`, `CompareExchange`, `Subscriptions` |
| **operateOnDatabaseRecordTypes** | `DatabaseRecordItemType[]` | Indicates what should be exported from database record. Default: `Client`, `ConflictSolverConfig`, `Expiration`, `ExternalReplications`, `PeriodicBackups`, `RavenConnectionStrings`, `RavenEtls`, `Revisions`, `SqlConnectionStrings`, `Sorters`, `SqlEtls`, `HubPullReplications`, `SinkPullReplications` |
| **includeExpired** | `boolean` | Should expired documents be included in the export. Default: `true` |
| **includeArtificial** | `boolean` | ? |
| **removeAnalyzers** | `boolean` | Should analyzers be removed from Indexes. Default: `false` |
| **transformScript** | `string` | JavaScript-based script applied to every exported document. Read more [here](../../client-api/smuggler/what-is-smuggler#transformscript). |
| **maxStepsForTransformScript** | `number` | Maximum number of steps that transform script can process before failing. Default: 10000 |
| **skipRevisionCreation** | `boolean` | skip revision craetion |
| **encryptionKey** | `string` | Encryption key used for restore |

### Example

{CODE:nodejs import_example@client-api\smuggler\WhatIsSmuggler.js /}

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

- [Backup Task](../../studio/database/tasks/backup-task)
