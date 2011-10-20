# Backup

You can use Raven.Backup.exe to perform manual or scheduled backups. This utility is available from the `/Tools` folder in both the distribution and nuget packages.

{NOTE The utility has to be run with administrive privileges /}

Example:

    Raven.Backup.exe --url=http://localhost:8080/ --dest=C:\Temp\Foo --nowait --readkey

The backup utility will output the progress to the console window, pinging the server to get updated progress unless ordered otherwise.

Parameters are as follows:

* url - Required. The RavenDB server URL. Backups will not work with Embedded databases.
* dest - Required. Backup destination. Has to be writable.
* nowait - Optional. By default the utility will ping the server and wait until backup is done, specifying this flag will make the utility return immediately after the backup process has started.
* readkey - Optional. Specifying this flag will make the utility wait for key press before exiting.