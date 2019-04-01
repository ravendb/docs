# Operations: How to Put Server-Wide Client Configuration

**PutServerWideClientConfigurationOperation** is used to save server-wide client configuration on the server. It allows you to override the client's settings remotely. 

{NOTE `ClientConfiguration` defined at database level overrides the server-wide client configuration. /}

## Syntax

{CODE config_2_0@ClientApi\Operations\ClientConfig.cs /}

## Example

{CODE config_2_2@ClientApi\Operations\ClientConfig.cs /}

## Related Articles

- [How to get **client configuration**?](../../../../client-api/operations/maintenance/configuration/get-client-configuration)
- [How to put **client configuration**?](../../../../client-api/operations/maintenance/configuration/put-client-configuration)
