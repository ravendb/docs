# Querying: MoreLikeThis

MoreLikeThis returns a list of similar documents that are related to a given document. This feature can be used for situations like when a user views an article. Many news sites show a list of the related articles at the bottom of the page. To accomplish this, the RavenDB MoreLikeThis uses the MoreLikeThis from the Lucene contrib project. To find out more about the algorithm, please read Aaron Johnson excellent blog post that is available [here](http://cephas.net/blog/2008/03/30/how-morelikethis-works-in-lucene/).      

## Setup

In order to work, MoreLikeThis requires access to the text in the index. Therefore, the index being queried needs to [store](../../indexes/storing-data-in-index) the fields or store the [term vectors](../../indexes/using-term-vectors) for those fields.

{CODE:nodejs more_like_this_4@indexes\querying\moreLikeThis.js /}

## Basic Usage

MoreLikeThis has many defaults already set and the simplest mode will satisfy the majority of the usage scenarios.

{CODE-TABS}
{CODE-TAB:nodejs:Node.js more_like_this_1@indexes\querying\moreLikeThis.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Articles/ByArticleBody' 
where morelikethis(id() = 'articles/1')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

MoreLikeThis will use all the fields defined in an index. To use only a specific field or fields, pass them in `MoreLikeThisOptions.fields` property.

{CODE-TABS}
{CODE-TAB:nodejs:Node.js more_like_this_2@indexes\querying\moreLikeThis.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Articles/ByArticleBody' 
where morelikethis(id() = 'articles/1', '{ "Fields" : [ "articleBody" ] }')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Options

Default parameters can be changed by manipulating `MoreLikeThisOptions` properties and passing them to the `MoreLikeThis`.

| Options | | |
| ------------- | ------------- | ----- |
| **minimumTermFrequency** | number | Ignores terms with less than this frequency in the source doc |
| **maximumQueryTerms** | number | Returns a query with no more than this many terms |
| **maximumNumberOfTokensParsed** | number | The maximum number of tokens to parse in each example doc field that is not stored with TermVector support |
| **minimumWordLength** | number | Ignores words less than this length or, if 0, then this has no effect |
| **maximumWordLength** | number | Ignores words greater than this length or if 0 then this has no effect |
| **minimumDocumentFrequency** | number | Ignores words which do not occur in at least this many documents |
| **maximumDocumentFrequency** | number | Ignores words which occur in more than this many documents |
| **maximumDocumentFrequencyPercentage** | number | Ignores words which occur in more than this percentage of documents |
| **boost** | boolean | Boost terms in query based on score |
| **boostFactor** | number |  Boost factor when boosting based on score |
| **stopWordsDocumentId** | string | Document ID containing custom stop words |
| **fields** | string[] | Fields to compare |

## Stop Words

Some of Lucene analyzers have a built-in list of common English words that are usually not useful for searching (like "a", "as", "the" etc.). Those words are called 
*stop words* and they are considered to be uninteresting and ignored. If a used analyzer does not support stop words, or you need to overload them, you can specify your own set of stop words.
A document with a list of stop words can be stored in RavenDB by storing the `MoreLikeThisStopWords` document:

{CODE:nodejs more_like_this_3@indexes\querying\moreLikeThis.js /}

The document ID is then set in the `MoreLikeThisOptions`.

## Remarks

{INFO Please note that default values for settings, like `minimumDocumentFrequency`, `minimumTermFrequency`, and `minimumWordLength`, may result in filtering out related articles, especially when there is little data set (e.g. during development). /}

## Related Articles

### Client API

- [How to Use MoreLikeThis](../../client-api/session/querying/how-to-use-morelikethis)
