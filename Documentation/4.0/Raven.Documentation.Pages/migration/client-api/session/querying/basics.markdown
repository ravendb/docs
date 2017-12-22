# How to migrate Queries from 3.x?

Following changes occured in 4.0 and need to be considered when migration is done.

## Paging

There is no longer a default page size send from the client API (128 by default) and maxium page size that to which server will cut the results (1024 by default).

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

## Transformers and Projections

Transformers have been removed from the RavenDB. Please read our migration article tackling this change. The article can be found [here](../../../../migration/client-api/session/querying/transformers).

## Default Operator

Default operator for `session.Query` was and still is `AND`, but the operator for `DocumentQuery` have changed from `OR` to `AND`. We have created a dedicated article that helps you with migration. It can be found [here](../../../../migration/client-api/session/querying/documentquery).
