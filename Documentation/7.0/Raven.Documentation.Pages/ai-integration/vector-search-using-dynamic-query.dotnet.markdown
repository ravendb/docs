# Vector Search using a Dynamic Query
---

{NOTE: }

* This article explains how to run a vector search using a **dynamic query**.  
  To learn how to run a vector search using a static-index, see [vector search using a static-index](../ai-integration/vector-search-using-static-index).

* In this page:
  * [What is a vector search](../ai-integration/vector-search-using-dynamic-query#what-is-a-vector-search)
  * [Dynamic vector search query - Overview](../ai-integration/vector-search-using-dynamic-query#dynamic-vector-search-query---overview)
     * [Creating embeddings for the auto-index](../ai-integration/vector-search-using-dynamic-query#creating-embeddings-for-the-auto-index)
     * [Retrieving results](../ai-integration/vector-search-using-dynamic-query#retrieving-results)
     * [The dynamic query parameters](../ai-integration/vector-search-using-dynamic-query#the-dynamic-query-parameters)
  * [Vector search on TEXT](../ai-integration/vector-search-using-dynamic-query#vector-search-on-text)
  * [Vector search on NUMERICAL content](../ai-integration/vector-search-using-dynamic-query#vector-search-on-numerical-content)
  * [Exact search](../ai-integration/vector-search-using-dynamic-query#exact-search)
  * [Quantization options](../ai-integration/vector-search-using-dynamic-query#quantization-options)
  * [Querying vector fields and regular data in the same query](../ai-integration/vector-search-using-dynamic-query#querying-vector-fields-and-regular-data-in-the-same-query)
  * [Syntax](../ai-integration/vector-search-using-dynamic-query#syntax)
    
{NOTE/}

---

{PANEL: What is a vector search}

* Vector search is a method for finding documents based on their **contextual similarity** to the search item provided in a given query.
 
* Your data is converted into vectors, known as **embeddings**, and stored in a multidimensional space.  
  Unlike traditional keyword-based searches, which rely on exact matches,
  vector search identifies vectors closest to your query vector and retrieves the corresponding documents.

{PANEL/}

{PANEL: Dynamic vector search query - Overview}

* To make a **dynamic vector search query**:  
  * From the Client API - use method `VectorSearch()`, examples are provided later in this article
  * In RQL - use method `vector.search()`

* The **source data types** that can be used for vector search are detailed in [Data types for vector search](../ai-integration/data-types-for-vector-search).

* Note: Vector search queries cannot be used with [subscription queries](../client-api/data-subscriptions/creation/api-overview#subscription-query).

* When executing a dynamic vector search query, RavenDB creates a Corax [Auto-index](../client-api/session/querying/how-to-query#queries-always-provide-results-using-an-index) to process the query,  
  and the results are retrieved from that index.    
     {INFO: }
     * Only [Corax indexes](../indexes/search-engine/corax) support vector search.
     * Even if your default auto-index engine is set to Lucene (via [Indexing.Auto.SearchEngineType](../server/configuration/indexing-configuration#indexing.auto.searchenginetype)),  
       performing a vector search using a dynamic query will create a new auto-index based on Corax.
     {INFO/}

---

{CONTENT-FRAME: }

#### Creating embeddings for the Auto-index
---

* **Creating embeddings from TEXTUAL content**:  
  When querying over textual data, for each document in the queried collection,  
  RavenDB will generate an embedding vector for the specified document field.  
  The embedding is created using the built-in [bge-micro-v2](https://huggingface.co/TaylorAI/bge-micro-v2) sentence-transformer model.  

* **Creating embeddings from NUMERICAL arrays**:  
  When querying over pre-made numerical arrays that are already in vector format,  
  RavenDB will use them without transformation (unless further quantization is applied).
    {WARNING: }
    To avoid index errors, ensure that the dimensionality of these numerical arrays (i.e., their length)  
    is consistent across all your source documents for the field you are querying.  
    If you wish to enforce such consistency -  
    perform a vector search using a [static-index](../ai-integration/vector-search-using-static-index) instead of a dynamic query.
    {WARNING/} 

* **Quantizing the embeddings**:  
  The embeddings are quantized based on the parameters specified in the query.  
  Learn more about quantization in [Quantization options](../ai-integration/vector-search-using-dynamic-query#quantization-options).

* **Indexing the embeddings**:  
  RavenDB indexes the embeddings on the server using the [HNSW algorithm](https://en.wikipedia.org/wiki/Hierarchical_navigable_small_world).  
  This algorithm organizes embeddings into a high-dimensional graph structure,  
  enabling efficient retrieval of Approximate Nearest Neighbors (ANN) during queries.

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Retrieving results
---

* **Processing the query**:  
  To ensure consistent comparisons, the **search term** is transformed into an embedding vector using the same method as the document fields.
  The server will search for the most similar vectors in the indexed vector space,
  taking into account all the [query parameters](../ai-integration/vector-search-using-dynamic-query#the-dynamic-query-parameters) described below.  
  The documents that correspond to the resulting vectors are then returned to the client. 

* **Search results**:  
  By default, the resulting documents will be ordered by their score.
  You can modify this behavior using the [Indexing.Corax.VectorSearch.OrderByScoreAutomatically](../server/configuration/indexing-configuration#indexing.corax.vectorsearch.orderbyscoreautomatically) configuration key.  
  In addition, you can apply any of the 'order by' methods to your query, as explained in [sort query results](../client-api/session/querying/sort-query-results).

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### The dynamic query parameters
---

* **Source data format**  
  RavenDB supports performing vector search on TEXTUAL values or NUMERICAL arrays.  
  the source data can be formatted as `Text`, `Single`, `Int8`, or `Binary`.

* **Target quantization**  
  You can specify the quantization encoding for the embeddings that will be created from source data.  
  Learn more about quantization in [Quantization options](../ai-integration/vector-search-using-dynamic-query#quantization-options).  

* **Minimum similarity**  
  You can specify the minimum similarity to use when searching for related vectors.  
  Can be a value between `0.0f` and `1.0f`.  
  A value closer to `1.0f` requires higher similarity between vectors,  
  while a value closer to `0.0f` allows for less similarity.

    If not specified, the default value is taken from the following configuration key:
    [Indexing.Corax.VectorSearch.DefaultMinimumSimilarity ](../server/configuration/indexing-configuration#indexing.corax.vectorsearch.defaultnumberofcandidatesforquerying).

* **Number of candidates**  
  You can specify the maximum number of vectors that RavenDB will return from a graph search.  
  The number of the resulting documents that correspond to these vectors may be:
    * lower than the number of candidates - when multiple vectors originated from the same document.
    * higher than the number of candidates - when the same vector is shared between multiple documents.

    If not specified, the default value is taken from the following configuration key:
    [Indexing.Corax.VectorSearch.DefaultNumberOfCandidatesForQuerying](../server/configuration/indexing-configuration#indexing.corax.vectorsearch.defaultminimumsimilarity).

* **Search method**
    * _Approximate Nearest-Neighbor search_ (Default):   
      Search for related vectors in an approximate manner, providing faster results.
    * _Exact search_:   
      Perform a thorough scan of the vectors to find the actual closest vectors,  
      offering better accuracy but at a higher computational cost.  
      Learn more in [Exact search](../ai-integration/vector-search-using-dynamic-query#exact-search).

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Vector search on TEXT}

* The following example searches for Product documents where the _'Name'_ field is similar to the search term `"italian food"`.

* Since this query does not specify a target quantization format,
  the generated embedding vectors will be encoded in the default _Single_ format (single-precision floating-point quantization).  
  Refer to [Quantization options](../ai-integration/vector-search-using-dynamic-query#quantization-options) for examples that specify the destination quantization.

    {CODE-TABS}
    {CODE-TAB:csharp:Query vs_1@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
    {CODE-TAB:csharp:Query_async vs_1_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
    {CODE-TAB:csharp:DocumentQuery vs_2@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
    {CODE-TAB:csharp:DocumentQuery_async vs_2_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
    {CODE-TAB:csharp:RawQuery vs_3@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
    {CODE-TAB:csharp:RawQuery_async vs_3_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
    {CODE-TAB-BLOCK:sql:RQL}
// Query the Products collection
from "Products"
// Call 'vector.search'
// Wrap the document field 'Name' with 'embedding.text' to indicate the source data type
where vector.search(embedding.text(Name), "italian food", 0.82, 20)
    {CODE-TAB-BLOCK/}
    {CODE-TABS/}

* Executing the above query on the RavenDB sample data will create the following auto-index:  
  `Auto/Products/ByVector.search(embedding.text(Name))`

    ![Search for italian food 1](images/vector-search-1.png "Products with a name similar to 'italian food' - high similarity")
  
* Running the same query at a lower similarity level will return more results related to _"Italian food"_ but they may be less similar:

    ![Search for italian food 2](images/vector-search-2.png "Products with a name similar to 'italian food' - with lower similarity")

{PANEL/}

{PANEL: Vector search on NUMERICAL content}

* The following examples will use the sample data shown below.  
  The _Movie_ class includes various formats of numerical vector data.  
  Note: This sample data is minimal to keep the examples simple.

* Note the usage of RavenDB's dedicated data type, [RavenVector](../ai-integration/data-types-for-vector-search#ravenvector), which is highly optimized for reading and writing arrays to disk.
  Learn more about the source data types suitable for vector search in [Data types for vector search](../ai-integration/data-types-for-vector-search).

* Unlike vector searches on text, where RavenDB transforms the raw text into an embedding vector,  
  numerical vector searches require your source data to already be in an embedding vector format.  

* If your raw data is in a _float_ format, you can request further quantization of the embeddings that will be indexed in the auto-index.
  See an example of this in: [Quantiztion options](../ai-integration/vector-search-using-dynamic-query#quantization-options).  

* Raw data that is already formatted as _Int8_ or _Binary_ **cannot** be quantized to lower-form (e.g. Int8 -> Int1).  
  When storing data in these formats in your documents, you should use [RavenDB’s vector quantizer methods](../ai-integration/vector-search-using-dynamic-query#section-1).

---

#### Sample data:

{CODE-TABS}
{CODE-TAB:csharp:Class movie_class@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:Sample_data sample_data@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:Sample_document sample_document@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TABS/}

---

#### Examples:

These examples search for Movie documents with vectors similar to the one provided in the query.

{CONTENT-FRAME: }

* Search on the `TagsEmbeddedAsSingle` field,  
  which contains numerical data in **floating-point format**. 

{CODE-TABS}
{CODE-TAB:csharp:Query vs_4@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:Query_async vs_4_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:DocumentQuery vs_5@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:DocumentQuery_async vs_5_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:RawQuery vs_6@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:RawQuery_async vs_6_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Movies"
// The source document field type is interpreted as 'Single' by default
where vector.search(TagsEmbeddedAsSingle, $p0, 0.85, 10)
{"p0" : { "@vector" : [6.599999904632568, 7.699999809265137] }}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

* Search on the `TagsEmbeddedAsInt8` field,  
  which contains numerical data that is already quantized in **_Int8_ format**.

{CODE-TABS}
{CODE-TAB:csharp:Query vs_7@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Movies"
// Wrap the source document field name with 'embedding.i8' to indicate the source data type
where vector.search(embedding.i8(TagsEmbeddedAsInt8), $p0)
{"p0" : [64, 127, -51, -52, 76, 62] }
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

* Search on the `TagsEmbeddedAsBase64` field,  
  which contains numerical data represented in **_Base64_ format**.

{CODE-TABS}
{CODE-TAB:csharp:Query vs_8@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Movies"
// * Wrap the source document field name using 'embedding.<format>' to specify
//   the source data type from which the Base64 string was generated.
// * If the document field is Not wrapped, 'single' is assumed as the default source type. 
where vector.search(TagsEmbeddedAsBase64, $p0)
{"p0" : "zczMPc3MTD6amZk+" }
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Exact search}

* When performing a dynamic vector search query, you can specify whether to perform an **exact search** to find the closest similar vectors in the vector space:
  * A thorough scan will be performed to find the actual closest vectors.
  * This ensures better accuracy but comes at a higher computational cost.

* If exact is Not specified, the search defaults to the **Approximate Nearest-Neighbor** (ANN) method,  
  which finds related vectors in an approximate manner, offering faster results.

* The following example demonstrates how to specify the exact method in the query.  
  Setting the param is similar for both text and numerical content searches.

    {CODE-TABS}
    {CODE-TAB:csharp:Query vs_9@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
    {CODE-TAB:csharp:Query_async vs_9_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
    {CODE-TAB:csharp:DocumentQuery vs_10@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
    {CODE-TAB:csharp:DocumentQuery_async vs_10_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
    {CODE-TAB:csharp:RawQuery vs_11@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
    {CODE-TAB:csharp:RawQuery_async vs_11_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
    {CODE-TAB-BLOCK:sql:RQL}
from "Products"
// Wrap the vector.search query with the 'exact()' method
where exact(vector.search(embedding.text(Name), "italian food"))
    {CODE-TAB-BLOCK/}
    {CODE-TABS/}

{PANEL/}

{PANEL: Quantization options}

#### What is quantization:

Quantization is a technique that reduces the precision of numerical data.
It converts high-precision values, such as 32-bit floating-point numbers, into lower-precision formats like 8-bit integers or binary representations.

The quantization process, applied to each dimension (or item) in the numerical array, 
serves as a form of compression by reducing the number of bits used to represent each value in the vector.
For example, transitioning from 32-bit floats to 8-bit integers significantly reduces data size while preserving the vector's essential structure.  

Although it introduces some precision loss, quantization minimizes storage requirements and optimizes memory usage.
It also reduces computational overhead, making operations like similarity searches faster and more efficient.

#### Quantization in RavenDB:

For non-quantized raw 32-bit data or text stored in your documents,
RavenDB allows you to choose the quantization format for the generated embeddings stored in the index.  
The selected quantization type determines the similarity search technique that will be applied.

If no target quantization format is specified, the `Single` option will be used as the default.

The available quantization options are:  

   * `Single` (a 32-bit floating point value per dimension):  
     Provides precise vector representations.  
     The [Cosine similarity](https://en.wikipedia.org/wiki/Cosine_similarity) method will be used for searching and matching.  

   * `Int8` (an 8-bit integer value per dimension):  
     Reduces storage requirements while maintaining good performance.  
     Saves up to 75% storage compared to 32-bit floating-point values.  
     The Cosine similarity method will be used for searching and matching.  

   * `Binary` (1-bit per dimension):  
     Minimizes storage usage, suitable for use cases where binary representation suffices.  
     Saves approximately 96% storage compared to 32-bit floating-point values.  
     The [Hamming distance](https://en.wikipedia.org/wiki/Hamming_distance) method will be used for searching and matching.  
      
     {NOTE: }
     If your documents contain data that is already quantized,  
     it cannot be re-quantized to a lower precision format (e.g., Int8 cannot be converted to Binary).
     {NOTE/}

---

#### Examples

{CONTENT-FRAME: }

* In this example: 
  * The source data consists of text.
  * The generated embeddings will use the _Int8_ format.

{CODE-TABS}
{CODE-TAB:csharp:Query vs_15@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:Query_async vs_15_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:DocumentQuery vs_16@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:DocumentQuery_async vs_16_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:RawQuery vs_17@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:RawQuery_async vs_17_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
// Wrap the 'Name' field with 'embedding.text_i8'
where vector.search(embedding.text_i8(Name), $p0)
{ "p0" : "italian food" }
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

* In this example:  
    * The source data is an array of 32-bit floats.
    * The generated embeddings will use the _Binary_ format.

{CODE-TABS}
{CODE-TAB:csharp:Query vs_18@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:Query_async vs_18_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:DocumentQuery vs_19@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:DocumentQuery_async vs_19_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:RawQuery vs_20@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB:csharp:RawQuery_async vs_20_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Movies"
// Wrap the 'TagsEmbeddedAsSingle' field with 'embedding.f32_i1'
where vector.search(embedding.f32_i1(TagsEmbeddedAsSingle), $p0)
{ "p0" : { "@vector" : [6.599999904632568,7.699999809265137] }}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CONTENT-FRAME/}

---

#### Quantization methods in RQL:

The following methods are available for performing a vector search via RQL:

{CONTENT-FRAME: }

  `embedding.text`: Encodes text into 32-bit floating-point values.  
  `embedding.text_i8`: Encodes text into 8-bit integers.  
  `embedding.text_i1`: Encodes text into binary.  
  
  `embedding.f32_i8`: Converts 32-bit floating-point values to 8-bit integers.  
  `embedding.f32_i1`: Converts 32-bit floating-point values to binary.  
    
  `embedding.i8`: Indicates that the source data is already quantized as Int8  (cannot be further quantized).  
  `embedding.i1`: Indicates that the source data is already quantized as binary (cannot be further quantized).  

{CONTENT-FRAME/}

Wrap the field name using any of the relevant methods listed above, based on your requirements.  
For example, the following RQL encodes **text to Int8**:

{CODE-TABS}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
// Wrap the document field with 'embedding.text_i8'
where vector.search(embedding.text_i8(Name), "italian food", 0.82, 20)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

When the field name is Not wrapped in any method,
the underlying values are treated as numerical values in the form of **32-bit floating-point** (Single) precision.
For example, the following RQL will use the floating-point values as they are, without applying further quantization:

{CODE-TABS}
{CODE-TAB-BLOCK:sql:RQL}
from "Movies"
// No wrapping
where vector.search(TagsEmbeddedAsSingle, $p0, 0.85, 10)
{"p0" : { "@vector" : [6.599999904632568, 7.699999809265137] }}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Querying vector fields and regular data in the same query}

* You can perform a vector search and a regular search in the same query.  
  A single auto-index will be created for both search predicates.

* In the following example, results will include Product documents with content similar to "Italian food" in their _Name_ field and a _PricePerUnit_ above 20.
  The following auto-index will be generated:  
  `Auto/Products/ByPricePerUnitAndVector.search(embedding.text(Name))`.

  {CODE-TABS}
  {CODE-TAB:csharp:Query vs_12@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
  {CODE-TAB:csharp:Query_async vs_12_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
  {CODE-TAB:csharp:DocumentQuery vs_13@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
  {CODE-TAB:csharp:DocumentQuery_async vs_13_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
  {CODE-TAB:csharp:RawQuery vs_14@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
  {CODE-TAB:csharp:RawQuery_async vs_14_async@AiIntegration\VectorSearchUsingDynamicQuery.cs /}
  {CODE-TAB-BLOCK:sql:RQL}
  from "Products"
  where (PricePerUnit > $p0) and (vector.search(embedding.text(Name), $p1))
  { "p0" : 20.0, "p1" : "italian food" }
  {CODE-TAB-BLOCK/}
  {CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

`VectorSearch`:  

{CODE:csharp syntax_1@AiIntegration\VectorSearchUsingDynamicQuery.cs /}

| Parameter                 | Type                                                                                                                                 | Description                                                                                                                         |
|---------------------------|--------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------|
| **embeddingFieldFactory** | `Func<IVectorFieldFactory<T>, IVectorEmbeddingTextField>`<br><br>`Func<IVectorFieldFactory<T>, IVectorEmbeddingField>`               | Factory creating embedding vector field for indexing purposes.                                                                      |
| **embeddingFieldFactory** | `Func<IVectorFieldFactory<T>, IVectorField>`                                                                                         | Factory using existing, already indexed vector field.                                                                               |
| **embeddingValueFactory** | `Action<IVectorEmbeddingTextFieldValueFactory>`<br>`Action<IVectorEmbeddingFieldValueFactory>`<br>`Action<IVectorFieldValueFactory>` | Factory preparing queried data to be used in vector search.                                                                         |
| **minimumSimilarity**     | `float?`                                                                                                                             | Minimum similarity between the queried value and the indexed value for the vector search to match.                                  |
| **numberOfCandidates**    | `int?`                                                                                                                               | Number of candidate nodes for the HNSW algorithm.<br>Higher values improve accuracy but require more computation.                   |
| **isExact**               | `bool`                                                                                                                               | `false` - vector search will be performed in an approximate manner.<br>`true` - vector search will be performed in an exact manner. |


The default value for `minimumSimilarity` is defined by this configuration key:  
[Indexing.Corax.VectorSearch.DefaultMinimumSimilarity ](../server/configuration/indexing-configuration#indexing.corax.vectorsearch.defaultnumberofcandidatesforquerying).

The default value for `numberOfCandidates` is defined by this configuration key:  
[Indexing.Corax.VectorSearch.DefaultNumberOfCandidatesForQuerying](../server/configuration/indexing-configuration#indexing.corax.vectorsearch.defaultminimumsimilarity).

---

`IVectorFieldFactory`:

{CODE:csharp syntax_2@AiIntegration\VectorSearchUsingDynamicQuery.cs /}

| Parameter                       | Type                          | Description                                                                            |
|---------------------------------|-------------------------------|----------------------------------------------------------------------------------------|
| **documentFieldName**           | `string`                      | The name of the document field containing<br>text / embedding / base64 encoded data.   |
| **indexFieldName**              | `string`                      | The name of the index-field that vector search will be performed on.                   |
| **propertySelector**            | `Expression<Func<T, object>>` | Path to the document field containing<br>text / embedding /base64 encoded data.        |
| **indexPropertySelector**       | `Expression<Func<T, object>>` | Path to the index-field containing indexed data.                                       |
| **storedEmbeddingQuantization** | `VectorEmbeddingType`         | Quantization format of the stored embeddings.<br>Default: `VectorEmbeddingType.Single` |

---

`IVectorEmbeddingTextField`:

{CODE:csharp syntax_3@AiIntegration\VectorSearchUsingDynamicQuery.cs /}

| Parameter                       | Type                  | Description                             |
|---------------------------------|-----------------------|-----------------------------------------|
| **targetEmbeddingQuantization** | `VectorEmbeddingType` | The desired target quantization format. |

{CODE:csharp syntax_4@AiIntegration\VectorSearchUsingDynamicQuery.cs /}

---

`IVectorEmbeddingTextFieldValueFactory`:

{CODE:csharp syntax_5@AiIntegration\VectorSearchUsingDynamicQuery.cs /}

---

#### `RavenVector`:  
RavenVector is RavenDB's dedicated data type for storing and querying numerical embeddings.  
Learn more in [RavenVector](../ai-integration/data-types-for-vector-search#ravenvector)

{CODE:csharp syntax_6@AiIntegration\VectorSearchUsingDynamicQuery.cs /}

---

#### `VectorQuanitzer`:   
RavenDB provides the following quantizer methods.  
Use them to transform your raw data to the dezired format.  
Other quantizers may not be compatible.  

{CODE:csharp syntax_7@AiIntegration\VectorSearchUsingDynamicQuery.cs /}

{PANEL/}

## Related Articles

### Vector Search

- [RavenDB as a vector database](../ai-integration/ravendb-as-vector-database)
- [Vector search using a static index](../ai-integration/vector-search-using-static-index)
- [Data types for vector search](../ai-integration/data-tuypes-for-vector-search)

### Querying

- [Query overview](../client-api/session/querying/how-to-query)
- [Full-text search](../client-api/session/querying/text-search/full-text-search)
