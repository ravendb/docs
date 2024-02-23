# Cloud: Scaling
---

{NOTE: }

After a RavenDB [cloud instance](../cloud/cloud-instances) has been set up, you can **scale it** up and down to modify the 
workload it can handle.  

* Instances of the [development](../cloud/cloud-instances#a-development-cloud-server) and 
  [production](../cloud/cloud-instances#a-production-cloud-cluster) tiers can be upscaled 
  or downscaled **within their own tier**.  
* An instance can't be converted to a different tier, but data can be [migrated](cloud-migration) between any 
  two RavenDB instances.  

* In this page:  
  * [Scaling](../cloud/cloud-scaling#scaling)  
     - [Change instance type](../cloud/cloud-scaling#change-instance-type)  
     - [Change storage](../cloud/cloud-scaling#change-storage)  

{NOTE/}

---

{PANEL: Scaling}

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

**1.** Click [Change Instance Type](../cloud/cloud-scaling#change-instance-type) to reconfigure your product.  
**2.** Click [Change Storage](../cloud/cloud-scaling#change-storage) to modify your product's storage parameters.  

---

####1. Change Instance Type  

Use the **CPU Priority** and **Cluster Size** slide bars to compose a configuration 
that would allow your product to properly handle its expected workload.  

![Figure 3 - Scaling instance type](images/portal-product-details-edit-tier.png "Figure 3 - Scaling instance type")

You can upscale or downscale only within the current product tier. The development-tier **Dev30** configuration,
for example, can upscale to **Dev50**, but not to the production-tier **PB10** configuration.  
Your databases and data will be automatically migrated into your new configuration.

  ---

####2. Change Storage  

There are two types of storage: **Standard** and **Premium**. Pick either to change your current storage capacity.  

![Figure 4 - Scaling storage](images/portal-product-details-edit-storage.png "Figure 4 - Scaling storage")

{INFO: }
**Standard** disk type **is not available** on **GCP** instances.
{INFO/}

The **Premium** storage type on **AWS** also lets you choose the number of IOPS (Input/Output Operations Per Second) that the instance can handle.  

![Figure 5 - Customized IOPS on AWS premium disks](images/portal-product-details-edit-storage-with-iops.png "Figure 5 - Customized IOPS on AWS premium disks")

{INFO: }
It is fairly obvious why the size of the storage matters, but it is important to also understand the impact 
of the storage type and allocated IOPS on the overall performance.  

RavenDB, as a database, is sensitive to I/O latencies resulting from slow storage. If your instances are running into 
high I/O latencies, RavenDB will alert you to the issue so you can upgrade the type of storage you are using and the 
number of IOPS reserved for your instances.  
{INFO/}


{NOTE: }
Scaling a [Development](../cloud/cloud-instances#a-development-cloud-server) product **brings it down** 
temporarily, while its single-node instance is being reconfigured.  
Scaling a [Production](../cloud/cloud-instances#a-production-cloud-cluster) product does **not** bring it down, 
because it is a multi-node cluster and the nodes are scaled in a **rolling update**, one instance at a time.  
{NOTE/}

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
