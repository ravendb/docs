# Commands : Querying : How to query a database?

Use **Query** method to fetch results of a selected index according to a specified query.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/indexes/{indexName}? \
		&metadata-only={metadataOnly} \
		&include={include} \
		[Other indexQuery parameters] \
	-X GET
{CODE-BLOCK/}

{SAFE:IndexQuery parameters}
This endpoint accepts [IndexQuery](../../../../glossary/index-query) object. All possible [IndexQuery](../../../../glossary/index-query) parameters are listed [here](../../../../client-api/commands/querying/how-to-query-a-database)
{SAFE/}

### Request

| Query parameter | Required | Description  |
| ------------- | -- | ---- |
| **indexName** | Yes | A name of an index to query |
| **include** | No | An array of relative paths that specify related documents ids which should be included in a query result. |
| **metadataOnly** | No | True if returned documents should include only metadata without a document body. |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **Results** | List of requested documents |
| **Includes** | List of included documents |

<hr />

## Example I

A sample **Query** method call that returns orders for a company specified:

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/indexes/Orders/Totals?&query=Company%3Acompanies%2F1&pageSize=128" 
< HTTP/1.1 200 OK
{"Results":[...results ...],"Includes":[]}
{CODE-BLOCK/}

## Example II

If a model of your documents is such that they reference others and you want to retrieve them together in a single query request, then you need to specify paths to properties that contain IDs of referenced documents:

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/indexes/Orders/Totals?&pageSize=128&include=Company&include=Employee" 
< HTTP/1.1 200 OK
{"Results":[...results ...],"Includes":[... includes ...]}
{CODE-BLOCK/}

## Related articles

- [Full RavenDB query syntax](../../../indexes/querying/full-query-syntax) 
- [How to **stream query** results?](../../../client-api/commands/querying/how-to-stream-query-results)