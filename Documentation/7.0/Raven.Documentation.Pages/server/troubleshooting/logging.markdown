# Logging
---

{NOTE: }

* **Multiple logging levels**  
  RavenDB's flexible logging system allows you to pick the minimal logging 
  level most suitable for your needs from a 
  [scale of 6 logging levels](../../server/troubleshooting/logging#available-logging-levels).  
  
     The _Trace_ logging level, for example, will output all events, transactions, 
     and database changes, producing logs that can be processed for auditing 
     and accounting, while _Warning-level_ log entries will include only warnings 
     and errors to keep the log's clarity and remain easy to evaluate and possibly 
     escalate even when the database grows bigger and more complex.  

* **Numerous logging destinations**  
  Starting with RavenDB version `7.0` RavenDB outputs all log data through 
  [NLog](https://nlog-project.org), a widely used `.Net` logging framework 
  capable of streaming logged data to various destinations using a large 
  number of available [NLog plugins](https://nlog-project.org/config/).  

* **High performance**  
  Logging is asynchronous and is handled by a thread dedicated to the 
  execution of I/O operations, minimizing its effect on server performance.  

* In this page:  
  * [Available logging destinations](../../server/troubleshooting/logging#available-logging-destinations)  
  * [Logging configuration](../../server/troubleshooting/logging#logging-configuration)  
  * [Available logging levels](../../server/troubleshooting/logging#available-logging-levels)  
      * [Customizing logging level after Migration](../../server/troubleshooting/logging#customize-after-migration)  
  * [Default values](../../server/troubleshooting/logging#default-values)  
  * [CLI customization: immediate temporary changes](../../server/troubleshooting/logging#cli-customization:-immediate-temporary-changes)  
  * [Configuring and using NLog](../../server/troubleshooting/logging#configuring-and-using-nlog)  
     * [Configure RavenDB to use an external NLog configuration file](../../server/troubleshooting/logging#use-external-config-file)  
     * [Install NLog plugins that RavenDB would log data through](../../server/troubleshooting/logging#install-nlog-plugins)  
     * [Set your NLog configuration file](../../server/troubleshooting/logging#set-nlog-config-file)  
         * [An available template](../../server/troubleshooting/logging#an-available-template)  
         * [Mandatory `logger` definitions](../../server/troubleshooting/logging#mandatory--definitions)  
         * [Your own loggers](../../server/troubleshooting/logging#your-own-loggers)  
  * [Studio: Admin Logs](../../server/troubleshooting/logging#studio:-admin-logs)  

{NOTE/}

---

{PANEL: Available logging destinations}

RavenDB versions up to `6.2` output log data to files on the server machine, and can 
optionally stream it to the server console.  

From version `7.0` on, RavenDB incorporates the [NLog](https://nlog-project.org) 
logging framework. The logging process has hardly changed, but the integration with 
NLog now allows RavenDB to log more versatile data to many additional destinations 
via [NLog plugins](https://nlog-project.org/config/).  
Available logging destinations include, among others, log aggregators like 
[Grafana Loki](https://grafana.com/oss/loki/) and [Splunk](https://en.wikipedia.org/wiki/Splunk), 
filtering and error handling tools, and a long list of applications that NLog 
is integrated with.  

{PANEL/}

{PANEL: Logging configuration}

* By default, RavenDB uses **internal configuration keys** to adjust logging.  
  Using this method, you can only output data to log files and to your console.  
   * Up to version `6.2`, this is the only available way to customize logging.  
   * [List of logging configuration keys](../../server/configuration/logs-configuration)  
   * [How to modify configuration keys](../../server/configuration/configuration-options)  

* If you want to utilize **NLog plugins** so you can output log data to additional 
  destinations, you must use an **external NLog configuration** file.  
   * Changing logging settings using an external configuration file is possible 
     from version `7.0` on, as part of NLog's integration with RavenDB. 
   * Once an external configuration file is applied, its settings override all 
     the values set by internal configuration keys.  
   * [Using an external configuration file](../../server/troubleshooting/logging#configuring-and-using-nlog)  

* To **determine whether to use an NLog configuration file or internal configuration keys**,  
  set the [Logs.ConfigPath](../../server/configuration/logs-configuration#logs.configpath) 
  configuration key with -  
   - a [path](../../server/troubleshooting/logging#use-external-config-file) 
     to the configuration file you want to apply  
   - or `null` to continue using internal configuration keys.  

---

{INFO: Applying changes}

* Permanent changes in the logging configuration, through either configuration keys 
  or an external configuration file, are _applied by restarting the server_.  

* It is also possible to apply _temporary changes_ without restarting the server, 
  using [the CLI](../../server/troubleshooting/logging#cli-customization-immediate-temporary-changes).  
  
* The _scope_ of all logging settings is _server-wide_, applying to all databases.  

{INFO/}

{PANEL/}

{PANEL: Available logging levels}

The logging levels offered by RavenDB have changed from versions prior to `7.0` 
to newer versions.  

---

#### Logging Modes

RavenDB versions up to `6.2` support proprietary **logging modes**.  

| Available logging Mode | Description |
| ------------- | ----------- |
| `Operations` | High-level info for operational users |
| `Information` | Low-level debug info |
| `None` | Logging is disabled |

---

#### Logging Levels

From version `7.0` on, RavenDB's **logging levels** are NLog-compliant.  

| Available logging Level | Description |
| ------------- | ----------- |
| `Trace` | A highly informative level that's mostly used only during development |
| `Debug` | Debug information reported by internal application processes |
| `Info` | Information related to application progress, events lifespan, etc. |
| `Warn` | Warnings returned by failed validations, mild, recoverable failures, etc. |
| `Error` | Error reports of functionality failures, caught exceptions, etc. |
| `Fatal` | Fatal failures |
| `Off` | Logging is disabled |

<a id="customize-after-migration" />
{CONTENT-FRAME: Customizing logging level after Migration:}

When migrating from an older version to `7.0` or higher, RavenDB **is** capable 
of understanding the old version's _logging mode_ configuration and translate 
it to the equivalent NLog level.  
Logging will therefore continue uninterrupted and there's no rush to modify the 
logging level right after migration.  

You _will_ need to modify these settings, however, if you want to utilize one of the 
newer logging levels, or if you want to transfer data to additional destinations via 
NLog plugins.  
If this is the case, e.g. if your existing [settings.json](../../server/configuration/configuration-options#settings.json) 
currently defines -  
{CODE-BLOCK:plain}
...
"Logs.Mode": "Operations",
...
{CODE-BLOCK/}
You will need to explicitly change this value to an NLog level.  
E.g. -  
{CODE-BLOCK:plain}
...
"Logs.MinLevel": "Info",
...
{CODE-BLOCK/}
{CONTENT-FRAME/}

{PANEL/}

{PANEL: Default values}

* **Default logging level**  
  The default minimal logging level is `LogLevel.Info`.  
   - To use a different level, set the 
     [Logs.MinLevel](../../server/configuration/logs-configuration#logs.minlevel) configuration key.  
* **Default destination**  
  Log entries are written by default to log files in the **Logs** folder on the server machine.  
   - To store log files in a different path, set the 
     [Logs.Path](../../server/configuration/logs-configuration#logs.path) configuration key.  
   - [Learn how to log to additional destinations](../../server/troubleshooting/logging#configuring-and-using-nlog)  
* **NLog configuration file defaults**  
  The **default values** given to settings in the 
  [NLog configuration file template](../../server/troubleshooting/logging#an-available-template) 
  are **identical** to those given to internal configuration keys.  
   - [The list of logging configuration keys and their default values](../../server/configuration/logs-configuration)  

{PANEL/}

{PANEL: CLI customization: immediate temporary changes}

Logging settings can also be customized via [CLI](../../server/administration/cli), 
using the [log](../../server/administration/cli#log) command.  
Unlike the permanent customization methods described above (via internal configuration 
keys or an external NLog file), that require a server restart to take effect, changes 
made using CLI commands will take **immediate effect**. However, they will also be 
overridden when the server is restarted and the permanent configuration is reloaded from 
[settings.json](../../server/configuration/configuration-options#settings.json) 
or the NLog configuration file.  

Use this syntax to customize logging via CLI:  
{CODE-BLOCK:plain}
log [on|off] [http-]<on|off> [info|debug] [no-console]
{CODE-BLOCK/}

#### Example:
To temporarily change the logging level to `debug`, issue this command in the server console:  
{CODE-BLOCK:plain}
ravendb> log debug
{CODE-BLOCK/}

{PANEL/}

{PANEL: Configuring and using NLog}

To use NLog you need to:  

1. [Configure RavenDB to use an external NLog configuration file](../../server/troubleshooting/logging#use-external-config-file)  
2. [Install NLog plugins that RavenDB would log data through](../../server/troubleshooting/logging#install-nlog-plugins)  
3. [Set your NLog configuration file](../../server/troubleshooting/logging#set-nlog-config-file)  

<a id="use-external-config-file" />
{CONTENT-FRAME: 1. Configure RavenDB to use an external NLog configuration file}  

NLog options are customized using an NLog configuration file.  
Direct RavenDB to the location of your configuration file using the 
[Logs.ConfigPath](../../server/configuration/logs-configuration#logs.configpath) 
configuration key with the file's path as a value.  

---

#### Example:
To use a configuration file named `NLog.config` that resides in the RavenDB server 
folder, add `settings.json` this line:  
{CODE-BLOCK:plain}
...
"Logs.ConfigPath": "NLog.config",
...
{CODE-BLOCK/}

{INFO: }

* Learn to set configuration keys [here](../../server/configuration/configuration-options).  
* Learn how to create and modify a configuration file [below](../../server/troubleshooting/logging#set-nlog-config-file).  
* Be aware that once a configuration file is used, the logging settings included in it 
  will  _override_ all [internal configuration keys](../../server/configuration/logs-configuration) 
  settings.  
{INFO/}
{CONTENT-FRAME/}

---

<a id="install-nlog-plugins" />
{CONTENT-FRAME: 2. Load and run NLog plugins Nuget packages}

NLog's biggest advantage lies in its [plugins library](https://nlog-project.org/config/), 
through which applications like RavenDB can stream log data to a variety of destinations.  
NLog plugins are available as Nuget packages, that we can easily instruct RavenDB to load 
and run during startup.  
We do this by defining the plugin Nuget package URL as a property of the 
[Logs.NuGet.AdditionalPackages](../../server/configuration/logs-configuration#logs.nuget.additionalpackages) 
configuration key, with the plugin version we want to use as a value.  

---

#### Example
To load a [Grafana Loki](https://grafana.com/oss/loki/) plugin, for example, 
add RavenDB's `Logs.NuGet.AdditionalPackages` configuration key an `NLog.Targets.Loki` 
property, with the plugin's version you want to use as a value.  

`settings.json`:
{CODE-BLOCK:plain}
...
"Logs.NuGet.AdditionalPackages": { "NLog.Targets.Loki": "3.3.0" },
...
{CODE-BLOCK/}

{INFO: Other Nuget-related configuration keys}

* [Logs.NuGet.PackagesPath](../../server/configuration/logs-configuration#logs.nuget.packagespath)  
  Use this key to select the path to which NuGet packages are downloaded.  
* [Logs.NuGet.PackageSourceUrl](../../server/configuration/logs-configuration#logs.nuget.packagesourceurl)  
  Use this key to set the default location from which NuGet packages are downloaded.  
* [Logs.NuGet.AllowPreReleasePackages](../../server/configuration/logs-configuration#logs.nuget.allowprereleasepackages)  
  Use this key to determine whether RavenDB is allowed to use pre-release versions of NuGet packages.  
{INFO/}
{CONTENT-FRAME/}

---

<a id="set-nlog-config-file" />
{CONTENT-FRAME: 3. Set your NLog configuration file}

Follow the procedure below to create and modify an NLog configuration file.  
When you're done, restart the server to apply the new settings.  

---

#### An available template:
We've created an NLog configuration file **template** for your convenience, it is 
available in the RavenDB **server** folder under the name `NLog.template.config`.  
You can copy the template and modify it by your needs, or use your own configuration 
file, as you prefer.  

---

#### Mandatory `logger` definitions:
Whether you base your configuration file on our template or use your own file, please 
be sure it includes the four _loggers_ defined in the template file's **rules** section.  
These definitions are mandatory, and failing to include them will generate an exception.  
{CODE-BLOCK:JSON}
<rules>
    <!--These loggers are mandatory-->
    <logger ruleName="Raven_System" 
        name="System.*" finalMinLevel="Warn" />
    <logger ruleName="Raven_Microsoft" 
        name="Microsoft.*" finalMinLevel="Warn" />
    <logger ruleName="Raven_Default_Audit" 
        name="Audit" levels="Info" final="true" />
    <logger ruleName="Raven_Default" 
        name="*" levels="Info,Warn,Error,Fatal" writeTo="Raven_Default_Target" />
</rules>
{CODE-BLOCK/}

---

#### Your own loggers:
The **Raven_Default** logger, for example, directs log records to **Raven_Default_Target**.  
Looking at the **Raven_Default_Target** definition (also included in the template file) we 
can see that this target outputs log data to log files in the server's **Logs** folder.  
{CODE-BLOCK:JSON}
<target xsi:type="AsyncWrapper" name="Raven_Default_Target">
  <target
    name="FileTarget"
    xsi:type="File"
    createDirs="true"
    fileName="${basedir}/Logs/${shortdate}.log"
    archiveNumbering="DateAndSequence"
    header="Date|Level|ThreadID|Resource|Component|Logger|Message|Data"
    layout="${longdate:universalTime=true}|${level:uppercase=true}|${threadid}|
            ${event-properties:item=Resource}|${event-properties:item=Component}|
            ${logger}|${message:withexception=true}|${event-properties:item=Data}"
    footer="Date|Level|Resource|Component|Logger|Message|Data"
    concurrentWrites="false"
    writeFooterOnArchivingOnly="true"
    archiveAboveSize="134217728"
    enableArchiveFileCompression="true" />
</target>
{CODE-BLOCK/}

To output log data through an NLog plugin (rather than into log files) you can either 
leave the default logger as is and modify its target, or add your own logger and target.  

To utilize a pre-installed `Loki` plugin, for example, you can -  

Add a new logger:  
{CODE-BLOCK:JSON}
<logger ruleName="Grafana_Loki" 
        name="*" levels="Info,Warn,Error,Fatal" writeTo="loki" />
{CODE-BLOCK/}

And add a new "loki" target for this logger, that specifies the logging properties 
for this destination.  
{CODE-BLOCK:JSON}
<target 
    name="loki" 
    xsi:type="loki" 
    endpoint="https://your_end_point.net" 
    username="{your_user_name}"
    password="{your_token}"
    layout="${longdate:universalTime=true}|${level:uppercase=true}|${threadid}|
            ${event-properties:item=Resource}|${event-properties:item=Component}|
            ${logger}|${message:withexception=true}|${event-properties:item=Data}">  
...
<label name="app" layout="layout" />

{CODE-BLOCK/}

{INFO: }
[For a complete guide to using Grafana Loki with RavenDB]()  
{INFO/}

{CONTENT-FRAME/}

{PANEL/}

{PANEL: Studio: Admin Logs}

Another way to view debug (or any other level) logs without having to restart 
the server, is Studio's [Admin Logs view](../../studio/server/debug/admin-logs).  

{PANEL/}

## Related Articles

### Administration

- [CLI](../../server/administration/cli)
- [log](../../server/administration/cli#log)

### Configuration

- [Overview](../../server/configuration/configuration-options)
- [settings.json](../../server/configuration/configuration-options#settings.json)
- [Logs options](../../server/configuration/logs-configuration)
- [Security options](../../server/configuration/security-configuration)
