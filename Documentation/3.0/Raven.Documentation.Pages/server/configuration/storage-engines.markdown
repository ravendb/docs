# Configuration : Storage Engines

{PANEL}

RavenDB supports two storage engines and each database can be created with a different one:

- _(Default)_ ESE (Extensible Storage Engine aka Esent) - Microsoft storage engine available in Windows. More info [here](http://en.wikipedia.org/wiki/Extensible_Storage_Engine).
- Voron - custom made managed storage engine created by Hibernating Rhinos.

{PANEL/}

{PANEL:**Voron**}

### Limitations

- no support for DTC transactions
- no support for [compaction](../../client-api/commands/how-to/compact-database)

### Requirements

- disk must handle UNBUFFERED_IO/WRITE_THROUGH properly
- .NET 4.5 or higher
- [Hotfix](http://support.microsoft.com/kb/2731284) for Windows 7 and Windows Server 2008 R2

{PANEL/}

{PANEL:**Using Studio**}

To change storage engine in Studio, during database creation go to `Advanced Settings` and change `Storage Engine` according to your needs.

![Figure 1: Creating database and changing Storage Engine](images/create-database-select-engine-studio.png)

{PANEL/}