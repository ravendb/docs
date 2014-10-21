# Indexes : Indexes View

This view lists all available indexes in current database and gives you the ability to create, edit, query or delete index. Indexes are grouped by a collection they are working on and each index contains basic information such as number of indexed entries and locked state.

{PANEL:Action Bar}

Action Bar on this View contains following capabilities:

- `New Index` - redirects to [Index Edit View]() where you can create new index,
- `Query` - redirects to [Query View](),
- `Collapse` - collapses all indexes,
- `Paste` - create quickly index by pasting [IndexDefinition](),
- `Delete` - remove idle/disabled/abandoned/all indexes

![Figure 0. Studio. Indexes View. Action Bar.](images/indexes-view-action-bar.png)  

{PANEL/}

{PANEL:Querying}

After clicking on index name you will be navigated to `Query View`. To read more about querying in Studio, please visit [this]() article.

![Figure 1. Studio. Indexes View. Query.](images/indexes-view-query.png)  

{PANEL/}

{PANEL:Editing}

To edit index just press on appropriate `Edit` button available for each index. This will navigate you to the `Index Edit View` about which you can read [here]().

![Figure 2. Studio. Indexes View. Edit.](images/indexes-view-edit.png)  

{PANEL/}

{PANEL:Menu}

Following menu actions are available in index menu:

![Figure 4. Studio. Indexes View. Menu.](images/indexes-view-menu-2.png)    

<hr />

### Copying

When `Copy index` is pressed the index definition will be loaded from server and popup will appear from which it can be copied. Later on definition can be used to create new index by using the `Paste` action from `Action Bar` (remember to change index name).

![Figure 5. Studio. Indexes View. Menu. Copying.](images/indexes-view-menu-copy.png)  

<hr />

### Deleting

If you want to delete index (with indexing data) press the `Delete index` from the menu.

![Figure 7. Studio. Indexes View. Menu. Deleting](images/indexes-view-menu-delete.png)  

{DANGER This operation cannot be undone. /}

<hr />

### Resetting

Resetting index will **remove all indexing data and start indexing from scratch**, to reset index press the `Reset index` from the menu.

![Figure 6. Studio. Indexes View. Menu. Resetting](images/indexes-view-menu-reset.png)  

{DANGER This operation cannot be undone. /}

<hr />

### Locking

You can read more about locking [here](../../../server/administration/index-administration#index-locking).

{PANEL/}

