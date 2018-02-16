# Configuration : Monitoring Options

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
