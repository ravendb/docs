# Session: How to use MoreLikeThis?

`MoreLikeThis` is available through `advanced()` in session operations. This method returns articles similar to the provided input.

## Syntax

{CODE:java more_like_this_1@ClientApi\Session\HowTo\MoreLikeThis.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **index** | String | Name of an index on which query will be executed. |
| **documentId** | String | Id of a document for which similarities will be searched. |
| **parameters** | [MoreLikeThisQuery](../../../glossary/more-like-this-query) | A more like this query definition that will be executed. |

| Return Value | |
| ------------- | ----- |
| T[] | Array of similar documents returned as entities. |

## Example I

{CODE:java more_like_this_2@ClientApi\Session\HowTo\MoreLikeThis.java /}

## Example II

{CODE:java more_like_this_4@ClientApi\Session\HowTo\MoreLikeThis.java /}

## Related articles

- [Server : Bundles : MoreLikeThis](../../../server/bundles/more-like-this)
