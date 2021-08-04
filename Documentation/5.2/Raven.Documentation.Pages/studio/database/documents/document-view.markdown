# Document View
---

{NOTE: }

* In this view, a document can be viewed and edited.  

* Actions such as cloning the document, deleting, adding attachments, and much more can be performed.  

* In this page:  
  * [Document View](../../../studio/database/documents/document-view#the-document-view)  
  * [Documents View - Actions](../../../studio/database/documents/document-view#document-view---actions)  
{NOTE/}

---

{PANEL: The Document View}

![Figure 1. Document View](images/document-view-1.png "Document: 'Suppliers/1-A' in the 'Suppliers' Collection")

1. **Document identifier (ID)**  

   * For a detailed explanation about the possible identifiers, see [Create New Document](../../../studio/database/documents/create-new-document)  
   * Clicking the link right by the ID will show the _raw document output_ that is received  from the server  

2. **Document properties**  

   * **Change-Vector**  

     * The _change-vector_ uniquely marks the specific version of the document globally in the cluster  
     * Each time a document is modified, a new _change-vector_ is generated  
     * Used for optimistic  concurrency control, various internal operations and caching  
     * It is composed of a list of node _tags_ and _etags_  
       _Node tag_ - uniquely identifies a node  
       _etag_ - a 64 bit number that is incremented on every operation in a database  

   * **Modified** - The last time the document was modified by any client, or by the Studio  
   * **Size** - The Document size (including attachments)  

3. **Document content** - The document properties and values in JSON format  
   * a. **Nested Data**  
        * Another JSON object can be nested in a property value  

   * b. **Referenced Documents**  
        * You can reference other documents from any other collection (or from the current collection)  
        * These referenced documents can then be _included_ in a single _Load_ request to the server. See [Load with Includes](../../../client-api/session/loading-entities#load-with-includes)  
        * In the example above, document _'categories/1-A'_ is referenced in the _'Category'_ property  

   * c. **Metadata**  
        * This is additional information about the document  
        * The metadata, also in a JSON format, is embedded inside the document and is an integral part of it  
        * RavenDB server reserves metadata properties that start with _'@'_ for its own use  
        * You can add properties to the metadata to store your own values  
        * Note: only the following metadata properties will show in the Studio:  
           * ***@collection*** - determines to which collection the document belongs to  
           * ***@flags*** - i.e. if a document has attachments, revisions, etc.  
           * ***Custom metadata*** properties - any metadata properties generated in code by a client  

4. **Related Document**  
   * List of related documents - those are the documents that are _referenced_ inside the document. (see 3b above)
   * Click to open each  
{PANEL/}

{PANEL: Document View - Actions}

![Figure 2. Document View Actions](images/document-view-2.png "Document View Actions")

1. **General** actions  
   * **Save** document  
   * **Delete** document  
     * The document _content_ is deleted and will not be available again
     * The document itself is marked as a _Tombstone_ , so that the delete action can be replicated to the other database instances  
   * **Clone** document - Create a clone of the current document  
     * A copy of the document without attachments is created  
     * It can be saved with a new ID  

2. **Copy** to clipboard  
   * Copy to clipboard - The document content is copied  
   * Copy as C# class - The C# entity class (reflecting the document JSON content) is copied  

3. **Format** content
   * Format - Adjust the document JSON format  
   * Toggle collapse - Toggle nested complex properties in the document  
   * Toggle new lines - Toggle between showing the character '/n', or the actual new lines  

4. **Attachments**  
   * Add any type of file as an attachment to the document  
{PANEL/}
