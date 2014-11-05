# Analyzers

The indexes each RavenDB server instance uses to facilitate fast queries are powered by [**Lucene**](http://lucene.apache.org/), the full-text search engine.

Lucene takes a **Document** , breaks it down into **fields** , and then splits all the text in a **Field** into tokens (**Terms**) in a process called **Tokenization**. Those tokens are what will be stored in the index, and later will be searched upon.

After a successful indexing operation, RavenDB feeds Lucene with each entity from the results as a **Document**, and marks every property in it as a **Field** . Then every property is going through the **Tokenization** process using an object called a "Lucene Analyzer", and then finally is stored into the index.

This process and its results can be controlled by various field options and Analyzers, as explained below.

## Understanding Analyzers

Lucene offers several out-of-the-box Analyzers, and the new ones can be created easily. Various analyzers differ in the way they split the text stream ("tokenize"), and in the way they process those tokens post-tokenization.

For example, given this sample text:

`The quick brown fox jumped over the lazy dogs, Bob@hotmail.com 123432.`

* **StandardAnalyzer**, which is Lucene's default, will produce the following tokens:

    `[quick]   [brown]   [fox]   [jumped]   [over]   [lazy]   [dog]   [bob@hotmail.com]   [123432]`

* **StopAnalyzer** will work similarly, but will not perform light stemming, and will only tokenize on white space:

    `[quick]   [brown]   [fox]   [jumped]   [over]   [lazy]   [dogs]   [bob]   [hotmail]   [com]`

* **SimpleAnalyzer**, on the other hand, will tokenize on all non-alpha characters, and will make all the tokens lowercase:

    `[the]   [quick]   [brown]   [fox]   [jumped]   [over]   [the]   [lazy]   [dogs]   [bob]   [hotmail]   [com]`

* **WhitespaceAnalyzer** will just tokenize on white spaces:

    `[The]   [quick]   [brown]   [fox]   [jumped]   [over]   [the]   [lazy]   [dogs,]   [bob@hotmail.com]   [123432.]`

* **KeywordAnalyzer** will perform no tokenization, and will consider the whole text a stream as one token:

    `[The quick brown fox jumped over the lazy dogs, bob@hotmail.com 123432.]`

## RavenDB's default analyzer

By default, RavenDB uses a custom analyzer called `LowerCaseKeywordAnalyzer` for all content. This implementation behaves like Lucene's KeywordAnalyzer, but it also performs case normalization by converting all characters to lower case. 

In other words, by default, RavenDB stores the entire term as a single token, in a lower case form. So given the same sample text from above, `LowerCaseKeywordAnalyzer` will produce a single token looking like this:

`[the quick brown fox jumped over the lazy dogs, bob@hotmail.com 123432.]`

This default behavior allows you to perform exact searches, which is exactly what you would expect. However, this doesn't allow you to perform full-text searches. For that, another analyzer should be used.

## Full-text search

To allow full-text search on the text fields, you can use the analyzers provided with out of the box Lucene. These are available as part of the Lucene distribution, which ships with RavenDB.

For most cases, Lucene's `StandardAnalyzer` would be your analyzer of choice. As shown above, this analyzer is aware of e-mail and network addresses when tokenizing, normalizes cases, filters out common English words, and does some basic English stemming as well.

For languages other than English, or if you need a custom analysis process, you can roll your own `Analyzer`. It is quite simple and may be already available as a contrib package for the Java Lucene or Lucene.NET. There are also `Collation analyzers` available (you can read more about them [here](../indexes/customizing-results-order#collation-support)).

## Using a non-default Analyzer

To make an entity property indexed using a specific Analyzer, all you need to do is to match it with the name of the property, like this:

{CODE-TABS}
{CODE-TAB:csharp:AbstractIndexCreationTask analyzers_1@Indexes\Analyzers.cs /}
{CODE-TAB:csharp:Commands analyzers_2@Indexes\Analyzers.cs /}
{CODE-TABS/}

{INFO The analyzer you are referencing to has to be available to the RavenDB server instance. When using analyzers that do not come with the default Lucene.NET distribution, you need to drop all the necessary DLLs into the "Analyzers" folder of the RavenDB server directory, and use their fully qualified type name (including the assembly name). /}

## Manipulating field indexing behavior

By default each indexed field is analyzed using the `LowerCaseKeywordAnalyzer` which indexes field as a single term in a lower case.

This behavior can be changed, for instance by turning off the field analysis (setting  the`FieldIndexing` option for this field to `NotAnalyzed`). This causes all the properties to be treated as a single token and the matches must be exact, similarly to using the KeywordAnalyzer on this field. The latter is useful for product Ids, for example:

{CODE analyzers_3@Indexes\Analyzers.cs /}

On the other hand, `FieldIndexing.Analyzed` allows performing full text search operations against the field:

{CODE analyzers_4@Indexes\Analyzers.cs /}

If you want to disable indexing on a particular field, use the `FieldIndexing.No` option. This can be useful when you want to [store](../indexes/storing-data-in-index) field data in index and do not make it available for querying, yet make it available for extraction by projections:

{CODE analyzers_5@Indexes\Analyzers.cs /}

## Related articles

- [Server : Analyzer Generators](../server/plugins/analyzer-generators)
- [Boosting](../indexes/boosting)
- [Storing data in index](../indexes/storing-data-in-index)
- [Term Vectors](../indexes/using-term-vectors)
- [Dynamic Fields](../indexes/using-dynamic-fields)
