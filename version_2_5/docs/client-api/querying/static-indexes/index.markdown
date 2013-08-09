
# Static indexes

As we explained earlier in this chapter, RavenDB uses indexes to satisfy queries. Whenever a user issues a query RavenDB will use an existing index or create a new one if no matching index was found.

RavenDB also allows you to manually define indexes, and also to query on them explicitly. Such manually created indexes are called **static indexes** in contrast to automatically created ones called **dynamic or auto indexes**.

The process of index creation is the same for both static and dynamic ones, and it looks as follows:    
 - first we index actually to memory, without hitting the disk   
 - only if we reached a configurable size or finished indexing everything will we spill everything to disk.

Howevery there are a few reasons why to prefer static indexes over dynamically created ones:

1. __High latency__ - Index creation is not a cheap process, and may take a while to execute. Since dynamic indexes are created on the fly on first user query, first non-stale results may take a long time to return.

2. __Flexibility__ - Static indexes expose much more functionality, like custom sorting, boosting, Full Text Search, spatial search support, and more.

While dynamic indexes are very easy to work with, static indexes are much more useful and more efficient in indexing data in real time. Therefore, we recommend to base most operations in an application on static indexes.

Querying static indexes is no different than what we have seen by now. Whenever you execute a query against RavenDB and an appropriate static index exists, RavenDB will direct your query to that index automatically. You can also specify the index name explicitly, like so:

{CODE static_indexes1@ClientApi\Querying\StaticIndexes\Index.cs /}

or use the different syntax where the index is specified as the second generic type of the query method:

{CODE static_indexes2@ClientApi\Querying\StaticIndexes\Index.cs /}

{NOTE RavenDB will throw if an explicit index name or type was used, when no such index exists. /}

In this chapter we will learn how to create static indexes, and explore the various capabilities they offer.
