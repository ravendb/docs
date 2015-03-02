#Basics

Another great feature offered by the session is the querying support. `Query` method is an entry point to build a query that will be used to look
for files on a file system according to specified criteria.

##Syntax

{CODE query_1@FileSystem\ClientApi\Session\Querying\Basics.cs /}

| Return Value | |
| ------------- | ------------- |
| **IAsyncFilesQuery&lt;FileHeader&gt;** | Instance implementing `IAsyncFilesQuery` interface containing additional query methods and extensions. |


The result of `Query` method returns an object that allows you to construct a query by using available predicates and logical operators.
In order to get the results you need to materialize the query by using one of the following methods:

* `ToListAsync`
* `FirstAsync`
* `FirstOrDefaultAsync`
* `SingleAsync`
* `SingleOrDefaultAsync`

{INFO: Async API}
The querying API the same like the rest of RavenFS client methods exposes only asynchronous methods.
{INFO/}

{SAFE: Default page size}
The default value of a page size for a query is 1024 results. In order to retrieve a different number of results in a single query use `.Take(pageSize)` method.
{SAFE/}

##Example I

If there is fewer than 1024 files in a file system then the below code will return all of them because the query does not contain any filtering
condition. If there is more than 1024 files it will return just first 1024 results according to default value of `pageSize` parameter.

{CODE query_2@FileSystem\ClientApi\Session\Querying\Basics.cs /}

##Example II

In this example we will get first file that has less than 100 bytes or `null` if there is no such file.

{CODE query_3@FileSystem\ClientApi\Session\Querying\Basics.cs /}
