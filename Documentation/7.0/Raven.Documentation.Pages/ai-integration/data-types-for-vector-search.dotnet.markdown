# Data Types for Vector Search
---

{NOTE: }

* Data for vector search can be stored in several data type formats, as outlined below.

* Text and numerical data that is Not pre-quantized can be further quantized in the generated embeddings.  
  Learn more in [Quantization options](../todo..).

* In this page:
  * [Supported data types for vector search](../ai-integration/data-types-for-vector-search#supported-data-types-for-vector-search)
      * [Textual data](../ai-integration/data-types-for-vector-search#textual-data)
      * [Numerical data](../ai-integration/data-types-for-vector-search#numerical-data) 
  * [RavenVector](../ai-integration/data-types-for-vector-search#ravenvector)
    
{NOTE/}

---

{PANEL: Supported data types for vector search}

### Textual data

{CONTENT-FRAME: }

`string` - A single text entry.  
`string[]` - An array of text entries.

{CONTENT-FRAME/}

---

### Numerical data

* You can store **pre-generated** embedding vectors in your documents,  
  typically created by machine-learning models from text, images, or other sources.

* When storing numerical embeddings in a document field:  
  Ensure that all vectors within this field across all documents in the collection have the **same dimension**.  
  All vectors indexed in same vector space must maintain consistent dimensionality.

* In addition to the native types described below, we highly recommended using [RavenVector](../../ai-integration/data-types-for-vector-search#ravenvector)  
  for efficient storage and fast queries when working with numerical embeddings.

{CONTENT-FRAME: }

**Raw embedding data**:  
Use when precision is critical.  

`float[]` - A single vector of numerical values representing raw embedding data.  
`foat[][]`- An array of vectors, where each entry is a separate embedding vector.  

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Pre-quantized data**:   
Use when you prioritize storage efficiency and query speed.  

`byte[] / sbyte[]` - A single pre-quantized embedding vector in the Int8 quantization format.   
`byte[][] / sbyte[][]` - An array of pre-quantized embedding vectors.  

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Base64-encoded data**:  
Use when embedding data needs to be represented as a compact and easily serializable string format.

`string` - A single vector encoded as a Base64 string.    
`string[]` - An array of Base64-encoded vectors.    

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Additional types**:  

* While arrays `(T[])` are the most direct representation of numerical embeddings,  
  you can also use lists (`List<T>` or `List<T[]>`) for dynamic sizing in your application code.

* You can use types such as `double` or `int` as the underlying data types.  
  However, while supported, these are generally less suitable for vector search,  
  as embeddings typically use `float` for a balanced trade-off between precision and efficiency in similarity search.

{CONTENT-FRAME/}
{PANEL/}

{PANEL: RavenVector}

RavenVector is RavenDB's dedicated data type for storing and querying **numerical embeddings**.   
It is highly optimized to minimize storage space and improve the speed of reading arrays from disk,  
making it ideal for vector search.

For example, you can define:

{CODE-BLOCK:csharp}
RavenVector<float>;       // A single vector of floating-point values.
List<RavenVector<float>>; // A collection of float-based vectors.
RavenVector<sbyte>;       // A single pre-quantized vector in Int8 format (8-bit signed integer).
List<RavenVector<sbyte>>; // A collection of sbyte-based vectors.
{CODE-BLOCK/}

Other numerical types, such as _byte_, _int_, or _double_, can also be used with `RavenVector`.  
However, _float_ and _sbyte_ are typically preferred for embeddings due to their efficiency.  

---

When a class property is stored as a `RavenVector`, the vector's content will appear under the `@vector` field in the JSON document stored in the database.
For example:

{CODE-TABS}
{CODE-TAB:csharp:CSharp_class class@AiIntegration\DataTypes.cs /}
{CODE-TABS/}

![json document](images/json-document.png "RavenVector in a JSON document")

{PANEL/}

## Related Articles

### Vector Search

- [RavenDB as a vector database](../ai-integration/ravendb-as-vector-database)
- [Vector search using a static index](../ai-integration/vector-search-using-static-index)
- [Vector search using a dynamic query](../ai-integration/vector-search-using-dynamic-query)

### Querying

- [Query overview](../client-api/session/querying/how-to-query)
- [Full-text search](../client-api/session/querying/text-search/full-text-search)



