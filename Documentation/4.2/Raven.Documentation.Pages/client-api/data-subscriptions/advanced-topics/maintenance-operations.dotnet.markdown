# Data Subscriptions: Maintenance Operations

---

{NOTE: }

This page covers data subscriptions maintenance operations:  

[Deletion](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#deletion)  
[Dropping Connection](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#dropping-connection)  
[Disabling subscription](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#disabling-subscription)  
[Enabling subscription](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#enabling-subscription)  
[Updating subscription](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#updating-subscription)  
[Getting subscription status](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#getting-subscription-status)  
[DocumentSubscriptions class](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#documentsubscriptions-class)  

{NOTE/}

---

{PANEL: Deletion}
Subscriptions can be entirely deleted from the system.  

This operation can be very useful in ad-hoc subscription scenarios when a lot of subscriptions tasks information may accumulate, making tasks management very hard.  

{CODE interface_subscription_deletion@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

usage: 

{CODE subscription_deletion@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}
    
{PANEL: Dropping connection}

Subscription connections with workers can be dropped remotely.  
A dropped worker will not try to reconnect to the server.

{CODE interface_subscription_dropping@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

usage: 

{CODE connection_dropping@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Disabling subscription}

Existing subscriptions can be disabled remotely.

{CODE interface_subscription_disabling@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

usage: 

{CODE subscription_disabling@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Enabling subscription}

Existing subscriptions can be enabled remotely.  
This operation can be useful for already disabled subscriptions. A newly created subscription is enabled initially.

{CODE interface_subscription_enabling@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

usage: 

{CODE subscription_enabling@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

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

| Method Signature| Return type | Description |
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
| **Enable(string name, string database)** | `void` | Enables existing subscription. |
| **EnableAsync(string name, string database)** | `Task` | Enables existing subscription. |
| **Disable(string name, string database)** | `void` | Disables existing subscription. |
| **DisableAsync(string name, string database)** | `Task` | Disables existing subscription. |
| **GetSubscriptions(int start, int take, string database)** | `List<SubscriptionState>` | Returns subscriptions list. |
| **GetSubscriptionsAsync(int start, int take, string database)** | `Task<List<SubscriptionState>>` | Returns subscriptions list. |
| **GetSubscriptionState(string subscriptionName, string database)** | `SubscriptionState ` | Get specific subscription state. |
| **GetSubscriptionStateAsync(string subscriptionName, string database)** | `Task<SubscriptionState> ` | Get specific subscription state. |
| **GetSubscriptionWorker<T>(string subscriptionName, string database)** | `SubscriptionWorker<T>` | Generates a subscription worker, using default configurations, that processes documents deserialized to `T` type . |
| **GetSubscriptionWorker(string subscriptionName, string database)** | `SubscriptionWorker<dynamic>` | Generates a subscription worker, using default configurations, that processes documents in it's raw `BlittableJsonReader`, wrapped by dynamic object. |
| **GetSubscriptionWorker(SubscriptionWorkerOptions options, string database)** | `SubscriptionWorker<T>` | Generates a subscription worker, using default configurations, that processes documents deserialized to `T` type . |
| **GetSubscriptionWorker(SubscriptionWorkerOptions options, string database)** | `SubscriptionWorker<dynamic>` | Generates a subscription worker, using default configurations, that processes documents in it's raw `BlittableJsonReader`, wrapped by dynamic object. |

{PANEL/}

## Related Articles

### Data Subscriptions

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
