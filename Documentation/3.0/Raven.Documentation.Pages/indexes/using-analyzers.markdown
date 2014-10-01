# Analyzers

The indexes each RavenDB server instance uses to facilitate fast queries are powered by [**Lucene**](http://lucene.apache.org/), the full-text search engine.

Lucene takes a **Document** , breaks it down into **fields** , and then split all text in a **Field** into tokens (**Terms**) in a process called **Tokenization**. Those tokens are what will be stored in the index, and be later searched upon.

After a successful indexing operation, RavenDB feeds Lucene with each entity from the results as a **Document**, and marks every property in it as a **Field** . Then every property is going through the **Tokenization** process using an object called a "Lucene Analyzer", and then finally is stored into the index.

This process and its results can be controlled by using various field options and Analyzers, as explained below.

## Understanding Analyzers

Lucene offers several Analyzers out-of-the-box, and new ones can be made easily. Different analyzers differ in the way they split the text stream ("tokenize"), and in the way they process those tokens post-tokenization.

For example, given this sample text:

`The quick brown fox jumped over the lazy dogs, Bob@hotmail.com 123432.`

* **StandardAnalyzer**, which is Lucene's default, will produce the following tokens:

    `[quick]   [brown]   [fox]   [jumped]   [over]   [lazy]   [dog]   [bob@hotmail.com]   [123432]`

* **StopAnalyzer** will work the same, but will not perform light stemming, and will only tokenize on white space:

    `[quick]   [brown]   [fox]   [jumped]   [over]   [lazy]   [dogs]   [bob]   [hotmail]   [com]`

* **SimpleAnalyzer** on the other hand will tokenize on all non-alpha characters, and will make all the tokens lowercase:

    `[the]   [quick]   [brown]   [fox]   [jumped]   [over]   [the]   [lazy]   [dogs]   [bob]   [hotmail]   [com]`

* **WhitespaceAnalyzer** will just tokenize on white spaces:

    `[The]   [quick]   [brown]   [fox]   [jumped]   [over]   [the]   [lazy]   [dogs,]   [bob@hotmail.com]   [123432.]`

* **KeywordAnalyzer** will perform no tokenization, and will consider the whole text stream as one token:

    `[The quick brown fox jumped over the lazy dogs, bob@hotmail.com 123432.]`

## RavenDB's default analyzer

By default, RavenDB uses a custom analyzer called `LowerCaseKeywordAnalyzer` for all content. This implementation behaves like Lucene's KeywordAnalyzer, but it also perform case normalization by converting all characters to lower case. 

In other words, by default RavenDB stores the entire term as a single token, in a lower case form. So given the same sample text from above, `LowerCaseKeywordAnalyzer` will produce a single token looking like this:

`[the quick brown fox jumped over the lazy dogs, bob@hotmail.com 123432.]`

This default behavior allows you to perform exact searches, which is exactly what you would expect. However, this doesn't allow you to perform full-text searches. For that, another analyzer should be used.

## Full-text search

To allow for full-text search on text fields, you can use the analyzers provided with Lucene out of the box. These are available as part of the Lucene distribution that ships with RavenDB.

For most cases, Lucene's `StandardAnalyzer` would be your analyzer of choice. As shown above, this analyzer is aware of e-mail and network addresses when tokenizing, normalizes case, filters out common English words, and also does some basic English stemming.

For languages other than English, or if you need a custom analysis process, you can roll your own `Analyzer`. It is quite simple to do, and may already be available as a contrib package for Java Lucene or Lucene.NET. There are also `Collation analyzers` available and you can read more about them [here](../indexes/customizing-results-order#collation-support).

## Using a non-default Analyzer

To make an entity property indexed using a specific Analyzer, all you need to do is match it with the name of the property, like so:

{CODE-TABS}
{CODE-TAB:csharp:AbstractIndexCreationTask analyzers_1@Indexes\Analyzers.cs /}
{CODE-TAB:csharp:Commands analyzers_2@Indexes\Analyzers.cs /}
{CODE-TABS/}

{INFO The analyzer you are referencing to has to be available to the RavenDB server instance. When using analyzers that do not come with the default Lucene.NET distribution, you need to drop all the necessary DLLs into the "Analyzers" folder of the RavenDB server directory, and use their fully qualified type name (including the assembly name). /}

## Manipulating field indexing behavior

By default each indexed field is analyzed using `LowerCaseKeywordAnalyzer` which index field as a single term in lower case.

This behavior can be changed, for instance field analysis can be turned off, by setting `FieldIndexing` option for this field to `NotAnalyzed`. This causes whole properties to be treated as a single token and matches must be exact, similarly to using a KeywordAnalyzer on this field. The latter is useful for product Ids, for example:

{CODE analyzers_3@Indexes\Analyzers.cs /}

On the other hand, `FieldIndexing.Analyzed` allows to perform full text search operations against the field:

{CODE analyzers_4@Indexes\Analyzers.cs /}

If you want to disable indexing on a particular field, use `FieldIndexing.No` option. This can be useful when you want to [store](../indexes/storing-data-in-index) field data in index and do not make it available for querying, but make it available for extraction by projections:

{CODE analyzers_5@Indexes\Analyzers.cs /}

## Related articles

- [Server : Analyzer Generators](../server/plugins/analyzer-generators)
- [Boosting](../indexes/boosting)
- [Storing data in index](../indexes/storing-data-in-index)
- [Term Vectors](../indexes/using-term-vectors)
- [Dynamic Fields](../indexes/using-dynamic-fields)
