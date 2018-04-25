# Client Requests Configuration (Per Database)
---

{NOTE: }

* The general server [Client Configuration](../../../studio/server/client-configuration) can be overwritten per database.  
{NOTE/}

---

{PANEL: Client Requests Configuration - Per Database}

![Figure 1. Client Configuration Per Database](images/client-configuration-database-1.png "Specific Client Configuration Per Database")

* **1**. Override Server Configuration -  
      This option needs to be checked in order to be able to override the existing requests configuration.  
      If not checked, then the existing general server requests configuration will be used.  

* **2**. Check these options to actually override the existing [general server requests configuration](../../../studio/server/client-configuration).  
      If not checked, when the above 'override' is turned on, then the Effective Configuration will be: 'Client Default'.  

* **3**. Set the specific _read-balance_ method and the _max requests_ number value desired for this database.  
      For a detailed explanation about each field see: [server requests configuration](../../../studio/server/client-configuration) 
      & [Load Balance & Failover](../../../client-api/configuration/load-balance-and-failover).  

* **4**. This is the existing general server requests configuration.  

* **5**. This is the **Effective Configuration** that will actually be used.  
{PANEL/}

## Related articles

- [Requests Configuration in Cluster](../../../studio/server/client-configuration)
- [Load Balance & Failover](../../../client-api/configuration/load-balance-and-failover)
