# Configuration : Storage Engines

RavenDB supports two storage types and each database can be created with a different engine:

- _(Default)_ ESE (Extensible Storage Engine aka Esent) - Microsoft storage engine available in Windows. More info [here](http://en.wikipedia.org/wiki/Extensible_Storage_Engine).
- Voron - custom made managed storage engine created by Hibernating Rhinos.

## Voron - requirements

- Disk must handle UNBUFFERED_IO/WRITE_THROUGH properly
- .NET 4.5 or higher
- [Hotfix](http://support.microsoft.com/kb/2731284) for Windows 7 and Windows Server 2008 R2

## Using Studio

To change storage engine in Studio, during database creation go to `Advanced Settings` and change `Storage Engine` according to your needs.

![Figure 1: Creating database and changing Storage Engine](images/create-database-select-engine-studio.png)