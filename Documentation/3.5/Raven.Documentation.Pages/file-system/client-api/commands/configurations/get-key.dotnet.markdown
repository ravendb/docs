#Commands: GetKeyAsync

**GetKeyAsync** is used to retrieve an object stored as [a configuration item](../../../configurations) in RavenFS.

## Syntax

{CODE get_key_1@FileSystem\ClientApi\Commands\Configurations.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | string | The configuration name |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;T&gt;** | A task that represents the asynchronous operation. The task result is the deserialized object of type *T*. |

## Example

{CODE get_key_2@FileSystem\ClientApi\Commands\Configurations.cs /}
