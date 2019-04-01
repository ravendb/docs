# Commands: Querying: How to work with Suggestion query?

To take advantage of a suggestion feature use the **Suggest** endpoint.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/suggest/{indexName}? \
		&term={term} \
		&field={field} \
		&max={max} \
		&popularity={popularity} \
		&distance={distance} \
		&accuracy={accuracy} \
	-X GET
{CODE-BLOCK/}

### Request

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **index** | Yes | A name of an index to query. |
| **term** | Yes | term used to compute suggestions |
| **field** | Yes | field used for suggestions |
| **max** | No | maximum number of suggestions |
| **popularity** | No | sort results by popularity |
| **distance** | No | method used for computing distance |
| **accuracy** | No | accuracy level |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **Suggestions** | array of suggestions |

| Header | Description |
| -------- | - |
| **ETag** | index etag |

<hr />

## Example

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/suggest/Users/ByFullName?term=johne&field=FullName&max=10&popularity=false&distance=Levenshtein" 

< HTTP/1.1 200 OK
{ "Suggestions" : [ "john", "jones", "johnson" ] }

{CODE-BLOCK/}

## Related articles

- [Full RavenDB query syntax](../../../indexes/querying/full-query-syntax)   
- [How to **query** a **database**?](../../../client-api/commands/querying/how-to-query-a-database)   
- [How to **stream query** results?](../../../client-api/commands/querying/how-to-stream-query-results)   
