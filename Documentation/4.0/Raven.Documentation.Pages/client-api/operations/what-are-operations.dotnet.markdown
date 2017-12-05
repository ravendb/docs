# Operations : What are the operations?

Operations are an encapsulation of a set of low level commands which are used to manipulate data, execute administrative tasks and change configuration on a server.
They are available in **[DocumentStore](../../client-api/what-is-a-document-store)** under **Operations**, **Maintenance** and **Maintenance.Server** properties.

### Client Operations
Client operations include set based operations (Patch, Delete-by-query) and distributed compare-exchange operation.

The syntax for client operations is as follows:
{CODE Client_Operations@ClientApi\Operations\WhatAreOperations.cs /}

#### The following client operations are available:
* [CompareExchange](../../client-api/operations/client/compare-exchange)   
* [DeleteByQueryOperation](../../client-api/operations/client/delete-by-query-operation)   
* [GetAttachmentOperation](../../client-api/operations/client/get-attachment-operation)   
* [PatchByQueryOperation](../../client-api/operations/client/patch-by-query-operation)   
* [PatchOperation](../../client-api/operations/client/patch-operation.markdown)   
* [PutAttachmentOperation](../../client-api/operations/client/patch-attachment-operation)   

### Maintenance Operations
Maintenance operations include operations for changing configuration at runtime and for management of index operations.

The syntax for maintenance operations is as follows:
{CODE Maintenance_Operations@ClientApi\Operations\WhatAreOperations.cs /}

#### The following maintenance operations are available:

##### Configuration
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

##### Migration

##### Misc
* [GetOperationStateOperation](../../client-api/operations/maintenance/get-operation-state-operation)   

## Related articles

- [How to **switch** commands to a different **database**?](../../client-api/commands/how-to/switch-commands-to-a-different-database)   
- [How to **switch** commands **credentials**?](../../client-api/commands/how-to/switch-commands-to-a-different-database)   
- [How to get **server build number**?](../../client-api/commands/how-to/get-server-build-number)   
- [How to get **names of all databases** on a server?](../../client-api/commands/how-to/get-names-of-all-databases-on-a-server)   
