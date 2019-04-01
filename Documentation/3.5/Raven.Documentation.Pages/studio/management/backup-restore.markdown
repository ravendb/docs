# Manage Your Server: Backup & Restore

`Backup` is used to create a backup of a database or a file system. All you need to do is give the name of a resource you want to backup, backup's location, and determine whether the backup should be incremental or not.

![Figure 1. Manage Your Server. Backup Database.](images/manage_your_server-backup_database-1.png)

<hr />

`Restore` is used to restore a backup. You need to provide a backup location and new database or file system name. 
Additionally, you may give a path to a place where the data,indexes and logs should end up 
(if you choose not to, a default path will be used), carry out defragmentation and decide to disable replication
destinations after the successful restore.

![Figure 2. Manage Your Server. Restore Database.](images/manage_your_server-restore_database-2.png)
