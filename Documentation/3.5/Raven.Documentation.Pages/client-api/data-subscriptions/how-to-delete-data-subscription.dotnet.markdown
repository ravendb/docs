#How to delete a data subscription?

The data subscription is never deleted unless you explicitly drop it. This is the method to do it:

{CODE delete_1@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | long | Subscription identifier. |
| **database** | string | Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |

##Example

{CODE delete_2@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{WARNING Deleting a subscription will kill the connection if it's active. /}