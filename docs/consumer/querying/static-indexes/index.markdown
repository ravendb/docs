# Static indexes

RavenDB allows you to manually define named indexes, and query on them explicitly. Such indexes are called `static indexes`.

Since we already know indexes are created automatically for us by RavenDB whenever needed, why would one want to create his indexes manually? There are two reasons to prefer static indexes over dynamically created ones:

1. __High latency__ - Index creation is not a cheap process, and may take a while to execute. Since dynamic indexes are created on the fly on first user query, first non-stale results may take a long time to return. Since dynamic indexes are created as temporary indexes, this is going to be a performance issue on first run.

2. __Flexibility__ - Static indexes expose much more functionality, like custom sorting, Full Text Search, Live Projections, spatial search support, and more.

While dynamic indexes are very easy to work with, static indexes are much more useful and more efficient in indexing data in real time. Therefore, we recommend to base most operations in an application on static indexes, or at least ensure the temporary indexes created by dynamic indexing have been promoted to permanent indexes.

Querying static indexes is no different than what we have seen by now. Whenever you execute a query against RavenDB and an appropriate static index exists, RavenDB will direct your query to that index automatically. You can also specify the index name explicitly, like so:

{CODE static_indexes1@Consumer\StaticIndexes.cs /}

###  Note: RavenDB will throw if an explicit index name was used, when no such index exists.

In this chapter we will learn how to create static indexes, and explore the various capabilities they offer.

[FILES-LIST]