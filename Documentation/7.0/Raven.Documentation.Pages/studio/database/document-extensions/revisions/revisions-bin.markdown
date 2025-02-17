# The Revisions Bin View
---

{NOTE: }

* The **Revisions Bin** stores revisions of deleted documents, ensuring they remain accessible.  
  From this view, you can access these revisions or permanently delete them if needed.

* In this page:
  * [Overview](../../../../studio/database/document-extensions/revisions/revisions-bin#overview)
  * [The Revisions Bin view](../../../../studio/database/document-extensions/revisions/revisions-bin#the-revisions-bin-view)
  * [Restoring revisions](../../../../studio/database/document-extensions/revisions/revisions-bin#restoring-revisions)

{NOTE/}

---

{PANEL: Overview}

**What is the Revisions Bin**:

* In RavenDB, revisions can be created automatically or manually, as explained in this [overview](../../../../document-extensions/revisions/overview).

* Regardless of how revisions are created or whether [revisions configuration](../../../../studio/database/settings/document-revisions#revisions-configuration) is enabled,
  deleting a document that **has** revisions will always create a `"Delete Revision"`.
  If you delete a document that does Not have revisions, a "Delete Revision" will be created only if the revisions configuration is set and enabled.

* This "Delete Revision" marks the document's deletion and is stored in the **Revisions Bin**.  
  In addition, all the revisions that were created over time for the deleted document are also moved to the Revisions Bin.
  Moving the revisions to the Revisions Bin ensures that historical data remains accessible.  

---

**Deleting revisions**:

* To clear up space, you can **manually** delete revisions from the Revisions Bin.  
  Removing a selected "Delete Revision" will also delete ALL its associated revisions.  
  This action is irreversible - once a revision is removed from the bin, it cannot be recovered.  

* To clean the Revisions Bin **automatically**, you can set up the [Revisions Bin Cleaner](../../../../document-extensions/revisions/revisions-bin-cleaner),  
  which deletes revisions based on a defined frequency and a specified "age to keep".

* While the Revisions Bin provides access only to a deleted document's revisions,  
  the [All Revisions](../../../../studio/database/document-extensions/revisions/all-revisions) view provides access to ALL revisions in the database,
  where they can also be deleted.

* For a complete list of deletion options, see [All ways to delete revisions](../../../../studio/database/document-extensions/revisions/all-revisions#all-ways-to-delete-revisions).

---

**Restoring revisions**:

* Revisions of a deleted document can be restored from the Revisions Bin and associated with a new document.
  Learn more [below](../../../../studio/database/document-extensions/revisions/revisions-bin#restoring-revisions).

{PANEL/}

{PANEL: The Revisions Bin view}

![Revisions bin items](images/revisions/revisions-bin-1.png "The Revisions Bin")

1. Go to **Documents > Revisions Bin**
2. Each item listed represents a "Delete Revision", marking the deletion of a document.
3. The ID of the deleted document.  
   Click the ID to view the associated revisions, which were created for the document before it was deleted.
4. The change vector of the "Delete Revision".
5. The date and time when the document was deleted.
6. Click the _Delete_ button to remove the selected "Delete Revision" items.  
   
    {WARNING: }
    When a "Delete Revision" is removed, ALL its associated revisions are permanently deleted and cannot be recovered.
    This action is irrevocable.
    {WARNING/}

---

Clicking the `orders/829-A` ID from the above list will open the following view:


![Delete revision](images/revisions/revisions-bin-2.png 'A "Delete Revision"')

1. This is the ID of the deleted document.
2. The "DELETE REVISION" label indicates that you are viewing a "Delete Revision".
3. This text appears for a "Delete Revision," displaying the time when the document was removed.  
   The flags include the `DeleteRevision` flag, confirming it is a "Delete Revision."
4. The timestamp of the "Delete Revision". 
5. These are the revisions that were created for the document before it was deleted.  
   You can click on each one to view and manage it like any other revision,  
   e.g., clone it or compare it with other revisions. 

{PANEL/}

{PANEL: Restoring revisions}

* Revisions of a deleted document can be restored and associated with a new document.  
  To restore them, create a new document with the **exact same ID** as the deleted document.  

* When a new document is created with the same ID:  
  * The revisions of the deleted document are retrieved from the Revisions Bin and associated with the new document.
  * These revisions are removed from the Revisions Bin once restored.
  * The "Delete Revision" that marked the original document's deletion will also be removed from the Revisions Bin  
    and will now appear in the new document’s revision history.

![Restoring revisions](images/revisions/revisions-bin-3.png "Restoring revisions")

1. The ID of the deleted document that was recreated.
2. The content of the newly created document.
3. This is the latest revision, indicating the creation of the new document.
4. This is the "Delete Revision" that marked the deletion of the original document.   
   This revision is now part of the new document's revision history.
6. These are the revisions of the deleted document that are now restored.

{PANEL/}

## Related Articles

### Document Extensions

* [Document Revisions Overview](../../../../document-extensions/revisions/overview)  
* [Revisions and Other Features](../../../../document-extensions/revisions/revisions-and-other-features)  
* [Revisions Bin Cleaner](../../../../document-extensions/revisions/revisions-bin-cleaner)  

### Client API

* [Revisions: API Overview](../../../../document-extensions/revisions/client-api/overview)  
* [Operations: Configuring Revisions](../../../../document-extensions/revisions/client-api/operations/configure-revisions)  
* [Session: Loading Revisions](../../../../document-extensions/revisions/client-api/session/loading)  

### Studio

* [Settings: Document Revisions](../../../../studio/database/settings/document-revisions)  
* [All Revisions](../../../../studio/database/document-extensions/revisions/all-revisions)  
