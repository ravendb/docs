# Operations: Server: How to Get Logs Configuration

To get the server logs configuration, use **GetLogsConfigurationOperation** from `Maintenance.Server`

## Syntax

{CODE:csharp get_logs_1@ClientApi\Operations\Server\GetAndSetLogsConfiguration.cs /}

### Return Value

The result of executing GetLogsConfigurationOperation is a **GetLogsConfigurationResult** object: 

{CODE:csharp get_logs_2@ClientApi\Operations\Server\GetAndSetLogsConfiguration.cs /}

| ------------- |----- |
| **CurrentMode** | Current mode that is active |
| **Mode** | Mode that is written in the configuration file and which will be used after a server restart |
| **Path** | Path to which logs will be written |
| **UseUtcTime** | Indicates if logs will be written in UTC or in server local time |

##Example

{CODE-TABS}
{CODE-TAB:csharp:Sync get_logs_3@ClientApi\Operations\Server\GetAndSetLogsConfiguration.cs /}
{CODE-TAB:csharp:Async get_logs_4@ClientApi\Operations\Server\GetAndSetLogsConfiguration.cs /}
{CODE-TABS/}
