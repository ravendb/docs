# More Like This Bundle

More Like This returns a list of similar documents that are related to a given document. This feature can be used, for example, when viewing an article many news sites show a list of related articles at the bottom of the page. To accomplish this, the RavenDB More Like This uses the More Like This from Lucene contrib project. To find out how this more about the algorithm Aaron Johnson has an [excellent blog](http://cephas.net/blog/2008/03/30/how-morelikethis-works-in-lucene/) post describing it.

## Installation

Place the Raven.Bundles.MoreLikeThis.dll file in the server's Plugins directory, and  reference Raven.Client.MoreLikeThis in your project to add MoreLikeThis support to the Client API.

## Usage
### Setup

In order for More Like This to work it must have access to the text in the index, therefore, the index being queried needs to store the fields.

{CODE MoreLikeThis1@Server\Bundles.cs /}

### Basic Usage

More Like This has many defaults already set, so the simplest usage will satisfy most usage scenarios.

{CODE-START:csharp /}
var list = session.Advanced.MoreLikeThis<Article, ArticleIndex>(key);
{CODE-END /}

More Like This will use all the fields defined in an index. To use only a specific field or fields, pass them in as the second parameter.

{CODE-START:csharp /}
var list = session.Advanced.MoreLikeThis<Article, ArticleIndex>(key, “ArticleBody”);
{CODE-END /}

### Advanced Usage

By passing in an object of type MoreLikeThisQueryParameters the More Like This default can be changed.

+ **Boost** - Boost terms in query based on score. Using this option may give more exact results by boosting the terms found by their score. The default is false.
+ **MaximumNumberOfTokensParsed** - The maximum number of tokens to parse in each example document field that is not stored with TermVector support. The default is 5000.
+ **MaximumWordLength** - Ignore words greater than this length or if 0 then this has no effect. The default is 0.
+ **MaximumQueryTerms** - Return a Query with no more than this many terms. The default is 25.
+ **MinimumDocumentFrequency** - Ignore words which do not occur in at least this many documents. The default is 5.
+ **MinimumTermFrequency** - Ignore terms with less than this frequency in the source doc. The default is 2.
+ **MinimumWordLength** - Ignore words less than this length or if 0 then this has no effect. The default is 0.
+ **StopWordsDocumentId** - See below.

### Stop Words

A document with a list of stop words can be stored in RavenDB. For example:

{CODE-START:csharp /}
session.Store(new StopWordsSetup{ Id = "Config/Stopwords", StopWords = new List<string> { "I", "A", "Be" }});
{CODE-END /}

The document id is then set in MoreLikeThisQueryParameters