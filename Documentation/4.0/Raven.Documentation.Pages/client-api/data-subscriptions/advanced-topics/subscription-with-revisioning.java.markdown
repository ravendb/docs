# Data Subscriptions: Revisions Support

---

{NOTE: }

Data subscription supports subscribing not only on documents, but also on all their revisions.  
Revision support should be defined in the subscription. It also requires revisions to be configured on the collection in question.  
While regular subscriptions process single documents, subscription on documents revisions processes pairs of subsequent document revisions.  
Such functionality allows keeping track of each change that was performed on a document, and even to compare two subsequent versions of a document.  
Both document revisions are accessible in the filtering and the projection process.

In this page:  
[Revisions processing](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning#revisions-processing-order)  
[Simple declaration and usage](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning#simple-declaration-and-usage)   
[Revisions processing and projection](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning#revisions-processing-and-projection)  

{NOTE/}

---

{PANEL:Revisions processing order}
Documents revisions feature allows tracking changes that were performed on a document, by storing the audit trail of its changes over time.  
An audit trail entry is called a Document Revision and is comprised of a document snapshot.  

In data subscription, Documents Revisions will be processed in pairs of subsequent entries.  
Example: 
Let us assume a user document that looks like:  

`{  
    Name:'James',  
    Age:'21'  
}`  

We update the User document twice, in separate operations:  
* We update the 'Age' field to the value of 22  
* We update the 'Age' field to the value of 23  

Data subscription's revision processing mechanism will receive pairs of revision in the following order:  


| # | Previous | Current  |
|---|---|-----| 
| 1 | `null` | `{ Name:'James', Age:'21' }`  |
| 2 | `{ Name:'James', Age:'21' }` | `{ Name:'James', Age:'22' }` |
| 3 | `{ Name:'James', Age:'22' }` | `{ Name:'James', Age:'23' }` |
 

{WARNING As seen above, in order for subscriptions on revisions to work properly, it needs the revisions entries to be available, otherwise, there will be no data to process. Therfore, it's crucial to make sure that the revisions configuration allows storing documents revisions enough time, without discarding unprocessed revisions /}

{PANEL/}

{PANEL:Simple declaration and usage}
Here we declare a simple revisions subscription that will send pairs of subsequent document revisions to the client:

Creation:
{CODE-TABS}
{CODE-TAB:java:Generic-syntax create_simple_revisions_subscription_generic@ClientApi\DataSubscriptions\DataSubscriptions.java /}
{CODE-TAB:java:RQL-syntax create_simple_revisions_subscription_RQL@ClientApi\DataSubscriptions\DataSubscriptions.java /}
{CODE-TABS/}

Consumption:
{CODE:java use_simple_revision_subscription_generic@ClientApi\DataSubscriptions\DataSubscriptions.java /}
{PANEL/}

{PANEL:Revisions processing and projection}
Here we declare a revisions subscription that will filter and project data from revisions pairs:

Creation:
{CODE:java create_projected_revisions_subscription_RQL@ClientApi\DataSubscriptions\DataSubscriptions.java /}

Consumption:
{CODE:java use_simple_revision_subscription_generic@ClientApi\DataSubscriptions\DataSubscriptions.java /}
{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)

**Session**:

- [What are Revisions](../../../client-api/session/revisions/what-are-revisions)

**Server**:

- [Revisions](../../../server/extensions/revisions)
