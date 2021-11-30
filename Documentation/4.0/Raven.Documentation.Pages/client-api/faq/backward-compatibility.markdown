# FAQ: Backward Compatibility
---

{NOTE: }

* RavenDB versions are released in **Major** releases like 4.0 and 5.0, 
  that are complemented over time by **Minor** releases like 5.1 and 5.2.  

* This article explains which RavenDB Clients are compatible with which 
  RavenDB Servers, and advises about the right way to upgrade.  

* In this page:  
  * [Client/Server Compatibility](../../client-api/faq/backward-compatibility#client/server-compatibility)  
     * [Compatibility Up to RavenDB 4.1](../../client-api/faq/backward-compatibility#compatibility-up-to-ravendb-4.1)  
     * [RavenDB 4.2 and Higher Compatibility](../../client-api/faq/backward-compatibility#ravendb-4.2-and-higher-compatibility)  
  * [Upgrading](../../client-api/faq/backward-compatibility#upgrading)  
     * [Upgrading RavenDB Versions Up to 4.1](../../client-api/faq/backward-compatibility#upgrading-ravendb-versions-up-to-4.1)  
     * [Upgrading RavenDB Versions From 4.2 On](../../client-api/faq/backward-compatibility#upgrading-ravendb-versions-from-4.2-on)  
     * [Upgrading Order](../../client-api/faq/backward-compatibility#upgrading-order)  


{NOTE/}

---

{PANEL: Client/Server Compatibility}

## Compatibility Up to RavenDB 4.1
RavenDB **Clients** of versions lower than 4.2 are compatible with **Servers 
of the same Major version** (3.X Clients with 3.X Servers, 4.X Clients 
with 4.X Servers), and a **Minor version the same as theirs or higher**.  
E.g. -  

* `Client 3.0` is **compatible** with `Server 3.0`, because they are of the exact 
  same version.  
* `Client 4.0` is **compatible** with `Server 4.1` because they are of the same 
  Major version and the server is of a higher Minor version.  
* `Client 4.1.7` is **compatible** with `Server 4.1.6` because 
  though the client is a little newer, the server is of the same 
  Minor version (1) as the client.  
* `Client 3.0` is **not** compatible with `Server 4.0` because the 
  server is of a different Major version.  
* `Client 4.5` is **not** compatible with `Server 4.0` because the 
  server is of a lower Minor version.  

## RavenDB 4.2 and Higher Compatibility
Starting with version 4.2, RavenDB clients are compatible with 
any server of their own version **and higher**.  
E.g. -  

* `Client 4.2` is **compatible** with `Server 4.2`, `Server 4.5`, 
  `Server 5.2`, and any other server of a higher version.  

{PANEL/}

{PANEL: Upgrading}

## Upgrading RavenDB Versions Up to 4.1
Upgrading RavenDB from a version earlier than 4.2 to a higher Major version, 
requires the upgrading of the server and all clients in lockstep.  
Please visit our [migration introduction](../../migration/client-api/introduction) 
page to learn more about migrating from early versions.  

## Upgrading RavenDB Versions From 4.2 On
When RavenDB is upgraded from version 4.2 and higher, e.g. from 4.2 to 5.3, 
it is recommended - but not mandatory - to upgrade the clients, since they 
are [compatible with servers of versions higher than theirs](../../client-api/faq/backward-compatibility#ravendb-4.2-and-higher-compatibility).  

## Upgrading Order
To properly upgrade your applications and server, we advise you to upgrade the server first, 
then the clients. This way, your applications will keep working as before and you can update 
them one-by-one if needed.

{PANEL/}

## Related Articles

### Installation
- [Upgrading to a New Version](../../start/installation/upgrading-to-new-version)  

### Migration
- [Migration Introduction](../../migration/client-api/introduction)  
