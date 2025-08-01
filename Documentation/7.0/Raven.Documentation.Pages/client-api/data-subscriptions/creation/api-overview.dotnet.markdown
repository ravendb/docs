# Create and Update Subscription API
---

{NOTE: }

* In this page:  
   * [Create subscription](../../../client-api/data-subscriptions/creation/api-overview#create-subscription)
   * [Subscription creation options](../../../client-api/data-subscriptions/creation/api-overview#subscription-creation-options)  
   * [Update subscription](../../../client-api/data-subscriptions/creation/api-overview#update-subscription)  
   * [Subscription update options](../../../client-api/data-subscriptions/creation/api-overview#subscription-update-options)  
   * [Subscription query](../../../client-api/data-subscriptions/creation/api-overview#subscription-query)  

{NOTE/}

---

{PANEL: Create subscription}

Subscriptions can be created using the following `Create` methods available through the `Subscriptions` property of the `DocumentStore`.

{CODE subscriptionCreationOverloads@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

| Parameter      | Type                                    | Description                                                                                                                                                                                              |
|----------------|-----------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **predicate**  | `Expression<Func<T, bool>>`             | An optional lambda expression that returns a boolean.<br>This predicate defines the filter criteria for the subscription documents.                                                                      |
| **options**    | `SubscriptionCreationOptions<T>`        | Contains subscription creation options.<br>See [Subscription creation options](../../../client-api/data-subscriptions/creation/api-overview#subscription-creation-options)                               |
| **options**    | `SubscriptionCreationOptions`           | Contains subscription creation options<br>(non-generic version).<br>See [Subscription creation options](../../../client-api/data-subscriptions/creation/api-overview#subscription-creation-options)      |
| **options**    | `PredicateSubscriptionCreationOptions ` | Contains subscription creation options<br>(when passing a predicate).<br>See [Subscription creation options](../../../client-api/data-subscriptions/creation/api-overview#subscription-creation-options) |
| **database**   | `string`                                | The name of the database where the subscription task will be created. If `null`, the default database configured in the DocumentStore will be used.                                                      |
| **token**      | `CancellationToken`                     | Cancellation token used in to halt the subscription creation process.                                                                                                                                    |

| Return value  | Description                                                                                                                                                                            |
|---------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `string`      | The name of the created data subscription.<br>If the name was provided in `SubscriptionCreationOptions`, it will be returned.<br>Otherwise, a unique name will be generated by server. |

{PANEL/}

{PANEL: Subscription creation options}

{CONTENT-FRAME: }

Options for the **generic** version of the subscription creation options object:
{CODE sub_create_options_strong@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

Options for the  **non-generic** version of the subscription creation options object:
{CODE sub_create_options@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

Options for the **non-generic** version of the subscription creation options object when passing a **predicate**:
{CODE sub_create_options_when_using_predicate@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

{CONTENT-FRAME/}

| Member                             | Type                                     | Description                                                                                                                                                                                                                                                                                                                                                                                         |
|------------------------------------|------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------| 
| **&lt;T&gt;**                      | `T`                                      | Type of object from which the collection of documents managed by the subscription will be derived.                                                                                                                                                                                                                                                                                                  |
| **Name**                           | `string`                                 | User defined name for the subscription.<br>The name must be unique in the database.                                                                                                                                                                                                                                                                                                                 |
| **Query**                          | `string`                                 | RQL query that defines the subscription. This RQL comes with additional support to JavaScript functions inside the `where` clause and special semantics for subscriptions on documents revisions.                                                                                                                                                                                                   |
| **Filter**                         | `Expression<Func<T, bool>>`              | Lambda expression defining the filter logic for the subscription. Will be translated to a JavaScript function.                                                                                                                                                                                                                                                                                      |
| **Projection**                     | `Expression<Func<T, object>>`            | Lambda expression defining the projection that will be sent by the subscription for each matching document. Will be translated to a JavaScript function.                                                                                                                                                                                                                                            |
| **Includes**                       | `Action<ISubscriptionIncludeBuilder<T>>` | An action that defines include clauses for the subscription. [Included documents](../../../client-api/data-subscriptions/creation/examples#create-subscription---include-documents) and/or [included counters](../../../client-api/data-subscriptions/creation/examples#create-subscription---include-counters) will be part of the batch sent by the subscription. Include methods can be chained. |
| **ChangeVector**                   | `string`                                 | Allows to define a change vector from which the subscription will start processing.<br>Learn more [below](../../../client-api/data-subscriptions/creation/api-overview#the--property).                                                                                                                                                                                                             |
| **Disabled**                       | `bool`                                   | `true` - task will be disabled.<br>`false` - task will be enabled.                                                                                                                                                                                                                                                                                                                                  |
| **MentorNode**                     | `string`                                 | Allows to define a node in the cluster that will be responsible to handle the subscription. Useful when you prefer a specific server due to its stronger hardware, closer geographic proximity to clients, or other reasons.                                                                                                                                                                        |
| **PinToMentorNode**                | `bool`                                   | `true` - the selected responsible node will be pinned to handle the task.<br>`false` - Another node will execute the task if the responsible node is down.                                                                                                                                                                                                                                          |
| **ArchivedDataProcessingBehavior** | `ArchivedDataProcessingBehavior?`        | Define whether [archived documents](../../../data-archival/archived-documents-and-other-features#archived-documents-and-subscriptions) will be included in the subscription.                                                                                                                                                                                                                                                     |

{NOTE: }

###### The `ChangeVector` property: 

* The _ChangeVector_ property allows you to define a starting point from which the subscription will begin processing changes.
* This is useful for ad-hoc processes that need to process only recent changes. In such cases, you can:
  * Set the field to _"LastDocument"_ to start processing from the latest document in the collection.
  * Or, provide an actual Change Vector to begin processing from a specific point.
* By default, the subscription will send all documents matching the RQL query, regardless of their creation time.

{NOTE/}

{PANEL/}

{PANEL: Update subscription}

Existing subscriptions can be modified using the following `Update` methods available through the `Subscriptions` property of the `DocumentStore`.

{CODE updating_subscription@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

| Parameter    | Type                        | Description                                                                                                                                                        |
|--------------|-----------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **options**  | `SubscriptionUpdateOptions` | The subscription update options object.<br>See [SubscriptionUpdateOptions](../../../client-api/data-subscriptions/creation/api-overview#subscriptionupdateoptions) |
| **database** | `string`                    | The name of the database where the subscription task resides.<br>If `null`, the default database configured in the DocumentStore will be used.                     |
| **token**    | `CancellationToken`         | Cancellation token used to halt the update process.                                                                                                                |

| Return value  | Description                                |
|---------------|--------------------------------------------|
| `string`      | The name of the updated data subscription. |

{PANEL/}

{PANEL: Subscription update options}

`SubscriptionUpdateOptions` inherits from [SubscriptionCreationOptions](../../../client-api/data-subscriptions/creation/api-overview#subscriptioncreationoptions)
and adds two additional fields:

{CODE sub_update_options@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

| Parameter     | Type    | Description                                                                                                                                                                                                                                                                                                                                                                                                                                                              |
|---------------|---------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Id**        | `long?` | The unique ID that was assigned to the subscription by the server at creation time.<br>You can retrieve it by [getting the subscription status](../../../client-api/data-subscriptions/advanced-topics/maintenance-operations#getting-subscription-status).<br>When updating, the `Id` can be used instead of the `Name` field, and takes precedence over it. This allows you to modify the subscription's name: provide the Id and submit a new name in the Name field. |
| **CreateNew** | `bool`  | Determines the behavior when the subscription you wish to update does Not exist.<br>`true` - a new subscription is created with the provided option parameters.<br>`false` - an exception will be thrown.<br>Default: `false`                                                                                                                                                                                                                                            |

{PANEL/}

{PANEL: Subscription query} 

All subscriptions are eventually translated to an RQL-like statement. These statements have the following parts:

* Functions definition part, like in ordinary RQL. Those functions can contain any JavaScript code,
  and also supports `load` and `include` operations.

* From statement, defining the documents source, ex: `from Orders`. The from statement can only address collections, therefore, indexes are not supported.    

* Where statement describing the criteria according to which it will be decided to either 
  send the documents to the worker or not. Those statements support either RQL like `equality` operations (`=`, `==`) ,  
  plain JavaScript expressions or declared function calls, allowing to perform complex filtering logic.  
  The subscriptions RQL does not support any of the known RQL searching keywords.

* Select statement, that defines the projection to be performed. 
  The select statements can contain function calls, allowing complex transformations.

* Include statement allowing to define include path in document.  

{INFO: Keywords}
Although subscription's query syntax has an RQL-like structure, it supports only the `declare`, `select` and `where` keywords, usage of all other RQL keywords is not supported.  
Usage of JavaScript ES5 syntax is supported.
{INFO/}

{INFO: Paths}
Paths in subscriptions RQL statements are treated as JavaScript indirections and not like regular RQL paths.  
It means that a query that in RQL would look like:  

```
from Orders as o
where o.Lines[].Product = "products/1-A"
```

Will look like that in subscriptions RQL:

```
declare function filterLines(doc, productId)
{
    if (!!doc.Lines){
        return doc.Lines.filter(x=>x.Product == productId).length >0;
    }
    return false;
}

from Orders as o
where filterLines(o, "products/1-A")
```
{INFO/}  

{INFO: Revisions}

To define a data subscription that sends document revisions to the client,  
you must first [configure revisions](../../../document-extensions/revisions/overview#defining-a-revisions-configuration)
for the specific collection managed by the subscription.

The subscription should be defined in a special way:  

* In case of the generic API, the `SubscriptionCreationOptions<>` generic parameter should be of the generic type `Revision<>`, 
  while it's generic parameter correlates to the collection to be processed.  Ex: `new SubscriptionCreationOptions<Revision<Order>>()`  
* For RQL syntax, concatenate the `(Revisions = true)` clause to the collection being queried.  
  For example: `From Orders(Revisions = true) as o`

{INFO/}
  
{PANEL/}

## Related Articles

**Data Subscriptions**:  

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)

**Knowledge Base**:

- [JavaScript Engine](../../../server/kb/javascript-engine)


