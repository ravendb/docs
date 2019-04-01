# Commands: Querying: How to work with MoreLikeThis query?

To find similar or related documents use the **MoreLikeThis** method from the commands.

## Syntax

{CODE more_like_this_1@ClientApi\Commands\Querying\HowToWorkWithMoreLikeThisQuery.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **query** | [MoreLikeThisQuery](../../../glossary/more-like-this-query) | A more like this query definition that will be executed |

| Return Value | |
| ------------- | ----- |
| [MultiLoadResult](../../../glossary/multi-load-result) | Instance of MultiLoadResult containing query `Results` and `Includes` (if any). |

## Example I

{CODE more_like_this_2@ClientApi\Commands\Querying\HowToWorkWithMoreLikeThisQuery.cs /}

## Example II

{CODE more_like_this_3@ClientApi\Commands\Querying\HowToWorkWithMoreLikeThisQuery.cs /}

## Related articles

- [Full RavenDB query syntax](../../../indexes/querying/full-query-syntax)   
- [How to **query** a **database**?](../../../client-api/commands/querying/how-to-query-a-database)   
- [How to **stream query** results?](../../../client-api/commands/querying/how-to-stream-query-results)   
