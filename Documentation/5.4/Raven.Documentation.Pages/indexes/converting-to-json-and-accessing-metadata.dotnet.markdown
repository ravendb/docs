# Indexes: Converting to JSON and Accessing Metadata

---
{NOTE: }

Entities passed to an index can be converted to JSON using the `AsJson` method.  
It is also possible to access metadata for a specified object using the `MetadataFor` method.  

* In this Page:  
   * [AsJson - Converting to JSON](../indexes/converting-to-json-and-accessing-metadata#asjson---converting-to-json)  
   * [MetadataFor - Accessing Metadata](../indexes/converting-to-json-and-accessing-metadata#metadatafor---accessing-metadata)  

{NOTE/}

---

{PANEL: AsJson - Converting to JSON}

{CODE indexes_1@Indexes/Metadata.cs /}

{CODE indexes_2@Indexes/Metadata.cs /}

{PANEL/}

{PANEL: MetadataFor - Accessing Metadata}

{CODE indexes_3@Indexes/Metadata.cs /}

{CODE indexes_4@Indexes/Metadata.cs /}

{PANEL/}

## Related articles

### Indexes

- [Indexing Basics](../indexes/indexing-basics)

### Client API

- [How to Get and Modify Entity Metadata](../client-api/session/how-to/get-and-modify-entity-metadata)
