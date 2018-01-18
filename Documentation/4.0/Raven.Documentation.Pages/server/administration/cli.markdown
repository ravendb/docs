# RavenDB CLI
- Running RavenDB as a console application provides basic information along with a command line interface which can be used for getting additional information about the server and in performing specific commands.

- RavenDB can operate as service/daemon without console input. It is possible to access the CLI through a provided `rvn` (`rvn.exe` in Windows) tool included in each distribution package. The process is as follows:
{CODE-BLOCK:plain}
rvn admin-channel [RavenDB process ID]
{CODE-BLOCK/}
*The rvn executable can be found in the distribution package under 'Server' directory*

- ***rvn admin-channel*** uses [Named Pipe Connection](https://en.wikipedia.org/wiki/Named_pipe), and can connect to RavenDB CLI only when running on the same machine as the server, and with appropriate privileges.

<br><br>

### CLI Information and Execution Commands

#### info

Usage : **info**

Prints basic information to the console, including build version information, process ID (PID), bitness, and system hardware information.

#Example:

{CODE-BLOCK:plain}
ravendb> info
  Node A in cluster eabe7a24-054a-48ef-9391-7f7b7707969d
  Build 40050, Version 4.0, SemVer 4.0.0, Commit fffffff
  PID 17591, 64 bits, 8 Cores, Arch: X64
  31.122 GBytes Physical Memory, 28.908 GBytes Available Memory
  Using GC in server concurrent mode retaining memory from the OS.
{CODE-BLOCK/}

#### stats

Usage : **stats**

Online display of memory usage by RavenDB. You can separate into Working Set, Native Mem, Managed Mem, and Memory Mapped Size. Hitting any key will return to CLI's input mode (beware not to hit Ctrl+C / Break to avoid unintended a shutdown of the server).

#Example:

{CODE-BLOCK:plain}
ravendb> stats
  Showing stats, press any key to close...
    working set     | native mem      | managed mem     | mmap size         | reqs/sec       | docs (all dbs)
 +  201.45 MBytes   | 17.36 MBytes    | 42.45 MBytes    | 2.02 GBytes       | 0              |      5,374,826
{CODE-BLOCK/}

#### log

Usage : **log <on | off\>**  or **log <http-on | http-off\>**
enable/disable online log printing to the console (http-on/off to enable/disable only http requests log information).

#Example:

{CODE-BLOCK:plain}
ravendb>log on
{CODE-BLOCK/}

* Note : If log enabled using ***rvn admin-channel***, the information will be displayed in the main console application. If RavenDB is running as a service, you will see the log output in the service log.

#### gc

Usage: gc [gen]

Force Garbage Collection to a specific generation 0, 1 or 2.  See [GC.Collect Method](https://msdn.microsoft.com/en-us/library/y46kxc5e(v=vs.110).aspx)

#Example:

{CODE-BLOCK:plain}
ravendb> gc 2
Before collecting, managed memory used: 48.92 MBytes
Garbage Collecting... Collected.
After collecting, managed memory used:  10.09 MBytes at 0.0078154 Seconds
{CODE-BLOCK/}

#### resetServer / shutdown / q

Gracefully shuts down RavenDB. `resetServer` will restart RavenDB after shutdown. 

# Example:

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

#### script

Usage : **script <server | database\> [database name]**

Executes Admin JavaScript patches. 

* Attention: Running scripts can harm the database beyond recovery. Use with care!!!

#Example:
{CODE-BLOCK:plain}
ravendb> script database ProductionDB

Enter JavaScript:
(to cancel enter in new line 'cancel' or 'EXEC' to execute)

&gt;&gt;&gt; return database.Configuration.Storage.MaxConcurrentFlushes
&gt;&gt;&gt; EXEC
{ "Result" : 10 }
ravendb> 
{CODE-BLOCK/}

#### addServerCert

Usage : **addServerCert <path\> [password]**

Adding another node's server certificate to be trusted on this server. This is required when building a cluster where each node has a different certificate.

<br><br>

### CLI Debugging Commands

The following commands are intended for debugging use only!

#### lowMem

Usage **lowMem**

Simulates low memory state in RavenDB

#### timer

Usage: **timer <on | off | fire\>**

Enable/Disable candidate selection timer (Rachis), or fires timeout immediately

<br><br>

### Miscellaneous Commands

#### clear
Usage: **clear**
Clear screen.

#### prompt
For usage type **helpPrompt**
Can be used to show memory information used by **stats** while using `rvn`

#Example :
{CODE-BLOCK:plain}
prompt %M
{CODE-BLOCK/}

#### logout
Exit CLI back to the terminal (with `rvn admin-channel` use only)

#### logo
Prints initial logo

#### help
Display help screen
