# Operations: Server: How to Put Server-Wide Client Configuration

**PutServerWideClientConfigurationOperation** is used to save a server-wide client configuration on the server. It allows you to override the client's settings remotely.  

{NOTE `ClientConfiguration` defined at the database level overrides the server-wide client configuration. /}

## Syntax

{CODE config_2_0@ClientApi\Operations\Server\ClientConfig.cs /}

## Example

{CODE config_2_2@ClientApi\Operations\Server\ClientConfig.cs /}

## Related Articles

- [How to get **Server-Wide client configuration**?](../../../../client-api/operations/server-wide/configuration/get-serverwide-client-configuration)
- [How to get **client configuration**?](../../../../client-api/operations/maintenance/configuration/get-client-configuration)
- [How to put **client configuration**?](../../../../client-api/operations/maintenance/configuration/put-client-configuration)
