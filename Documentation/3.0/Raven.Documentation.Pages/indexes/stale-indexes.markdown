# Stale indexes

RavenDB performs data indexing in a background thread, which is executed whenever the new data comes in or the existing data is updated. Running this as a background thread allows the server to respond quickly, even when the large amounts of data have been changed, however in that case you may query stale indexes.

The notion of stale indexes comes from the close observation of the way RavenDB is designed and the assumption that the user should never suffer from assigning big tasks to a server. As far as RavenDB is concerned, it is better to be stale than offline, and as such it will return results to queries even if it knows they may not be as up-to-date as possible.

Indeed, RavenDB returns quickly each client request, even if it involves re-indexing hundreds of thousands of documents. And since the previous request has returned so quickly, the next query can be made a millisecond after that, and the results will be returned, although they will be marked as `Stale`.

## Checking for stale results

As part of the response when an index is queried, a property is attached indicating whether the results are stale - that is, whether there are currently any tasks outstanding against that index. It can be fetched using the [Statistics](../client-api/session/querying/how-to-get-query-statistics) method:

{CODE stale1@Indexes\StaleIndexes.cs /}

When `IsStale` is true, that means someone probably added or changed a `Product`, and the indexes didn't have enough time to fully update before our query.

In most cases you don't need to worry about it, however there are scenarios where you cannot work with possibly stale data.

## Explicitly waiting for non-stale results

When it is a requirement to get non-stale results back from a query, it is possible to specify this while querying:

{CODE stale2@Indexes\StaleIndexes.cs /}

Note that in the sample above a time-out of 5 seconds was specified. While you can ask RavenDB to wait indefinitely long until there are non-stale results, this should only be used in unit-testing, and never in a real-world application, unless you are 100% sure you understand the implications and it is what you actually demand.

## Setting cut-off point

A better approach to make sure you are working with non-stale results is to use a cut-off point and tell the server to use that as the point the database should index to:

{CODE stale3@Indexes\StaleIndexes.cs /}

This will make sure that you get the latest results up to that point in time. All pending tasks for changes that occurred after this cut-off point will not be considered. And just as before, a time-out can be set as well.

`WaitForNonStaleResultsAsOfNow` is also available; it is equivalent of calling `WaitForNonStaleResultsAsOf(DateTime.Now)`.

Another option is to use `WaitForNonStaleResultsAsOfLastWrite`, which does exactly what it says, namely, it tracks the last write by the application, and uses that as the cutoff point. This is usually recommended if you are working on the machines where clock synchronization might be an issue, since `WaitForNonStaleResultsAsOfLastWrite` doesn't use the machine time, but etag values for the writes.

{INFO:Convention}
You can also setup the document store to always wait for the last write, like this:

{CODE stale4@Indexes\StaleIndexes.cs /}

All queries in the store would behave as if `WaitForNonStaleResultsAsOfLastWrite` was applied to them.
{INFO/}

## Related articles

- [What are indexes?](../indexes/what-are-indexes)
- [Indexing : Basics](../indexes/indexing-basics)
