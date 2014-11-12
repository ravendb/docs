# Commands : Querying : How to work with Suggestion query?

To take advantage of a suggestion feature use the **Suggest** method from the commands.

## Syntax


{CODE-BLOCK:json}

  curl -X GET http://{serverName}/databases/{databaseName}/suggest/{indexName}? \
	&term={term} \
	&field={field} \
	&max={max} \
	&popularity={popularity} \
	&distance={distance} \
	&accuracy={accuracy}
{CODE-BLOCK/}

### Request

| Query parameters | Required | |
| ------------- | -- | ---- |
| **index** | yes | A name of an index to query. |
| **term** | yes | term used to compute suggestions |
| **field** | yes | field used for suggestions |
| **max** | no | maximum number of suggestions |
| **popularity** | no | sort results by popularity |
| **distance** | no | method used for computing distance |
| **accuracy** | no | accuracy level |

### Response

| Status code | |
| ----------- | - |
| `200` | OK |

| Return Value | |
| ------------- | ------------- |
| Suggestions | array of suggestions |

| Header | |
| -------- | - |
| **ETag** | index etag |

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