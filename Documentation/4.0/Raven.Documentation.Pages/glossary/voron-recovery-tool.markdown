# Voron Recovery Tool

### What is it?
Voron recovery tool is designed to extract your data even on the worst corruption state imaginable.

### How to run it?
For Windows, the syntax to run will be:
{CODE-BLOCK:powershell}
Voron.Recovery.exe recover <Voron data-file directory> <Recovery directory>
{CODE-BLOCK/}

For Linux, the syntax to run will be:
{CODE-BLOCK:powershell}
dotnet Voron.Recovery.dll recover <Voron data-file directory> <Recovery directory>
{CODE-BLOCK/}

The process may take some time to run, depending on how fast the storage is.
The tool will create a `recovery.ravendump` file and a final report stating the number of recovered documents and attachments, it will also state the number of corrupted pages found so the size of lost data can be estimated.

The file `recovery.ravendump` has the standard RavenDB export format, and can be imported to RavenDB.

{NOTE Recovery of encrypted data files is not supported at the moment.}

### Additional flags
`--OutputFileName`: overwrite the default output file name

`--PageSizeInKB`: overwrite the expected Voron page size of 8KB, should never be used unless told by the support team.

`--InitialContextSizeInMB`: overwrite the starting size of memory used by the recovery tool, default is 4KByte.

`--InitialContextLongLivedSizeInKB`: overwirte the starting size of memory used by the recovery tool for long lived objects, default is 4KByte.

`--ProgressIntervalInSec`: overwrite the time interval in which the recovery tool refreshes the report in the console.

`--DisableCopyOnWriteMode`: disables the copy on write. This option should be used when recovering the journals failed, which would happen most likely due to them being corrupted. In this case, the error indicating corruption of journals will be thrown by the Voron engine, and this will stop the recovery process.
{NOTE The data-file should be backed up before using this option. /}

`--LoggingMode`: controls the logging level, either `Operations` or `Information` are valid values.


