# Client Configuration
---

{NOTE: }

* Configure the RavenDB client requests behavior for ***all*** databases in the cluster  

* These default values can be overwritten ***per database*** in [Client Requests Configuration - per database](../../studio/database/settings/client-configuration-per-database)  
{NOTE/}

---

{PANEL: Client Requests Configuration}

![Figure 1. Client Requests Configuration](images/client-configuration.png "Client Requests Configuration")

**1. Identity parts separator**  

  * Changes the default **separator** for automatically generated document IDs.  
    You can use any `char` except `|` (pipe).  
    Default value: `/`  

**2. Max number of requests per session**  

  * Set this number to restrict the number of requests (***Reads*** & ***Writes***) per session in the client API.  
    Default value: 30  

**3. Use Session Context for Load Balancing**  

  * Allow client sessions to select their topology by tag, 
    so they'd be able to load-balance their requests.  
  
  * Optionally, select a hash seed to fix the topology that clients would use.  

  * For a detailed explanation see: [Load Balance Behavior](../../client-api/configuration/load-balance/load-balance-behavior).

**4. Read balance behavior**  

  * Set the load-balance method that the client will use when accessing a node with ***Read*** requests.  
    The method selected will also affect the client's decision of which node to failover to in case of issues with the ***Read*** request.  
    Note: ***Write*** requests will always access the [preferred node](../../client-api/configuration/load-balance/overview#the-preferred-node) calculated by the client.  

  * Available options are:  
     * _None_  
     * _Round Robin_  
     * _Fastest Node_  

  *  For a detailed explanation about each option see: [Read Balance Behavior](../../client-api/configuration/load-balance/read-balance-behavior). 

{PANEL/}

## Related Articles

- [Requests Configuration Per Database](../../studio/database/settings/client-configuration-per-database)
- [Load Balancing Overview](../../client-api/configuration/load-balance/overview)
- [Read Balance Behavior](../../client-api/configuration/load-balance/read-balance-behavior)
- [Load Balance Behavior](../../client-api/configuration/load-balance/load-balance-behavior)
