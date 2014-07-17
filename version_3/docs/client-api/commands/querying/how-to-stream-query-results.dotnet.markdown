# Client API : Querying : How to stream query results?

Use **StreamQuery** method to stream results of a selected index according to a specified query.

## Syntax

{CODE stream_query_1@ClientApi\Commands\Querying\HowToStreamQueryResults.cs /}

index
:   Type: string   
A name of an index to query

query
:   Type: [IndexQuery]()   
A query definition containing all information required to query a specified index.

queryHeaderInfo
:   Type: [QueryHeaderInformation ]()   
Information about performed query

**Return Value**    

Type: IEnumerator`<RavenJObject>`   
Enumerator with query results

Type: QueryHeaderInformation    
Information about performed query

## Example

{CODE stream_query_2@ClientApi\Commands\Querying\HowToStreamQueryResults.cs /}

#### Related articles

- [Full RavenDB query syntax](../../../Indexes/full-query-syntax) 
- [How to **query** a **database**?](../../../client-api/commands/querying/how-to-query-a-database)