# Troubleshooting : Logging

RavenDB has extensive support for logging, enabling you to figure out exactly what is going on in the server.

{PANEL:Logging to Files}

The logging to files can be setup by [logs configuration](../../server/configuration/logs-configuration) defined in `settings.json` file. By default, the logging
is turned on with the following options:

- logging level: `Operations` (high level info for operational users)
- logs directory: 'Logs' (next to RavenDB executables)

If you wan to see the low level debug information you need to set the logging level to `Information`. The server restart is required to apply the changes.

{NOTE: Async log}

In order to not affect the server performance too much, even with debug info logging enabled, the RavenDB logging is asynchronous and 
handled by the dedicated thread responsible for executing the I/O operations.

{NOTE/}

{PANEL/}

{PANEL:Studio: Admin Logs}

Another option that allows you to see the debug logging without the need to restart the server is to use the Admin Logs feature available in the Studio: `Manage Server -> Admin Logs`.

{PANEL/}
