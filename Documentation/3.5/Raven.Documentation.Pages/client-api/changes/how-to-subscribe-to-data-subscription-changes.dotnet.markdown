# Changes API: How to subscribe to data subscription changes?

All subscription changes can be tracked by using `ForAllDataSubscriptions` and `ForDataSubscription` methods.

## Syntax

{PANEL:ForAllDataSubscriptions}
{CODE data_subscription_changes_1@ClientApi\Changes\HowToSubscribeToDataSubscriptionChanges.cs /}

| Return value | |
| ------------- | ----- |
| IObservableWithTask<[DataSubscriptionChangeNotification](../../glossary/data-subscription-change-notification)> | Observable that allows to add subscriptions to notifications for all data subscription changes. |

{PANEL/}

{PANEL:ForDataSubscription}

{CODE data_subscription_changes_3@ClientApi\Changes\HowToSubscribeToDataSubscriptionChanges.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | long | Id of a data subscription for which notifications will be processed. |

| Return value | |
| ------------- | ----- |
| IObservableWithTask<[DataSubscriptionChangeNotification](../../glossary/data-subscription-change-notification)> | Observable that allows to add subscriptions to notifications for a specified data subscription changes. |

{PANEL/}

## Example I

{CODE data_subscription_changes_2@ClientApi\Changes\HowToSubscribeToDataSubscriptionChanges.cs /}

## Example II

{CODE data_subscription_changes_4@ClientApi\Changes\HowToSubscribeToDataSubscriptionChanges.cs /}

## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions](https://www.nuget.org/packages/Rx-Main) package to your project. /}

## Related articles

 - [How to create a data subscription?](../../client-api/data-subscriptions/how-to-create-data-subscription)
