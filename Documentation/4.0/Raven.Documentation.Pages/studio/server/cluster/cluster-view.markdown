# Cluster View
---

{NOTE: }

* This view shows your cluster's current state and structure.  

* You can manage the cluster by actions such as:  

  * Adding a node to the cluster  
  * Changing the leader  
  * Reassigning cores  
  * And much more  
{NOTE/}

---

{PANEL: Cluster Stats}

![Figure 1. Cluster Stats](images/cluster-view-1.png "Cluster Stats")

1. **Available Cores**  
   Assigned: Total number of cores assigned to use for server nodes in all of the cluster  
   Licensed: Total number of cores available for usage according to your license - per the whole cluster
2. **Term**  
   Number of times that elections occurred in the cluster
3. **Node State, Tag, and Address:**  
   A node state can be one of the following:  
   * _Passive_ - Node doesn't belong to any cluster
   * _Active_ - Node is a fully functional member in the cluster
   * _Error_ - Node is down or not reachable
   * _Voting_ - Elections for a new leader are taking place
   * _Waiting_ -  This is the state of a watcher when the other members are in a voting process
4. **Node Type**  
   A node type can be one of the following:  (See a more detailed explanation below)  
   * _Member_ (Note: A _Leader_ is a special member)
   * _Watcher_
   * _Promotable_
5. **Cores Info**  
   Number of cores assigned for the node to use - out of available cores on this node's machine
6. **Memory**  
    Memory used by the node out of total memory installed on this node's machine.  
    The core-memory ratio is determined by your license type
{PANEL/}

{PANEL: Cluster Operations}

![Figure 2. Cluster Operations](images/cluster-view-2.png "Cluster Operations")

1. **Add New Node to Cluster**  
   See [Add node to cluster](add-node-to-cluster)
2. **Demote**  
   Demote a Member to be a Watcher  
3. **Reassign Cores**  
   Reassign the number of cores to be used by the server on this node  
4. **Step Down**  
   This option is available only on the leader node 
   A new voting process will be triggered and a new leader will be elected  
5. **Force Timeout**  
   The default configuration for the RavenDB cluster is that each node expects to get a heartbeat from the cluster leader every 300 milliseconds.  
   Clicking 'Force Timeout' will trigger actions on the node as if it did Not hear from the Leader in this time period  
{PANEL/}

{PANEL: Nodes Types}

* **Member**
  * A Member is a fully functional voting member in the cluster  
<br/>  
* **Leader**
  * A leader is a member
  * The leader is responsible for monitoring the cluster’s health, making sure that decisions making is consistent at the cluster level as long as a majority of the nodes are functioning and can talk to one another.  
    For example, the decision to add a database to a node will be either accepted by the entire cluster (eventually) or fail to register altogether.  
  * The leader maintains the database topology, which is fetched by the clients as part of their initialization.  
  * Cluster-wide operations can't be done when the leader is down  
<br/>  
* **Watcher**
  * A watcher is a non-voting node in the cluster that is still fully managed by it
  * A watcher can be assigned databases and work to be done
  * Grow your RavenDB cluster by adding watchers without suffering from large voting majorities and the latencies they can incur. These nodes don’t take part in majority calculations and are only there to watch what’s going on in the cluster.
    Cluster decisions can be made with a majority of only a small amount of nodes while the actual size of the cluster can be much higher.
  * Any number of watchers can be added to handle the work load  
<br/>  
* **Promotable**  
  * Promotable is a pre-state before becoming a watcher or a member 
  * Cannot make cluster decisions (i.e vote for leader, enter a new raft command to the log) 
  * Updated by the leader to the latest RAFT state 
{PANEL/}

{PANEL: Nodes States & Types Flow}

![Figure 3. States Flow](images/cluster-states.png "States Flow")

{PANEL}

**1.** A new server/node will start as **Passive**, meaning it is not part of any cluster yet.  

**2.** When a node is added to a cluster, it immediately becomes the **Leader** if it is the only node in the cluster.  

**3.** When a node is added to a cluster and a leader already exists, it will become **Promotable** (on its way to either becoming a member or a watcher - depending on what was specified when created.)

**4.** A node will become a **member** of the cluster when not specified otherwise  

**5.** A node will become a **watcher** if specified when adding the node  

**6.** A member can be `Demoted` to a watcher  

**7.** A watcher can be `Promoted` to a member. It will first become promotable and a member thereafter  

**8.** A member (a regular member or a leader - but not a watcher) can become a **Candidate** when a voting process for a new leader takes place.  
{NOTE: }
       Elections can occur when:  
       * `Step Down` is clicked on the leader  
       * `Force Timeout` was clicked  
       * The leader node is down for some reason  
{NOTE/}

**9.** When the voting is over and a new leader is elected, one node will become a **leader** and the rest will go back to being **members**.  
{NOTE: }
 A **watcher**  does not take part in the voting process.  
     During elections, a watcher enters a **waiting** state until elections are over.
{NOTE/}

{PANEL/}
{PANEL/}

