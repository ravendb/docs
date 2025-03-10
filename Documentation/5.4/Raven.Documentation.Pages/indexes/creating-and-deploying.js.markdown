# Creating and Deploying Indexes
---

{NOTE: }

* This article explains how to create indexes in RavenDB.  
  For a general overview of indexes, see [What are indexes](../indexes/what-are-indexes).

* You can either:
    * [create a Static-index](../indexes/creating-and-deploying#define-a-static-index) yourself, which involves **Defining** the index and **Deploying** it to the RavenDB server, or
    * let the RavenDB server [create an Auto-index](../indexes/creating-and-deploying#creating-auto-indexes) for you based on query patterns.

* Static-indexes can be created:
    * using the Client API, as outlined in this article, or
    * from the [Indexes list view](../studio/database/indexes/indexes-list-view) in the Studio.

---

* In this page:
    * [Static-indexes](../indexes/creating-and-deploying#static-indexes)
        * [Define a static-index](../indexes/creating-and-deploying#define-a-static-index)
        * [Deploy a static-index](../indexes/creating-and-deploying#deploy-a-static-index)
            * [Deploy single index](../indexes/creating-and-deploying#deploy-single-index)
            * [Deploy multiple indexes](../indexes/creating-and-deploying#deploy-multiple-indexes)
            * [Deploy syntax](../indexes/creating-and-deploying#deploy-syntax)
            * [Deployment behavior](../indexes/creating-and-deploying#deployment-behavior)
        * [Creating a static-index - Example](../indexes/creating-and-deploying#create-a-static-index---example)
        * [Creating a static-index - using an Operation](../indexes/creating-and-deploying#create-a-static-index---using-an-operation)
    * [Auto-indexes](../indexes/creating-and-deploying#auto-indexes)
        * [Creating auto-indexes](../indexes/creating-and-deploying#creating-auto-indexes)
        * [Disabling auto-indexes](../indexes/creating-and-deploying#disabling-auto-indexes)

{NOTE/}

<a id="static-indexes" />
{PANEL: Define a static-index}

{CONTENT-FRAME: }

##### Static-indexes
---

* Indexes that are explicitly **created by the user** are called `static` indexes.
* Static-indexes can perform calculations, data conversions, and other processes behind the scenes.  
  This reduces the workload at query time by offloading these costly operations to the indexing phase.
* To query with a static-index, you must explicitly specify the index in the query definition.  
  For more details, see [Querying an index](../indexes/querying/query-index).

{CONTENT-FRAME/}
{CONTENT-FRAME: }

##### Define a static-index using a custom class
---

* To define a static-index using a custom class, extend the `AbstractJavaScriptIndexCreationTask` class.
* This method is recommended over the [Creating an index using an operation](../indexes/creating-and-deploying#create-a-static-index---using-an-operation) method  
  for its simplified index definition, offering a straightforward way to define the index.
{CODE:nodejs indexes_1@indexes/creating.js /}
* A complete example of creating a static-index is provided [below](../indexes/creating-and-deploying#create-a-static-index---example).

{CONTENT-FRAME/}
{CONTENT-FRAME: }

##### Naming convention
---

* Static-index class names follow a single naming convention:  
  Each `_` in the class name is translated to `/` in the index name on the server.
* In the above example, the index class name is `Orders_ByTotal`.  
  The name of the index that will be generated on the server will be: `Orders/ByTotal`.

{CONTENT-FRAME/}
{CONTENT-FRAME: }

##### Customizing configuration
---

* You can set various [indexing configuration](../server/configuration/indexing-configuration) values within the index definition.
* Setting a configuration value within the index will override the matching indexing configuration values set at the server or database level.
{CODE:nodejs indexes_2@indexes/creating.js /}

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Deploy a static-index}

* To begin indexing data, the index must be deployed to the server.
* This section provides options for deploying indexes that inherit from `AbstractJavaScriptIndexCreationTask`.
* To create and deploy an index using the `IndexDefinition` class via `PutIndexesOperation`,  
  see [Creating a static-index - using an Operation](../indexes/creating-and-deploying#create-a-static-index---using-an-operation).

---

{CONTENT-FRAME: }

##### Deploy single index
---

* Use `execute()` or `executeIndex()` to deploy a single index.
* The following examples deploy index `Ordes/ByTotal` to the default database defined in your _DocumentStore_ object.
  See the [syntax](../indexes/creating-and-deploying#deploy-syntax) section below for all available overloads.

{CODE:nodejs indexes_3@indexes/creating.js /}
{CODE:nodejs indexes_4@indexes/creating.js /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

##### Deploy multiple indexes
---

* Use `executeIndexes()` or `IndexCreation.createIndexes()` to deploy multiple indexes.
* The `IndexCreation.createIndexes` method attempts to create all indexes in a single request.  
  If it fails, it will repeat the execution by calling the `execute` method for each index, one by one,  
  in separate requests.
* The following examples deploy indexes `Ordes/ByTotal` and `Employees/ByLastName` to the default database defined in your _DocumentStore_ object.  
  See the [syntax](../indexes/creating-and-deploying#deploy-syntax) section below for all available overloads.

{CODE:nodejs indexes_5@indexes/creating.js /}
{CODE:nodejs indexes_6@indexes/creating.js /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

##### Deploy syntax
---

{CODE:nodejs syntax@indexes/creating.js /}

| Parameter          | Type                  | Description                                                                                                       |
|--------------------|-----------------------|-------------------------------------------------------------------------------------------------------------------|
| **store**          | `object`              | Your document store object.                                                                                       |
| **conventions**    | `DocumentConventions` | The [Conventions](../client-api/configuration/conventions) used by the document store.                            |
| **database**       | `string`              | The target database to deploy the index to. If not specified, the default database set on the store will be used. |
| **index**          | `object`              | The index object to deploy.                                                                                       |
| **indexes**        | `object[]`            | A list of index objects to deploy.                                                                                |

{CONTENT-FRAME/}
{CONTENT-FRAME: }

##### Deployment behavior
---

{INFO: }

###### Deployment mode:
---

* When your database spans multiple nodes,  
  you can choose between **Rolling** index deployment or **Parallel** index deployment.
* Rolling deployment applies the index to one node at a time,  
  while Parallel deployment deploys the index on all nodes simultaneously.
* Learn more in [Rolling index deployment](../indexes/rolling-index-deployment).

{INFO/}

{SAFE: }

###### When the index you are deploying already exists on the server:
---

* **If the index definition is updated**:
    * RavenDB uses a side-by-side strategy for all index updates.
    * When an existing index definition is modified, RavenDB creates a new index with the updated definition.
      The new index will replace the existing index once it becomes non-stale.
    * If you want to swap the indexes immediately, you can do so through the Studio.  
      For more details, see [Side by side indexing](../studio/database/indexes/indexes-list-view#indexes-list-view---side-by-side-indexing).
* **If the index definition is unchanged**:
    * If the definition of the index being deployed is identical to the one on the server,  
      the existing index will not be overwritten.
    * The indexed data will remain intact, and the indexing process will not restart.

{SAFE/}
{CONTENT-FRAME/}
{PANEL/}

{PANEL: Create a static-index - Example}

{CODE:nodejs indexes_7@indexes/creating.js /}

{PANEL/}

{PANEL: Create a static-index - using an Operation}

* An index can also be defined and deployed using the [PutIndexesOperation](../client-api/operations/maintenance/indexes/put-indexes) maintenance operation.

* When using this operation:

    * Unlike the [naming convention](../indexes/creating-and-deploying#naming-convention) used with indexes inheriting from `AbstractJavaScriptIndexCreationTask`,  
      you can choose any string-based name for the index.   
      However, when querying, you must use that string-based name rather than the index class type.

    * You can also modify various low-level settings available in the [IndexDefinition](../client-api/operations/maintenance/indexes/put-indexes#put-indexes-operation-with-indexdefinition) class.

* Consider using this operation only if inheriting from `AbstractJavaScriptIndexCreationTask` is not an option.

* For a detailed explanation and examples, refer to the dedicated article: [Put Indexes Operation](../client-api/operations/maintenance/indexes/put-indexes).

{PANEL/}

<a id="auto-indexes" />
{PANEL: Creating auto-indexes}

{CONTENT-FRAME: }

##### Auto-indexes creation
---

* Indexes **created by the server** are called `dynamic` or `auto` indexes.
* Auto-indexes are created when all of the following conditions are met:
    * A query is issued without specifying an index (a dynamic query).
    * The query includes a filtering condition.
    * No suitable auto-index exists that can satisfy the query.
    * Creation of auto-indexes has not been disabled.
* For such queries, RavenDB's Query Optimizer searches for an existing auto-index that can satisfy the query.
  If no suitable auto-index is found, RavenDB will either create a new auto-index or optimize an existing auto-index.
  (Static-indexes are not taken into account when determining which auto-index should handle the query).
* Note: dynamic queries can be issued either when [querying](../studio/database/queries/query-view#query-view) or when [patching](../studio/database/documents/patch-view#patch-configuration).
* Over time, RavenDB automatically adjusts and merges auto-indexes to efficiently serve your queries.  
  For more details, see [Query a collection - with filtering (dynamic query)](../client-api/session/querying/how-to-query#dynamicQuery).

{CONTENT-FRAME/}
{CONTENT-FRAME: }

##### Naming convention
---

* Auto-indexes are easily identified by their names, which start with the `Auto/` prefix.
* Their name also includes the name of the queried collection and a list of fields used in the query predicate to filter matching results.
* For example, issuing the following query:
  {CODE-TABS}
  {CODE-TAB:nodejs:Query indexes_8@indexes/creating.js /}
  {CODE-TAB-BLOCK:sql:RQL}
from Employees
where FirstName = "Robert" and LastName = "King"
  {CODE-TAB-BLOCK/}
  {CODE-TABS/}
  will result in the creation of an auto-index named `Auto/Employees/ByFirstNameAndLastName`.

{CONTENT-FRAME/}
{CONTENT-FRAME: }

##### Auto-index idle state
---

* To reduce server load, an auto-index is marked as `idle` when it hasn't been used for a while.  
  Specifically, if the time difference between the last time the auto-index was queried
  and the last time a query was made on the database (using any index) exceeds the configured threshold (30 minutes by default),
  the auto-index will be marked as `idle`.
* This is done in order to avoid marking indexes as idle for databases that were offline for a long period of time,
  as well as for databases that were just restored from a snapshot or a backup.
* To set the time before marking an index as idle, use the
  [Indexing.TimeToWaitBeforeDeletingAutoIndexMarkedAsIdleInHrs](../server/configuration/indexing-configuration#indexing.timetowaitbeforedeletingautoindexmarkedasidleinhrs) configuration key.  
  Setting this value too high is not recommended, as it may lead to performance degradation by causing unnecessary and redundant work for the indexes.
* An `idle` auto-index will resume its work and return to `normal` state upon its next query,  
  or when resetting the index.
* If not resumed, the idle auto-index will be deleted by the server after the time period defined in the
  [Indexing.TimeToWaitBeforeDeletingAutoIndexMarkedAsIdleInHrs](../server/configuration/indexing-configuration#indexing.timetowaitbeforedeletingautoindexmarkedasidleinhrs) configuration key  
  (72 hours by default).

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Disabling auto-indexes}

**Why disable**:

* Disabling auto-index creation prevents the accidental deployment of resource-consuming auto-indexes that may result from one-time, ad-hoc queries issued from the Studio.
* In production environments, disabling this feature helps avoid the creation and background execution of expensive indexes.

**How to disable**:

* You can disable auto-indexes by setting the [Indexing.DisableQueryOptimizerGeneratedIndexes](../server/configuration/indexing-configuration#indexing.disablequeryoptimizergeneratedindexes) configuration key.
  This will affect all queries made both from the **Client API** and the **Studio**.

* Alternatively, you can disable auto-indexes from the Studio.  
  However, this will affect queries made only from the **Studio**.

    * To disable auto-index creation for a specific query made from the query view, see these [Query settings](../studio/database/queries/query-view#query-settings).
    * To disable auto-index creation for a specific query made from the patch view, see these [Patch settings](../studio/database/documents/patch-view#patch-settings).
    * Disabling auto-index creation for ALL queries made on a database can be configured in the [Studio configuration view](../studio/database/settings/studio-configuration#disabling-auto-index-creation-on-studio-queries-or-patches).

{PANEL/}

## Related Articles

### Indexes
- [What are Indexes](../indexes/what-are-indexes)
- [Indexing Basics](../indexes/indexing-basics)

### Operations
- [Put Indexes Operation](../client-api/operations/maintenance/indexes/put-indexes)

### Querying
- [Query Overview](../client-api/session/querying/how-to-query)
- [Querying an Index](../indexes/querying/query-index)

### Studio
- [Indexes: Overview](../studio/database/indexes/indexes-overview#indexes-overview)
- [Studio Index List View](../studio/database/indexes/indexes-list-view)

---

### Inside RavenDB
- [Working with Indexes](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/12-working-with-indexes)

---

### Code Walkthrough
- [See various indexing guides in the online DEMO](https://demo.ravendb.net/)
