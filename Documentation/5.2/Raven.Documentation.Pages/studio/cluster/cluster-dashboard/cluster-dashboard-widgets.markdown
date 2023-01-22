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

![Let's Get Started Widget](images/cluster-dashboard-09-lets-get-started-widget.png "Let's Get Started Widget")

1. The **Let's Get Started** widget offers a comfortable starting point with links 
   to setting up your cluster, creating a new database, and learning basic RavenDB concepts.  
2. **Create New Database**  
   Click to [create a new database](../../../studio/database/create-new-database/general-flow).  
3. **Setup Your Cluster**  
   Click to open Studio's [Cluster View](../../../studio/cluster/cluster-view) 
   so you can create and manage your cluster.  
4. **Learn how to connect your client to the database**  
   Click to learn how to [connect your client to your database](../../../start/getting-started#documentstore) using RavenDB's API.  
5. **Learn more about Querying and Indexes**  
   Click to learn how [Querying](../../../indexes/querying/what-is-rql) 
   and [Indexes](../../../indexes/what-are-indexes) are managed in RavenDB.  
6. **Hide This Widget**  
   Click to remove the **Let's Get Started** widget from the cluster dashboard view.  
   The operation is reversible, any widget that was removed can be [added](../../../studio/cluster/cluster-dashboard/cluster-dashboard-customize#add-widget) 
   later to the cluster dashboard.  

{PANEL/}

{PANEL: CPU Widget}

![CPU Widget](images/cluster-dashboard-10-cpu-widget.png "CPU Widget")

1. **RavenDb CPU Usage**  
   RavenDB usage of CPU & Cores per node.  
2. **Machine CPU Usage**  
   Machine usage of CPU & Cores per node.  
3. **Data Displayed**  
   The CPU widget shows the current CPU usage.  
   Hover over the timeline to display earlier data.  
   

{PANEL/}

{PANEL: Memory Widget}

![Memory Widget](images/cluster-dashboard-11_1-memory-widget.png "Memory Widget")

1. **RavenDB Memory Usage**  
   Memory used by RavneDB.  
2. **Machine Memory Usage**  
   Memory used by the nodes' machines.  
3. Click to toggle on/off additional statistics.  

**Memory Widget - Additional Statistics View**  

![Memory Widgwt - Additional Statistics View](images/cluster-dashboard-11_2-memory-widget-details.png "Memory Widgwt - Additional Statistics View")

* **Data Displayed**  
  The memory widget shows the current memory usage.  
  Hover over the timeline to display earlier data.  
  

{PANEL/}

{PANEL: Traffic Widget}

![Traffic Widget](images/cluster-dashboard-12_1-traffic-widget.png "Traffic Widget")

1. **Requests/s**  
   Number of HTTP requests made to the node per second.  
2. **Writes/s**  
   Number of items (documents, attachments, etc.) written by the node per second.  
3. **Data Written/s**  
   Amount of data written by the node per second.  
4. Click to toggle on/off additional statistics.  

**Traffic Widget - Additional Statistics View**  

![Traffic Widget - Additional Statistics View](images/cluster-dashboard-12_2-traffic-widget-details.png "Traffic Widget - Additional Statistics View")

* **Data displayed**  
  The traffic widget shows the current traffic usage.  
  Hover over the timeline to display earlier data.  

{PANEL/}

{PANEL: Traffic Per Database Widget}

![Traffic Per Database Widget](images/cluster-dashboard-13-traffic-per-database-widget.png "Traffic Per Database Widget")

1. **Database Name**  
   The Database column lists all your databases.  
2. **Node Tag**  
   Click a node tag to open the node's _Traffic Watch view_ where all HTTP requests made to the node can be viewed.  

{PANEL/}

{PANEL: Indexing Widget}

![Indexing Widget](images/cluster-dashboard-14-indexing-widget.png "Indexing Widget")

1. **Map Indexes**  
   Indexed items (documents, attachments, counters, and time series) per second.  
2. **Map-Reduce Indexes**  
   Mapped Items and Reduced Mapped Entries per second.  
3. **Data displayed**  
   The indexing widget shows the current indexing volume.  
   Hover over the timeline to display earlier data.  
  

{PANEL/}

{PANEL: Indexing Per Database Widget}

![Indexing Per Database Widget](images/cluster-dashboard-15-indexing-per-database-widget.png "Indexing Per Database Widget")

1. **Database Name**  
   The Database column lists all your databases.  
2. **Node Tag**  
   Click a node tag to open the _Indexing Performance view_ for the selected node.  

{PANEL/}

{PANEL: Storage Widget}

![Storage Widget](images/cluster-dashboard-16-storage-widget.png "Storage Widget")

1. Storage used by RavenDB.  
2. Storage used by the node's machine.  
3. Free storage remaining on the node's machine.  
4. Overall disk capacity on the node's machine.  

{PANEL/}

{PANEL: Storage Per Database Widget}

![Storage Per Database Widget](images/cluster-dashboard-17-storage-per-database-widget.png "Storage Per Database Widget")

1. **Database Name**  
   The Database column lists all your databases.  
2. **Node Tag**  
   Click a node tag to open the _Storage Report view_ for the selected node.  

{PANEL/}

{PANEL: License Widget}

![License Widget](images/cluster-dashboard-18-license-widget.png "License Widget")

1. The **License** widget displays your license **Type**, **Expiration** 
   date and remaining period, and **Support Type**.  
2. **License Details**  
   Click to open the [About view](../../../start/licensing/licensing-overview) which contains information about your 
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
