# Studio: Manage Server
---

{NOTE: }

* Use the **Manage Server** menu to access your server's configuration and debugging options.  

* In this page:  
   * [Manage Server menu](../../studio/server/manage-server#manage-server-menu)  
       * [Configuration options](../../studio/server/manage-server#manage-server-configuration-options)  
       * [Debug options](../../studio/server/manage-server#debug-debugging-and-statistics-options)  

{NOTE/}

---

{PANEL: Manage Server menu}

Open the **Manage Server** menu from Studio's main menu.  

![Main menu: Click to manage server](images/manage-server_click-to-manage-server.png "Main menu: Click to manage server")

---

The menu is divided into two main sections, one to configure various server 
options and the second to debug the server and view various statistics.  

![Manage Server menu](images/manage-server.png "Manage Server menu")

### 1. Manage Server: Configuration options

* [Cluster](../../studio/cluster/cluster-view)  
  View and modify your cluster topology.  
* [Client Configuration](../../studio/server/client-configuration)  
  Set a client configuration that applies to all the databases in the cluster.  
* [Studio Configuration](../../studio/database/settings/studio-configuration#studio-configuration---server-wide)  
  Set server-wide Studio-related options, applied to all the databases hosted by this server.  
* [Server Settings](../../studio/server/server-settings)  
  Display settings common to all the databases hosted by this server.  
* [Admin JS Console](../../studio/server/debug/admin-js-console)  
  Use this console to run JavaScript code, to execute advanced operations on the server.  
* [Certificates](../../studio/server/certificates/server-management-certificates-view)  
  Export, Import and Customization options for server and client certificates.  
* **Server-Wide Tasks**  
  Define server-wide 
  [External Replication](../../studio/database/tasks/ongoing-tasks/external-replication-task) 
  and [Periodic Backup](../../studio/database/tasks/backup-task) 
  tasks that apply to all the databases hosted by this server.  
* [Server-Wide Analyzers](../../studio/database/settings/custom-analyzers#server-wide-custom-analyzer-view)  
  Add server-wide custom analyzers, to split index fields into tokens 
  that can be used by indexes on all the databases hosted by this server.  
* [Server-Wide Sorters](../../studio/database/settings/custom-sorters#server-wide-custom-sorter-view)  
  Add server-wide custom sorters, to define how query results are ordered 
  by queries made on all the databases hosted by this server.  

### 2. Debug: Debugging and statistics options

* [Admin Logs](../../studio/server/debug/admin-logs)  
  View a live stream of RavenDB log data, and modify logging settings.  
* **Traffic Watch**  
  View the HTTP requests made to the server.  
* **Gather Debug Info**  
  Collect diagnostics data from selected or all databases on this 
  server or the entire cluster, to help troubleshoot and resolve issues.
* **Storage Report**  
* **IO Stats**  
  View, Export, and Import I/O statistics.  
* **Stack Traces**  
  Capture debugging stack traces for this server or the entire cluster.  
* **Running Queries**  
* **Advanced**  
  Advanced debug options, including the 
  [Cluster Observer](../../studio/server/debug/advanced/cluster-observer).  

{PANEL/}
