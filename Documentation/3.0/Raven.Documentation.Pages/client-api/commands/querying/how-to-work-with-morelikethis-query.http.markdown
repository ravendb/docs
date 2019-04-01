# Commands: Querying: How to work with MoreLikeThis query?

To find similar or related documents use the **/morelikethis/** endpoint.

## Syntax

{CODE-BLOCK:json}
curl \
	http://{serverName}/databases/{databaseName}/morelikethis/? \
		&index={indexName} \
		&docId={documentId} \
		&fields={field} \
		&boost={boost} \ 
		&boostFactor={boostFactor} \
		&maxQueryTerms={maxQueryTerms} \
		&maxNumTokens={maxNumTokens} \
		&maxWordLen={maxWordLen} \
		&minDocFreq={minDocFreq} \
		&maxDocFreq={maxDocFreq} \
		&maxDocFreqPct={maxDocFreqPct} \
		&minTermFreq={minTermFreq} \
		&minWordLen={minWordLen} \
		&stopWords={stopWords} \
		&resultsTransformer={resultsTransformer} \
		&tp-{param}={value} \
		&include={include} \
	-X GET
{CODE-BLOCK/}

### Request

| Query parameter | Required | Multiple allowed | Description |
| ------------- | -- | ---- |
| **indexName** | Yes | No | A name of an index to query. |
| **documentId** | Yes | No | Document id to be used for more like this |
| **fields** | No | Yes | Fields used for comparision |
| **boost** | No | No | Whether to enable boost |
| **boostFactor** | No | No | Boost factor |
| **maxQueryTerms** | No | No | Maximum query terms |
| **maxNumTokens** | No | No | Maximum number of tokens |
| **maxWordLen** | No | No | Maximum word length |
| **minDocFreq** | No | No | Minimum document frequency |
| **maxDocFreq** | No | No | Maximum document frequency |
| **maxDocFreqPct** | No | No | Maximum document frequency (as percentage) |
| **minTermFreq** | No | No | Minimum term frequency |
| **minWordLen** | No | No | Minimum word length |
| **stopWords** | No | No | Name of document which contains stop words |
| **resultsTransformer** | No | No | Result transformer to use |
| {param}={value} | No | Yes | Transformer parameters |
| **include** | No | No | Include paths |


### Response

| Status code | Description |
| ----------- | - |
| `200` | OK |

| Return Value | Description |
| ------------- | ------------- |
| **Results** | List of requested documents |
| **Includes** | List of included documents |

<hr />

## Example

{CODE-BLOCK:json}
curl -X GET "http://localhost:8080/databases/NorthWind/suggest/Users/ByFullName?term=johne&field=FullName&max=10&popularity=false&distance=Levenshtein" 
< HTTP/1.1 200 OK
{"Results":[...results ...],"Includes":[]}
{CODE-BLOCK/}

## Related articles

- [Full RavenDB query syntax](../../../indexes/querying/full-query-syntax)   
- [How to **query** a **database**?](../../../client-api/commands/querying/how-to-query-a-database)   
- [How to **stream query** results?](../../../client-api/commands/querying/how-to-stream-query-results)   
