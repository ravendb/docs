﻿# Backup and Restore

RavenDB supports backup and restore. Backups can be performed while the database in online and accepting requests (including writes). 

There are two ways to perform a backup:

* Using your existing enterprise backup solution. RavenDB supports VSS backups, which is how most backup solutions on Windows work. You can do that by configuring your backup solution to take backups of Raven's data directory. 
* Using RavenDB's own backup & restore system. You can ask a RavenDB server to perform a complete backup of its data to a specified directory at any time. During the backup procedure, the database remains online and can respond to read and write requests. However, it is the state of the database at the start of the backup.

Unlike backups, restores are offline operation. You cannot restore to a running database (indeed, the notion makes little sense).

## Backward compatibility

RavenDB relies on OS services to manage data storage and backup. Those services are forward compatible (if you backup on Windows XP you can restore on Windows 7) but not  backward compatible (if you backup on Windows 2008 you cannot restore on Windows 2003).

If you are looking to move a database content between different Operating System versions, you should use the Import / Export function, done using Raven.Smuggler.

## Initiating a backup

When running in embedded mode, all you need is to call the method `DocumentDatabase.StartBackup()`.

If running in a client/server mode, you can use Raven.Backup.exe to perform manual or scheduled backups, or access the backup endpoint directly using HTTP.

If running from IIS, make sure to enable Windows Authentication for RavenDB's IIS application.

Only one backup operation may run at any given time.

{NOTE The utility has to be run with administrative privileges /}

### Using the Raven.Backup utility

The utility `Raven.Backup.exe` is available from the `/Tools` folder in both the distribution and nuget packages.

Usage example:

{CODE-START:plain /}
    Raven.Backup.exe --url=http://localhost:8080/ --dest=C:\Temp\Foo --nowait --readkey
{CODE-END /}

The backup utility will output the progress to the console window, pinging the server to get updated progress unless ordered otherwise.

Parameters are as follows:

* url - Required. The RavenDB server URL. Backups will not work with Embedded databases.
* dest - Required. Backup destination. Has to be writable.
* nowait - Optional. By default the utility will ping the server and wait until backup is done, specifying this flag will make the utility return immediately after the backup process has started.
* readkey - Optional. Specifying this flag will make the utility wait for key press before exiting.
* incremental - Optional. When specified, the backup process will be incremental when done to a folder where a previous backup lies. If `dest` is an empty folder, or it does not exist, a full backup will be created. For incremental backups to work, the configuration option `Raven/Esent/CircularLog` has to be set to false.

### Using the HTTP API

Alternatively, you can send an HTTP POST command to the `/admin/backup` endpoint as follows:

{CODE-START:json /}
    curl -X POST http://localhost:8080/admin/backup -d "{ 'BackupLocation': 'C:\\Backups\\2010-05-06' }"
{CODE-END /}

The backup operation is asynchronous. The backup process will start, and the request will complete before the backup process is completed.

You can check the status of the backup by querying the document with the key: "Raven/Backup/Status". The backup is completed when the IsRunning field in the document is set to false. The backup current status can be tracked by querying the backup status document, this includes any errors that occur during the backup process.

## How to restore a database

Restoring a database is an offline operation, it cannot operate on a running instance of RavenDB.

In embedded mode, you can initiate the restore operation by calling `DocumentDatabase.Restore()`, or through the command line:

{CODE-START:plain /}
    Raven.Server.exe -src [backup location] -dest [restore location] -restore
{CODE-END /}
    
If the restore location doesn't exists, RavenDB will create it.

You cannot restore to an existing database data directory, the restore operation will fail if it detects that the restore operation will overwrite existing data. If you need to restore to an existing database data directory, shutdown the database instance and delete the data directory.

Unlike backups, restores are fully synchronous.