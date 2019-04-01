# Changes API: How to subscribe to data subscription changes?

All subscription changes can be tracked by using `forAllDataSubscriptions` and `forDataSubscription` methods.

## Syntax

{PANEL:ForAllDataSubscriptions}
{CODE:java data_subscription_changes_1@ClientApi\Changes\HowToSubscribeToDataSubscriptionChanges.java /}

| Return value | |
| ------------- | ----- |
| IObservable<[DataSubscriptionChangeNotification](../../glossary/data-subscription-change-notification)> | Observable that allows to add subscriptions to notifications for all data subscription changes. |

{PANEL/}

{PANEL:ForDataSubscription}

{CODE:java data_subscription_changes_3@ClientApi\Changes\HowToSubscribeToDataSubscriptionChanges.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | long | Id of a data subscription for which notifications will be processed. |

| Return value | |
| ------------- | ----- |
| IObservable<[DataSubscriptionChangeNotification](../../glossary/data-subscription-change-notification)> | Observable that allows to add subscriptions to notifications for a specified data subscription changes. |

{PANEL/}

## Example I

{CODE:java data_subscription_changes_2@ClientApi\Changes\HowToSubscribeToDataSubscriptionChanges.java /}

## Example II

{CODE:java data_subscription_changes_4@ClientApi\Changes\HowToSubscribeToDataSubscriptionChanges.java /}

## Related articles

 - [How to create a data subscription?](../../client-api/data-subscriptions/how-to-create-data-subscription)
