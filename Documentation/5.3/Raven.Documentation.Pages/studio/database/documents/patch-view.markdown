# Documents: Patch View
---

{NOTE: }

* Use the **Patch View** to apply an update on your documents in a single operation.  
* A patch includes a **query** that finds the documents you want to update, and an **update operation**.  
* Read more about patching [here](../../../client-api/operations/patching/set-based).  

* In this page:  
  * [Patch View](../../../studio/database/documents/patch-view#patch-view)  
  * [Patch Configuration](../../../studio/database/documents/patch-view#patch-configuration)  
  * [Apply Patch](../../../studio/database/documents/patch-view#apply-patch)  
  * [Test Patch](../../../studio/database/documents/patch-view#test-patch)  

{NOTE/}

---

{PANEL: Patch View}

![Figure 1. Patch View](images/patch-view-1.png "Figure 1. Patch View")

1. **Patch View**  
   Click to open the patch view.  
2. **Database**  
   Click to select the database you want to patch.  
3. **Syntax Samples**  
   Click to display patching [syntax samples](../../../client-api/operations/patching/set-based#examples).  
4. **Patch Box**  
   Enter your patch script in the patch box.  
5. **Apply Patch**  
   Click to apply the patch to your documents (see [Apply Patch](../../../studio/database/documents/patch-view#apply-patch) below).  
6. **Test Patch**  
   Click to test your patch without actually applying it to your documents (see [Test Patch](../../../studio/database/documents/patch-view#test-patch) below).  
{PANEL/}

{PANEL: Patch Configuration}

![Figure 2. Patch Configuration](images/patch-view-2.png "Figure 2. Patch Configuration")

1. **Save Patch**  
   Click to save the currently displayed patch in the local browser storage.  
   ![Save Patch](images/patch-view-save-patch.png "Save Patch")  
   Enter a name and click the Save button again to save the patch.  
2. **Load Patch**  
   Click to load a stored patch.  
   Recent (unsaved) patches will also be shown/listed here.  
   ![Load Patch](images/patch-view-load-patch.png "Load Patch")  
   Hover over a patch name to display its preview.  
   Click the patch name or the preview **Load** button to load the patch.  
3. <a id="patch-settings" /> **Patch Settings**  
   ![Patch Settings](images/patch-view-settings.png "Patch Settings")  

    * a. **Patch immediately**  
         When this option is selected, clicking the **Apply Patch** button applies 
         the patch immediately.  

    * b. **Define timeout to wait for index to become non-stale**  
         When this option is selected, clicking the **Apply Patch** button 
         applies the patch only after the index used by the patch has become 
         [non-stale](../../../indexes/indexing-basics#stale-indexes).  
         If the index remains stale for the duration of the set timeout, 
         the operation is canceled and an exception is thrown.  
         ![Patch Timeout](images/patch-view-timeout.png "Patch Timeout")  

    * c. **Disable creating new Auto-Indexes**  
         If toggled ON, the patch command will not create an auto-index if there is no existing index that serves the patch.  
         In that case, an exception will be thrown. 
         * Toggling this ON will not affect Auto-Index creation in future Studio patch requests.
           To disable all future Auto-Index creation from Studio patches, change the default setting in [Studio Configuration](../../../studio/database/settings/studio-configuration#disabling-auto-index-creation-on-studio-queries-or-patches)

    * d. **Limit number of operations**  
         Toggle this option ON to set the max number of patch operations per second.  
         ![Patch Operations Limit](images/patch-view-operations-limit.png "Patch Operations Limit")  

{PANEL/}

{PANEL: Apply Patch}

![Run Patch](images/patch-view-apply-patch.png "Run Patch")  

While the patch is executed, a runtime dialog presents -  

1. **Time Elapsed**  
   Time since the operation began  
2. **Index**  
   Either the index that was explicitly used by the patch, or the internal RavenDB 
   index that is used [for collection queries](../../../client-api/faq/what-is-a-collection#collection-usages).  
3. **Processed**  
   Number of documents already processed / Number of documents yet to be processed  
4. **Processing Speed**  
   Number of documents processed per sec  
5. **Estimated time left**  
6. **Query**  
   Patch's query and update operation  
7. **Progress Bar**  
8. **Close**  
   Click to **close this dialog**.  
   The operation will proceed in the background.  
   The dialog can be re-opened from Studio's Notification Center.  
9. **Abort**  
   Click to **abort the patch operation**.  
   The operation will be reverted.  

{PANEL/}

{PANEL: Test Patch}

![Open Test Dialog](images/patch-view-test-dialog.png "Open Test Dialog")  
Click **Test** to open the patch test dialog.

---

![Test Patch](images/patch-view-test-patch.png "Test Patch")  

1. **Document**  
   Enter a document ID on which to run the patch in the test mode and click Load Document.  
   The document preview will show in the 'Before' area on the left side.  
2. **Test**  
   Click 'Test' to apply the patch to the selected document.  
   A copy of the document will be patched, leaving the original document unchanged.  
   A preview of the document after the patch will show in the 'After' area on the right.  
3. **Before**  
   Browse the document before the patch has been applied to it.  
   Click Shift+F11 to expand or collapse an enlarged view of the document.  
4. **After**  
   Browse the document After the patch has been applied to it.  
   See line 198 in the "After" view depicted above for an example of a text modified by the patch.  
   Click Shift+F11 to expand or collapse an enlarged view of the document.  
5. **Done**  
   Click 'Done' to close the test view and return to the patch view.  


{PANEL/}

## Related Articles

### Patching
- [Patching: Perform Set Based Operations on Documents](../../../client-api/operations/patching/set-based)  
- [Patching: Perform Single Document Patch Operations](../../../client-api/operations/patching/single-document)  
- [What Are Operaions](../../../client-api/operations/what-are-operations)  

### Queries
- [RQL - Raven Query Language](../../../client-api/session/querying/what-is-rql)  
- [Query Overview](../../../client-api/session/querying/how-to-query)  
- [Querying an Index](../../../indexes/querying/query-index)  

### Indexes
- [Indexing Basics](../../../indexes/indexing-basics)  
