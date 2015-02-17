#Conflicts

Each file in the file system has a version number and history (both stored in file metadata). Every time you modify a file then its version is increased and 
then an old value is moved to a history list. 

The conflict detection mechanism works based on these metadata records. A conflict will appear if two files having the same name are synchronized while they have different versions 
and don't have any common records in the history lists. From RavenFS perspective those two files are completely unrelated, so they cannot override each other.


##Role of history in synchronization

If a file that has been created on a file system *A* is synchronized to a destination called *B*, then its metadata is sent together with a file content.
In result this file has the same version and history on the both file systems. Any file modification on *B* server assigns a new version number and sets a source file system identifier
in the following metadata:

* `Raven-Synchronization-Version`
* `Raven-Synchronization-Source`

The old ones are incorporated into the history list (each history item is a pair of these values). Now if the file system *B* wants to synchronize
this file to *A* (*master/master* model), then there will be no conflict on *A* because the file version from *A* will be contained in the history 
of the received file.

{INFO: File relationships}
If a file on a destination file system is an ancestor of a file sent from a source there is no conflict and a file can be synchronized.
{INFO/}

{WARNING: History length}
RavenFS keeps track of the last 50 file changes. If a file had more than 50 updates since the last synchronization, then a conflict would happen on next
synchronization run (it can happen if a destination server was down for a long time).
{WARNING/}

##Conflict items

If a conflict is detected then an appropriate configuration item is created on a destination file system and stored under `Conflicted/[FILENAME]` name.
It contains the following info:

* a name of conflicted file,
* a remote file system URL,
* remote file history,
* local file history.

The full list of all conflicted files can be retrieved from `/synchronization/conflicts` endpoint.

##Conflict detection optimization

In general conflicts can be detected on a destination file system (because existing file metadata is there). However for large files it would be a waste to send
a very big file just to determine that it cannot be synchronized because of a conflict. So the detection is performed on a very early step of the synchronization
operation on a source file system by retrieving remote file metadata and checking it locally. If a conflict is detected then the source creates a conflict item
remotely on the destination.

{INFO: Important}
Conflict items always exist a destination file system.
{INFO/}

##Resolving conflicts

We can resolve a conflict on a destination file system by using one of the following strategies:

* `Local (Current) Version` - take a file that exists on the destination,
* `Remote Version` - take a file from the source server.

The usage of `Local Version` strategy will incorporate a remote (source) version history into local (destination) file metadata, so it will look like a file indeed came from a source file system.

If you choose `Remote Version` then `Raven-Synchronization-Conflict-Resolution` record will be added to metadata of a file existing on a destination.
It will be used by a source file system during a next synchronization attempt (scheduled run or manual execution forced by user) to determine that it can overwrite
a previously conflicted file.

There is also an option to setup default conflict resolution strategy or introduce a custom conflict resolver. More details about dealing with conflicts
by using Client API you can find [here](TODOarek).


