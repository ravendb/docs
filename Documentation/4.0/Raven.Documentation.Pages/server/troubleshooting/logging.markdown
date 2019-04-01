# Troubleshooting: Logging

RavenDB has extensive support for logging, enabling you to figure out exactly what is going on in the server.

{PANEL:Logging to Files}

The logging to files can be setup by [logs configuration](../../server/configuration/logs-configuration) defined in the [settings.json](../configuration/configuration-options#json) file. By default, the logging
is turned on with the following options:

- logging level: `Operations` (high level info for operational users)
- logs directory: 'Logs' (next to RavenDB executables)

If you want to see the low level debug information you need to set the logging level to `Information`. The server restart is required to apply the changes.

{NOTE: RavenCLI}

Modifying the [settings.json](../configuration/configuration-options#json) file requires the server restart. In order to avoid that you can modify the current log level by using the [CLI](../../server/administration/cli). 

This will change the logging level without the need of doing any server restarts, but will not modify the [settings.json](../configuration/configuration-options#json) file, so please bare in mind that after restart the logging level be read from configuration file again.

{CODE-BLOCK:plain}
log <on|off|http-on|http-off|none|operations|information>
{CODE-BLOCK/}

You can read more about the command [here](../../server/administration/cli#log).

{NOTE/}

{NOTE: Async log}

In order to not affect the server performance too much, even with debug info logging enabled, the RavenDB logging is asynchronous and 
handled by the dedicated thread responsible for executing the I/O operations.

{NOTE/}

{PANEL/}

{PANEL:Studio: Admin Logs}

Another option that allows you to see the debug logging without the need to restart the server is to use the Admin Logs feature available in the Studio: `Manage Server -> Admin Logs`.

{PANEL/}

## Related Articles

### Administration

- [Command Line Interface (CLI)](../../server/administration/cli)

### Configuration

- [Logs](../../server/configuration/logs-configuration)
