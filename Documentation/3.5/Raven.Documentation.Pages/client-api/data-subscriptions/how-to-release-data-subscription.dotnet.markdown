#How to release a data subscription?

Because just a single client can keep the subscription open the API exposes a method that allows to release a subscription by forcing a connected client to drop:

{CODE release_1@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | long | Subscription identifier. |
| **database** | string | Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |

##Example

{CODE release_2@ClientApi\DataSubscriptions\DataSubscriptions.cs /}