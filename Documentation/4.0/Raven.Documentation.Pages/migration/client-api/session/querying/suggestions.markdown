# How to migrate Suggestions from 3.x?

Suggestion functionality have been merged into [RQL](../../indexes/querying/what-is-rql) and to reflect that change the Client API have integrated this feature into the `session.Query` and `session.Advanced.DocumentQuery`. Below migration samples will focus on the `session.Query` - the most common and recommended way of interaction with querying capabilities on RavenDB.

## Namespaces

| 3.x | 4.0 |
|:---:|:---:|
| {CODE suggestions_3@Migration\ClientApi\Session\Querying\Suggestions.cs /} | {CODE suggestions_4@Migration\ClientApi\Session\Querying\Suggestions.cs /} |

## Example

| 3.x | 4.0 |
|:---:|:---:|
| {CODE suggestions_1@Migration\ClientApi\Session\Querying\Suggestions.cs /} | {CODE suggestions_2@Migration\ClientApi\Session\Querying\Suggestions.cs /} |

## Remarks

{INFO You can read more about Suggestions in a dedicated Client API article that can be found [here](../../../../client-api/session/querying/how-to-work-with-suggestions). /}
