# Configuration: Storage

The following configuration options allow you configure [the storage engine](../../server/storage/storage-engine).

{PANEL:Storage.TempPath}

You can use this setting to specify a different path to temporary files. By default, it is empty, which means that temporary files will be created at same location as data file under the `Temp` directory.

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Storage.TransactionsModeDurationInMin}

How long transaction mode (Danger/Lazy) lasts before returning to Safe Mode. Value in minutes with default set to 1440 (24 hours). Set value to 0 for infinite.

- **Type**: `int`
- **Default**: `1440`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Storage.MaxConcurrentFlushes}

Maximum concurrent flushes.

- **Type**: `int`
- **Default**: `10`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Storage.TimeToSyncAfterFlushInSec}

Time to sync after flush in seconds

- **Type**: `int`
- **Default**: `30`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Storage.NumberOfConcurrentSyncsPerPhysicalDrive}

Number of concurrent syncs per physical drive.

- **Type**: `int`
- **Default**: `3`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Storage.CompressTxAboveSizeInKb}

Compress transactions above size (value in KB)

- **Type**: `int`
- **Default**: `512`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Storage.ForceUsing32BitsPager}

Use the 32 bits memory mapped pager even when running on 64 bits.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Storage.MaxScratchBufferSizeInMb}

Maximum size of `.buffers` files

- **Type**: `int`
- **Default**: `256` when running on 64 bits, `32` when running on 32 bits or `Storage.ForceUsing32BitsPager` is set to `true`
- **Scope**: Server-wide or per database

{PANEL/}


{PANEL:Storage.PrefetchBatchSizeInKb}

Size of the batch in kilobytes that will be requested to the OS from disk when prefetching (value in powers of 2). Some OSs may not honor certain values. Experts only.

- **Type**: `int`
- **Default**: `1024`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Storage.PrefetchResetThresholdInGb}

How many gigabytes of memory should be prefetched before restarting the prefetch tracker table. Experts only.

- **Type**: `int`
- **Default**: `8`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Storage.OnDirectoryInitialize.Exec}

A command or executable to run when creating/opening a directory (storage environment). Experts only.  
RavenDB will execute:  
{CODE-BLOCK:plain}
command [user-arg-1] ... [user-arg-n] <environment-type> <database-name> <data-dir-path> <temp-dir-path> <journal-dir-path>  
{CODE-BLOCK/}

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Storage.OnDirectoryInitialize.Exec.Arguments}

The optional user arguments for the 'Storage.OnDirectoryInitialize.Exec' command or executable. The arguments must be escaped for the command line. Experts only.  

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Storage.OnDirectoryInitialize.Exec.TimeoutInSec}

The number of seconds to wait for the OnDirectoryInitialize executable to exit. Default: 30 seconds. Experts only.  

- **Type**: `int`
- **Default**: `30`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Storage.Dangerous.SkipChecksumValidationOnDatabaseLoading}

Skip checksum validation on database loading process (applicable only for ARM 32/64). Default: false. Experts only.
Should be set to `true` in order to skip checksum verification of modifications in database recovery, if process takes too long (checksum calculation on some ARM machines is somewhat slower sometimes)
Although gaining faster recovery time and server startup time, skipping checksum validation may cause later report on corrupted data if one was exists during historical revocery.

- **Type**: `bool`
- **Default**: false
- **Scope**: Server-wide

{PANEL/}

## Related Articles

- [Storage Engine](../../server/storage/storage-engine)
