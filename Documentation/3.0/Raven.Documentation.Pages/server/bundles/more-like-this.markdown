# Bundle : MoreLikeThis

More Like This returns a list of similar documents that are related to a given document. This feature can be used, for example, when viewing an article. Many news sites show a list of related articles at the bottom of the page. To accomplish this, the RavenDB More Like This uses the More Like This from Lucene contrib project. To find out more about the algorithm, please read Aaron Johnson excellent blog post that is available [here](http://cephas.net/blog/2008/03/30/how-morelikethis-works-in-lucene/).

## Installation

This bundle is built-in into server and client and do not need any installation. All extensions are available under `Raven.Client.Bundles.MoreLikeThis` namespace.

## Setup

In order for More Like This to work it must have access to the text in the index, therefore, the index being queried needs to [store]() the fields or store [term vectors]() for those fields.

{CODE more_like_this_4@Server\Bundles\MoreLikeThis.cs /}

## Basic Usage

More Like This has many defaults already set, so the simplest usage will satisfy most usage scenarios.

{CODE more_like_this_1@Server\Bundles\MoreLikeThis.cs /}

More Like This will use all the fields defined in an index. To use only a specific field or fields, pass them in as the second parameter.

{CODE more_like_this_2@Server\Bundles\MoreLikeThis.cs /}

## Advanced Usage

By passing in an object of type MoreLikeThisQuery the More Like This default can be changed.

+ **Fields** - Limit the fields that we search on to just the specified field names.
+ **Boost** - Boost terms in query based on score. Using this option may give more exact results by boosting the terms found by their score. The default is false.
+ **BoostFactor** - Factor that is used during term boosting. Default: 1.   
+ **IndexName** - Name of an index to use in operation.   
+ **DocumentId** - The id of a document that will be used as the basis for comparison.   
+ **MapGroupFields** - The values for the mapping group fields that will be used as the basis for comparison.   
+ **MaximumNumberOfTokensParsed** - The maximum number of tokens to parse in each example document field that is not stored with TermVector support. The default is 5000.
+ **MaximumWordLength** - Ignore words greater than this length or if 0 then this has no effect. The default is 0.
+ **MaximumQueryTerms** - Return a Query with no more than this many terms. The default is 25.
+ **MinimumDocumentFrequency** - Ignore words which do not occur in at least this many documents. The default is 5.
+ **MaximumDocumentFrequency** - Ignore words which occur in more than this many documents. Default is Int32.MaxValue.
+ **MaximumDocumentFrequencyPercentage** - Ignore words which occur in more than this percentage of documents.
+ **MinimumTermFrequency** - Ignore terms with less than this frequency in the source doc. The default is 2.
+ **MinimumWordLength** - Ignore words less than this length or if 0 then this has no effect. The default is 0.
+ **StopWordsDocumentId** - the document ID that contains the set of stop words (see below).
+ **ResultsTransformer** - name of a transformer that will be used.
+ **TransformerParameters** - parameters that will be passed to transformer.

## Stop Words

Some of Lucene analyzers have a built-in list of common English words that are usually not useful for searching (like "a", "as", "the" etc.). Those words are called 
*stop words* and they are considered to be uninteresting and ignored. If a used analyzer does not support stop words or you need to overload them you can specify your own.
A document with the list of stop words can be stored in RavenDB by storing `StopWordsSetup` document:

{CODE more_like_this_3@Server\Bundles\MoreLikeThis.cs /}

The document ID is then set in `MoreLikeThisQueryParameters`.

## Remarks

{INFO Please note that default values for settings like `MinimumDocumentFrequency`, `MinimumTermFrequency` and `MinimumWordLength` may result in filtering out related articles, especially when data set is small (e.g. during development). /}

## Related articles

TODO