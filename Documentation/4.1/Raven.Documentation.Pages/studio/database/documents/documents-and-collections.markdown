# Documents and Collections
---

{NOTE: Documents}

* A document holds your data in a JSON format object  
* For more information about documents, see [Document View](../../../studio/database/documents/document-view/document-view)  
{NOTE/}

{NOTE: Collections}

* Collections are used to group documents together so that it is convenient to apply some operation to them,  
  i.e. subscribing to changes, indexing, querying, ETL, etc.  

* Every document belongs to exactly one collection.  

* Typically, a collection holds similarly structured documents based on the entity type of the document.  

* Note: It is not required that documents within the same collection will share the same structure or have any sort of schema. The only requirement for documents to be in the same collection is that they must have the same `@collection` metadata property.  

* For more information see [What is a Collection](../../../client-api/faq/what-is-a-collection)  
{NOTE/}

{NOTE: }

* In this page:  
  * [The Documents View](../../../studio/database/documents/documents-and-collections#the-documents-view)  
  * [The @hilo Collection](../../../studio/database/documents/documents-and-collections#the-@hilo-collection)  
  * [The @empty Collection](../../../studio/database/documents/documents-and-collections#the-@empty-collection)  
{NOTE/}

---

{PANEL: The Documents View}  

* Shows all collections and the documents each contains.  
* You can perform actions such as create, delete, or export a document.  

![Figure 1. Documents and Collections](images/documents-and-collections-1.png "Collection 'Categories'")

1.  **Recent**:  
  *  Click on `Recent` to see a list of **all** documents from **all** collections in the selected database  
  *  Documents are ordered by the time they were last modified  

2.  **Collections**:  
  *  The existing **collections** in the selected database  
  *  The number of documents each collection has is indicated  

3.  **Documents**:  
  *  The list of documents within the selected collection  
  *  Each **column** corresponds to a _property_ in the document JSON  
  *  Documents are ordered by the time they were last modified  
{PANEL/}

{PANEL}  

![Figure 2. Actions](images/documents-and-collections-2.png "Actions")

* 1  
  * **New Document**: Create a new document (in a new collection -or- in the current collection)  
  * **Delete**: Delete selected documents  
  * **Copy**: Copy documents or just document IDs of selected documents  

* 2  
  *  **Query**: Navigates to query current collection view  
  *  **Export CSV**: Export the collection data into a CSV file (visible columns only -or- all documents columns)  
  *  **Display**: Customize which columns to view. A custom column can be added  

* 3  
  * **Patch**: Patch documents in a collection or in an index. See [Patch Documents](../../../../todo-update-me-later)  
  * **Query**: Query documents in a collection or in an index. See [Query Documents](../../../../todo-update-me-later)  
  * **Conflicts**: View and resolve conflicting documents. See [Conflicts](../../../studio/database/documents/conflicts-view)  
{PANEL/}

{PANEL}  

Click `Display` to:  

* Select which columns to view  
* Reorder columns viewed  
* Add a custom column  


![Figure 3. Manage Displayed Columns](images/documents-and-collections-3.png "Manage Displayed Columns")

{PANEL/}

{PANEL: The @hilo Collection}  

![Figure 4. hilo collection](images/documents-and-collections-4.png "The @hilo Collection")

* Documents in the _@hilo_ collection are created when documents are created from the [RavenDB client](../../../client-api/session/storing-entities) (Not from the studio) **without an explicit ID**.  

* The _'Max'_ property value that shows in the hilo doc represents the largest ID number that was used for a _client generated document_ in that collection.  

* For more information, see [HiLo Algorithm](../../../client-api/document-identifiers/hilo-algorithm).  
{PANEL/}

{PANEL: The @empty Collection}  

![Figure 5. empty collection](images/documents-and-collections-5.png "The @empty Collection")

* The _@empty_ collection includes:  
  1. Documents that were created with a **GUID identifier** (without specifying a collection)  
  2. Documents that were created with a **Semantic ID** that does _not_ end with (/) or (|)  

* For more information about the various documents identifiers that can be generated, 
  see [Create New Document](../../../studio/database/documents/create-new-document#create-new-document).  
{PANEL/}

