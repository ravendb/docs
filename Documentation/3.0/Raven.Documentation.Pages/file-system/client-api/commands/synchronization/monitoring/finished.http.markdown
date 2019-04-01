#Commands: GetFinished

The **GET** method allows to page through the [`SynchronizationReports`](../../../../../glossary/synchronization-report) of already accomplished file synchronizations on the destination server.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/synchronization/finished?start={start}&pageSize={pageSize}  \
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
| **JSON** | The `ItemsPage` object, which contains the number of total results and the list of the `SynchronizationReport` objects for the requested page. |

<hr />

## Example

{CODE-BLOCK:json}
curl \
	-X GET http://localhost:8081/fs/SlaveNorthwindFS/synchronization/finished?start=2&pageSize=25  \
< HTTP/1.1 200 OK

{
    "TotalCount":3,
    "Items":
    [
        {
            "FileName":"/pdfs/NoSQL Tech Comparison Report.pdf",
            "FileETag":"00000000-0000-0001-0000-000000000006",
            "BytesTransfered":1408461,
            "BytesCopied":0,
            "NeedListLength":1,
            "Exception":null,
            "Type":"ContentUpdate"
        }
    ]
}

{CODE-BLOCK/}
