# Commands: Indexes: How to get index terms?

**GetTerms** will retrieve all stored terms for a field of an index.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/terms/{indexName}? \
		field={field} \
		&pageSize={pageSize} \
		&start={start} \
		&fromValue={fromValue} \
	-X GET 
{CODE-BLOCK/}

### Request

| Query parameter | Required |  Description |
| ------------- | -- | ---- |
| **indexName** | Yes | Name of an index |
| **field** | Yes | Index field |
| **fromValue** | No | Starting point for a query, used for paging |
| **pageSize** | No | Maximum number of terms that will be returned |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| payload | array of strings |

<hr />

### Example I

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/terms/Orders/Totals?field=Company&pageSize=128&fromValue=" 

< HTTP/1.1 200 OK
["companies/1","companies/10", ... ]
{CODE-BLOCK/}


## Related articles

- [How to **reset index**?](../../../../client-api/commands/indexes/how-to/reset-index)   
- [How to **get index merge suggestions**?](../../../../client-api/commands/indexes/how-to/get-index-merge-suggestions)   
