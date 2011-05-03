# Querying

In order to enable user queries to return fast, the RavenDB server instance indexes your data whenever it is added or changed within store. All indices in RavenDB are Lucene-based, and we take advantage of this to provide a very feature-rich and flexible querying system. RavenDB accepts Linq expressions as queries; whether they are sent from the Client API or through the HTTP RESTful API, they are being translated into a Lucene query and executed against the appropriate index.

For most operations this is done transparently, and all you need to be familiar with is Linq and some RavenDB-specific operations. However, there are many other handy features that only become available once you understand the way RavenDB operates on its indexes.

In this chapter we explore the various data querying options available in RavenDB, from the immediately visible and most frequently used ones, to more advanced topics like Includes, Attachments, Live Projections and more.

Before we start, it is important to understand that <u>all</u> queries sent to a RavenDB server use an index to return results. While you can define your own indexes manually (we will see how later), RavenDB does this for you automatically.

Therefore, there are 3 types of indexes in RavenDB:

* *Static indexes* are named indexes which are created explicitly by the user.

* *Dynamic indexes* are created by RavenDB automatically following some user query, if no matching index to query was found. Unless the user specifies what index to query explicitly, RavenDB will find an appropriate index to query, and create it on the fly if one does not already exist. Dynamically created indexes are temporary, and reside in memory until they reach 25MB in size, then they are copied to the file-system. If an index is being accessed enough times in a predefined amount of time, it graduates to become a permanent index, which is persisted. All these values are configurable.

* *Internal indexes*, general purpose indexes that are created by RavenDB.

Also worth mentioning at this stage is the notion of *stale indexes*. Because RavenDB follows the "better stale than offline" approach, querying an index may return stale results - for example when a user queries a database that while a mass-update in progress. RavenDB will let the user know if results are stale, and can also be told to wait until non-stale results are available.

We will start by understanding the full range of querying options supported by RavenDB, and showing how they can be used intuitively by the RavenDB Linq provider. Then we will step in and learn about the Map/Reduce indexes that RavenDB uses and how to create them by hand, and after that we will explore more, less-obvious options supported by RavenDB.

[FILES-LIST-RECURSIVE]