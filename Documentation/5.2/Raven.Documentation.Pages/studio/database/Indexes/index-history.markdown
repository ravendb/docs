# Index History
---

{NOTE: }

* You can open an **Index History** dialog for any index, browse the history of 
  changes made in this index over time, and revert the currently active index to any 
  of its past revisions.  

* When an index is reverted to one of its past revisions, the revision can be loaded 
  along with its index [field options](../../../studio/database/indexes/create-map-index#index-field-options) 
  and all other settings.  
  Alternatively, you can revert only the index's Map and Reduce sections to those of 
  the selected revision and leave its other settings unchanged.  

* The number of revisions RavenDB keeps for each index can be updated.  

* In this page:  
  * [Open Index History Dialog](../../../studio/database/indexes/index-history#open-index-history-dialog)  
  * [Index History Dialog](../../../studio/database/indexes/index-history#index-history-dialog)  
  * [Select and Load an Index Revision](../../../studio/database/indexes/index-history#select-and-load-an-index-revision)  
  * [View and Update Number of Index Revisions](../../../studio/database/indexes/index-history#view-and-update-number-of-index-revisions)  

{NOTE/}

---

{PANEL: Open Index History Dialog}

To open the index history dialog:  

1. Open the **Indexes** > **List of Indexes** [view](../../../studio/database/indexes/indexes-list-view).  
   ![Figure 1. List Of Indexes](images/index-history-01-list-of-indexes.png "Figure 1. List Of Indexes")
2. Select an Index.  
   ![Figure 2. Select Index](images/index-history-02-select-index.png "Figure 2. Select Index")
3. Open the Index History dialog.  
   ![Figure 3. Index History Button](images/index-history-03-index-history-button.png "Figure 3. Index History Button")

{PANEL/}

{PANEL: Index History Dialog}

![Figure 4. History Dialog](images/index-history-04-history-view.png "Figure 4. History Dialog")

1. **Index History View**  
2. **Revisions List**  
   The history of changes made in this index, ordered by revision creation time.  
3. **Index Definition Preview**  
   The full definition of the highlighted revision, including not just its Map and 
   Reduce sections but also *all the field options and other settings* that determine 
   its behavior.  

{PANEL/}

{PANEL: Select and Load an Index Revision}

![Figure 5. Index Revision Selection](images/index-history-05-index-revision-selection.png "Figure 5. Index Revision Selection")

1. **Revisions List**  
   To preview a revision, hover over its name or select it using the arrow keys.  
2. **Load Index**  
   Click the Load Index button to load the previewed revision,  
   or click a revision name to load it without preview.  

---

![Figure 6. Load Index](images/index-history-06-load-map-and-reduce-only.png "Figure 6. Load Index")

1. **Load Index**  
   Click the Load Index button to load the previewed revision in its entirety, 
   including its [field options](../../../studio/database/indexes/create-map-index#index-field-options) 
   and any additional settings.  
2. **Load Map & Reduce Only**  
   Click the arrow and choose the "Load Map & Reduce Only" dropdown to load the selected 
   revision's *Map and Reduce sections* only, leaving its other settings unchanged.  

{PANEL/}

{PANEL: View and Update Number of Index Revisions}

To find how many index revisions RavenDB keeps, and optionally 
change this number, use the *Database Settings* view.  

1. Open the **Settings** > **Database Settings** view.  
   ![Figure 7. Open Database Settings View](images/index-history-07-open-database-settings-view.png "Figure 7. Open Database Settings View")
2. Enter **Indexing.History.NumberOfRevisions** in the search box to locate the 
   index revisions number *Configuration Key* and see its current value.  
   ![Figure 8. Configuration Key and Value](images/index-history-08-configuration-key-and-value.png "Figure 8. Configuration Key and Value")
3. Click **Edit** to change the configuration key value.  
   ![Figure 9. Edit Value Key](images/index-history-09-edit-value-key.png "Figure 9. Edit Value Key")
4. Edit the configuration key value and apply your changes.  
   ![Figure 10. Edit Value](images/index-history-10-edit-value.png "Figure 10. Edit Value")

{PANEL/}


## Related Articles

### Indexes

- [Map Indexes](../../../indexes/map-indexes)
- [Multi-Map Indexes](../../../indexes/multi-map-indexes)
- [Map-Reduce Indexes](../../../indexes/map-reduce-indexes)

### Studio

- [Indexes Overview](../../../studio/database/indexes/indexes-overview)
- [Indexes List View](../../../studio/database/indexes/indexes-list-view)
- [Create Multi-Map Index](../../../studio/database/indexes/create-multi-map-index)
- [Create Map-Reduce Index](../../../studio/database/indexes/create-map-reduce-index)

