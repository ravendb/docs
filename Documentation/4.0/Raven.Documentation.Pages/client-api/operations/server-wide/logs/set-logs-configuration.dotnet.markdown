# Operations: Server: How to Get Logs Configuration

To set the server logs configuration, use **SetLogsConfigurationOperation** from `Maintenance.Server`. The server logs configuration is not persisted and will get back to the original value after server restart.

## Syntax

{CODE:csharp set_logs_1@ClientApi\Operations\Server\GetAndSetLogsConfiguration.cs /}

{CODE:csharp set_logs_2@ClientApi\Operations\Server\GetAndSetLogsConfiguration.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **Mode** | `LogMode`  | Logging mode (level) to be set |

##Example

{CODE-TABS}
{CODE-TAB:csharp:Sync set_logs_3@ClientApi\Operations\Server\GetAndSetLogsConfiguration.cs /}
{CODE-TAB:csharp:Async set_logs_4@ClientApi\Operations\Server\GetAndSetLogsConfiguration.cs /}
{CODE-TABS/}
