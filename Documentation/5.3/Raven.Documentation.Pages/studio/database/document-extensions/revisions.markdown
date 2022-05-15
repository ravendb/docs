# Document Extensions: Revisions
---

{NOTE: }

* [Document Revisions](../../../document-extensions/revisions/overview) 
  are snapshots of documents, that can be created automatically upon 
  document creation, modification, or deletion.  
* Revisions can be used for data auditing, instant restoration after 
  document corruption, and various other uses.  
* The Studio views that this article describes are -  
   * the Document View [Revisions tab](../../../studio/database/document-extensions/revisions#revisions-tab), 
     that lists the revisions created for a document and allows to inspect 
     each revision, compare it with other revisions, and create new revisions.  
   * The [Revisions Bin](../../../studio/database/document-extensions/revisions#revisions-bin), 
     that holds the revisions of deleted documents.  
* Find [here](../../../document-extensions/revisions/overview#how-it-works) 
  a dynamic walkthrough that demonstrates revisions management.  

* In this page:
  * [Revisions Tab](../../../studio/database/document-extensions/revisions#revisions-tab)  
  * [Revision Inspection](../../../studio/database/document-extensions/revisions#revision-inspection)  
  * [Revisions Comparison](../../../studio/database/document-extensions/revisions#revisions-comparison)  
  * [Revisions Bin](../../../studio/database/document-extensions/revisions#revisions-bin)  
     * [Restoring Revisions](../../../studio/database/document-extensions/revisions#restoring-revisions)  

{NOTE/}

---

{PANEL: Revisions Tab}

A document's Revisions tab displays the full trail of revisions created for the document, 
allowing you to inspect each revision, force the creation of a new revision, and compare 
revisions to the live document version.  

![Revisions Tab](images/revisions/document-revisions.png "Revisions Tab")

1. [Document View](../../../studio/database/documents/document-view) > **Revisions tab**  
   Click to display the document's Revisions tab.  
   Revisions are listed in the Revisions tab by their creation time signatures.  
   The current number of revisions is displayed in the tab header.  
2. **Create Revision**  
   Click to create a new revision for this document.  
   A new revision will be created if the document doesn't already have a revision for the latest content.  
   [Learn here](../../../document-extensions/revisions/overview#force-revision-creation) 
   about forcing the creation of a new revision via Studio or the API.  
3. **A Revision**  
   Click to inspect this revision.  
4. **Compare Revision**  
   Click to compare this revision with the current live version of this document.  

{PANEL/}

{PANEL: Revision Inspection}

![Click to Inspect](images/revisions/click-revision.png "Click to Inspect")

* **Click a revision time signature** to inspect the revision's contents.  

---

![Revision Inspection](images/revisions/revision-inspection.png "Revision Inspection")

1. **Revision**  
   The REVISION label indicates that you are viewing a revision and not 
   the parent document.  
2. **Clone**  
   Click to create a document that copies the revision's contents.  
   {INFO: }
   Cloning the revision will open the 'new document view' with this revision's contents.  
   You can then save the clone under a new name to create a new document.  
   Saving the clone with the exact same ID as the revision's parent document will revert the document to this revision.  
   {INFO/}
3. **See the current document**  
   Click to return to the revision's parent document view.  
4. **Revision Inspection Area**  
   The revision's content, displayed in read-only mode.  

{PANEL/}


{PANEL: Revisions Comparison}

![Click to Compare](images/revisions/click-to-compare.png "Click to Compare")

* Click a revision's Comparison button to compare it with other revisions.  

---

![Revision Comparison](images/revisions/revision-comparison.png "Revision Comparison")

1. **Exit revisions compare mode**  
   Click to exit the comparison window and return to the Revisions tab in the Document View.  
2. **Compare with**  
   Click to select a revision to compare with.  
   ![Compare-with Drop List](images/revisions/compare-with-drop-list.png "Compare-with Drop List")
3. **Left-hand revision**  
   The left-hand view displays the revision selected in the Revisions tab.  
4. **Right-hand revision**  
  The right-hand view displays the revision selected using the "Compare with" drop-down list.  
5. **Summary line**  
  This line summarizes the differences found between the two revisions.  

{PANEL/}

{PANEL: Revisions Bin}

When the Revisions feature is **Enabled** for a document's collection, and the document is deleted:  

* A delete-revision will be created for the document, marking its deletion.  
* The delete-revision and all the document's revisions, both Automatically-Created 
  and Manually-Created, will be moved to the Revisions Bin.  

When the Revisions feature is **Disabled** for a document's collection, and the document is deleted:  

* The document and all its revisions will be deleted irrevocably.  


![Revisions Bin](images/revisions/revisions-bin.png "Revisions Bin")

1. **Revisions Bin**  
   Click to open the Revisions Bin view.  
   Each item listed in the Revisions Bin view references all the revisions created for a deleted document.  
2. **Selection Boxes**  
   Check to select items.  
3. **Delete**  
   Click to remove selected items.  
   Deleting revisions from the revisions bin will dispose of these revisions irrevocably.  
4. **Deleted Document ID**  
   This is the ID of the document that was deleted.  
   Click it to inspect the revisions created for this document (aka "orphaned revisions" 
   because their parent document has been deleted).  
   ![Orphaned Revisions](images/revisions/orphaned-revisions.png "Orphaned Revisions")
     * Revisions stored in the revisions bin can be 
       [inspected](../../../studio/database/document-extensions/revisions#revision-inspection) 
       and cloned just like the revisions of a live document.  
5. **Change Vector**  
   The change vector of the revision that was created for the document when it was deleted.  
6. **Deletion Date**  
   The date/time when the document was deleted.  

---

### Restoring Revisions

Giving a **new document** the ID of a deleted document whose revisions are 
kept in the Revisions Bin, will restore the revisions from the bin and add 
them to the new document.  

Opening the document's Revisions tab will display the whole audit trail, 
including the delete-revision created when the old document was deleted 
and the revision created when the new document was created.  

![Restored Revisions](images/revisions/restored-revisions.png "Restored Revisions")

{PANEL/}

## Related Articles

### Document Extensions

* [Document Revisions Overview](../../../document-extensions/revisions/overview)  
* [Revert Revisions](../../../document-extensions/revisions/revert-revisions)  
* [Revisions and Other Features](../../../document-extensions/revisions/revisions-and-other-features)  

### Client API

* [Revisions: API Overview](../../../document-extensions/revisions/client-api/overview)  
* [Operations: Configuring Revisions](../../../document-extensions/revisions/client-api/operations/configure-revisions)  
* [Session: Loading Revisions](../../../document-extensions/revisions/client-api/session/loading)  
* [Session: Including Revisions](../../../document-extensions/revisions/client-api/session/including)  
* [Session: Counting Revisions](../../../document-extensions/revisions/client-api/session/counting)  

### Studio

* [Settings: Document Revisions](../../../studio/database/settings/document-revisions)  
