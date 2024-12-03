# Server: Ongoing Tasks: External Replication
---

{NOTE: }

* Schedule an **External Replication Task** to have a _live_ replica of your data in another database:
    * In a separate RavenDB cluster [on local machines](../../start/getting-started) or [a cloud instance](../../cloud/cloud-overview),  
      which can be used as a failover if the source cluster is down.
    * In the same cluster if you want a live copy that won't be a client failover target.

* "Live" means that the replica is up to date at all times.  
  Any changes in the source database will be reflected in the replica once they occur.

* This ongoing task replicates **one-way**, from the source to the destination.  
  For additional functionality such as filtration and two-way replication consider [Hub/Sink Replication](../../server/ongoing-tasks/hub-sink-replication).

* To replicate between two separate, secure RavenDB servers,  
  you need to [pass a client certificate](../../server/ongoing-tasks/external-replication#step-by-step-guide) from the source server to the destination.

* The External Replication task **does _not_ create a backup** of your data and indexes.  
  See more in [Backup -vs- Replication](../../studio/database/tasks/backup-task#backup-task--vs--replication-task)

---

* In this page: 
  * [General Information about External Replication Task](../../server/ongoing-tasks/external-replication#general-information-about-external-replication-task)
  * [Code Sample](../../server/ongoing-tasks/external-replication#code-sample)
  * [Step-by-Step Guide](../../server/ongoing-tasks/external-replication#step-by-step-guide)
  * [Definition](../../server/ongoing-tasks/external-replication#definition)  
  * [Offline Behavior](../../server/ongoing-tasks/external-replication#offline-behavior)
  * [Delayed Replication](../../server/ongoing-tasks/external-replication#delayed-replication)

{NOTE/}

{PANEL: General Information about External Replication Task}

**What is being replicated:**  

 * All database documents and related data:  
   * [Attachments](../../document-extensions/attachments/what-are-attachments)  
   * [Revisions](../../document-extensions/revisions/overview)  
   * [Counters](../../document-extensions/counters/overview)
   * [Time Series](../../document-extensions/timeseries/overview)

---

**What is _not_ being replicated:**  

  * Server and cluster level features:  
    * [Indexes](../../indexes/creating-and-deploying)  
    * [Conflict resolution scripts](../../server/clustering/replication/replication-conflicts#conflict-resolution-script)  
    * [Compare-Exchange](../../client-api/operations/compare-exchange/overview)
    * [Subscriptions](../../client-api/data-subscriptions/what-are-data-subscriptions)
    * [Identities](../../server/kb/document-identifier-generation#strategy--3)  
    * Ongoing tasks
      * [ETL](../../server/ongoing-tasks/etl/basics)
      * [Backup](../../studio/database/tasks/backup-task)
      * [Hub/Sink Replication](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview)

        {NOTE: }

        **Why are cluster-level features not replicated?**

        RavenDB is designed with a cluster-level data ownership model to prevent conflicts between clusters,  
        especially in scenarios where ACID transactions are critical.

        This approach ensures that certain features, such as policies, configurations, and ongoing tasks,  
        remain specific to each cluster, avoiding potential inconsistencies.

        To explore this concept further, refer to the [Data Ownership in a Distributed System](https://ayende.com/blog/196769-B/data-ownership-in-a-distributed-system) blog post.

        {NOTE/}

---

**Conflicts:**  

  * Two databases that have an External Replication task defined between them will detect and resolve document 
    [conflicts](../../server/clustering/replication/replication-conflicts) according to each database's conflict resolution policy.  
  * It is recommended to have the same [policy configuration](../../server/clustering/replication/replication-conflicts#configuring-conflict-resolution-using-the-client) 
    on both the source and the target databases.  

{PANEL/}

{PANEL: Code Sample}

The required elements of an External Replication task are:

* The `UpdateExternalReplicationOperation()` method.
* The destination server needs the [certificate from the source server](../../server/security/authentication/certificate-management#enabling-communication-between-servers:-importing-and-exporting-certificates) 
  so that it will trust the source.
* The [connection string](../../client-api/operations/maintenance/connection-strings/add-connection-string#add-a-raven-connection-string) 
  with the destination server URL and any other details needed to access the destination server.
* The following properties in the `ExternalReplication` object:
  * **ConnectionStringName**  
    The connection string name.  
  * **Name**  
    The External Replication task name.

{CODE ExternalReplication@Server\OngoingTasks\ExternalReplicationSamples.cs /}

Optional elements include the following properties in the `ExternalReplication` object:

* **MentorNode**  
  The preferred responsible node in the source server.
* **DelayReplicationFor**  
  The amount of time to delay replication.  
  The following sample shows a 30-minute delay.  Delay can also be set by days, hours, and seconds.  

{CODE ExternalReplicationWithMentorAndDelay@Server\OngoingTasks\ExternalReplicationSamples.cs /}

`ExternalReplication` properties:

{CODE ExternalReplicationProperties@Server\OngoingTasks\ExternalReplicationSamples.cs /}

{PANEL/}


{PANEL: Step-by-Step Guide}

To create an external replication task via the RavenDB Studio, see the [Step-by-Step Guide](../../studio/database/tasks/ongoing-tasks/external-replication-task#step-by-step-guide)

{PANEL/}

{PANEL: Definition}

To learn how to define an external replication task via code, see [code sample](../../server/ongoing-tasks/external-replication#code-sample).  

You can also configure external eplication tasks [via RavenDB Studio](../../studio/database/tasks/ongoing-tasks/external-replication-task#definition).  

{PANEL/}


{PANEL: Offline Behavior}

* **When the source cluster is down** (and there is no leader):  

  * Creating a _new_ Ongoing Task is a Cluster-Wide operation,  
    thus, a new Ongoing External Replication Task ***cannot*** be scheduled.  

  * If an External Replication Task was _already_ defined and active when the cluster went down,  
    then the task will _not_ be active and no replication will take place.

* **When the node responsible for the external replication task is down**  

  * If the responsible node for the External Replication Task is down,  
    then another node from the Database Group will take ownership of the task so that the external replica is up to date.  

* **When the destination node is down:**  

  * The external replication will wait until the destination is reachable again and proceed from where it left off.  

  * If there is a cluster on the other side, and the URL addresses of the destination database group nodes are listed in the connection string, 
    then when the destination node is down, the replication task will simply start transferring data to one of the other nodes specified.  
{PANEL/}


## Delayed Replication

In RavenDB we introduced a new kind of replication, _delayed replication_. It replicates data that is 
delayed by `X` amount of time. 
_Delayed replication_ works just like normal replication but instead of sending data immediately, 
it waits `X` amount of time. 
Having a delayed instance of a database allows you to "go back in time" and undo contamination to your data 
due to a faulty patch script or other human errors.  
While you can and should always use backup for those cases, having a live database makes it quick to failover 
to prevent business losses while you repair the faulty databases.  

* To set delayed replication, see "3. **Set Replication Delay Time**" in the [definition instructions](../../studio/database/tasks/ongoing-tasks/external-replication-task#definition).  

## Related articles

### Replication

**Studio Articles**:  

- [External Replication](../../studio/database/tasks/ongoing-tasks/external-replication-task)  
- [Hub/Sink Replication: Overview](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview)  
- [Replication Hub Task](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-hub-task)  
- [Replication Sink Task](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-sink-task)  

**Server Articles**:  

- [How Replication Works](../../server/clustering/replication/replication)  
- [Replication Conflicts](../../server/clustering/replication/replication-conflicts#configuring-conflict-resolution-using-the-client)  
- [Certificates Management](../../server/security/authentication/certificate-management#enabling-communication-between-servers:-importing-and-exporting-certificates)  
- [Hub/Sink Replication](../../server/ongoing-tasks/hub-sink-replication)  
- [Client Certificate Usage](../../server/security/authentication/client-certificate-usage)  

**Client API Articles**:  

- [Adding a Connection String](../../client-api/operations/maintenance/connection-strings/add-connection-string#operations-how-to-add-a-connection-string)  


