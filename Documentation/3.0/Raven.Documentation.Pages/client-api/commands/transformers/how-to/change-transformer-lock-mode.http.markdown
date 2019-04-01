# Commands: Transformers: How to change transformer lock mode?

**SetTransformerLock** method allows you to change transformer lock mode for a given transformer.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/transformers/{transformerName}?op=lockModeChange&mode={lockMode} \
	-X POST \
	-d ""
{CODE-BLOCK/}

| Payload |
| ------- |
| Empty |

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **transformerName** | Yes | Name of an transformer |
| **lockMode** | Yes | Transformer lock mode: Unlock or LockedIgnore |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |
| `404` | Transformer not found |

| Return Value | Description |
| ------------- | ----- |
| None | - |

### Example

{CODE-BLOCK:json}
curl -X POST "http://localhost:8080/databases/Northwind/transformers/Orders/Company?op=lockModeChange&mode=LockedIgnore" -d ""
< HTTP/1.1 201 Created
{CODE-BLOCK/}

## Related articles

- [How to **change index lock mode**?](../../../../client-api/commands/indexes/how-to/change-index-lock-mode)  
