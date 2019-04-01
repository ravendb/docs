# Commands: Indexes: Get

There are few methods that allow you to retrieve an index from a database:   
- [GetIndex](../../../client-api/commands/indexes/get#getindex)   
- [GetIndexes](../../../client-api/commands/indexes/get#getindexes)   
- [GetIndexNames](../../../client-api/commands/indexes/get#getindexnames)   

{PANEL:GetIndex}

**GetIndex** is used to retrieve an index definition from a database.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/indexes/{indexName}?definition=yes \
	-X GET 
{CODE-BLOCK/}

### Request

| Query parameter | Required |  Description |
| ------------- | -- | ---- |
| **indexName** | Yes | Name of an index |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **Index** | [IndexDefinition](../../../glossary/index-definition) json |

<hr />

### Example I

Get `Orders/Totals` index.

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/indexes/Orders/Totals?definition=yes"
< HTTP/1.1 200 OK
{"Index":
	{"IndexId":6,
	"Name":"Orders/Totals",
	"LockMode":"Unlock",
	"Map":" from order in docs.Orders  
		select new  {    
			order.Employee,    
			order.Company,    
			Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))}",
	"Maps":[
		" from order in docs.Orders  select new  {     
			order.Employee,    
			order.Company,    
			Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))}"],
	"Reduce":null,
	"IsMapReduce":false,
	"IsCompiled":false,
	"Stores":{},
	"Indexes":{},
	"SortOptions":{},
	"Analyzers":{},
	"Fields":["Total","__document_id","Employee","Company"],
	"Suggestions":{},
	"TermVectors":{},
	"SpatialIndexes":{},
	"InternalFieldsMapping":null,
	"MaxIndexOutputsPerDocument":null,
	"Type":"Map",
	"DisableInMemoryIndexing":false
	}
}
{CODE-BLOCK/}

{PANEL/}

{PANEL:GetIndexes}

**GetIndexes** is used to retrieve multiple index definitions from a database.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/indexes/ \
		&start={start} \
		&pageSize={pageSize} \
	-X GET 
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **start** | No | Number of indexes that should be skipped |
| **pageSize** | No | Maximum number of indexes that will be retrieved  |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| payload | Array of [IndexDefinition](../../../glossary/index-definition) |

<hr />

### Example I

Get up to 10 index definitions

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/indexes/?start=0&pageSize=10"
< HTTP/1.1 200 OK
[ indexDefinition, indexDefinition, ... ]
{CODE-BLOCK/}

{PANEL/}

{PANEL:GetIndexNames}

**GetIndexNames** is used to retrieve multiple index names from a database.


### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/indexes/? \
		namesOnly=true \
		&start={start} \
		&pageSize={pageSize} \
	-X GET
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **start** | No | Number of index names that should be skipped |
| **pageSize** | No | Maximum number of index names that will be retrieved |


### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| payload | list of strings: index names |

<hr />

### Example I

Gets up to 10 index names.

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/indexes/?namesOnly=true&start=0&pageSize=10" 
< HTTP/1.1 200 OK
["Orders/ByCompany","Orders/Total2s","Orders/Totals","Product/Sales","Raven/DocumentsByEntityName"]
{CODE-BLOCK/}

{PANEL/}

## Related articles

- [PutIndex](../../../client-api/commands/indexes/put)  
- [DeleteIndex](../../../client-api/commands/indexes/delete)  
