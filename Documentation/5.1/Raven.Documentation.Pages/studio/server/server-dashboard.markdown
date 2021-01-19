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

---

{INFO: Help Us Improve Prompt}
When you first launch RavenDB, you will see this prompt asking if you'd be willing to 
anonymously share some Studio usage data with us in order to help us improve RavenDB:  

![NoSQL Database Share Studio Usage](images/help-us-improve.png "Help Us Improve")

Once you respond to this prompt, it should not appear again. However, in some scenarios, 
such as running RavenDB embedded, or working without browser cookies, the prompt may 
appear again.  

If necessary, you can add this flag to the Studio URL to prevent the prompt from 
appearing:  

`<Studio URL>#dashboard?disableAnalytics=true`

{INFO/}




