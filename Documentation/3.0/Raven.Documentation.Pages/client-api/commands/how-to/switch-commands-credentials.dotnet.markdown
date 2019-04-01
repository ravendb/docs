# Commands: How to switch commands credentials?

By default, commands available directly in store are working with credentials that were set up for that store. To use different credentials for commands use **With** method.

## Syntax

{CODE with_1@ClientApi\Commands\HowTo\SwitchCommandsCredentials.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **credentialsForSession** | ICredentials | credentials that should be used by new commands |

| Return Value | |
| ------------- | ----- |
| IDatabaseCommands | New instance of commands that will use given credentials. |

## Example

{CODE with_2@ClientApi\Commands\HowTo\SwitchCommandsCredentials.cs /}

## Related articles

- [How to **switch** commands to a different **database**?](../../../client-api/commands/how-to/switch-commands-to-a-different-database)   
