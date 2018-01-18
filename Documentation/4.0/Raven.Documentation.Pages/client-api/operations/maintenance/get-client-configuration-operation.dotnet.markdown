# Operations : How to get client configuration?

**GetClientConfigurationOperation** is used to return client configuration, which is saved on server and overrides client behavior. 

## Syntax

{CODE config_1@ClientApi\Operations\ClientConfiguration.cs /}

{CODE config_2@ClientApi\Operations\ClientConfiguration.cs /}

| Return Value | | |
| ------------- | ----- | ---- |
| **Etag** | string | Etag of configuration |
| **Configuration** | [ClientConfiguration](../../../glossary/ClientConfiguration) | configuration which will be used by client API |

## Example

{CODE config_3@ClientApi\Operations\ClientConfiguration.cs /}

## Related articles

- [How to get server-wide **client configuration**?](../../../client-api/operations/server/get-serverwide-client-configuration-operation)
