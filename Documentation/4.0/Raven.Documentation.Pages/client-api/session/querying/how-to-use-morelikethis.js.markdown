# Session: How to Use MoreLikeThis

`MoreLikeThis` is available through query methods and will return similar documents according to the provided criteria and options.

## Syntax

{CODE:nodejs more_like_this_1@client-api\session\querying\moreLikeThis.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **moreLikeThis** | `MoreLikeThisBase` | Defines the type of MoreLikeThis that should be executed |
| **builder** | `function` | Builder with fluent API that constructs the `MoreLikeThisBase` instance |

### Builder

{CODE:nodejs more_like_this_3@client-api\session\querying\moreLikeThis.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **documentJson** | string | Inline JSON document that will be used as a base for operation |
| **builder** | `(filterBuilder) => void` | Filtering expression utilized to find a document that will be used as a base for operation |
| **options** | object | Non-default options that should be used for operation |
| &nbsp;&nbsp;&nbsp;*minimumTermFrequency* | number | Ignores terms with less than this frequency in the source doc |
| &nbsp;&nbsp;&nbsp;*maximumQueryTerms* | number | Returns a query with no more than this many terms |
| &nbsp;&nbsp;&nbsp;*maximumNumberOfTokensParsed* | number | The maximum number of tokens to parse in each example doc field that is not stored with TermVector support |
| &nbsp;&nbsp;&nbsp;*minimumWordLength* | number | Ignores words less than this length or, if 0, then this has no effect |
| &nbsp;&nbsp;&nbsp;*maximumWordLength* | number | Ignores words greater than this length or if 0 then this has no effect |
| &nbsp;&nbsp;&nbsp;*minimumDocumentFrequency* | number | Ignores words which do not occur in at least this many documents |
| &nbsp;&nbsp;&nbsp;*maximumDocumentFrequency* | number | Ignores words which occur in more than this many documents |
| &nbsp;&nbsp;&nbsp;*maximumDocumentFrequencyPercentage* | number | Ignores words which occur in more than this percentage of documents |
| &nbsp;&nbsp;&nbsp;*boost* | boolean | Boost terms in query based on score |
| &nbsp;&nbsp;&nbsp;*boostFactor* | number |  Boost factor when boosting based on score |
| &nbsp;&nbsp;&nbsp;*stopWordsDocumentId* | string | Document ID containing custom stop words |
| &nbsp;&nbsp;&nbsp;*fields* | string[] | Fields to compare |

## Example I

{CODE-TABS}
{CODE-TAB:nodejs:Node.js more_like_this_4@client-api\session\querying\moreLikeThis.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Articles/MoreLikeThis' 
where morelikethis(id() = 'articles/1', '{ "Fields" : [ "body" ] }')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example II

{CODE-TABS}
{CODE-TAB:nodejs:Node.js more_like_this_6@client-api\session\querying\moreLikeThis.js /}
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
