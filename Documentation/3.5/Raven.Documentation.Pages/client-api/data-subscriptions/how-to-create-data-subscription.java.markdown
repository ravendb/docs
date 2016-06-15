##How to create a data subscription?

In order to create a data subscription you have to specify the criteria that documents must match to be incorporated into subscription results.

{CODE:java create_1@ClientApi\DataSubscriptions\DataSubscriptions.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **clazz** | Class | Use for creating strongly typed subscription. |
| **criteria** | SubscriptionCriteria | Subscription criteria. |
| **database** | String | Name of database to create a data subscription. If `null`, default database configured in DocumentStore will be used. |

| Return value | |
| ------------- | ----- |
| long | A data subscription identifier. |

In `SubscriptionCriteria`, where you can specify the following filters:

* _KeyStartsWith_ - a document id must starts with a specified prefix,
* _BelongsToAnyCollection_ - list of collections that the subscription deals with,
* _PropertiesMatch_ - map of field names and related values that a document must have,
* _PropertiesNotMatch_ - map of field names and related values that a document must not have,
* _StartEtag_ - an etag of a document which a subscription is going to consider as already acknowledged and start processing docs with higher etags.

_BelongsToAnyCollection_ will be automatically filled in by a single value based on the entity type (see [findTypeTagName](../configuration/conventions/identifier-generation/global#findtypetagname-and-finddynamictagname) convention), when using override with clazz parameter.

Additionally the criteria object has _StartEtag_ property which sets the etag of the first document that the subscription starts from (in large databases can be used to skip documents that you already know they won't match the given criteria).

The execution of `create` method will create a data subscription in a database and return its identifier (you can see `Raven/Subscriptions/[id]` documents in the database).
This identifier is needed to open the subscription so you need to keep that information to be able to make use of it later.

###Example I

{CODE:java create_2@ClientApi\DataSubscriptions\DataSubscriptions.java /}

In this case we want to get all documents that id starts with `employees/` but does not contain `Address.City` field with value `Seattle`.
Note that although we specified `employees/` prefix id it doesn't mean we will get documents belonging to the same collection. It can return a developer (`employees/developers/1`)
as well as a manager (`employees/managers/1`), anyhow we can be sure that none of them is living in `Seattle`.

###Example II

{CODE:java create_3@ClientApi\DataSubscriptions\DataSubscriptions.java /}

In this case, it will give us all the orders, but only those which are handled by `employees/1` employee. Here we used generic type criteria, so we can specify properties filtering in strongly typed way.
Also we can be sure that all returned documents belong to `Orders` collection (so we will be able to open the strongly typed subscription).
