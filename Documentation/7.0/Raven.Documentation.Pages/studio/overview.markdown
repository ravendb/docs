# Studio Overview
---

{NOTE: }

* RavenDB's **Management Studio** is installed along with the server, and can 
  be accessed via any modern browser.  
  Access Studio using the server's URL, including its port number. E.g.: `http://localhost:8080` 

* Use Studio to manage your RavenDB 
  [cluster](../studio/cluster/cluster-dashboard/cluster-dashboard-overview), 
  [servers](../studio/server/server-overview), 
  and [databases](../studio/database/databases-list-view). 
  You can observe, modify, and create [indexes](../studio/database/indexes/indexes-overview), 
  issue [queries](../studio/database/queries/query-view), 
  adjust [settings](../studio/database/settings/database-settings), 
  view [statistics](../studio/database/stats/ongoing-tasks-stats/overview) 
  and [logs](../studio/server/debug/admin-logs), 
  define [ongoing tasks](../studio/database/tasks/ongoing-tasks/general-info), 
  [import](../studio/database/tasks/import-data/import-data-file) 
  and [export](../studio/database/tasks/export-database) data, 
  and much more.

* In this page:  
  * [Accessing Studio After Setup](../studio/overview#the-cluster-dashboard)  
  * [Manage Your RavenDB Server](../studio/overview#manage-your-server)  
  * [Manage Your Databases](../studio/overview#manage-your-databases)  
  * [Header and Footer Areas](../studio/overview#header-and-footer-areas)  
{NOTE/}

---
{PANEL: Accessing Studio After Setup}

Studio can be operated using a browser, to connect and manage RavenDB.  

To connect a [secure server](../start/installation/setup-wizard#select-setup-mode), the browser is required to own a **client certificate** with 
[admin privileges](../server/security/authentication/client-certificate-usage).  
Trying to connect a secure server without a certificate 
[will fail](../server/security/common-errors-and-faq#authentication-error-occurred-using-edge).  

{NOTE: }
A certificate is **not** required when accessing an [unsecure](../start/installation/setup-wizard#unsecure-setup) 
RavenDB server (normally used only during development).  
{NOTE/}

Different browsers look for certificates in different locations.  
**Edge** looks for certificates in the OS root store.  
**Firefox** looks for certificates in the browser's root store.  
**Chrome** (version 105 and up) looks for certificates in the [browser's root store](https://blog.chromium.org/2022/09/announcing-launch-of-chrome-root-program.html).  

It is therefore important to install your certificate where your browser can find it.  

* You can generate your certificate [during setup](../start/installation/setup-wizard#secure-setup-with-a-free-let).  
  The Let's Encrypt certificates that are generated this way are automatically registered 
  where the browsers mentioned above can find them.  

* You can also obtain your certificate [elsewhere](../start/installation/setup-wizard#secure-setup-with-your-own-certificate).  
  If you use a self-signed certificate, you must make sure it is properly registered 
  where your browser can find it.  
  {NOTE: }
  To register such certificate for Firefox or Chrome, you may need to 
  [explicitly import the certificate](../server/security/common-errors-and-faq#authentication-error-occurred-using-chrome) 
  into the browser's root store.  
  {NOTE/}

{PANEL/}

---

{PANEL: The Cluster Dashboard}

* Open the **Cluster Dashboard** from Studio's main menu.  
* A [Let's get started](../studio/cluster/cluster-dashboard/cluster-dashboard-widgets#let) 
  widget makes for a great starting point If you are new to RavenDB or this is a fresh setup.  
  You can use it to set your cluster, create a first database, or learn about basic database features.  
* The dashboard is [fully customizable](../studio/cluster/cluster-dashboard/cluster-dashboard-customize), 
  allowing you to easily add, remove, and rearrange your diagnostics widgets so any visit 
  to this view would instantly reveal precisely the information you're interested in.  

{INFO: }
[Learn more about the cluster dashboard](../studio/cluster/cluster-dashboard/cluster-dashboard-overview)  
{INFO/}

![Cluster Dashboard](images/overview_cluster-dashboard.png "Cluster Dashboard")

{PANEL/}

{PANEL: Manage your Server}

To access your server's configuration and debugging options, open the **Manage Server** 
menu from Studio's main menu.  

![Main menu: Click to manage server](images/overview_click-to-manage-server.png "Main menu: Click to manage server")

---

The menu's **Cluster** option, for example, will allow you, among other options, 
to add your server to a cluster, see and change its role as a cluster node, and 
assign it CPU cores.  

{INFO: }
[Learn more about the Cluster view](../studio/cluster/cluster-view)
{INFO/}

![Manage Server menu: Cluster view](images/overview_manage-server_cluster-view.png "Manage Server menu: Cluster view")

{PANEL/}

{PANEL: Manage your Databases}

Access the **Databases View** from Studio's main menu.  
![Click to manage databases](images/overview_click-to-manage-databases.png "Click to manage databases")

This is where databases can be easily created or deleted, and where you can 
view a concise summary of database metrics and perform operations like disabling 
or enabling a database or modifying its database group.  

![Databases view](images/overview_databases-view.png "Databases view")

{PANEL/}

{PANEL: Header and Footer Areas}

![Header and Footer areas](images/overview_header-and-footer.png "Header and Footer areas")

1. **Header area**
    * **A**. **Database selector**  
      Click from different Studio views to select a database.  
    * **B**. **Search bar**  
      Search for an entity in the selected database.  
    * **C**. **UI theme selector**  
      Select the UI theme that suits you.  
    * **D**. **Notification center**  
      See and dismiss notifications.  
2. **Footer area**
    * **A**. **Help and resources**  
      Click to consult with the community, to contact support, or for documentation.  
    * **B**. **Number of Documents in the selected database**
    * **C**. **Number of Indexes**
    * **D**. **Number of Stale indexes**
    * **E**. **Database Authentication status**
    * **F**. **Local Node tag**  
      Click to observe and configure another cluster node.  
    * **G**. **Studio version**
    * **H**. **Server version**
    * **I**. **License information**
    * **J**. **Support information**
    * **K**. [RavenDB Community](https://ravendb.net/community)

{PANEL/}
