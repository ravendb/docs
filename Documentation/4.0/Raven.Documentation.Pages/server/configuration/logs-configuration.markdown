# Configuration : Logs Options

{PANEL:Logs.Path}

Path to the directory where logs will be placed. By default it is placed in the 'Logs' directory in the same folder as the server.

- **Type**: `string`
- **Default**: `Logs`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Logs.Mode}

Level of logs that should be written to the log files. The available options are:

- `None` (logging disabled)
- `Operations` (high level info for operational users)
- `Information` (low level debug info)

- **Type**: `string`
- **Default**: `Operations`
- **Scope**: Server-wide only

{PANEL/}
