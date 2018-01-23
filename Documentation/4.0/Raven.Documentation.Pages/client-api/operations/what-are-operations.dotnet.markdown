# Operations : What are the Operations?

The RavenDB client API is built with the notion of layers. At the top, and what you will usually interact with, are the **[DocumentStore](../../client-api/what-is-a-document-store)** and the **[DocumentSession](../../client-api/session/what-is-a-session-and-how-does-it-work)**.

They, in turn, are built on top of the notion of Operations and Commands.

Operations are an encapsulation of a set of low level commands which are used to manipulate data, execute administrative tasks, and change the configuration on a server.  

They are available in the DocumentStore under the **Operations**, **Maintenance**, and **Maintenance.Server** properties.

{PANEL:Common Operations}

Common operations include set based operations for [Patching](../../client-api/operations/patching/set-based-patching) or removal of documents by using queries (more can be read [here](../../client-api/operations/delete-by-query)).  
There is also the ability to handle distributed [Compare-Exchange](../../client-api/operations/compare-exchange) operations and manage [Attachments](../../client-api/operations/attachments/get-attachment).

### How to Send an Operation

In order to excecute an operation, you will need to use the `Send` or `SendAsync` methods. Avaliable overloads are:
{CODE-TABS}
{CODE-TAB:csharp:Sync Client_Operations_api@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TAB:csharp:Async Client_Operations_api_async@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TABS/}

### The following operations are available:

#### Compare Exchange

* [CompareExchange](../../client-api/operations/compare-exchange)   

#### Attachments

* [GetAttachmentOperation](../../client-api/operations/attachments/get-attachment)
* [PutAttachmentOperation](../../client-api/operations/attachments/put-attachment)
* [DeleteAttachmentOperation](../../client-api/operations/attachments/gdelete-attachment)

#### Patching

* [PatchByQueryOperation](../../client-api/operations/patching/set-based-patching)   
* [PatchOperation](../../client-api/operations/patching/single-document-patching)   

#### Misc

* [DeleteByQueryOperation](../../client-api/operations/delete-by-query)   

### Example - Get Attachment

{CODE-TABS}
{CODE-TAB:csharp:Sync Client_Operations_1@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TAB:csharp:Async Client_Operations_1_async@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:Maintenance Operations}

Maintenance operations include operations for changing the configuration at runtime and for management of index operations.

### How to Send an Operation

{CODE-TABS}
{CODE-TAB:csharp:Sync Maintenance_Operations_api@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TAB:csharp:Async Maintenance_Operations_api_async@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TABS/}

### The following maintenance operations are available:

#### Client Configuration

* [PutClientConfigurationOperation](../../client-api/operations/maintenance/configuration/put-client-configuration)   
* [GetClientConfigurationOperation](../../client-api/operations/maintenance/configuration/get-client-configuration)   

#### Connection Strings

* [GetConnectionStringsOperation](../../client-api/operations/maintenance/connection-strings/get-connection-strings)   
* [PutConnectionStringOperation](../../client-api/operations/maintenance/connection-strings/put-connection-strings)   
* [RemoveConnectionStringOperation](../../client-api/operations/maintenance/connection-strings/remove-connection-strings)   

#### ETL

* [AddEtlOperation](../../client-api/operations/maintenance/etl/add-etl)   
* [UpdateEtlOperation](../../client-api/operations/maintenance/etl/update-etl)   
* [ResetEtlOperation](../../client-api/operations/maintenance/etl/reset-etl)   

#### Indexing

* [DeleteIndexOperation](../../client-api/operations/maintenance/indexes/delete-index)   
* [DisableIndexOperation](../../client-api/operations/maintenance/indexes/disable-index)   
* [EnableIndexOperation](../../client-api/operations/maintenance/indexes/enable-index)   
* [ResetIndexOperation](../../client-api/operations/maintenance/indexes/reset-index)   
* [SetIndexesLockOperation](../../client-api/operations/maintenance/indexes/set-indexes-lock)   
* [SetIndexesPriorityOperation](../../client-api/operations/maintenance/indexes/set-indexes-priority)   
* [StartIndexOperation](../../client-api/operations/maintenance/indexes/start-index)   
* [StartIndexingOperation](../../client-api/operations/maintenance/indexes/start-indexing)   
* [StopIndexOperation](../../client-api/operations/maintenance/indexes/stop-index)   
* [StopIndexingOperation](../../client-api/operations/maintenance/indexes/stop-indexing)   
* [GetIndexErrorsOperation](../../client-api/operations/maintenance/indexes/get-index-errors)   
* [GetIndexOperation](../../client-api/operations/maintenance/indexes/get-index)   
* [GetIndexPerformanceStatisticsOperation](../../client-api/operations/maintenance/indexes/get-index-performance-statistics)   
* [GetIndexStatisticsOperation](../../client-api/operations/maintenance/indexes/get-index-statistics)   
* [GetIndexesOperation](../../client-api/operations/maintenance/indexes/get-indexes)   
* [GetIndexesStatisticsOperation](../../client-api/operations/maintenance/indexes/get-indexes-statistics)   
* [GetIndexingStatusOperation](../../client-api/operations/maintenance/indexes/get-indexing-status)   
* [GetTermsOperation](../../client-api/operations/maintenance/indexes/get-terms)   
* [IndexHasChangedOperation](../../client-api/operations/maintenance/indexes/index-has-changed)   
* [PutIndexesOperation](../../client-api/operations/maintenance/indexes/put-indexes)   

