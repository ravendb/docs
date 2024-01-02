# Configuration: Logs Options

{PANEL: Logs.Path}

The path to the directory where the RavenDB server logs will be stored.  
By default, the logs are placed in the 'Logs' directory in the same folder as the server.

- **Type**: `string`
- **Default**: `Logs`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.Mode}

The level of logs that will be written to the log files.  
Available options:

- `None` (logging disabled)
- `Operations` (high level info for operational users)
- `Information` (low level debug info)

---

- **Type**: `string`
- **Default**: `Operations`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.UseUtcTime}

Determine whether logs are timestamped in UTC or with server-local time.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{INFO: }
Writing logs in UTC is more performant than using server-local time.
{INFO/}

{PANEL/}

{PANEL: Logs.MaxFileSizeInMb}

The maximum log file size in megabytes.

- **Type**: `int`
- **Default**: `128`
- **Minimum**: `16`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.RetentionTimeInHrs}

The number of hours logs are kept before they are deleted.

- **Type**: `int`
- **Default**: `72`
- **Minimum**: `24`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.RetentionSizeInMb}

The maximum log size after which older files will be deleted.  
No log files will be deleted if this configuration is not set.

- **Type**: `int`
- **Default**: `null`
- **Minimum**: `256`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.Compress}

Determine whether to compress the log files.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.Microsoft.Disable}

Determine whether to disable Microsoft logs.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.Microsoft.ConfigurationPath}

The path to the JSON configuration file for Microsoft logs.

- **Type**: `string`
- **Default**: `settings.logs.microsoft.json`
- **Scope**: Server-wide only

{PANEL/}
