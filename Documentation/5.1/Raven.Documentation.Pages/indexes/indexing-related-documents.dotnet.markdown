# Indexes: Indexing Related Documents
---

{NOTE: }

* As described in [modeling considerations in RavenDB](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/3-document-modeling#summary),  
  it is recommended for documents to be: independent, isolated, and coherent.  
  However, to accommodate varied models, __documents can reference other documents__.  

* The related data from a referenced (related) document can be indexed,  
  this will allow querying the collection by the indexed related data.

* The related documents that are loaded in the index definition are tracked for changes.

* In this page:

   * [What are related documents](../indexes/indexing-related-documents#what-are-related-documents)<br/><br/>
   * [Index related documents](../indexes/indexing-related-documents#index-related-documents)
     * [Example I - basic](../indexes/indexing-related-documents#example-i---basic)
     * [Example II - list](../indexes/indexing-related-documents#example-ii---list)
     * [Tracking implications](../indexes/indexing-related-documents#tracking-implications)
   * [Document changes that cause re-indexing](../indexes/indexing-related-documents#document-changes-that-cause-re-indexing)
   * [LoadDocument Syntax](../indexes/indexing-related-documents#loaddocument-syntax)
  
{NOTE/}

{PANEL: What are related documents}

* Whenever a document references another document, the referenced document is called a __Related Document__.  

* In the image below, document `products/34-A` references documents `categories/1-A` & `suppliers/16-A`,  
  which are considered Related Documents.

![Referencing related documents](images/index-related-documents.png "Referencing related documents")

{PANEL/}

{PANEL: Index related documents}

{NOTE: }
#### Example I - basic

---

__What is tracked__:

* Both the documents from the __indexed collection__ and the __indexed related documents__ are tracked for changes.  
  Re-indexing will be triggered per any change in either collection.  
  (See changes that cause re-indexing [here](../indexes/indexing-related-documents#document-changes-that-cause-re-indexing)).

__The index__:

* Following the above `Product - Category` relationship from the Northwind sample database,  
  an index defined on the Products collection can index data from the related Category document.

{CODE-TABS}
{CODE-TAB:csharp:LINQ-index indexing_related_documents_1@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TAB:csharp:JavaScript-index indexing_related_documents_1_JS@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TABS/}

__The query__:
 
* We can now query the index for Product documents by `CategoryName`,  
  i.e. get all matching Products that reference a Category that has the specified name term.

{CODE-TABS}
{CODE-TAB:csharp:Query(sync) indexing_related_documents_2@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TAB:csharp:Query(async) indexing_related_documents_2_async@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByCategoryName"
where CategoryName == "Beverages"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }
#### Example II - list

---

__The documents__:

{CODE indexing_related_documents_3@Indexes\IndexingRelatedDocuments.cs /}

__The index__:

* This index will index all names of the related Book documents.

{CODE-TABS}
{CODE-TAB:csharp:LINQ-index indexing_related_documents_4@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TAB:csharp:JavaScript-index indexing_related_documents_4_JS@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TABS/}

__The query__:

* We can now query the index for Author documents by a book's name,  
  i.e. get all Authors that have the specified book's name in their list.

{CODE-TABS}
{CODE-TAB:csharp:Query(sync) indexing_related_documents_5@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TAB:csharp:Query(async) indexing_related_documents_5_async@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// Get all authors that have books with title: "The Witcher"
from index "Authors/ByBooks"
where BookNames = "The Witcher"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{INFO: }
#### Tracking implications

* Indexing related data with tracking can be a useful way to query documents by their related data.  
  However, that may come with performance costs.

* __Re-indexing__ will be triggered whenever any document in the collection that is referenced by `LoadDocument` is changed. 
  Even when indexing just a single field from the related document, any change to any other field will cause re-indexing. 
  (See changes that cause re-indexing [here](../indexes/indexing-related-documents#document-changes-that-cause-re-indexing)).

* Frequent re-indexing will increase CPU usage and reduce performance.  
  The index may be in a non-stable state for prolonged periods, and results may be stale.

* Tracking indexed related data is more useful when the indexed related collection is known not to change much.

{INFO/}

{PANEL/}

{PANEL: Document changes that cause re-indexing}

* The following changes done to a document will trigger re-indexing:  

    * Any modification to any document field (not just to the indexed fields)
    * Adding/Deleting an attachment
    * Creating a new Time series (modifying existing will not trigger)
    * Creating a new Counter (modifying existing will not trigger)

* Any such change done either on any document in the __indexed collection__ or in the  __indexed related documents__ will trigger re-indexing.

{PANEL/}

{PANEL: LoadDocument syntax}

#### Syntax for LINQ-index:

{CODE:csharp syntax@Indexes\IndexingRelatedDocuments.cs /}

#### Syntax for JavaScript-index:

{CODE:nodejs syntax_JS@Indexes\IndexingRelatedDocuments.js /}

| Parameters                |                       |                                        |
|---------------------------|-----------------------|----------------------------------------|
| **relatedDocumentId**     | `string`              | ID of the related document to load     |
| **relatedCollectionName** | `string`              | The related collection name            |
| **relatedDocumentIds**    | `IEnumerable<string>` | A list of related document IDs to load |

{PANEL/}

## Related Articles

### Indexes

- [Indexing Basics](../indexes/indexing-basics)
- [Indexing Hierarchical Data](../indexes/indexing-hierarchical-data)
- [Indexing Spatial Data](../indexes/indexing-spatial-data)
- [Indexing Polymorphic Data](../indexes/indexing-polymorphic-data)

### Querying

- [Basics](../indexes/querying/basics)

---

## Code Walkthrough

- [Index Related Documents](https://demo.ravendb.net/demos/csharp/related-documents/index-related-documents)

---

## Inside RavenDB

- [Document Modeling](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/3-document-modeling)
- [Indexing Referenced Data](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/10-static-indexes-and-other-advanced-options#indexing-referenced-data)

