# You Cloud Portal: The Products Tab

{NOTE: }

The Products tab lets you [provision](../cloud/cloud-overview#provisioning) a new cloud product, or manage an existing one.  

You can provision products of three types:  [Free](../cloud/cloud-overview#the-free-tier), 
[Development](../cloud/cloud-overview#the-development-tier) 
and [Production](../cloud/cloud-overview#the-production-tier).  

* In this page:  
  * [Provisioning a New Product](../../cloud/portal/cloud-portal-products-tab#provisioning-a-new-product)  
  * [Managing an Existing Product](../../cloud/portal/cloud-portal-products-tab#managing-an-existing-product)  
     - [The General tab: Change Instance Type and Storage](../../cloud/portal/cloud-portal-products-tab#manage-product-the-general-tab)  
     - [The Security tab: Your Certificate and Allowed IPs](../../cloud/portal/cloud-portal-products-tab#manage-product-the-security-tab)  
     - [The Danger Zone tab: Terminate your Instance](../../cloud/portal/cloud-portal-products-tab#manage-product--the-danger-zone-tab)  
{NOTE/}

{PANEL: Provisioning a New Product}

To provision a new product, open your Portal's Products tab and click **Add Product**.  
!["Add Product"](images\portal-001.png "Add Product")  
The New Product wizard will open, and walk you through four simple stages:  
  
1. **Plan**  
2. **Billing**  
3. **Customize**  
4. **Summary**  

---

####1. New Product: Plan
Use the Plan page to choose your product's **cloud provider**, **region** and **tier**.  
!["Products Tab Plan"](images\portal-products-tab-plan-001.png "Products Tab Plan")  
  

  - **Cloud Provider**  
   Choose your cloud host. It can currently be one of two providers: **Amazon AWS** and **Microsoft Azure**.  

- **Region**  
  Select where your equipment would be physically located.  
  {NOTE: }
   You can currently locate **Free** Instances only in the **US East (N. Virginia) - us-east-1** region.  
  {NOTE/}
  
- **Tier**  
  You can raise a [Free node](../cloud/cloud-instances#a-free-cloud-node), 
  a [Development node](../cloud/cloud-instances#a-development-cloud-server) or a 
  [Production cluster](../cloud/cloud-instances#a-production-cloud-cluster).  
  Learn more about them in the [Tiers and Instances](../cloud/cloud-instances) page.  

---

####2. New Product: Billing
Enter your billing details and click Next,  
or click **Later** to evaluate the cloud service using a basic free product.  
!["New Product: Billing"](images\portal-products-001-billing.png "New Product: Billing")  

---

####3. New Product: Customize
Choose your product's display name and allowed IP addresses.  
!["New Product: Customize"](images\portal-products-002-customize.png "New Product: Customize")  

- The display name is simply the name by which this product would appear in your Products tab.  
- The Allowed IPs selection is important.  
  It determines which addresses would be allowed to manage your database, adding an important layer to your database security.  

  ---

####4. New Product: Summary  
The Summary stage shows you your choices and lets you edit them if you wish.  
!["New Product: Summary"](images\portal-products-003-summary.png "New Product: Summary")  

---

When you finish defining your product, the cloud provisioning routine will show you 
your new product's status until it's ready to go.  
!["New Product: Provisioning"](images\portal-products-provisioning-001-setting.png "New Product: Provisioning")  
!["New Product: Active"](images\portal-products-provisioning-002-active.png "New Product: Active")  

{PANEL/}

---

{PANEL: Managing an Existing Product}
  
To modify an existing product, find it in the Products tab and click its Manage button.  
!["Manage Product: Manage Button"](images\portal-products-004-manage-button.png "Manage Product: Manage Button")  
You can set it using its three tabs:  

1. **General**  
2. **Security**  
3. **Danger Zone**  

---

#### 1. Manage Product: The General tab  
You can view your configuration and change your product's instance type and storage size here.  
!["Manage Product tab"](images\portal-products-005-manage-general.png "Manage Product tab")  

* **Change Instance Type**  
  Click this button to scale your product up or down by choosing yourself another configuration.  
  !["Manage Product: Scale"](images\portal-products-0051-manage-General-scale.png "Manage Product: Scale")  
  You can upscale or downscale only within the current product tier. The development-tier DEV30 configuration,
  for example, can upscale to Dev50, but not to the production-tier PB10 configuration.  
  Your databases and data will be automatically migrated into your new configuration.  

* **Change Storage**  
  Click this button to modify your product's storage.  
  !["Manage Product: Storage"](images\portal-products-0052-manage-general-storage.png "Manage Product: Storage")  

---

#### 2. Manage Product: The Security tab  
Use the security tab to download your [certificate](../cloud/cloud-security) or determine which addresses are 
allowed to connect your database instance.  
!["Manage Product: Security"](images\portal-products-006-manage-security.png "Manage Product: Security")  

* **Download**  
  Click this button to download your certificate.  
  Your product will communicate only with trusted sources. Install this certificate only on trusted machines.  

* **Edit**  
  Click this button to edit your product's list of Allowed IPs.  
  !["Manage Product: Edit IPs"](images\portal-products-0061-manage-security-addresses.png "Manage Product: Edit IPs")  
  The default setting, 0.0.0.0/0, grants access to **all** IP addresses.  
  All your RavenDB Cloud instances are secured using TLS 1.2 and X509 certificates, but you can increase your 
  system's security further using this in-depth security measure and restrict access to your cloud instance to 
  well-known sources, e.g. your application servers. **We recommend it.**  
  
    {NOTE: }
    Cross-instance communication **inside the cluster** is **not** subject to these restrictions.
    {NOTE/}

---

#### 3. Manage Product:  The Danger Zone tab  
  Use this tab's **Terminate** button to eliminate your cluster.  
  !["Manage Product: Terminate"](images\portal-products-007-manage-terminate.png "Manage Product: Terminate")  

  {NOTE: }
  Terminating your instance is **irreversible**. Your data and cluster properties will be permanently lost.  
  {NOTE/}

{PANEL/}

##Related Articles

[Overview](../../cloud/cloud-overview)  
  
[The Backups Tab](../../cloud/portal/cloud-portal-backups-tab)  
[The Billing Tab](../../cloud/portal/cloud-portal-billing-tab)  
[The Support Tab](../../cloud/portal/cloud-portal-support-tab)  
[The Account Tab](../../cloud/portal/cloud-portal-account-tab)  
  
**Links**  
[Register]( https://cloud.ravendb.net/user/register)  
[Login]( https://cloud.ravendb.net/user/login)  
