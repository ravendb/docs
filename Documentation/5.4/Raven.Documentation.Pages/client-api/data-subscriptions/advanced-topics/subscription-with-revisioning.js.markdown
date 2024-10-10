# Data Subscriptions: Revisions Support

---

{NOTE: }

* When the [Revisions feature](../../../document-extensions/revisions/overview) is enabled, a document revision is created with each change made to the document.  
  Each revision contains a snapshot of the document at the time of modification, forming a complete audit trail.
  
* The **Data Subscription** feature supports subscribing not only to documents but also to their **revisions**.  
  This functionality allows the subscribed client to track changes made to documents over time.
 
* The revisions support is specified within the subscription definition.  
  See how to create and consume it in the examples below.

* In this page:  
  * [Regular subscription vs Revisions subscription](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning#regular-subscription-vs-revisions-subscription)
  * [Revisions processing order](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning#revisions-processing-order)  
  * [Simple creation and consumption](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning#simple-creation-and-consumption)   
  * [Filtering revisions](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning#filtering-revisions)
  * [Projecting fields from revisions](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning#projecting-fields-from-revisions)

{NOTE/}

---

{PANEL: Regular subscription vs Revisions subscription}

{NOTE: }

##### Regular subscription
---

* **Processed items**:  
  The subscription processes **documents** from the defined collection.  
  Only the latest version of the document is processed, even if the document has revisions.
* **Query access scope**:  
  The subscription query running on the server has access only to the latest/current version of the documents.
* **Data sent to client**:   
  Each item in the batch sent to the client contains a single document (or a projection of it),   
  as defined in the subscription.

{NOTE/}
{NOTE: }

##### Revisions subscription
---

* **Processed items**:   
  The subscription processes all **revisions** of documents from the defined collection,  
  including revisions of deleted documents from the revision bin if they have not been purged.
* **Query access scope**:  
  For each revision, the subscription query running on the server has access to both the currently processed revision and its previous revision.
* **Data sent to client**:  
  By default, unless the subscription query is projecting specific fields,
  each item in the batch sent to the client contains both the processed revision (`result.current`) and its preceding revision (`result.previous`).
  If the document has just been created, the previous revision will be `null`. 

---

{WARNING: }

* In order for the revisions subscription to work,  
  [Revisions must be configured](../../../document-extensions/revisions/overview#defining-a-revisions-configuration) and enabled for the collection the subscription manages.

* A document that has no revisions will Not be processed,
  so make sure that your revisions configuration does not purge revisions before the subscription has a chance to process them.

{WARNING/}

{NOTE/}
{PANEL/}

{PANEL: Revisions processing order}

In the revisions subscription, revisions are processed in pairs of subsequent entries.  
For example, consider the following User document:  

{CODE-BLOCK:json}
{
    Name: "James",
    Age: "21"
}
{CODE-BLOCK/}
 
We update this User document in two consecutive operations:  

* Update the 'Age' field to the value of 22  
* Update the 'Age' field to the value of 23  

The subscription worker in the client will receive pairs of revisions ( _previous_ & _current_ )  
within each item in the batch in the following order:  

| Batch item | Previous                       | Current                        |
|------------|--------------------------------|--------------------------------| 
| item #1    | `null`                         | `{ Name: "James", Age: "21" }` |
| item #2    | `{ Name: "James", Age: "21" }` | `{ Name: "James", Age: "22" }` |
| item #3    | `{ Name: "James", Age: "22" }` | `{ Name: "James", Age: "23" }` |
 
{PANEL/}

{PANEL: Simple creation and consumption}

Here we set up a basic revisions subscription that will deliver pairs of consecutive _Order_ document revisions to the client:

**Create subscription**:

{CODE:nodejs revisions_1@client-api\dataSubscriptions\advanced\revisionsSubscription.js /}

**Consume subscription**:

{CODE:nodejs revisions_2@client-api\dataSubscriptions\advanced\revisionsSubscription.js /}

{PANEL/}

{PANEL: Filtering revisions}

Here we set up a revisions subscription that will send the client only document revisions in which the order was shipped to Mexico.

**Create subscription**:

{CODE:nodejs revisions_3@client-api\dataSubscriptions\advanced\revisionsSubscription.js /}

**Consume subscription**:

{CODE:nodejs revisions_4@client-api\dataSubscriptions\advanced\revisionsSubscription.js /}

{PANEL/}

{PANEL: Projecting fields from revisions}

Here we define a revisions subscription that will filter the revisions and send projected data to the client.

**Create subscription**:

{CODE-TABS}
{CODE-TAB:nodejs:Subscription_definition revisions_5@client-api\dataSubscriptions\advanced\revisionsSubscription.js /}
{CODE-TAB:nodejs:Projection_class projection_class@client-api\dataSubscriptions\advanced\revisionsSubscription.js /}
{CODE-TABS/}

**Consume subscription**:

Since the revision fields are projected into the `OrderRevenues` class in the subscription definition,  
each item received in the batch has the format of this projected class instead of the default `result.previous` and `result.current` fields,
as was demonstrated in the [simple example](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning#simple-creation-and-consumption).

{CODE:nodejs revisions_6@client-api\dataSubscriptions\advanced\revisionsSubscription.js /}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)

**Session**:

- [Revisions API Overview](../../../document-extensions/revisions/client-api/overview)

**Document Extensions**:

- [Revisions Overview](../../../document-extensions/revisions/overview)

**Knowledge Base**:

- [JavaScript Engine](../../../server/kb/javascript-engine)
