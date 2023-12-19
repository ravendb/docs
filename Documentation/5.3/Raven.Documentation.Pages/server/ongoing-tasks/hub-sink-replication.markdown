# Hub/Sink Replication

---

{NOTE: }

Hub/Sink replication is used to maintain a live replica of a database 
or a chosen part of it, through a secure connection between ongoing Hub 
and Sink replication tasks.  

RavenDB instances distributed onboard mobile library busses, for example, 
can collect data locally (e.g. bus GPS coordinates and books returns and 
borrows), and replicate it via local Sink tasks to the central library's 
Hub whenever they are online.  

{INFO: }

* Learn more about **Hub/Sink replication** [Here](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview).  
* You can use the Studio to define 
  [Hub](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-hub-task) 
  and [Sink](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-sink-task) 
  tasks.  

{INFO/}  

* In this page:  
   * [What is and is not replicated?](../../server/ongoing-tasks/hub-sink-replication#what-is-and-is-not-replicated?)  
   * [Defining Replication Tasks](../../server/ongoing-tasks/hub-sink-replication#defining-replication-tasks)  
     * [Defining a Replication Hub](../../server/ongoing-tasks/hub-sink-replication#defining-a-replication-hub)  
     * [Defining a Hub Access](../../server/ongoing-tasks/hub-sink-replication#defining-a-hub-access)  
     * [Defining a Replication Sink](../../server/ongoing-tasks/hub-sink-replication#defining-a-replication-sink)  
     * [Defining a Connection String](../../server/ongoing-tasks/hub-sink-replication#defining-a-connection-string)  
   * [Usage Sample](../../server/ongoing-tasks/hub-sink-replication#usage-sample)  
   * [Failover](../../server/ongoing-tasks/hub-sink-replication#failover)  
   * [Backward Compatibility](../../server/ongoing-tasks/hub-sink-replication#backward-compatibility)  
{NOTE/}

---


{PANEL: What is and is not replicated?}

{INFO: }

After the tasks are defined, **changed** documents whose replication is allowed by 
both the Hub and the Sink filters will replicate.  

If you want the entire database to be replicated in the destination, you can [import the database](../../studio/database/tasks/import-data/import-from-ravendb) 
into the destination.  

After the data is in the destination server, setting up a hub/sink replication ongoing task will keep the two databases up to date.  

{INFO/}

**What is being replicated:**  

  * All database documents and related data:  
    * [Attachments](../../document-extensions/attachments/what-are-attachments)  
    * [Revisions](../../document-extensions/revisions/overview)  
    * [Counters](../../document-extensions/counters/overview)
    * [Time Series](../../document-extensions/timeseries/overview)

**What is _not_ being replicated:**  

  * Server and cluster level features:  
    * [Indexes](../../indexes/creating-and-deploying)  
    * [Conflict resolver definitions](../../server/clustering/replication/replication-conflicts#conflict-resolution-script)  
    * [Compare-Exchange](../../client-api/operations/compare-exchange/overview)
    * [Subscriptions](../../client-api/data-subscriptions/what-are-data-subscriptions)
    * [Identities](../../server/kb/document-identifier-generation#strategy--3)  
    * Ongoing tasks
      * [ETL](../../server/ongoing-tasks/etl/basics)
      * [Backup](../../studio/database/tasks/backup-task)
      * [Hub/Sink Replication](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview)

{NOTE: Why are some cluster-level features not replicated?}
To provide for architecture that prevents conflicts between clusters, especially when ACID transactions are important, 
RavenDB is designed so that data ownership is at the cluster level.  
To learn more, see [Data Ownership in a Distributed System](https://ayende.com/blog/196769-B/data-ownership-in-a-distributed-system).

It is also best to ensure that each cluster defines policies, configurations, and ongoing tasks that are relevant for it.  
{NOTE/}

{PANEL/}



{PANEL: Defining Replication Tasks}

To start replication via Hub and Sink tasks, you need to define -  

1. **A Hub task**  
2. **Hub Access/es**  
    * Multiple Sink tasks can connect a Hub using each Access you define for it.  
    * Each Access has an associated certificate, that is used by the Sink to 
      authenticate with the Hub. This certificate is used to identify the specific 
      Access and the relevant filters for the connection.  
3. **Sink task/s**  
4. **Filtering**  
    * You can enable or disable *replication filtering*, and specify the paths 
      of documents whose replication is allowed.  
    * Allowed paths are defined separately for the Hub and for the Sink.  
    * You can filter incoming and outgoing replication defining separate lists of allowed paths 
      for *incoming* and *outgoing* documents.  
      * Only documents that are allowed by both the hub and sink filters will be replicated.  




---

## Defining a Replication Hub  

Use `PutPullReplicationAsHubOperation` to register a new Hub task,  
and configure it using a `PullReplicationDefinition` class.  

{CODE-BLOCK: csharp}
await store.Maintenance.SendAsync(new PutPullReplicationAsHubOperation 
    (new PullReplicationDefinition {
        Name = "Hub1_Bidirectional",
        Mode = PullReplicationMode.SinkToHub | PullReplicationMode.HubToSink,
        WithFiltering = true
    }));
{CODE-BLOCK/}

* **`PutPullReplicationAsHubOperation` definition**
      {CODE-BLOCK: csharp}
      public PutPullReplicationAsHubOperation(string name)  
      public PutPullReplicationAsHubOperation(PullReplicationDefinition pullReplicationDefinition)
      {CODE-BLOCK/}

* **`PullReplicationDefinition` parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `DelayReplicationFor` | `TimeSpan` | Amount of time to wait before starting replication |
    | `Disabled` | `bool` | Disable task or leave it enabled |
    | `MentorNode` | `string` | Preferred Mentor Node |
    | `Mode` | `PullReplicationMode` | Data Direction (HubToSink, SinkToHub, or Both) |
    | `Name` | `string` | Task Name |
    | `TaskId` | `long` | Task ID |
    | `WithFiltering` | `bool` | Allow Replication Filtering |

---

### Defining a Hub Access

Use `RegisterReplicationHubAccessOperation` to define a Hub Access,  
and configure it using a `ReplicationHubAccess` class.  

{CODE-BLOCK: csharp}
await store.Maintenance.SendAsync(new RegisterReplicationHubAccessOperation
   ("Hub1_Bidirectional", new ReplicationHubAccess {
        Name = "Access1",
        AllowedSinkToHubPaths = new[]
        {
            "products/*",
        },
        AllowedHubToSinkPaths = new[]
        {
            "products/*",
        },
        CertificateBase64 = Convert.ToBase64String(pullCert.Export(X509ContentType.Cert))
    }));
{CODE-BLOCK/}

* **`RegisterReplicationHubAccessOperation` definition**
      {CODE-BLOCK: csharp}
      public RegisterReplicationHubAccessOperation(string hubName, ReplicationHubAccess access)  
      {CODE-BLOCK/}

* **`ReplicationHubAccess` parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `Name` | `string` | Task Name |
    | `CertificateBase64` | `string` | Task Certificate |
    | `AllowedHubToSinkPaths` | `string[]` | Allowed paths from Hub to Sink |
    | `AllowedSinkToHubPaths` | `string[]` | Allowed paths from Sink to Hub |

{INFO: }
To **Remove** an existing Access, use `UnregisterReplicationHubAccessOperation`.  

* **`UnregisterReplicationHubAccessOperation` definition**:
      {CODE-BLOCK: csharp}
      public UnregisterReplicationHubAccessOperation(string hubName, string thumbprint)  
      {CODE-BLOCK/}
{INFO/}


## Defining a Replication Sink  

Use `UpdatePullReplicationAsSinkOperation` to define a Sink task,  
and configure it using a `PullReplicationAsSink` class.  

{CODE-BLOCK: csharp}
await store.Maintenance.SendAsync(new UpdatePullReplicationAsSinkOperation
   (new PullReplicationAsSink {
        ConnectionStringName = dbName + "_ConStr",
        Mode = PullReplicationMode.SinkToHub | PullReplicationMode.HubToSink,
        CertificateWithPrivateKey = Convert.ToBase64String(pullCert.Export(X509ContentType.Pfx)),
        HubName = "Bidirectional",
        AllowedHubToSinkPaths = new[]
        {
            "employees/8-A"
        },
        AllowedSinkToHubPaths = new[]
        {
            "employees/8-A"
        }
    }));
{CODE-BLOCK/}

* **`UpdatePullReplicationAsSinkOperation` definition**
      {CODE-BLOCK: csharp}
      public UpdatePullReplicationAsSinkOperation(PullReplicationAsSink pullReplication)  
      {CODE-BLOCK/}

* **`PullReplicationAsSink` parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `Mode` | `PullReplicationMode` | Data Direction (HubToSink, SinkToHub, or Both) |
    | `AllowedHubToSinkPaths` | `string[]` | Allowed paths from Hub to Sink |
    | `AllowedSinkToHubPaths` | `string[]` | Allowed paths from Sink to Hub |
    | `CertificateWithPrivateKey` | `string` | A certificate with the Sink's Private key |
    | `CertificatePassword` | `string` | Certificate Password |
    | `AccessName` | `string` | Access Name to connect to |
    | `HubName` | `string` | Hub Name to connect to |

---

### Defining a Connection String  

The Sink needs a connection string to locate the Hub task it is to use.  

Use `PutConnectionStringOperation` to define a connection string,  
and configure it using a `RavenConnectionString` class.  

{CODE-BLOCK: csharp}
await storeA.Maintenance.SendAsync(
    new PutConnectionStringOperation<RavenConnectionString>(new RavenConnectionString
    {
        Database = dbNameB,
        Name = dbName + "_ConStr",
        TopologyDiscoveryUrls = store.Urls
    }));
{CODE-BLOCK/}

Learn about Connection Strings [here](../../client-api/operations/maintenance/connection-strings/add-connection-string#operations-how-to-add-a-connection-string).  


{PANEL/}

{PANEL: Usage Sample}

{CODE-BLOCK: csharp}
// Issue a certificate
var pullCert = new X509Certificate2("/path/to/cert.pfx", 
    (string)null, X509KeyStorageFlags.Exportable);

// Define a Hub task
await store.Maintenance.SendAsync(new PutPullReplicationAsHubOperation(
    new PullReplicationDefinition
    {
        Name = "Hub1_SinkToHub_Filtered",
        Mode = PullReplicationMode.SinkToHub | PullReplicationMode.HubToSink,
        WithFiltering = true
    }));

// Define Hub access
await store.Maintenance.SendAsync(new RegisterReplicationHubAccessOperation(
    "Hub1_SinkToHub_Filtered", new ReplicationHubAccess
    {
        Name = "Access1",
        AllowedSinkToHubPaths = new[]
        {
            "products/*",
            "orders/*"
        },
        
        // The public portion of the certificate, in base 64
        CertificateBase64 = Convert.ToBase64String(pullCert.Export(X509ContentType.Cert))
    }));

// Define a Connection String
await store.Maintenance.SendAsync(
    new PutConnectionStringOperation<RavenConnectionString>(new RavenConnectionString
    {
        Database = dbNameB,
        Name = dbNameB + "_ConStr",
        TopologyDiscoveryUrls = store.Urls
    }));

// Define a Sink task
await store.Maintenance.SendAsync(
    new UpdatePullReplicationAsSinkOperation(new PullReplicationAsSink
    {
        ConnectionStringName = dbNameB + "_ConStr",
        Mode = PullReplicationMode.SinkToHub,
        CertificateWithPrivateKey = Convert.ToBase64String(pullCert.Export(X509ContentType.Pfx)),
        HubName = "Hub1_SinkToHub_Filtered"
    }));
{CODE-BLOCK/}

{PANEL/}

{PANEL: Failover}

Since the Sink task always initiates the replication, it is 
also the Sink's responsibility to reconnect on network failure. 

---

### Hub Failure 
As part of the connection handshake, the Sink fetches an ordered list 
of nodes from the Hub cluster. If a preferred node is defined (by explicitly 
selecting a mentor node), it will be at the top of this list.  
The Sink will try to connect to the first node in the list, and proceed 
down the list with every failed attempt.  
If the connection fails with all nodes, the Sink will request the list again.  

---

### Sink Failure 
If the failure occurs on the Sink node, the Sink cluster will 
select a different node for the job.  

{PANEL/}

{PANEL: Backward Compatibility}

RavenDB versions that precede 5.1 support **Pull Replication**, which allows 
you to define *Hub and Sink* tasks and replicate data from Hub to Sink.  

In RavenDB 5.1 and on, *Pull Replication* is replaced and enhanced by 
*Hub/Sink Replication*, which provides everything *Pull Replication* does 
and adds to it *Sink to Hub* replication and *Replication Filtering*.  

* Pull Replication tasks defined on a RavenDB version earlier than 5.1, 
  **will remain operative** when you upgrade to version 5.1 and on.  

* A Hub or a Sink task that runs on a RavenDB version earlier than 
  5.1, **can** connect a Hub or a Sink defined on RavenDB 5.1 and on.  
  You do **not** need to upgrade the task's instance to keep the task operative.  

{INFO: }
Upgrade RavenDB from a version earlier than 5.1 if you want to implement 
*Hub/Sink Replication* added features, i.e. Sink-to-Hub replication and 
Replication Filtering.  
{INFO/}

{PANEL/}


## Related Articles

**Studio Articles**:  
[Hub/Sink Replication: Overview](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview)  
[Replication Hub Task](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-hub-task)  
[Replication Sink Task](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-sink-task)  

**Server Articles**:  
[External Replication](../../server/ongoing-tasks/external-replication)  
[Client Certificate Usage](../../server/security/authentication/client-certificate-usage)  

**Client Articles**:  
[Adding a Connection String](../../client-api/operations/maintenance/connection-strings/add-connection-string#operations-how-to-add-a-connection-string)  
