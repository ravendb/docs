#How to get all subscriptions?

In order to get info about all existing subscriptions we exposed the following method:

{CODE get_subscriptions_1@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | Indicates how many data subscriptions should be skipped. |
| **take** | int | Indicates how many data subscriptions should be taken. |
| **database** | string | Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |

| Return value | |
| ------------- | ----- |
| List&lt;SubscriptionConfig&gt; | Configurations of existing subscriptions providing such information as: id, criteria, last acknowledged Etag, times of sending last batch and client activity.|
##Example

{CODE get_subscriptions_2@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
