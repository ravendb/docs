# Commands: Documents: How to delete or update documents using index?

Sometimes we need to update or delete a large amount of documents answering certain criteria. With SQL this is a simple operation, and a query doing that will look like this:

`DELETE FROM Users WHERE LastLogin < '2009-01-01'`   
`UPDATE Users SET IsActive = 0 WHERE LastLogin < '2010-01-01'`   

This is usually not the case for NoSQL databases, where batch operations are not supported. RavenDB does support them, and by passing it a query and an operation definition, it will run the query and perform that operation on its results.

The same queries and indexes that are used for data retrieval are used for the batch operations, therefore the syntax defining which documents to work on is exactly the same as you'd specified for those documents to be pulled from store.

{PANEL:**DeleteByIndex**}

To issue a batch-delete command you need to specify an index and a query to be sent to it. To minimize the chances of stale results coming back, bulk operations should only be performed on static indexes.

### Syntax

{CODE-BLOCK:json}
curl \ 
	http://{serverName}/databases/{databaseName}/bulk_docs/{indexName}? \
		query={query}& \
		allowStale={allowStale} \
		[Other indexQuery parameters] \
	-X DELETE 
{CODE-BLOCK/}

{SAFE:IndexQuery parameters}
This endpoint accepts [IndexQuery](../../../../glossary/index-query) object. All possible [IndexQuery](../../../../glossary/index-query) parameters are listed [here](../../../../client-api/commands/querying/how-to-query-a-database#indexquery-parameters)
{SAFE/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **indexName** | Yes | name of an index to perform a query on |
| **query** | Yes | query that will be performed |
| **allowStale** | No | can operation be performed on a stale index (default: `false`) |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **OperationId** | Operation id |

<hr />

### Remarks

{NOTE `DeleteByIndex` can only be performed on map index. Executing it on map-reduce index will lead to an exception. /}

<hr />

### Example

Remove all documents from 'Shops' collection

{CODE-BLOCK:json}
curl -X DELETE "http://localhost:8080/databases/NorthWind/bulk_docs/Raven/DocumentsByEntityName?&query=Tag%3AShops&allowStale=false" 
< HTTP/1.1 200 OK
{"OperationId":1}
{CODE-BLOCK/}

{PANEL/}
{PANEL:**UpdateByIndex**}

Performing a mass-update is exactly the same as making a mass-delete, but this time it uses the Patching API to make it easy for us to define what to do with the documents matching our query.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/bulk_docs/{indexName}? \
		query={query}& \
		allowStale={allowStale} \
		[Other indexQuery parameters] \
	-X PATCH \
	-d @patchs.txt
{CODE-BLOCK/}

{SAFE:IndexQuery parameters}
This endpoint accepts [IndexQuery](../../../../glossary/index-query) object. All possible [IndexQuery](../../../../glossary/index-query) parameters are listed [here](../../../../client-api/commands/querying/how-to-query-a-database#indexquery-parameters)
{SAFE/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **indexName** | Yes | name of an index to perform a query on |
| **query** | Yes | query that will be performed |
| **allowStale** | No | can operation be performed on a stale index (default: `false`) |

| Payload |
| ------- |
| Array of patch requests |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **OperationId** | Operation id |

<hr />

### Example

Set property 'FirstName' for all documents in collection 'Employees' to 'Patched Name'

{CODE-BLOCK:json}
curl -X PATCH "http://localhost:8080/databases/NorthWind/bulk_docs/Raven/DocumentsByEntityName?&query=Tag%3AEmployees&pageSize=128&allowStale=false" \
 -d "[{\"Name\":\"FirstName\",\"Value\":\"Patched Name\",\"Type\":\"Set\"}]"

< HTTP/1.1 200 OK
{"OperationId":4}
{CODE-BLOCK/}

{PANEL/}
{PANEL:**UpdateByIndex** - using JavaScript}

Mass-update can also be executed with JavaScript patch.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/bulk_docs/{indexName}? \
		query={query}& \
		allowStale={allowStale} \
		[Other indexQuery parameters] \
	-X EVAL \
	-d @jsScript.txt
{CODE-BLOCK/}

{SAFE:IndexQuery parameters}
This endpoint accepts [IndexQuery](../../../../glossary/index-query) object. All possible [IndexQuery](../../../../glossary/index-query) parameters are listed [here](../../../../client-api/commands/querying/how-to-query-a-database#indexquery-parameters)
{SAFE/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **indexName** | Yes | name of an index to perform a query on |
| **query** | Yes | query that will be performed |
| **allowStale** | No | can operation be performed on a stale index (default: `false`) |

| Payload |
| ------- |
| Java script used for patching |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **OperationId** | Operation id |

<hr />

### Example

Set property 'FirstName' for all documents in collection 'Employees' to 'Patched Name'. 

{CODE-BLOCK:json}
curl -X EVAL "http://localhost:8080/databases/NorthWind/bulk_docs/Raven/DocumentsByEntityName?&query=Tag%3AEmployees&pageSize=128&allowStale=false" \
 -d "{\"Values\":{},\"Script\":\"this.FirstName = 'Patched Name';\"}"

< HTTP/1.1 200 OK
{"OperationId":4}
{CODE-BLOCK/}

{PANEL/}

## Remarks

{SAFE By default, Set-based operations will **not work** on indexes that are stale, and the operation will **only succeed** if the specified **index is not stale**. This is to make sure you only delete what you intended to delete. /}

For indexes that are updated all the time, you can set a `cutoff` which will make sure the operation is executed and that you know what results to expect.

When you are absolutely certain that you can perform the operation when the index is stale, simply set the `allowStale` parameter to true.

## Related articles

- [Put](../../../../client-api/commands/documents/put)  
- [Delete](../../../../client-api/commands/documents/delete)  
