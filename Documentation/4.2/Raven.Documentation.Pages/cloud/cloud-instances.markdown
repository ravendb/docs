# RavenDB on the Cloud: Tiers and Instances

{NOTE: }

Use your portal's [Products tab](../cloud/portal/cloud-portal-products-tab) to raise or modify 
a Free node, a Development node, or a Production cluster.  
Free and Development nodes, as well as Production "basic" grade clusters, are operated by 
[burstable instances](../cloud/cloud-overview#burstable-vs.-reserved-clusters).  
The higher production-cluster grades [Standard](../cloud/cloud-instances#standard-grade-production-cluster) 
and [Performance](../cloud/cloud-instances#performance-grade-production-cluster) are 
[Reserved clusters](../cloud/cloud-overview#burstable-vs.-reserved-clusters).  

* In this page:  
    * [A Free Cloud Node](../cloud/cloud-instances#a-free-cloud-node)  
    * [A Development Cloud Server](../cloud/cloud-instances#a-development-cloud-server)  
    * [A Production Cloud Cluster](../cloud/cloud-instances#a-production-cloud-cluster)  
       - [Basic-grade Production Cluster](../cloud/cloud-instances#basic-grade-production-cluster)  
       - [Standard-grade Production Cluster](../cloud/cloud-instances#standard-grade-production-cluster)  
       - [Performance-grade Production Cluster](../cloud/cloud-instances#performance-grade-production-cluster)  
    * [The three tiers: a light summary](../cloud/cloud-instances#the-three-tiers-a-light-summary)  
{NOTE/}

---

{PANEL: }

### A Free Cloud Node  

Free RavenDB cloud nodes are great for experiments and evaluation. They are equipped with the 
most basic configuration and capabilities and have a single node and no Service-level agreement (SLA).  
You can run only one free node per [account](../cloud/cloud-overview#your-account).  

To raise a free cloud node, use the [Products tab](../cloud/portal/cloud-portal-products-tab) 
to create a **free**-tier instance.  
!["Tiers and Instances: Free"](images\tiers-and-instances-001-free.png "Tiers and Instances: Free")  

* A free instance is identical to a development [Dev10](../cloud/cloud-instances#a-development-cloud-server) instance.  
  It comes with a monthly credit of **10$**, used to cover the cost of the instance and of incidentals like backup storage 
  and traffic.  
  {NOTE: }
  If your free instance is using more resources than the provided credit, you'll need to pay for these resources or stop your instance. 
  {NOTE/}

* The free instance is limited to the [community](https://ravendb.net/buy) subset of features.  

* If you don't use a free instance for over 14 days, it will be deleted.  

---

### A Development Cloud Server  

A development server is equipped with all RavenDB's features, like Pull Replication and Encrypted Databases.  

Such cloud servers are perfect for development, though not for production because of their single-node configuration, 
[burstable](../cloud/cloud-overview#burstable-vs.-reserved-clusters) CPU usage and lack of 
[backup](../cloud/cloud-backup-and-restore#cloud-backup) procedure and [SLA](../cloud/portal/cloud-portal-support-tab#support-entitlement).  

To raise a cloud Development server, create a product using the Products tab and choose the **development** tier.  
!["Tiers and Instances: Development"](images\tiers-and-instances-002-development.png "Tiers and Instances: Development")  
  
---
  
####Select your server's configuration  
  
Choose from the increasingly powerful configurations Dev10 to Dev70  
!["Development Tier DEV10"](images\tiers-and-instances-0021-development-dev10.png "Development Tier DEV10")  
!["Development Tier DEV70"](images\tiers-and-instances-0022-development-dev70.png "Development Tier DEV70")  

---

####Select your storage  

Choose your storage type and size  
!["Development Tier: Storage"](images\tiers-and-instances-0023-development-storage.png "Development Tier: Storage")  

---

### A Production Cloud Cluster  

The default production configuration is a three-node cluster, provisioned in separate 
availability zones for maximum durability.  
{NOTE: }
RavenDB Cloud also offers **multi-region clusters** and clusters with a **higher number of nodes**.  
Contact Support to provision clusters with configurations that suit your needs.
{NOTE/}
!["Production Tier"](images\tiers-and-instances-003-production.png "Production Tier")  

The production tier offers three instance levels:  
1. **Basic**  
2. **Standard**  
3. **Performance**  
  
---
  
####1. Basic-grade Production Cluster
Basic production clusters are [burstable](../cloud/cloud-overview#burstable-vs.-reserved-clusters).  
While suitable for low to medium workloads, they trade-off peak efficiency for lower costs.  
!["Production: Basic"](images\tiers-and-instances-0031-production-basic.png "Production: Basic")  

---

####2. Standard-grade Production Cluster
The resources of standard production clusters are [reserved](../cloud/cloud-overview#burstable-vs.-reserved-clusters).  
The cluster can utilize them at all times, and is equipped to handle constant production load.  

Use the slide bars to choose your Standard Production cluster's configuration and storage.  
!["Production: Standard P10"](images\tiers-and-instances-0032-production-standard-P10.png "Production: Standard P10")  
!["Production: Standard P50"](images\tiers-and-instances-0033-production-standard-P50.png "Production: Standard P50")  
!["Standard Storage"](images\tiers-and-instances-0034-production-standard-storage.png "Standard Storage")  

---

####3. Performance-grade Production Cluster
Performance cloud clusters are [reserved](../cloud/cloud-overview#burstable-vs.-reserved-clusters), and we can 
**custom-tailor** them for your production environment needs so they include High memory, reserved 
IO with NVMe drives, High compute, and up to 2 terabytes of storage space.  
!["Production: Performance"](images\tiers-and-instances-0034-production-performance.png "Production: Performance")  

{NOTE: }
The production tier is **highly customizable**.  
Contact Support to modify your server configuration, the number of nodes in your cluster, 
and various parameters of their deployment.  
If you're interested in high global availability for example, a multi-region deployment 
can be easily set and would probably suit you better than the default arrangement.  
{NOTE/}

  ---

### The three tiers: a light summary  

The three instance types differ from each other in purpose, capabilities and cost.  

| **Tier** | **Cores** | **Nodes** | **RAM** | **Storage** |
| -- | -- | -- | -- | -- |
| Free | 2 | 1 | 0.5 GB | 10 GB |
| Development | 2-8 | 1 | 0.5 - 32 GB | Up to 1 TB |
| Production | Up to 48 | 3 (default) | Up to 192 GB | Up to 2 TB |

| **Tier** | **Sub-tiers** | **CPU** | **Options** | **SLA** |
| -- | -- | -- | -- | -- |
| Free | - | Very low | None | No |
| Development | Dev0-Dev6 | Very low - High | All | [Yes](../cloud/portal/cloud-portal-support-tab#support-entitlement) |
| Production | **By CPU Priority** (Basic/Standard/Performance) <br> **By Cluster Size**| Up to extra performance, high network and reserved NVMe | All | [Yes](../cloud/portal/cloud-portal-support-tab#support-entitlement) |


{PANEL/}


##Related Articles
  
[Portal](../cloud/portal/cloud-portal)  
  
[RavenDB on Burstable Instances](https://ayende.com/blog/187681-B/running-ravendb-on-burstable-cloud-instances)  
[AWS CPU Credits](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/burstable-credits-baseline-concepts.html)  

**Links**  
[Register]( https://cloud.ravendb.net/user/register)  
[Login]( https://cloud.ravendb.net/user/login)  
