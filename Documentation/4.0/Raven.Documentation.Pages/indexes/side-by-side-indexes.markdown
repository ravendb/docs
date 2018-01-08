# Side-by-Side indexes

This feature enables you to create an index that will be replaced by another one after one of the following conditions are met:

- new index becomes non-stale (non-optional)
- new index reaches last indexed etag (in the moment of creation of a new side-by-side index) of a index that will be replaced (optional)
- particular date is reached (optional)

## Applications

As you probably know, making any changes in index definition will reset its indexing state and indexing process will start from scratch. This situation can be troublesome when you need to update index (assuming that changes are backward compatible) on production server without having your application display partial results (due to index reset). This is why side-by-side indexes were introduced.

