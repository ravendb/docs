# Studio Overview
---

{NOTE: }

* RavenDB comes with a **Management Studio** that can be accessed by any modern browser using the server URL,  
  (e.g. http://localhost:8080/ - when your server runs at port 8080).  

* The Studio lets you manage RavenDB servers, databases, and indexes. 
  You can issue queries, perform maintenance actions, view various stats graphs and logs, define tasks, import & export data, and much more in a very easy manner.

* The Studio can be accessed on any RavenDB server regardless of how it is deployed.  

* In this page:  
  * [Accessing Studio After Setup](../studio/overview#accessing-studio-after-setup)  
  * [Manage Your RavenDB Server](../studio/overview#manage-your-ravendb-server)  
  * [Manage Your Databases](../studio/overview#manage-your-databases)  
  * [Header and Footer Area](../studio/overview#header-and-footer-area)  
{NOTE/}

---
{PANEL: Accessing Studio After Setup}

Studio can be operated using a browser, to connect and manage RavenDB.  
This requires the browser to own a **client certificate** with admin privileges.  
Trying to connect RavenDB without a certificate 
[will fail](../server/security/common-errors-and-faq#authentication-error-occurred-using-edge).  

The three popular browsers are looking for certificates in different locations.  
**Edge** looks for certificates in the OS root store.  
**Firefox** looks for certificates in the browser's root store.  
**Chrome** (version 105 and up) looks for certificates in the [browser's root store](https://blog.chromium.org/2022/09/announcing-launch-of-chrome-root-program.html).  

It is therefore important to install your certificate where your browser would be 
able to find it.  

* You can generate your certificate [during setup](../start/installation/setup-wizard#secure-setup-with-a-free-let).  
  The Let's Encrypt certificates that are generated this way are automatically registered 
  where all three browsers can find them.  

* You can also obtain your certificate [elsewhere](../start/installation/setup-wizard#secure-setup-with-your-own-certificate).  
  If you use a self-signed certificate, you must make sure it is properly registered 
  where your browser can find it.  
  To register such certificate for Firefox or Chrome, you may need to 
  [explicitly import the certificate](../server/security/common-errors-and-faq#authentication-error-occurred-using-chrome) 
  into the browser's root store.  

{PANEL/}

---

{PANEL: Manage Your RavenDB Server}

![Figure 1. Studio overview - Manage server](images/overview-1.png "Manage server")

{PANEL/}

{PANEL: Manage Your Databases}

![Figure 2. Studio overview - Manage databases](images/overview-2.png "Manage databases")

{PANEL/}

{PANEL: Header and Footer Area}

![Figure 5. Studio overview - Header and Footer](images/overview-3.png "Header and Footer area")

1. **Database selector**

2. **Send feedback** & **Notification Center**

3. **Number of documents & indexes in the database**

4. **The local server node**

5. **Server & Studio versions**

{PANEL/}
