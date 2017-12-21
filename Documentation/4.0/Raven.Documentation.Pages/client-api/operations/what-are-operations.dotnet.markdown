# Operations : What are the operations?

The RavenDB client API is built with the notion of layers. At the top,
and what you will usually interact with, are the **[DocumentStore](../../client-api/what-is-a-document-store)** and the **[DocumentSession](../../client-api/session/what-is-a-session-and-how-does-it-work)**.
They, in turn, are built on top of the notion of Operations and Commands.

Operations are an encapsulation of a set of low level commands which are used to manipulate data, execute administrative tasks and change configuration on a server.  
They are available in DocumentStore under **Operations**, **Maintenance** and **Maintenance.Server** properties.

### Common Operations
Common operations include set based operations for [Patching](../../client-api/operations/patch/set-based-patch-operation) or removal of documents by using queries (more can be read [here](../../client-api/operations/delete-by-query-operation)).  
There is also the ability to handle distributed [Compare-Exchange](../../client-api/operations/compare-exchange) operations and manage [Attachments](../../client-api/operations/get-attachment-operation).

{PANEL:Operations.Send}
In order to excecute an Operation, you will need to use the `Send` or `SendAsync` methods. Avaliable overloads are:
{CODE-TABS}
{CODE-TAB:csharp:Sync Client_Operations_api@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TAB:csharp:Async Client_Operations_api_async@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TABS/}
{PANEL/}

#### The following common operations are available:
* [CompareExchange](../../client-api/operations/compare-exchange)   
* [DeleteByQueryOperation](../../client-api/operations/delete-by-query-operation)   
* [GetAttachmentOperation](../../client-api/operations/get-attachment-operation)   
* [PatchByQueryOperation](../../client-api/operations/patch/patch-by-query-operation)   
* [PatchOperation](../../client-api/operations/patch/patch-operation.markdown)   
* [PutAttachmentOperation](../../client-api/operations/put-attachment-operation)


####Example : Get Attachment
{CODE-TABS}
{CODE-TAB:csharp:Sync Client_Operations_1@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TAB:csharp:Async Client_Operations_1_async@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TABS/}

### Maintenance Operations
Maintenance operations include operations for changing configuration at runtime and for management of index operations.

{PANEL:Maintenance.Send}
{CODE-TABS}
{CODE-TAB:csharp:Sync Maintenance_Operations_api@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TAB:csharp:Async Maintenance_Operations_api_async@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TABS/}
{PANEL/}

#### The following maintenance operations are available:

##### Client Configuration
* [PutClientConfigurationOperation](../../client-api/operations/maintenance/put-client-configuration-operation)   
* [GetClientConfigurationOperation](../../client-api/operations/maintenance/get-client-configuration-operation)   

##### Indexing
* [DeleteIndexOperation](../../client-api/operations/maintenance/delete-index-operation)   
* [DisableIndexOperation](../../client-api/operations/maintenance/disable-index-operation)   
* [EnableIndexOperation](../../client-api/operations/maintenance/enable-index-operation)   
* [ResetIndexOperation](../../client-api/operations/maintenance/reset-index-operation)   
* [SetIndexesLockOperation](../../client-api/operations/maintenance/set-indexes-lock-operation)   
* [SetIndexesPriorityOperation](../../client-api/operations/maintenance/set-indexes-priority-operation)   
* [StartIndexOperation](../../client-api/operations/maintenance/start-index-operation)   
* [StartIndexingOperation](../../client-api/operations/maintenance/start-indexing-operation)   
* [StopIndexOperation](../../client-api/operations/maintenance/stop-index-operation)   
* [StopIndexingOperation](../../client-api/operations/maintenance/stop-indexing-operation)   
* [GetIndexErrorsOperation](../../client-api/operations/maintenance/get-index-errors-operation)   
* [GetIndexOperation](../../client-api/operations/maintenance/get-index-operation)   
* [GetIndexPerformanceStatisticsOperation](../../client-api/operations/maintenance/get-index-performance-statistics-operation)   
* [GetIndexStatisticsOperation](../../client-api/operations/maintenance/get-index-statistics-operation)   
* [GetIndexesOperation](../../client-api/operations/maintenance/get-indexes-operation)   
* [GetIndexesStatisticsOperation](../../client-api/operations/maintenance/get-indexes-statistics-operation)   
* [GetIndexingStatusOperation](../../client-api/operations/maintenance/get-indexing-status-operation)   
* [GetTermsOperation](../../client-api/operations/maintenance/get-terms-operation)   
* [IndexHasChangedOperation](../../client-api/operations/maintenance/index-has-changed-operation)   
* [PutIndexesOperation](../../client-api/operations/maintenance/put-indexes-operation)   

