# Side-by-Side indexes

This feature enables you to create an index that will be replaced by another one after one of the following conditions are met:

- new index becomes non-stale (non-optional)
- new index reaches last indexed etag (in the moment of creation of a new side-by-side index) of a index that will be replaced (optional)
- particular date is reached (optional)

## Applications

As you probably know, making any changes in index definition will reset its indexing state and indexing process will start from scratch. This situation can be troublesome when you need to update index (assuming that changes are backward compatibile) on production server without having your application display partial results (due to index reset). This is why side-by-side indexes were introduced.

## Example

- first you need to [edit](../studio/overview/indexes/index-edit-view) index. In our example we are picking `Orders/Totals` and we are adding `ShipVia` field.

![Figure 1. Side-by-Side. Index Edit.](images/side-by-side-1.png)

- finally, when index definiton is ready, you need to save index as `Side-by-Side` using action bar. The popup will appear with the name of an index that will be replaced and a list of replacement conditions.

![Figure 2. Side-by-Side. Index Edit. Definition. Change](images/side-by-side-2.png)

![Figure 3. Side-by-Side. Index. Popup.](images/side-by-side-3.png)

## Related articles

- [Testing indexes](../indexes/testing-indexes)