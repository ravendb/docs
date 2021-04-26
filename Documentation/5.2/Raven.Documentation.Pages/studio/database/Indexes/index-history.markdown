# Index History
---

{NOTE: }

* You can open an **Index History** dialog for any index, browse the history of 
  changes made in this index over time and revert the currently active index to any 
  of its past revisions.  

* When an index is reverted to one of its past revisions, the revision can be loaded 
  along with its index [field options](../../../studio/database/indexes/create-map-index#index-field-options) 
  and all other settings.  
  Alternatively, you can revert only the index's Map and Reduce sections to those of 
  the past revision and leave its other settings unchanged.  

* In this page:  
  * [Open Index History Dialog](../../../studio/database/indexes/index-history#open-index-history-dialog)  
  * [Index History Dialog](../../../studio/database/indexes/index-history#index-history-dialog)  
  * [Select and Load an Index Revision](../../../studio/database/indexes/index-history#select-and-load-an-index-revision)  
{NOTE/}

---

{PANEL: Open Index History Dialog}

![Figure 1. Indexes List](images/index-history-01-indexes-list.png "Figure 1. Indexes List")

1. **List Of Indexes**  
   Click to open the [indexes list view](../../../studio/database/indexes/indexes-list-view).  
2. **Indexes**  
   Click an index name to [view and edit the index definition](../../../studio/database/indexes/create-map-index).  
3. **Index History Button**  
   ![Figure 2. Index History Button](images/index-history-02-history-button.png "Figure 2. Index History Button")
   Click the *Index History* button to open this index's history dialog.  

{PANEL/}

{PANEL: Index History Dialog}

![Figure 3. History Dialog](images/index-history-03-history-view.png "Figure 3. History Dialog")

1. **Index History View**  
2. **Revisions List**  
   The history of changes made in this index, ordered by revision creation time.  
3. **Index Definition Preview**  
   The full definition of the highlighted index revision, including not just its Map and 
   Reduce sections but *all the fields and configurations* that determine its behavior.  


{PANEL/}

{PANEL: Select and Load an Index Revision}

![Figure 4. Index Revision Selection](images/index-history-04-index-revision-selection.png "Figure 4. Index Revision Selection")

1. **Revisions List**  
   To highlight a revision, hover over it using the mouse or use the arrow keys.  
   Highlighting a revision previews its definition.  
2. **Load Index**  
   Click *Load Index* or press the *Enter* key to load the currently highlighted revision, 
   or left-click a revision to load it without highlighting it first.  

---

![Figure 5. Load Index](images/index-history-05-load-map-and-reduce-only.png "Figure 5. Load Index")

1. **Load Index**  
   Click the Load Index button to load the selected revision in its entirety, 
   including its [field options](../../../studio/database/indexes/create-map-index#index-field-options) 
   and any additional settings.  
2. **Load Map & Reduce Only**  
   Click the arrow and choose the "Load Map & Reduce Only" dropdown to load the selected 
   revision's *Map and Reduce sections* only, leaving its other settings unchanged.  

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

