# Configuring index options

The indexes each RavenDB server instance uses to facilitate fast queries are powered by Lucene, the full-text search engine.

Lucene takes a _Document_ , breaks it down into _Fields_ , and then split all text in a _Field_ into  tokens ( _Terms_ ) in a process called _Tokenization_ . Those tokens are what will be stored in the index, and be later searched upon.

After a successful Map/Reduce operation, RavenDB feeds Lucene with each entity from the results as a _Document_ , and marks every property in it as a _Field_ . Then every property is going through the _Tokenization_ process using an object called a "Lucene Analyzer", and then finaly is stored into the index.

This process and its results can be controlled by using various field options and Analyzers, as explained below.

## Configuring the analysis process

### Understanding Analyzers

Lucene offers several Analyzers out-of-the-box, and new ones can be easily made. Different analyzers differ in the way they split the text stream ("tokenize"), and in the way they process those tokens post-tokenization.

RavenDB uses Lucene's `StandardAlayzer` as it's default Analyzer. This analyzer is aware of e-mail and network addresses when tokenizing, normalizes case, filters out common English words, and also does some basic English stemming. Other available Analyzers behave a bit differently, as shown below

Given this sample text:

`The quick brown fox jumped over the lazy dogs, bob@hotmail.com 123432.`

* **StandardAnalyzer** will produce the following tokens:

    `[quick]   [brown]   [fox]   [jumped]   [over]   [lazy]   [dog]   [bob@hotmail.com]   [123432]`

* **StopAnalyzer** will work the same, but will not perform light stemming, and will only tokenize on white space:

    `[quick]   [brown]   [fox]   [jumped]   [over]   [lazy]   [dogs]   [bob]   [hotmail]   [com]`

* **SimpleAnalyzer** on the other hand will tokenize on all non-space characters, and will only make all the tokens lowercase:

    `[the]   [quick]   [brown]   [fox]   [jumped]   [over]   [the]   [lazy]   [dogs]   [bob]   [hotmail]   [com]`

* **WhitespaceAnalyzer** will just tokenize on white spaces:

    `[The]   [quick]   [brown]   [fox]   [jumped]   [over]   [the]   [lazy]   [dogs,]   [bob@hotmail.com]   [123432.]`

* **KeywordAnalyzer** will perform no tokenization, and will consider the whole text stream as one token:

    `[The quick brown fox jumped over the lazy dogs, bob@hotmail.com 123432.]`

### Using a non-default Analyzer

It's sometimes useful to use a non-default analyzer, for example to improve full-text search of long text fields, especially when they are in a different language than English.

To make an entity property indexed using a specific Analyzer, all you need to do is match it with the name of the property, like so:

{CODE analyzers1@Consumer\StaticIndexes.cs /}

The Analyzer you are referencing to has to be available to the RavenDB server instance. When using analyzers that do not come with the default Lucene.NET distribution, you need to drop all the necessary DLLs into the "Analyzers" folder of the RavenDB server directory, and use their fully qualified type name (including the assembly name).

## Field options

After the tokenization and analysis process is complete, the resulting tokens are stored in an index, which is now ready to be search with. As we have seen before, only fields in the Map/Reduce projections could be used for searched, and the actual tokens stored for each depends on how the selected Analyzer processed the original text.

Lucene allows storing the original token text for fields, and RavenDB exposes this feature in the index definition object via `Stores`.

By default, tokens are saved to the index as Indexed and Analyzed but not Stored - that is: they can be searched on using a specific Analyzer (or the default one), but their original value is unavailable after indexing. Enabling field storage causes values to be available to be retrieved via `IDocumentQuery<T>.SelectFields<TProjection>(...)`, and is done like so:

{CODE stores1@Consumer\StaticIndexes.cs /}

The default values for each field are `FieldStorage.No` in Stores and `FieldIndexing.Analyzed` in Indexes.

Setting `FieldIndexing.No` causes values to not be available in where clauses when querying (similarly to not being present in the original projection). `FieldIndexing.NotAnalyzed` causes whole properties to be treated as a single token and matches must be exact, similarly to using a KeywordAnalyzer on this field. The latter is useful for product Ids, for example.