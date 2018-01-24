## Documents and Collections
---

{NOTE: Documents}
A document holds your data in a JSON format object.  
See more about a document in [Document View](../../../../todo-update-me-later)
{NOTE/}

{NOTE: Collections}
Collections are used to group documents together so that it is convenient to apply some operation to them,  
i.e. subscribing to changes, indexing, querying, ETL, etc.

Every document belongs to exactly one collection.  

Typically, a collection holds similar structured documents - based on the entity type of the document.  
Note: It is Not required that documents within the same collection will share the same structure or have any sort of schema.  
{NOTE/}

---

{PANEL}
**Documents view:**  

* Shows all collections and the documents each contains.  
* Actions such as create, delete or export a document and more can be done.  

![Figure 1. Documents and Collections](images/documents-and-collections-1.png "Collection 'Categories'")  

**1.**  The existing **collections** in the database showing with the number of documents each collection contains.  
**2.**  The **documents** within a selected collection.  

*  The documents are ordered by the modification time
*  Each **column** corresponds to a key property in the document json
{PANEL/}

{NOTE: Recent Documents}
List of **all** documents from **all** collections in the database.  
Ordered by modification time.
{NOTE/}

{NOTE: @hilo Collection}
Documents in the _@hilo_ collection are created when a client (Not from studio) is creating documents without an explicit ID.
In this case, RavenDB will reserve a range of identifiers and ensure that this range is provided only to this client.
The client can safely generate identifiers within this given range.  

See more about documents IDs in [Create New Document](../../../../todo-update-me-later)  

The _'Max'_ property value in the hilo doc represents the largest ID number that was used (from the given range)  
for a document in the collection.  
{NOTE/}

---

{PANEL}
![Figure 2. Actions](images/documents-and-collections-2.png "Actions")

1.  
  * **New Document**: Create a new document (in a new collection -or- in the current collection)  
  * **Delete**: Delete selected documents  
  * **Copy**: Copy documents or just documents IDs of selected documents  
<br/>
2.  
  *  **Export CSV**: Export the collection data into a CSV file (visible columns only -or- all documents columns)  
  *  **Display**: Customize which columns to view. A custom column can be added  
<br/>
3.  
  * **Patch**: Patch documents in a collection or in an index. See [Patch Documents](../../../../todo-update-me-later)  
  * **Query**: Query documents in a collection or in an index. See [Query Documents](../../../../todo-update-me-later)  
  * **Conflicts**: View and resolve conflicting documents. See [Conflicts](../../../../todo-update-me-later)  
{PANEL/}

---

{PANEL}
Click `Display` to:  

* Select which columns to view
* Reorder columns viewed
* Add a custom column
<br/>

![Figure 3. Manage Displayed Columns](images/documents-and-collections-3.png "Manage Displayed Columns")
{PANEL/}
