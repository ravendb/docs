# Configuration: Monitoring Options
---

{NOTE: }

* In this page:
    * OpenTelemetry monitoring:  
      [Monitoring.OpenTelemetry.ConsoleExporter](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.consoleexporter)  
      [Monitoring.OpenTelemetry.Enabled](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.enabled)  
      [Monitoring.OpenTelemetry.Meters.AspNetCore.Enabled](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.meters.aspnetcore.enabled)  
      [Monitoring.OpenTelemetry.Meters.Runtime.Enabled](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.meters.runtime.enabled)  
      [Monitoring.OpenTelemetry.Meters.Server.CPUCredits.Enabled](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.meters.server.cpucredits.enabled)  
      [Monitoring.OpenTelemetry.Meters.Server.Enabled](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.meters.server.enabled)  
      [Monitoring.OpenTelemetry.Meters.Server.GC.Enabled](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.meters.server.gc.enabled)  
      [Monitoring.OpenTelemetry.Meters.Server.General.Enabled](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.meters.server.general.enabled)  
      [Monitoring.OpenTelemetry.Meters.Server.Requests.Enabled](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.meters.server.requests.enabled)  
      [Monitoring.OpenTelemetry.Meters.Server.Resources.Enabled](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.meters.server.resources.enabled)  
      [Monitoring.OpenTelemetry.Meters.Server.Storage.Enabled](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.meters.server.storage.enabled)  
      [Monitoring.OpenTelemetry.Meters.Server.TotalDatabases.Enabled](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.meters.server.totaldatabases.enabled)  
      [Monitoring.OpenTelemetry.OpenTelemetryProtocol.Enabled](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.opentelemetryprotocol.enabled)  
      [Monitoring.OpenTelemetry.OpenTelemetryProtocol.Endpoint](../..//server/configuration/monitoring-configuration#monitoring.opentelemetry.opentelemetryprotocol.endpoint)  
      [Monitoring.OpenTelemetry.OpenTelemetryProtocol.ExportProcessorType](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.opentelemetryprotocol.exportprocessortype)  
      [Monitoring.OpenTelemetry.OpenTelemetryProtocol.Headers](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.opentelemetryprotocol.headers)  
      [Monitoring.OpenTelemetry.OpenTelemetryProtocol.Protocol](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.opentelemetryprotocol.protocol)  
      [Monitoring.OpenTelemetry.OpenTelemetryProtocol.Timeout](../../server/configuration/monitoring-configuration#monitoring.opentelemetry.opentelemetryprotocol.timeout)    
    * SNMP monitoring  
      [Monitoring.Snmp.AuthenticationPassword](../../server/configuration/monitoring-configuration#monitoring.snmp.authenticationpassword)  
      [Monitoring.Snmp.AuthenticationPassword.Secondary](../../server/configuration/monitoring-configuration#monitoring.snmp.authenticationpassword.secondary)  
      [Monitoring.Snmp.AuthenticationProtocol](../../server/configuration/monitoring-configuration#monitoring.snmp.authenticationprotocol)  
      [Monitoring.Snmp.AuthenticationProtocol.Secondary](../../server/configuration/monitoring-configuration#monitoring.snmp.authenticationprotocol.secondary)  
      [Monitoring.Snmp.AuthenticationUser](../../server/configuration/monitoring-configuration#monitoring.snmp.authenticationuser)  
      [Monitoring.Snmp.AuthenticationUser.Secondary](../../server/configuration/monitoring-configuration#monitoring.snmp.authenticationuser.secondary)  
      [Monitoring.Snmp.Community](../../server/configuration/monitoring-configuration#monitoring.snmp.community)  
      [Monitoring.Snmp.DisableTimeWindowChecks](../../server/configuration/monitoring-configuration#monitoring.snmp.disabletimewindowchecks)  
      [Monitoring.Snmp.Enabled](../../server/configuration/monitoring-configuration#monitoring.snmp.enabled)  
      [Monitoring.Snmp.Port](../../server/configuration/monitoring-configuration#monitoring.snmp.port)  
      [Monitoring.Snmp.PrivacyPassword](../../server/configuration/monitoring-configuration#monitoring.snmp.privacypassword)  
      [Monitoring.Snmp.PrivacyPassword.Secondary](../../server/configuration/monitoring-configuration#monitoring.snmp.privacypassword.secondary)   
      [Monitoring.Snmp.PrivacyProtocol](../../server/configuration/monitoring-configuration#monitoring.snmp.privacyprotocol)  
      [Monitoring.Snmp.PrivacyProtocol.Secondary](../../server/configuration/monitoring-configuration#monitoring.snmp.privacyprotocol.secondary)  
      [Monitoring.Snmp.SupportedVersions](../../server/configuration/monitoring-configuration#monitoring.snmp.supportedversions)  
    * Other monitoring:  
      [Monitoring.Cpu.Exec](../../server/configuration/monitoring-configuration#monitoring.cpu.exec)  
      [Monitoring.Cpu.Exec.Arguments](../../server/configuration/monitoring-configuration#monitoring.cpu.exec.arguments)  
      [Monitoring.Disk.ReadStatsDebounceTimeInMs](../../server/configuration/monitoring-configuration#monitoring.disk.readstatsdebouncetimeinms)  

{NOTE/}

---

{PANEL: Monitoring.OpenTelemetry.ConsoleExporter}

Indicates if metrics should be exported to the console output.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.Enabled}

Indicates if OpenTelemetry is enabled or not.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.Meters.AspNetCore.Enabled}

Indicates if AspNetCore metric is enabled or not.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.Meters.Runtime.Enabled}

Indicates if Runtime metric is enabled or not.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.Meters.Server.CPUCredits.Enabled}

Expose metrics related to CPU credits.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.Meters.Server.Enabled}

Indicates if RavenDB's OpenTelemetry metrics are enabled or not.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.Meters.Server.GC.Enabled}

Expose metrics related to GC.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.Meters.Server.General.Enabled}

Expose metrics related to general information about the cluster and its licensing.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.Meters.Server.Requests.Enabled}

Expose metrics related to requests.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.Meters.Server.Resources.Enabled}

Expose metrics related to resources usage.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.Meters.Server.Storage.Enabled}

Expose metrics related to server storage.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.Meters.Server.TotalDatabases.Enabled}

Expose metrics related to aggregated database statistics.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.OpenTelemetryProtocol.Enabled}

Indicates if metrics should be exported via the OpenTelemetry protocol.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.OpenTelemetryProtocol.Endpoint}

Endpoint where OpenTelemetryProtocol should sends data.  

- **Type**: `string`
- **Default**: `null` (internal OTLP default settings)
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.OpenTelemetryProtocol.ExportProcessorType}

OpenTelemetryProtocol export processor type.

- **Type**: `enum ExportProcessorType` (Simple | Batch)
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.OpenTelemetryProtocol.Headers}

OpenTelemetryProtocol custom headers.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.OpenTelemetryProtocol.Protocol}

Defines the protocol that OpenTelemetryProtocol should use to send data.

- **Type**: `enum OtlpExportProtocol` (Grpc | HttpProtobuf)
- **Default**: `null` (internal OTLP default settings)
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.OpenTelemetry.OpenTelemetryProtocol.Timeout}

OpenTelemetryProtocol timeout value.

- **Type**: `int?`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Snmp.AuthenticationPassword}

Authentication password used for SNMP v3 authentication.  
When set to `null` then the value from 'Monitoring.Snmp.Community' is used.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Snmp.AuthenticationPassword.Secondary}

Authentication password used by secondary user for SNMP v3 authentication.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Snmp.AuthenticationProtocol}

Authentication protocol used for SNMP v3 authentication.

- **Type**: `SnmpAuthenticationProtocol`
- **Default**: `SHA1`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Snmp.AuthenticationProtocol.Secondary}

