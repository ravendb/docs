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

Using a browser like Edge, Firefox, or Chrome, to manage RavenDB using Studio, 
requires the browser to use a **client certificate**.  
The certificate can be generated [during setup](../start/installation/setup-wizard#secure-setup-with-a-free-let), 
or you can use a certificate [of another origin of your choice](../start/installation/setup-wizard#secure-setup-with-your-own-certificate).  
However you generate the certificate, your browser has to be able to find it.  

* Edge locates its certificates in the OS root store.  
* Firefox and Chrome (version 105 and up) locate their certificates 
  in the [browser's root store](https://blog.chromium.org/2022/09/announcing-launch-of-chrome-root-program.html).  

This means that if you use Firefox or Chrome to run Studio, you need to make sure that 
your RavenDB client certificate for Studio is properly registered with the browser store 
so Studio would be able to locate the certificate and use it to access the server.  
Failing to do so, an attempt to connect RavenDB from the browser 
[will fail](../server/security/common-errors-and-faq#authentication-error-occurred-using-chrome).  

* The Let's-Encrypt certificates that you can generate from the 
  [setup wizard](../start/installation/setup-wizard) 
  are automatically registered where the browser can find them, be it Edge, Firefox, 
  or Chrome.  
* If you use Firefox or Chrome and obtain your client certificates elsewhere, 
  you may need to [explicitly import them](../server/security/common-errors-and-faq#authentication-error-occurred-using-chrome) 
  into your browser's root store.  

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
