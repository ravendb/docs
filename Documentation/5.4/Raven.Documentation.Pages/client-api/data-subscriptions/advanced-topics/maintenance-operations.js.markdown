# Data Subscriptions: Maintenance Operations
---

{NOTE: }

* This article covers data subscriptions maintenance operations.  

* In this page:  
   * [Delete subscription](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#delete-subscription)  
   * [Disable subscription](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#disable-subscription)  
   * [Enable subscription](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#enable-subscription)  
   * [Update subscription](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#update-subscription)
   * [Drop connection](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#drop-connection)
   * [Get subscriptions](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#get-subscriptions)
   * [Get subscription state](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#get-subscription-state)
   * [DocumentSubscriptions class](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#documentsubscriptions-class)

{NOTE/}

---

{PANEL: Delete subscription}

Subscription tasks can be entirely deleted from the system.  

{CODE:nodejs delete@client-api\dataSubscriptions\advanced\maintenance.js /}
{CODE:nodejs delete_syntax@client-api\dataSubscriptions\advanced\maintenance.js /}

{PANEL/}

{PANEL: Disable subscription}

Existing subscription tasks can be disabled from the client.

{CODE:nodejs disable@client-api\dataSubscriptions\advanced\maintenance.js /}
{CODE:nodejs disable_syntax@client-api\dataSubscriptions\advanced\maintenance.js /}

{PANEL/}

{PANEL: Enable subscription}

Existing subscription tasks can be enabled from the client.  
This operation can be useful for already disabled subscriptions.

{CODE:nodejs enable@client-api\dataSubscriptions\advanced\maintenance.js /}
{CODE:nodejs enable_syntax@client-api\dataSubscriptions\advanced\maintenance.js /}

{PANEL/}

{PANEL: Update subscription}

See [examples](../../../client-api/data-subscriptions/creation/examples#update-existing-subscription) 
and [API description](../../../client-api/data-subscriptions/creation/api-overview#update-subscription).  

{CODE:nodejs update@client-api\dataSubscriptions\advanced\maintenance.js /}
{CODE:nodejs update_syntax@client-api\dataSubscriptions\advanced\maintenance.js /}

{PANEL/}

{PANEL: Drop connection}

Active subscription connections established by workers can be dropped remotely from the client.  
Once dropped, the worker will not attempt to reconnect to the server.

{CODE:nodejs drop_connection@client-api\dataSubscriptions\advanced\maintenance.js /}
{CODE:nodejs drop_connection_syntax@client-api\dataSubscriptions\advanced\maintenance.js /}

{PANEL/}

{PANEL: Get subscriptions}

Get a list of all existing subscription tasks in the database.

{CODE:nodejs get_subscriptions@client-api\dataSubscriptions\advanced\maintenance.js /}
{CODE:nodejs get_subscriptions_syntax@client-api\dataSubscriptions\advanced\maintenance.js /}

{PANEL/}

{PANEL: Get subscription state}

{CODE:nodejs get_state@client-api\dataSubscriptions\advanced\maintenance.js /}
{CODE:nodejs get_state_syntax@client-api\dataSubscriptions\advanced\maintenance.js /}

{INFO: }

##### SubscriptionState

| Member                                    | Type         | Description                                                                                                                                                 |
|-------------------------------------------|--------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------| 
| **query**                                 | `string`     | Subscription's RQL like query.                                                                                                                              |
| **lastBatchAckTime**                      | `DateTime?`  | Last time a batch processing progress was acknowledged.                                                                                                     |
| **nodeTag**                               | `string`     | Processing server's node tag.                                                                                                                               |
| **mentorNode**                            | `string`     | The mentor node that was manually set.                                                                                                                      |
| **subscriptionName**                      | `string`     | The subscription's name, which is also its unique identifier.                                                                                               |
| **subscriptionId**                        | `long`       | Subscription's internal identifier (cluster's operation etag during subscription creation).                                                                 |
| **changeVectorForNextBatchStartingPoint** | `string`     | The Change Vector from which the subscription will begin sending documents.<br>This value is updated on batch acknowledgement and can also be set manually. |
| **disabled**                              | `bool`       | If `true`, subscription will not allow workers to connect.                                                                                                  |
| **lastClientConnectionTime**              | `DateTime?`  | Time when last client was connected (value sustained after disconnection).                                                                                  |                

{INFO/}

{PANEL/}

{PANEL: DocumentSubscriptions class}

The `DocumentSubscriptions` class manages all interaction with the data subscriptions.  
The class is available through the `subscriptions` property in the `documentStore`.

| Method Signature                                         | Return type                    | Description                                                  |
|----------------------------------------------------------|--------------------------------|--------------------------------------------------------------| 
| **create(options)**                                      | `Promise<string>`              | Create a new data subscription.                              |
| **create(options, database)**                            | `Promise<string>`              | Create a new data subscription.                              |
| **create(documentType)**                                 | `Promise<string>`              | Create a new data subscription.                              |
| **create(optionsOrDocumentType, database)**              | `Promise<string>`              | Create a new data subscription.                              |
| **createForRevisions(options)**                          | `Promise<string>`              | Create a new data subscription.                              |
| **createForRevisions(options, database)**                | `Promise<string>`              | Create a new data subscription.                              |
| **delete(name)**                                         | `Promise<void>`                | Delete subscription.                                         |
| **delete(name, database)**                               | `Promise<void>`                | Delete subscription.                                         |
| **dropConnection(name)**                                 | `Promise<void>`                | Drop all existing subscription connections with workers.     |
| **dropConnection(name, database)**                       | `Promise<void>`                | Drop all existing subscription connections with workers.     |
| **dropSubscriptionWorker(worker, database)**             | `Promise<void>`                | Drop an existing subscription connection with a worker.      |
| **enable(name)**                                         | `Promise<void>`                | Enable existing subscription.                                |
| **enable(name, database)**                               | `Promise<void>`                | Enable existing subscription.                                |
| **disable(name)**                                        | `Promise<void>`                | Disable existing subscription.                               |
| **disable(name, database)**                              | `Promise<void>`                | Disable existing subscription.                               |
| **update(updateOptions)**                                | `Promise<string>`              | Update an existing data subscription.                        |
| **update(updateOptions, database)**                      | `Promise<string>`              | Update an existing data subscription.                        |
| **getSubscriptions(start, take)**                        | `Promise<SubscriptionState[]>` | Returns subscriptions list.                                  |
| **getSubscriptions(start, take, database)**              | `Promise<SubscriptionState[]>` | Returns subscriptions list.                                  |
| **getSubscriptionState(subscriptionName)**               | `Promise<SubscriptionState> `  | Get the state of a specific subscription.                    |
| **getSubscriptionState(subscriptionName, database)**     | `Promise<SubscriptionState> `  | Get the state of a specific subscription.                    |
| **getSubscriptionWorker(options)**                       | `SubscriptionWorker`           | Generate a subscription worker.                              |
| **getSubscriptionWorker(options, database)**             | `SubscriptionWorker`           | Generate a subscription worker.                              |
| **getSubscriptionWorker(subscriptionName)**              | `SubscriptionWorker`           | Generate a subscription worker.                              |
| **getSubscriptionWorker(subscriptionName, database)**    | `SubscriptionWorker`           | Generate a subscription worker.                              |
| **getSubscriptionWorkerForRevisions(options)**           | `SubscriptionWorker`           | Generate a subscription worker for a revisions subscription. |
| **getSubscriptionWorkerForRevisions(options, database)** | `SubscriptionWorker`           | Generate a subscription worker for a revisions subscription. |

{PANEL/}

## Related Articles

### Data Subscriptions

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)
