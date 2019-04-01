# Indexes: Stale Indexes

RavenDB performs indexing in the background threads. The indexes start processing whenever the new data comes in, and the existing documents are updated or deleted.

Running them in the background allows the server to return query results immediately even if a large number of documents has been just changed.

However, in that case the index is stale until it processes them. 

The notion of stale indexes comes from the close observation of the way RavenDB is designed, and the assumption that the user should never suffer from assigning big tasks to
the server. As far as RavenDB is concerned, it is better to be stale than offline, and it will return query results even if it knows they may not be up-to-date.

## Checking for Stale Results

The querying response has an `isStale` method indicating whether the results are stale: that is, whether there were any outstanding tasks against the index at the time of querying.

It can be retrieved using the [Statistics](../client-api/session/querying/how-to-get-query-statistics) method:

{CODE:java stale1@Indexes\StaleIndexes.java /}

When `isStale` is true, some `Product` has been just added or changed, and the index didn't have enough time to fully update before the query.

Typically, the index updates the records within milliseconds. But there are scenarios where you cannot work with possibly stale data.

## Explicitly Waiting for Non-Stale Results

In order to assure that a query won't return stale results, you can use a few approaches. For each of them the default wait timeout will be 15 seconds unless it was specified
differently by the user. It is handled by the server, the client won't send any additional requests meanwhile. If it exceeds the timeout then a `TimeoutException` will be thrown.

### Customizing Single Query

You have an option to instruct that a particular query should wait until the index is up-to-date:

{CODE:java stale2@Indexes\StaleIndexes.java /}

### Customizing All Queries

You can also apply such customization at the document store level so all queries will wait for non-stale results:

{CODE:java stale3@Indexes\StaleIndexes.java /}

### Waiting for Documents Stored in Session

If you need to ensure the indexes process, the documents stored in the current session before `saveChanges` returns, you can use:

{CODE:java stale4@Indexes\StaleIndexes.java /}

It will wait for the indexes to catch up with the just saved changes. You can control the behavior and specify indexes you want to wait for using the optional 
parameters in the following example:

{CODE:java stale5@Indexes\StaleIndexes.java /}

The default parameters are:

  - timeout - null (will wait 15 seconds),
  - throwOnTimeout - false,
  - indexes - null (will wait for all indexes impacted by the changes made in the session)
  
{DANGER:Beware of waiting for non stale results overuse}

The indexing mechanism in RavenDB is built on [a BASE model](../client-api/faq/transaction-support#base-for-query-operations). 
In order to avoid querying consistency pitfalls, you need to consider this at the data modeling phase.

The usage of `WaitForNonStaleResults` at the query level is usually reasonable on only rare occasions. 
Taking advantage of `WaitForNonStaleResults` customization applied to the all queries is very often a symptom of deeper issues in an application model and 
misunderstanding of the querying concepts in RavenDB. 
{DANGER/}


## Cutoff Point

If a query sent to the server specifies that it needs to wait for non-stale results then RavenDB sets the cutoff Etag for the staleness check.
It is the Etag of the last document (or document tombstone), from the collection(s) processed by the index, as of the query arrived to the server.
This way the server won't be waiting forever for the non-stale results even though documents are constantly updated meanwhile.

If the last Etag processed by the index is greater than the cutoff, then the results are considered as non-stale.

## Related Articles

### Indexes

- [What are Indexes](../indexes/what-are-indexes)
- [Indexing Basics](../indexes/indexing-basics)

### Client API

- [How to Customize Query](../client-api/session/querying/how-to-customize-query)
