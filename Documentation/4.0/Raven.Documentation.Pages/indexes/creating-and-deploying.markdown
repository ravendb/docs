# Creating and deploying indexes

**Indexes are used by server to satisfy queries**, whenever a user issues a query RavenDB will use an existing index if it matches query or create a new one if no match is found.

{INFO:Remember}

Indexes created by issuing a query are called `dynamic` or `Auto` indexes and can be easily identified, due to their name that starts with `Auto/` prefix.

Indexes created explicitly by user are called `static`.

{INFO/}

{PANEL:**Static indexes**}

There are a couple of ways to create a `static index` and send it to server, we can use [maintenance operations](../indexes/creating-and-deploying#using-maintenance-operations) or create a [custom class](../indexes/creating-and-deploying#using-abstractindexcreationtask). There is also a possibility to [scan an assembly](../indexes/creating-and-deploying#using-assembly-scanner) and deploy all found indexes.

<hr />

### using AbstractIndexCreationTask

If you are interested in having a **strongly-typed syntax** during index creation, have an **ability to deploy indexes using assembly scanner**, **avoid hard-coding index names in every query**, then `AbstractIndexCreationTask` should be your choice. 

{NOTE We recommend creating and using indexes in this form due to its simplicity, many benefits and few disadvantages. /}

#### Naming conventions

Actually there is only one naming conventions: each `_` in class name will be translated to `/` in index name.

e.g.

In `Northwind` samples there is a index called `Orders/Totals`. To get such a index name, we need to create class called `Orders_Totals`.

{CODE indexes_1@Indexes/Creating.cs /}

#### Sending to server

There is not much use from an index if it is not deployed to server. In order to do so, we need to create instance of our class that inherits from `AbstractIndexCreationTask` and use one of the deployment methods: `Execute` or `ExecuteAsync` for asynchronous call.

{CODE indexes_2@Indexes/Creating.cs /}

{CODE indexes_3@Indexes/Creating.cs /}

{SAFE If index exists on server and stored definition is the same as the one that was send, then it will not be overwritten, which implies that indexed data will not be deleted and indexation will not start from scratch. /}

#### Using assembly scanner

All classes that inherit from `AbstractIndexCreationTask` can be deployed at once using one of `IndexCreation.CreateIndexes` method overloads.

{CODE indexes_4@Indexes/Creating.cs /}

Underneath, the `IndexCreation` will attempt to create all indexes in a single request. If it fails, then it will repeat the execution by calling `Execute` method one-by-one for each of found indexes in separate requests.

#### Example

{CODE indexes_8@Indexes/Creating.cs /}

<hr />

### using Maintenance Operations

`PutIndexesOperation` maintenance operation (which API reference can be found [here](../client-api/commands/indexes/put)) can be used also to send index(es) to server.

The benefit of this approach is that you can choose the name as you feel fit and change various settings available in `IndexDefinition`, but loose the ability to deploy using assembly scanner. Also you will have to use string-based names of indexes when querying.

{CODE indexes_5@Indexes/Creating.cs /}

#### IndexDefinitionBuilder

`IndexDefinitionBuilder` is a very useful class that enables you to create `IndexDefinitions` using strongly-typed syntax with an access to low-level settings not available when `AbstractIndexCreationTask` approach is used.

{CODE indexes_6@Indexes/Creating.cs /}

#### Remarks

{INFO Maintenance Operations or `IndexDefinitionBuilder` approaches are not recommended and should be used only if you can't do that by inheriting from `AbstractIndexCreationTask`. /}

{PANEL/}

{PANEL:**Auto indexes**}

Auto-indexes are **created** when queries that do **not specify an index name** are executed and (after in-depth query analysis) **no matching auto index is found** on server-side.

### Naming convention

As mentioned earlier auto-indexes can be recognized by `Auto/` prefix in name. Their name also contains name of a collection that was queried and list of fields that were required to find valid query results.

For instance, issuing query like this

{CODE-TABS}
{CODE-TAB:csharp:C# indexes_7@Indexes/Creating.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
FROM Employees
WHERE FirstName = 'Robert' AND LastName = 'King'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

will result in a creation of a index named `Auto/Employees/ByFirstNameAndLastName`.

### Auto indexes and indexing state

To reduce the server load, if auto-indexes are not queried for a certain amount of time defined in `Indexing.TimeToWaitBeforeMarkingAutoIndexAsIdleInMin` setting (30 minutes by default), then they will be marked as `Idle`. You can read more about implications of marking index as `Idle` [here](../server/administration/index-administration#index-prioritization).

Setting this configuration option to a high value may result in performance degradation due to possibility of having high amount of unnecessary work that all redundant (and not needed) indexes need to perform. This is _not_ a recommended configuration.

{PANEL/}

## Related articles

- [What are indexes?](../indexes/what-are-indexes)
- [Indexing : Basics](../indexes/indexing-basics)
- [Querying : Basics](../indexes/querying/basics)
