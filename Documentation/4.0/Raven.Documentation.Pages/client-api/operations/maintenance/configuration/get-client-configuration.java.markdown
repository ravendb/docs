# Operations : How to Get Client Configuration

**GetClientConfigurationOperation** is used to return a client configuration, which is saved on the server and overrides client behavior. 

## Syntax

{CODE:java config_1_0@ClientApi\Operations\ClientConfig.java /}

{CODE:java config_1_1@ClientApi\Operations\ClientConfig.java /}

| Return Value | | |
| ------------- | ----- | ---- |
| **Etag** | String | Etag of configuration |
| **Configuration** | `ClientConfiguration` | configuration which will be used by the client API |

## Example

{CODE:java config_1_2@ClientApi\Operations\ClientConfig.java /}

## Related Articles

- [How to get server-wide **client configuration**?](../../../../client-api/operations/server-wide/configuration/get-serverwide-client-configuration)
