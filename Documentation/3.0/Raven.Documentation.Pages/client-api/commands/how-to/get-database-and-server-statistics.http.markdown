# Commands: How to get database and server statistics?

There are two types of statistics available. First type of statistics returns server-wide information, the second one returns statistics for particular database.

{PANEL:Database statistics}

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/stats \
	-X GET
{CODE-BLOCK/}

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| [DatabaseStatistics](../../../glossary/database-statistics) | Current statistics for a database that commands work on |

{PANEL/}

{PANEL:Server statistics}

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/admin/stats \
	-X GET
{CODE-BLOCK/}

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| [AdminStatistics](../../../glossary/admin-statistics) | Current statistics for a server |

{PANEL/}

## Related articles

- [How to **switch** commands to different **database**?](../../../client-api/commands/how-to/switch-commands-to-a-different-database)   
