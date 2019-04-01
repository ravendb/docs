# Changes API: How to subscribe to replication conflicts?

Replication conflicts, for both documents and attachments, can be tracked by using `forAllReplicationConflicts` method available in changes API.

## Syntax

{CODE:java replication_conflicts_changes_1@ClientApi\Changes\HowToSubscribeToReplicationConflictsChanges.java /}

| Return value | |
| ------------- | ----- |
| IObservable<[ReplicationConflictNotification](../../glossary/replication-conflict-notification)> | Observable that allows to add subscriptions to notifications for all replication conflicts. |

## Example

{CODE:java replication_conflicts_changes_2@ClientApi\Changes\HowToSubscribeToReplicationConflictsChanges.java /}

## Automatic document conflict resolution

In RavenDB client you have an opportunity to register [conflict listeners](../../client-api/listeners/what-are-conflict-listeners-and-how-to-work-with-them) which are used to resolve conflicted document. However this can happen only if you get the conflicted document. The ability to subscribe to the replication conflicts gives the client more power. Now if you listen to the conflicts and have any conflict listener registered then the client will automatically resolve the conflict right after the arrival of the notification.

## Related articles

- [Server : Replication : Conflicts](../../server/scaling-out/replication/replication-conflicts)
