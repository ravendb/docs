# Configuration: Backup

---

{NOTE: }

* Configuration options for backups (both on premise and RavenDB Cloud).  

* Another relevant server configuration option can be found [here](../../server/configuration/server-configuration#server.cpucredits.exhaustionbackupdelayinmin).

* In this page:  
    * [Backup.TempPath](../../server/configuration/backup-configuration#backup.temppath)  
    * [Backup.LocalRootPath](../../server/configuration/backup-configuration#backup.localrootpath)  
    * [Backup.AllowedDestinations](../../server/configuration/backup-configuration#backup.alloweddestinations)  
    * [Backup.AllowedAwsRegions](../../server/configuration/backup-configuration#backup.allowedawsregions)  
    * [Backup.MaxNumberOfConcurrentBackups](../../server/configuration/backup-configuration#backup.maxnumberofconcurrentbackups)  
    * [Backup.ConcurrentBackupsDelayInSec](../../server/configuration/backup-configuration#backup.concurrentbackupsdelayinsec)  
    * [Backup.LowMemoryBackupDelayInMin](../../server/configuration/backup-configuration#backup.lowmemorybackupdelayinmin)  

{NOTE/}

---

{PANEL:Backup.TempPath}

Use this setting to specify a different path to the temporary backup files.  
By default, backup temporary files are written under the Database directory or under [Storage.TempPath](../../server/configuration/storage-configuration) if defined.  
Learn more about RavenDB directory structure [here](../../server/storage/directory-structure).

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Backup.LocalRootPath}

Local backups can only be created under this root path.  

- **Type**: `string`  
- **Default**: `null`  
- **Scope**: Server-wide only  

{PANEL/}

{PANEL:Backup.AllowedDestinations}

Semicolon separated list of allowed backup destinations. If not specified, all destinations are allowed.   

- **Type**: `string`  
- **Default**: `null`  
- **Scope**: Server-wide only  

Possible values:  

- `None`  
- `Local`  
- `Azure`  
- `AmazonGlacier`  
- `AmazonS3`  
- `FTP`  

{PANEL/}

{PANEL:Backup.AllowedAwsRegions}

Semicolon separated list of allowed AWS regions. If not specified, all regions are allowed.  

- **Type**: `string`  
- **Default**: `null`  
- **Scope**: Server-wide only  

{PANEL/}

{PANEL:Backup.MaxNumberOfConcurrentBackups}

Maximum number of concurrent backup tasks.  

- **Type**: `int`  
- **Default**: the number of CPU cores assigned to this server, divided by 2.  
  (By default a server with 1, 2, or 3 CPU cores can perform 1 backup at a time. A server with 4 or 5 cores can perform 2 backups at a time, 6 or 7 cores can perform 3, and so on)
- **Scope**: Server-wide only  

{PANEL/}

{PANEL:Backup.ConcurrentBackupsDelayInSec}

Number of seconds to delay the backup after hitting the maximum number of concurrent backups limit (see `MaxNumberOfConcurrentBackups` above).  

- **Type**: `TimeSetting`  
- **TimeUnit**: `TimeUnit.Seconds`  
- **Default**: `30`  
- **Scope**: Server-wide only  

{PANEL/}

{PANEL:Backup.LowMemoryBackupDelayInMin}

Number of minutes to delay the backup if the server enters a low memory state.  

- **Type**: `TimeSetting`  
- **TimeUnit**: `TimeUnit.Minutes`  
- **Default**: `10`  
- **Scope**: Server-wide only  

{PANEL/}

## Related Articles  

### Cloud  
- [Backup and Restore](../../cloud/cloud-backup-and-restore)  

### Client API  
- [Backup](../../client-api/operations/maintenance/backup/backup)  

### Server  
- [Backup Overview](../../server/ongoing-tasks/backup-overview)  
- [Server Configuration](../../server/configuration/server-configuration)  
