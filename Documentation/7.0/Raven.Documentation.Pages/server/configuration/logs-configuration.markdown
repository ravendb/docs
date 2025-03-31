# Configuration: Logs Options
---

{NOTE: }

* The following configuration keys allow you to control logging behavior in a RavenDB server.  
  To learn more about RavenDB's logging see [Logging](../../server/troubleshooting/logging).  

* In this page:  
    * RavenDB logging configuration keys:  
      [Logs.ArchiveAboveSizeInMb](../../server/configuration/logs-configuration#logs.archiveabovesizeinmb)  
      [Logs.ConfigPath](../../server/configuration/logs-configuration#logs.configpath)  
      [Logs.EnableArchiveFileCompression](../../server/configuration/logs-configuration#logs.enablearchivefilecompression)  
      [Logs.MaxArchiveDays](../../server/configuration/logs-configuration#logs.maxarchivedays)  
      [Logs.MaxArchiveFiles](../../server/configuration/logs-configuration#logs.maxarchivefiles)  
      [Logs.Microsoft.MinLevel](../../server/configuration/logs-configuration#logs.microsoft.minlevel)  
      [Logs.MinLevel](../../server/configuration/logs-configuration#logs.minlevel)  
      [Logs.NuGet.AdditionalPackages](../../server/configuration/logs-configuration#logs.nuget.additionalpackages)  
      [Logs.NuGet.AllowPreReleasePackages](../../server/configuration/logs-configuration#logs.nuget.allowprereleasepackages)  
      [Logs.NuGet.PackagesPath](../../server/configuration/logs-configuration#logs.nuget.packagespath)  
      [Logs.NuGet.PackageSourceUrl](../../server/configuration/logs-configuration#logs.nuget.packagesourceurl)  
      [Logs.Path](../../server/configuration/logs-configuration#logs.path)  
      [Logs.ThrowConfigExceptions](../../server/configuration/logs-configuration#logs.throwconfigexceptions)
    * Internal NLog configuration keys:  
      [Logs.Internal.Level](../../server/configuration/logs-configuration#logs.internal.level)  
      [Logs.Internal.LogToStandardError](../../server/configuration/logs-configuration#logs.internal.logtostandarderror)  
      [Logs.Internal.LogToStandardOutput](../../server/configuration/logs-configuration#logs.internal.logtostandardoutput)  
      [Logs.Internal.Path](../../server/configuration/logs-configuration#logs.internal.path)  

{NOTE/}

---

{PANEL: Logs.ArchiveAboveSizeInMb}

The maximum size (in megabytes) a log file may reach before it is archived and logging is directed to a new file.  
This setting ensures that logs are stored in multiple smaller files rather than a few large ones.

- **Type**: `int`
- **Default**: `128`
- **Min Value**: `16`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.ConfigPath}

The path to an XML file that overrides all logging configuration parameters.  
Set to `null` to apply the configuration params detailed in this section,  
or provide a path to an XML configuration file whose content overrides these settings.  

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.EnableArchiveFileCompression}

Determines whether to compress archived log files.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only
- **Alias:** `Logs.Compress`

{PANEL/}

{PANEL: Logs.MaxArchiveDays}

The maximum number of days to retain an archived log file.  
Set this value to the number of days after which log files will be deleted,  
or set it to `null` to keep log files indefinitely.

- **Type**: `int?`
- **Default**: `3`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.MaxArchiveFiles}

The maximum number of archived log files to keep.  

- **Type**: `int?`
- **Default**: `null`
- **Min Value**: `0`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.Microsoft.MinLevel}

The minimum logging level for Microsoft logs.

- **Type**: `enum LogLevel` (`Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`)
- **Default**: `LogLevel.Error`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.MinLevel}

Determines the minimum logging level.  
Log entries will be included starting from the specified MinLevel and higher.  

- **Type**: `enum LogLevel` (`Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`)
- **Default**: `LogLevel.Info`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.NuGet.AdditionalPackages}

A dictionary of additional NuGet packages to load during server startup for additional logging targets.  
Each key represents the package name, and the corresponding value specifies the package version.

- **Type**: `Dictionary<string, string>`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.NuGet.AllowPreReleasePackages}

Determines whether to allow installation of NuGet pre-release packages.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.NuGet.PackagesPath}

The path where NuGet packages required by RavenDB are downloaded.

- **Type**: `string`
- **Default**: `Packages/NuGet/Logging`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.NuGet.PackageSourceUrl}

The default URL for the NuGet package source.

- **Type**: `string`
- **Default**: `https://api.nuget.org/v3/index.json`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.Path}

The path to the folder where RavenDB server log files are stored.  
By default, it is the `Logs` folder under the server folder.

- **Type**: `string`
- **Default**: `Logs`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.ThrowConfigExceptions}

Determines whether to throw an exception if NLog detects a logging configuration error.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.Internal.Level}

Determines the logging level for NLog's internal logs.

- **Type**: `enum LogLevel` (`Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Off`)
- **Default**: `LogLevel.Info`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.Internal.LogToStandardError}

Determines whether to write NLog's internal logs to the standard error stream.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.Internal.LogToStandardOutput}

Determines whether to write NLog's internal logs to the standard output stream.  
This can be useful when running the server to verify that it operates without issues.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Logs.Internal.Path}

The path to the folder where NLog's internal logs are written.  
This is useful for debugging NLog.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}
