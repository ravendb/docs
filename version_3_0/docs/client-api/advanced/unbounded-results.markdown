#Unbounded results API

##Query streaming

By default RavenDB limits the number of returned query results to protect you against unbounded result sets. You can use a paging
mechanism to control the range and the number of items that a server will return. However, there are times when you really want to
get _all of them_ in a single request. This is especially important for exporting purposes on a running system, because if you
used the paging you could get duplicates or miss some items.

The unbounded results implementation is based on the following approach:

* do it in a single request (we don't want to store any state to keep the client and the server in sync),
* use a streaming based model (to avoid memory usage problems in case of loading millions of records),
* freeze the returned stream (what you read is a snapshot of the data as it was when you started reading it).

In order to take advantage of the query results streaming use the code:

{CODE query_streaming_1@ClientApi\Advanced\UnboundedResults.cs /}

As you can see to get all of the query results you need to iterate by using the enumerator returned by `Stream()` method. The important
thing is that the streaming API does not track the entities in the session, and will not includes changes there when `SaveChanges()`
is called.

The same way you can also stream the results of the Lucene query:

{CODE query_streaming_2@ClientApi\Advanced\UnboundedResults.cs /}

Apart from the query parameter you can also pass the second `out` parameter to retrieve `QueryHeaderInformation`:

{CODE query_streaming_3@ClientApi\Advanced\UnboundedResults.cs /}

{CODE query_streaming_4@ClientApi\Advanced\UnboundedResults.cs /}

##Documents streaming

By using the `Stream()` method you are also able to download the documents directly without specifying any query. There are two methods for this purpose.
The first one accepts ETag of a document that you want to starts from:

{CODE doc_streaming_1@ClientApi\Advanced\UnboundedResults.cs /}

If you use the second one then you will have to provide a prefix key of the documents you want to stream and optionally a string value that
should match the documet key after the specified prefix:

{CODE doc_streaming_2@ClientApi\Advanced\UnboundedResults.cs /}

The query above will return all documents where ID starts with _"users/"_ and the end matches the expression _"*Ra?en"_ (e.g. "users/Admins/Raven", "Users/Ragen").

Note that here you still have an option to page the results. The parameters _start_ and _pageSize_ have the default values here.

## Async version

There is also an asynchronous version of Unbounded results API available. Here is the sample usage presented:

{CODE query_streaming_async@ClientApi\Advanced\UnboundedResults.cs /}
 
