#Commands: Start

The **POST** methods are used to manually force the synchronization to the destinations. 
There are two endpoints that allow you either to synchronize all the files which require that or to synchronize just one specified file.

{PANEL:Synchronization of all destinations}

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/synchronization/ToDestinations?forceSyncingAll={forceSyncingAll}  \
	-X POST 
    -d ""
{CODE-BLOCK/}

### Request

| Payload |
| ------- |
| Empty|

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **forceSyncingAll** | Yes | Determines whether finished synchronization should schedule a next pending one (there can be only [a limited number of concurrent synchronizations](../../../synchronization/configurations#ravensynchronizationconfig) to a destination file system). The reports of such synchronizations will *not* be included in the returned `DestinationSyncResult` object.  |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| JSON array | The result is an array of `DestinationSyncResult` objects that contains reports about performed operations. |
<hr />

## Example

{CODE-BLOCK:json}
curl \
	-X POST http://localhost:8080/fs/NorthwindFS/synchronization/ToDestinations?forceSyncingAll=false \
    -d ""
< HTTP/1.1 200 OK

[
    {
        "DestinationServer":"http://localhost:8081",
        "DestinationFileSystem":"SlaveNorthwindFS",
        "Reports":[
            {
                "FileName":"/pdfs/DZone_DatabasePersistenceMgmt.pdf",
                "FileETag":"00000000-0000-0001-0000-000000000004",
                "BytesTransfered":3604201,
                "BytesCopied":0,
                "NeedListLength":1,
                "Exception":null,
                "Type":"ContentUpdate"
            },
            {
                "FileName":"/pdfs/NoSQL Tech Comparison Report.pdf",
                "FileETag":"00000000-0000-0001-0000-000000000006",
                "BytesTransfered":1408461,
                "BytesCopied":0,
                "NeedListLength":1,
                "Exception":null,
                "Type":"ContentUpdate"
            }
        ],
        "Exception":null
    }
]
{CODE-BLOCK/}

{PANEL/}

{PANEL:Synchronization of a particular file to a single file system}

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/synchronization/start/{fileName}  \
	-X POST \
	-d {destination} 
{CODE-BLOCK/}

### Request

| Payload |
| ------- |
| The JSON formatted [`SynchronizationDestination`](../../../../glossary/synchronization-destination) object|

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **fileName** | Yes | The full file name|

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **[`SynchronizationReport`](../../../../glossary/synchronization-report)** | The object that represent the result of the synchronization |

<hr />

## Example

Synchronize file `/books/Inside.RavenDB.3.0.pdf` to `SlaveNorthwindFS` file system on `http://localhost:8081/` server:

{CODE-BLOCK:json}
curl \
    -X POST http://localhost:8080/fs/NorthwindFS/synchronization/start/books/Inside.RavenDB.3.0.pdf \
    -d "{'ServerUrl':'http://localhost:8081','FileSystem':'SlaveNorthwindFS'}

< HTTP/1.1 200 OK

{
    "FileName":"/books/Inside.RavenDB.3.0.pdf",
    "FileETag":"00000000-0000-0001-0000-000000000002",
    "BytesTransfered":1202187,
    "BytesCopied":0,"NeedListLength":1,
    "Exception":null,
    "Type":"ContentUpdate"
}
{CODE-BLOCK/}

{PANEL/}


- [How file synchronization works?](../../../synchronization/how-it-works)

