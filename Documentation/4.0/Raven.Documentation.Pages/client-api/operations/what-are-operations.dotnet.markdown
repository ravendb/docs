# Operations : What are the operations?

Operations are an encapsulation of a set of low level commands which are used to manipulate data, execute administrative tasks and change configuration on a server.
They are available in **[DocumentStore](../../client-api/what-is-a-document-store)** under **Operations**, **Maintenance** and **Maintenance.Server** properties.

### Common Operations
Common operations include set based operations (Patch, Delete-by-query) and distributed compare-exchange operation.

The syntax for client operations is as follows:
{CODE Client_Operations@ClientApi\Operations\WhatAreOperations.cs /}

#### The following common operations are available:
* [CompareExchange](../../client-api/operations/compare-exchange)   
* [DeleteByQueryOperation](../../client-api/operations/delete-by-query-operation)   
* [GetAttachmentOperation](../../client-api/operations/get-attachment-operation)   
* [PatchByQueryOperation](../../client-api/operations/patch/patch-by-query-operation)   
* [PatchOperation](../../client-api/operations/patch/patch-operation.markdown)   
* [PutAttachmentOperation](../../client-api/operations/put-attachment-operation)   

### Maintenance Operations
Maintenance operations include operations for changing configuration at runtime and for management of index operations.

The syntax for maintenance operations is as follows:
{CODE Maintenance_Operations@ClientApi\Operations\WhatAreOperations.cs /}

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

### Server Operations
This type of operations contain various administrative and miscellaneous configuration operations.

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
