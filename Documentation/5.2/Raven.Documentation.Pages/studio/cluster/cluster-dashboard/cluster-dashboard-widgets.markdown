# Cluster Dashboard: Widgets
---

{NOTE: }

* Widgets in the cluster dashboard display live diagnostic information about the whole cluster.  
* Information is displayed for all cluster nodes.  

* Available Widgets:  
  * [Let's Get Started Widget](../../../studio/cluster/cluster-dashboard/cluster-dashboard-widgets#let)  
  * [CPU Widget](../../../studio/cluster/cluster-dashboard/cluster-dashboard-widgets#cpu-widget)  
  * [Memory Widget](../../../studio/cluster/cluster-dashboard/cluster-dashboard-widgets#memory-widget)  
  * [Traffic Widget](../../../studio/cluster/cluster-dashboard/cluster-dashboard-widgets#traffic-widget)  
  * [Traffic Per Database Widget](../../../studio/cluster/cluster-dashboard/cluster-dashboard-widgets#traffic-per-database-widget)  
  * [Indexing Widget](../../../studio/cluster/cluster-dashboard/cluster-dashboard-widgets#indexing-widget)  
  * [Indexing Per Database Widget](../../../studio/cluster/cluster-dashboard/cluster-dashboard-widgets#indexing-per-database-widget)  
  * [Storage Widget](../../../studio/cluster/cluster-dashboard/cluster-dashboard-widgets#storage-widget)  
  * [Storage Per Database Widget](../../../studio/cluster/cluster-dashboard/cluster-dashboard-widgets#storage-per-database-widget)  
  * [License Widget](../../../studio/cluster/cluster-dashboard/cluster-dashboard-widgets#license-widget)  
{NOTE/}

---

{PANEL: Let's Get Started Widget}

The **Let's Get Started** widget offers a comfortable starting point with links 
to setting up your cluster, creating a new database, and learning basic RavenDB concepts.  

![Let's Get Started Widget](images/cluster-dashboard-09-lets-get-started-widget.png "Let's Get Started Widget")

1. **Create New Database**  
   Click to [create a new database](../../../studio/database/create-new-database/general-flow).  
2. **Setup Your Cluster**  
   Click to open Studio's [Cluster View](../../../studio/cluster/cluster-view) 
   so you can create and manage your cluster.  
3. **Learn how to connect your client to the database**  
   Click to learn how to [connect your client to your database](../../../start/getting-started#documentstore) using RavenDB's API.  
4. **Learn more about Querying and Indexes**  
   Click to learn how [Querying](../../../indexes/querying/what-is-rql) 
   and [Indexes](../../../indexes/what-are-indexes) are managed in RavenDB.  
5. **Hide This Widget**  
   Click to remove the **Let's Get Started** widget from the cluster dashboard view.  
   The operation is reversible, any widget that was removed can be [added](../../../studio/cluster/cluster-dashboard/cluster-dashboard-customize#add-widget) 
   later to the cluster dashboard.  

{PANEL/}

{PANEL: CPU Widget}

The **CPU** widget displays **cores utilization** and **CPU percentage 
usage** statistics for all RavenDB nodes and for the machines that run these nodes.  

![CPU Widget](images/cluster-dashboard-10-cpu-widget.png "CPU Widget")

1. RavenDB nodes CPU Usage.  
2. Node machines CPU Usage.  
3. The CPU widget shows CPU usage as it is presently.  
   Hover over the timeline to display CPU usage at a time of your choice.  
   

{PANEL/}

{PANEL: Memory Widget}

The **Memory** widget displays memory usage statistics 
for all RavenDB nodes and for the machines that run these nodes.  

![Memory Widget](images/cluster-dashboard-11_1-memory-widget.png "Memory Widget")

1. RavenDB nodes memory usage.  
2. Click to toggle on/off additional **RavenDB memory usage** statistics.  
3. Machines memory usage.  
4. Click to toggle on/off additional **machines memory usage** statistics.  

**Memory Widgwt - Additional Statistics View**  

![Memory Widgwt - Additional Statistics View](images/cluster-dashboard-11_2-memory-widget-details.png "Memory Widgwt - Additional Statistics View")

* The memory widget shows memory usage as it is presently.  
  Hover over the timeline to display memory usage at a time of your choice.  
  

{PANEL/}

{PANEL: Traffic Widget}

The **Traffic** widget displays traffic statistics for all RavenDB nodes 
and for the machines that run these nodes.  

![Traffic Widget](images/cluster-dashboard-12_1-traffic-widget.png "Traffic Widget")

1. Nodes' **Requests** per Second.  
2. Nodes' **Writes** per Second.  
3. Click to toggle on/off additional Writes statistics.  
4. Nodes' **Data Written** per Second.  
5. Click to toggle on/off additional Data Written statistics.  

**Traffic Widget - Additional Statistics View**  

![Traffic Widget - Additional Statistics View](images/cluster-dashboard-12_2-traffic-widget-details.png "Traffic Widget - Additional Statistics View")

* The traffic widget shows traffic statistics as they are presently.  
  Hover over the timeline to display traffic statistics at a time of your choice.  
  

{PANEL/}

{PANEL: Traffic Per Database Widget}

The **Traffic Per Database** widget displays **Requests**, **Writes**, 
and **Data Written** statistics for each database..  

![Traffic Per Database Widget](images/cluster-dashboard-13-traffic-per-database-widget.png "Traffic Per Database Widget")

* **Node Tag**  
  Click a node tag to open the node's Traffic Watch view for details 
  about the server's HTTP requests methods.  

{PANEL/}

{PANEL: Indexing Widget}

The **Indexing** widget displays indexing statistics for 
**Map** and **Map Reduce** indexes.  

![Indexing Widget](images/cluster-dashboard-14-indexing-widget.png "Indexing Widget")

* The indexing widget shows indexing statistics as they are presently.  
  Hover over the timeline to display indexing statistics at a time of your choice.  
  

{PANEL/}

{PANEL: Indexing Per Database Widget}

The **Indexing Per Database** widget displays indexing statistics for all cluster nodes 
per database.  

![Indexing Per Database Widget](images/cluster-dashboard-15-indexing-per-database-widget.png "Indexing Per Database Widget")

* **Node Tag**  
  Click a node tag to open the Indexing Performance view for the selected node.  

{PANEL/}

{PANEL: Storage Widget}

The **Storage** widget displays the storage usage in each cluster node.  

![Storage Widget](images/cluster-dashboard-16-storage-widget.png "Storage Widget")

{PANEL/}

{PANEL: Storage Per Database Widget}

The **Storage Per Database** widget displays storage usage for all cluster nodes per database.  

![Storage Per Database Widget](images/cluster-dashboard-17-storage-per-database-widget.png "Storage Per Database Widget")

* **Node Tag**  
  Click a node tag to open the Storage Report view for the selected node 
  and see its storage occupancy in detail.  

{PANEL/}

{PANEL: License Widget}

The **License** widget displays your license **Type**, **Expiration** 
date and remaining period, and **Support Type**.  

![License Widget](images/cluster-dashboard-18-license-widget.png "License Widget")

* **License Details**  
  Click to open the About view which contains information about your 
  License and Support plan.  

{PANEL/}

## Related Articles  

### Server
- [create a new database](../../../studio/database/create-new-database/general-flow)

###Studio
- [Cluster Dashboard - Overview](../../../studio/cluster/cluster-dashboard/cluster-dashboard-overview)  
- [Cluster Dashboard - Customize](../../../studio/cluster/cluster-dashboard/cluster-dashboard-customize)  
- [Server Dashboard](../../../studio/server/server-dashboard)  
- [Cluster View](../../../studio/cluster/cluster-view)

###Client API
- [connect your client to your database](../../../start/getting-started#documentstore)

###Querying
- [Querying](../../../indexes/querying/what-is-rql) 

###Indexing
- [Indexes](../../../indexes/what-are-indexes)
