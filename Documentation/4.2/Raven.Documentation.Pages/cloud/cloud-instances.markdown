# RavenDB on the Cloud: Cloud Instances
---

{NOTE: }

* You can raise a cloud instance while creating your account, and create or modify an instance 
  through your Control Panel's Products tab.  
* There are three instance types: Free, Development, and Production.  

* In this page:  
    * [A Free Instance](../cloud/cloud-instances#a-free-instance)  
    * [A Development Instance](../cloud/cloud-instances#a-development-instance)  
    * [A Production Instance](../cloud/cloud-instances#a-production-instance)  
    * [Summary](../cloud/cloud-instances#summary)  
    * [Migration](../cloud/cloud-instances#migration)  
{NOTE/}

---

{PANEL: The Three Tiers}

### A Free Instance  

Free instances are great for experiments and evaluation. They are equipped with 
the most basic configuration and capabilities, and have a single node and no SLA.  
If you don't use a free instance for over 14 days, it will be deleted.  

A free instance always runs on a burstable CPU. It is given a 9$-worth monthly 
credit when you start it, for the instance itself and for incidentals like backup 
storage and traffic.  

---

### A Development Instance  

A development instance is equipped with all RavenDB options (e.g. Pull Replication), 
and you can choose its equipment level out of 7 increasingly-stronger configurations, 
Dev0 to Dev7.  

It is perfect for development, though not for production because of its single-node 
configuration, burstable CPU usage and no SLA.  

[a table with dev0 to dev7 capabilities)]

---

### A Production Instance  

The default configuration for a production instance is a three-node cluster.  

* The production tier includes three instance levels.
   - Basic  
     Basic production instances are burstable.  
     Calculate your needs carefully, and make sure you notice it when your needs 
     overgrow the credits system and require dedicated machines.  
   - Standard  
     Standard production instances are dedicated. They can use 100% of their CPU's 
     time and power, need no credits and are threatened by no throttling.  
   - Performance  
     Performance instances are dedicated, and we can **custom-tailor** them for your production 
     so they include High memory, Dedicated IO with NVMe drives, High compute, and up to 2 terabytes 
     of storage space.  

* Deployment  
  Our default configuration is a three-node cluster, located in different availability zones of 
  the same region. E.g. the whole cluster is in a data center in central London, with each node in 
  a different building.  

  {NOTE: }
  The production tier is **highly customizable**. You can approach our support to modify 
  your server configuration, the number of nodes in your cluster, and their deployment.  
  If you're interested in a high global availability for example, a multi-region deployment 
  would probably suit you better.  
  Contact our support to custom-tailor your configuration and consult you with whatever you need 
  (e.g. help you locate an appropriate router and set your infrastructure environment variables).  
  {NOTE/}

  ---

### Summary  

The three instance types differ from each other in purpose, capabilities and cost.  

| **Tier** | **Cores** | **Nodes** | **RAM** | **Storage** |
| -- | -- | -- | -- | -- |
| Free | 2 | 1 | 0.5 GB | 10 GB |
| Development | 2-8 | 1 | 0.5 - 32 GB | Up to 1 TB |
| Production | Up to 48 | 3 (default) | Up to 192 GB | Up to 2 TB |

| **Tier** | **Sub-tiers** | **CPU** | **Options** | **SLA** |
| -- | -- | -- | -- | -- |
| Free | - | Very low | None | No |
| Development | Dev0-Dev6 | Very low - High | All | [Yes](../cloud/cloud-control-panel#support-tab) |
| Production | **By CPU Priority** (Basic/Standard/Performance) <br> **By Cluster Size**| Up to extra performance, high network and dedicated NVMe | All | [Yes](../cloud/cloud-control-panel#support-tab) |


{PANEL/}


{PANEL: Migration}

(preliminary)

Instances can be upgraded or downgraded, **within each tier**.
A Dev2 instance for example, can be upgraded to Dev5 because both are Development Instances. 
It cannot be upgraded to a Production instance though, nor downgraded to a Free instance.

Migrate one instance into another from within the instances.  
* Run both instances (the one you want to migrate from and the one you want to migrate to).
* From your new instance, use the "import from RavenDB instances" option.

{NOTE: }
You also need to export the old instance's certificate so the new instance would recognize it and be able to retrieve its data.
[elaborate & explain how]
{NOTE/}

{PANEL/}


## Related articles
**Studio Articles**:  
[xxx](../../../xxx)  

**Client-API Articles**:  
[xxx](../../../xxx)  
