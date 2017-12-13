## Server Configuration : Cluster Options

<br>

#### ElectionTimeoutInMs
###### Timeout in which the node expects to receive a heartbeat from the leader, in milliseconds
###### Default Value : 300

<br><br>

#### WorkerSamplePeriodInMs
###### Time in millisecond between sampling the information about the databases and send it to the maintenance supervisor
###### Default Value : 250

<br><br>

#### SupervisorSamplePeriodInMs
###### As the maintenance supervisor, time in millisecond between sampling the information received from the nodes
###### Default Value : 500

<br><br>

#### ReceiveFromWorkerTimeoutInMs
###### As the maintenance supervisor, time in millisecond to wait to hear from a worker before it is time out
###### Default Value : 5000

<br><br>

#### OnErrorDelayTimeInMs
###### As the maintenance supervisor, time in millisecond to wait after we received an exception from a worker, before retry
###### Default Value : 5000

<br><br>

#### OperationTimeoutInSec
###### As a cluster node, set timeout in seconds for operation between two cluster nodes
###### Default Value : 15

<br><br>

#### StatsStabilizationTimeInSec
###### As a cluster node, time in seconds in which it takes to timeout operation between two cluster nodes
###### Default Value : 5

<br><br>

#### TimeBeforeAddingReplicaInSec
###### The time in seconds we give to a database instance to be in a good and responsive state, before adding a replica to match the replication factor
###### Default Value : 900

<br><br>

#### TcpTimeout
###### Tcp connection read/write timeout in milliseconds
###### Default Value : 15000

<br><br>

#### HardDeleteOnReplacement
###### Set hard/soft delete for a database that was removed by the observer form the cluster topology in order to maintain the replication factor
###### Default Value : true

