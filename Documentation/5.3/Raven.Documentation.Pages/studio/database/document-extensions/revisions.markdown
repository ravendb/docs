# Revisions 
---

{NOTE: }

* [Document Revisions](../../../document-extensions/revisions/overview) 
  are snapshots of documents, that can be created automatically upon 
  document creation, modification, or deletion and can be used for data 
  auditing, instant restoration after document corruption, and various 
  other uses.  
* This page describes -  
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
     * [Returning Revisions From The Revisions Bin](../../../studio/database/document-extensions/revisions#returning-revisions-from-the-revisions-bin)  

{NOTE/}

---

{PANEL: Revisions Tab}

A document's Revisions tab displays the full trail of revisions created for the document, 
allowing you to inspect each revision, force the creation of a new revision, and compare 
revisions to the live document version.  

![Revisions Tab](images/revisions/document-revisions.png "Revisions Tab")

1. [document view](../../../studio/database/documents/document-view) > **Revisions tab**  
   Click to display the document's Revisions tab.  
   Revisions are listed in the Revisions tab by their creation time signatures.  
2. **Create Revision**  
   Click to create a new revision for this document.  
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

1. **Clone**  
   Click to create a document that replicates the revision's contents.  
   Cloning the revision using the same ID as its parent document will revert the document to this revision.  
2. **See the current document**  
   Click to return to the revision's parent document view.  
3. **Revision Inspection Box**  
   The revision is displayed in read-only mode.  

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
  The right-hand view displays the revision selected using the "Compare with" drop down list.  
5. **Summary line**  
  This line summarizes the differences found between the two revisions.  

{PANEL/}

{PANEL: Revisions Bin}

Deleting a document that the Revisions feature is enabled for will create 
a new revision for the document and move all its revisions to the revisions bin.  

![Revisions Bin](images/revisions/revisions-bin.png "Revisions Bin")

1. **Revisions Bin**  
   Click to open the Revisions Bin view.  
   Each item listed in the Revisions Bin view references all the revisions created for a deleted document.  
2. **Delete**  
   Click to remove selected items.  
   Deleting revisions from the revisions bin will dispose of these revisions irrevocably.  
3. **Selection Boxes**  
   Check to select items.  
4. **Deleted Document ID**  
   Click to inspect the revisions created for this document (aka "orphaned revisions" 
   because their parent document has been deleted).  
   ![Orphaned Revisions](images/revisions/orphaned-revisions.png "Orphaned Revisions")
     * Revisions stored in the revisions bin can be 
       [inspected](../../../studio/database/document-extensions/revisions#revision-inspection) 
       and cloned just like the revisions of a live document.  
5. **Deletion Revision Change Vector**  
   Change vector of the revision that was created for the document when it was deleted.  
6. **Deletion Date**  

---

### Returning Revisions From The Revisions Bin

Giving a **new document** the ID of a deleted document whose revisions are 
in the revisions bin, will remove the revisions from the bin and add them to 
the new document.  
The revision that was created when the document was deleted, will mark 
the deletion event in the document history.  
![New document with revisions added from Revisions Bin](images/revisions/returned-revisions.png "New document with revisions added from Revisions Bin")

{PANEL/}

## Related Articles

### Client API

- [Session: What are Revisions](../../document-extensions/revisions/client-api/session/what-are-revisions)  
- [Session: Loading Revisions](../../document-extensions/revisions/client-api/session/loading)  
- [Operations: How to Configure Revisions](../../document-extensions/revisions/client-api/operations/configure-revisions)  
- [Revisions in Data Subscriptions](../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning)  
