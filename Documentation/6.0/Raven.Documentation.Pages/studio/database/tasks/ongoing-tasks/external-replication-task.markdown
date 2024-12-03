# External Replication Task
---

{NOTE: }

* Schedule an **External Replication Task** in order to have a _live_ replica of your data in another database:  
  * In the same cluster if you want a live copy that won't be a client failover target.
  * In a separate RavenDB cluster [on local machines](../../../../start/getting-started) or [a cloud instance](../../../../cloud/cloud-overview),  
    which can be used as a failover if the source cluster is down.

* "Live" means that the replica is up to date at all times.  
  Any changes in the source database will be reflected in the replica once they occur.  

* This ongoing task replicates **one-way**, from the source to the destination. For additional functionality,  
  such as filtration and two-way (master-master) replication, consider [Hub/Sink Replication](../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview).  

* To replicate between two separate, secure RavenDB servers,  
  you need to [pass a client certificate](../../../../studio/database/tasks/ongoing-tasks/external-replication-task#step-by-step-guide) from the source server to the destination.

* External replication can be a comfortable means of data migration into a [sharded](../../../../sharding/overview) database.  
  You can read more about this option in the sharding documentation [external replication](../../../../sharding/external-replication) and [migration](../../../../sharding/migration) sections.  

* The External Replication task **does _not_ create a backup** of your data and indexes.  
  See more in [Backup -vs- Replication](../../studio/database/tasks/backup-task#backup-task--vs--replication-task)

* In this page:  
  * [General Information about External Replication Task](../../../../studio/database/tasks/ongoing-tasks/external-replication-task#general-information-about-external-replication-task)
  * [Step-by-Step Guide](../../../../studio/database/tasks/ongoing-tasks/external-replication-task#step-by-step-guide)
  * [Definition](../../../../studio/database/tasks/ongoing-tasks/external-replication-task#definition)  
  * [Details in Tasks List View](../../../../studio/database/tasks/ongoing-tasks/external-replication-task#details-in-tasks-list-view)  
  * [Offline Behavior](../../../../studio/database/tasks/ongoing-tasks/external-replication-task#offline-behavior)

{NOTE/}

---

{PANEL: General Information about External Replication Task}

**What is being replicated:**  

  * All database documents and related data:  
    * [Attachments](../../../../document-extensions/attachments/what-are-attachments)  
    * [Revisions](../../../../document-extensions/revisions/overview)  
    * [Counters](../../../../document-extensions/counters/overview)
    * [Time Series](../../../../document-extensions/timeseries/overview)

---

**What is _not_ being replicated:**  

  * Server and cluster level features:  
    * [Indexes](../../../../indexes/creating-and-deploying)  
    * [Conflict resolver definitions](../../../../server/clustering/replication/replication-conflicts#conflict-resolution-script)  
    * [Compare-Exchange](../../../../client-api/operations/compare-exchange/overview)
    * [Subscriptions](../../../../client-api/data-subscriptions/what-are-data-subscriptions)
    * [Identities](../../../../server/kb/document-identifier-generation#strategy--3)  
    * Ongoing tasks
      * [ETL](../../../../server/ongoing-tasks/etl/basics)
      * [Backup](../../../../studio/database/tasks/backup-task)
      * [Hub/Sink Replication](../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview)

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
    [conflicts](../../../../server/clustering/replication/replication-conflicts) according to each database conflict resolution policy.  
  * It is recommended to have the same [conflict resolution policy configuration](../../../../server/clustering/replication/replication-conflicts#configuring-conflict-resolution-using-the-client) on both the source and the target databases.  

{PANEL/}

{PANEL: Step-by-Step Guide}

1. **Pass Certificate from Source Server to Destination Server**  
  This step must be done if replicating *to a separate secure cluster* so that the destination cluster trusts the source.  
  * **Via RavenDB Studio:**  
    Navigate from the "Manage Server" tab (left side) > "Certificates" to open the [Certificate Management](../../../../server/security/authentication/certificate-management) view.  
     - Learn how to [pass certificates here](../../../../server/security/authentication/certificate-management#enabling-communication-between-servers:-importing-and-exporting-certificates).  
  * **Via API:**  
    See the code sample to learn how to [define a client certificate in the DocumentStore()](../../../../client-api/creating-document-store).  
     * To generate and configure a client certificate from the source server:  
       **Via code**, see [CreateClientCertificateOperation](../../../../client-api/operations/server-wide/certificates/create-client-certificate).  
       **Via RavenDB CLI** PowerShell command, see [Client Certificate Usage](../../../../server/security/authentication/client-certificate-usage)  
     * Learn the rationale needed to configure client certificates in [The RavenDB Security Authorization Approach](../../../../server/security/authentication/certificate-management#the-ravendb-security-authorization-approach)
2. **Create Target Database in Destination Server**  
  * Consider [creating an empty target database](../../../../studio/database/create-new-database/general-flow) 
    because data transfer can overwrite existing data.  
  * If the source database is [encrypted](../../../../studio/database/create-new-database/encrypted#creating-encrypted-database), 
    be sure that the target database is as well.  
     ![Encrypted Logo](images/encrypted-logo.png "Encrypted Logo")
3. **Define External Replication Task in the Source Database**  
    Learn more about [defining the task](../../../../studio/database/tasks/ongoing-tasks/external-replication-task#definition) in the dedicated section.  
  * **Optional Parameters**  
     - Task Name  
     - Delay Time  
     - Preferred Node  
  * **Required Parameters**  
     - Connection String  
  * **Save**  
     - Click "Save" to activate the External Replication task.  
     - Check the target database to see if data has been transferred. This can take at least about 20-30 seconds, depending on the dataset size.  
     - If the data did not transfer properly, check the notifications (top-right of Studio) 
       in the responsible node to see if there were any errors.  



{PANEL/}

{PANEL: Definition}

To access the External Replication Task Studio interface:  

a. Open the **Databases** view in the source server.  
b. Select the **source** database.  
c. Click **Tasks** tab.  
d. Select **Ongoing Tasks**  
e. Click **Add a database task**  
f. Click **External Replication** to access the following interface.  

![Figure 1. External Replication Task Definition](images/external-replication-1.png "Create New External Replication Task")

1. **Source Database**  
   Be sure that you are defining the task from the correct source database.  

2. **Task Name** (Optional)  
   * Choose a name of your choice  
   * If no name is given then the RavenDB server will create one for you based on the defined connection string  

3. **Set Replication Delay Time** (Optional)  
   * If a delay time is set then data will be replicated only after this time period has passed for each data change.  
   * Having a delayed instance of a database allows you to "go back in time" and undo contamination to your data due to an attack, faulty patch script, or other human errors.  
     * This doesn't replace the need to [safely backup your databases](../../../../studio/database/tasks/backup-task), but it does provide a way to stay online while repairing.  

4. **Set Preferred Responsible Node** (Optional)  
  * Select a preferred mentor node from the [Database Group](../../../../studio/database/settings/manage-database-group) to be the responsible node for this External Replication Task  
  * If not selected, then the cluster will assign a responsible node (see [Members Duties](../../../../studio/database/settings/manage-database-group#database-group-topology---members-duties))  

5. **Create a new RavenDB connection string**  
  * Select a connection string from the pre-defined list -or- create a new connection string to be used.  
  * The connection string defines the external database and its server URL to replicate to.  
    ![External Replication: Connection String](images/external-replication-connection-string.png "External Replication: Connection String")
      1. **Name**  
        Give the connection string a meaningful name.  
      2. **Database**  
        Copy the exact name of the destination database.  
          * If the source database is encrypted, make sure that the destination is encrypted as well.
      3. **Discovery URL**  
        Copy the URL from the destination server here.
         ![External Replication: URL](images/external-replication-url.png "External Replication: URL")
           * Be sure to copy only the server URL - without extraneous details.  
      4. **Save**  
         Click "Save" to activate the External Replication task.

    {NOTE: If the destination database is in a cluster}
    You can set multiple connection strings to multiple different nodes on different machines 
    so that if one is down, the other can keep the destination updated.  
    See [Offline Behavior - When the destination node is down](../../../../studio/database/tasks/ongoing-tasks/external-replication-task#offline-behavior).  
    If you define only one node's connection string, RavenDB will wait until that node is online and will then update any missing information.  
    {NOTE/}

{PANEL/}

{PANEL: Details in Tasks List View}

![Figure 2. External Replication Task - Task List View](images/external-replication-2.png "Tasks List View Details")

1. **External Replication Task Details**:
   *  **Task Status** - Active / Not Active / Not on Node / Reconnect  
   *  **Connection String** - The connection string used  
   *  **Destination Database** - The external database to which the data is being replicated  
   *  **Actual Destination URL** - The server URL to which the data is actually being replicated,  
      the one that is currently used out of the available _Topology Discovery URLs_  
   *  **Topology Discovery URLs** - List of the available destination Database Group servers URLs  

2. **Database Group Topology**:  
   Visual representation showing the responsible node for the External Replication Task  
{PANEL/}

{PANEL: Offline Behavior}

* **When the source cluster is down** (and there is no leader):  

  * Creating a _new_ Ongoing Task is a Cluster-Wide operation,  
    thus, a new Ongoing External Replication Task ***cannot*** be scheduled.  

  * If an External Replication Task was _already_ defined and active when the cluster went down,  
    then the task will _not_ be active, no replication will take place.

* **When the node responsible for the external replication task is down**  

  * If the responsible node for the External Replication Task is down,  
    then another node from the Database Group will take ownership of the task so that the external replica is up to date.  

* **When the destination node is down:**  

  * The external replication will wait until the destination is reachable again and proceed from where it left off.  

  * If there is a cluster on the other side, and the URL addresses of the destination database group nodes are listed in the connection string, 
    then when the destination node is down, the replication task will simply start transferring data to one of the other nodes specified.  
{PANEL/}



## Related Articles

### Studio Articles
- [Hub/Sink Replication: Overview](../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview)  
- [Replication Hub Task](../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-hub-task)  
- [Replication Sink Task](../../../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-sink-task)  

### Server Articles**
- [Replication Conflicts](../../../../server/clustering/replication/replication-conflicts#configuring-conflict-resolution-using-the-client)
- [Certificates Management](../../../../server/security/authentication/certificate-management#enabling-communication-between-servers:-importing-and-exporting-certificates)  
- [External Replication](../../../../server/ongoing-tasks/external-replication)  
- [Client Certificate Usage](../../../../server/security/authentication/client-certificate-usage)  

### Client API Articles
- [Adding a Connection String](../../../../client-api/operations/maintenance/connection-strings/add-connection-string#operations-how-to-add-a-connection-string)  

### Sharding
- [Sharding Overview](../../../../sharding/overview)  
- [Sharding: Export](../../../../sharding/import-and-export#export)  
- [Sharding: Import](../../../../sharding/import-and-export#import)  
