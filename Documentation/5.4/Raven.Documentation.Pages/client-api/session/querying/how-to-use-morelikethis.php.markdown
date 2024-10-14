# Using &nbsp; `moreLikeThis`

`moreLikeThis` is available through query extension methods and will return similar documents according to the provided criteria and options.

## Syntax

{CODE:php more_like_this_1@ClientApi\Session\Querying\MoreLikeThis.php /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **$moreLikeThisOrBuilder** | `null`<br>`MoreLikeThisBase`<br>`Closure` | Defines the type of MoreLikeThis that should be executed |

### Builder

{CODE:php more_like_this_3@ClientApi\Session\Querying\MoreLikeThis.php /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **$documentJsonOrBuilder** | `null`<br>`string`<br>`Closure` | Builder or Inline JSON document to be used as a base for the operation |
| **$documentJson** | ?string | Inline JSON document to be used as a base for the operation |
| **$builder** | `?Closure` | Builder with fluent API that constructs the `MoreLikeThisOperationsInterface` instance |
| **$options** | `MoreLikeThisOptions` | Available operation options (see below) |

### Options

{CODE:php more_like_this_2@ClientApi\Session\Querying\MoreLikeThis.php /}

| Options | | |
| ------------- | ------------- | ----- |
| **$minimumTermFrequency** | `?int` | Ignores terms with less than this frequency in the source doc |
| **$maximumQueryTerms** | `?int` | Returns a query with no more than this many terms |
| **$maximumNumberOfTokensParsed** | `?int` | The maximum number of tokens to parse in each example doc field that is not stored with TermVector support |
| **$minimumWordLength** | `?int` | Ignores words less than this length or, if 0, then this has no effect |
| **$maximumWordLength** | `?int` | Ignores words greater than this length or if 0 then this has no effect |
| **$minimumDocumentFrequency** | `?int` | Ignores words which do not occur in at least this many documents |
| **$maximumDocumentFrequency** | `?int` | Ignores words which occur in more than this many documents |
| **$maximumDocumentFrequencyPercentage** | `int?` | Ignores words which occur in more than this percentage of documents |
| **$boost** | `?bool` | Boost terms in query based on score |
| **$boostFactor** | `?float` |  Boost factor when boosting based on score |
| **$stopWordsDocumentId** | `?string` | Document ID containing custom stop words |
| **$fields** | `?array` | Fields to compare |

## Example I

{CODE-TABS}
{CODE-TAB:php:Sync more_like_this_4@ClientApi\Session\Querying\MoreLikeThis.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Articles/MoreLikeThis' 
where morelikethis(id() = 'articles/1', '{ "Fields" : [ "Body" ] }')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example II

{CODE-TABS}
{CODE-TAB:php:Sync more_like_this_6@ClientApi\Session\Querying\MoreLikeThis.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Articles/MoreLikeThis' 
where morelikethis(id() = 'articles/1', '{ "Fields" : [ "Body" ] }') and Category == 'IT'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Querying

- [MoreLikeThis](../../../indexes/querying/morelikethis)
