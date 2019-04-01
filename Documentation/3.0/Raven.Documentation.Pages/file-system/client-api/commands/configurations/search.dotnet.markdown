#Commands: SearchAsync

**SearchAsync** retrieves the names of configurations that starts with a specified prefix.

## Syntax

{CODE search_1@FileSystem\ClientApi\Commands\Configurations.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **prefix** | string | The prefix value with which the name of a configuration has to start |
| **start** | int | The number of results that should be skipped |
| **pageSize** | int | The maximum number of results that will be returned |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;ConfigurationSearchResults&gt;** | A task that represents the asynchronous operation. The task result is [`ConfigurationSearchResults`](../../../../glossary/configuration-search-results) object which represents results of a prefix query. |

## Example

In order to get all configuration names which keys start with `descriptions/` prefix you can use the following code:

{CODE search_2@FileSystem\ClientApi\Commands\Configurations.cs /}
