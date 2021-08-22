# Documents: Patch View
---

{NOTE: }

* Use Studio's **Patch View** to apply an update to a large number of documents in a single operation.  
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
   Click to choose the database you want to patch.  
3. **Syntax Samples**  
   Click to display patching [syntax samples](../../../client-api/operations/patching/set-based#examples).  
4. **Patch Box**  
   Enter your patch in the patch box.  
5. **Apply Patch**  
   Click to apply the patch to your documents (see [Apply Patch](../../../studio/database/documents/patch-view#apply-patch) below).  
6. **Test Patch**  
   Click to test your patch without actually applying it to your documents (see [Test Patch](../../../studio/database/documents/patch-view#test-patch) below).  
{PANEL/}

{PANEL: Patch Configuration}

![Figure 2. Patch Configuration](images/patch-view-2.png "Figure 2. Patch Configuration")

1. **Save Patch**  
   Save the patch currently displayed in the patch box under a name of your choice.  
   ![Save Patch](images/patch-view-save-patch.png "Save Patch")  
   Enter a name in the text box, and click the **Save** button again to store the patch.  
2. **Load Patch**  
   Load a stored patch.  
   ![Load Patch](images/patch-view-load-patch.png "Load Patch")  
   Hover over a patch name to display its preview.  
   Click a patch name or the preview **Load** button to load the patch.  
3. **Patch Settings**  
   ![Patch Settings](images/patch-view-settings.png "Patch Settings")  
    * a. **Patch Immediately**  
         When this option is selected, clicking the **Apply Patch** button applies 
         the patch immediately.  
    * b. **Define Timeout**  
         When this option is selected, Clicking the **Apply Patch** button 
         applies the patch only after the index used by the patch has become 
         non-stale.  
         If the index remains stale for the duration of the set timeout, 
         the operation is reverted and an exception is thrown.  
         ![Patch Timeout](images/patch-view-timeout.png "Patch Timeout")  
    * c. **Limit Number of Operations**  
         Toggle between limiting the max number of patch operations per second, 
         and allowing an unlimited number of patch operations per second.  
         ![Patch Operations Limit](images/patch-view-operations-limit.png "Patch Operations Limit")  

{PANEL/}

{PANEL: Apply Patch}

![Run Patch](images/patch-view-apply-patch.png "Run Patch")  

While the patch is executed, a runtime dialog presents -  

1. **Time Elapsed**  
   Time since the operation began  
2. **Index**  
   Index used by the patch  
3. **Processed**  
   Number of documents already processed / Number of documents yet to be processed  
4. **Processing Speed**  
   Number of documents processed per sec  
5. **Estimated time left**  
6. **Query**  
   Patch's query and update operation  
7. **Procession Bar**  
8. **Close**  
   Click to **close this view** (the operation will proceed)  
9. **Abort**  
   Click to **abort the operation** (the operation will be reverted)  

{PANEL/}

{PANEL: Test Patch}

![Test Patch](images/patch-view-test-patch.png "Test Patch")  

1. **Document**  
    Enter the ID of a document that the patch is meant to update, and click **Load Document**.  
    The test will patch a copy of the document, leaving the original document unharmed.  
2. **Test**  
   Click to apply the patch to the document.  
3. **Done**  
   Click to close the test view and return to the patch view.  
4. **Before**  
   Browse the document before the patch has been applied to it.  
   Click Shift+F11 to expand or collapse an enlarged view of the document.  
5. **After**  
   Browse the document after the patch has been applied to it.  
   See line 198 in the "after" view depicted above for an example of a text the patch updated.  
   Click Shift+F11 to expand or collapse an enlarged view of the document.  

{PANEL/}

## Related Articles

### Patching
- [Patching: Perform Set Based Operations on Documents](../../../client-api/operations/patching/set-based)  
- [Patching: Perform Single Document Patch Operations](../../../client-api/operations/patching/single-document)  
- [What Are Operaions](../../../client-api/operations/what-are-operations)  

### Queries
- [RQL - Raven Query Language](../../../indexes/querying/what-is-rql)  
- [Basics](../../../indexes/querying/basics)  

### Indexes
- [Indexing Basics](../../../indexes/indexing-basics)  
