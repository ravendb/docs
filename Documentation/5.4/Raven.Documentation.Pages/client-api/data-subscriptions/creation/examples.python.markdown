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
    * [Update existing subscription](../../../client-api/data-subscriptions/creation/examples#update-existing-subscription)  

{NOTE/}

---

{PANEL: Create subscription - for all documents in a collection}

Here we create a plain subscription on the _Orders_ collection without any constraints or transformations.  
The server will send ALL documents from the _Orders_ collection to a client that connects to this subscription.

{CODE-TABS}
{CODE-TAB:python:Generic-syntax create_whole_collection_generic_with_name@ClientApi\DataSubscriptions\DataSubscriptions.py /}
{CODE-TAB:python:RQL-syntax create_whole_collection_RQL@ClientApi\DataSubscriptions\DataSubscriptions.py /}
{CODE-TABS/}

{PANEL/}

{PANEL: Create subscription - filter documents}

Here we create a subscription for documents from the _Orders_ collection where the total order revenue is greater than 100.
Only documents that match this condition will be sent from the server to a client connected to this subscription.

{CODE:python create_filter_only_RQL@ClientApi\DataSubscriptions\DataSubscriptions.py /}

{PANEL/}

{PANEL: Create subscription - filter and project fields}

Here, again, we create a subscription for documents from the _Orders_ collection where the total order revenue is greater than 100.
However, this time we only project the document ID and the Total Revenue properties in each object sent to the client.

{CODE:python create_filter_and_projection_RQL@ClientApi\DataSubscriptions\DataSubscriptions.py /}

{PANEL/}

{PANEL: Create subscription - project data from a related document}

In this subscription, in addition to projecting the document fields,  
we also project data from a [related document](../../../indexes/indexing-related-documents#what-are-related-documents) that is loaded using the `load` method.

{CODE:python create_filter_and_load_document_RQL@ClientApi\DataSubscriptions\DataSubscriptions.py /}

{PANEL/}

{PANEL: Create subscription - include documents}

Here we create a subscription on the _Orders_ collection, which will send all the _Order_ documents.

In addition, the related _Product_ documents associated with each Order are **included** in the batch sent to the client.
This way, when the subscription worker that processes the batch in the client accesses a _Product_ document, no additional call to the server will be made.

See how to consume this type of subscription [here](../../../client-api/data-subscriptions/consumption/examples#subscription-that-uses-included-documents).

{CODE-TABS}
{CODE-TAB:python:Builder-syntax create_subscription_with_includes_strongly_typed@ClientApi\DataSubscriptions\DataSubscriptions.py /}
{CODE-TAB:python:RQL-path-syntax create_subscription_with_includes_rql_path@ClientApi\DataSubscriptions\DataSubscriptions.py /}
{CODE-TAB:python:RQL-javascript-syntax create_subscription_with_includes_rql_javascript@ClientApi\DataSubscriptions\DataSubscriptions.py /}
{CODE-TABS/}

{NOTE: }

**Include using builder**:

Include statements can be added to the subscription with `SubscriptionIncludeBuilder`.  
This builder is assigned to the  `includes` property in [SubscriptionCreationOptions](../../../client-api/data-subscriptions/creation/api-overview#subscriptioncreationoptions<t>).  
It supports methods for including documents as well as [counters](../../../client-api/data-subscriptions/creation/examples#create-subscription---include-counters).
These methods can be chained.  

To include related documents, use method `include_documents`.  
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

`SubscriptionIncludeBuilder` has three methods for including counters:  

{CODE:python include_builder_counter_methods@ClientApi\DataSubscriptions\DataSubscriptions.py /}

`include_counter` is used to specify a single counter.  
`include_counters` is used to specify multiple counters.  
`include_all_counters` retrieves all counters from all subscribed documents.  

| Parameter   | Type  | Description                                                                                                                                      |
|-------------|-------|--------------------------------------------------------------------------------------------------------------------------------------------------|
| **name**    | `str` | The name of a counter. The subscription will include all counters with this name that are contained in the documents the subscription retrieves. |
| **\*names** | `str` | Array of counter names.                                                                                                                          |

The following subscription, which includes multiple counters in the batch sent to the client,  
demonstrates how the methods can be chained.

{CODE:python create_subscription_include_counters_builder@ClientApi\DataSubscriptions\DataSubscriptions.py /}

{PANEL/}

{PANEL: Update existing subscription}

The subscription definition can be updated after it has been created.  
In this example we update the filtering query of an existing subscription named "my subscription".

{CODE:python update_subscription_example_0@ClientApi\DataSubscriptions\DataSubscriptions.py /}

---

{NOTE: }

**Modifying the subscription's name**:

In addition to the subscription name, each subscription is assigned a **subscription ID** when it is created by the server. 
This ID can be used to identify the subscription, instead of the name, when updating the subscription.

This allows users to change an existing subscription's **name** by specifying the subscription's ID  
and submitting a new string in the `name` field of `SubscriptionUpdateOptions`.

{CODE:python update_subscription_example_1@ClientApi\DataSubscriptions\DataSubscriptions.py /}

{NOTE/}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)

**Knowledge Base**:

- [JavaScript Engine](../../../server/kb/javascript-engine)
