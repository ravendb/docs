# Data Subscriptions : Maintenance Operations

---

{NOTE: }

This page covers data subscriptions maintenance operations:  

[Deletion](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#deletion)  
[Dropping Connection](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#dropping-connection)  
[Disabling subscription](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#disabling-subscription)  
[Updating subscription](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#updating-subscription)  
[Getting subscription status](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#getting-subscription-status)  

{NOTE/}

---

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
{INFO This operation can only be performed through the management studio /}
{PANEL/}

{PANEL: Updating subscription}
{INFO This operation can only be performed through the management studio /}
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
