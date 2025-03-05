# Cluster Observer
---

{NOTE: }

* The primary goal of the **Cluster Observer** is to monitor the health of each database in the cluster  
  and adjust its topology to maintain the desired [Replication Factor](../../../server/clustering/distribution/distributed-database#replication-factor).

* This observer is always running on the Leader node.  
{NOTE/}

---

{PANEL: Operation Flow}

* In order to maintain the Replication Factor, every newly elected [Leader](../../../server/clustering/rachis/cluster-topology#leader) starts measuring the health of each node 
  by creating dedicated maintenance TCP connections to all other nodes in the cluster.  

* Each node reports the current status of _all_  its databases in intervals of [250 milliseconds](../../../server/configuration/cluster-configuration#cluster.workersampleperiodinms) (by default).  
  The `Cluster Observer` will consume those reports every [500 milliseconds](../../../server/configuration/cluster-configuration#cluster.supervisorsampleperiodinms) (by default).  

* Upon a **node failure**, the [Dynamic Database Distribution](../../../server/clustering/distribution/distributed-database#dynamic-database-distribution) sequence  
  will take place in order to ensure that the `Replication Factor` does not change.  

{NOTE: Note}
The _Cluster Observer_ stores its information **in memory**, so when the `Leader` loses its leadership, the collected reports of the _Cluster Observer_ and its decisions log are lost.  
{NOTE/}
{PANEL/}

{PANEL: Interacting with the Cluster Observer}

* You can interact with the `Cluster Observer` using the following REST API calls:  

| URL | Method | Query Params. | Description |
| - | - | - | - |
| `/admin/cluster/observer/suspend` | POST | value=[`bool`] | Setting `false` will suspend the _Cluster Observer_ operation for the current [Leader term](../../../studio/cluster/cluster-view#cluster-nodes-states-&-types-flow). |
| `/admin/cluster/observer/decisions` | GET | | Fetch the log of the recent decisions made by the cluster observer. |
| `/admin/cluster/maintenance-stats` | GET | | Fetch the latest reports of the _Cluster Observer_ |
{PANEL/}

{NOTE: }

**For example**:

* Let us assume a five-node cluster with servers A, B, C, D, E.  
  We create a database with a replication factor of 3 and define an ETL task.

* The newly created database will be distributed automatically to three of the cluster nodes.  
  Let's assume it is distributed to B, C, and E (so the database group is [B,C,E]),  
  and the cluster decides that node C is responsible for performing the ETL task.

* If node C goes offline or becomes unreachable, the Cluster Observer detects the issue.
  Initially:
    * After the duration specified in the [Cluster.TimeBeforeMovingToRehabInSec](../../../server/configuration/cluster-configuration#cluster.timebeforemovingtorehabinsec) configuration,  
      the observer moves node C to rehab mode, allowing time for recovery.
    * The ETL task fails over to another available node in the Database Group.

* If node C remains offline beyond the period specified in the [Cluster.TimeBeforeAddingReplicaInSec](../../../server/configuration/cluster-configuration#cluster.timebeforeaddingreplicainsec) configuration, the observer begins replicating the database to another node in the Database Group as a last resort.

{NOTE/}
