# Migration: How to Migrate Highlighting from 3.x

Highlighting functionality has been merged into [RQL](../../../../indexes/querying/what-is-rql). Following minor and cosmetic changes have been introduced.

## Namespaces

| 3.x |
|:---:|
| {CODE highlighting_1@Migration\ClientApi\Session\Querying\Highlighting.cs /} |

| 4.0 |
|:---:|
| {CODE highlighting_2@Migration\ClientApi\Session\Querying\Highlighting.cs /} |

## Example I

| 3.x |
|:---:|
| {CODE highlighting_3@Migration\ClientApi\Session\Querying\Highlighting.cs /} |

| 4.0 |
|:---:|
| {CODE-TABS}
{CODE-TAB:csharp:Query highlighting_4@Migration\ClientApi\Session\Querying\Highlighting.cs /} 
{CODE-TAB:csharp:DocumentQuery highlighting_5@Migration\ClientApi\Session\Querying\Highlighting.cs /}
{CODE-TABS/} |

## Example II

| 3.x |
|:---:|
| {CODE highlighting_6@Migration\ClientApi\Session\Querying\Highlighting.cs /} |

| 4.0 |
|:---:|
| {CODE-TABS}
{CODE-TAB:csharp:Query highlighting_7@Migration\ClientApi\Session\Querying\Highlighting.cs /} 
{CODE-TAB:csharp:DocumentQuery highlighting_8@Migration\ClientApi\Session\Querying\Highlighting.cs /}
{CODE-TABS/} |

## Remarks

{INFO You can read more about Highlighting in our dedicated [Client API article](../../../../client-api/session/querying/how-to-use-highlighting) or our [Querying article](../../../../indexes/querying/highlighting). /}
