#Paging

Paging is the process of splitting a set of results into pages and reading one page at a time. This is useful for optimizing bandwidth traffic, optimizing hardware usage, or simply because no user can handle huge amounts of data at once.

{SAFE: Safe By Default}
The default value of the page size on the client side is 128.
{SAFE/}

##Take

In order to overwrite the default page size use the `Take` method:

{CODE paging_1@FileSystem\ClientApi\Session\Querying\Paging.cs /}

##Skip

Use the `Skip` method to specify the number of results to skip to be able to get the results from the next pages:

{CODE paging_2@FileSystem\ClientApi\Session\Querying\Paging.cs /}

##Example

The following code retrieves all files; each page contains no more that 10 items:

{CODE paging_3@FileSystem\ClientApi\Session\Querying\Paging.cs /}