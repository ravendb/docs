# Cloud Portal: The Products Tab

{NOTE: }

The Products tab lets you [provision](../../cloud/cloud-overview#provisioning) a new cloud product, or manage an existing one.  

You can provision products of three types:  [Free](../../cloud/cloud-overview#the-free-tier), 
[Development](../../cloud/cloud-overview#the-development-tier) 
and [Production](../../cloud/cloud-overview#the-production-tier).  

* In this page:  
  * [Provisioning a New Product](../../cloud/portal/cloud-portal-products-tab#provisioning-a-new-product)  
  * [Managing an Existing Product](../../cloud/portal/cloud-portal-products-tab#managing-an-existing-product)  
     - [The General tab: Change Instance Type and Storage](../../cloud/portal/cloud-portal-products-tab#e.-manage-product-the-general-tab)  
     - [The Security tab: Your Certificate and Allowed IPs](../../cloud/portal/cloud-portal-products-tab#f.-manage-product-the-security-tab)  
     - [The Danger Zone tab: Terminate your Instance](../../cloud/portal/cloud-portal-products-tab#g.-manage-product--the-danger-zone-tab)  
{NOTE/}

{PANEL: Provisioning a New Product}

To provision a new product, open your Portal's Products tab and click **Add Product**.  

!["Figure 1 - Add Product"](images\portal-001.png "Figure 1 - Add Product")

The New Product wizard will open, and walk you through the following simple stages:  
  
A. **Plan**  
B. **Billing**  
C. **Customize**  
D. **Summary**  

---

####A. New Product: Plan
Use the Plan page to choose your product's **cloud provider**, **region** and **tier**.  

!["Plan Provider Region & Tier"](images\provider-region-tier-selection.png "Figure 2 - Plan Provider Region & Tier")

  

1.  **Cloud Provider**  
   Choose your cloud host. It can currently be one of three providers: **Amazon AWS**, **Microsoft Azure**, and **Google Cloud Platform**.  

2.  **Region**  
  Select where your equipment would be physically located.  
  {NOTE: }
   You can currently locate **Free** Instances only in **Amazon AWS - US East (N. Virginia) - us-east-1** region.  
  {NOTE/}
  
3.  **Tier**  
  You can raise a [Free node](../../cloud/cloud-instances#a-free-cloud-node), 
  a [Development node](../../cloud/cloud-instances#a-development-cloud-server) or a 
  [Production cluster](../../cloud/cloud-instances#a-production-cloud-cluster).  
  Learn more about them in the [Tiers and Instances](../../cloud/cloud-instances) page.  

---

####B. New Product: Billing
Enter your billing details and click Next,  
or click **Later** to evaluate the cloud service using a basic free product.  

!["Figure 3 - New Product: Billing"](images\portal-products-001-billing.png "Figure 3 - New Product: Billing")

---

####C. New Product: Customize
Choose your product's display name and allowed IP addresses.  

!["Figure 4 - New Product: Customize"](images\portal-products-002-customize.png "Figure 4 - New Product: Customize")

- The display name is simply the name by which this product would appear in your Products tab.  
- The Allowed IPs selection is important.  
  It determines which addresses would be allowed to manage your database, adding an important layer to your database security.  
  You can set it to `0.0.0.0/0`, which would allow access from any location, or you can specify certain IPs or IP ranges.  
  Be aware that you will not be able to access your instance from locations that are not specified in the allowed IPs list.  
  You can edit the list at any time through the Portal.  
  Regardless of the allowed IPs setting, your RavenDB Cloud instances will **always require** authentication using X509 
  certificates for access. The allowed IPs list limits service as an additional layer of security, but isn't the only one.  

    {WARNING: }
    Azure products **do not permit** overlapping of addresses in the Allowed IPs list.  
    If addresses in your list overlap, your product will not be created. E.g. -  
  
     - Listing both 13.64.151.161/32 and 13.74.249.156/32 is **not permitted**.  
     - Listing 0.0.0.0/0 and any other address is **not permitted**.  
     - Listing both 51.140.148.192/27 and 13.74.249.156/32 **is permitted**.  
    {WARNING/}

  ---

####D. New Product: Summary  
The Summary stage shows you your choices and lets you edit them if you wish.  

!["Figure 5 - New Product: Summary"](images\portal-products-003-summary.png "Figure 5 - New Product: Summary")

---

When you finish defining your product, the cloud provisioning routine will show you 
your new product's status until it's ready to go.  

!["Figure 6 - New Product: Provisioning"](images\portal-products-provisioning-001-setting.png "Figure 6 - New Product: Provisioning")

!["Figure 7 - New Product: Active"](images\portal-products-provisioning-002-active.png "Figure 7 - New Product: Active")

{PANEL/}

---

{PANEL: Managing an Existing Product}
  
To modify an existing product, find it in the Products tab and click its Manage button.  

!["Figure 8 - Manage Product: Manage Button"](images\portal-products-004-manage-button.png "Figure 8 - Manage Product: Manage Button")

You can set it using its three tabs:  

1. **General**  
2. **Security**  
3. **Danger Zone**  

---

#### E. Manage Product: The General tab  
You can view your configuration and change your product's instance type and storage size here.  

!["Figure 9 - Manage Product tab"](images\portal-products-005-manage-general.png "Figure 9 - Manage Product tab")

* **Change Instance Type**  
  Click this button to scale your product up or down by choosing yourself another configuration.  

!["Figure 10 - Manage Product: Scale"](images\portal-products-0051-manage-General-scale.png "Figure 10 - Manage Product: Scale")

  You can upscale or downscale only within the current product tier. The development-tier DEV30 configuration,
  for example, can upscale to Dev50, but not to the production-tier PB10 configuration.  
  Your databases and data will be automatically migrated into your new configuration.  

* **Change Storage**  
  Click this button to modify your product's storage.  

!["Figure 11 - Manage Product: Storage"](images\portal-products-0052-manage-general-storage.png "Figure 11 - Manage Product: Storage")
  
  You can allocate more disk space to your cluster (but not reduce it), and you can select Premium disks and the 
  number of IOPS reserved for them.  

---

#### F. Manage Product: The Security tab  
Use the security tab to download your [certificate](../../cloud/cloud-security) or determine which addresses are 
allowed to connect your database instance.  

!["Figure 12 - Manage Product: Security"](images\portal-products-006-manage-security.png "Figure 12 - Manage Product: Security")

* **Download**  
  Click this button to download your certificate.  
  Your product will communicate only with trusted sources. Install this certificate only on trusted machines.  

* **Edit**  
  Click this button to edit your product's list of Allowed IPs.  

!["Figure 13 - Manage Product: Edit IPs"](images\portal-products-0061-manage-security-addresses.png "Figure 13 - Manage Product: Edit IPs")

  The default setting, 0.0.0.0/0, grants access to **all** IP addresses.  
  All your RavenDB Cloud instances are secured using TLS 1.2 or 1.3 and X509 certificates, but you can increase your 
  system's security further using this in-depth security measure and restrict access to your cloud instance to 
  well-known sources, e.g. your application servers. **We recommend it.**  
  
    {NOTE: }
    Cross-instance communication **inside the cluster** is **not** subject to these restrictions.
    {NOTE/}

    {WARNING: }
    Azure products **do not permit** overlapping of addresses in the Allowed IPs list. E.g. -  
  
   - Listing both 13.64.151.161/32 and 13.74.249.156/32 is **not permitted**.  
   - Listing 0.0.0.0/0 and any other address is **not permitted**.  
   - Listing both 51.140.148.192/27 and 13.74.249.156/32 **is permitted**.  
  {WARNING/}

---

#### G. Manage Product:  The Danger Zone tab  
  Use this tab's **Terminate** button to eliminate your cluster.  

  {DANGER: }
  Terminating your instance is **irreversible**. Your data and cluster properties will be permanently lost.  
  {DANGER/}

!["Figure 14 - Manage Product: Terminate"](images\portal-products-007-manage-terminate.png "Figure 14 - Manage Product: Terminate")



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
