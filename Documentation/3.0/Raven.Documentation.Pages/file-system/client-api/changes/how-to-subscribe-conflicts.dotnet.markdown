#How to subscribe to synchronization conflicts?

Synchronization conflicts can be tracked by using `ForConflicts` method. You will receive notifications when a new conflicts is created or an existing one is resolved.

##Syntax

{CODE for_conflicts_1@FileSystem\ClientApi\Changes.cs /}


| Return Value | |
| ------------- | ------------- |
| **IObservableWithTask&lt;[ConflictNotification](../../../glossary/conflict-notification)&gt;** | The observable that allows to add subscriptions to received notifications |

##Example

{CODE for_conflicts_2@FileSystem\ClientApi\Changes.cs /}

## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions](https://www.nuget.org/packages/Rx-Main) package to your project. /}
