#How file synchronization works?

A synchronization of a single file is a multi-step process. The synchronization is started when a file is modified and it is considered as completed
if a remote file system confirms that it has the same file on its side. The synchronization is initialized by the file system where the file is changed - *a source*. 
It pushes all data needed to synchronize the file to the remote file system called *a destination*. One source file system can have multiple destinations. 

##Destinations

The list of destination servers is kept as configuration item under `Raven/Synchronization/Destinations` key. Adding a new destination file system
will automatically start to synchronize files there. If you want to stop syncing to some file system you can just drop it from the list or setting `Enabled`
property to `false`. The easiest way to manipulate destination servers is to do it by using the studio.

{CODE-BLOCK:json}
{
    "Destinations": 
		[
			{
				"ServerUrl": "http://localhost:8080",
				"Username": null,
				"Password": null,
				"Domain": null,
				"ApiKey": null,
				"FileSystem": "Northwind",
				"Enabled": true
			}
		]
}
{CODE-BLOCK/}

##Available topologies

If you setup a file system to synchronize files in one direction to an another raven file system then they work in the *master/slave* model.
Any changes made to the master will be propagated to the destination, but changes made on the slave server will not be reflected on the master.

RavenFS also supports *master/master* synchronization scenario. Then any file modification on any server will be propagated to other masters.
Be aware that such configuration can result in conflicts between different versions of the same file ([read more about conflicts]()).


##Synchronization process pipeline

Any file modification triggers the synchronization task work. It goes through the list of active destinations and performs the following actions
for each of them:

1. It queries a remote file system for the last synchronized file (actually its `Etag`).
2. For each file that changed since last synchronization (which have greater `Etags`):
 * it retrieves a files's metadata from the destination
 * it determines what kind of synchronization work has to be done based on just downloaded metadata (see [Synchronization types](synchronization-types))
 * it adds an appropriate synchronization work item to a pending synchronization queue
3. It runs as many file synchronizations as it can concurrently (there is a configurable limit of simultaneous synchronizations for a single destination - `Raven-Synchronization-Limit`).

{INFO: Synchronization types}
Depending on the determined synchronization work type, different data will be sent to a destination server to synchronize a file.
{INFO/}

{INFO: Keeping synchronization}
Each finishing synchronization always attempts to get next file from the synchronization queue in order to synchronize it. This way it performs
concurrently as many synchronizations as possible according to the limit.
{INFO/}

#File locking and synchronization timeout

When a destination file system is in the middle of synchronization process of a give file, then it denies to perform any other operation on it.
An attempt to modify it will result it  `PreconditionFailed (412)` response. Behind the scenes there is a locking mechanism which creates
`SyncingLock-[filename]` configuration when synchronization starts and as long as it exists, the file is not accessible. 
The file lock is removed at the end of the file synchronization process. 

In order to avoid potential deadlocks (e.g. when server restarts in the middle of the synchronization) we have timeouts. You can control their value 
by specifying in `Raven-Synchronization-Lock-Timeout` configuration. If the synchronization limit reaches the timeout value, then accessing the file
will automatically unlock it. By default the synchronization timeout is 10 minutes.

{INFO: Keeping synchronization}
Checking whether a file is already locked and a lock operation are made in a single transaction. There is no option that two servers
 will synchronize a file with the same name simultaneously. Taking the lock by a first one will prevent from syncing the second one.
{INFO/}

##Synchronization aborts

In contrast to a destination system, a source does need to lock a file during the synchronization. You are able to read and modify it. 
However if any file modification made in the middle of the synchronization, it will abort the active synchronization process and retry it with 
a new version.

##Handling failures and restarts

RavenFS has been designated to work with large files, so some synchronizations can take quite a long time. Especially in the case when a new 
file was uploaded and then the source has to transfer the entire file to destination nodes. It makes that some failures might be caused by
network problems or restarts of destination servers. The Raven File System ensures that every file change will be reflected on the destination 
even though any failure happens or the destination won't be available for a long time. In order to make sure that no updates were missed it 
uses the following mechanisms:


TODO arek

* Tracking last ETag
Every successful synchronization on the destination side stores an ETag number of already synchronized file. If the source determines what files require the synchronization it asks the destination about last ETag seen from that server. 
ETags used in RavenFS are sequential, so we just need to synchronize files with greater ETag that last seen.

###Confirmations
Every synchronization must be confirmed. After a synchronization cycle the source stores what files was synchronized. In next cycle it asks the destination about status of the already acomplished synchronizations. If the synchronization failed the file is synchronized again.

###Periodic running
The synchronization task runs every 10 minutes.
