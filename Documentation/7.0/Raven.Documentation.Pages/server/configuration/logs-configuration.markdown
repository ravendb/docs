# Configuration: Logs Options
---

{NOTE: }

* The following configuration keys allow you to control logging behavior in a RavenDB server.  
  To learn more about RavenDB's logging see [Logging](../../server/troubleshooting/logging).  

* In this page:
   * [Logs.ConfigPath](../../server/configuration/logs-configuration#logs.configpath)  
   * [Logs.Path](../../server/configuration/logs-configuration#logs.path)  
   * [Logs.MinLevel](../../server/configuration/logs-configuration#logs.minlevel)  
   * [Logs.Internal.Path](../../server/configuration/logs-configuration#logs.internal.path)  
   * [Logs.Internal.Level](../../server/configuration/logs-configuration#logs.internal.level)  
   * [Logs.Internal.LogToStandardOutput](../../server/configuration/logs-configuration#logs.internal.logtostandardoutput)  
   * [Logs.Internal.LogToStandardError](../../server/configuration/logs-configuration#logs.internal.logtostandarderror)  
   * [Logs.ArchiveAboveSizeInMb](../../server/configuration/logs-configuration#logs.archiveabovesizeinmb)  
   * [Logs.MaxArchiveDays](../../server/configuration/logs-configuration#logs.maxarchivedays)  
   * [Logs.MaxArchiveFiles](../../server/configuration/logs-configuration#logs.maxarchivefiles)  
   * [Logs.EnableArchiveFileCompression, Logs.Compress](../../server/configuration/logs-configuration#logs.enablearchivefilecompression,-logs.compress)  
   * [Logs.Microsoft.MinLevel](../../server/configuration/logs-configuration#logs.microsoft.minlevel)  
   * [Logs.ThrowConfigExceptions](../../server/configuration/logs-configuration#logs.throwconfigexceptions)  
   * [Logs.NuGet.PackagesPath](../../server/configuration/logs-configuration#logs.nuget.packagespath)  
   * [Logs.NuGet.PackageSourceUrl](../../server/configuration/logs-configuration#logs.nuget.packagesourceurl)  
   * [Logs.NuGet.AllowPreReleasePackages](../../server/configuration/logs-configuration#logs.nuget.allowprereleasepackages)  
   * [Logs.NuGet.AdditionalPackages](../../server/configuration/logs-configuration#logs.nuget.additionalpackages)  


{NOTE/}

---

{PANEL: Logs.ConfigPath}

A path to an XML file that overrides all internal logging configuration parameters.  
Set to `null` to use the configuration parameters detailed below,  
or provide a path to an XML configuration file whose content overrides these settings.  

- **Type**: `PathSetting`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.Path}

A path to the folder where RavenDB server log files are stored.  
By default, it is the `Logs` folder under the server folder.  

- **Type**: `string`
- **Default**: `Logs`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.MinLevel}

Determines the minimal logging level.  
Log entries will be added from the set `MinLevel`and up.  

- **Type**: `LogLevel`
- **Default**: `LogLevel.Info`
- **Scope**: Server-wide only

{INFO: }
[For a table of available logging levels](../../server/troubleshooting/logging#logging-levels)
{INFO/}

{PANEL/}

{PANEL: Logs.Internal.Path}

The path to a folder that NLog internal usage logs are written to.  
This can be used when NLog needs to be debugged, for example.  

- **Type**: `PathSetting`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.Internal.Level}

Determines the logging level for NLog internal-usage logs.

- **Type**: `LogLevel`
- **Default**: `LogLevel.Info`
- **Scope**: Server-wide only

{INFO: }
[For a table of available logging levels](../../server/troubleshooting/logging#logging-levels)
{INFO/}

{PANEL/}

{PANEL: Logs.Internal.LogToStandardOutput}

Determines whether to write log messages to the standard output stream.  
Can be used, for example, to run the server and verify that it runs without issues.  

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.Internal.LogToStandardError}

Determines whether to write log messages to the standard output error stream.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.ArchiveAboveSizeInMb}

The largest size (in megabytes) that a log file may reach 
before it is archived and logging is directed to a new file.  
Can be used to make certain that logs are collected in many small files rather than a few large ones.  

- **Type**: `Size`
- **Default**: `128`
- **Min Value**: `16`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.MaxArchiveDays}

The maximum number of days that an archived log file is kept.  

- **Type**: `int`
- **Default**: `3`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.MaxArchiveFiles}

The maximum number of archived log files to keep.  
Set this value to the number of days after which log files will be deleted,  
or set it to `null` to refrain from removing log files.  

- **Type**: `int`
- **Default**: `null`
- **Min Value**: `0`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.EnableArchiveFileCompression, Logs.Compress}

These two names relate to the same configuration parameter (the older `Logs.Compress` 
is kept for backward compatibility), determining whether to compress archived log files.  

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.Microsoft.MinLevel}

The minimal logging level for Microsoft logs

- **Type**: `LogLevel`
- **Default**: `LogLevel.Error`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.ThrowConfigExceptions}

Determines whether to throw an exception if NLog detects a logging configuration error.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.NuGet.PackagesPath}

The location of the NuGet packages that RavenDB needs to download and resolve to utilize NLog.  

- **Type**: `PathSetting`
- **Default**: `Packages/NuGet/Logging`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.NuGet.PackageSourceUrl}

Default NuGet source URL

- **Type**: `string`
- **Default**: `https://api.nuget.org/v3/index.json`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.NuGet.AllowPreReleasePackages}

Determines whether to allow installation of NuGet pre-release packages.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.NuGet.AdditionalPackages}

A list of additional Nuget packages to load during server startup, for additional logging targets.  

- **Type**: `Dictionary<string, string>`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

