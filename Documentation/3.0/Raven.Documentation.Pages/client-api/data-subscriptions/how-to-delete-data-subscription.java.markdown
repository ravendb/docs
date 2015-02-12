#How to delete a data subscription?

The data subscription is never deleted unless you explicitly drop it. This is the method to do it:

{CODE:java delete_1@ClientApi\DataSubscriptions\DataSubscriptions.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | long | Subscription identifier. |
| **database** | string | Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |

##Example

{CODE:java delete_2@ClientApi\DataSubscriptions\DataSubscriptions.java /}