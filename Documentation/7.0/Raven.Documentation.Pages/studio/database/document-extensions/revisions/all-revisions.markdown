# The All Revisions View
---

{NOTE: }

**Displaying revisions**:  

* The **All Revisions** view displays all [revisions](../../../../studio/database/document-extensions/revisions/revisions) created over time for all your documents in the database.  
  This is a Studio-only feature; there is no Client API method to retrieve all revisions from the server.
  
* You can view all existing revisions or filter them by their **Collection** or **Type**.  
  Revision types:   
  * **"Regular Revision"** - A revision that is created when the document is created or modified.
  * **"Delete Revision"** - A revision that is created when the document is deleted.

---

**Deleting revisions**:  

* From this view, you can manually delete selected revisions ("Regular Revisions" only).  
  Other available deletion methods are summarized in [All ways to delete revisions](../../../../studio/database/document-extensions/revisions/all-revisions#all-ways-to-delete-revisions).

* Removing revisions from a secure server requires a client certificate with an
  [Admin](../../../../server/security/authorization/security-clearance-and-permissions#section) or higher security clearance.

     {WARNING: }
     * Exercise caution when deleting revisions, as this is a permanent action and cannot be undone.  
       Once a revision is removed, it will no longer be available and will Not be listed in this view.
     {WARNING/}

---

* In this page:
  * [Opening the All Revisions view](../../../../studio/database/document-extensions/revisions/all-revisions#opening-the-all-revisions-view)  
  * [Using the All Revisions view](../../../../studio/database/document-extensions/revisions/all-revisions#using-the-all-revisions-view)
  * [All ways to delete revisions](../../../../studio/database/document-extensions/revisions/all-revisions#all-ways-to-delete-revisions)
  
{NOTE/}

---

{PANEL: Opening the All Revisions view}

To access the All Revisions view, go to **Documents > All Revisions**

![Open All Revisions](images/revisions/all-revisions_click-to-open-view.png "Open All Revisions")

1. **Documents**  
   Select the _Documents_ section
2. **All Revisions**  
   Click to open the _All Revisions_ view

{PANEL/}

{PANEL: Using the All Revisions view}

![All Revisions](images/revisions/all-revisions.png "All Revisions")

1. **Filter by Collection**  
   Click to select the collection whose revisions you want to list and manage.  
   To list revisions for ALL collections, leave the selection box blank or deselect the currently selected collection.

    ![Filter by Collection](images/revisions/all-revisions_select-collection.png "Filtering by the Orders collection")

2. **Filter by Type**  
    * Select **All** to list all document revisions for the selected collection or for all collections.  
    * Select **Regular** to list revisions indicating the creation or modification of documents (for the selected collection/s).
    * Select **Deleted** to list revisions indicating the deletion of documents (for the selected collection/s).

    ![Regular revisions](images/revisions/all-revisions_regular.png '"Regular revisions"')

    ![Deleted revisions](images/revisions/all-revisions_deleted.png '"Delete revisions"')

3. **Revision Data**  
   The following details are listed for each revision:  
   * Id - The ID of the parent document for this revision. Click to view the content of this revision.
   * Collection - The collection this revision belongs to. Click to navigate to the [documents view](../../../../studio/database/documents/documents-and-collections#the-documents-view) of this collection.
   * Etag - A unique identifier for this revision, used for internal tracking within the database.
   * Change Vector - Tracks the document’s version across the cluster for replication and conflict resolution.
   * Last Modified - The time this revision was created.
   * Flags - Flags indicate whether a revision contains counters, time series, attachments, or is archived.  
     The "DeleteRevision" flag indicates that the revision is a "Delete Revision".

4. **Select revisions**   
   Check the boxes for the revisions you want to remove.  
   Only "Regular revisions" indicating the creation or modification of documents can be deleted from this view.

5. **Remove Revisions**  
   Click the "Remove revisions" button to delete selected revisions.

{PANEL/}

{PANEL: All ways to delete revisions}

* Revisions can be categorized as follows:
    * **"Regular Revision"** - A revision that is created when the document is created or modified.
    * **"Delete Revision"** - A revision that is created when the document is deleted.

* The sections below outline the deletion options available for each type.

    {WARNING: }
    * Exercise caution when deleting revisions, as this is a **permanent** action and **cannot** be undone.
    * Removing revisions from a secure server requires a client certificate with an
      [Admin](../../../../server/security/authorization/security-clearance-and-permissions#section) or higher security clearance.
    {WARNING/}

---

{CONTENT-FRAME: }

**Deleting "Regular Revisions"**:

* "Regular Revisions" can be manually deleted from the All Revisions view.   
  See [using the All Revisions view](../../../../studio/database/document-extensions/revisions/all-revisions#using-the-all-revisions-view).

* These revisions can also be deleted through the [Delete revisions operation](../../../../document-extensions/revisions/client-api/operations/delete-revisions) in the Client API.

* Individual revisions can also be deleted when viewing the revision content.  
  See [Inspecting the revision](../../../../studio/database/document-extensions/revisions/revisions#revision-inspection).

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Deleting "Delete Revisions"**:

* "Delete Revisions" can be removed via:
    * The [Revisions Bin view](../../../../studio/database/document-extensions/revisions/revisions-bin) - manual deletion
    * The [Revisions Bin Cleaner](../../../../document-extensions/revisions/revisions-bin-cleaner) task - automatic deletion at a defined frequency

* Removing a "Delete Revision" will delete ALL revisions associated with the deleted document.

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Configuration-based revision removal**:

* Revisions may be removed based on the configured revisions setting,  
  e.g., limiting the number of revisions to keep or setting an expiration based on age.  
  See [Default revision configuration](../../../../studio/database/settings/document-revisions#define-default-configuration) and
  [Collection-specific configuration](../../../../studio/database/settings/document-revisions#define-collection-specific-configuration).

* Revisions may also be deleted as a result of enforcing the current revision configuration.  
  Learn more in [Enforce configuration](../../../../studio/database/settings/document-revisions#enforce-configuration).

{CONTENT-FRAME/}
{PANEL/}

## Related Articles

### Document Extensions

* [Document Revisions Overview](../../../../document-extensions/revisions/overview)  
* [Revert Revisions](../../../../document-extensions/revisions/revert-revisions)  
* [Revisions and Other Features](../../../../document-extensions/revisions/revisions-and-other-features)  
* [Revisions Bin Cleaner](../../../../document-extensions/revisions/revisions-bin-cleaner)  

### Client API

* [Revisions: API Overview](../../../../document-extensions/revisions/client-api/overview)  
* [Configuring Revisions](../../../../document-extensions/revisions/client-api/operations/configure-revisions)  
* [Session: Loading Revisions](../../../../document-extensions/revisions/client-api/session/loading)  
* [Session: Including Revisions](../../../../document-extensions/revisions/client-api/session/including)  
* [Session: Counting Revisions](../../../../document-extensions/revisions/client-api/session/counting)  

### Studio

* [Revisions](../../../../studio/database/document-extensions/revisions/revisions)  
* [Revisions Bin](../../../../studio/database/document-extensions/revisions/revisions-bin)  
* [Settings: Document Revisions](../../../../studio/database/settings/document-revisions)  
