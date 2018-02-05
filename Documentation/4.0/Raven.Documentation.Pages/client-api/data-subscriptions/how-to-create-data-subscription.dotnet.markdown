# How to create a data subscription?

{INFO:General}

Subscriptions in their essence all defined by an RQL like query that describes both the filtering and the projection. For convenience, we've provided several ways to express that definition, we'll cover all of them in this page.

This page covers two topics:

[Common subscription creation examples](how-to-create-data-subscription#common-subscription-creation-examples): Coverst most common use cases

[API overview](how-to-create-data-subscription#api-overview): Describes detailed overview of the subscriptions creation API

{INFO/}

## Common subscription creation examples

{PANEL:Create subscription on whole collection}

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

{PANEL:Create versioning enabled subscription}

Here we create a subscription on Orders collection, which returns current and previous version of the subscriptions. Please see the versioning subscription dedicated page for more details.
{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax create_filter_and_projection_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TAB:csharp:RQL-syntax create_versioned_subscription_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TABS/}

{PANEL/}

## Advanced subscription creation options

`SubscriptionCreationOptions` allows to define several parameters that we didn't speak about yet:

* Name - User defined name of the subscription: allows you to have a human readable identification of a subscription.
* ChangeVector - Allows to define a change vector, from which the subscription will start processing. It might be useful for ad-hoc processes that need to process only recent changes in data, for that specific use, the field may receive a special value: "LastDocument", that will take the latest change vector in the machine. 
* MentorNode - Allows to define a specific node in the cluster that we want to treat the subscription. That's useful in cases when one server is preffered over other, either because of stronger hardware or closer geographic proximity to clients etc.

## API overview

{PANEL:SubscriptionCreationOptions}

Non generic version of the class, relies on user's full knowledge of the RQL query structure

| Member | Type | Description |
|--------|:-----|-------------| 
| **Name** | `string` | User defined name of subscription. |
| **Query** | `string` | **Required.** RQL query that describes the subscription. That RQL comes with additional support to javascript clause inside the 'Where' statement and special semantics for subscriptions on documents revisions.
| **ChangeVector** | `string` | Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |
| **MentorNode** | `string` | Node tag of the preffered node to run the subscription. If the node is down, the subscription Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |

{PANEL/}

{PANEL:SubscriptionCreationOptions&lt;T&gt;}

An RQL statement will be built based on the fields.

| Member | Type | Description |
|--------|:-----|-------------| 
| **&lt;T&gt;** | type | Type of the object, from which the collection will be derived. |
| **Name** | `string` | User defined name of subscription. |
| **Filter** | `Expression<Func<T, bool>>` | Lambda describing filter logic for the subscription. Will be transalted to a javascript function.
| **Projection** | `Expression<Func<T, object>>` | Lambda describing the projection of returned documents. |Will be translated to a javascript function
| **ChangeVector** | `string` | Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |
| **MentorNode** | `string` | Node tag of the preffered node to run the subscription. If the node is down, the subscription Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |

{PANEL/}

{PANEL:Create and CreateAsync overloads summary}

{CODE definition@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **predicate** | `Expression<Func<T, bool>>` | Predicate that returns a boolean, describing filter of the subscription documents |
| **options** | `SubscriptionCreationOptions<T>` | Contains subscription creation options |
| **database** | `string` | Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |

| Return value | |
| ------------- | ----- |
| `string` | Created data subscription name. If Name was provided in SubscriptionCreationOptions, it will be returned, otherwise, a unique name will be generated by server. |

{PANEL/}

## Related articles

- [What are data subscriptions?](../../client-api/data-subscriptions/what-are-data-subscriptions)
- [How to **consume** a data subscription?](../../client-api/data-subscriptions/how-to-consume-data-subscription)
