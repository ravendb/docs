# Commands : Querying : How to work with MoreLikeThis query?

To find similar or related documents use **MoreLikeThis** method from commands.

## Syntax

{CODE more_like_this_1@ClientApi\Commands\Querying\HowToWorkWithMoreLikeThisQuery.cs /}

**Parameters**

query
:   Type: [MoreLikeThisQuery]()   
A more like this query definition that will be executed

**Return Value**

Type: [MultiLoadResult]()   
Instance of MultiLoadResult containing query `Results` and `Includes` (if any).

## Example

{CODE more_like_this_2@ClientApi\Commands\Querying\HowToWorkWithMoreLikeThisQuery.cs /}

#### Related articles

- [Full RavenDB query syntax](../../../Indexes/full-query-syntax)   
- [How to **query** a **database**?](../../../client-api/commands/querying/how-to-query-a-database)   
- [How to **stream query** results?](../../../client-api/commands/querying/how-to-stream-query-results)   