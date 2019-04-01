# Data Subscriptions: Common Data Subscription Creation Examples

---

{NOTE: }

In this page:  

[Create subscription on all documents in a collection](../../../client-api/data-subscriptions/creation/examples#create-subscription-on-all-documents-in-a-collection)  
[Create subscription with filtering](../../../client-api/data-subscriptions/creation/examples#create-subscription-with-filtering)  
[Create subscription with filtering and projection](../../../client-api/data-subscriptions/creation/examples#create-subscription-with-filtering-and-projection)  
[Create subscription with load document in filter projection](../../../client-api/data-subscriptions/creation/examples#create-subscription-with-load-document-in-filter-projection)  
[Create revisions enabled subscription](../../../client-api/data-subscriptions/creation/examples#create-revisions-enabled-subscription)  

{NOTE/}

---

{PANEL:Create subscription on all documents in a collection}

Here we create a plain subscription on the Orders collection, without any constraint or transformation.
{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax create_whole_collection_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TAB:csharp:RQL-syntax create_whole_collection_RQL@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:Create subscription with filtering}

Here we create a subscription on Orders collection, which total order revenue is greater than 100.
{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax create_filter_only_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TAB:csharp:RQL-syntax create_filter_only_RQL@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:Create subscription with filtering and projection}

Here we create a subscription on Orders collection, which total order revenue is greater than 100, and return only ID and total revenue.
{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax create_filter_and_projection_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TAB:csharp:RQL-syntax create_filter_and_projection_RQL@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:Create subscription with load document in filter projection}

Here we create a subscription on Orders collection, which total order revenue is greater than 100, and return ID, total revenue, shipping address and responsible employee name.
{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax create_filter_and_projection_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TAB:csharp:RQL-syntax create_filter_and_projection_RQL@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:Create revisions enabled subscription}

Here we create a subscription on Orders collection, which returns current and previous version of the subscriptions. 
Please see the [page](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning) dedicated to subscriptions with revisions for more details and examples.

{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax create_simple_revisions_subscription_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TAB:csharp:RQL-syntax create_simple_revisions_subscription_RQL@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

**Data Subscriptions**:

- [What are Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to Create a Data Subscription](../../../client-api/data-subscriptions/creation/how-to-create-data-subscription)
- [How to Consume a Data Subscription](../../../client-api/data-subscriptions/consumption/how-to-consume-data-subscription)

**Knowledge Base**:

- [JavaScript Engine](../../../server/kb/javascript-engine)
