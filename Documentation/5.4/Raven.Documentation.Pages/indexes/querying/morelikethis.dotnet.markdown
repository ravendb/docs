# Querying: MoreLikeThis
---

{NOTE: }

* `MoreLikeThis` returns a list of documents that are related to a given document.  
* This feature can be used, for example, to show a list of related articles at the 
  bottom of the currently-read article page, as done in many news sites.  
* To accomplish this, RavenDB uses the Lucene contrib project `MoreLikeThis` feature.  

* In this page:

    * [Setup](../../indexes/querying/morelikethis#setup)
    * [Basic Usage](../../indexes/querying/morelikethis#basic-usage)
    * [Options](../../indexes/querying/morelikethis#options)
    * [Stop Words](../../indexes/querying/morelikethis#stop-words)
    * [Remarks](../../indexes/querying/morelikethis#remarks)

{NOTE/}

---

{PANEL: Setup}

To be able to work, `MoreLikeThis` requires access to the index text.  
The queried index needs, therefore, to [store](../../indexes/storing-data-in-index) 
the fields or the [term vectors](../../indexes/using-term-vectors) for these fields.

{CODE more_like_this_4@Indexes\Querying\MoreLikeThis.cs /}

{PANEL/}

{PANEL: Basic Usage}

Many `MoreLikeThis` options are set by default.  
The simplest mode will satisfy most usage scenarios.  

{CODE-TABS}
{CODE-TAB:csharp:Query more_like_this_1@Indexes\Querying\MoreLikeThis.cs /}
{CODE-TAB:csharp:DocumentQuery more_like_this_1_1@Indexes\Querying\MoreLikeThis.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Articles/ByArticleBody' 
where morelikethis(id() = 'articles/1')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

`MoreLikeThis` will use **all** the fields defined in an index.  
To use only specific fields, pass these fields in the `MoreLikeThisOptions.Fields` property.

{CODE-TABS}
{CODE-TAB:csharp:Query more_like_this_2@Indexes\Querying\MoreLikeThis.cs /}
{CODE-TAB:csharp:DocumentQuery more_like_this_2_1@Indexes\Querying\MoreLikeThis.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Articles/ByArticleBody' 
where morelikethis(id() = 'articles/1', '{ "Fields" : [ "ArticleBody" ] }')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Options}

Default parameters can be changed by manipulating `MoreLikeThisOptions` properties and passing them 
to `MoreLikeThis`.

| Option | Type | Description |
| ------------- | ------------- | ----- |
| **MinimumTermFrequency** | `int?` | Ignores terms with less than this frequency in the source doc |
| **MaximumQueryTerms** | `int?` | Returns a query with no more than this many terms |
| **MaximumNumberOfTokensParsed** | `int?` | The maximum number of tokens to parse in each example doc field that is not stored with TermVector support |
| **MinimumWordLength** | `int?` | Ignores words less than this length or, if 0, then this has no effect |
| **MaximumWordLength** | `int?` | Ignores words greater than this length or if 0 then this has no effect |
| **MinimumDocumentFrequency** | `int?` | Ignores words which do not occur in at least this many documents |
| **MaximumDocumentFrequency** | `int?` | Ignores words which occur in more than this many documents |
| **MaximumDocumentFrequencyPercentage** | `int?` | Ignores words which occur in more than this percentage of documents |
| **Boost** | `bool?` | Boost terms in query based on score |
| **BoostFactor** | `float?` |  Boost factor when boosting based on score |
| **StopWordsDocumentId** | `string` | Document ID containing custom stop words |
| **Fields** | `string[]` | Fields to compare |

{PANEL/}

{PANEL: Stop Words}

Some Lucene analyzers have a built-in list of common English words that are usually not useful 
for searching, like "a", "as", "the", etc.  
These words, called *stop words*, are considered uninteresting and are ignored.  
If a used analyzer does not support *stop words*, or you need to overload these terms, you can 
specify your own set of stop words.  
A document with a list of stop words can be stored in RavenDB by storing the `MoreLikeThisStopWords` document:

{CODE more_like_this_3@Indexes\Querying\MoreLikeThis.cs /}

The document ID will then be set in the `MoreLikeThisOptions`.

{PANEL/}

{PANEL: Remarks}

{INFO: }
Please note that default values for settings, like `MinimumDocumentFrequency`, `MinimumTermFrequency`, 
and `MinimumWordLength`, may result in filtering out related articles, especially with a small data set 
(e.g. during development). 
{INFO/}

{PANEL/}

## Related Articles

### Client API

- [How to Use MoreLikeThis](../../client-api/session/querying/how-to-use-morelikethis)

### Additional Resources

- [Aaron Johnson blog post](http://cephas.net/blog/2008/03/30/how-morelikethis-works-in-lucene/)
