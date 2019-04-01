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

{PANEL:Storage.TimeToSyncAfterFlashInSec}

Time to sync after flash in seconds

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

## Related Articles

- [Storage Engine](../../server/storage/storage-engine)
