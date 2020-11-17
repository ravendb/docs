# Hub/Sink Replication

---

{NOTE: }

Hub/Sink replication is used to maintain a live replica of a database 
or a chosen part of it, through a secure connection between ongoing Hub 
and Sink replication tasks.  

{INFO: }

* Learn more about **Hub/Sink replication** [Here](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview).  
* You can use the Studio to define 
  [Hub](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-hub-task) 
  and [Sink](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-sink-task) 
  tasks.  

{INFO/}  

* In this page:  
   * [Defining Replication Tasks](../../server/ongoing-tasks/hub-sink-replication#defining-replication-tasks)  
     * [Defining a Replication Hub](../../server/ongoing-tasks/hub-sink-replication#defining-a-replication-hub)  
     * [Defining a Hub Access](../../server/ongoing-tasks/hub-sink-replication#defining-a-hub-access)  
     * [Defining a Replication Sink](../../server/ongoing-tasks/hub-sink-replication#defining-a-replication-sink)  
     * [Defining a Connection String](../../server/ongoing-tasks/hub-sink-replication#defining-a-connection-string)  
   * [Usage Sample](../../server/ongoing-tasks/hub-sink-replication#usage-sample)  
   * [Failover](../../server/ongoing-tasks/hub-sink-replication#failover)  
{NOTE/}

---

{PANEL: Defining Replication Tasks}

To start replication via Hub and Sink tasks, you need to define -  

1. **A Hub task**  
2. **Hub Access/es**  
   Multiple Sink tasks can connect the Hub using each access.  
   For each access, you need to issue a certificate with the Hub's public key and with a private 
   key (that the Hub task doesn't keep) for Sink tasks that need to connect the Hub using this access.  
3. **Sink task/s**  

When this is done, changed documents whose replication is allowed by 
both Hub and Sink will replicate.  

---

## Defining a Replication Hub  

Use `PutPullReplicationAsHubOperation` to register a new Hub task,  
and configure it using a `PullReplicationDefinition` class.  

{CODE-BLOCK: JSON}
await store.Maintenance.SendAsync(new PutPullReplicationAsHubOperation 
    (new PullReplicationDefinition {
        Name = "Hub1_Bidirectional",
        Mode = PullReplicationMode.SinkToHub | PullReplicationMode.HubToSink,
        WithFiltering = true
    }));
{CODE-BLOCK/}

* **`PutPullReplicationAsHubOperation` definition**
      {CODE-BLOCK: JSON}
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

{CODE-BLOCK: JSON}
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
      {CODE-BLOCK: JSON}
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
      {CODE-BLOCK: JSON}
      public UnregisterReplicationHubAccessOperation(string hubName, string thumbprint)  
      {CODE-BLOCK/}
{INFO/}


## Defining a Replication Sink  

Use `UpdatePullReplicationAsSinkOperation` to define a Sink task,  
and configure it using a `PullReplicationAsSink` class.  

{CODE-BLOCK: JSON}
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
      {CODE-BLOCK: JSON}
      public UpdatePullReplicationAsSinkOperation(PullReplicationAsSink pullReplication)  
      {CODE-BLOCK/}

* **`PullReplicationAsSink` parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `Mode` | `PullReplicationMode` | Data Direction (HubToSink, SinkToHub, or Both) |
    | `AllowedHubToSinkPaths` | `string[]` | Allowed paths from Hub to Sink |
    | `AllowedSinkToHubPaths` | `string[]` | Allowed paths from Sink to Hub |
    | `CertificateWithPrivateKey` | `string` | Certificate with a public key for a Hub Access and the Sink's Private key |
    | `CertificatePassword` | `string` | Certificate Password |
    | `AccessName` | `string` | Access Name to connect to |
    | `HubName` | `string` | Hub Name to connect to |

---

### Defining a Connection String  

The Sink needs a connection string to locate the Hub task it is to use.  

Use `PutConnectionStringOperation` to define a connection string,  
and configure it using a `RavenConnectionString` class.  

{CODE-BLOCK: JSON}
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

{CODE-BLOCK: JSON}
// Issue a certificate
var pullCert = new X509Certificate2(File.ReadAllBytes(certificates.ClientCertificate2Path), 
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
As part of the connection handshake, the Sink fetches an ordered 
list of nodes from the Hub cluster. If defined, the preferred node 
will be at the top of it.  
The Sink will try to connect the first node in the list, and 
proceed down the list with every failed attempt.  
If connection fails with all nodes, the Sink will request 
the list again. 

---

### Sink Failure 
If the failure occurrs on the Sink node, the Sink cluster will 
select a different node for the job.  

{PANEL/}

## Related Articles

**Studio Articles**:  
[Hub/Sink Replication: Overview](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview)  
[Replication Hub Task](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-hub-task)  
[Replication Sink Task](../../studio/database/tasks/ongoing-tasks/hub-sink-replication/replication-sink-task)  

**Server Articles**:  
[External Replication](../../server/ongoing-tasks/external-replication)  
[Authentication: Client Certificate Usage](../../server/security/authentication/client-certificate-usage)  

**Client Articles**:  
[Adding a Connection String](../../client-api/operations/maintenance/connection-strings/add-connection-string#operations-how-to-add-a-connection-string)  
