#Cloud: Tiers and Instances
---

{NOTE: }

Use your Control Panel's [Products tab](../cloud/cloud-control-panel#the-products-tab) to raise or modify 
a free node, a development node, or a production cluster.  
Free and Development nodes, as well as Production "basic" grade clusters, are operated by 
[burstable instances](../cloud/cloud-overview#burstable-instances).  
The higher production cluster grades Standard and Performance are 
[Reserved clusters](../cloud/cloud-overview#reserved-clusters).  

* In this page:  
    * [Cloud Free Instance](../cloud/cloud-instances#cloud-free-node)  
    * [Cloud Development Node](../cloud/cloud-instances#cloud-development-node)  
    * [Cloud Production Cluster](../cloud/cloud-instances#cloud-production-cluster)  
    * [Summary](../cloud/cloud-instances#summary)  
{NOTE/}

---

{PANEL: }

### Cloud Free Node  

Free RavenDB cloud nodes are great for experiments and evaluation. They are equipped with 
the most basic configuration and capabilities, and have a single node and no SLA.  
You can have only one free node per [account](../cloud/cloud-overview#your-account).  

To raise a free cloud node, choose the **free** tier when you create your product using the 
[Products tab](../cloud/cloud-control-panel#provisioning-a-new-product).  
!["Tiers and Instances: Free"](images\tiers-and-instances-001-free.png "Tiers and Instances: Free")  

{NOTE: }
If your free instance is using more resources than the provided credit, you'll need
to pay for these resources or stop your instance. 
{NOTE/}

* A free instance is identical to a development [Dev10](../cloud/cloud-instances#cloud-development-node) instance.  
  It comes with a monthly credit of 10$, used to cover the cost of the instance and of incidentals like backup storage 
  and traffic.  
* The free instance is limited to the [community](https://ravendb.net/buy) subset of features. 
* If you don't use a free instance for over 14 days, it will be deleted.  

---

### Cloud Development Node  

A development node is equipped with all RavenDB's features, like Pull Replication and Encrypted Databases.  
You can choose its equipment out of 7 increasingly-powerful configurations, Dev10 to Dev70.  

It is perfect for development, though not for production because of its single-node configuration, burstable 
CPU usage and no SLA.  

To raise a cloud Development node, choose the **development** tier when you create your product using the 
[Products tab](../cloud/cloud-control-panel#provisioning-a-new-product).  
!["Tiers and Instances: Development"](images\tiers-and-instances-002-development.png "Tiers and Instances: Development")  

* Use the configuration slide bar to choose your development node's configuration.  
  !["Development Tier DEV10"](images\tiers-and-instances-0021-development-dev10.png "Development Tier DEV10")  
  !["Development Tier DEV70"](images\tiers-and-instances-0022-development-dev70.png "Development Tier DEV70")  

* Use the storage slide bar to choose your storage size.  
  !["Development Tier: Storage"](images\tiers-and-instances-0023-development-storage.png "Development Tier: Storage")  

---

### Cloud Production Cluster  

The default production configuration is a three-node cluster, provisioned in separate 
availability zones for maximum durability.  

!["Production Tier"](images\tiers-and-instances-003-production.png "Production Tier")  


{NOTE: }
RavenDB Cloud also offers multi-region clusters and clusters with a higher number of nodes.  
Contact Support for guidance regarding provisioning such clusters.
{NOTE/}

* The production tier offers three instance levels.
   - **Basic**  
     Basic production clusters are burstable. While suitable for low to medium workloads, 
     they trade-off peak efficency for lower costs.  
     !["Production: Basic"](images\tiers-and-instances-0031-production-basic.png "Production: Basic")  

   - **Standard** production clusters have reserved resources they can utilize at all time, and are 
     equipped to handle constant production load.  
     !["Production: Standard P10"](images\tiers-and-instances-0032-production-standard-P10.png "Production: Standard P10")  
     !["Production: Standard P50"](images\tiers-and-instances-0033-production-standard-P50.png "Production: Standard P50")  

     * Use the slide bar to choose your Standard Production cluster's storage size.  
     !["Standard Storage"](images\tiers-and-instances-0034-production-standard-storage.png "Standard Storage")  

   - Performance  
     Performance instances are reserved, and we can **custom-tailor** them for your production environment 
     so they include High memory, reserved IO with NVMe drives, High compute, and up to 2 terabytes of storage space.  
     !["Production: Performance"](images\tiers-and-instances-0034-production-performance.png "Production: Performance")  

{NOTE: }
The production tier is **highly customizable**.  
Contact Support to modify your server configuration, the number of nodes in your cluster, 
and various parameters of their deployment.  
If you're interested in a high global availability for example, a multi-region deployment 
would probably suit you better than the default.  
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
| Development | Dev0-Dev6 | Very low - High | All | [Yes](../cloud/cloud-control-panel#the-support-tab) |
| Production | **By CPU Priority** (Basic/Standard/Performance) <br> **By Cluster Size**| Up to extra performance, high network and reserved NVMe | All | [Yes](../cloud/cloud-control-panel#the-support-tab) |


{PANEL/}
