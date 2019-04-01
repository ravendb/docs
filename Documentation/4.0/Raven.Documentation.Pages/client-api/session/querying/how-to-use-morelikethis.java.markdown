# Session: How to Use MoreLikeThis

`MoreLikeThis` is available through query methods and will return similar documents according to the provided criteria and options.

## Syntax

{CODE:java more_like_this_1@ClientApi\Session\Querying\MoreLikeThis.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **moreLikeThis** | `MoreLikeThisBase` | Defines the type of MoreLikeThis that should be executed |
| **builder** | `Consumer<IMoreLikeThisBuilderForDocumentQuery<T>>` | Builder with fluent API that constructs the `MoreLikeThisBase` instance |

### Builder

{CODE:java more_like_this_3@ClientApi\Session\Querying\MoreLikeThis.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **documentJson** | String | Inline JSON document that will be used as a base for operation |
| **builder** | `Consumer<IFilterDocumentQueryBase<T, IDocumentQuery<T>>>` | Filtering expression utilized to find a document that will be used as a base for operation |
| **options** | `MoreLikeThisOptions` | Non-default options that should be used for operation |

### Options

{CODE:java more_like_this_2@ClientApi\Session\Querying\MoreLikeThis.java /}

| Options | | |
| ------------- | ------------- | ----- |
| **MinimumTermFrequency** | Integer | Ignores terms with less than this frequency in the source doc |
| **MaximumQueryTerms** | Integer | Returns a query with no more than this many terms |
| **MaximumNumberOfTokensParsed** | Integer | The maximum number of tokens to parse in each example doc field that is not stored with TermVector support |
| **MinimumWordLength** | Integer | Ignores words less than this length or, if 0, then this has no effect |
| **MaximumWordLength** | Integer | Ignores words greater than this length or if 0 then this has no effect |
| **MinimumDocumentFrequency** | Integer | Ignores words which do not occur in at least this many documents |
| **MaximumDocumentFrequency** | Integer | Ignores words which occur in more than this many documents |
| **MaximumDocumentFrequencyPercentage** | Integer | Ignores words which occur in more than this percentage of documents |
| **Boost** | Boolean | Boost terms in query based on score |
| **BoostFactor** | Float |  Boost factor when boosting based on score |
| **StopWordsDocumentId** | String | Document ID containing custom stop words |
| **Fields** | String[] | Fields to compare |

## Example I

{CODE-TABS}
{CODE-TAB:java:Java more_like_this_4@ClientApi\Session\Querying\MoreLikeThis.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Articles/MoreLikeThis' 
where morelikethis(id() = 'articles/1', '{ "Fields" : [ "body" ] }')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example II

{CODE-TABS}
{CODE-TAB:java:Java more_like_this_6@ClientApi\Session\Querying\MoreLikeThis.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Articles/MoreLikeThis' 
where morelikethis(id() = 'articles/1', '{ "Fields" : [ "body" ] }') and category == 'IT'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Querying

- [MoreLikeThis](../../../indexes/querying/morelikethis)
