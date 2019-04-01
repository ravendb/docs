# Migration: How to migrate Queries from 3.x?

Following changes occurred in 4.0 and need to be considered when migration is done.

{PANEL:Namespaces}

The following namespaces are no longer valid and have to be removed:

- Raven.Client.Linq
- Raven.Json.Linq

{PANEL/}

{PANEL:Paging}

There is no longer a default page size send from the client API (128 by default) and maximum page size that to which server will cut the results (1024 by default).

Following query will return **all** results from the database (even when there is 1M of them):

{CODE basics_1_0@Migration\ClientApi\Session\Querying\Basics.cs /}

What can be done to mitigate this?

1. `DocumentConventions.ThrowIfQueryPageSizeIsNotSet` can be set to **true** so all queries will throw if their page size is not explicitly set (even when you have just a few records in the database, check is done on the client-side before request is sent to the server).
2. Set on all queries maximum number of results that you expect using `.Take(pageSize)` method.

To fix previous example we will set the convention to `true` and add `Take` to the query:

{CODE basics_1_2@Migration\ClientApi\Session\Querying\Basics.cs /}

{CODE basics_1_1@Migration\ClientApi\Session\Querying\Basics.cs /}

{INFO:Performance Hint}

If the number of records exceeds 2048 (default value that can be changed by `PerformanceHints.MaxNumberOfResults` configuration option) then performance hint notification will be issued and visible in the Studio. Giving you enough information to perform counter-measures if necessary.

{INFO/}

You can read more about paging in our [dedicated article](../../../../indexes/querying/paging).

{PANEL/}

{PANEL:Transformers and Projections}

Transformers have been removed from the RavenDB. Please read our migration article tackling this change. The article can be found [here](../../../../migration/client-api/session/querying/transformers).

{PANEL/}

{PANEL:Default Operator}

Default operator for `session.Query` was and still is `AND`, but the operator for `DocumentQuery` have changed from `OR` to `AND`. We have created a dedicated article that helps you with migration. It can be found [here](../../../../migration/client-api/session/querying/documentquery).

{PANEL/}

{PANEL:Waiting for Non Stale Results}

The following methods have been removed:

- `WaitForNonStaleResultsAsOf`
- `WaitForNonStaleResultsAsOfNow`

You should use `WaitForNonStaleResults` instead. Its behavior has changed in 4.0 - it's like `WaitForNonStaleResultsAsOfNow` in 3.x. Please read a dedicated article discussing [how to deal with non stale results](../../../../indexes/stale-indexes).

{CODE basics_1_3@Migration\ClientApi\Session\Querying\Basics.cs /}

{PANEL/}

{PANEL:Raven/DocumentsByEntityName index}

`Raven/DocumentsByEntityName` is no longer necessary in RavenDB 4.0. You can perform queries directly on collections, in particular we support queries that you can run to modify the docs:

| 3.x | 4.0 |
|:---:|:---:|
| {CODE basics_1_5@Migration\ClientApi\Session\Querying\Basics.cs /} | {CODE basics_1_6@Migration\ClientApi\Session\Querying\Basics.cs /} |

{PANEL/}

{PANEL:RavenQueryStatistics}

`RavenQueryStatistics` has been renamed to `QueryStatistics`.

{PANEL/}

{PANEL:As}

`As` has been removed in favor to built-in LINQ method `OfType`.

{PANEL/}

{PANEL:ProjectFromIndexFieldsInto}

`ProjectFromIndexFieldsInto` has been renamed to `ProjectInto`.

{PANEL/}
