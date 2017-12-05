# How to create a data subscription?

Subscriptions in their essence all defined by an RQL like query that describes both the filtering and the projection. For conveniece, we've provided several ways to express that defintion, we'll cover all of them in this page.

## Common subscription creation examples
{PANEL: Create subscription on whole collection}
Here we create a plain subscription on the Orders collection, without any constraint or transformation.
{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax create_whole_collection_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TAB:csharp:RQL-syntax create_whole_collection_RQL@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL: Create subscription with filtering}
Here we create a subscription on Orders collection, which total order revenue is greater than 100.
{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax create_filter_only_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TAB:csharp:RQL-syntax create_filter_only_RQL@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL: Create subscription with filtering and projection}
Here we create a subscription on Orders collection, which total order revenue is greater than 100, and return only Id and total revenue.
{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax create_filter_and_projection_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TAB:csharp:RQL-syntax create_filter_and_projection_RQL@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TABS/}
{PANEL/}


{PANEL: Create subscription with load document in filter projection }
Here we create a subscription on Orders collection, which total order revenue is greater than 100, and return Id, total revenue, shipping address and responsilbe employee name.
{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax create_filter_and_projection_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TAB:csharp:RQL-syntax create_filter_and_projection_RQL@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TABS/}
{PANEL/}


{PANEL: Create versioning enabled subscription}
Here we create a subscription on Orders collection, which returns current and previous version of the subscriptions. Please see the versioning subscription dedicated page for more details.
{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax create_filter_and_projection_generic@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TAB:csharp:RQL-syntax create_filter_and_projection_RQL@ClientApi\DataSubscriptions\DataSubscriptions.cs /}
{CODE-TABS/}
{PANEL/}

## Advanced subscription creation options
'SubscriptionCreationOptions' allows to define several parameters that we didn't speak about yet:

* Name - User defined name of the subscription: allows you to have a human readable identification of a subscription.
* ChangeVecotr - Allows to define a change vector, from which the subscription will start processing. It might be usefull for ad-hoc processes that need to process only recent changes in data, for that specific use, the field may receive a special value: "LastDocument", that will take the latest change vector in the mahcine. 
* MentorNode - Allows to define a specific node in the cluster that we want to treat the subscription. That's usefull in cases when one server is preffered over other, either because of stronger hardware or closer geographic proximity to clients etc.

## API overview

### SubscriptionCreationOptions

Non generic version of the class, relies on user's full knowledge of the RQL query structure

| Member | Type | Required | Description |
| ------------- | -- | ------------- | ----- | 
| **Name** | string | N | User defined name of subscription. |
| **Query** | string | Y | RQL query that describes the subscription. That RQL comes with additional support to javascript clause inside the 'Where' statement and special semantics for subscriptions on documents revisions.
| **ChangeVector** | string | N | Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |
| **MentorNode** | N | string | Node tag of the preffered node to run the subscription. If the node is down, the subscription Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |

### SubscriptionCreationOptions&lt;T&gt;

An RQL statement will be built based on the fields.

| Member | Type | Required | Description |
| ------------- | -- | ------------- | ----- | 
| **&lt;T&gt;** | type | Y | Type of the object, from which the collection will be derived. |
| **Name** | string | N | User defined name of subscription. |
| **Filter** | Expression<Func<T, bool>> | N | Lambda describing filter logic for the subscription. Will be transalted to a javascript function.
| **Projection** | Expression<Func<T, object>> | N | Lamda describing the projection of returned documents. |Will be translated to a javascript funciton
| **ChangeVector** | N | string | Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |


### IReliableSubscription.Create\CreateAsync overloads summary
All overload apply both to sync and async create variants.

{PANEL: Return value}

| Return value | |
| ------------- | ----- |
| string | Created data subscription name. If Name was provided in SubscriptionCreationOptions, it will be returned, otherwise, it will be generated by server. | 

{PANEL/}


{PANEL: Create(SubscriptionCreationOptions criteria, string database = null)}

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | SubscriptionCreationOptions&lt;T&gt; | Contains subscription creation options, must have at least "query" field |
| **database** | string | Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |

{PANEL/}

{PANEL: Create&lt;T&gt;(SubscriptionCreationOptions&lt;T&gt; options, string database = null)}

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | SubscriptionCreationOptions&lt;T&gt; | Contains typed subscription creation options. |
| **database** | string | Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |

{PANEL/}

{PANEL: Create&lt;T&gt;(Expression&lt;Func&lt;T, bool&gt;&gt; predicate = null, SubscriptionCreationOptions options = null, string database = null)}

| Parameters | | |
| ------------- | ------------- | ----- |
| **predicate** | Expression&lt;Func&lt;T, bool&gt;&gt; | Predicate that returns a boolean, describing filter of the subscription documents |
| **options** | SubscriptionCreationOptions&lt;T&gt; | Contains typed subscription creation options. |
| **database** | string | Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |

{PANEL/}
