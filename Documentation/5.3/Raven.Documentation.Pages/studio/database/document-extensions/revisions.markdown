# Revisions 
---

{NOTE: }

* [Document Revisions](../../../document-extensions/revisions/overview) 
  are snapshots of documents, that can be created automatically upon 
  document creation, modification, or deletion and can be used for data 
  auditing, instant restoration after document corruption, and various 
  other uses.  
* This page describes the Document View 
  [Revisions tab](../../studio/database/document-extensions/revisions), 
  that shows each document's revisions trail and can be used to inspect, 
  administer, and create revisions.  
* Find [here](../../../document-extensions/revisions/overview#how-it-works) 
  a dynamic walkthrough that demonstrates revisions management.  

* In this page:
  * [Revisions Tab](../../../studio/database/document-extensions/revisions#revisions-tab)
  * [Revision Inspection](../../../studio/database/document-extensions/revisions#revision-inspection)
  * [Revisions Comparison](../../../studio/database/document-extensions/revisions#revisions-comparison)

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

## Related Articles

### Client API

- [Session: What are Revisions](../../document-extensions/revisions/client-api/session/what-are-revisions)  
- [Session: Loading Revisions](../../document-extensions/revisions/client-api/session/loading)  
- [Operations: How to Configure Revisions](../../document-extensions/revisions/client-api/operations/configure-revisions)  
- [Revisions in Data Subscriptions](../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning)  
