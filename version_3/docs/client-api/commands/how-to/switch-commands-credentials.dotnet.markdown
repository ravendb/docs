# Client API : Commands : How to switch commands credentials?

By default, commands available directly in store are working with credentials that were setup for that store. To use different credentials for commands use **With** method.

## Syntax

{CODE with_1@ClientApi\Commands\HowTo\SwitchCommandsCredentials.cs /}

**Parameters**

- credentialsForSession - credentials that should be used by new commands   

**Return Value**

Method returns new instance of commands that will use desired credentials.

## Example

{CODE with_2@ClientApi\Commands\HowTo\SwitchCommandsCredentials.cs /}
