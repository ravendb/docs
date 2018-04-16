# Administration : Index administration

RavenDB indexes can be administrated easily by the user with the Studio or via maintenance operations in the Client API.

## Stop & Start 

Stopping the indexing for a database will result in pausing all indexes. You can do that using [the Studio](../../studio/database/indexes/todo) or [the Client API](../../client-api/operations/maintenance/indexes/stop-indexing). 
The same way you can resume it (the operation to start indexing can be found [here](../../client-api/operations/maintenance/indexes/start-indexing)).

You can also stop and start a single index. The Client API operations are [StopIndex](../../client-api/operations/maintenance/indexes/stop-index) and [StartIndex](../../client-api/operations/maintenance/indexes/start-index).

{NOTE: }
Indexing will be resumed automatically after a server restart.
{NOTE /}

## Disable & Enable

Disabling an index can be done via [the Studio](../../studio/database/indexes/todo) or the Client API operations: [DisableIndex](../../client-api/operations/maintenance/indexes/disable-index), [EnableIndex](../../client-api/operations/maintenance/indexes/enable-index). 

{NOTE: }
Querying a disabled index is allowed but it may return stale results. Unlike stopping the index, the disable index is a persistent operation, so the index remains disabled 
even after a server restart.
{NOTE /}


## Reset

An index usually needs to be reset once it reached its error quota and its state was changed to `Error`. Resetting an index means forcing RavenDB to re-index all documents
matched by the index definition (can be a lengthy process on large databases).

You can reset an index using [the Studio](../../studio/database/indexes/todo) or [the Client API](../../client-api/operations/maintenance/indexes/reset-index).

## Delete

You can delete an index by using [the Studio](../../studio/database/indexes/todo) or [the Client API](../../client-api/operations/maintenance/indexes/delete-index).


## Lock Mode

This feature applies to changing an index definition on the production server. Index locking has two possible results: 

- any changes to the locked index will be ignored,
- an error will be raised when someone tries to modify the index. 

The typical flow is that you update the index definition on the server, next update it on the codebase, and finally deploy the application to match them.
While the index is locked, at any time when `IndexCreation.CreateIndexes()` on start up is called, the changes you have introduced will not be reverted.

It is important to note that this is not a security feature, and you can unlock an index at any time.

To lock the index you can use [the Studio](../../studio/database/indexes/todo) or [the Client API](../../client-api/operations/maintenance/indexes/set-indexes-lock).

The available modes are:

* Unlock
* LockedIgnore
* LockedError

## Priority

Each index has a dedicated thread that handles all the work for the index. RavenDB uses thread priorities at the operating system level to hint what
should be done first. You can increase or lower the index priority and RavenDB will update the indexing thread accordingly:


| RavenDB Index Priority | OS Thread Priority |
| --- | ------ |
| Low | Lowest |
| Normal (default) | BelowNormal |
| High | Normal |

You can change the index priority by using [the Studio](../../studio/database/indexes/todo) or [the Client API](../../client-api/operations/maintenance/indexes/set-indexes-priority).

{NOTE:Expert configuration options}

You can control the affinity of indexing threads and number of cores that _won't_ be used by indexes via the following configurations:

- [Server.IndexingAffinityMask](../../server/configuration/server-configuration#server.indexingaffinitymask)
- [Server.NumberOfUnusedCoresByIndexes](../../server/configuration/server-configuration#server.numberofunusedcoresbyindexes)

{NOTE/}
