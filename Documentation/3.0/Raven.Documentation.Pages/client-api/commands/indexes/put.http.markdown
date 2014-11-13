# Commands : Indexes : Put


**PutIndex** is used to insert an index into a database.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/indexes/{indexName}?definition=yes \
	-X PUT \
	-d @indexDefinition.txt
{CODE-BLOCK/}

| Payload |
| ------- |
| [Index Definition](../../../glossary/index-definition) |

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **indexName** | Yes | Name of an index |

### Response

| Status code | Description |
| ----------- | - |
| `201` | Created |

| Return Value | Description |
| ------------- | ----- |
| **Index** | Index **name** |

<hr />

### Example

{CODE-BLOCK:json}
curl -X PUT "http://localhost:8080/databases/NorthWind/indexes/Orders/Totals?definition=yes" \
 -d " {\"MaxIndexOutputsPerDocument\":null, \
	\"IndexId\":0, \
	\"Name\":null, \
	\"LockMode\":\"Unlock\", \
	\"Maps\":[\" from order in docs.Orders  select new  {     order.Employee,    order.Company,    Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))}\"], \
	\"Reduce\":null, \
	\"Stores\":{}, \
	\"Indexes\":{}, \
	\"SortOptions\":{}, \
	\"Analyzers\":{}, \
	\"Fields\":[], \
	\"Suggestions\":{}, \
	\"TermVectors\":{}, \
	\"SpatialIndexes\":{}, \
	\"DisableInMemoryIndexing\":false, \
	\"Type\":\"Map\", \
	\"Map\":\" from order in docs.Orders  select new  {     order.Employee,    order.Company,    Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))}\", \
	\"IsCompiled\":false, \
	\"IsMapReduce\":false} "
< HTTP/1.1 201 Created
{"Index":"Orders/Totals"}
{CODE-BLOCK/}

## Related articles

- [How to **reset index**?](../../../client-api/commands/indexes/how-to/reset-index)  
- [GetIndex](../../../client-api/commands/indexes/get)  
- [DeleteIndex](../../../client-api/commands/indexes/delete)  
