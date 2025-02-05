# Cluster Observer
---

{NOTE: }

* The primary goal of the `Cluster Observer` is to maintain the [Replication Factor](../../../server/clustering/distribution/distributed-database#replication-factor) of each database in the cluster.  

* This observer is always running on the [Leader](../../../server/clustering/rachis/cluster-topology#leader) node.

* In this page:
  * [Operation flow](../../../server/clustering/distribution/cluster-observer#operation-flow)
  * [Interacting with the Cluster Observer](../../../server/clustering/distribution/cluster-observer#interacting-with-the-cluster-observer)

{NOTE/}

---

{PANEL: Operation flow}

* To maintain the Replication Factor, every newly elected [Leader](../../../server/clustering/rachis/cluster-topology#leader) starts measuring the health of each node 
  by creating dedicated maintenance TCP connections to all other nodes in the cluster.  

* Each node reports the current status of _all_  its databases at intervals of [500 milliseconds](../../../server/configuration/cluster-configuration#cluster.workersampleperiodinms) (by default).  
  The `Cluster Observer` consumes those reports every [1000 milliseconds](../../../server/configuration/cluster-configuration#cluster.supervisorsampleperiodinms) (by default).  

* Upon a **node failure**, the [Dynamic Database Distribution](../../../server/clustering/distribution/distributed-database#dynamic-database-distribution) sequence
  will take place in order to ensure that the `Replication Factor` does not change.  

    {NOTE: }
    
    **For example**:  
    
    * Let us assume a five-node cluster with servers A, B, C, D, E.  
      We create a database with a replication factor of 3 and define an ETL task.
    
    * The newly created database will be distributed automatically to three of the cluster nodes.  
      Let's assume it is distributed to B, C and E (so the database group is [B,C,E]),  
      and the cluster decides that node C is responsible for performing the ETL task.
    
    * If node C goes offline or is not reachable, the Observer will notice it and relocate the database from node C to another available node.
      Meanwhile, the ETL task will fail over to another available node from the Database Group.  
    
    {NOTE/}

    {WARNING: }

    **Note**:  

    * The _Cluster Observer_ stores its information **in memory**, so when the `Leader` loses leadership,  
      the collected reports of the _Cluster Observer_ and its decision log are lost.

    {WARNING/}

{PANEL/}

{PANEL: Interacting with the Cluster Observer}

You can interact with the `Cluster Observer` using the following REST API calls:  

| URL                                 | Method  | Query Params   | Description                                                                                                                                                          |
|-------------------------------------|---------|----------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `/admin/cluster/observer/suspend`   | POST    | value=[`bool`] | Setting `false` will suspend the _Cluster Observer_ operation for the current [Leader term](../../../studio/cluster/cluster-view#cluster-nodes-states-&-types-flow). |
| `/admin/cluster/observer/decisions` | GET     |                | Fetch the log of the recent decisions made by the cluster observer.                                                                                                  |
| `/admin/cluster/maintenance-stats`  | GET     |                | Fetch the latest reports of the _Cluster Observer_                                                                                                                   |

{PANEL/}
