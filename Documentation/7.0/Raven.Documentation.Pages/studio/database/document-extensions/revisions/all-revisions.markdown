# Document Extensions: All Revisions
---

{NOTE: }

* The **All Revisions** view allows you to list the 
  [revisions](../../../../studio/database/document-extensions/revisions/revisions) 
  that were created over time for all your documents.  
* You can list **all** available revisions, or filter the revisions -  
   - By **collection**  
   - By **type**, selecting either revisions that indicate the **creation 
     or modification** of documents or revisions that indicate that documents 
     were **deleted**.  
* All Revisions is a Studio-only feature, there is no available API for these operations.  

---

* In this page:
  * [Opening All Revisions](../../../../studio/database/document-extensions/revisions/all-revisions#opening-all-revisions)  
  * [Using All Revisions](../../../../studio/database/document-extensions/revisions/all-revisions#using-all-revisions)  
  
{NOTE/}

---

{PANEL: All Revisions}

### Opening All Revisions

![Open All Revisions](images/revisions/all-revisions_click-to-open-view.png "Open All Revisions")

1. **Documents**  
   Click to open the Documents view.  
2. **All Revisions**  
   Click to open All Revisions.  

---

### Using All Revisions

![All Revisions](images/revisions/all-revisions.png "All Revisions")

1. **Remove revisions**  
   Click to remove selected revisions (see below).  
   {WARNING: }
    Please **be aware** that removing revisions is an **irrevocable operation**.  
   {WARNING/}

2. **Info Hub**  
   Click for information about this view.  
3. **Filter by Collection**  
   Click to select the collection whose revisions you want to list and manage.  

      ![Filter by Collection](images/revisions/all-revisions_select-collection.png "Filter by Collection")

      To list revisions for **all** collection, leave the selection box blank 
      or de-select the currently selected collection.  

4. **Filter by Type**  
    * Filter by **All** to list **all** document revisions for the selected collection/s.  
    * Filter by **Regular** to list revisions that indicate the **creation** or **modification** of documents.  
    
         ![Regular revisions](images/revisions/all-revisions_regular.png "Regular revisions")
      
    * Filter by **Deleted** to list revisions that indicate the **deletion** of documents.  
    
         ![Deleted revisions](images/revisions/all-revisions_deleted.png "Deleted revisions")
      
5. **Revision Selection Column**  
   Select revisions that you want to remove.  
   To remove the selected revisions, click the **Remove revisions** buttin (see above).  
6. **Revision Data**  
   Browse for revisions' **ID**, **Collection**, **ETag**, 
   **Change Vector**, **Last Modification** data, and **Flags**.  
    - Click a revision's **ID** to visit this revision in the 
      [Revisions tab](../../../../studio/database/document-extensions/revisions/revisions).  
    - Click a revision's **Collection name** to visit this 
      [collection](../../../../studio/database/documents/documents-and-collections).  
     
{PANEL/}


## Related Articles

### Document Extensions

* [Document Revisions Overview](../../../../document-extensions/revisions/overview)  
* [Revert Revisions](../../../../document-extensions/revisions/revert-revisions)  
* [Revisions and Other Features](../../../../document-extensions/revisions/revisions-and-other-features)  

### Client API

* [Revisions: API Overview](../../../../document-extensions/revisions/client-api/overview)  
* [Configuring Revisions](../../../../document-extensions/revisions/client-api/operations/configure-revisions)  
* [Session: Loading Revisions](../../../../document-extensions/revisions/client-api/session/loading)  
* [Session: Including Revisions](../../../../document-extensions/revisions/client-api/session/including)  
* [Session: Counting Revisions](../../../../document-extensions/revisions/client-api/session/counting)  

### Studio

* [Revisions](../../../../studio/database/document-extensions/revisions/revisions)  
* [Settings: Document Revisions](../../../../studio/database/settings/document-revisions)  
