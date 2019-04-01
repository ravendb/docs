# Commands: How to create or delete database?

This article will describe the following commands that enable you to manage databases on a server:   
- [CreateDatabase](../../../client-api/commands/how-to/create-delete-database#createdatabase)   
- [DeleteDatabase](../../../client-api/commands/how-to/create-delete-database#deletedatabase)   

{PANEL:**CreateDatabase**}

Used to create a new database on a server.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/admin/databases/{databaseName} \
	-X PUT \
	-d @databaseDocument.txt
{CODE-BLOCK/}

### Request

| Payload |
| ------- |
| [DatabaseDocument](../../../glossary/database-document) |

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **databaseName** | Yes | Name of a database to created |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

<hr />

### Example

{CODE-BLOCK:json}
curl -X PUT "http://localhost:8080/admin/databases/NewDatabase" \
	-d "{\"SecuredSettings\":{},\"Settings\":{\"Raven/ActiveBundles\":\"PeriodicExport\",\"Raven/DataDir\":\"~//\"},\"Disabled\":false,\"Id\":\"NewDatabase\"}"
< HTTP/1.1 200 OK
{CODE-BLOCK/}

{PANEL/}

{PANEL:**DeleteDatabase**}

Used to delete a database from a server, with a possibility to remove all the data from hard drive.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/admin/databases/{databaseName} \
		&hard-delete={hardDelete} \
	-X DELETE
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description  |
| ------------- | -- | ---- |
| **databaseName** | Yes | Name of a database to delete |
| **hardDelete** | No | Should all data be removed (data files, indexing files, etc.). Default: `false` |

### Response

| Status code | Description |
| ----------- | - |
| `204` | No Content |

<hr />

### Example

{CODE-BLOCK:json}
curl -X PUT "http://localhost:8080/admin/databases/NewDatabase?hard-delete=true" \
< HTTP/1.1 204 No Content
{CODE-BLOCK/}

{PANEL/}


## Related articles

- [How to **switch** commands to different **database**?](../../../client-api/commands/how-to/switch-commands-to-a-different-database)   
- [How to get database and server **statistics**?](../../../client-api/commands/how-to/get-database-and-server-statistics)   
- [How to start **backup** or **restore** operations?](../../../client-api/commands/how-to/start-backup-restore-operations)   
