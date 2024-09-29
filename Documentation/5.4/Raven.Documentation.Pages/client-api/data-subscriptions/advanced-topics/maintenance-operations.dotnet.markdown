# Data Subscriptions: Maintenance Operations

---

{NOTE: }

* This article covers data subscriptions maintenance operations.  

* In this page:  
   * [DocumentSubscriptions class](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#documentsubscriptions-class)  
   * [Delete subscription](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#delete-subscription)  
   * [Disabling subscription](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#disable-subscription)  
   * [Enable subscription](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#enable-subscription)  
   * [Update subscription](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#update-subscription)
   * [Drop Connection](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#drop-connection)
   * [Get subscription state](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#get-subscription-state)  

{NOTE/}

---

{PANEL: DocumentSubscriptions class}

The `DocumentSubscriptions` class is the class that manages all interaction with the data subscriptions.  
The class is available through `DocumentStore`'s `Subscriptions` property.

| Method Signature                                                                                              | Return type                     | Description                                                                                                                                         |
|---------------------------------------------------------------------------------------------------------------|---------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------| 
| **Create<T>(SubscriptionCreationOptions<T> options, string database)**                                        | `string`                        | Create a new data subscription.                                                                                                                     |
| **Create(SubscriptionCreationOptions criteria, string database)**                                             | `string`                        | Create a new data subscription.                                                                                                                     |
| **Create(SubscriptionCreationOptions criteria, string database)**                                             | `string`                        | Create a new data subscription.                                                                                                                     |
| **CreateAsync<T>(SubscriptionCreationOptions<T> options, string database)**                                   | `Task<string>`                  | Create a new data subscription.                                                                                                                     |
| **CreateAsync<T>(Expression<Func<T, bool>> predicate, SubscriptionCreationOptions options, string database)** | `Task<string>`                  | Create a new data subscription.                                                                                                                     |
| **Delete(string name, string database)**                                                                      | `void`                          | Delete subscription.                                                                                                                                |
| **DeleteAsync(string name, string database)**                                                                 | `Task`                          | Delete subscription.                                                                                                                                |
| **DropConnection(string name, string database)**                                                              | `void`                          | Drop all existing subscription connections with workers.                                                                                            |
| **DropConnectionAsync(string name, string database)**                                                         | `Task`                          | Drop all existing subscription connections with workers.                                                                                            |
| **DropSubscriptionWorker<T>(SubscriptionWorker<T> worker, string database = null)**                           | `void`                          | Drop an existing subscription connection with a worker                                                                                              |
| **Enable(string name, string database)**                                                                      | `void`                          | Enable existing subscription.                                                                                                                       |
| **EnableAsync(string name, string database)**                                                                 | `Task`                          | Enable existing subscription.                                                                                                                       |
| **Disable(string name, string database)**                                                                     | `void`                          | Disable existing subscription.                                                                                                                      |
| **DisableAsync(string name, string database)**                                                                | `Task`                          | Disable existing subscription.                                                                                                                      |
| **GetSubscriptions(int start, int take, string database)**                                                    | `List<SubscriptionState>`       | Returns subscriptions list.                                                                                                                         |
| **GetSubscriptionsAsync(int start, int take, string database)**                                               | `Task<List<SubscriptionState>>` | Returns subscriptions list.                                                                                                                         |
| **GetSubscriptionState(string subscriptionName, string database)**                                            | `SubscriptionState `            | Get specific subscription state.                                                                                                                    |
| **GetSubscriptionStateAsync(string subscriptionName, string database)**                                       | `Task<SubscriptionState> `      | Get specific subscription state.                                                                                                                    |
| **GetSubscriptionWorker<T>(string subscriptionName, string database)**                                        | `SubscriptionWorker<T>`         | Generate a subscription worker, using default configurations, that processes documents deserialized to `T` type .                                   |
| **GetSubscriptionWorker(string subscriptionName, string database)**                                           | `SubscriptionWorker<dynamic>`   | Generate a subscription worker, using default configurations, that processes documents in its raw `BlittableJsonReader`, wrapped by dynamic object. |
| **GetSubscriptionWorker(SubscriptionWorkerOptions options, string database)**                                 | `SubscriptionWorker<T>`         | Generate a subscription worker, using default configurations, that processes documents deserialized to `T` type .                                   |
| **GetSubscriptionWorker(SubscriptionWorkerOptions options, string database)**                                 | `SubscriptionWorker<dynamic>`   | Generate a subscription worker, using default configurations, that processes documents in its raw `BlittableJsonReader`, wrapped by dynamic object. |
| **Update(SubscriptionUpdateOptions options, string database = null)**                                         | `string`                        | Update an existing data subscription.                                                                                                               |
| **UpdateAsync(SubscriptionUpdateOptions options, string database = null, CancellationToken token = default)** | `Task<string>`                  | Update an existing data subscription.                                                                                                               |

{PANEL/}

{PANEL: Delete subscription}

Subscriptions can be entirely deleted from the system.  

This operation can be very useful in ad-hoc subscription scenarios when a lot of subscriptions tasks information may accumulate, making tasks management very hard.  

{CODE interface_subscription_deletion@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

usage: 

{CODE subscription_deletion@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Disable subscription}

Existing subscription tasks can be disabled from the client.

{CODE interface_subscription_disabling@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

usage: 

{CODE subscription_disabling@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Enable subscription}

Existing subscription tasks can be enabled from the client.  
This operation can be useful for already disabled subscriptions.

{CODE interface_subscription_enabling@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

usage: 

{CODE subscription_enabling@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Update subscription}

See [examples](../../../client-api/data-subscriptions/creation/examples#update-existing-subscription) 
and [API description](../../../client-api/data-subscriptions/creation/api-overview#update-subscription).  

{CODE updating_subscription@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Drop connection}

Active subscription connections established by workers can be dropped remotely from the client.  
Once dropped, the worker will not attempt to reconnect to the server.

{CODE interface_subscription_dropping@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

usage:

{CODE connection_dropping@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{PANEL/}

{PANEL: Get subscription state}

{CODE interface_subscription_state@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

usage: 

{CODE subscription_state@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{INFO: }

##### SubscriptionState

| Member                                    | Type        | Description                                                                                                                                                 |
|-------------------------------------------|-------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------| 
| **Query**                                 | `string`    | Subscription's RQL like query.                                                                                                                              |
| **LastBatchAckTime**                      | `DateTime?` | Last time a batch processing progress was acknowledged.                                                                                                     |
| **NodeTag**                               | `string`    | Processing server's node tag.                                                                                                                               |
| **MentorNode**                            | `string`    | The mentor node that was manually set.                                                                                                                      |
| **SubscriptionName**                      | `string`    | The subscription's name, which is also its unique identifier.                                                                                               |
| **SubscriptionId**                        | `long`      | Subscription's internal identifier (cluster's operation etag during subscription creation).                                                                 |
| **ChangeVectorForNextBatchStartingPoint** | `string`    | The Change Vector from which the subscription will begin sending documents.<br.This value is updated on batch acknowledgement and can also be set manually. |
| **Disabled**                              | `bool`      | If `true`, subscription will not allow workers to connect.                                                                                                    |
| **LastClientConnectionTime**              | `DateTime?` | Time when last client was connected (value sustained after disconnection).                                                                                  |                

{INFO/}

{PANEL/}

## Related Articles

### Data Subscriptions

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
