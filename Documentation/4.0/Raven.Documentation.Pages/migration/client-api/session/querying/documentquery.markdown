# Migration : How to migrate DocumentQuery from 3.x?

There were many changes to `DocumentQuery` regarding specific functionalities, we encourage you to read about them in their respective articles, but the biggest change that this particular article wants to address is the **default operator change**.

In previous version of RavenDB there was a discrepancy between `Query` and `DocumentQuery` in regards to default operator. The `Query` was using `AND` and `DocumentQuery` was using `OR`. Now both of those types of query are using the same operator which is `AND`.

## Example I

Retrieve all employees which `FirstName` is `John` or `Bob` by using `UsingDefaultOperator` method.

| 3.x |
|:---:|
| {CODE document_query_1@Migration\ClientApi\Session\Querying\DocumentQuery.cs /} |

| 4.0 |
|:---:|
| {CODE document_query_2@Migration\ClientApi\Session\Querying\DocumentQuery.cs /} |

## Example II

Retrieve all employees which `FirstName` is `John` or `Bob` using `OrElse` method.

| 3.x |
|:---:|
| {CODE document_query_1@Migration\ClientApi\Session\Querying\DocumentQuery.cs /} |

| 4.0 |
|:---:|
| {CODE document_query_3@Migration\ClientApi\Session\Querying\DocumentQuery.cs /} |

