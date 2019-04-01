# Indexes: Indexes View

This view lists all available indexes in a current database and gives you the ability to create, edit, query, or delete indexes. Indexes are grouped by a collection they are working on and each index contains basic information, such as a number of indexed entries and locked state.

{PANEL:Action Bar}

Action Bar in this View contains the following capabilities:

- `New Index` - redirects to [Index Edit View](../../../studio/overview/indexes/index-edit-view) where you can create a new index,
- `Query` - redirects to [Query View](../../../studio/overview/query/query-view),
- `Collapse All` - collapses all indexes,
- `Paste` - quickly creates index by pasting [IndexDefinition](../../../glossary/index-definition),
- `Index Merge Suggestions` - retrieves all suggestions for an index merging,
- `Delete` - removes idle/disabled/abandoned/all indexes

![Figure 0. Studio. Indexes View. Action Bar.](images/indexes-view-action-bar.png)  

{PANEL/}

{PANEL:Querying}

After clicking on an index name you will be navigated to the `Query View`. To read more about querying in Studio, please visit [this](../../../studio/overview/query/query-view) article.

![Figure 1. Studio. Indexes View. Query.](images/indexes-view-query.png)  

{PANEL/}

{PANEL:Editing}

To edit index just press the `Edit` button available for each index. This will navigate you to the `Index Edit View` about which you can read [here](../../../studio/overview/indexes/index-edit-view).

![Figure 2. Studio. Indexes View. Edit.](images/indexes-view-edit.png)  

{PANEL/}

{PANEL:Menu}


The following menu actions are available in the index menu:


- `Copy index`,
- `Delete index`,
- `Reset Index`,
- `Unlocked` / `Locked` / `Locked (Error)`

![Figure 4. Studio. Indexes View. Menu.](images/indexes-view-menu-2.png)    

<hr />

### Copying

When the `Copy index` button is pressed, the index definition is loaded from a server and a popup from which it can be copied appears. Later on this definition can be used to create a new index with the `Paste` action from  the `Action Bar` (remember to change an index name).

![Figure 5. Studio. Indexes View. Menu. Copying.](images/indexes-view-menu-copy.png)  

<hr />

### Deleting

If you want to delete an index (with the indexing data) press the `Delete index` button in the menu.

![Figure 7. Studio. Indexes View. Menu. Deleting](images/indexes-view-menu-delete.png)  

{DANGER This operation cannot be undone. /}

<hr />

### Resetting

Resetting an index will **remove all indexing data and start indexing from scratch**. To reset an index press the `Reset index` button in the menu.

![Figure 6. Studio. Indexes View. Menu. Resetting](images/indexes-view-menu-reset.png)  

{DANGER This operation cannot be undone. /}

<hr />

### Locking

You can read more about locking [here](../../../server/administration/index-administration#index-locking).

{PANEL/}