Authentication protocol used by secondary user for SNMP v3 authentication.

- **Type**: `SnmpAuthenticationProtocol`
- **Default**: `SHA1`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Snmp.AuthenticationUser}

Authentication user used for SNMP v3 authentication.

- **Type**: `string`
- **Default**: `"ravendb"`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Snmp.AuthenticationUser.Secondary}

Authentication secondary user used for SNMP v3 authentication.

- **Type**: `string`
- **Default**: `null (disabled)`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Snmp.Community}

Community string used for SNMP v2c authentication.

- **Type**: `string`
- **Default**: `"ravendb"`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Snmp.DisableTimeWindowChecks}

EXPERT ONLY.  
Disables time window checks, which are problematic for some SNMP engines.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Snmp.Enabled}

Indicates if SNMP endpoint is enabled or not.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Snmp.Port}

Port at which SNMP listener will be active.

- **Type**: `int`
- **Default**: `161`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Snmp.PrivacyPassword}

Privacy password used for SNMP v3 privacy.

- **Type**: `string`
- **Default**: `"ravendb"`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Snmp.PrivacyPassword.Secondary}

Privacy password used by secondary user for SNMP v3 privacy.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Snmp.PrivacyProtocol}

Privacy protocol used for SNMP v3 privacy.

- **Type**: `SnmpPrivacyProtocol`
- **Default**: `SnmpPrivacyProtocol.None`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Snmp.PrivacyProtocol.Secondary}

Privacy protocol used by secondary user for SNMP v3 privacy.

- **Type**: `SnmpPrivacyProtocol`
- **Default**: `SnmpPrivacyProtocol.None`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Snmp.SupportedVersions}

List of supported SNMP versions. Values must be semicolon separated.

- **Type**: `string[]`
- **Default**: `V2C;V3`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Cpu.Exec}

A command or executable to run which will provide machine CPU usage and process CPU to standard output.  
If specified, RavenDB will use this information for monitoring CPU usage.  
Note: the write to standard output should be unbuffered to work properly.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Cpu.Exec.Arguments}

The command line arguments for the 'Monitoring.Cpu.Exec' command or executable.  
The arguments must be escaped for the command line.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Monitoring.Disk.ReadStatsDebounceTimeInMs}

The minimum interval between measures to calculate the disk stats.

- **Type**: `TimeSetting`
- **Default**: `1000`
- **Scope**: Server-wide only

{PANEL/}
