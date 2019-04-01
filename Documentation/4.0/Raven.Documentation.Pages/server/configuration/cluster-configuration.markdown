# Configuration: Cluster

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
