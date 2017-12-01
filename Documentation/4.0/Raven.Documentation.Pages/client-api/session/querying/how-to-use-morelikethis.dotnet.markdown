# Session : How to use MoreLikeThis?

`MoreLikeThis` is available through query extension methods and will return documents similar to the provided input.

## Syntax

{CODE more_like_this_1@ClientApi\Session\Querying\MoreLikeThis.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **moreLikeThis** | `MoreLikeThisBase` | Defines the type of MoreLikeThis that should be executed. |
| **factory** | `Action<IMoreLikeThisFactory<T>>` | Factory with fluent API that helps to build `MoreLikeThisBase` implementation |

### Options

{CODE more_like_this_2@ClientApi\Session\Querying\MoreLikeThis.cs /}

| Options | | |
| ------------- | ------------- | ----- |
| **MinimumTermFrequency** | int? | Ignore terms with less than this frequency in the source doc |
| **MaximumQueryTerms** | int? | Return a Query with no more than this many terms |
| **MaximumNumberOfTokensParsed** | int? | The maximum number of tokens to parse in each example doc field that is not stored with TermVector support |
| **MinimumWordLength** | int? | Ignore words less than this length or if 0 then this has no effect |
| **MaximumWordLength** | int? | Ignore words greater than this length or if 0 then this has no effect |
| **MinimumDocumentFrequency** | int? | Ignore words which do not occur in at least this many documents |
| **MaximumDocumentFrequency** | int? | Ignore words which occur in more than this many documents |
| **MaximumDocumentFrequencyPercentage** | int? | Ignore words which occur in more than this percentage of documents |
| **Boost** | bool? | Boost terms in query based on score |
| **BoostFactor** | float? |  Boost factor when boosting based on score |
| **StopWordsDocumentId** | string | Document id containing custom stop words |
| **Fields** | string[] | Fields to compare |

### Factory

{CODE more_like_this_3@ClientApi\Session\Querying\MoreLikeThis.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **documentJson** | string | Inline JSON document that will be used as a base for operation |
| **predicate** | `Expression<Func<T, bool>>` | Filtering expression utilized to find a document that will be used as a base for operation |
| **options** | `MoreLikeThisOptions` | Non-default options that should be used for operation |

## Example I

{CODE-TABS}
{CODE-TAB:csharp:Sync more_like_this_4@ClientApi\Session\Querying\MoreLikeThis.cs /}
{CODE-TAB:csharp:Async more_like_this_5@ClientApi\Session\Querying\MoreLikeThis.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Articles/MoreLikeThis' 
where morelikethis(id() == 'articles/1', '{ Fields : [ "Body" ] }')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example II

{CODE-TABS}
{CODE-TAB:csharp:Sync more_like_this_6@ClientApi\Session\Querying\MoreLikeThis.cs /}
{CODE-TAB:csharp:Async more_like_this_7@ClientApi\Session\Querying\MoreLikeThis.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Articles/MoreLikeThis' 
where morelikethis(id() == 'articles/1', '{ Fields : [ "Body" ] }') AND Category == 'IT'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Remarks

Do not forget to add following **using** statement which contains necessary extensions:

{CODE more_like_this_8@ClientApi\Session\Querying\MoreLikeThis.cs /}

## Related articles

- [Server : Bundles : MoreLikeThis](../../../server/bundles/more-like-this)
