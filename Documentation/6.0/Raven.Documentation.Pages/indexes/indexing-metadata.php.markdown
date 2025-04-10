# Indexing Metadata
---

{NOTE: }

* Each document in the database includes a metadata section, stored in a special JSON object under the `@metadata` property.

* This metadata is not part of the document's content but holds internal system information (used by RavenDB),
  such as the document ID, collection name, change vector, last modified timestamp, and more,  
  as well as optional user-defined entries.

* To learn how to access (get and modify) the metadata from your client code,  
  see [How to get and modify the metadata](../client-api/session/how-to/get-and-modify-entity-metadata).

* Content from metadata properties can be extracted and **indexed** within a static index, alongside content from the document fields.
  This allows you to query for documents based on values stored in the metadata.  
  See the examples below.

---

* In this article:
    * [Indexing metadata properties](../indexes/indexing-metadata#indexing-metadata-properties)
    * [Metadata properties that can be indexed](../indexes/indexing-metadata#metadata-properties-that-can-be-indexed)

{NOTE/}

---

{PANEL: Indexing metadata properties}

* To access a document's metadata, use the `MetadataFor` method, which is available in the **C# LINQ string**
  that is assigned to the `$this->map` property in the PHP index class, as shown in the example below.

* You can retrieve metadata values using one of two C# syntaxes:

    * **Generic method syntax**  
      Use `Value<T>()` to retrieve and cast the metadata value to the expected type.  
      This is type-safe and preferred when the type is known (e.g., _DateTime_).
    * **Indexer syntax**  
      Use `metadata["key"]` to retrieve the raw object.  
      You can cast it manually if needed.

---

* The following index definition indexes content from the `@last-modified` and `@counters` metadata properties.

{CODE-TABS}
{CODE-TAB:php:Index_accessMetadataViaValue index_1@Indexes/Metadata.php /}
{CODE-TAB:php:Index_accessMetadataViaIndexer index_2@Indexes/Metadata.php /}
{CODE-TABS/}

* Query for documents based on metadata values:  
  Retrieve documents that have counters and order them by their last modified timestamp.

{CODE-TABS}
{CODE-TAB:php:Query query_1@Indexes/Metadata.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByMetadata/AccessViaValue"
where hasCounters == true
order by lastModified desc
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Metadata properties that can be indexed}

* The following are the **predefined metadata properties that can be indexed**:
    * `@archive-at`
    * `@attachments`
    * `@change-vector`
    * `@collection`
    * `@counters`
    * `@etag`
    * `@expires`
    * `@id`
    * `@last-modified`
    * `@refresh`
    * `@timeseries`
    * `Raven-Clr-Type`

* You can add custom metadata properties to any document as needed.  
  These custom properties can be indexed just like the predefined ones.

---

{WARNING: }

Note:

* The `@attachments` metadata property can only be indexed using a **Lucene** index.
* The **Corax** search engine does not support indexing complex JSON properties.  
  Learn more in [Corax: Handling complex JSON objects](../indexes/search-engine/corax#handling-of-complex-json-objects).

{WARNING/}
{PANEL/}

## Related articles

### Indexes

- [Indexing Basics](../indexes/indexing-basics)

### Client API

- [How to get and modify the metadata](../client-api/session/how-to/get-and-modify-entity-metadata)
