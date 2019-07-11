#Cloud: Scaling
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
     - [Change Instance Type](../cloud/cloud-scaling#change-instance-type)  
     - [Change Storage](../cloud/cloud-scaling#change-storage)  

{NOTE/}

---

{PANEL: Scaling}

To scale a RavenDB cloud instance, open your [portal](../cloud/portal/cloud-portal)'s [products tab](../cloud/portal/cloud-portal-products-tab) 
and click the **Manage** button for the product you want to scale.  

!["Manage Product"](images/scaling-001-manage.png "Manage Product")  
  
In the **General** tab, you will see buttons to **Change Instance Type** and **Change Storage**.  

{NOTE: }
The scaling buttons are presented only for [Development](../cloud/cloud-instances#a-development-cloud-server) and 
[Production](../cloud/cloud-instances#a-production-cloud-cluster) products.  
The [Free](../cloud/cloud-instances#a-free-cloud-node) product doesn't show them because its tier includes only one configuration.  
{NOTE/}

!["Scaling Buttons"](images/scaling-002-buttons.png "Scaling Buttons")  

**1.** Click **Change Instance Type** to reconfigure your product.  
**2.** Click **Change Storage** to modify your product's storage capacity.  

---

####1. Change Instance Type  

Use the **CPU Priotity** and **Cluster Size** slide bars to compose a configuration 
that would allow your product to properly handle its expected workload.  

!["Scaling Instance Type"](images/scaling-003-instance.png "Scaling Instance Type")  

  ---

####2. Change Storage  

!["Scaling Storage"](images/scaling-004-storage.png "Scaling Storage")  
  
There are two types of storage: Standard and Premium. Pick either to change your current storage capacity.  
The **Premium** storage type also lets you choose the number of IOPS (Input/Output Operations Per Second) that the instance can handle.  

!["Premium IOPS"](images/scaling-005-premium.png "Premium IOPS")  

{NOTE: }
Scaling a [Development](../cloud/cloud-instances#a-development-cloud-server) product **brings it down** 
temporarily, while its single-node's instance is being reconfigured.  
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
