#Manage file systems

{PANEL: Create or update file system}

The **PUT** method is used to create a new file system. If the file system already exists then it will return `409 Conflict` result.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/admin/fs/{newFileSystemName}?update={update}  \
	-X PUT \
	--d "{filesystemDocument}"
{CODE-BLOCK/}

### Request

| Payload |
| ------- |
| The JSON formatted [FileSystemDocument](../../../../glossary/file-system-document). The document containing all configuration options for a new file system (e.g. active bundles, name/id, data path) |

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **newFileSystemName** | Yes | The new file system name |
| **update** | No | Determines whether it should create the new file system or update the existing one (default: `false`) |

### Response

| Status code | Description |
| ----------- | - |
| `201` | Created |
| `409` | The file system already exists (if `update` parameter was `false`) |

| Return Value | Description |
| ------------- | ------------- |
| **None** | The request does not return any message |

<hr />

## Example

Let's create a new file system or update the configuration of already existing one according to the specified document:

{CODE-BLOCK:json}
curl \
	-X PUT http://localhost:8080/admin/fs/NorthwindFS?update=true  \
	-d "{'Id' : 'Raven/FileSystems/NorthwindFS', 'Settings' : {'Raven/FileSystem/DataDir' : '~/FileSystems/NorthwindFS'}}"
< HTTP/1.1 201 Created

{CODE-BLOCK/}

{PANEL/}


{PANEL: Delete file system}

The **DELETE** method is used to delete a file system from a server, with a possibility to remove its all data from the hard drive.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverUrl}/admin/fs/{fileSystemName}?hard-delete={hard-delete}  \
	-X DELETE
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **fileSystemName** | Yes | The name of a file system to delete |
| **hardDelete** | No | Determines if all data should be removed (data files, indexing files, etc.). Default: `false` |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **None** | The request does not return any message |

<hr />

## Example

The below code deletes the file system `NorthwindFS` together with its data on the hard drive:

{CODE-BLOCK:json}
curl \
	-X DELETE http://localhost:8080/admin/fs/NorthwindFS?hard-delete=true 
< HTTP/1.1 200 OK

{CODE-BLOCK/}

{PANEL/}