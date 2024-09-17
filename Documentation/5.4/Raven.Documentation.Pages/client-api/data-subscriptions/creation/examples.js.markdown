# Data Subscription Creation Examples
---

{NOTE: }

* This page contains examples of **creating a subscription**.  
  To learn how to consume and process documents sent by the subscription, see these [examples](../../../client-api/data-subscriptions/consumption/examples).

* For a detailed syntax of the available subscription methods and objects, see this [API overview](../../../client-api/data-subscriptions/creation/api-overview).

* In this page:  
   * [Create subscription - for all documents in a collection](../../../client-api/data-subscriptions/creation/examples#create-subscription---for-all-documents-in-a-collection)  
   * [Create subscription - filter documents](../../../client-api/data-subscriptions/creation/examples#create-subscription---filter-documents)  
   * [Create subscription - filter and project fields](../../../client-api/data-subscriptions/creation/examples#create-subscription---filter-and-project-fields)  
   * [Create subscription - project data from a related document](../../../client-api/data-subscriptions/creation/examples#create-subscription---project-data-from-a-related-document)  
   * [Create subscription - include documents](../../../client-api/data-subscriptions/creation/examples#create-subscription---include-documents)  
   * [Create subscription - include counters](../../../client-api/data-subscriptions/creation/examples#create-subscription---include-counters)  
   * [Create subscription - subscribe to revisions](../../../client-api/data-subscriptions/creation/examples#create-subscription---subscribe-to-revisions)  
   * [Create subscription - via update](../../../client-api/data-subscriptions/creation/examples#create-subscription---via-update)
   * [Update existing subscription](../../../client-api/data-subscriptions/creation/examples#update-existing-subscription)

{NOTE/}

---

{PANEL: Create subscription - for all documents in a collection}

Here we create a plain subscription on the _Orders_ collection without any constraints or transformations.  
The server will send ALL documents from the _Orders_ collection to a client that connects to this subscription.

{CODE:nodejs create_1@client-api\dataSubscriptions\creation\examples.js /}
{CODE:nodejs create_1_1@client-api\dataSubscriptions\creation\examples.js /}
{CODE:nodejs create_1_2@client-api\dataSubscriptions\creation\examples.js /}

{PANEL/}

{PANEL: Create subscription - filter documents}

Here we create a subscription for documents from the _Orders_ collection where the total order revenue is greater than 100. 
Only documents that match this condition will be sent from the server to a client connected to this subscription.

{CODE:nodejs create_2@client-api\dataSubscriptions\creation\examples.js /}

{PANEL/}

{PANEL: Create subscription - filter and project fields}

Here, again, we create a subscription for documents from the _Orders_ collection where the total order revenue is greater than 100.
However, this time we only project the document ID and the Total Revenue properties in each object sent to the client.

{CODE:nodejs create_3@client-api\dataSubscriptions\creation\examples.js /}

{PANEL/}

{PANEL: Create subscription - project data from a related document} 

In this subscription, in addition to projecting the document fields,  
we also project data from a [related document](../../../indexes/indexing-related-documents#what-are-related-documents) that is loaded using the `load` method.

{CODE:nodejs create_4@client-api\dataSubscriptions\creation\examples.js /}

{PANEL/}

{PANEL: Create subscription - include documents}

Here we create a subscription on the _Orders_ collection, which will send all the _Order_ documents.  

In addition, the related _Product_ documents associated with each Order are **included** in the batch sent to the client. 
This way, when the subscription worker that processes the batch in the client accesses a _Product_ document, no additional call to the server will be made.

See how to consume this type of subscription [here](../../../client-api/data-subscriptions/consumption/examples#subscription-that-uses-included-documents).

{CODE-TABS}
{CODE-TAB:nodejs:Builder-syntax create_5@client-api\dataSubscriptions\creation\examples.js /}
{CODE-TAB:nodejs:RQL-path-syntax create_5_1@client-api\dataSubscriptions\creation\examples.js /}
{CODE-TAB:nodejs:RQL-javascript-syntax create_5_2@client-api\dataSubscriptions\creation\examples.js /}
{CODE-TABS/}

{NOTE: }

**Include using builder**:

Include statements can be added to the subscription with a _builder_ object.  
This builder is assigned to the  `includes` property in the _options_ object.  
It supports methods for including documents as well as [counters](../../../client-api/data-subscriptions/creation/examples#create-subscription---include-counters). 
These methods can be chained.

See this [API overview](../../../client-api/data-subscriptions/creation/api-overview#include-methods) for all available include methods.

To include related documents, use method `includeDocuments`.  
(See the _Builder-syntax_ tab in the example above).

{NOTE/}
{NOTE: }

**Include using RQL**:

The include statements can be written in two ways:  
 
1. Use the `include` keyword at the end of the query, followed by the paths to the fields containing the IDs of the documents to include.
   It is recommended to prefer this approach whenever possible, both for the clarity of the query and for slightly better performance.  
   (See the _RQL-path-syntax_ tab in the example above).

2. Define the `include` within a JavaScript function that is called from the `select` clause.  
   (See the _RQL-javascript-syntax_ tab in the example above).

{NOTE/}

{INFO: }

If you include documents when making a [projection](../../../client-api/data-subscriptions/creation/examples#create-subscription---filter-and-project-fields), 
the include will search for the specified paths in the projected fields rather than in the original document.

{INFO/}
{PANEL/}

{PANEL: Create subscription - include counters}

Here we create a subscription on the _Orders_ collection, which will send all the _Order_ documents.  
In addition, values for the specified counters will be **included** in the batch.

Note:  
Modifying an existing counter's value after the document has been sent to the client does Not trigger re-sending.  
However, adding a new counter to the document or removing an existing one will trigger re-sending the document.

{CODE-TABS}
{CODE-TAB:nodejs:Builder-syntax create_7@client-api\dataSubscriptions\creation\examples.js /}
{CODE-TAB:nodejs:RQL-syntax create_7_1@client-api\dataSubscriptions\creation\examples.js /}
{CODE-TABS/}

**All include methods can be chained**:  
For example, the following subscription includes multiple counters and documents:

{CODE:nodejs create_8@client-api\dataSubscriptions\creation\examples.js /}

{PANEL/}

{PANEL: Create subscription - subscribe to revisions}

Here we create a simple revisions subscription on the _Orders_ collection that will send pairs of subsequent document revisions to the client.

{CODE-TABS}
{CODE-TAB:nodejs:documentType-syntax create_9@client-api\dataSubscriptions\creation\examples.js /}
{CODE-TAB:nodejs:RQL-syntax create_9_1@client-api\dataSubscriptions\creation\examples.js /}
{CODE-TABS/}

Learn more about subscribing to document revisions in [subscriptions: revisions support](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning).

{PANEL/}

{PANEL: Create subscription - via update}

When attempting to update a subscription that does Not exist,  
you can request a new subscription to be created by setting `createNew` to `true`.  
In such a case, a new subscription will be created with the provided query.

{CODE:nodejs update_0@client-api\dataSubscriptions\creation\examples.js /}

{PANEL/}

{PANEL: Update existing subscription}

**Update subscription by name**:  
The subscription definition can be updated after it has been created.  
In this example we update the filtering **query** of an existing subscription named "my subscription".

{CODE:nodejs update_1@client-api\dataSubscriptions\creation\examples.js /}

---

**Update subscription by id**:  
In addition to the subscription name, each subscription is assigned a subscription ID when it is created by the server.
This ID can be used instead of the name when updating the subscription.

{CODE:nodejs update_2@client-api\dataSubscriptions\creation\examples.js /}

Using the subscription ID allows you to modify the subscription name:

{CODE:nodejs update_3@client-api\dataSubscriptions\creation\examples.js /}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)

**Knowledge Base**:

- [JavaScript Engine](../../../server/kb/javascript-engine)
