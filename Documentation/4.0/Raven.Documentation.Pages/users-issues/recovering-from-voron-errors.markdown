#Recovering from Voron errors 

##Symptoms: 
- RavenDB server side error messages like:
    - VoronUnrecoverableErrorException: `Index points to a non leaf page`
    - VoronUnrecoverableErrorException: `Was unable to retrieve the correct node. Data corruption possible`
    - VoronUnrecoverableErrorException: `Error syncing the data file. The last sync tx is...`

##Cause:
1. Hardware failure.
2. File system doesn't support `fsync`.

##Resolution:
1. Identify and replace faulty disk.
2. Restore your database from backup.
3. If you don't have a recent backup use the Voron's recovery tool to extract your data as smuggler dump and import it to a newly created database.

##Voron recovery

Voron recovery tool is designed to extract your data even on the worst corruption state imaginable.
The ussage it pretty simple, you invoke Voron.Recovery.exe <Voron data-file directory> <Revcovery directory> and it should generate a `recovery.ravendump` under the selected direcory.
The process may take a while to run, a few seconds per GByte of data.
Voron.Recovery will produce a final report stating the amound of documents and attachments recovered, it will also state the number of corrupted pages found so you could estimate the size of lost data.

Additional flags:

`--OutputFileName`: overwrite the default output file name

`--PageSizeInKB`: overwrite the expected Voron page size of 8KB, should never be used unless told by the support team.

`--InitialContextSizeInMB`: overwrite the starting size of memory used by the recovery tool, default is 4KByte.

`--InitialContextLongLivedSizeInKB`: overwirte the starting size of memory used by the recovery tool for long lived objects, default is 4KByte.

`--ProgressIntervalInSec`: overwrite the time interval in which the recovery tool refreshes the report in the console.

`--DisableCopyOnWriteMode`: disables the copy on write, this should be used when recovering the journals failed (probably because they are corrupted), you should backup the data-file before using this option.

`--LoggingMode`: controls the logging level, either Operations or `Information` are `valid`.

