# Data Subscriptions: Revisions Support

---

{NOTE: }

* The **Data Subscription** feature supports subscribing not only to documents, but also to [document revisions](../../../document-extensions/revisions/overview).  

* The revisions support is defined within the subscription.  
  A [Revisions Configuration](../../../document-extensions/revisions/client-api/operations/configure-revisions) must be defined for the subscribed collection.  

* While a regular subscription processes a single document, a Revisions subscription processes **pairs of subsequent document revisions**.  
    
    Using this functionality allows you to keep track of each change made in a document, as well as compare pairs of subsequent versions of the document.  
    
    Both revisions are accessible for filtering and projection.  

* In this page:  
  * [Revisions processing order](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning#revisions-processing-order)  
  * [Simple declaration and usage](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning#simple-declaration-and-usage)   
  * [Revisions processing and projection](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning#revisions-processing-and-projection)  

{NOTE/}

---

{PANEL: Revisions processing order}

The Revisions feature allows the tracking of changes made in a document, by storing the audit trail of its changes over time. 
An audit trail entry is called a **Document Revision**, and is comprised of a document snapshot.  
Read more about revisions [here](../../../document-extensions/revisions/overview).  

In a data subscription, revisions will be processed in pairs of subsequent entries.  
For example, consider the following User document:  

`{
    Name:'James',
    Age:'21'
}`

We update the User document twice, in separate operations:  

* We update the 'Age' field to the value of 22  
* We update the 'Age' field to the value of 23  

The data subscriptions revisions processing mechanism will receive pairs of revisions in the following order:  

| # | Previous                     | Current                      |
|---|------------------------------|------------------------------| 
| 1 | `null`                       | `{ Name:'James', Age:'21' }` |
| 2 | `{ Name:'James', Age:'21' }` | `{ Name:'James', Age:'22' }` |
| 3 | `{ Name:'James', Age:'22' }` | `{ Name:'James', Age:'23' }` |
 
{WARNING: }
The revisions subscription will be able to function properly only if the revisions it needs to process are available.
Please make sure that your revisions configuration doesn't purge revisions before the subscription had the chance to process them.  
{WARNING/}

{PANEL/}

{PANEL: Simple declaration and usage}

Here we declare a simple revisions subscription that will send pairs of subsequent document revisions to the client:

Creation:
{CODE-TABS}
{CODE-TAB:java:Generic-syntax create_simple_revisions_subscription_generic@ClientApi\DataSubscriptions\DataSubscriptions.java /}
{CODE-TAB:java:RQL-syntax create_simple_revisions_subscription_RQL@ClientApi\DataSubscriptions\DataSubscriptions.java /}
{CODE-TABS/}

Consumption:
{CODE:java use_simple_revision_subscription_generic@ClientApi\DataSubscriptions\DataSubscriptions.java /}

{PANEL/}

{PANEL: Revisions processing and projection}

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

- [Revisions API Overview](../../../document-extensions/revisions/client-api/overview)

**Document Extensions**:

- [Revisions Overview](../../../document-extensions/revisions/overview)

**Knowledge Base**:

- [JavaScript Engine](../../../server/kb/javascript-engine)
