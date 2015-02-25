#Commands : SearchOnDirectoryAsync

**SearchOnDirectoryAsync** method returns files located in a given directory and matching specified file name search pattern. 

## Syntax

{CODE search_on_directory_1@FileSystem\ClientApi\Commands\Search.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **folder** | string | The directory path to look for files |
| **options** | FilesSortOptions | It determines the sorting options when returning results |
| **fileNameSearchPattern** | string | The pattern that a file name must match ('?' any single character, '*' any characters, default: empty string - means that the file name matching is skipped) |
| **start** | int | The number of files that should be skipped |
| **pageSize** | int | The maximum number of files that will be returned |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;SearchResults&gt;** | A task that represents the asynchronous operation. The task result is [`SearchResults`](../../../../../glossary/search-results) object which represents results of a specified query. |

## Example

{CODE search_on_directory_2@FileSystem\ClientApi\Commands\Search.cs /}