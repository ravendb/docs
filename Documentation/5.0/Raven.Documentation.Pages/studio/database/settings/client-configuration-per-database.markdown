# Client Configuration (Per Database)
---

{NOTE: }

* The general server [Client Configuration](../../../studio/server/client-configuration) can be overwritten per database.  
{NOTE/}

---

{PANEL: Client Requests Configuration - Per Database}

![Figure 1. Client Configuration Per Database](images/client-configuration-database-1.png "Specific Client Configuration Per Database")


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
  
  * Optionally, select a hash seed to randomize the topology that clients would use.  

**4. Read balance behavior**  

  * Set the load-balance method that the client will use when accessing a node with ***Read*** requests.  
    The method selected will also affect the client's decision of which node to failover to in case of issues with the ***Read*** request.  
    Note: ***Write*** requests will always access the [preferred node](../../client-api/configuration/load-balance-and-failover#preferred-node) calculated by the client.  

  * Available options are:  
     * _None_  
     * _Round Robin_  
     * _Fastest Node_  

  *  For a detailed explanation about each option see: [Read Balance Behavior Options](../../client-api/configuration/load-balance-and-failover#readbalancebehavior-options)  

{PANEL/}

{NOTE: Note}
This view will be as in the above image when a general configuration is defined.  
If a general configuration is not yet defined then this view will be similar to the general server configuration view.  
{NOTE/}

## Related Articles

- [Requests Configuration in Cluster](../../../studio/server/client-configuration)
- [Load Balance & Failover](../../../client-api/configuration/load-balance-and-failover)
