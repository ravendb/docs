# Maintenance operation

This page covers data subscriptions maintenance operations:  

[Deletion](#deletion)  
[Dropping Connection](#dropping-connection)  
[Disabling subscription](#disabling-subscription)  
[Updating subscription](#updating-subscription)  
[Getting subscription status](#getting-subscription-status)  
[DocumentSubscriptions class](#documentsubscriptions-class)  

{PANEL: Deletion}
Subscriptions can be entirely deleted from the system.  
This operation can be very useful in ad-hoc subscription scenarios, when a lot of subscriptions tasks information may accumulate, making tasks management very hard.  

{CODE interface_subscription_deletion@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

usage: 

{CODE subscription_deletion@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}
    
{PANEL: Dropping connection}

Subscription connections with workers can be dropped remotely.  
Dropped worker will not try to reconnect to the server.

{CODE interface_subscription_dropping@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

usage: 

{CODE connection_dropping@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Disabling subscription}
{INFO This operation can only be performed through the management studio /}
{PANEL/}

{PANEL: Updating subscription}
{INFO This operation can only be performed through the management studio /}
{PANEL/}

{PANEL: Getting subscription status}

{CODE interface_subscription_state@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

usage: 

{CODE subscription_state@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{INFO: SubscriptionState}

| Member | Type | Description |
|--------|:-----|-------------| 
| **Query** | `string` | Subscription's RQL like query. |
| **LastBatchAckTime** | `DateTime?` | Last time a batch processing progress was acknowledged. |
| **NodeTag** | `string` | Processing server's node tag |
| **MentorNode** | `string` | The mentor node that was manually set. |
| **SubscriptionName** | `string` | Subscription's name, and also it's unique identifier |
| **SubscriptionId** | `long` | Subscription's internal identifier (cluster's operation etag during subscription creation) |
| **ChangeVectorForNextBatchStartingPoint** | `string` | Change vector, starting from which the subscription will send documents. This value is updated manually, or automatically on batch acknowledgment  |
| **Disabled** | `bool` | If true, subscription will not allow workers to connect |
| **LastClientConnectionTime** | `DateTime?` | Time when last client was connected (value sustained after disconnection) |                

{INFO/}

{PANEL/}

{PANEL:DocumentSubscriptions class}
The `DocumentSubscriptions` class is the class that manages all interaction with the data subscriptions.  
The class is available through `DocumentStore`'s `Subscriptions` property.

| Method signature| Return type | Description |
|--------|:---|-------------| 
| **Create<T>(SubscriptionCreationOptions<T> options, string database)** | `string` | Creates a new data subscription. |
| **Create(SubscriptionCreationOptions criteria, string database)** | `string` | Creates a new data subscription. |
| **Create(SubscriptionCreationOptions criteria, string database)** | `string` | Creates a new data subscription. |
| **CreateAsync<T>(SubscriptionCreationOptions<T> options, string database)** | `Task<string>` | Creates a new data subscription. |
| **CreateAsync<T>(Expression<Func<T, bool>> predicate, SubscriptionCreationOptions options, string database)** | `Task<string>` | Creates a new data subscription. |
| **Delete(string name, string database)** | `void` | Deletes subscription. |
| **DeleteAsync(string name, string database)** | `Task` | Deletes subscription. |
| **DropConnection(string name, string database)** | `void` | Drops existing subscription connection with worker. |
| **DropConnectionAsync(string name, string database)** | `Task` | Drops existing subscription connection with worker. |
| **GetSubscriptions(int start, int take, string database)** | `List<SubscriptionState>` | Returns subscriptions list. |
| **GetSubscriptionsAsync(int start, int take, string database)** | `Task<List<SubscriptionState>>` | Returns subscriptions list. |
| **GetSubscriptionState(string subscriptionName, string database)** | `SubscriptionState ` | Get specific subscription state. |
| **GetSubscriptionStateAsync(string subscriptionName, string database)** | `Task<SubscriptionState> ` | Get specific subscription state. |
| **GetSubscriptionWorker<T>(string subscriptionName, string database)** | `SubscriptionWorker<T>` | Generate subscription worker, using default configurations, that processes documents deserialized to `T` type . |
| **GetSubscriptionWorker(string subscriptionName, string database)** | `SubscriptionWorker<dynamic>` | Generate subscription worker, using default configurations, that processes documents in it's raw `BlittableJsonReader`, wrapped by dynamic object. |
| **GetSubscriptionWorker(SubscriptionWorkerOptions options, string database)** | `SubscriptionWorker<T>` | Generate subscription worker, using default configurations, that processes documents deserialized to `T` type . |
| **GetSubscriptionWorker(SubscriptionWorkerOptions options, string database)** | `SubscriptionWorker<dynamic>` | Generate subscription worker, using default configurations, that processes documents in it's raw `BlittableJsonReader`, wrapped by dynamic object. |

{PANEL/}

## Related articles

- [What are data subscriptions?](../what-are-data-subscriptions)
- [How to **consume** a data subscription?](../subscription-consumption/how-to-consume-data-subscription)
- [How to **create** a data subscription?](../subscription-creation/how-to-create-data-subscription)
