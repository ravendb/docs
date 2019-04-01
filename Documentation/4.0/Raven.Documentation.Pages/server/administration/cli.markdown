# Administration: RavenDB CLI

Running RavenDB as a console application provides basic information along with a command line interface which can be used for getting additional information about the server and in performing specific commands.

RavenDB can operate as service/daemon without console input. It is possible to access the CLI through a provided `rvn` (`rvn.exe` in Windows) tool included in each distribution package. The process is as follows:

{CODE-BLOCK:bash}
rvn admin-channel [RavenDB process ID]
{CODE-BLOCK/}

The `rvn admin-channel` uses [Named Pipe Connection](https://en.wikipedia.org/wiki/Named_pipe), and can connect to RavenDB CLI only when running on the same machine as the server, and with appropriate privileges.

{INFO The `rvn` executable can be found in the distribution package under **Server** directory /}

<hr />

{PANEL:info}

Prints basic information to the console, including build version information, process ID (PID), bitness, and system hardware information.

{CODE-BLOCK:plain}
ravendb> info
  Node A in cluster eabe7a24-054a-48ef-9391-7f7b7707969d
  Build 40050, Version 4.0, SemVer 4.0.0, Commit fffffff
  PID 17591, 64 bits, 8 Cores, Arch: X64
  31.122 GBytes Physical Memory, 28.908 GBytes Available Memory
  Using GC in server concurrent mode retaining memory from the OS.
{CODE-BLOCK/}

{PANEL/}

{PANEL:stats}

Online display of memory usage by RavenDB. You can separate into Working Set, Native Mem, Managed Mem, and Memory Mapped Size. Hitting any key will return to CLI's input mode (beware not to hit Ctrl+C / Break to avoid unintended a shutdown of the server).

{CODE-BLOCK:plain}
ravendb> stats
  Showing stats, press any key to close...
    working set     | native mem      | managed mem     | mmap size         | reqs/sec       | docs (all dbs)
 +  201.45 MBytes   | 17.36 MBytes    | 42.45 MBytes    | 2.02 GBytes       | 0              |      5,374,826
{CODE-BLOCK/}

{PANEL/}

{PANEL:log}

Enable (or disable) online log printing to the console.

{CODE-BLOCK:plain}
log <on|off|http-on|http-off|none|operations|information>
{CODE-BLOCK/}

| Parameters | |
| ------------- | ------------- |
| `on` or `off` | enables or disables log printing |
| `http-on` or `http-off` | enables or disables HTTP request log information |
| `none`, `operations` or `information` | sets the log mode to desired [level](../../server/configuration/logs-configuration#logs.mode) |

{CODE-BLOCK:plain}
ravendb> log on
{CODE-BLOCK/}

{NOTE If log is enabled using `rvn admin-channel`, the information will be displayed in the main console application. If RavenDB is running as a service, you will see the log output in the service log. /}

{PANEL/}

{PANEL:gc}

Force Garbage Collection to a specific generation (0, 1 or 2).  See [GC.Collect Method](https://docs.microsoft.com/en-us/dotnet/api/system.gc.collect?redirectedfrom=MSDN&view=netframework-4.7.2#System_GC_Collect_System_Int32_)

{CODE-BLOCK:plain}
gc <0|1|2>
{CODE-BLOCK/}

{CODE-BLOCK:plain}
ravendb> gc 2
Before collecting, managed memory used: 48.92 MBytes
Garbage Collecting... Collected.
After collecting, managed memory used:  10.09 MBytes at 0.0078154 Seconds
{CODE-BLOCK/}

{PANEL/}

{PANEL:shutdown | q}

Gracefully shuts down the Server.

{CODE-BLOCK:plain}
ravendb> q

Are you sure you want to reset the server ? [y/N] : y
Starting shut down...
Shutdown completed
{CODE-BLOCK/}

{PANEL/}

{PANEL:resetServer}

Gracefully shuts down and restarts the Server.

{CODE-BLOCK:plain}
ravendb> resetServer

Are you sure you want to reset the server ? [y/N] : y
Starting shut down...
Shutdown completed

Restarting Server...
Using GC in server concurrent mode retaining memory from the OS.
Node A in cluster eabe7a24-054a-48ef-9391-7f7b7707969d
Server available on: http://rave-pc:8080
Tcp listening on 0.0.0.0:32797
Server started, listening to requests...
TIP: type 'help' to list the available commands.
ravendb> 
{CODE-BLOCK/}

{PANEL/}

{PANEL:script}

Executes Admin JavaScript patches.

{CODE-BLOCK:plain}
script <server|database> [database name]
{CODE-BLOCK/}

{CODE-BLOCK:plain}
ravendb> script database ProductionDB

Enter JavaScript:
(to cancel enter in new line 'cancel' or 'EXEC' to execute)

&gt;&gt;&gt; return database.Configuration.Storage.MaxConcurrentFlushes
&gt;&gt;&gt; EXEC
{ "Result" : 10 }
ravendb> 
{CODE-BLOCK/}

{DANGER Running scripts can harm the database beyond recovery. Use with care. /}

{PANEL/}

{PANEL:generateClientCert}

Generate a new trusted client certificate with `ClusterAdmin` security clearance.

{CODE-BLOCK:plain}
ravendb> generateClientCert <name> <path-to-output-folder> [password]
{CODE-BLOCK/}

{PANEL/}

{PANEL:trustServerCert}

Register a server certificate of another node to be trusted on this server. This is required when building a cluster where each node has a different certificate.

{CODE-BLOCK:plain}
ravendb> trustServerCert <name> <path-to-pfx> [password]
{CODE-BLOCK/}

{PANEL/}

{PANEL:trustClientCert}

Register a client certificate to be trusted on this server with `ClusterAdmin` security clearance.

{CODE-BLOCK:plain}
ravendb> trustClientCert <name> <path-to-pfx> [password]
{CODE-BLOCK/}

{PANEL/}

{PANEL:replaceClusterCert}

Replace the cluster certificate.  
{DANGER If **replaceImmediately** is specified, RavenDB will replace the certificate by force, even if some nodes are not responding. In that case, you will have to manually replace the certificate in those nodes. Use with care. /}

{CODE-BLOCK:plain}
ravendb> replaceClusterCert [-replaceImmediately] <name> <path-to-pfx> [password]
{CODE-BLOCK/}

{PANEL/}

<hr />

{PANEL:clear}

Clears the screen.

{CODE-BLOCK:plain}
ravendb> clear
{CODE-BLOCK/}

{PANEL/}

{PANEL:prompt}

For usage type **helpPrompt**.

Can be used to show memory information used by **stats** while using `rvn`.

{CODE-BLOCK:plain}
ravendb> prompt %M
{CODE-BLOCK/}

{PANEL/}

{PANEL:logout}

Exits CLI back to the terminal (with `rvn admin-channel` use only).

{CODE-BLOCK:plain}
ravendb> logout
{CODE-BLOCK/}

{PANEL/}

{PANEL:logo}

Prints initial logo.

{CODE-BLOCK:plain}
ravendb> logo
{CODE-BLOCK/}

{PANEL/}

{PANEL:help}

Display help screen.

{CODE-BLOCK:plain}
ravendb> help
{CODE-BLOCK/}

{PANEL/}

<hr />

{PANEL:lowMem}

{WARNING Debugging command. Not intended for normal use. /}

Simulates low memory state in RavenDB.

{CODE-BLOCK:plain}
ravendb> lowMem
{CODE-BLOCK/}

{PANEL/}

{PANEL:timer}

{WARNING Debugging command. Not intended for normal use. /}

Enable (or disable) candidate selection timer (Rachis), or fires timeout immediately.

{CODE-BLOCK:plain}
timer <on|off|fire>
{CODE-BLOCK/}

{PANEL/}

## Related articles

### Configuration

- [Configuration Options](../../server/configuration/configuration-options)
- [Command Line Arguments](../../server/configuration/command-line-arguments)
