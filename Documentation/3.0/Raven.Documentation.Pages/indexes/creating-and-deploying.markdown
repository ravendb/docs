# Creating and deploying indexes

**Indexes are used by server to satisfy queries**, whenever a user issues a query RavenDB will use an existing index if it matches query or create a new one if no match is found.

{INFO:Remember}

Indexes created by issuing a query are called `dynamic` or `Auto` indexes and can be easily identified, due to their name that starts with `Auto/` prefix.

Indexes created explicitly by user are called `static`.

{INFO/}

{PANEL:**Static indexes**}

There are a couple of ways to create `static index` and send it to server, we can use low-level [commands](../indexes/creating-and-deploying#using-commands) or create a [custom class](../indexes/creating-and-deploying#using-abstractindexcreationtask). There is also a possibility to [scan an assembly](../indexes/creating-and-deploying#using-assembly-scanner) and deploy all found indexes.

<hr />

### using AbstractIndexCreationTask

If you are interested in having a **strongly-typed syntax** during index creation, have an **ability to deploy indexes using assembly scanner** or **do not hard-code index names in every query**, then `AbstractIndexCreationTask` should be your choice. 

Drawback of this choice is that index names are auto-generated from type name and cannot be changed, but certain naming conventions can be followed to shape up index name (more about it later).

{NOTE We recommend creating and using indexes in this form due to its simplicity, many benefits and minor amount of disadvantages. /}

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

Underneath, the `IndexCreation` will call `Execute` methods for each of found indexes (and transformers).

{WARNING `IndexCreation.CreateIndexes` will also deploy all classes that inherit from `AbstractTransformerCreationTask` (more about it [here](../transformers/creating-and-deploying)). /}

#### Example

{CODE indexes_8@Indexes/Creating.cs /}

<hr />

### using Commands

Low-level `PutIndex` command from `DatabaseCommands` (which API reference can be found [here](../client-api/commands/indexes/put)) can be used also to send index to server.

The benefit of this approach is that you can choose the name as you feel fit and change various settings available in `IndexDefinition`, but loose the ability to deploy using assembly scanner. Also you will have to use string-based names of indexes when querying.

{CODE indexes_5@Indexes/Creating.cs /}

#### IndexDefinitionBuilder

`IndexDefinitionBuilder` is a very useful class that enables you to create `IndexDefinitions` using strongly-typed syntax with an access to low-level settings not available when `AbstractIndexCreationTask` approach is used.

{CODE indexes_6@Indexes/Creating.cs /}

#### Remarks

{SAFE If index exists on server and stored definition is the same as the one that was send, then it will not be overwritten, which implies that indexed data will not be deleted and indexation will not start from scratch. /}

{INFO Commands approach is not recommended and should be used only if needed. /}

{PANEL/}

{PANEL:**Auto indexes**}

Auto-indexes are created when queries that do not specify an index name are executed and (after in-depth query analysis) no matching index is found on server-side.

### Naming convention

As mentioned earlier auto-indexes can be recognized by `Auto/` prefix in name. Their name also contains name of a collection that was queried and list of fields that were required to find valid query results.

For instance, issuing query like this

{CODE indexes_7@Indexes/Creating.cs /}

will result in a creation of a index named `Auto/Employees/ByFirstNameAndLastName`.

### Disabling creation of Auto indexes

By default, `Raven/CreateAutoIndexesForAdHocQueriesIfNeeded` configuration option is set to `true`, which allows auto-index creation if needed. To disable it server or database-wide please refer to [this article](../server/configuration/configuration-options).

### Auto indexes and indexing prioritization

To reduce the server load, if auto-indexes are not queried for a certain amount of time defined in `Raven/TimeToWaitBeforeMarkingAutoIndexAsIdle` setting (1 hour by default), then they will be marked as `Idle`. You can read more about implications of marking index as `Idle` [here](../server/administration/index-administration#index-prioritization).

{PANEL/}

## Related articles

- [What are indexes?](../indexes/what-are-indexes)
- [Indexing basics](../indexes/indexing-basics)
- [Querying : Basics](../indexes/querying/basics)
