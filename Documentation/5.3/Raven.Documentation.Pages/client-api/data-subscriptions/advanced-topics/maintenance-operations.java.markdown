# Data Subscriptions: Maintenance Operations

---

{NOTE: }

This page covers data subscriptions maintenance operations:  

[DocumentSubscriptions class](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#documentsubscriptions-class)  
[Deletion](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#deletion)  
[Dropping Connection](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#dropping-connection)  
[Disabling subscription](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#disabling-subscription)  
[Enabling subscription](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#enabling-subscription)  
[Updating subscription](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#updating-subscription)  
[Getting subscription status](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#getting-subscription-status)  

{NOTE/}

---

{PANEL:DocumentSubscriptions class}
The `DocumentSubscriptions` class is the class that manages all interaction with the data subscriptions.  
The class is available through `DocumentStore`'s `subscriptions()` method.

| Method Signature| Return type | Description |
|--------|:---|-------------| 
| **create(SubscriptionCreationOptions options)** | `String` | Creates a new data subscription. |
| **create(SubscriptionCreationOptions options, String database)** | `String` | Creates a new data subscription. |
| **create(SubscriptionCreationOptions options)** | `String` | Creates a new data subscription. |
| **create(Class<T> clazz)** | `String` | Creates a new data subscription. |
| **create(Class<T> clazz, SubscriptionCreationOptions options)** | `String` | Creates a new data subscription. |
| **create(Class<T> clazz, SubscriptionCreationOptions options, String database)** | `String` | Creates a new data subscription. |
| **createForRevisions(Class<T> clazz)** | `String` | Creates a new data subscription. |
| **createForRevisions(Class<T> clazz, SubscriptionCreationOptions options)** | `String` | Creates a new data subscription. |
| **createForRevisions(Class<T> clazz, SubscriptionCreationOptions options, String database)** | `String` | Creates a new data subscription. |
| **delete(String name)** | `void` | Deletes subscription. |
| **delete(String name, String database)** | `void` | Deletes subscription. |
| **dropConnection(String name)** | `void` | Drops existing subscription connection with worker. |
| **dropConnection(String name, String database)** | `void` | Drops existing subscription connection with worker. |
| **getSubscriptions(int start, int take)** | `List<SubscriptionState>` | Returns subscriptions list. |
| **getSubscriptions(int start, int take, String database)** | `List<SubscriptionState>` | Returns subscriptions list. |
| **getSubscriptionState(String subscriptionName)** | `SubscriptionState ` | Get specific subscription state. |
| **getSubscriptionState(String subscriptionName, String database)** | `SubscriptionState ` | Get specific subscription state. |
| **getSubscriptionWorker(string subscriptionName)** | `SubscriptionWorker<ObjectNode>` | Generates a subscription worker, using default configurations, that processes documents in it's raw `ObjectNode` type . |
| **getSubscriptionWorker(string subscriptionName, String database)** | `SubscriptionWorker<ObjectNode>` | Generates a subscription worker, using default configurations, that processes documents in it's raw `ObjectNode` type . |
| **getSubscriptionWorker(SubscriptionWorkerOptions options)** | `SubscriptionWorker<ObjectNode>` | Generates a subscription worker, using default configurations, that processes documents in it's raw `ObjectNode` type . |
| **getSubscriptionWorker(SubscriptionWorkerOptions options, String database)** | `SubscriptionWorker<ObjectNode>` | Generates a subscription worker, using default configurations, that processes documents in it's raw `ObjectNode` type . |
| **getSubscriptionWorker<T>(Class<T> clazz, String subscriptionName)** | `SubscriptionWorker<T>` | Generates a subscription worker, using default configurations, that processes documents deserialized to `T` type . |
| **getSubscriptionWorker<T>(Class<T> clazz, String subscriptionName, String database)** | `SubscriptionWorker<T>` | Generates a subscription worker, using default configurations, that processes documents deserialized to `T` type . |
| **getSubscriptionWorker<T>(Class<T> clazz, SubscriptionWorkerOptions options)** | `SubscriptionWorker<T>` | Generates a subscription worker, using provided configuration, that processes documents deserialized to `T` type . |
| **getSubscriptionWorker<T>(Class<T> clazz, SubscriptionWorkerOptions options, String database)** | `SubscriptionWorker<T>` | Generates a subscription worker, using provided configuration, that processes documents deserialized to `T` type . |
| **getSubscriptionWorkerForRevisions<T>(Class<T> clazz, String subscriptionName)** | `SubscriptionWorker<T>` | Generates a subscription worker, using default configurations, that processes documents deserialized to `T` type . |
| **getSubscriptionWorkerForRevisions<T>(Class<T> clazz, String subscriptionName, String database)** | `SubscriptionWorker<T>` | Generates a subscription worker, using default configurations, that processes documents deserialized to `T` type . |
| **getSubscriptionWorkerForRevisions<T>(Class<T> clazz, SubscriptionWorkerOptions options)** | `SubscriptionWorker<T>` | Generates a subscription worker, using provided configuration, that processes documents deserialized to `T` type . |
| **getSubscriptionWorkerForRevisions<T>(Class<T> clazz, SubscriptionWorkerOptions options, String database)** | `SubscriptionWorker<T>` | Generates a subscription worker, using provided configuration, that processes documents deserialized to `T` type . |

{PANEL/}

{PANEL: Deletion}
Subscriptions can be entirely deleted from the system.  

This operation can be very useful in ad-hoc subscription scenarios when a lot of subscriptions tasks information may accumulate, making tasks management very hard.  

{CODE:java interface_subscription_deletion@ClientApi\DataSubscriptions\DataSubscriptions.java /}

usage: 

{CODE:java subscription_deletion@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}
    
{PANEL: Dropping connection}

Subscription connections with workers can be dropped remotely.  
A dropped worker will not try to reconnect to the server.

{CODE:java interface_subscription_dropping@ClientApi\DataSubscriptions\DataSubscriptions.java /}

usage: 

{CODE:java connection_dropping@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

{PANEL: Disabling subscription}

Existing subscriptions can be disabled remotely.

{CODE:java interface_subscription_disabling@ClientApi\DataSubscriptions\DataSubscriptions.java /}

usage: 

{CODE:java subscription_disabling@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

{PANEL: Enabling subscription}

Existing subscriptions can be enabled remotely.  
This operation can be useful for already disabled subscriptions. A newly created subscription is enabled initially.

{CODE:java interface_subscription_enabling@ClientApi\DataSubscriptions\DataSubscriptions.java /}

usage: 

{CODE:java subscription_enabling@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

{PANEL: Updating subscription}

See [example](../../../client-api/data-subscriptions/creation/examples#update-existing-subscription) 
and [API description](../../../client-api/data-subscriptions/creation/api-overview#update-subscription).  

{CODE:java updating_subscription@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

{PANEL: Getting subscription status}

{CODE:java interface_subscription_state@ClientApi\DataSubscriptions\DataSubscriptions.java /}

usage: 

{CODE:java subscription_state@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{INFO: SubscriptionState}

| Member | Type | Description |
|--------|:-----|-------------| 
| **query** | `String` | Subscription's RQL like query. |
| **lastBatchAckTime** | `Date` | Last time a batch processing progress was acknowledged. |
| **nodeTag** | `String` | Processing server's node tag |
| **mentorNode** | `String` | The mentor node that was manually set. |
| **subscriptionName** | `String` | Subscription's name, and also it's unique identifier |
| **subscriptionId** | `long` | Subscription's internal identifier (cluster's operation etag during subscription creation) |
| **changeVectorForNextBatchStartingPoint** | `String` | Change vector, starting from which the subscription will send documents. This value is updated manually, or automatically on batch acknowledgment  |
| **disabled** | `boolean` | If true, subscription will not allow workers to connect |
| **lastClientConnectionTime** | `Date` | Time when last client was connected (value sustained after disconnection) |

{INFO/}

{PANEL/}

## Related Articles

### Data Subscriptions

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
