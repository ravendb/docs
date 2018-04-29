# Operations : How to Put Client Configuration

**PutClientConfigurationOperation** is used to save a client configuration on the server. It allows you to override client's settings remotely. 

## Syntax

{CODE:java config_2_0@ClientApi\Operations\ClientConfig.java /}

| Return Value | | |
| ------------- | ----- | ---- |
| **configuration** | `ClientConfiguration` | configuration which will be used by client API |

## Example

{CODE:java config_2_2@ClientApi\Operations\ClientConfig.java /}

## Related Articles

- [How to get server-wide **client configuration**?](../../../../client-api/operations/server-wide/configuration/get-serverwide-client-configuration)
- [How to put server-wide **client configuration**?](../../../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration)
