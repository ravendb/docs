#Commands: Delete

The **DELETE** methods allow to remove a single file or multiple files at once from the file system.

{PANEL: Single file deletion}

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/files/{name}  \
	-X DELETE \
    --header "If-None-Match:{etag}"
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **name** | Yes | The name of a file to be deleted |

| Header | Required | Description |
| --------| ------- | --- |
| **If-None-Match** | No |  Used to pass the file `Etag` |

### Response

| Status code | Description |
| ----------- | - |
| `204` | No Content status means that the file has been deleted successfully |
| `405` | The concurrency exception occurred |
| `420` | The synchronization exception occurred |

| Return Value | Description |
| ------------- | ------------- |
| **None** | The request does not return any message |

<hr />

## Example

{CODE-BLOCK:json}
curl -X DELETE http://localhost:8080/fs/NorthwindFS/files/movies/intro.avi
< HTTP/1.1 204 No Content
{CODE-BLOCK/}

{PANEL/}

{PANEL: Multiple files deletion}

In order to delete multiple files you need to send the **DELETE** request to the search endpoint that specifies a query.
All files matching that query will be deleted. The actual delete operation is implemented as the background operation which identifier
is returned in the response. Later you use it to ask about the delete operation status.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/fs/{fileSystemName}/search?query={query}  \
	-X DELETE
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **query** | Yes | The Lucene query used to find files to delete |


### Response

| Status code | Description |
| ----------- | - |
| `200` | OK means that delete by query operation has been scheduled |

| Return Value | Description |
| ------------- | ------------- |
| **OperationId** | The identifier of the background delete by query operation |

<hr />

## Example

In order to start delete operation of all files located in the `/temp` folder and its subdirectories, run the following command:

{CODE-BLOCK:json}
curl -X DELETE http://localhost:8080/fs/NorthwindFS/search?query=__directoryName:/temp
< HTTP/1.1 200 OK
{
    "OperationId":1
}
{CODE-BLOCK/}

Check the operation status by using the following endpoint:

{CODE-BLOCK:json}
curl -X GET http://localhost:8080/fs/NorthwindFS/operation/status
{CODE-BLOCK/}

You will either get `OK` response with a state message or `NotFound` if the task completed successfully and was deleted.

{PANEL/}
