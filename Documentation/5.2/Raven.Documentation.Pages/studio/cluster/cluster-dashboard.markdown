# Cluster Dashboard
---

{NOTE: }

* The cluster dashboard displays live diagnostics related to all cluster nodes.  
* Diagnostics are displayed in dedicated widgets.  
* The dashboard view can be rearranged at will by adding, removing, dragging, and resizing widgets.  
* Widgets can invoke other diagnostics or management database sections, making the dashboard an intuitive control center. 

{NOTE/}

---

{PANEL: The Cluster Dashboard}

![Figure 1. Cluster Dashboard](images/cluster-dashboard-04-3nodes.png "Cluster Dashboard")

1. **Server Node Details:**  
   Data about the running server such as:   
   * The server IP address   
   * Time it is up and running   
   * Secured info
   * Number of members in cluster  

2. **Traffic:**   
   * Requests - Number of requests handled by the server        
   * Writes/s - Documents count written per sec by the server   
   * Data written/s - Number of bytes written by the server to disk        

3. **Databases:**   
   Databases summary - number of documents, indexes, alerts & replica factor  

4. **CPU & Memory:**   
   CPU & memory usage by the machine and by RavenDB

5. **Indexing:**    
   * Number of documents indexed per second with detailed info about mapped and reduced actions  
   * Showing total documents and per database  

6. **Storage:**   
   Databases size on disk

{PANEL/}




