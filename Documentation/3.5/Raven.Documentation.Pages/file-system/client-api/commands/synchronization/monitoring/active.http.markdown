#Commands: GetActive

The **GET** method returns the information about the active outgoing synchronizations.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/synchronization/active?start={start}&pageSize={pageSize}  \
	-X GET
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **start** | No | The number of results that should be skipped |
| **pageSize** | No | The maximum number of results that will be returned |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **JSON** | The `ItemsPage` object that contains the number of total results and the list of the [`SynchronizationDetails`](../../../../../glossary/synchronization-details) objects, which contains info about synchronizations being in progress. |

<hr />

## Example

{CODE-BLOCK:json}
curl \
	-X GET http://localhost:8081/fs/SlaveNorthwindFS/synchronization/active  \
< HTTP/1.1 200 OK

{
    "TotalCount":0,
    "Items":[]
}

{CODE-BLOCK/}

