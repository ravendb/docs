# Client API : Querying : How to query a database?

Use **Query** method to fetch results of a selected index according to a specified query.

## Syntax

{CODE query_database_1@ClientApi\Commands\Querying\HowToQueryDatabase.cs /}

**Parameters**

index
:   Type: string   
A name of an index to query

query
:   Type: [IndexQuery]()   
A query definition containing all information required to query a specified index.

includes
:   Type: string[]   
An array of relative paths that specify related documents ids which should be included to query result.

metadataOnly
:   Type: bool   
True if returned documents should include only metadata withoud a document body.

indexEntriesOnly 
:   Type: bool   
True if query results should contains only [index entries](../../../glossary/indexing#index-entry).

**Return Value**    

Type: [QueryResult]()   
Object which represents results of a specified query.

##Example I

A sample **Query** method call that returns users with a specified name:

{CODE query_database_2@ClientApi\Commands\Querying\HowToQueryDatabase.cs /}

##Example II

If a model of your documents is that they reference others and you want to retrieve them together in a single query request then you need to specify paths to properties that contains IDs of referenced documents:

{CODE query_database_3@ClientApi\Commands\Querying\HowToQueryDatabase.cs /}

#### Related articles

- [Full RavenDB query syntax](../../../Indexes/full-query-syntax) 
- [How to **stream query** results?](../../../client-api/commands/querying/how-to-stream-query-results)