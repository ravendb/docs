# Operations: Server: How to Get Server-Wide Client Configuration

**GetServerWideClientConfigurationOperation** is used to return a server-wide client configuration which is saved on the server and overrides the client behavior. 

{NOTE `ClientConfiguration` defined at the database level overrides server-wide client configurations. /}

## Syntax

{CODE config_1_0@ClientApi\Operations\Server\ClientConfig.cs /}

| Return Value | |
| ------------- | ---- |
| `ClientConfiguration` | Configuration which will be used by the RavenDB Client |

## Example

{CODE config_1_2@ClientApi\Operations\Server\ClientConfig.cs /}

## Related Articles

- [How to put **Server-Wide client configuration**?](../../../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration)
- [How to get **client configuration**?](../../../../client-api/operations/maintenance/configuration/get-client-configuration)
- [How to put **client configuration**?](../../../../client-api/operations/maintenance/configuration/put-client-configuration)
