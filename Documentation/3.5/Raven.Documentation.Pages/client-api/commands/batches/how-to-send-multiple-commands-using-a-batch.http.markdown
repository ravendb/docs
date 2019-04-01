# Batches: How to send multiple commands using a batch?

To send **multiple operations** in a **single request**, reducing the number of remote calls and allowing several operations to share **same transaction**, `bulk_docs` endpoint should be used.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/bulk_docs \
	-X POST \
	-d @commands.txt
{CODE-BLOCK/}

### Request

| Payload |
| ------- |
| A serialized array of following commands:<br />- [PutCommandData](../../../glossary/put-command-data)<br />- [DeleteCommandData](../../../glossary/delete-command-data)<br />- [PatchCommandData](../../../glossary/patch-command-data)<br />- [ScriptedPatchCommandData](../../../glossary/scripted-patch-command-data)  Commands to process. |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| [BatchResult](../../../glossary/batch-result)[] | An array of batch results matching **exactly** the order of commands send. |

<hr />

## Example

{CODE-BLOCK:json}
curl -X POST "http://localhost:8080/databases/NorthWind/bulk_docs" 
-d 
[
	{"Document":
		{"Name":"My Product","Supplier":"suppliers/999","Id":null},
	"Key":"products/999",
	"Method":"PUT",
	"AdditionalData":null,
	"Metadata":{}
},{
	"Document":
		{"Name":"My Supplier","Id":null},
	"Key":"suppliers/999",
	"Method":"PUT",
	"AdditionalData":null,
	"Metadata":{}
},{
	"Key":"products/2",
	"Method":"DELETE",
	"AdditionalData":null,
	"Etag":null}
]
< HTTP/1.1 200 OK
[
	{
		"Etag":"01000000-0000-0009-0000-000000000001",
		"Method":"PUT",
		"Key":"products/999",
		"Metadata":{"Last-Modified":"2014-11-12T11:38:42.3180730Z"},
		"AdditionalData":null,
		"PatchResult":null,
		"Deleted":null
	},
	{
		"Etag":"01000000-0000-0009-0000-000000000002",
		"Method":"PUT",
		"Key":"suppliers/999",
		"Metadata":{"Last-Modified":"2014-11-12T11:38:42.3180730Z"},
		"AdditionalData":null,
		"PatchResult":null,
		"Deleted":null
	},
	{
		"Etag":null,
		"Method":"DELETE",
		"Key":"products/2",
		"Metadata":null,
		"AdditionalData":null,
		"PatchResult":null,
		"Deleted":true
	}
]
{CODE-BLOCK/}

## Concurrency

If an ETag is specified in the command, that ETag is compared to the current ETag on the document on the server. If the ETag does not match, a 409 Conflict status code will be returned from the server, causing a **ConcurrencyException** to be thrown. In such a case, the entire operation fails and non of the updates that were attempted will succeed.

## Transactions

All the operations in the batch will succeed or fail as a transaction. Other users will not be able to see any of the changes until the entire batch completes.

## Related articles

- [Put](../../../client-api/commands/documents/put)   
- [Delete](../../../client-api/commands/documents/delete)   
- [How to work with **patch requests**?](../../../client-api/commands/patches/how-to-work-with-patch-requests)   
- [How to use **JavaScript** to **patch** your documents?](../../../client-api/commands/patches/how-to-use-javascript-to-patch-your-documents)  
