# Configuration: Logs Options

{PANEL:Logs.Path}

The path to the directory where logs will be placed. By default it is placed in the 'Logs' directory in the same folder as the server.

- **Type**: `string`
- **Default**: `Logs`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Logs.Mode}

The level of logs that should be written to the log files. The available options are:

- `None` (logging disabled)
- `Operations` (high level info for operational users)
- `Information` (low level debug info)

<br />

- **Type**: `string`
- **Default**: `Operations`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Logs.UseUtcTime}

This indicates if logs should be written with UTC or server-local time.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{INFO Writing logs in UTC is more performant than using server-local time. /}

{PANEL/}
