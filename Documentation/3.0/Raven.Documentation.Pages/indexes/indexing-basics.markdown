# Indexing basics

To achieve very fast response times, RavenDB handles **indexing in the background** whenever data is added or changed. This approach allows the server to respond quickly even when large amounts of data have changed, however drawback of this choice is that results might be stale (more about staleness in next section). Underneath, the server is using [Lucene](http://lucene.apache.org/) to perform indexation and because of that queries are using Lucene-syntax (with some [additions](../indexes/querying/full-query-syntax)) to perform search.

## Stale indexes

The notion of stale indexes comes from an observation deep in RavenDB's design, assuming that the user should never suffer from assigning the server big tasks. As far as RavenDB is concerned, it is better to be stale than offline, and as such it will return results to queries even if it knows they may not be as up-to-date as possible.

And indeed, RavenDB returns quickly for every client request, even if involves re-indexing hundreds of thousands of documents. And since the previous request has returned so quickly, the next query can be made a millisecond after that and results will be returned, but they will be marked as `Stale`.

{INFO You can read more about stale indexes [here](../indexes/stale-indexes). /}

## Lucene

As mentioned earlier, RavenDB uses `Lucene` syntax (with some [additions](../indexes/querying/full-query-syntax)) for querying. The easiest way for us would be to expose a method in which you could pass your Lucene-flavored query as a string (we [did](../client-api/commands/querying/how-to-query-a-database) that) and do not bother about anything else.

The fact is, that we did not stop at this point, we went much further, by exposing LINQ-based querying with strong-type support that hides all Lucene syntax complexity:

{CODE-TABS}
{CODE-TAB:csharp:Method-syntax indexes_2@Indexes/IndexingBasics.cs /}
{CODE-TAB:csharp:Query-syntax indexes_1@Indexes/IndexingBasics.cs /}
{CODE-TABS/}

Of course, we did not forget about our advanced users, they can create queries manually by using low-level commands or `DocumentQuery` (API reference [here](../client-api/session/querying/lucene/how-to-use-lucene-in-queries)) in advanced session operations:

{CODE indexes_3@Indexes/IndexingBasics.cs /}

By the end, all of the above queries will execute [Query command](../client-api/commands/querying/how-to-query-a-database) with (in this case) a very simple Lucene-flavored query `FirstName:Robert`:

{CODE indexes_4@Indexes/IndexingBasics.cs /}

## Type of indexes

You probably know that indexes can be divided by their source of origin to the `static` and `auto` indexes (if not, read about it [here](../indexes/creating-and-deploying)), but more interesing division is by functionality and in this case we have `Map` and `Map-Reduce` indexes.

`Map` indexes (sometimes refered as simple indexes) contain one (or more) mapping functions that indicate which fields from documents should be indexed (in other words they indicate which documents can be searched by which fields).

On the other hand there are `Map-Reduce` indexes that allow complex aggregations to be performed in two-step process. First by selecting appropriate records (using Map function), then by applying specified reduce function to these records to produce smaller set of results.

{INFO:Map Indexes}
We urge you to read more about `Map` indexes [here](../indexes/map-indexes).
{INFO/}

{INFO:Map-Reduce Indexes}
More detailed information about `Map-Reduce` indexes can be found [here](../indexes/map-reduce-indexes).
{INFO/}

### Default index

Each RavenDB database comes with a built-in index called `Raven/DocumentsByEntityName`. Its purpose is to index all documents by using two metadata fields: `Raven-Entity-Name`
and `Last-Modified` (if they are present). Thanks to this we can query for documents that belong only to the given [collection](../client-api/faq/what-is-a-collection) or by specifying the modification date.

{CODE raven_by_entity_name@Indexes/IndexingBasics.cs /}

## Related articles

TODO