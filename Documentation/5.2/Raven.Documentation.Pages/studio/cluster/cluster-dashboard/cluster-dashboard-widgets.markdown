# Cluster Dashboard: Widgets
---

{NOTE: }

* The **Cluster Dashboard** includes **diagnostics**, **configuration**, and **informational** widgets.  

* In this page:  
  * [Let's Get Started Widget](../../../studio/cluster/cluster-dashboard/cluster-dashboard-widgets#lets-get-started-widget)  
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

{PANEL: Cluster Dashboard Widgets}

---

### Let's Get Started Widget

The **Let's Get Started** widget offers a comfortable startup point at 
which you can set your cluster and database, and learn basic RavenDB concepts.  
![Let's Get Started Widget](images/cluster-dashboard-09-lets-get-started-widget.png "Let's Get Started Widget")

1. **Create New Database**  
   Click to [create a new database](../../../studio/server/databases/create-new-database/general-flow#2.-database-name).  
2. **Learn how to connect your client to the database**  
   Click to learn how to [connect your client to your database](../../../start/getting-started#documentstore) using RavenDB's API.  
3. **Setup Your Cluster**  
   Click to open Studio's [Cluster View](../../../studio/cluster/cluster-view) 
   so you can create and manage your cluster.  
4. **Learn more about Querying and Indexes**  
   Click to learn how [Querying](../../../indexes/querying/what-is-rql) 
   and [Indexes](../../../indexes/what-are-indexes) are managed in RavenDB.  
5. **Hide This Widget**  
   Click to remove the **Let's Get Started** widget from the cluster dashboard view.  
   The operation is reversible, widgets can be freely [added to the dashboard and removed from it]().  

---

### CPU Widget

The **CPU** widget displays **cores utilization** and **CPU percentage 
usage** statistics for all RavenDB nodes and for the machines that run these nodes.  
![CPU Widget](images/cluster-dashboard-10-cpu-widget.png "CPU Widget")

* Hover over the timeline to display CPU usage at a time of your choice.  
  By default, the widget shows CPU usage as it is presently.  

---

### Memory Widget

The **Memory** widget displays memory usage statistics 
for all RavenDB nodes and for the machines that run these nodes.  

**Memory Widget**  
![Memory Widget](images/cluster-dashboard-11_1-memory-widget.png "Memory Widget")

1. Click to toggle on/off additional **RavenDB memory usage** statistics.  
2. Click to toggle on/off additional **node machines memory usage** statistics.  

**Memory Widgwt - Additional Statistics View**  
![Memory Widgwt - Additional Statistics View](images/cluster-dashboard-11_2-memory-widget-details.png "Memory Widgwt - Additional Statistics View")

* Hover over the timeline to display memory usage at a time of your choice.  
  By default, the widget shows memory usage as it is presently.  

---

### Traffic Widget

The **Traffic** widget displays traffic statistics for all RavenDB nodes 
and for the machines that run these nodes.  
![Traffic Widget](images/cluster-dashboard-12_1-traffic-widget.png "Traffic Widget")

1. Click to toggle on/off additional Data Write statistics.  
2. Click to toggle on/off additional Data Written statistics.  

**Traffic Widget - Additional Statistics View**  
![Traffic Widget - Additional Statistics View](images/cluster-dashboard-12_2-traffic-widget-details.png "Traffic Widget - Additional Statistics View")

* Hover over the timeline to display traffic statistics at a time of your choice.  
  By default, the widget shows traffic statistics as it is presently.  

---

### Traffic Per Database Widget

The **Traffic Per Database** widget displays **Requests**, **Writes**, 
and **Data Written** statistics for all cluster nodes and databases.  
![Traffic Per Database Widget](images/cluster-dashboard-13-traffic-per-database-widget.png "Traffic Per Database Widget")

* **Node Tag**  
  Click a node tag to open a Traffic Watch view for this node and 
  see transactions methods, durations, and other details.  

---

### Indexing Widget

The **Indexing** widget displays **Map** and **Map Reduce** indexing statistics 
for all nodes.  
![Indexing Widget](images/cluster-dashboard-14-indexing-widget.png "Indexing Widget")

* Hover over the timeline to display indexing statistics at a time of your choice.  
  By default, the widget shows indexing statistics as they are presently.  

---

### Indexing Per Database Widget

The **Indexing Per Database** widget displays indexing statistics for all cluster nodes and databases.  
![Indexing Per Database Widget](images/cluster-dashboard-15-indexing-per-database-widget.png "Indexing Per Database Widget")

* **Node Tag**  
  Click a node tag to open an Indexing Performance view for the selected node.  

---

### Storage Widget

The **Storage** widget displays storage usage for all cluster nodes.  
![Storage Widget](images/cluster-dashboard-16-storage-widget.png "Storage Widget")

---

### Storage Per Database Widget

The **Storage Per Database** widget displays storage usage for all cluster nodes and databases.  
![Storage Per Database Widget](images/cluster-dashboard-17-storage-per-database-widget.png "Storage Per Database Widget")

* **Node Tag**  
  Click a node tag to open a Storage Report view for the selected node 
  and see its storage occupancy in detail.  

---

### License Widget

The **License** widget displays your license **Type**, **Expiration** 
date and remaining period, and **Support Type**.  
![License Widget](images/cluster-dashboard-18-license-widget.png "License Widget")

* **License Details**  
  Click to open the **License information** and **Support plan** page.  

{PANEL/}

**Server**  
[create a new database](../../../studio/server/databases/create-new-database/general-flow#2.-database-name)

**Studio**  
[Cluster Dashboard Overview](../../../studio/cluster/cluster-dashboard/cluster-dashboard-overview)  
[Cluster Dashboard Customization](../../../studio/cluster/cluster-dashboard/cluster-dashboard-customization)  
[Server Dashboard](../../../studio/server/server-dashboard)  
[Cluster View](../../../studio/cluster/cluster-view)

**Client API**  
[connect your client to your database](../../../start/getting-started#documentstore)

**Querying**  
[Querying](../../../indexes/querying/what-is-rql) 

**Indexing**  
[Indexes](../../../indexes/what-are-indexes)
