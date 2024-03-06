# Cloud: Scaling
---

{NOTE: }

After a RavenDB [cloud instance](../cloud/cloud-instances) has been set up, you can **scale it** up and down to modify the 
workload it can handle.  

Instances of the [development](../cloud/cloud-instances#a-development-cloud-server) and
[production](../cloud/cloud-instances#a-production-cloud-cluster) tiers can be upscaled or downscaled **within their own tier**. 
An instance can't be converted to a different tier, but data can be [migrated](cloud-migration) between any 
two RavenDB instances.

* In this page:  
  * [Scaling - General](../cloud/cloud-scaling#scaling-general)  
     - [Change instance type](../cloud/cloud-scaling#1.-scaling---change-instance-type)  
     - [Change storage](../cloud/cloud-scaling#2.-scaling---change-storage)  

{NOTE/}

---

{PANEL: Scaling - General}

To scale a RavenDB cloud instance, open your [portal](../cloud/portal/cloud-portal)'s [products tab](../cloud/portal/cloud-portal-products-tab) 
and click the **Manage** button for the product you want to scale.

![Figure 1 - Manage product](images/portal-product-list-manage-button.png "Figure 1 - Manage product")

In the main section, you will see buttons to **Change Instance Type** and **Change Storage**.  

{NOTE: }
The scaling buttons are presented only for [Development](../cloud/cloud-instances#a-development-cloud-server) and 
[Production](../cloud/cloud-instances#a-production-cloud-cluster) products.  
The [Free](../cloud/cloud-instances#a-free-cloud-node) product doesn't show them because its tier includes only one configuration.  
{NOTE/}

![Figure 2 - Scaling buttons](images/portal-product-edit-storage-and-instance-type-area.png "Figure 2 - Scaling buttons")

**1.** Click [Change Instance Type](../cloud/cloud-scaling#1.-scaling---change-instance-type) to reconfigure your product.  
**2.** Click [Change Storage](../cloud/cloud-scaling#2.-scaling---change-storage) to modify your product's storage parameters.  

{INFO: }
Scaling a [Development](../cloud/cloud-instances#a-development-cloud-server) product **brings it down**
temporarily, while its single-node instance is being reconfigured.  
Scaling a [Production](../cloud/cloud-instances#a-production-cloud-cluster) product does **not** bring it down,
because it is a multi-node cluster and the nodes are scaled in a **rolling update**, one instance at a time.  
{INFO/}

{WARNING: }
**The instance can limit disk parameters**.

![Figure 3 - Instance & Disk limitations](images/portal-product-details-instance-limitations-disk-limitations.png "Figure 3 - Instance & Disk limitations")

In the figure above, the **disk Throughput** parameter is lower than the **instance Throughput**.
To avoid throttling, consider changing the instance type.
{WARNING/}

{PANEL/}
{PANEL: 1. Scaling - Change Instance Type}

Use the **CPU Priority** and **Cluster Size** slide bars to compose a configuration 
that would allow your product to properly handle its expected workload.  

![Figure 4 - Scaling instance type](images/portal-product-details-edit-tier.png "Figure 4 - Scaling instance type")

You can upscale or downscale only within the current product tier. The development-tier **Dev30** configuration,
for example, can upscale to **Dev50**, but not to the production-tier **PB10** configuration.  
Your databases and data will be automatically migrated into your new configuration.

{PANEL/}
{PANEL: 2. Scaling - Change Storage} 

There are two types of storage: **Standard** and **Premium**. Pick either to change your current storage capacity.  

![Figure 5 - Scaling storage](images/portal-product-details-edit-storage.png "Figure 5 - Scaling storage")

---

###AWS disks

####AWS Standard & Premium - Default Performance

The performance of **Standard** disks is always the same, regardless of disk size.
The parameters are **3000 IOPS** and **125 MB/s** of **Throughput**.

For **Premium** disks, the **IOPS** parameter is set to **500** by default. The **Throughput** parameter is always the same and equals **1000MB/s**.

---

####AWS Premium - IOPS customization

The **Premium** storage type on **AWS** lets you choose the number of IOPS (Input/Output Operations Per Second) that the instance can handle.

![Figure 6 - Customized IOPS on AWS premium disks](images/portal-product-details-edit-storage-with-iops.png "Figure 6 - Customized IOPS on AWS premium disks")

{INFO: }
The cost per IOPS is dependent on the selected region.
{INFO/}

---

###Azure disks

####Azure Standard & Premium - Default Performance

The performance of **Standard** disks is always the same, regardless of disk size.
The parameters are **500 IOPS** and **60 MB/s** of **Throughput**.

The performance of **Premium** disks increases with their size.

---

####Azure Premium - Azure Performance Tier customization

To handle events that temporarily require a consistently higher
level of performance, the **Premium** storage type on **Azure** lets you use a higher performance tier for as
long as you need it. You can then return to the original tier when
you no longer need the additional performance.

![Figure 7 - Azure Performance Tier on Azure premium disks](images/portal-product-details-edit-storage-with-azure-performance-tier.png "Figure 7 - Azure Performance Tier on Azure premium disks")

{INFO: }
The cost of running on a higher Performance Tier is the same as
running on a disk of size for which a given Performance Tier is the
default one.
{INFO/}

---

###GCP disks

**GCP** offers only **Premium** disk type.

{PANEL/}

##Related Articles

**Cloud**  
[Overview](cloud-overview)  
[Tiers and Instances](cloud-instances)  
[Pricing, Payment and Billing](cloud-pricing-payment-billing)  
[Migration](cloud-migration)

  
[Portal](../cloud/portal/cloud-portal)  
  
[RavenDB on Burstable Instances](https://ayende.com/blog/187681-B/running-ravendb-on-burstable-cloud-instances)  
[AWS CPU Credits](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/burstable-credits-baseline-concepts.html)  
