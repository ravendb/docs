# Migration: How to Migrate MoreLikeThis from 3.x

MoreLikeThis functionality has been merged into [RQL](../../../../indexes/querying/what-is-rql). To reflect this change the Client API has integrated this feature into the `session.Query` and `session.Advanced.DocumentQuery`. The following migration samples will focus on the `session.Query` - the most common and recommended way of interaction with querying capabilities on RavenDB.

## Namespaces

| 3.x |
|:---:|
| {CODE more_like_this_5@Migration\ClientApi\Session\Querying\MoreLikeThis.cs /} |

| 4.0 |
|:---:|
| {CODE more_like_this_6@Migration\ClientApi\Session\Querying\MoreLikeThis.cs /} |

## Example I

| 3.x |
|:---:|
| {CODE more_like_this_1@Migration\ClientApi\Session\Querying\MoreLikeThis.cs /} |

| 4.0 |
|:---:|
| {CODE more_like_this_2@Migration\ClientApi\Session\Querying\MoreLikeThis.cs /} |

## Example II

| 3.x |
|:---:|
| {CODE more_like_this_3@Migration\ClientApi\Session\Querying\MoreLikeThis.cs /} |

| 4.0 |
|:---:|
| {CODE more_like_this_4@Migration\ClientApi\Session\Querying\MoreLikeThis.cs /} |

## Remarks

{INFO You can read more about MoreLikeThis in a dedicated Client API article that can be found [here](../../../../client-api/session/querying/how-to-use-morelikethis). /}
