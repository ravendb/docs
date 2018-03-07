# Server Dashboard
---

{NOTE: }
This dashboard provides a diagnostic overview of the RavenDB server performance and databases state.  
{NOTE/}

---

{PANEL: The Server Dashboard}

![Figure 1. Server Dashboard](images/server-dashboard.png "Server Dashboard")

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