##### SQL Migration
* [SqlMigrationImportOperation](../../client-api/operations/maintenance/sql-migration-import-operation)   
* [SqlMigrationSchemaOperation](../../client-api/operations/maintenance/sql-migration-schema-operation)   

##### Misc
* [GetOperationStateOperation](../../client-api/operations/maintenance/get-operation-state-operation)   
* [GetCollectionStatisticsOperation](../../client-api/operations/maintenance/get-collection-statistics-operation)   
* [GetIdentitiesOperation](../../client-api/operations/maintenance/get-identities-operation)   
* [GetReplicationPerformanceStatisticsOperation](../../client-api/operations/maintenance/get-replication-performance-statistics-operation)   
* [GetStatisticsOperation](../../client-api/operations/maintenance/get-statistics-operation)      


####Example : Stop Index
{CODE-TABS}
{CODE-TAB:csharp:Sync Maintenance_Operations_1@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TAB:csharp:Async Maintenance_Operations_1_async@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TABS/}

{NOTE By default, operations available directly in store are working on a default database that was setup for that store. To switch operations to a different database that is available on that server use **[ForDatabase](../../client-api/operations/how-to/switch-operations-to-a-different-database)** method. /}

### Server Operations
This type of operations contain various administrative and miscellaneous configuration operations.

{PANEL:Maintenance.Server.Send}
{CODE-TABS}
{CODE-TAB:csharp:Sync Server_Operations_api@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TAB:csharp:Async Server_Operations_api_async@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TABS/}
{PANEL/}

#### The following server operations are available:

##### Client Certificates
* [CreateClientCertificateOperation](../../client-api/operations/server/create-client-certificate-operation)   
* [GetCertificatesOperation](../../client-api/operations/server/get-certificates-operation)   
* [DeleteCertificateOperation](../../client-api/operations/server/delete-certificate-operation)   
* [PutClientCertificateOperation](../../client-api/operations/server/put-client-certificate-operation)   

##### Server-wide Configuration
* [GetServerWideClientConfigurationOperation](../../client-api/operations/server/get-serverwide-client-configuration-operation)   
* [PutServerWideClientConfigurationOperation](../../client-api/operations/server/put-serverwide-client-configuration-operation)   

##### Connection Strings
* [GetConnectionStringsOperation](../../client-api/operations/server/get-connection-strings-operation)   
* [PutConnectionStringOperation](../../client-api/operations/server/put-connection-strings-operation)   
* [RemoveConnectionStringOperation](../../client-api/operations/server/remove-connection-strings-operation)   

##### ETL
* [AddEtlOperation](../../client-api/operations/server/add-etl-operation)   
* [UpdateEtlOperation](../../client-api/operations/server/update-etl-operation)   
* [ResetEtlOperation](../../client-api/operations/server/reset-etl-operation)   

##### Cluster Management
* [AddDatabaseNodeOperation](../../client-api/operations/server/add-database-node-operation)   
* [ReorderDatabaseMembersOperation](../../client-api/operations/server/reorder-database-members-operation)   
* [CreateDatabaseOperation](../../client-api/operations/server/create-database-operation)   
* [DeleteDatabasesOperation](../../client-api/operations/server/delete-database-operation)   
* [GetDatabaseTopology](../../client-api/operations/server/get-database-topology-operation)   
* [PromoteDatabaseNodeOperation](../../client-api/operations/server/promote-database-node-operation)   
* [ToggleDatabasesStateOperation](../../client-api/operations/server/toggle-databases-state-operation)   

##### Backup/Restore
* [RestoreBackupOperation](../../client-api/operations/server/restore-backup-operation)   
* [StartBackupOperation](../../client-api/operations/server/start-backup-operation)   
* [UpdatePeriodicBackupOperation](../../client-api/operations/server/update-periodic-backup-operation)   

##### Replication
* [UpdateExternalReplicationOperation](../../client-api/operations/server/update-external-replication-operation)   

##### Miscellaneous
* [GetDatabaseNamesOperation](../../client-api/operations/server/get-database-names-operation)   

####Example : Get Build Number
{CODE-TABS}
{CODE-TAB:csharp:Sync Server_Operations_1@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TAB:csharp:Async Server_Operations_1_async@ClientApi\Operations\WhatAreOperations.cs /}
{CODE-TABS/}
