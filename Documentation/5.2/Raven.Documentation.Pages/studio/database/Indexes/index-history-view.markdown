# Index History View
---

{NOTE: }

* The **Index History View** allows you to browse the history of changes made 
  in indexes over time. You can load any past version of an index to reinstate 
  it as the current active index.  

* A past index version can be reinstated along with its [field options](../../../studio/database/indexes/create-map-index#index-field-options) 
  and any other settings.  
  Alternatively, you can load only the version's Map and Reduce sections and leave 
  other settings of the currently active index unchanged.  

* In this page:  
  * [Open the Index History View](../../../studio/database/indexes/index-history-view#open-the-index-history-view)  
  * [Select and Load an Index Version](../../../studio/database/indexes/index-history-view#select-and-load-an-index-version)  
{NOTE/}

---

{PANEL: Open the Index History View}

![Figure 1. Indexes List](images/index-history-01-indexes-list.png "Figure 1. Indexes List")

1. **List Of Indexes**  
   Click to open the [indexes list view](../../../studio/database/indexes/indexes-list-view).  
2. **Indexes**  
   Click an index name to [view and edit the index definition](../../../studio/database/indexes/create-map-index) 
   or *open its history view*.  

---

![Figure 2. Index History Button](images/index-history-02-history-button.png "Figure 2. Index History Button")

* Click the *Index History* button to open this index's history view.  

---

![Figure 3. History View](images/index-history-03-history-view.png "Figure 3. History View")

1. **Index History View**  
2. **Versions List**  
   The history of changes made in this index, ordered by version creation time.  
3. **Index Definition Preview**  
   The full definition of the highlighted index, including not just its Map and 
   Reduce sections but *all the fields and configurations* that determine its behavior.  


{PANEL/}

{PANEL: Select and Load an Index Version}

![Figure 4. Index Version Selection](images/index-history-04-index-version-selection.png "Figure 4. Index Version Selection")

1. **Versions List**  
   To highlight a version, hover over it using the mouse or use the arrow keys.  
   Highlighting a version displays its contents in the Index Definition Preview box.  
2. **Load Index**  
   Click *Load Index* or press the *Enter* key to load the currently highlighted version, 
   or left-click a version to load it without highlighting it first.  

---

![Figure 5. Load Index](images/index-history-05-load-map-and-reduce-only.png "Figure 5. Load Index")

1. **Load Index**  
   Click the Load Index button to load the selected version in its entirety, 
   including its [field options](../../../studio/database/indexes/create-map-index#index-field-options) 
   and any additional settings.  
2. **Load Map & Reduce Only**  
   Click the arrow and choose the "Load Map & Reduce Only" popup to load the selected 
   version's *Map and Reduce sections* only, leaving its other settings unchanged.  

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

