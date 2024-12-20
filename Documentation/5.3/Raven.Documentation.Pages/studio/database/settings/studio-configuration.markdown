# Studio Configuration 
---

{NOTE: }

* The Studio Configuration view enables configuration of studio-related operations and interfaces.  

* Settings changed in this view do not change configurations in other scopes such as Client or Server API.
  * To disable the creation of Auto-Indexes in other scopes, set [Indexing.DisableQueryOptimizerGeneratedIndexes](../../../server/configuration/indexing-configuration#indexing.disablequeryoptimizergeneratedindexes) 
    to `true`:
     * Server-wide - set it in the server's [settings.json](../../../server/configuration/configuration-options#settings.json) file.
     * Per database - set it in the database's [Document Store](../../../client-api/operations/maintenance/configuration/database-settings-operation).

In this page:

[Studio Configuration - Single Database](../../../studio/database/settings/studio-configuration#studio-configuration---single-database)

* [Database Studio Configuration View](../../../studio/database/settings/studio-configuration#database-studio-configuration-view)
* [Disabling Auto-Index Creation on Studio Queries or Patches](../../../studio/database/settings/studio-configuration#disabling-auto-index-creation-on-studio-queries-or-patches)
  * [About Auto-Index Creation](../../../studio/database/settings/studio-configuration#about-auto-indexes-and-the-studio)
  * [If Disabled, Temporarily Enable Auto-Index Creation](../../../studio/database/settings/studio-configuration#if-disabled,-temporarily-enable-auto-index-creation)

[Studio Configuration - Server-wide](../../../studio/database/settings/studio-configuration#studio-configuration---server-wide)

* [Server Studio Configuration View](../../../studio/database/settings/studio-configuration#server-studio-configuration-view)

{NOTE/}

## Studio Configuration - Single Database

{PANEL: Database Studio Configuration View}

!["Studio Configuration View"](images/studio-configuration-view.png "Studio Configuration View")

{PANEL/}


{PANEL: Disabling Auto-Index Creation on Studio Queries or Patches}

* To prevent accidentally deploying resource-consuming Auto-Indexes as a result of one-time, ad-hoc queries,
  toggle **Disable auto-index-creation** to `true` in the Studio Configuration view.  
  The setting is configured to `false` by default.  

* This action is done per-database and will not affect other databases.

* If set to `true`, **Disable auto-index-creation** will prevent future queries or patches from creating Auto-Indexes.  
  * If there is no index to satisfy a query or patch, an exeption will be thrown stating that 
    `Creation of Auto Indexes was disabled and no Auto Index matching the given query was found.`
  * You can temporarily allow auto-index creation as a result of a query or patch in the Studio [query settings](../../../studio/database/queries/query-view#query-view) 
    and [patch settings](../../../studio/database/documents/patch-view#patch-configuration)


!["Disable Auto Index"](images/disabling-auto-index-studio.png "Disable Auto Index")

1. **Disable auto-index creation**  
   Toggle to disable all future auto-index creation on this database from the studio.  
   This will not affect other databases or Client/Server API configurations.  
2. **Save**  
   Click to save new configuration settings. 

{PANEL/}

{PANEL: About Auto-Indexes and the Studio}

**What are Auto-Indexes**  

* Auto-indexes are created when all of the following conditions are met:
  * A query is issued without specifying an index (a [dynamic query](../../../client-api/session/querying/how-to-query#dynamicQuery)).
  * The query includes a filtering condition. 
  * No suitable auto-index exists that can satisfy the query.
  * Creation of auto-indexes has not been disabled.

* For such queries, RavenDB's Query Optimizer searches for an existing auto-index that can satisfy the query.  
  If no suitable auto-index is found, RavenDB will either [create a new auto-index](../../../indexes/creating-and-deploying#auto-indexes) or optimize an existing auto-index.

* Note: dynamic queries can be issued either when [querying](../../../studio/database/queries/query-view#query-view) or when [patching](../../../studio/database/documents/patch-view#patch-configuration).

* Auto-indexes are dynamically maintained to change automatically in response to changing query demands.  
  After a certain amount of time that an auto-index is not used [(30 minutes by default)](../../../server/configuration/indexing-configuration#indexing.timetowaitbeforemarkingautoindexasidleinmin),  
  the index goes into an [idle state](../../../studio/database/indexes/indexes-list-view#index-state)
  and deleted after a set time-period [(72 hours by default)](../../../server/configuration/indexing-configuration#indexing.timetowaitbeforedeletingautoindexmarkedasidleinhrs).

* To provide for fast queries, indexes process information in the background.  
  If they are processing large datasets, each index can be demanding on I/O resources.

**Why disable Auto-Index in Studio queries or patches**  
Some people use the Studio for one-time, ad-hoc queries and don't want a new index to start using resources.  
In a playground database, it may be worth keeping auto-indexing active, even for random queries, because you want to be able to experiment. 
On the other hand, disabling it in production can prevent expensive indexes from being created and running in the background.

{PANEL/}


{PANEL: If Disabled, Temporarily Enable Auto-Index Creation}

If you disabled Auto-Indexing in the Studio Database Configuration page, and  
you want a one-time Auto-Index set up to satisfy a Query or Patch,  
temporarily allow Auto-Index in the Query or Patch settings interface.  

* [Query Settings](../../../studio/database/queries/query-view#query-settings)
* [Patch Settings](../../../studio/database/documents/patch-view#patch-settings)

Note: These settings only affect Auto-Indexing as a result of Queries or Patches done in the Studio.  
They do not affect API-based Queries or Patches.  
To configure API-based Auto-Indexing, see [Indexing.DisableQueryOptimizerGeneratedIndexes](../../../server/configuration/indexing-configuration#indexing.disablequeryoptimizergeneratedindexes)

{PANEL/}

## Studio Configuration - Server-wide

{PANEL: Server Studio Configuration View}

!["Server Studio Configuration View"](images/server-wide-studio-configuration.png "Server Studio Configuration View")

1. **Manage Server**  
   Select to see Studio server management options.
2. **Studio Configuration**  
   Select to configure Studio options related to the cluster.
3. **Environment**  
   Choose work environment.  This does not affect settings or features.  It makes it clear if, for example, you are in a production 
   server and should be much more careful than in a development environment.
4. **Default replication factor**  
   Change the default [replication factor](../../../studio/database/create-new-database/general-flow#3.-configure-replication) from 
   automatically replicating data to every node in the cluster.  
5. **Help us improve the Studio**  
   Allow the Studio to send usage statistics to inform our developers.  
6. **Collapse documents when opening**  
   Display documents as collapsed when opening them.  
7. **Save**  
   Click to save configurations.  

{PANEL/}

## Related Articles

### Indexes

- [Auto-Index (Dynamic Index)](../../../indexes/creating-and-deploying#auto-indexes)

### Studio 

- [Query settings](../../../studio/database/queries/query-view#query-view)
- [Patch settings](../../../studio/database/documents/patch-view#patch-configuration)
- [Replication factor](../../../studio/database/create-new-database/general-flow#3.-configure-replication)

### API

- [Indexing.DisableQueryOptimizerGeneratedIndexes](../../../server/configuration/indexing-configuration#indexing.disablequeryoptimizergeneratedindexes)  

---

### Code Walkthrough

- [Auto-Map Index I](https://demo.ravendb.net/demos/csharp/auto-indexes/auto-map-index1)
- [Auto-Map Index II](https://demo.ravendb.net/demos/csharp/auto-indexes/auto-map-index2)
- [Auto-Map-Reduce Index](https://demo.ravendb.net/demos/csharp/auto-indexes/auto-map-reduce-index)
