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

{NOTE/}

---

{PANEL: Indexing metadata properties}

{CODE:php index_1@Indexes/Metadata.php /}

{CODE:php query_1@Indexes/Metadata.php /}

{PANEL/}

## Related articles

### Indexes

- [Indexing Basics](../indexes/indexing-basics)

### Client API

- [How to get and modify the metadata](../client-api/session/how-to/get-and-modify-entity-metadata)
