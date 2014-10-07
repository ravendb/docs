# Administration : Backup and Restore

{PANEL:**How to backup a database?**}

Backups can be performed while the database in online and accepting requests (including writes) and there are two ways to perform a backup:

* Using your existing enterprise backup solution. RavenDB supports VSS backups, which is how most backup solutions on Windows work. You can do that by configuring your backup solution to take backups of Raven's data directory. 
* Using RavenDB's own backup & restore system. You can ask a RavenDB server to perform a complete backup of its data to a specified directory at any time. During the backup procedure, the database remains online and can respond to read and write requests. However, it is the state of the database at the start of the backup.

### Backward compatibility

RavenDB relies on OS services to manage data storage and backup. Those services are forward compatible (if you backup on Windows XP you can restore on Windows 7) but not  backward compatible (if you backup on Windows 2008 you cannot restore on Windows 2003).

If you are looking to move a database content between different Operating System versions, you should use the [Import / Export](../server/administration/exporting-and-importing) function, done using Raven.Smuggler.

<hr />

### Initiating a backup

When running in embedded mode, all you need is to call the method `DocumentDatabase.Maintenance.StartBackup()`.

If running in a client/server mode, you can use Raven.Backup.exe to perform manual or scheduled backups, or access the backup endpoint directly using HTTP.

If running from IIS, make sure to enable Windows Authentication for RavenDB's IIS application.

Only one backup operation may run at any given time.

<hr />

### Using the Raven.Backup utility

The utility `Raven.Backup.exe` is available from the `/Tools` folder in both the distribution and nuget packages.

Usage example:

{CODE-BLOCK:plain}
    Raven.Backup.exe --url=http://localhost:8080/ --database=Northwind --dest=C:\Temp\Foo --nowait --readkey
{CODE-BLOCK/}

The backup utility will output the progress to the console window, pinging the server to get updated progress unless ordered otherwise.

Parameters are as follows:

* `url` - The URL to database that will be backuped. Server root URL (e.g. http://localhost:8080/) will point to `system` database, to point to a **specific database** please use `/databases/<database_name>` endpoint (e.g. to backup `ExampleDB` use http://localhost:8080/databases/ExampleDB).     
* `dest` - Backup destination. Has to be writable.
* _(Optional)_ `database` - The database to operate on. If no specified, the operations will be on the default database.
* _(Optional)_ `nowait` - By default the utility will ping the server and wait until backup is done, specifying this flag will make the utility return immediately after the backup process has started.
* _(Optional)_ `readkey` - Specifying this flag will make the utility wait for key press before exiting.
* _(Optional)_ `incremental` - When specified, the backup process will be incremental when done to a folder where a previous backup lies. If `dest` is an empty folder, or it does not exist, a full backup will be created. For incremental backups to work, the configuration option `Raven/Esent/CircularLog` has to be set to false.
* _(Optional)_ `timeout` - Timeout (in milliseconds) to use for requestes.
* _(Optional)_ `username`, `password`, `domain`, `api-key` - credentials used when authentication is requred.

#### Remarks

{INFO:Information}

- Backups are not supported for Embedded databases when `Raven.Backup.exe` is used.
- `Raven.Backup.exe` has to be run with administrative privileges.

{INFO/}

<hr />

### Using Client API

Alternatively, you can use Client API to start backup. API reference can be found [here](../../client-api/commands/how-to/start-backup-restore-operations).

The backup operation is asynchronous. The backup process will start, and the request will complete before the backup process is completed.

You can check the status of the backup by querying the document with the key: "Raven/Backup/Status". The backup is completed when the IsRunning field in the document is set to false. The backup current status can be tracked by querying the backup status document, this includes any errors that occur during the backup process.

{PANEL/}

{PANEL:**How to restore a database?**}

### Restoring SYSTEM database

Restoring a **system** database is an **offline operation**, it cannot operate on a running instance of RavenDB.

{CODE-BLOCK:plain}
    Raven.Server.exe --restore-source=[backup location] --restore-destination=[restore location] --restore-system-database
{CODE-BLOCK/}

- `restore-source=PATH` - path to a folder where database backup is present.
- `restore-destination=PATH` - path to a folder where database will be restored.
- _(Optional)_ `restore-defrag` - indicates if database should be defragmented after restore.
- `restore-system-database` - marks that SYSTEM database will be restored.
 
{INFO:Information}

- Unlike backups, **system** database restores are fully synchronous.
- In embedded mode, you can initiate the restore operation by calling `Raven.Database.Actions.MaintenanceActions.Restore()` or by using `Raven.Server.exe` restore functions described above.

{INFO/}

<hr />

### Restoring non-SYSTEM database

Restoring a **non-SYSTEM** database is an **online operation**, it means that the destination server must be running. Remember, that only one restore operation can be running simultaneously.

{CODE-BLOCK:plain}
    Raven.Server.exe --restore-source=[backup location] --restore-database=[URL to destination server]
{CODE-BLOCK/}

- `restore-source=PATH` - path to a folder where database backup is present (on destination server).
- _(Optional)_ `restore-destination=PATH` - path to a folder where database will be restored (on destination server). If not specified it will be located in default data directory.
- _(Optional)_ `restore-defrag` - indicates if databases should be defragmented after restore.
- _(Optional)_ `restore-database-name=NAME` - the name of the new database. If not specified, it will be extracted from backup.
- `restore-database=URL` - marks that non-SYSTEM database will be restored and sets the URL to the destination server.
- _(Optional)_ `restore-no-wait` - Return immediately without waiting for a restore to complete.
- _(Optional)_ `restore-start-timeout=TIMEOUT` - The maximum timeout in seconds to wait for another restore to complete. Default: 15 seconds.

### Remarks

{INFO:Information}

- If the restore location doesn't exists, server will create it.
- By default, **non-SYSTEM** restore will wait for operation to complete. This behavior can be altered by using `--restore-no-wait` option.

{INFO/}

{WARNING:Warning}
You cannot restore to an existing database data directory, the restore operation will fail if it detects that the restore operation may overwrite existing data. If you need to restore to an existing database data directory, shutdown the database instance and delete the data directory.
{WARNING/}

{DANGER Simultaneous restore operations are forbidden. /}

{PANEL/}

## Bundles

{WARNING:Encryption & Backup} 
The backup of an encrypted database contains the encryption key (`Raven/Encryption/Key`) as a plain text. This is required to make RavenDB able to restore the backup on a different machine.
{WARNING/}

## Related articles

TODO