#Conflict listeners

To allow users to handle [file synchronization conflicts](../../synchronization/conflicts) automatically on the client side, we introduced `IFilesConflictListener`. 
In order to create the listener that will be able to deal with conflicted files implement this interface.

{CODE interface@FileSystem\ClientApi\Listeners\Conflict.cs /}

The attempt to resolve the conflict is taken when [a conflict notification](../changes/how-to-subscribe-conflicts) arrives. It means that you need to subscribe to the conflict notifications first to take
advantage of the registered conflict listeners.

The conflict is resolved according to the first encountered strategy different than `ConflictResolutionStrategy.NoResolution` returned by the listener. The listeners are executed in 
the order of addition. To get more details about resolution strategies read [Resolving conflicts](../../synchronization/conflicts#resolving-conflicts) section.

##Example

The below listener resolves conflict in favor of the newer file:

{CODE example@FileSystem\ClientApi\Listeners\Conflict.cs /}