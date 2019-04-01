#Commands: GetSearchFieldsAsync

**GetSearchFieldsAsync** is used to retrieve the list of all available field names to build a query.

## Syntax

{CODE get_search_fields_1@FileSystem\ClientApi\Commands\Search.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | The number of results that should be skipped |
| **pageSize** | int | The maximum number of results that will be returned |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;string[]&gt;** | A task that represents the asynchronous operation. The task result is the array of indexed field names. |

## Example

**GetSearchFieldsAsync** method returns all the indexed fields, so all the keys of built-in metadata (e.g. `Etag`), metadata provided by a user (e.g. `Owner`) and the [default indexing fields](../../../../indexing) (e.g `__key` or `__fileName`) will be included.

{CODE get_search_fields_2@FileSystem\ClientApi\Commands\Search.cs /}

## Related articles

- [Indexing](../../../../indexing)
