# Querying: MoreLikeThis

MoreLikeThis returns a list of similar documents that are related to a given document. This feature can be used for situations like when a user views an article. Many news sites show a list of the related articles at the bottom of the page. To accomplish this, the RavenDB MoreLikeThis uses the MoreLikeThis from the Lucene contrib project. To find out more about the algorithm, please read Aaron Johnson excellent blog post that is available [here](http://cephas.net/blog/2008/03/30/how-morelikethis-works-in-lucene/).      

## Setup

In order to work, MoreLikeThis requires access to the text in the index. Therefore, the index being queried needs to [store](../../indexes/storing-data-in-index) the fields or store the [term vectors](../../indexes/using-term-vectors) for those fields.

{CODE more_like_this_4@Indexes\Querying\MoreLikeThis.cs /}

## Basic Usage

MoreLikeThis has many defaults already set and the simplest mode will satisfy the majority of the usage scenarios.

{CODE-TABS}
{CODE-TAB:csharp:Query more_like_this_1@Indexes\Querying\MoreLikeThis.cs /}
{CODE-TAB:csharp:DocumentQuery more_like_this_1_1@Indexes\Querying\MoreLikeThis.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Articles/ByArticleBody' 
where morelikethis(id() = 'articles/1')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

MoreLikeThis will use all the fields defined in an index. To use only a specific field or fields, pass them in `MoreLikeThisOptions.Fields` property.

{CODE-TABS}
{CODE-TAB:csharp:Query more_like_this_2@Indexes\Querying\MoreLikeThis.cs /}
{CODE-TAB:csharp:DocumentQuery more_like_this_2_1@Indexes\Querying\MoreLikeThis.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Articles/ByArticleBody' 
where morelikethis(id() = 'articles/1', '{ "Fields" : [ "ArticleBody" ] }')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Options

Default parameters can be changed by manipulating `MoreLikeThisOptions` properties and passing them to the `MoreLikeThis`.

| Options | | |
| ------------- | ------------- | ----- |
| **MinimumTermFrequency** | int? | Ignores terms with less than this frequency in the source doc |
| **MaximumQueryTerms** | int? | Returns a query with no more than this many terms |
| **MaximumNumberOfTokensParsed** | int? | The maximum number of tokens to parse in each example doc field that is not stored with TermVector support |
| **MinimumWordLength** | int? | Ignores words less than this length or, if 0, then this has no effect |
| **MaximumWordLength** | int? | Ignores words greater than this length or if 0 then this has no effect |
| **MinimumDocumentFrequency** | int? | Ignores words which do not occur in at least this many documents |
| **MaximumDocumentFrequency** | int? | Ignores words which occur in more than this many documents |
| **MaximumDocumentFrequencyPercentage** | int? | Ignores words which occur in more than this percentage of documents |
| **Boost** | bool? | Boost terms in query based on score |
| **BoostFactor** | float? |  Boost factor when boosting based on score |
| **StopWordsDocumentId** | string | Document ID containing custom stop words |
| **Fields** | string[] | Fields to compare |

## Stop Words

Some of Lucene analyzers have a built-in list of common English words that are usually not useful for searching (like "a", "as", "the" etc.). Those words are called 
*stop words* and they are considered to be uninteresting and ignored. If a used analyzer does not support stop words, or you need to overload them, you can specify your own set of stop words.
A document with a list of stop words can be stored in RavenDB by storing the `MoreLikeThisStopWords` document:

{CODE more_like_this_3@Indexes\Querying\MoreLikeThis.cs /}

The document ID is then set in the `MoreLikeThisOptions`.

## Remarks

{INFO Please note that default values for settings, like `MinimumDocumentFrequency`, `MinimumTermFrequency`, and `MinimumWordLength`, may result in filtering out related articles, especially when there is little data set (e.g. during development). /}

## Related Articles

### Client API

- [How to Use MoreLikeThis](../../client-api/session/querying/how-to-use-morelikethis)
