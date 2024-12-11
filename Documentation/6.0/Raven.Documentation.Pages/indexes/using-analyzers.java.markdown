# Indexes: Analyzers

RavenDB uses indexes to facilitate fast queries powered by [**Lucene**](http://lucene.apache.org/), the full-text search engine.

The indexing of a single document starts from creating Lucene's **Document** according an index definition. Lucene processes it by breaking it into **fields** and splitting all the text
from each **Field** into tokens (**Terms**) in a process called **Tokenization**. Those tokens will be stored in the index, and later will be searched upon.
The **Tokenization** process uses an object called an Analyzer underneath.

The indexing process and its results can be controlled by various field options and Analyzers.

## Understanding Analyzers

Lucene offers several out of the box Analyzers, and the new ones can be created easily. Various analyzers differ in the way they split the text stream ("tokenize"), and in the way they process those tokens in post-tokenization.

For example, given this sample text:

`The quick brown fox jumped over the lazy dogs, Bob@hotmail.com 123432.`

* **StandardAnalyzer**, which is Lucene's default, will produce the following tokens:

    `[quick]   [brown]   [fox]   [jumped]   [over]   [lazy]   [dog]   [bob@hotmail.com]   [123432]`

* **StopAnalyzer** will work similarly, but will not perform light stemming and will only tokenize on white space:

    `[quick]   [brown]   [fox]   [jumped]   [over]   [lazy]   [dogs]   [bob]   [hotmail]   [com]`

* **SimpleAnalyzer** will tokenize on all non-alpha characters and will make all the tokens lowercase:

    `[the]   [quick]   [brown]   [fox]   [jumped]   [over]   [the]   [lazy]   [dogs]   [bob]   [hotmail]   [com]`

* **WhitespaceAnalyzer** will just tokenize on white spaces:

    `[The]   [quick]   [brown]   [fox]   [jumped]   [over]   [the]   [lazy]   [dogs,]   [bob@hotmail.com]   [123432.]`

* **KeywordAnalyzer** will perform no tokenization, and will consider the whole text a stream as one token:

    `[The quick brown fox jumped over the lazy dogs, bob@hotmail.com 123432.]`

* **NGramAnalyzer** will tokenize on pre define token lengths, 2-6 chars long, which are defined by `Indexing.Analyzers.NGram.MinGram` and `Indexing.Analyzers.NGram.MaxGram` configuration options:  
  
   `[.c]  [.co]  [.com]  [12]  [123]  [1234]  [12343]  [123432]  [23]  [234]  [2343]  [23432]  [32]  [34]  [343]  [3432]  [43]  [432]  [@h]  [@ho]  [@hot]  [@hotm]  [@hotma]  [ai]  [ail]  [ail.]  [ail.c]  [ail.co]  [az]  [azy]  [b@]  [b@h]  [b@ho]  [b@hot]  [b@hotm]  [bo]  [bob]  [bob@]  [bob@h]  [bob@ho]  [br]  [bro]  [brow]  [brown]  [ck]  [co]  [com]  [do]  [dog]  [dogs]  [ed]  [er]  [fo]  [fox]  [gs]  [ho]  [hot]  [hotm]  [hotma]  [hotmai]  [ic]  [ick]  [il]  [il.]  [il.c]  [il.co]  [il.com]  [ju]  [jum]  [jump]  [jumpe]  [jumped]  [l.]  [l.c]  [l.co]  [l.com]  [la]  [laz]  [lazy]  [ma]  [mai]  [mail]  [mail.]  [mail.c]  [mp]  [mpe]  [mped]  [ob]  [ob@]  [ob@h]  [ob@ho]  [ob@hot]  [og]  [ogs]  [om]  [ot]  [otm]  [otma]  [otmai]  [otmail]  [ov]  [ove]  [over]  [ow]  [own]  [ox]  [pe]  [ped]  [qu]  [qui]  [quic]  [quick]  [ro]  [row]  [rown]  [tm]  [tma]  [tmai]  [tmail]  [tmail.]  [ui]  [uic]  [uick]  [um]  [ump]  [umpe]  [umped]  [ve]  [ver]  [wn]  [zy]`  
   You can override NGram analyzer default token lengths by configuring `Indexing.Analyzers.NGram.MinGram` and `Indexing.Analyzers.NGram.MaxGram` per index e.g. setting them to 3 and 4 accordingly will generate:  
   `[.co]  [.com]  [123]  [1234]  [234]  [2343]  [343]  [3432]  [432]  [@ho]  [@hot]  [ail]  [ail.]  [azy]  [b@h]  [b@ho]  [bob]  [bob@]  [bro]  [brow]  [com]  [dog]  [dogs]  [fox]  [hot]  [hotm]  [ick]  [il.]  [il.c]  [jum]  [jump]  [l.c]  [l.co]  [laz]  [lazy]  [mai]  [mail]  [mpe]  [mped]  [ob@]  [ob@h]  [ogs]  [otm]  [otma]  [ove]  [over]  [own]  [ped]  [qui]  [quic]  [row]  [rown]  [tma]  [tmai]  [uic]  [uick]  [ump]  [umpe]  [ver]  `  

## RavenDB Default Analyzer

By default, RavenDB uses the custom analyzer called `LowerCaseKeywordAnalyzer` for all indexed content. Its implementation behaves like Lucene's KeywordAnalyzer, but it also performs case normalization by converting all characters to lower case. 

RavenDB stores the entire term as a single token, in a lower cased form. Given the same sample above text, `LowerCaseKeywordAnalyzer` will produce a single token:

`[the quick brown fox jumped over the lazy dogs, bob@hotmail.com 123432.]`

This default analyzer allows you to perform exact searches which is exactly what you would expect. However, it doesn't allow you to perform full-text searches. For that purposes, a different analyzer should be used.

## Full-Text Search

To allow full-text search on the text fields, you can use the analyzers provided out of the box with Lucene. These are available as part of the Lucene library which ships with RavenDB.

For most cases, Lucene's `StandardAnalyzer` would be your analyzer of choice. As shown above, this analyzer is aware of e-mail and network addresses when tokenizing. It normalizes cases, filters out common English words, and does some basic English stemming as well.

For languages other than English, or if you need a custom analysis process, you can roll your own `Analyzer`. It is quite simple and may be already available as a contrib package for Lucene. 
There are also `Collation analyzers` available (you can read more about them [here](../indexes/sorting-and-collation#collation)).

## Using Non-Default Analyzer

To make a document property indexed using a specific Analyzer, all you need to do is to match it with the name of the property:

{CODE-TABS}
{CODE-TAB:java:AbstractIndexCreationTask analyzers_1@Indexes\Analyzers.java /}
{CODE-TAB:java:Operation analyzers_2@Indexes\Analyzers.java /}
{CODE-TABS/}

{INFO The analyzer you are referencing to has to be available to the RavenDB server instance. When using analyzers that do not come with the default Lucene.NET distribution, you need to drop all the necessary DLLs into the RavenDB working directory (where `Raven.Server` executable is located), and use their fully qualified type name (including the assembly name). /}

## Creating Own Analyzer

You can create a custom analyzer on your own and deploy it to RavenDB server. To do that pefrom the following steps:

- create a class that inherits from abstract `Lucene.Net.Analysis.Analyzer` (you need to reference `Lucene.Net.dll` supplied with RavenDB Server package),
- your DLL needs to be placed next to RavenDB binaries (note it needs to be compatible with .NET Core 2.0 e.g. .NET Standard 2.0 assembly)
- the fully qualified name needs to be specified for an indexing field that is going to be tokenized by the analyzer

{CODE my_custom_analyzer@Indexes\Analyzers.cs /}

## Manipulating Field Indexing Behavior

By default, each indexed field is analyzed using the `LowerCaseKeywordAnalyzer` which indexes a field as a single, lower cased term.

This behavior can be changed by turning off the field analysis (setting the `FieldIndexing` option for this field to `Exact`). This causes all the properties to be treated as a single token and the matches must be exact (case sensitive), similarly to using the `KeywordAnalyzer` on this field.

{CODE:java analyzers_3@Indexes\Analyzers.java /}

`FieldIndexing.SEARCH` allows performing full text search operations against the field:

{CODE:java analyzers_4@Indexes\Analyzers.java /}

If you want to disable indexing on a particular field, use the `FieldIndexing.NO` option. This can be useful when you want to [store](../indexes/storing-data-in-index) field data in the index, but don't want to make it available for querying, however it will available for extraction by projections:

{CODE:java analyzers_5@Indexes\Analyzers.java /}

## Ordering When Field is Searchable

When field is marked as `SEARCH` sorting must be done using additional field. More [here](../indexes/querying/sorting#ordering-when-a-field-is-searchable).

## Related Articles

### Indexes

- [Boosting](../indexes/boosting)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Dynamic Fields](../indexes/using-dynamic-fields)
