#Basics

Another great feature offered by the session is the querying support. The `Query` method is an entry point to build a query that will be used to look for files on a file system according to the specified criteria.

##Syntax

{CODE query_1@FileSystem\ClientApi\Session\Querying\Basics.cs /}

| Return Value | |
| ------------- | ------------- |
| **IAsyncFilesQuery&lt;FileHeader&gt;** | Instance implementing `IAsyncFilesQuery` interface containing additional query methods and extensions. |


The `Query` method returns an object that allows you to construct a query by using available predicates and logical operators.
In order to get the results you need to materialize the query by using one of the following methods:

* `ToListAsync`
* `FirstAsync`
* `FirstOrDefaultAsync`
* `SingleAsync`
* `SingleOrDefaultAsync`

{INFO: Async API}
The querying API is the same as the rest of RavenFS client methods, which means it exposes only the asynchronous methods.
{INFO/}

{SAFE: Default page size}
The default value of a page size for a query is 1024 results. In order to retrieve a different number of results in a single query use the `.Take(pageSize)` method.
{SAFE/}

##Example I

If there are fewer than 1024 files in a file system, the below code will return all of them because the query does not contain any filtering condition. If there are more than 1024 files, it will return only the first 1024 results according to the default value of the `pageSize` parameter.

{CODE query_2@FileSystem\ClientApi\Session\Querying\Basics.cs /}

##Example II

In this example we will get the first file that has less than 100 bytes or `null` if no such file does not exist.

{CODE query_3@FileSystem\ClientApi\Session\Querying\Basics.cs /}


## Related articles

- [Indexing](../../../indexing)