# Indexes: Creating and Deploying Indexes

**Indexes are used by the server to satisfy queries.** Whenever a user issues a query, RavenDB will use an existing index if it matches the query. If it doesn't, RavenDB will create a new one.

{INFO:Remember}

Indexes created by issuing a query are called `dynamic` or `Auto` indexes. They can be easily identified. Their name starts with `Auto/` prefix.

Indexes created explicitly by the user are called `static`.

{INFO/}

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

#### Example

{CODE:java indexes_8@Indexes/Creating.java /}

<hr />

### Using Maintenance Operations

The `PutIndexesOperation` maintenance operation (which API references can be found [here](../client-api/operations/maintenance/indexes/put-indexes)) can be used also to send index(es) to the server.

The benefit of this approach is that you can choose the name as you feel fit, and change various settings available in `IndexDefinition`. You will have to use string-based names of indexes when querying.

{CODE:java indexes_5@Indexes/Creating.java /}

#### Remarks

{INFO:Side-by-Side}

Since RavenDB 4.0, **all** index updates are side-by-side by default. The new index will replace the existing one once it becomes non-stale. If you want to force an index to swap immediately, you can use the Studio for that.

{INFO/}

{PANEL/}

{PANEL:**Auto indexes**}

Auto-indexes are **created** when queries that do **not specify an index name** are executed and, after in-depth query analysis, **no matching AUTO index is found** on the server-side.

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

To reduce the server load, if auto-indexes are not queried for a certain amount of time defined in `Indexing.TimeToWaitBeforeMarkingAutoIndexAsIdleInMin` setting (30 minutes by default), then they will be marked as `Idle`. You can read more about the implications of marking index as `Idle` [here](../server/administration/index-administration#index-prioritization).

Setting this configuration option to a high value may result in performance degradation due to the possibility of having a high amount of unnecessary work that is all redundant and not needed by indexes to perform. This is _not_ a recommended configuration.

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../indexes/what-are-indexes)
- [Indexing Basics](../indexes/indexing-basics)

### Querying

- [Basics](../indexes/querying/basics)

### Studio

- [Indexes Overview](../studio/database/indexes/indexes-overview#indexes-overview)
- [Studio Indexes List View](../studio/database/indexes/indexes-list-view)
