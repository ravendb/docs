# Configuration: Cluster
---

{NOTE: Avoid different cluster configurations among the cluster's nodes}
Configuration mismatches tend to cause interaction problems between nodes.

If you must set cluster configurations differently in separate nodes,  
**we recommend first testing it** in a development environment to see that each node interacts properly with the others.
{NOTE/}

---

{PANEL:Cluster.ElectionTimeoutInMs}

Timeout in which the node expects to receive a heartbeat from the leader, in milliseconds.

- **Type**: `int`
- **Default**: `300`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.WorkerSamplePeriodInMs}

Time in milliseconds between sampling the information about the databases and sending it to the maintenance supervisor.

- **Type**: `int`
- **Default**: `250`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.SupervisorSamplePeriodInMs}

 As the maintenance supervisor, time in milliseconds between sampling the information received from the nodes.

- **Type**: `int`
- **Default**: `500`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.ReceiveFromWorkerTimeoutInMs}

  As the maintenance supervisor, time in milliseconds to wait to hear from a worker before it is timed out.

- **Type**: `int`
- **Default**: `5000`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.OnErrorDelayTimeInMs}

As the maintenance supervisor, how long we wait after we received an exception from a worker. Before we retry.

- **Type**: `int`
- **Default**: `5000`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.OperationTimeoutInSec}

As a cluster node, set timeout in seconds for operation between two cluster nodes.

- **Type**: `int`
- **Default**: `15`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.StatsStabilizationTimeInSec}

As a cluster node, time in seconds in which it takes to timeout an operation between two cluster nodes.

- **Type**: `int`
- **Default**: `5`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.TimeBeforeAddingReplicaInSec}

The time in seconds we give to a database instance to be in a good and responsive state, before adding a replica to match the replication factor.

- **Type**: `int`
- **Default**: `900`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.TcpTimeoutInMs}

TCP connection read/write timeout in milliseconds.

- **Type**: `int`
- **Default**: `15000`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.HardDeleteOnReplacement}

Set hard/soft delete for a database that was removed by the observer from the cluster topology in order to maintain the replication factor.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.MaximalAllowedClusterVersion}

EXPERT: If exceeded, clamp the cluster to the specified version.  

- **Type**: `int?`
- **Default**: `null`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.LogHistoryMaxEntries}

EXPERT: Maximum number of log entires to keep in the history log table.  

- **Type**: `int`
- **Default**: `2048`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.DisableAtomicDocumentWrites}

EXPERT: Disable automatic atomic writes with cluster write transactions.  
If set to 'true', will only consider explicitly added compare exchange values 
to validate cluster wide transactions.  

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Cluster.MaxSizeOfSingleRaftCommandInMb}

EXPERT: The maximum allowed size allowed for a single raft command (in megabytes).

- **Type**: `Size?`
- SizeUnit(SizeUnit.Megabytes)
- **Default**: `128`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.MaxChangeVectorDistance}

Excceding the allowed change vector distance between two nodes, will move the lagged node to rehab.

- **Type**: `long`
- **Default**: `10_000`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.CompareExchangeExpiredDeleteFrequencyInSec}

Time (in seconds) between expired compare exchange cleanup.

- **Type**: `TimeSetting`
- TimeUnit(TimeUnit.Seconds)
- **Default**: `60`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.CompareExchangeTombstonesCleanupIntervalInMin}

Time (in minutes) between compare exchange tombstones cleanup.

- **Type**: `TimeSetting`
- TimeUnit(TimeUnit.Minutes)
- **Default**: `10`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.TcpReceiveBufferSizeInBytes}

TCP connection receive buffer size in bytes.

- **Type**: `Size`
- SizeUnit(SizeUnit.Bytes)
- **Default**: `32 * 1024`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.TcpSendBufferSizeInBytes}

TCP connection send buffer size in bytes.

- **Type**: `Size`
- SizeUnit(SizeUnit.Bytes)
- **Default**: `32 * 1024`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.TimeBeforeRotatingPreferredNodeInSec}

The grace time we give to the preferred node before we move it to the end of the members list.

- **Type**: `TimeSetting`
- TimeUnit(TimeUnit.Seconds)
- **Default**: `5`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Cluster.TimeBeforeMovingToRehabInSec}

The grace time we give to a node before it will be moved to rehab.

- **Type**: `TimeSetting`
- TimeUnit(TimeUnit.Seconds)
- **Default**: `60`
- **Scope**: Server-wide only

{PANEL/}
