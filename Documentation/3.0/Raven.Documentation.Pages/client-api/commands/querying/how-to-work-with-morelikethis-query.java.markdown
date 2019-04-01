# Commands: Querying: How to work with MoreLikeThis query?

To find similar or related documents use the **MoreLikeThis** method from commands.

## Syntax

{CODE:java more_like_this_1@ClientApi\Commands\Querying\HowToWorkWithMoreLikeThisQuery.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **query** | [MoreLikeThisQuery](../../../glossary/more-like-this-query) | A more like this query definition that will be executed |

| Return Value | |
| ------------- | ----- |
| [MultiLoadResult](../../../glossary/multi-load-result) | Instance of MultiLoadResult containing query `Results` and `Includes` (if any). |

## Example I

{CODE:java more_like_this_2@ClientApi\Commands\Querying\HowToWorkWithMoreLikeThisQuery.java /}

## Example II

{CODE:java more_like_this_3@ClientApi\Commands\Querying\HowToWorkWithMoreLikeThisQuery.java /}

## Related articles

- [Full RavenDB query syntax](../../../indexes/querying/full-query-syntax)   
- [How to **query** a **database**?](../../../client-api/commands/querying/how-to-query-a-database)   
- [How to **stream query** results?](../../../client-api/commands/querying/how-to-stream-query-results)   
