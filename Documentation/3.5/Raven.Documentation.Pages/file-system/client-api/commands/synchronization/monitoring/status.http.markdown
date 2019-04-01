#Commands: GetSynchronizationStatus

**GET** method allow to retrieve a report that contains the information about the synchronization of a specified file.

{INFO: The actual server to ask}
This method is intended to ask the destination file system about the performed synchronization report. It means that the actual request should be sent there (not to the source server which pushed the data there).
{INFO/}

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/synchronization/status?fileName={fileName} \
	-X GET 
{CODE-BLOCK/}

### Request
The file's content|

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **fileName** | Yes | The full file name |


### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| JSON | JSON formatted [`SynchronizationReport`](../../../../../glossary/synchronization-report). |

<hr />

## Example

{CODE-BLOCK:json}
curl \
	-X GET http://localhost:8081/fs/SlaveNorthwindFS/synchronization/status?fileName=books/Inside.RavenDB.3.0.pdf
< HTTP/1.1 200 OK

{
    "FileName":"/books/Inside.RavenDB.3.0.pdf",
    "FileETag":"00000000-0000-0001-0000-000000000002",
    "BytesTransfered":1202187,
    "BytesCopied":0,
    "NeedListLength":1,
    "Exception":null,
    "Type":"ContentUpdate"
}
{CODE-BLOCK/}