#### SQL Migration

* [SqlMigrationImportOperation](../../client-api/operations/maintenance/migration/sql-migration-import)   
* [SqlMigrationSchemaOperation](../../client-api/operations/maintenance/migration/sql-migration-schema)   

#### Backup/Restore

* [StartBackupOperation](../../client-api/operations/maintenance/backups/start-backup)   
* [UpdatePeriodicBackupOperation](../../client-api/operations/maintenance/backups/update-periodic-backup)   

#### Replication

* [UpdateExternalReplicationOperation](../../client-api/operations/maintenance/replication/update-external-replication)   
* [GetReplicationPerformanceStatisticsOperation](../../client-api/operations/maintenance/replication/get-replication-performance-statistics)   

#### Misc

* [GetCollectionStatisticsOperation](../../client-api/operations/maintenance/get-collection-statistics)   
* [GetStatisticsOperation](../../client-api/operations/maintenance/get-statistics)     
* [GetIdentitiesOperation](../../client-api/operations/maintenance/get-identities)   
* [GetOperationStateOperation](../../client-api/operations/maintenance/get-operation-state)   

### Example - Stop Index

{CODE-TABS}
{CODE-TAB:csharp:Sync Maintenance_Operations_1@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TAB:csharp:Async Maintenance_Operations_1_async@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:Server Operations}

These type of operations contain various administrative and miscellaneous configuration operations.

### How to Send an Operation

{CODE-TABS}
{CODE-TAB:csharp:Sync Server_Operations_api@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TAB:csharp:Async Server_Operations_api_async@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TABS/}

### The following server-wide operations are available:

#### Client Certificates

* [CreateClientCertificateOperation](../../client-api/operations/server-wide/create-client-certificate)   
* [GetCertificatesOperation](../../client-api/operations/server-wide/get-certificates)   
* [DeleteCertificateOperation](../../client-api/operations/server-wide/delete-certificate)   
* [PutClientCertificateOperation](../../client-api/operations/server-wide/put-client-certificate)   

#### Server-wide Configuration

* [GetServerWideClientConfigurationOperation](../../client-api/operations/server-wide/configuration/get-serverwide-client-configuration)   
* [PutServerWideClientConfigurationOperation](../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration)   

#### Cluster Management

* [AddDatabaseNodeOperation](../../client-api/operations/server-wide/add-database-node)   
* [ReorderDatabaseMembersOperation](../../client-api/operations/server-wide/reorder-database-members)   
* [CreateDatabaseOperation](../../client-api/operations/server-wide/create-database)   
* [DeleteDatabasesOperation](../../client-api/operations/server-wide/delete-database)   
* [GetDatabaseTopology](../../client-api/operations/server-wide/get-database-topology)   
* [PromoteDatabaseNodeOperation](../../client-api/operations/server-wide/promote-database-node)   
* [ToggleDatabasesStateOperation](../../client-api/operations/server-wide/toggle-databases-state)   

#### Backup/Restore

* [RestoreBackupOperation](../../client-api/operations/server-wide/restore-backup)   

#### Miscellaneous

* [GetBuildNumberOperation](../../client-api/operations/server-wide/get-build-number)   
* [GetDatabaseNamesOperation](../../client-api/operations/server-wide/get-database-names)   

### Example - Get Build Number

{CODE-TABS}
{CODE-TAB:csharp:Sync Server_Operations_1@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TAB:csharp:Async Server_Operations_1_async@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TABS/}

{PANEL/}

## Remarks

{NOTE By default, operations available in `store.Operations` or `store.Maintenance` are working on a default database that was setup for that store. To switch operations to a different database that is available on that server use the **[ForDatabase](../../client-api/operations/how-to/switch-operations-to-a-different-database)** method. /}
