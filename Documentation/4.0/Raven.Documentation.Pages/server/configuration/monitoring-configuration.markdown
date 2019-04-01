# Configuration: Monitoring Options

{PANEL:Monitoring.Snmp.Enabled}

Indicates if SNMP endpoint is enabled or not.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Monitoring.Snmp.Port}

Port at which SNMP listener will be active.

- **Type**: `int`
- **Default**: `161`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Monitoring.Snmp.Community}

Community string used for authentication.

- **Type**: `string`
- **Default**: `ravendb`
- **Scope**: Server-wide only

{PANEL/}


{PANEL:Monitoring.Cpu.Exec}

A command or executable to run which will provide machine CPU usage and process CPU to standard output. If specified, RavenDB will use this information for monitoring CPU usage.  
Note: the write to standard output should be unbuffered to work properly.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Monitoring.Cpu.Exec.Arguments}

The command line arguments for the 'Monitoring.Cpu.Exec' command or executable. The arguments must be escaped for the command line.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}
