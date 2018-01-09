## Cluster View
---

{PANEL}
This view shows your cluster current state and structure.  
You can manage the cluster by actions such as:  

* Adding a node to the cluster
* Changing the Leader
* Reassigning cores
* And much more
{PANEL/}

---
### Cluster Stats

![Figure 1. Cluster Stats](images/cluster-view-1.png "Cluster Stats")

{PANEL}

1. **Available Cores**  
   Assigned: Total number of cores assigned to use for server nodes in all of the cluster  
   Licensed: Total number of cores available for usage according to your license - per the whole cluster
2. **Term**  
   Number of times that elections occured in the cluster
3. **Node State, Tag, and Address:**  
   A node state can be one of the following:  
   * _Passive_ - Node doesn't belong to any cluster
   * _Active_ - Node is a fully functional member in the cluster
   * _Error_ - Node is down or not reachable
   * _Voting_ - Elections for a new Leader are taking place
   * _Waiting_ -  This is the state of a Watcher when the other members are in a voting process
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

---
### Cluster Operations

![Figure 2. Cluster Operations](images/cluster-view-2.png "Cluster Operations")

{PANEL}

1. **Add New Node to Cluster**  
   // TODO: Add Link to page when created .......  
2. **Demote**  
   Demote a Member to be a Watcher  
3. **Reassign Cores**  
   Reassign the number of cores to be used by the server on this node  
4. **Step Down**  
   This option is available only on the Leader node,  
   A new voting process will be triggered and a new Leader will be elected  
5. **Force Timeout**  
   The default configuration for the RavenDB cluster is that each node expects to get a heartbeat from the cluster leader every 300 milliseconds.  
   Clicking 'Force Timeout' will trigger actions on the node as if it did Not hear from the Leader in this time period  
{PANEL/}

---
{PANEL: Nodes Types}

* **Member**
  * A Member is a fully functional voting member in the cluster  
<br/>  
* **Leader**
  * A Leader is a Member
  * The Leader is responsible for monitoring the cluster’s health,  
    making sure that decisions making is consistent at the cluster level - as long as a majority of the nodes are functioning and can talk to one another.  
    For example - the decision to add a database to a node will be either accepted by the entire cluster (eventually) or failed to register altogether.  
  * The Leader maintains the database topology, which is fetched by the clients as part of their initialization  
  * Cluster-wide operations can't be done when the Leader is down  
<br/>  
* **Watcher**
  * A Watcher is a non-voting node in the cluster that is still fully managed by it
  * A Watcher can be assigned databases and work to be done
  * Grow your RavenDB cluster by adding watchers without suffering from large voting majorities and the latencies they can incur,
    as these nodes don’t take part in majority calculations and are only there to watch what’s going on in the cluster.
    Thus, cluster decisions can be made with a majority of only small amount of nodes while the actual size of the cluster can be much higher.
  * Any number of Watchers can be added to handle the work load  
<br/>  
* **Promotable**  
  * Promotable is a pre-state before becoming a Watcher or a Member 
  * Cannot make cluster decisions (i.e vote for leader, enter a new raft command to the log) 
  * Updated by the leader to the latest Raft state 
{PANEL/}

---
{PANEL: Nodes States & Types Flow}

![Figure 3. States Flow](images/cluster-states.png "States Flow")

{PANEL}

**1.** A new server/node will start as **Passive**, meaning it is Not part of any cluster yet.  

**2.** When a node is added to a cluster, it immediatly becomes the **Leader** if it is the only node in the cluster  

**3.** When a node is added to a cluster and a Leader already exists,  
       it will become **Promotable** (on its way to either becoming a Member or a Watcher - depending on what was specified when created)

**4.** Node will become a **Member** of the cluster - if not specified otherwise  

**5.** Node will become a **Watcher** - if so specified when adding the node  

**6.** A member can be `Demoted` to a Wacher  

**7.** A Watcher can be `Promoted` to a Member, which means it will first become Promotabale and a Member thereafter  

**8.** A Member (a regular Member or a Leader - but Not a Watcher) can become a **Candidate** when a voting process for a new Leader takes place.  
{NOTE: }
       Elections can occur when:  
       * `Step Down` is clicked on the Leader  
       * `Force Timeout` was clicked  
       * Leader node is down for some reason  
{NOTE/}

**9.** When voting is over and a new Leader has been elected, one node will become a **Leader** and the rest will go back to being **Members**.  
{NOTE: }
 A **Watcher**  does Not take part in the voting process.  
      During elections, a Watcher enters a **Waiting** state until elections are over.
{NOTE/}

{PANEL/}
{PANEL/}

