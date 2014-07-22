# Client API : Querying : How to work with Suggestion query?

To take advantage of a suggestion feature use **Suggest** method from commands.

## Syntax

{CODE suggest_1@ClientApi\Commands\Querying\HowToWorkWithSuggestionQuery.cs /}

**Parameters**

index
:   Type: string  
A name of an index to query.

suggestionQuery
:   Type: [SuggestionQuery]()  
A suggestion query definition containing all information required to query a specified index.

**Return Value**

Type: [SuggestionQueryResult]()  
Result containing array of all suggestions for executed query

## Example

{CODE suggest_2@ClientApi\Commands\Querying\HowToWorkWithSuggestionQuery.cs /}

#### Related articles

- [Full RavenDB query syntax](../../../Indexes/full-query-syntax)   
- [How to **query** a **database**?](../../../client-api/commands/querying/how-to-query-a-database)   
- [How to **stream query** results?](../../../client-api/commands/querying/how-to-stream-query-results)   