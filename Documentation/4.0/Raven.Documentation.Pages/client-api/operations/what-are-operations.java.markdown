# Operations: What are the Operations

The RavenDB client API is built with the notion of layers. At the top, and what you will usually interact with, are the **[DocumentStore](../../client-api/what-is-a-document-store)** and the **[DocumentSession](../../client-api/session/what-is-a-session-and-how-does-it-work)**.

They in turn, are built on top of the notion of Operations and Commands.

Operations are an encapsulation of a set of low level commands which are used to manipulate data, execute administrative tasks, and change the configuration on a server.  

They are available in the DocumentStore under the **operations**, **maintenance**, and **maintenance().server** methods.

{PANEL:Common Operations}

Common operations include set based operations for [Patching](../../client-api/operations/patching/set-based) or removal of documents by using queries (more can be read [here](../../client-api/operations/delete-by-query)).  
There is also the ability to handle distributed [Compare Exchange](../../client-api/operations/compare-exchange/overview) operations and manage [Attachments](../../client-api/operations/attachments/get-attachment).

### How to Send an Operation

In order to excecute an operation, you will need to use the `send` or `sendAsync` methods. Avaliable overloads are:
{CODE-TABS}
{CODE-TAB:java:Sync Client_Operations_api@ClientApi\Operations\WhatAreOperations.java /}
{CODE-TAB:java:Async Client_Operations_api_async@ClientApi\Operations\WhatAreOperations.java /}
{CODE-TABS/}

### The following operations are available:

#### Compare Exchange

* [CompareExchange](../../client-api/operations/compare-exchange/overview)   

#### Attachments

* [GetAttachmentOperation](../../client-api/operations/attachments/get-attachment)
* [PutAttachmentOperation](../../client-api/operations/attachments/put-attachment)
* [DeleteAttachmentOperation](../../client-api/operations/attachments/delete-attachment)

#### Patching

* [PatchByQueryOperation](../../client-api/operations/patching/set-based)   
* [PatchOperation](../../client-api/operations/patching/single-document)   

#### Misc

* [DeleteByQueryOperation](../../client-api/operations/delete-by-query)   

### Example - Get Attachment

{CODE:java Client_Operations_1@ClientApi\Operations\WhatAreOperations.java /}

{PANEL/}

{PANEL:Maintenance Operations}

Maintenance operations include operations for changing the configuration at runtime and for management of index operations.

### How to Send an Operation

{CODE:java Maintenance_Operations_api@ClientApi\Operations\WhatAreOperations.java /}

### The following maintenance operations are available:

#### Client Configuration

* [PutClientConfigurationOperation](../../client-api/operations/maintenance/configuration/put-client-configuration)   
* [GetClientConfigurationOperation](../../client-api/operations/maintenance/configuration/get-client-configuration)   

#### Indexing

* [DeleteIndexOperation](../../client-api/operations/maintenance/indexes/delete-index)   
* [DisableIndexOperation](../../client-api/operations/maintenance/indexes/disable-index)   
* [EnableIndexOperation](../../client-api/operations/maintenance/indexes/enable-index)   
* [ResetIndexOperation](../../client-api/operations/maintenance/indexes/reset-index)   
* [SetIndexesLockOperation](../../client-api/operations/maintenance/indexes/set-index-lock)   
* [SetIndexesPriorityOperation](../../client-api/operations/maintenance/indexes/set-index-priority)  
* [StartIndexOperation](../../client-api/operations/maintenance/indexes/start-index)   
* [StartIndexingOperation](../../client-api/operations/maintenance/indexes/start-indexing)   
* [StopIndexOperation](../../client-api/operations/maintenance/indexes/stop-index)   
* [StopIndexingOperation](../../client-api/operations/maintenance/indexes/stop-indexing)   
* [GetIndexErrorsOperation](../../client-api/operations/maintenance/indexes/get-index-errors)   
* [GetIndexOperation](../../client-api/operations/maintenance/indexes/get-index)   
* [GetIndexesOperation](../../client-api/operations/maintenance/indexes/get-indexes)   
* [GetTermsOperation](../../client-api/operations/maintenance/indexes/get-terms)   
* [IndexHasChangedOperation](../../client-api/operations/maintenance/indexes/index-has-changed)   
* [PutIndexesOperation](../../client-api/operations/maintenance/indexes/put-indexes)   

#### Misc

* [GetCollectionStatisticsOperation](../../client-api/operations/maintenance/get-collection-statistics)   
* [GetStatisticsOperation](../../client-api/operations/maintenance/get-statistics)     
* [GetIdentitiesOperation](../../client-api/operations/maintenance/identities/get-identities)   

### Example - Stop Index

{CODE:java Maintenance_Operations_1@ClientApi\Operations\WhatAreOperations.java /}

{PANEL/}

{PANEL:Server Operations}

These type of operations contain various administrative and miscellaneous configuration operations.

### How to Send an Operation

{CODE-TABS}
{CODE-TAB:java:Sync Server_Operations_api@ClientApi\Operations\WhatAreOperations.java /}
{CODE-TAB:java:Async Server_Operations_api_async@ClientApi\Operations\WhatAreOperations.java /}
{CODE-TABS/}

### The following server-wide operations are available:


#### Cluster Management

* [CreateDatabaseOperation](../../client-api/operations/server-wide/create-database)   
* [DeleteDatabasesOperation](../../client-api/operations/server-wide/delete-database)   

#### Miscellaneous

* [GetDatabaseNamesOperation](../../client-api/operations/server-wide/get-database-names)   

### Example - Get Build Number

{CODE:java Server_Operations_1@ClientApi\Operations\WhatAreOperations.java /}

{PANEL/}

## Remarks

{NOTE By default, operations available in `store.operations` or `store.maintenance` are working on a default database that was setup for that store. To switch operations to a different database that is available on that server use the **[forDatabase](../../client-api/operations/how-to/switch-operations-to-a-different-database)** method. /}

## Related articles

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)

### Operations

- [How to Switch Operations to a Different Database](../../client-api/operations/how-to/switch-operations-to-a-different-database)
