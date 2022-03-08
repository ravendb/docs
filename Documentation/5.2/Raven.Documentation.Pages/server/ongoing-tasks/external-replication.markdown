# Server: Ongoing Tasks: External Replication
---

{NOTE: }

* A live replication between two different database groups is called _external replication_.  

* This ongoing task replicates one-way - from the source to the destination.

* For additional functionality, such as filtration, two-way replication, and flexible connectivity, consider [Hub/Sink Replication](../../server/ongoing-tasks/hub-sink-replication).  

* To replicate between two separate RavenDB servers, you need to [pass a client certificate](../../server/ongoing-tasks/external-replication#step-by-step-guide) from the source server to the destination.

In this page: 

* [General Information about External Replication Task](../../server/ongoing-tasks/external-replication#general-information-about-external-replication-task)
* [Code Sample - External Replication](../../server/ongoing-tasks/external-replication#code-sample---external-replication)
* [Step-by-Step Guide](../../server/ongoing-tasks/external-replication#step-by-step-guide)
* [External Replication Task - Definition](../../server/ongoing-tasks/external-replication#external-replication-task---definition)  
* [External Replication Task - Offline Behaviour](../../studio/database/tasks/ongoing-tasks/external-replication-task#external-replication---offline-behaviour)
* [Delayed Replication](../../server/ongoing-tasks/external-replication#delayed-replication)

{NOTE/}

{PANEL: General Information about External Replication Task}

**What is being replicated:**  

  * All database documents  
  * [Attachments](../../document-extensions/attachments/what-are-attachments)  
  * [Revisions](../../server/extensions/revisions)  
  * [Counters](../../document-extensions/counters/overview)
  * [Time Series](../../document-extensions/timeseries/overview)

**What is _not_ being replicated:**  

  * [Indexes](../../indexes/creating-and-deploying)  
  * Any server level behaviours such as:  
    *  [Conflict resolver definition](../../server/clustering/replication/replication-conflicts#conflict-resolution-script)  

**Conflicts:**  

  * Two databases that have an External Replication task defined between them will detect and resolve document 
    [conflicts](../../server/clustering/replication/replication-conflicts) according to each database conflict resolution policy.  
  * It is recommended to have the same [policy configuration](../../server/clustering/replication/replication-conflicts#configuring-conflict-resolution-using-the-client) on both the source and the target databases.  

{PANEL/}

{PANEL: Code Sample - External Replication}

The vital elements of an External Replication task are:

* The `UpdateExternalReplicationOperation()` method.
* The target server needs the [certificate from the source](../../server/security/authentication/certificate-management#enabling-communication-between-servers-importing-and-exporting-certificates) 
  server so that it will trust the source.
* The [connection string](../../client-api/operations/maintenance/connection-strings/add-connection-string#add-a-raven-connection-string) with target server URL and any other details needed to access the target server.
* The following properties in the `ExternalReplication` object:
  * **ConnectionStringName**  
    The connection string name.  
  * **Name**  
    The target database name.

{CODE ExternalReplication@Server\OngoingTasks\ExternalReplicationSamples.cs /}

Optional elements include the following properties in the `ExternalReplication` object:

* **MentorNode**  
  Preferred responsible node in source server.
* **DelayReplicationFor**  
  Amount of time to delay replication.  
  The following sample shows a 30-minute delay.  Delay can also be set by days, hours, and seconds.  

{CODE ExternalReplicationWithMentorAndDelay@Server\OngoingTasks\ExternalReplicationSamples.cs /}

`ExternalReplication` properties:

{CODE ExternalReplicationProperties@Server\OngoingTasks\ExternalReplicationSamples.cs /}

{PANEL/}


{PANEL: Step-by-Step Guide}

1. **Pass Certificate from Source Server to Destination Server**  
  This step must be done if replicating *to a separate server* so that the destination server trusts the source.  
  * **Via RavenDB Studio:**  
    Navigate from the "Manage Server" tab (left side) > "Certificates" to open the [Certificate Management](../../server/security/authentication/certificate-management) view.  
     - Learn how to [pass certificates here](../../server/security/authentication/certificate-management#enabling-communication-between-servers-importing-and-exporting-certificates).  
  * **Via API:**  
    See the code sample to learn how to [define a client certificate in the DocumentStore()](../../../../client-api/creating-document-store).  
     - To generate and configure a client certificate from the source server, see [CreateClientCertificateOperation](../../../../client-api/operations/server-wide/certificates/create-client-certificate)
     - Learn the rationale needed to configure client certificates in [The RavenDB Security Authorization Approach](../../../../server/security/authentication/certificate-management#the-ravendb-security-authorization-approach)
2. **Create Target Database in Destination Server**  
  * Consider [creating an empty target database](../../../../studio/database/create-new-database/general-flow) 
    because data transfer can overwrite existing data.  
  * If the source database is [encrypted](../../../../studio/database/create-new-database/encrypted#creating-encrypted-database), 
    be sure that the target database is as well.  
    The Studio databases list view has a logo that shows which databases are encrypted.
     ![Encrypted Logo](images/encrypted-logo.png "Encrypted Logo")
3. **Define External Replication Task in the Source Database**  
    Learn more about [defining the task](../../../../studio/database/tasks/ongoing-tasks/external-replication-task#external-replication-task---definition) in the dedicated section.  
  * **Optional Definitions**  
     - Task Name  
     - Delay Time  
     - Preferred Node  
  * **Required Definition**  
     - Connection String  
  * **Save**  
    Click "Save" to activate the External Replication task.  
    Check the target database to see if data has transferred.  



{PANEL/}

{PANEL: External Replication Task - Definition}

To learn how to define an external replication task via code, see [code sample](../../server/ongoing-tasks/external-replication#code-sample---external-replication).  

To configure via RavenDB Studio:  

a. Select the source database  
b. Select **Tasks** tab  
c. **Ongoing Tasks**  
d. **Add a database task**  
e. **External Replication** to use the following interface.  

![Figure 1. External Replication Task Definition](images/external-replication-1.png "Create New External Replication Task")

1. **Source Database**  
   Be sure that you are defining the task from the correct source database.  

2. **Task Name** (Optional)  
   * Choose a name of your choice  
   * If no name is given then RavenDB server will create one for you based on the defined connection string  

3. **Task Delay Time** (Optional)  
   * If a delay time is set then data will be replicated only after this time period has passed for each data change.  
   * Having a delayed instance of a database allows you to "go back in time" and undo contamination to your data due to a faulty patch script or other human errors.  
     * This doesn't replace the need to [safely backup your databases](../../../../studio/database/tasks/backup-task), but it does provide a way to stay online while repairing.  

4. **Preferred Node** (Optional)  
  * Select a preferred mentor node from the [Database Group](../../../../studio/database/settings/manage-database-group) to be the responsible node for this External Replication Task  
  * If not selected, then the cluster will assign a responsible node (see [Members Duties](../../../../studio/database/settings/manage-database-group#database-group-topology---members-duties))  

5. **Connection String**  
  * Select a connection string from the pre-defined list -or- create a new connection string to be used.  
  * The connection string defines the external database and its server URL to replicate to.  
    ![External Replication: Connection String](images/external-replication-connection-string.png "External Replication: Connection String")
      1. **Name**  
        Give the connection string a meaningful name.  
      2. **Database**  
        Copy the exact name of the destination database.  
          * If the source database is encrypted, make sure that the destination is encrypted as well.
      3. **Discovery URL**  
        Copy the URL from the destination database here.
         ![External Replication: URL](images/external-replication-url.png "External Replication: URL")
           * Be sure to copy only the server URL - without extraneous details.  
      4. **Save**  
         Click "Save" to activate the External Replication task.

{PANEL/}

{PANEL: External Replication - Task Details}

![Figure 2. External Replication Task - Task List View](images/external-replication-2.png "Tasks List View Details")

1. **External Replication Task Details**:
   *  Task Status - Active / Not Active / Not on Node / Reconnect  
   *  Connection String - The connection string used  
   *  Destination Database - The external database to which the data is being replicated  
   *  Actual Destination URL - The server URL to which the data is actually being replicated,  
      the one that is currently used out of the available _Topology Discovery URLs_  
   *  Topology Discovery URLs - List of the available destination Database Group servers URLs  

2. **Graph view**:  
   Graph view of the responsible node for the External Replication Task  
{PANEL/}

{PANEL: External Replication - Offline Behaviour}

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


## Delayed Replication

In RavenDB we introduced a new kind of replication, _delayed replication_. It replicates data that is 
delayed by `X` amount of time.  
The _delayed replication_ works just like normal replication but instead of sending data immediately, 
it waits `X` amount of time.  
Having a delayed instance of a database allows you to "go back in time" and undo contamination to your data 
due to a faulty patch script or other human errors.  
While you can and should always use backup for those cases, having a live database makes it super fast to failover 
to prevent business losses while you repair the faulty databases.  

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
- [Certificates Management](../../server/security/authentication/certificate-management#enabling-communication-between-servers-importing-and-exporting-certificates)  
- [Hub/Sink Replication](../../server/ongoing-tasks/hub-sink-replication)  
- [Client Certificate Usage](../../server/security/authentication/client-certificate-usage)  

**Client API Articles**:  

- [Adding a Connection String](../../client-api/operations/maintenance/connection-strings/add-connection-string#operations-how-to-add-a-connection-string)  


