#How to subscribe to synchronization notifications?

In order to be notified about synchronization activity use `ForSynchronization` to observe synchronization notifications.

##Syntax

{CODE for_synchronization_1@FileSystem\ClientApi\Changes.cs /}


| Return Value | |
| ------------- | ------------- |
| **IObservableWithTask&lt;[SynchronizationUpdateNotification](../../../glossary/synchronization-update-notification)&gt;** | The observable that allows to add subscriptions to received notifications |

##Example

{CODE for_synchronization_2@FileSystem\ClientApi\Changes.cs /}

## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions](https://www.nuget.org/packages/Rx-Main) package to your project. /}
