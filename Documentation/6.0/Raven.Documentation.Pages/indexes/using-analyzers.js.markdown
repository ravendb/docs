# Indexes: Analyzers
---

{NOTE: }

* RavenDB supports fast and efficient querying through indexes,
  which are powered by either [Lucene](http://lucene.apache.org/) or [Corax](../indexes/search-engine/corax),  
  a high-performance search engine developed specifically for RavenDB.  
  (You can choose which search engine to use for each index).

* **Analyzers** are components used in the indexing and querying processes of the search engines,   
  controlling how data is indexed and how search queries interact with the indexed data.

* **The Corax search engine fully respects and supports all Lucene analyzers**,  
  ensuring that existing configurations work seamlessly,  
  while also leveraging Corax's optimized performance for faster query execution.

* This means you can use any analyzer with either search engine,  
  giving you full flexibility in configuring your indexes.

* In this page:
    * [Understanding the role of analyzers](../indexes/using-analyzers#understanding-the-role-of-analyzers)
    * [Analyzers available in RavenDB](../indexes/using-analyzers#analyzers-available-in-ravendb)
    * [Setting analyzer for index-field](../indexes/using-analyzers#setting-analyzer-for-index-field)
    * [RavenDB's default analyzers](../indexes/using-analyzers#ravendb)
    * [Disabling indexing for index-field](../indexes/using-analyzers#disabling-indexing-for-index-field)
    * [Creating custom analyzers](../indexes/using-analyzers#creating-custom-analyzers)
    * [Viewing the indexed terms](../indexes/using-analyzers#viewing-the-indexed-terms)

{NOTE/}

---

{PANEL: Understanding the role of analyzers}

{CONTENT-FRAME: }

###### Analyzers in the index definition:
---

The [index definition](../studio/database/indexes/indexes-overview#index-definition) determines what content from the documents will be indexed for each index-field.  
For each index-field you can specify a particular analyzer to process the content of that field.

{CONTENT-FRAME/}
{CONTENT-FRAME: }

###### Analyzers at indexing time:
---

During the [indexing process](../studio/database/indexes/indexes-overview#indexing-process),
the content to be indexed is processed and broken down into smaller components called tokens (or terms) through a process known as **tokenization**.  
This is done by the **Analyzers**, which are objects that determine how text is split into tokens.

Different analyzers vary in how they split the text stream ("tokenize"), and how they process those tokens after tokenization.  
Analyzers can apply additional transformations, such as converting text to lowercase, removing stop words  
(e.g., "the," "and"), or applying stemming (reducing words to their base forms, e.g., "running" → "run").

The resulting tokens are then stored in the index for each index-field and can later be searched by queries,  
enabling [Full-text search](../indexes/querying/searching).

{CONTENT-FRAME/}
{CONTENT-FRAME: }

###### Analyzers at query time:
---

When running a [Full-text search with a dynamic query](../client-api/session/querying/text-search/full-text-search),  
the auto-index created by the server breaks down the text of the searched document field using the [default search analyzer](../indexes/using-analyzers#using-the-default-search-analyzer).

When running a [Full-text search on a static-index](../indexes/querying/query-index),  
the **same analyzer** used to tokenize field content at indexing time is typically applied
to process the terms provided in the full-text search query before they are sent to the search engine to retrieve matching documents.

There are two exceptions to this rule:

1. When setting the [NGramAnalyzer](../indexes/using-analyzers#analyzers-that-tokenize-according-to-the-defined-number-of-characters) in the index definition,
   it tokenizes the index field at indexing time.  
   However, at query time, when performing a full-text search on that field,  
   the default [RavenStandardAnalyzer](../indexes/using-analyzers#using-the-default-search-analyzer) is used to tokenize the search term from the query predicate.

   Currently, for query time, you cannot specify a different analyzer than the one defined in the index definition,  
   so to address this issue, you have two options:
    * Increase the [MaxGram](../server/configuration/indexing-configuration#indexing.lucene.analyzers.ngram.maxgram) value to generate larger tokens during indexing (when using Lucene).
    * Use a different analyzer other than _NGramAnalyzer_ that better matches your requirements.

2. Behavior is also different when making a full-text search with wildcards in the search terms.  
   This is explained in detail in [Searching with wildcards](../indexes/querying/searching#searching-with-wildcards).

{CONTENT-FRAME/}
{CONTENT-FRAME: }

###### Full-text search:
---

In most cases, Lucene's [StandardAnalyzer](../indexes/using-analyzers#analyzers-that-remove-common-stop-words) is sufficient for full-text searches.  
For languages other than English, or when a custom analysis process is needed, you can provide your own [Custom analyzer](../indexes/using-analyzers#creating-custom-analyzers).  
It is straightforward and may already be available as a contrib package for Lucene.

You can also configure a specific collation for an index field to sort based on culture specific rules.  
Learn more in [Sorting and Collation](../indexes/sorting-and-collation#collation).

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Analyzers available in RavenDB}

* RavenDB offers the following Lucene analyzers 'out of the box' (their details are listed below):

    * **StandardAnalyzer**
    * **StopAnalyzer**
    * **SimpleAnalyzer**
    * **WhitespaceAnalyzer**
    * **LowerCaseWhitespaceAnalyzer**
    * **KeywordAnalyzer**
    * **NGramAnalyzer**

* If needed, you can create your own [Customized Analyzers](../indexes/using-analyzers#creating-custom-analyzers).

* To assign the analyzer of your choice to a specific index-field,
  see: [Setting analyzer for index-field](../indexes/using-analyzers#setting-analyzer-for-index-field).

* When no analyzer is explicitly assigned to an index-field in the index definition,  
  RavenDB will use its [Default Analyzers](../indexes/using-analyzers#ravendb) to process and tokenize the content of a field.

---

{INFO: }
All examples below use the following text:  
`The quick brown fox jumped over the lazy dogs, Bob@hotmail.com 123432.`  
{INFO/}

---

{NOTE: }

##### Analyzers that remove common "stop words":

---

{CONTENT-FRAME: }

* **StandardAnalyzer**, which is Lucene's default, will produce the following tokens:

    `[quick] [brown] [fox] [jumped] [over] [lazy] [dogs] [bob@hotmail.com] [123432]`

    This analyzer:

    * Removes common "stop words".
    * Converts text to lowercase, ensuring searches are case-insensitive.
    * Separates on whitespace and punctuation that is followed by whitespace - a dot that is not followed by whitespace is considered part of the token.
    * Email addresses and internet hostnames are treated as a single token.
    * Splits words at hyphens, unless there's a number in the token, in which case the whole token is interpreted as a product number and is not split.

{CONTENT-FRAME/}
{CONTENT-FRAME: }

* **StopAnalyzer**, which works similarly, will produce the following tokens:

    `[quick] [brown] [fox] [jumped] [over] [lazy] [dogs] [bob] [hotmail] [com]`

    This analyzer:

    * Removes common "stop words".
    * Converts text to lowercase, ensuring searches are case-insensitive.
    * Separates and tokenizes text based on whitespace without performing light stemming.
    * Removes numbers and symbols, separating tokens at those positions.  
      This means email and web addresses are split into separate tokens.

{CONTENT-FRAME/}

{INFO: }

* **Stop words**:

    * [Stop words](https://en.wikipedia.org/wiki/Stop_word) (e.g. the, it, a, is, this, who, that...)  
      are often removed to narrow search results by focusing on less frequently used words.
    * If you want to include words such as IT (Information Technology),
      be aware that analyzers removing common stop words may treat IT as a stop word and exclude it from the resulting terms.
      This can also affect acronyms such as WHO (World Health Organization) or names such as "The Who" or "The IT Crowd".
    * To avoid excluding acronyms, you can either spell out the full title instead of abbreviating it
      or use an [Analyzer that doesn't remove stop words](../indexes/using-analyzers#analyzers-that-do-not-remove-common-stop-words).

{INFO/}

{NOTE/}
{NOTE: }

##### Analyzers that do not remove common "stop words"

---

{CONTENT-FRAME: }

* **SimpleAnalyzer** will produce the following tokens:

    `[the] [quick] [brown] [fox] [jumped] [over] [lazy] [dogs] [bob] [hotmail] [com]`

    This analyzer:

    * Includes common "stop words".
    * Converts text to lowercase, ensuring searches are case-insensitive.
    * Separates on white spaces.
    * Will tokenize on all non-alpha characters.
    * Removes numbers and symbols, separating tokens at those positions.  
      This means email and web addresses are split into separate tokens.

{CONTENT-FRAME/}
{CONTENT-FRAME: }

* **WhitespaceAnalyzer** will produce the following tokens:

    `[The] [quick] [brown] [fox] [jumped] [over] [the] [lazy] [dogs,] [Bob@hotmail.com] [123432.]`
  
    This analyzer:
  
    * Includes common "stop words".
    * Tokenizes text by separating it on whitespaces.
    * Preserves upper/lower case in text, which means that searches will be case-sensitive.
    * Keeps forms like email addresses, phone numbers, and web addresses whole.

{CONTENT-FRAME/}
{CONTENT-FRAME: }

* **LowerCaseWhitespaceAnalyzer** will produce the following tokens:

    `[the] [quick] [brown] [fox] [jumped] [over] [lazy] [dogs,] [bob@hotmail.com] [123432.]`
  
    This analyzer:
  
    * Includes common "stop words".
    * Tokenizes text by separating it on whitespaces.
    * Converts text to lowercase, ensuring searches are case-insensitive.
    * Keeps forms like email addresses, phone numbers, and web addresses whole.

{CONTENT-FRAME/}
{CONTENT-FRAME: }

* **KeywordAnalyzer** will produce the following single token:

    `[The quick brown fox jumped over the lazy dogs, bob@hotmail.com 123432.]`

    This analyzer:

    * Will perform no tokenization, and will consider the whole text as one token.
    * Preserves upper/lower case in text, which means that searches will be case-sensitive.
    * Useful in situations like IDs and codes where you do not want to separate into multiple tokens.

{CONTENT-FRAME/}
{NOTE/}
{NOTE: }

##### Analyzers that tokenize according to the defined number of characters

---

{CONTENT-FRAME: }

* **NGramAnalyzer** tokenizes based on predefined token lengths.

    By default, the minimum token length is **2** characters, and the maximum is **6** characters.  
    Using these defaults, the following tokens will be generated:

    `[.c] [.co] [.com] [12] [123] [1234] [12343] [123432] [23] [234] [2343] [23432]  
     [32] [34] [343] [3432] [43] [432] [@h] [@ho] [@hot] [@hotm] [@hotma]  
     [ai] [ail] [ail.] [ail.c] [ail.co] [az] [azy] [b@] [b@h] [b@ho] [b@hot] [b@hotm]  
     [bo] [bob] [bob@] [bob@h] [bob@ho] [br] [bro] [brow] [brown] [ck] [co] [com]  
     [do] [dog] [dogs] [ed] [er] [fo] [fox] [gs] [ho] [hot] [hotm] [hotma] [hotmai]  
     [ic] [ick] [il] [il.] [il.c] [il.co] [il.com] [ju] [jum] [jump] [jumpe] [jumped]  
     [l.] [l.c] [l.co] [l.com] [la] [laz] [lazy] [ma] [mai] [mail] [mail.] [mail.c]  
     [mp] [mpe] [mped] [ob] [ob@] [ob@h] [ob@ho] [ob@hot] [og] [ogs] [om] [ot] [otm] 
     [otma] [otmai] [otmail] [ov] [ove] [over] [ow] [own] [ox] [pe] [ped] [qu] [qui] 
     [quic] [quick] [ro] [row] [rown] [tm] [tma] [tmai] [tmail] [tmail.] 
     [ui] [uic] [uick] [um] [ump] [umpe] [umped] [ve] [ver] [wn] [zy]`

* **Overriding default token length**: (only when using Lucene as the search engine)

    You can override the default token lengths of the NGram analyzer by setting the following configuration keys:  
    [Indexing.Lucene.Analyzers.NGram.MinGram](../server/configuration/indexing-configuration#indexing.lucene.analyzers.ngram.mingram)
    and [Indexing.Lucene.Analyzers.NGram.MaxGram](../server/configuration/indexing-configuration#indexing.lucene.analyzers.ngram.maxgram).

    For example, setting them to 3 and 4, respectively, will generate the following tokens:

    `[.co] [.com] [123] [1234] [234] [2343] [343] [3432] [432] [@ho] [@hot]  
     [ail] [ail.] [azy] [b@h] [b@ho] [bob] [bob@] [bro] [brow] [com] [dog] [dogs] [fox]  
     [hot] [hotm] [ick] [il.] [il.c] [jum] [jump] [l.c] [l.co] [laz] [lazy] [mai] [mail]  
     [mpe] [mped] [ob@] [ob@h] [ogs] [otm] [otma] [ove] [over] [own] [ped] [qui] [quic]  
     [row] [rown] [tma] [tmai] [uic] [uick] [ump] [umpe] [ver]`

* **Querying with NGram analyzer**:

    In RavenDB, the analyzer configured in the index definition is typically used both at indexing time and query time (the same analyzer).
    However, the `NGramAnalyzer` is an exception to this rule.
  
    Refer to section [Analyzers at query time](../indexes/using-analyzers#analyzers-at-query-time) to learn about the different behaviors.

{CONTENT-FRAME/}
{NOTE/}
{PANEL/}

{PANEL: Setting analyzer for index-field}

* To explicitly set an analyzer that will process/tokenize the content of a specific index-field,  
  set the `analyze()` method within the index definition for that field.

* Either:
    * Specify an analyzer from the [Analyzers available in RavenDB](../indexes/using-analyzers#analyzers-available-in-ravendb),
    * Or specify your own custom analyzer (see [Creating custom analyzers](../indexes/using-analyzers#creating-custom-analyzers)).

* If you want RavenDB to use the default analyzers, see [RavenDB's default analyzers](../indexes/using-analyzers#ravendb).

* An analyzer may also be set from the Edit Index view in the Studio, see [Index field options](../studio/database/indexes/create-map-index#index-field-options).

{CODE-TABS}
{CODE-TAB:nodejs:AbstractIndexCreationTask setting_analyzers_1@indexes\analyzers.js /}
{CODE-TAB:nodejs:PutIndexesOperation setting_analyzers_2@indexes\analyzers.js /}
{CODE-TABS/}

{PANEL/}

{PANEL: RavenDB's default analyzers}

*  When no specific analyzer is explicitly assigned to an index-field in the index definition,  
   RavenDB will use the Default Analyzers to process and tokenize the content of the field,  
   depending on the specified Indexing Behavior.

* The **Default Analyzers** are:
    * `RavenStandardAnalyzer` - Serves as the [Default Search Analyzer](../indexes/using-analyzers#using-the-default-search-analyzer).
    * `KeywordAnalyzer` - Servers as the [Default Exact Analyzer](../indexes/using-analyzers#using-the-default-exact-analyzer).
    * `LowerCaseKeywordAnalyzer`- Serves as the [Default Analyzer](../indexes/using-analyzers#using-the-default-analyzer).

* The available **Indexing Behavior** values are:
    * `Exact`
    * `Search`
    * `No` - This behavior [disables field indexing](../indexes/using-analyzers#disabling-indexing-for-index-field).

* See the detailed explanation for each scenario below:

---

{NOTE: }

##### Using the Default Search Analyzer

---

* When the indexing behavior is set to `Search` and no analyzer is specified for the index-field,  
  RavenDB will use the Default Search Analyzer called `RavenStandardAnalyzer`.  
  (This analyzer inherits from Lucene's _StandardAnalyzer_).

* Using a search analyzer enables full-text search queries against the field.  
  Given the same sample text from above, _RavenStandardAnalyzer_ will produce the following tokens:  
  `[quick] [brown] [fox] [jumped] [over] [lazy] [dogs] [bob@hotmail.com] [123432]`

  {CODE:nodejs use_search_analyzer@indexes\analyzers.js /}

* To customize a different analyzer that will serve as your Default Search Analyzer,  
  set the [Indexing.Analyzers.Search.Default](../server/configuration/indexing-configuration#indexing.analyzers.search.default) configuration key.

{NOTE/}
{NOTE: }

##### Using the Default Exact Analyzer

---

* When the indexing behavior is set to `Exact`,  
  RavenDB will use the Default Exact Analyzer called `KeywordAnalyzer`.

* This analyzer treats the entire content of the index-field as a single token,
  preserving the original text's case and ensuring no transformations, such as case normalization or stemming, are applied.  
  The field's value is indexed exactly as provided, enabling precise, case-sensitive matching at query time.

* Given the same sample text from above, _KeywordAnalyzer_ will produce a single token:            
  `[The quick brown fox jumped over the lazy dogs, Bob@hotmail.com 123432.]`

  {CODE:nodejs use_exact_analyzer@indexes\analyzers.js /}

* To customize a different analyzer that will serve as your Default Exact Analyzer,  
  set the [Indexing.Analyzers.Exact.Default ](../server/configuration/indexing-configuration#indexing.analyzers.exact.default) configuration key.

{NOTE/}
{NOTE: }

##### Using the Default Analyzer

---

* When no indexing behavior is set and no analyzer is specified for the index-field,  
  RavenDB will use the default custom analyzer called `LowerCaseKeywordAnalyzer`.

* This analyzer behaves like Lucene's _KeywordAnalyzer_, but additionally performs case normalization, converting all characters to lowercase.
  The entire content of the field is processed into a single, lowercased token.

* Given the same sample text from above, _LowerCaseKeywordAnalyzer_ will produce a single token:            
  `[the quick brown fox jumped over the lazy dogs, bob@hotmail.com 123432.]`

  {CODE:nodejs use_default_analyzer@indexes\analyzers.js /}

* To customize a different analyzer that will serve as your Default Analyzer,  
  set the [Indexing.Analyzers.Default](../server/configuration/indexing-configuration#indexing.analyzers.default) configuration key.

{NOTE/}
{PANEL/}

{PANEL: Disabling indexing for index-field}

* Use the `No` indexing behavior option to disable indexing of a particular index-field.  
  In this case:
    * No analyzer will process the field, and no terms will be generated from its content.
    * The field will not be available for querying.
    * The field will still be accessible for extraction when [projecting query results](../indexes/querying/projections).

* This is useful when you need to [store the field data in the index](../indexes/storing-data-in-index) and only intend to use it for query projections.

  {CODE:nodejs no_indexing@indexes\analyzers.js /}

{PANEL/}

{PANEL: Creating custom analyzers}

* **Availability & file type**:  
  The custom analyzer you are referencing must be available to the RavenDB server instance.  
  You can create and add custom analyzers to RavenDB as `.cs` files.

* **Scope**:  
  Custom analyzers can be defined as:

    * **Database Custom Analyzers** - can only be used by indexes of the database where they are defined.
    * **Server-Wide Custom Analyzers** - can be used by indexes on all databases across all servers in the cluster.

      A database analyzer may have the same name as a server-wide analyzer.  
      In this case, the indexes of that database will use the database version of the analyzer.  
      You can think of database analyzers as overriding server-wide analyzers with the same names.

* **Ways to create**:  
  There are three ways to create a custom analyzer and add it to your server:

    1. [Add custom analyzer via Studio](../indexes/using-analyzers#add-custom-analyzer-via-studio)
    2. [Add custom analyzer via Client API](../indexes/using-analyzers#add-custom-analyzer-via-client-api)
    3. [Add custom analyzer directly to RavenDB's binaries](../indexes/using-analyzers#add-custom-analyzer-directly-to-ravendbs-binaries)

---

{NOTE: }

##### Add custom analyzer via Studio

Custom analyzers can be added from the Custom Analyzers view in the Studio.  
Learn more in this [Custom analyzers](../studio/database/settings/custom-analyzers) article.

{NOTE/}
{NOTE: }

##### Add custom analyzer via Client API

First, create a class that inherits from the abstract `Lucene.Net.Analysis.Analyzer` class.  
(you need to reference `Lucene.Net.dll`, which is included with the RavenDB Server package).   
For example:

{CODE:csharp my_custom_analyzer@Indexes\Analyzers.cs /}

Next, use `PutAnalyzersOperation` to deploy the analyzer to a specific database.  
By default, `PutAnalyzersOperation` will apply to the [default database](../client-api/setting-up-default-database) of the document store you're using.  
To target a different database, use the [forDatabase()](../client-api/operations/how-to/switch-operations-to-a-different-database) method.

To make it a server-wide analyzer, use the `PutServerWideOperation` operation.`

{CODE:nodejs put_analyzers_1@indexes\analyzers.js /}
{CODE:nodejs put_analyzers_2@indexes\analyzers.js /}
{CODE:nodejs put_analyzers_3@indexes\analyzers.js /}

| Parameter  | Type     | Description                                                                                                                                                       |
|------------|----------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **name**   | `string` | The class name of your custom analyzer, as defined in your code.                                                                                                  |
| **code**   | `string` | Compilable csharp code:<br>A class that inherits from `Lucene.Net.Analysis.Analyzer`,<br>including the containing namespace and the necessary `using` statements. |

**Client API example**:

{CODE:nodejs my_custom_analyzer_example@indexes\analyzers.js /}

{NOTE/}
{NOTE: }

##### Add custom analyzer directly to RavenDB's binaries

Another way to add custom analyzers to RavenDB is by placing them next to RavenDB's binaries.

The fully qualified name must be specified for any index-field that will be tokenized by the analyzer.

Note that the analyzer must be compatible with .NET Core 2.0 (e.g., a .NET Standard 2.0 assembly).

This is the only method for adding custom analyzers in RavenDB versions older than 5.2.

{NOTE/}
{PANEL/}

{PANEL: Viewing the indexed terms}

The terms generated for each index-field can be viewed in the Studio.

![The index terms](images/index-terms-1.png "The index terms")

1. These are the index-fields
2. Click the "Terms" button to view the generated terms for each field

----

![The index terms](images/index-terms-2.png "The index terms")

1. This is the "index-field name".
2. These are the terms generated for the index-field.  
   In this example the `StopAnalyzer` was used to tokenize the text.

{PANEL/}

## Related Articles

### Indexes

- [Boosting](../indexes/boosting)
- [Storing data in index](../indexes/storing-data-in-index)
- [Dynamic Fields](../indexes/using-dynamic-fields)

### Studio
- [Custom Analyzers](../studio/database/settings/custom-analyzers)
- [Create Map Index](../studio/database/indexes/create-map-index)  
