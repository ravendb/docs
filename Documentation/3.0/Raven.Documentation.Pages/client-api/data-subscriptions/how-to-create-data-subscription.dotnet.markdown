##How to create a data subscription?

In order to create a data subscription you have to specify the criteria that documents must match to be incorporated into subscription results. Note there are two types of
criteria you can pass into `Create` method:

{CODE create_1@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **criteria** | SubscriptionCriteria / SubscriptionCriteria&lt;T&gt; | Subscription criteria - standard or strongly typed. |
| **database** | string | Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |

| Return value | |
| ------------- | ----- |
| long | A data subscription identifier. |

The first one is `SubscriptionCriteria`, where you can specify the following filters:

* _KeyStartsWith_ - a document id must starts with a specified prefix,
* _BelongsToAnyCollection_ - list of collections that the subscription deals with,
* _PropertiesMatch_ - dictionary of field names and related values that a document must have,
* _PropertiesNotMatch_ - dictionary of field names and related values that a document must not have.

The second one is generic `SubscriptionCriteria<T>` where `T` is an entity type. Analogously you can set:

* <em>KeyStartsWith</em>,
* <em>PropertiesMatch</em>,
* <em>PropertiesNotMatch</em>,

_BelongsToAnyCollection_ will be automatically filled in by a single value based on the entity type (see [FindTypeTagName](../configuration/conventions/identifier-generation/global#findtypetagname-and-finddynamictagname) convention).

Additionally the criteria object has _StartEtag_ property which sets the etag of the first document that the subscription starts from (in large databases can be used to skip documents that you already know they won't match the given criteria).

The execution of `Create` method will create a data subscription in a database and return its identifier (you can see `Raven/Subscriptions/[id]` documents in the database).
This identifier is needed to open the subscription so you need to keep that information to be able to make use of it later.

###Example I

{CODE create_2@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

In this case we want to get all documents that id starts with `employees/` but does not contain `Address.City` field with value `Seattle`.
Note that although we specified `employees/` prefix id it doesn't mean we will get documents belonging to the same collection. It can return a developer (`employees/developers/1`)
as well as a manager (`employees/managers/1`), anyhow we can be sure that none of them is living in `Seattle`.

###Example II

{CODE create_3@ClientApi\DataSubscriptions\DataSubscriptions.cs /}

In this case, it will give us all the orders, but only those which are handled by `employees/1` employee. Here we used generic type criteria, so we can specify properties filtering in strongly typed way.
Also we can be sure that all returned documents belong to `Orders` collection (so we will be able to open the strongly typed subscription).
