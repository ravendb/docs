# Client Configuration
---

{NOTE: }

* Configures the RavenDB client requests behavior for ***all*** databases in the cluster  

* These default values can be overwritten ***per database*** in [Client Requests Configuration - per database](../../studio/database/settings/client-configuration-per-database)  
{NOTE/}

---

{PANEL: Client Requests Configuration}

![Figure 1. Client Requests Configuration](images/client-configuration.png "Client Requests Configuration")

**1. Read Balance Behavior**  

  * Set the load-balance method that the client will use when accessing a node with ***Read*** requests.  
  The method selected will also affect the client's decision of which node to failover to in case of issues with the ***Read*** request.  
  Note: ***Write*** requests will always access the [preferred node](../../client-api/configuration/load-balance-and-failover#preferred-node) calculated by the client.  

  * Available options are:  
     * _None_  
     * _Round Robin_  
     * _Fastest Node_  

  *  For a detailed explanation about each option see: [Read Balance Behavior Options](../../client-api/configuration/load-balance-and-failover#readbalancebehavior-options)  

**2. Max number of requests per session**  

  * Set this number to restrict the number of requests (***Reads*** & ***Writes***) per session in the client API.  
  The default value is 30.  
{PANEL/}

## Related Articles

- [Requests Configuration Per Database](../../studio/database/settings/client-configuration-per-database)
- [Load Balance & Failover](../../client-api/configuration/load-balance-and-failover)
