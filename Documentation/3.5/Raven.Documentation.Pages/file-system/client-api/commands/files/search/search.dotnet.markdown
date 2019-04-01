#Commands: SearchAsync

Use the **SearchAsync** method to fetch the list of files matching the specified query.

## Syntax

{CODE search_1@FileSystem\ClientApi\Commands\Search.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **query** | string | The query containing search criteria (you can use the [built-in fields](../../../../indexing) or metadata entries) consistent with [Lucene syntax](http://lucene.apache.org/core/3_0_3/queryparsersyntax.html) |
| **sortFields** | string[] | The fields to sort by |
| **start** | int | The start number to read index results |
| **pageSize** | int | The max number of results that will be returned |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;SearchResults&gt;** | A task that represents the asynchronous operation. The task result is [`SearchResults`](../../../../../glossary/search-results) object which represents results of a specified query. |

## Example I

In order to get the list of files that has `Everyone` value under the `AllowRead` metadata key returned in ascending order by a full file name (stored under built-in `__key` field), you need to build the following query:

{CODE search_2@FileSystem\ClientApi\Commands\Search.cs /}

{INFO: Results order}
There is a convention which determines the ordering type: *ascending* or *descending*.
The usage of `+` symbol or no prefix before a name of the sorted field means that ascending sorting will be applied. In order to retrieve results in descending order you need to add `-` sign before the field name.
{INFO/}

## Example II

{CODE search_3@FileSystem\ClientApi\Commands\Search.cs /}

## Related articles

- [Indexing](../../../../indexing)
