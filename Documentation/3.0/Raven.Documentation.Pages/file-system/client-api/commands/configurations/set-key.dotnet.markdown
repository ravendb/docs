#Commands: SetKeyAsync

**SetKeyAsync** is used to store any object as [a configuration item](../../../configurations) under the specified key.

## Syntax

{CODE set_key_1@FileSystem\ClientApi\Commands\Configurations.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | string | The configuration name |
| **data** | T | The stored object that will be serialized to JSON and saved as a configuration. |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task** | A task that represents the asynchronous set operation |

## Example

Let's assume that we need to store file descriptions for some files but we don't want to add such information to their metadata. The file description is represented by the `FileDescription` class:

{CODE set_key_2@FileSystem\ClientApi\Commands\Configurations.cs /}

We can achieve that simply by setting such an object under the selected key:

{CODE set_key_3@FileSystem\ClientApi\Commands\Configurations.cs /}
