# Counters Interoperability
---

{NOTE: }

* This section describes the relationships between Counters and other RavenDB features:  
   * How Counters are supported by the different features.  
   * How Counters trigger features' execution.  

* In this page:  
  * [Counters and Indexing](../../../client-api/session/counters/interoperability#counters-and-indexing)
  * [Counters and Queries](../../../client-api/session/counters/interoperability#counters-and-queries)
  * [Counters and Revisions](../../../client-api/session/counters/interoperability#counters-and-revisions)
  * [Counters and Smuggler](../../../client-api/session/counters/interoperability#counters-and-smuggler)
  * [Counters and Changes API](../../../client-api/session/counters/interoperability#counters-and-changes-api)
  * [Counters and Ongoing Tasks (Backup, External replication, ETL, Subscriptions)](../../../client-api/session/counters/interoperability#counters-and-ongoing-tasks)
  * [Interoperability Summary: Triggers and Values](../../../client-api/session/counters/interoperability#interoperability-summary-triggers-and-values)
  * [Including Counters](../../../client-api/session/counters/interoperability#including-counters)
{NOTE/}

---

{PANEL: }

### Counters and Indexing  
 * Counter **Names** [can be indexed](../../../indexes/indexing-counters#indexes--indexing-counters).  
 * Counter **Values** are **not** indexed.  
   RavenDB refrains from indexing Counter results, to keep indexing as light a task as possible even when Counter values are modified extensively.  
 * **Trigger**: Document Change  
   * Document changes (when creating or deleting a Counter) initiate re-indexing.  
   * Modifying a Counter's value does **not** initiate re-indexing.  

---

###Counters and Queries  
You can create queries using your code, or send the server raw queries for execution.  
In both cases, documents can be queried by Counter names, but **not** be filtered by Counter values.  
This is because queries are generally [based on indexes](../../../start/getting-started#example-iii---querying), and Counter values are [not indexed](../../../client-api/session/counters/interoperability#counters-and-indexing).  

* You can use [Session.Query](../../../client-api/session/querying/how-to-query#session.query) to code queries yourself.  
        
  * **Returned Counter Value**: Accumulated.  
    A Counter's value is returned as a single sum, with no specification of the Counter's value on each node.  
    * Code sample:
      {CODE counters_region_query@ClientApi\Session\Counters\Counters.cs /}  
* You can use [RawQuery](../../../client-api/session/querying/how-to-query#session.advanced.rawquery) to send the server raw RQL expressions for execution.  
    
  * Use the `counter` method.  
      * **Returned Counter Value**: Accumulated.  
      * Code sample:
        {CODE counters_region_rawqueries_counter@ClientApi\Session\Counters\Counters.cs /}  
  * Use the `counterRaw` method.  
      * **Returned Counter Value**: Distributed.  
          A Counter's value is returned as a series of values, each maintained by one of the nodes.  
      * Code sample:
        {CODE counters_region_rawqueries_counterRaw@ClientApi\Session\Counters\Counters.cs /}

---

###Counters and Revisions  

 * A [document revision](../../../client-api/session/revisions/what-are-revisions) includes all the document's Counter names and values.  
 * **Trigger**: Document Change  
    * When Revisions are enabled, the creation or deletion of a Counter initiates the creation of a new document revision.  
    * Counter **value** modifications do **not** initiate the creation of new revisions.  
 * **Counter Value**: Accumulated.  
     A revision stores a Counter's value as a single sum, with no specification of the Counter's value on each node.  

---

###Counters and Smuggler  
 * [Smuggler](../../../client-api/smuggler/what-is-smuggler) is a DocumentStore property, that you can use to [export](../../../client-api/smuggler/what-is-smuggler#databasesmugglerexportoptions) chosen database item types to an external file, or [import](../../../client-api/smuggler/what-is-smuggler#databasesmugglerimportoptions) database item types from an existing file into the database.  
 * To make Smuggler handle Counters, set the `OperateOnTypes` parameter to `DatabaseItemType.Counters`.  
 * **Counter Value**: Distributed  
     * Smuggler transfers the entire series of values that the different nodes maintain for a Counter.  

---

###Counters and Changes API
* [Changes API](../../../client-api/changes/what-is-changes-api#changes-api) is a Push service, that can inform you of various changes on the Server.  
  Changes API **can** detect [changes in Counter values](../../../client-api/changes/how-to-subscribe-to-counter-changes#changes-api--how-to-subscribe-to-counter-changes).  
  You can target all Counters, or specify the ones you wish to follow.  
  * **Trigger**: Counter Value Modification  
  * **Counter Value**: Accumulated  
      `Changes API` methods return a Counter's value as a single sum, without specifying its value on each node.  

---

###Counters and Ongoing Tasks:
[Ongoing tasks](../../../studio/database/tasks/ongoing-tasks/general-info) may be triggered to act by a Counter name or value modification, or scheduled for routine execution.  

* **Counters and the Backup task**  
    There are two [backup](../../../studio/database/tasks/ongoing-tasks/backup-task) types: "Snapshot" and "Backup".  
    Both types store and restore **all** data automatically, including Counters.  
    * A "Snapshot" stores all data and settings as a single image.  
      All components are restored to the exact same state they've been at during backup.  
    * A "Backup" stores all data in a JSON format.  
      * **Trigger**: Time Schedule  
          Invoked by a pre-set time routine.  
      * **Counter Value**: Distributed  
        Backup is an upper-level implementation of [Smuggler](../../../client-api/session/counters/interoperability#counters-and-smuggler).  
        As with Smuggler, Counters are backed-up and restored including their values on all nodes.  

* **Counters and the External Replication task**  
    The ongoing [external replication](../../../studio/database/tasks/ongoing-tasks/external-replication-task) task replicates all data automatically, including Counters.  
    * **Triggers**:
        * Document Change  
        * Counter Value Modification  
    * **Counter Value**: Distributed  
        Counters are replicated along with their values on all nodes.  

* **Counters and the ETL task**  
    [ETL](../../../server/ongoing-tasks/etl/basics) is used in order to export data from a Raven database to an external (either Raven or SQL) database.  
    * [SQL ETL](../../../server/ongoing-tasks/etl/sql) - **Not supported**  
      Counters are not exported to SQL databases over SQL ETL.  
    * [RavenDB ETL](../../../server/ongoing-tasks/etl/raven) - **Supported**  
      Counters [are](../../../server/ongoing-tasks/etl/raven#counters) exported over RavenDB ETL.  
      When an ETL script is not provided, Counters are exported by default.  
      * **Triggers**:
          * Document Change  
          * Counter Value Modification  
      * **Counter Value**: Distributed  
          Counters are exported along with their values on all nodes.  

* **Counters and the [Data Subscriptions](../../../client-api/data-subscriptions/what-are-data-subscriptions#data-subscriptions) task**  
    * **Trigger**:
      * Document Change  
        Data Subscriptions are **not** triggered by Counter Value modifications.  


{NOTE: }
###Interoperability Summary: Triggers and Values

In the table below, find a summary of - 

* "Triggered By" - Counters-related Triggers that may invoke different features.  
    * "**Document Change**" - Features triggered by changes to Counter Names.  
    * "**Counter Value**" - Features triggered by changes to Counter Values.  
    * "**Time Schedule**" - Features invoked by a pre-set time routine.  
    * "**No Trigger**" - Executed manually, through the Studio or by a Client.  
* "Counter Value" - The way features handle Counter values.  
    * "**Accumulated**" - Counter value is handled as a single accumulated sum.  
    * "**Distributed**" - Counter value is handled as a series of values maintained by cluster nodes.  

| Feature | Triggered by | Counter Value |
|-------------|:-------------|:-------------|
| [Indexing](../../../client-api/session/counters/interoperability#counters-and-indexing) | Document Change | doesn't handle values |
| [LINQ Queries](../../../client-api/session/counters/interoperability#counters-and-queries) | No trigger | Accumulated |
| [Raw Queries](../../../client-api/session/counters/interoperability#counters-and-queries) | No trigger | `counter()` - Accumulated, <br> `counterRaw()` - Distributed |
| [Smuggler](../../../client-api/session/counters/interoperability#counters-and-smuggler) | No trigger | Distributed |
| [Backup Task](../../../client-api/session/counters/interoperability#counters-and-ongoing-tasks) | Time Schedule | Distributed |
| [RavenDB ETL Task](../../../client-api/session/counters/interoperability#counters-and-ongoing-tasks) | Document Change, <br> Countrer Value Modification | Distributed |
| [External Replication task](../../../client-api/session/counters/interoperability#counters-and-ongoing-tasks) | Document Change, <br> Countrer Value Modification | Distributed |
| [Subscription update Task](../../../client-api/session/counters/interoperability#counters-and-ongoing-tasks) | Document Change | |
| [Changes API](../../../client-api/session/counters/interoperability#counters-and-ongoing-tasks) | Countrer Value Modification | Accumulated |
| [Revision creation](../../../client-api/session/counters/interoperability#counters-and-revisions) | Document Change | Accumulated |

{NOTE/}

---

###Including Counters  
You can [include](../../../client-api/how-to/handle-document-relationships#includes) Counters while loading a document.  
An included Counter is cached, so it can be immediately retrieved when needed.  

* **Including Counters when using [Session.Load](../../../client-api/session/loading-entities#session--loading-entities)**:  
    * Include a single Counter using `IncludeCounter` -
      {CODE counters_region_load_include1@ClientApi\Session\Counters\Counters.cs /}
    * Include multiple Counters using `IncludeCounters` -
      {CODE counters_region_load_include2@ClientApi\Session\Counters\Counters.cs /}
* **Including Counters when using [Session.Query](../../../client-api/session/querying/how-to-query#session--querying--how-to-query)**:  
    * Include a single Counter using `IncludeCounter` -
      {CODE counters_region_query_include_single_Counter@ClientApi\Session\Counters\Counters.cs /}
    * Include multiple Counters using `IncludeCounters` -
      {CODE counters_region_query_include_multiple_Counters@ClientApi\Session\Counters\Counters.cs /}


{PANEL/}

## Related articles
### Studio
- [Studio Counters Management](../../../studio/database/documents/document-view/additional-features/counters#counters)  

###Client-API - Session
- [Counters Overview](../../../client-api/session/counters/overview)
- [Create or Modify Counter](../../../client-api/session/counters/create-or-modify)
- [Delete Counter](../../../client-api/session/counters/delete)
- [Retrieve Counter Values](../../../client-api/session/counters/retrieve-counter-values)
- [Counters in a Cluster](../../../client-api/session/counters/counters-in-a-cluster)

###Client-API - Operations
- [Counters Operations](../../../client-api/operations/counters/get-counters#operations--counters--how-to-get-counters)
