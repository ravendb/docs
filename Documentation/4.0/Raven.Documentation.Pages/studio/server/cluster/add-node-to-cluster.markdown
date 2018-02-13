# Adding a Node to a Cluster
---

{NOTE: }

* When a server is started for the first time, it is in a `passive` state.  

* Performing any RAFT command (such as listed below) will make the server become part of a cluster.  

  * Adding a license
  * Creating a database on this server
  * Adding another node to this server cluster (will cause both to be part of the same cluster)

* In this page:  
  * [Add Node to the Cluster](../../../studio/server/cluster/add-node-to-cluster#add-node-to-the-cluster)  
  * [A Cluster with 2 nodes](../../../studio/server/cluster/add-node-to-cluster#a-cluster-with-2-nodes)  

{NOTE/}

---

{PANEL: Add Node to the Cluster}

![Figure 1. Click to add a new node](images/cluster-add-node-1.png "Click to add a new node")

<br/>
![Figure 2. Adding a new node](images/cluster-add-node-2.png "Adding a new Node")

**1.** Enter the URL of the server for the new node  
**2.** Decide if to add the new node as a `Member` -or- as a `Watcher` (difference explained in [Cluster View](cluster-view))  
**3.** Click to use all available nodes -or- Enter the number of cores to be assigned for this node  
**4.** Click _Test connection_ to test the connection for the above url entered  

When done, click *Save* to add this server as a node to the cluster  
{PANEL/}

{PANEL: A Cluster with 2 nodes}

![Figure 3. Cluster with 2 nodes](images/cluster-add-node-3.png "A cluster with 2 nodes")

**Now your cluster contains 2 nodes**:  

* The server running on _localhost:8081_ shows as Node A and is the `Leader` of the cluster.  
* The server running on _localhost:8082_ shows as Node B and is a `Member` of the cluster.  

{NOTE: }
 More nodes can be added as needed.  
{NOTE/}
{PANEL/}
