# The Collections Screen

In this screen we show a list of all available collections in the database, and documents associated with them. A **Collection** in RavenDB is a group of documents sharing the same entity name. It is not a "database table", but rather a logical way of thinking of document groups.

This is how the Collections screen looks like:

![Figure 1: The Collections screen](Images/studio_collections_1.PNG)

In the top there is the list of all available collections, each with a number of documents currently associated with it, and each represented by a different color. Clicking on a collection in that list will load the documents associated with it in the viewing pane below. Navigating the documents is done the same way as it is done in the [Documents screen](documents).

![Figure 2: The list of available collection](Images/studio_collections_2.PNG)

## Deleting collections

From this screen you can delete a bulk of documents based on their collection association.

Since a Collection is just a logical unit in RavenDB, there is no actual meaning in deleting a collection. By deleting a collection in this screen, you are telling RavenDB to delete _all documents_ sharing the same entity name which is equal to the name of the Collection you are asking to delete.

To perform this delete operation, right click on a Collection name from the list on the top and select "Delete". A confirmation dialog will appear. (You can also delete by pressing the "Delete" key)

![Figure 3: Deleting a collection](Images/studio_collections_3.PNG)

If you wish to delete more than one collection you can do that by selecting several collections with the Ctrl or Shift keys (The same way it is done for windows with ctrl for selecting collections one by one and shift for selecting start and end) and right-clicking and selecting delete (or pressing the "Delete" key).

![Figure 4: Confirm Collection Delete](Images/studio_collections_4.PNG)

{WARNING This operation cannot be undone, and is likely to delete a lot of documents. Use this option wisely. /}
