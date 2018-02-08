# Operations : Server : How to Get Server-Wide Client Configuration?

**GetClientConfigurationOperation** is used to return a server-wide client configuration, which is saved on the server and overrides client behavior. 

{NOTE Client Configuration at database level overrides server-wide client configuration. /}

## Syntax

{CODE config_1_0@ClientApi\Operations\Server\ClientConfig.cs /}

| Return Value | |
| ------------- | ---- |
| [ClientConfiguration](../../../glossary/ClientConfiguration) | configuration which will be used by the client API |

## Example

{CODE config_1_2@ClientApi\Operations\Server\ClientConfig.cs /}

## Related Articles

- [How to get **client configuration**?](../../../../client-api/operations/maintenance/configuration/get-client-configuration)
