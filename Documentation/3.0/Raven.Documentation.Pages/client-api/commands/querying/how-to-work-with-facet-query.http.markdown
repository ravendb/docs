# Commands: Querying: How to work with Facet query?

There are two methods that allow you to send a Facet query to a database:   
- [GetFacets](../../../client-api/commands/querying/how-to-work-with-facet-query#getfacets)    
- [GetMultiFacets](../../../client-api/commands/querying/how-to-work-with-facet-query#getmultifacets)   

{PANEL:GetFacets}

There are few overloads for the **GetFacets** method and the main difference between them is a source of the facets. In one facets are passed as a parameter, in the other user must provide a `key` to a `facet setup` document.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/facets/{indexName}? \
		&facetDoc={facetSetupDoc} \
		&facetStart={start} \
		&facetPageSize={pageSize} \
		&facets={facets}
		[Other indexQuery parameters] \
	-X GET 
&nbsp;
curl \
	http://{serverName}/databases/{databaseName}/facets/{indexName}? \
		&facetStart={start} \
		&facetPageSize={pageSize} \
		[Other indexQuery parameters] \
	-X POST \
	-d @facets.txt
{CODE-BLOCK/}

{SAFE:IndexQuery parameters}
This endpoint accepts [IndexQuery](../../../glossary/index-query) object. All possible [IndexQuery](../../../glossary/index-query) parameters are listed [here](../../../client-api/commands/querying/how-to-query-a-database#indexquery-parameters)
{SAFE/}

### Request

| Method | Description |
| -------| - |
| `GET` | serialized facets length < 1024 |
| `PUT` | serialized facets length > 1024 (pass facets as payload) |

| Payload |
| ------- |
| Facets data (for `GET` only) |

| Query parameter | Required | Description |
| ------------- | -- | ---- |
| **indexName** | Yes | A name of an index to query |
| **facets** | Yes | Serialized list of [facets](../../../glossary/facet) required to perform a facet query (mutually exclusive with `facetSetupDoc`). |
| **facetSetupDoc** | Yes | Document key that contains predefined [FacetSetup](../../../glossary/facet-setup) (mutually exclusive with `facets`). |
| **start** | No | number of results that should be skipped.|
| **pageSize** | No | maximum number of results that will be retrieved. |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ----- |
| [FacetResults](../../../glossary/facet-results) | Facet query results containing query `Duration` and a list of `Results` - one entry for each term/range as specified in [FacetSetup](../../../glossary/facet-setup) document or passed in parameters. |

{PANEL/}

{PANEL:GetMultiFacets}

Sending multiple facet queries is achievable by using `/facets/multisearch` endpoint.

### Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/facets/multisearch \
	-X POST \
	-d @facets.txt
{CODE-BLOCK/}

### Request

| Payload |
| ------- |
| Serialized list of [FacetQuery](../../../glossary/facet-query) |

### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **Results** | list of [FacetResults](../../../glossary/facet-results) |

{PANEL/}

## Related articles

- [Full RavenDB query syntax](../../../indexes/querying/full-query-syntax)   
- [How to **query** a **database**?](../../../client-api/commands/querying/how-to-query-a-database)   
- [How to **stream query** results?](../../../client-api/commands/querying/how-to-stream-query-results)   
