# Indexes: Creating and Deploying Indexes
---

{NOTE: }

* Indexes are used by the server to satisfy queries.  
  They are at the heart of RavenDB's efficiency and should be understood before indexes and queries are defined in production.

* Static indexes can do a number of operations on the data behind the scenes so that queries that use this already processed data are as fast as possible.  
  Indexes keep the processed data in a separate storage so that the raw data isn't affected.

* Whenever a user issues a query _that doesn't specify an index_, RavenDB's Query Optimizer will try to find an 
  existing auto-index that fulfills the query.  
   * If one doesn't yet exist, RavenDB will either create an auto-index or optimize an existing one if it almost satisfies the query.

* Indexes process data assigned to them as the data changes. For example, if changes are made to documents in the collection "Orders", 
  the indexes that are defined to handle queries on "Orders" will be triggered to update the index with the new data.
   * These behind-the-scenes processes remove a lot of burden from queries. Also, indexes need to process entire datasets just once, 
     after which, they only process new data.  
     Still, they utilize machine resources and this should be considered when defining indexes and queries.  


{INFO: Auto and Static Indexes}

* Indexes created by issuing a query are called `dynamic` or `Auto` indexes. 
   * They can be easily identified. Their name starts with the `Auto/` prefix.
   * If no [Auto-Index](../indexes/creating-and-deploying#auto-indexes) exists to satisfy a query, 
     a new Auto-Index will be created and maintained automatically.
* Indexes created explicitly by the user are called `static`.
   * [To use a Static Index in a query](../indexes/querying/query-index), 
     **you must specify the index in the query definition**. If you don't specify the index, 
     RavenDB will look for an auto-index and potentially create a new one.  
   * Static Indexes can be defined to do calculations, conversions, and various other processes behind the scenes, to prevent
     doing these costly processes at query time (see [Using AbstractIndexCreationTask](../indexes/creating-and-deploying#using-abstractindexcreationtask), 
     our [map-indexes](../indexes/map-indexes) article, [indexing multiple collections](../indexes/multi-map-indexes), 
     and [map-reduce indexing](../indexes/map-reduce-indexes)).  

{INFO/}

* In this page:
   * [Static indexes](../indexes/creating-and-deploying#static-indexes)
      * [Using AbstractIndexCreationTask](../indexes/creating-and-deploying#using-abstractindexcreationtask)
         * [Naming Convention](../indexes/creating-and-deploying#naming-convention)
         * [Sending to Server](../indexes/creating-and-deploying#sending-to-server)
         * [Creating an Index with Custom Configuration](../indexes/creating-and-deploying#creating-an-index-with-custom-configuration)
         * [Using Assembly Scanner](../indexes/creating-and-deploying#using-assembly-scanner)
         * [Example](../indexes/creating-and-deploying#example)
      * [Using Maintenance Operations](../indexes/creating-and-deploying#using-maintenance-operations)
         * [IndexDefinitionBuilder](../indexes/creating-and-deploying#indexdefinitionbuilder)
   * [Auto-indexes](../indexes/creating-and-deploying#auto-indexes)
      * [Naming Convention](../indexes/creating-and-deploying#naming-convention-1)
      * [Auto Indexes and Indexing State](../indexes/creating-and-deploying#auto-indexes-and-indexing-state)
   * [If Indexes Exhaust System Resources](../indexes/creating-and-deploying#if-indexes-exhaust-system-resources)


{NOTE/}

{PANEL:**Static indexes**}

There are a couple of ways to create a `static index` and send it to the server. We can use [maintenance operations](../indexes/creating-and-deploying#using-maintenance-operations) or create a [custom class](../indexes/creating-and-deploying#using-abstractindexcreationtask). 

<hr />

### Using AbstractIndexCreationTask

AbstractIndexCreationTask let you avoid hard-coding index names in every query.

{NOTE We recommend creating and using indexes in this form due to its simplicity. There are many benefits and few disadvantages. /}

#### Naming Convention

There is only one naming convention: each `_` in the class name will be translated to `/` in the index name.

e.g.

In the `Northwind` samples, there is a index called `Orders/Totals`. To get such a index name, we need to create a class called `Orders_Totals`.

{CODE:java indexes_1@Indexes/Creating.java /}

#### Sending to Server

There is not much use from an index if it is not deployed to the server. To do so, we need to create an instance of our class that inherits from `AbstractIndexCreationTask` and use `execute` method.

{CODE:java indexes_2@Indexes/Creating.java /}

{CODE:java indexes_3@Indexes/Creating.java /}

{SAFE If an index exists on the server and the stored definition is the same as the one that was sent, it will not be overwritten. The indexed data will not be deleted and indexation will not start from scratch. /}

#### Creating an Index with Custom Configuration

If you need to create an index with a custom [`index configuration`](../server/configuration/indexing-configuration) you can set them in the index class constructor like so: 
{CODE:java indexes_9@Indexes/Creating.java /}

#### Example

{CODE:java indexes_8@Indexes/Creating.java /}

<hr />

### Using Maintenance Operations

The `PutIndexesOperation` maintenance operation (which API references can be found [here](../client-api/operations/maintenance/indexes/put-indexes)) can be used also to send index(es) to the server.

The benefit of this approach is that you can choose the name as you feel fit, and change various settings available in `IndexDefinition`. You will have to use string-based names of indexes when querying.

{CODE:java indexes_5@Indexes/Creating.java /}

#### IndexDefinitionBuilder

`IndexDefinitionBuilder` is a very useful class that enables you to create `IndexDefinitions` using strongly-typed syntax with access to low-level settings not available when the `AbstractIndexCreationTask` approach is used.

{CODE:java indexes_6@Indexes/Creating.java /}

#### Remarks

{INFO Maintenance Operations or `IndexDefinitionBuilder` approaches are not recommended and should be used only if you can't do it by inheriting from `AbstractIndexCreationTask`. /}

{INFO:Side-by-Side}

Since RavenDB 4.0, **all** index updates are side-by-side by default. The new index will replace the existing one once it becomes non-stale. If you want to force an index to swap immediately, you can use the Studio for that.

{INFO/}

{PANEL/}

{PANEL:**Auto indexes**}

Auto-indexes are **created** when queries that do **not specify an index name** are executed and, after in-depth query analysis, **no matching AUTO index is found** on the server-side.

{NOTE The query optimizer doesn't take into account the static indexes when it determines what index should be used to handle a query. /}

### Naming Convention

Auto-indexes can be recognized by the `Auto/` prefix in their name. Their name also contains the name of a collection that was queried, and list of fields that were required to find valid query results.

For instance, issuing a query like this

{CODE-TABS}
{CODE-TAB:java:Java indexes_7@Indexes/Creating.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees
where FirstName = 'Robert' and LastName = 'King'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

will result in a creation of a index named `Auto/Employees/ByFirstNameAndLastName`.

### Auto Indexes and Indexing State

To reduce the server load, if auto-indexes are not queried for a certain amount of time defined in `Indexing.TimeToWaitBeforeMarkingAutoIndexAsIdleInMin` setting (30 minutes by default), then they will be marked as `Idle`. You can read more about the implications of marking index as `Idle` [here](../studio/database/indexes/indexes-list-view#index-state).

Setting this configuration option to a high value may result in performance degradation due to the possibility of having a high amount of unnecessary work that is all redundant and not needed by indexes to perform. This is _not_ a recommended configuration.

{PANEL/}

{PANEL: If Indexes Exhaust System Resources}

* Indexes process data assigned to them as the data changes. For example, if changes are made in the collection "Orders", the indexes that are 
  defined to handle queries on "Orders" will be triggered to update.
* These processes utilize machine resources.  
  If indexing drains system resources, it usually means that either they were defined in a way that [causes inefficient processing](../studio/database/indexes/indexing-performance#common-indexing-issues), 
  or that your [license](https://ravendb.net/buy), [cloud instance](../cloud/cloud-instances#a-production-cloud-cluster) 
  or hardware must be optimized to satisfy your usage needs.  

{PANEL/}


## Related Articles

### Indexes
- [What are Indexes](../indexes/what-are-indexes)
- [Indexing Basics](../indexes/indexing-basics)

### Querying
- [Basics](../indexes/querying/query-index)

### Studio
- [Indexes: Overview](../studio/database/indexes/indexes-overview#indexes-overview)
- [Studio Index List View](../studio/database/indexes/indexes-list-view)

---

### Inside RavenDB 
- [Working with Indexes](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/12-working-with-indexes)

---

### Code Walkthrough
- [Scroll to see various indexing guides](https://demo.ravendb.net/)
