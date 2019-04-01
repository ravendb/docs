#Commands: GetKeyNamesAsync

**GetKeyNamesAsync** retrieves names of all stored configurations.

## Syntax

{CODE get_key_names_1@FileSystem\ClientApi\Commands\Configurations.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | The number of results that should be skipped |
| **pageSize** | int | The maximum number of results that will be returned |
<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;string[]&gt;** |A task that represents the asynchronous operation. The result is the array of configuration names.  |

## Example

The below code will retrieve first 25 names of the existing configurations.

{CODE get_key_names_2@FileSystem\ClientApi\Commands\Configurations.cs /}
