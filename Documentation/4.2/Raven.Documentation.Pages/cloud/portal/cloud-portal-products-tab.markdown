# Cloud Portal: The Products Tab

{NOTE: }

The Products tab lets you [provision](../../cloud/cloud-overview#provisioning) a new cloud product, or manage an existing one.  

You can provision products of three types:  [Free](../../cloud/cloud-overview#the-free-tier), 
[Development](../../cloud/cloud-overview#the-development-tier) 
and [Production](../../cloud/cloud-overview#the-production-tier).  

* In this page:  
  * [Provisioning a New Product](../../cloud/portal/cloud-portal-products-tab#provisioning-a-new-product)  
  * [View the Product Metrics](../../cloud/portal/cloud-portal-products-tab#view-the-product-metrics)  
  * [View the Cluster Health](../../cloud/portal/cloud-portal-products-tab#view-the-cluster-health)  
  * [Managing an Existing Product](../../cloud/portal/cloud-portal-products-tab#managing-an-existing-product)  
     - [Change Instance Type and Storage](../../cloud/portal/cloud-portal-products-tab#change-instance-type-and-storage)  
     - [Security: Your Certificate, Audit Logs and Allowed IPs](../../cloud/portal/cloud-portal-products-tab#security-your-certificate-audit-logs-and-allowed-ips)  
     - [Manage features](#manage-features)
     - [Nodes: Additional product nodes](../../cloud/portal/cloud-portal-products-tab#nodes-additional-product-nodes)
     - [Maintenance and Danger Zones: Terminate and Restart your Instance](../../cloud/portal/cloud-portal-products-tab#maintenance-and-danger-zones-terminate-and-restart-your-instance)  
{NOTE/}

{PANEL: Provisioning a New Product}

To provision a new product, open your Portal's Products tab and click **Add Product**.

!["Figure 1 - Add Product"](images\portal-product-list-add-product-button.png "Figure 1 - Add Product")

The New Product wizard will open and walk you through the following simple stages:  
  
A. [Plan](../../cloud/portal/cloud-portal-products-tab#a.-new-product-plan)  
B. [Billing](../../cloud/portal/cloud-portal-products-tab#b.-new-product-billing)  
C. [Customize](../../cloud/portal/cloud-portal-products-tab#c.-new-product-customize)  
D. [Summary](../../cloud/portal/cloud-portal-products-tab#d.-new-product-summary)  

---

####A. New Product: Plan
Use the Plan page to choose your product's cloud **Provider**, **Region** and **Tier**.  

!["Figure 2 - Plan Provider Region & Tier"](images\provider-region-tier-selection.png "Figure 2 - Plan Provider Region & Tier")

  

1.  **Cloud Provider**  
   Choose your cloud host. It can currently be one of three providers:  
   Amazon AWS, Microsoft Azure, and Google Cloud Platform.  

2.  **Region**  
  Select where your equipment would be physically located.  

  
3.  **Tier**  
  You can raise a [Free node](../../cloud/cloud-instances#a-free-cloud-node), 
  a [Development node](../../cloud/cloud-instances#a-development-cloud-server) or a 
  [Production cluster](../../cloud/cloud-instances#a-production-cloud-cluster).  
  Learn more about them in the [Tiers and Instances](../../cloud/cloud-instances) page.  

---

####B. New Product: Billing
Enter your billing details and click Next,  
or click **Skip Billing Information** to evaluate the cloud service using a basic free product.  

!["Figure 3 - New Product: Billing"](images\portal-product-create-billing-section.png "Figure 3 - New Product: Billing")

---

####C. New Product: Customize
Choose your product's display name, release channel and allowed IP addresses.

!["Figure 4 - New Product: Customize"](images\portal-product-create-customize-section.png "Figure 4 - New Product: Customize")

The **Display Name** is simply the name that would appear in your Cloud Products tab.  
{NOTE: Important }
  The **Allowed IPs** section determines which addresses would be allowed to manage your database.  
  This adds an important layer to your database security on the network level.
  You can read more about it in the [Security](../cloud-security#access-your-product) page.
{NOTE/}
  ---

####D. New Product: Summary  
The Summary stage shows you your choices and lets you edit them if you wish.

!["Figure 5 - New Product: Summary"](images\portal-product-create-summary-section.png "Figure 5 - New Product: Summary")

---

When you finish defining your product, the cloud provisioning routine will show you 
your new product's status until it's ready to go.  

!["Figure 6 - New Product: Provisioning"](images\portal-product-list-product-creating.png "Figure 6 - New Product: Provisioning")

!["Figure 7 - New Product: Active"](images\portal-product-list-product-active.png "Figure 7 - New Product: Active")

{PANEL/}

---

{PANEL: View the Product Metrics}

!["Figure 8 - Product Metrics: General View"](images\portal-product-details-metrics.png "Figure 8 - Product Metrics: General View")

This allows you to analyse your machine resources for a selected time period and cluster node.
It's useful for analysing usage, instance overload and traffic.

{PANEL/}

---

{PANEL: View the Cluster Health}

!["Figure 9 - Cluster Health: General View"](images\portal-product-details-cluster-health.png "Figure 9 - Cluster Health: General View")

This allows you to analyse your cluster incidents for a selected *time period*, *cluster node*, *severity*, *category* and *event status*.  
In addition, **Cluster Health** generates suggestions for a selected *cluster node* based on incident trends that can help resolve cluster stability issues.  
It's useful for analysing usage, instance overload and traffic.  

More details can be found [here](../cloud-maintenance-troubleshooting#cluster-health).

{PANEL/}

---

{PANEL: Managing an Existing Product}
  
To modify an existing product, find it in the Products tab and click its Manage button.  

!["Figure 10 - Manage Product: Manage Button"](images/portal-product-list-manage-button.png "Figure 10 - Manage Product: Manage Button")

This is a general view of the product details page:

!["Figure 11 - Product Details"](images/portal-product-details.png "Figure 11 - Manage Product: Product Details")

---

### Change Instance Type and Storage  
You can view your configuration and change your product's instance type and storage size here.  

!["Figure 12 - Manage Product tab"](images\portal-product-details-instance-parameters.png "Figure 12 - Manage Product: Instance Parameters")

---

* **Change Instance Type**  
  Use sliders to set your desired configuration. This can be changed later as you need to scale to changing data processing needs.

!["Figure 13 - Manage Product: Scale"](images\portal-product-details-edit-tier.png "Figure 13 - Manage Product: Change Tier")

  More details can be found [here](../cloud-scaling#change-instance-type).

---

* **Change Storage**  
  Use this slider to modify your product's storage.  

!["Figure 14 - Manage Product: Storage"](images\portal-product-details-edit-storage.png "Figure 14 - Manage Product: Change Storage")

  More details can be found [here](../cloud-scaling#change-storage).

---
### Instance Access

* **Open Studio**  
  Click this button to open the RavenDB Studio of node A.

### Security: Your Certificate, Audit Logs and Allowed IPs  
Use the security tab to download your [certificate](../../cloud/cloud-security) or determine which addresses are 
allowed to connect your database instance.  

!["Figure 15 - Manage Product: Manage access"](images/portal-product-details-manage-access.png "Figure 15 - Manage Product: Access")

* **Download Certificate**  
  Click this button to download your certificate.  
  Your product will communicate only with trusted sources. Install this certificate only on trusted machines.  

* **Regenerate Certificate**  
  Click the dropdown button next to the *Download certificate*. Additional button to regenerate your certificate will appear.  
  If your certificate has expired, you can regenerate a new one.  
  After this operation you need to download the certificate again using **Download Certificate** button.  

  {DANGER: } 
  The previous certificate is not going to be removed by this operation. Old certificates can be
  removed using RavenDB Studio Certificates view.
  {DANGER/}

* **See audit logs**
  Click this button to view audit logs. A popup will show where you have to select a year and month.
  You can download audit logs from there.
  !["Figure 16 - See audit logs"](images/portal-product-details-audit-logs.png "Figure 16 - See audit logs")

* **Edit**  
  Click this button to edit your product's list of Allowed IPs.

  !["Figure 17 - Manage Product: Edit IPs"](images/portal-product-details-allowed-ips.png "Figure 17 - Manage Product: Edit Allowed IPs")

  {INFO: We recommend fortifying your security by allowing access only to specific IPs}
  You can increase your system's security further using this in-depth security measure and restrict access to
  trusted sources, e.g. your application servers. 
  More details can be found in the [Security](../cloud-security#access-your-product) page.
  {INFO/}

---

### Manage features

You can view features available for your product, enable, disable and configure them here.  

{NOTE: Important }
After enabling features some of them must be configured to work properly.
{NOTE/}

!["Figure 18 - Manage Features: Enable features"](images/portal-product-features-disabled.png "Figure 18 - Manage Features: Enable features")

!["Figure 19 - Manage Features: Configure or disable features"](images/portal-product-features-enabled.png "Figure 19 - Manage Features: Configure or disable features")

Available features are described on [Product Features](../cloud-features) page.

---

### Nodes: Additional product nodes

You can expand your cluster by adding *more product nodes* to your cluster. This helps improve *High Availability* and *task/load balancing*.

!["Figure 20 - Nodes: Manage additional nodes"](images/portal-product-nodes-additional-nodes.png "Figure 20 - Nodes: Manage additional nodes")

More details can be found [here](../cloud-scaling#additional-product-nodes---general).

---

### Maintenance and Danger Zones: Terminate and Restart your Instance
You can restart your product nodes, deploy additional tools and terminate your product here.

!["Figure 21 - Manage Product: Maintenance and Termination"](images\portal-product-details-maintenance-and-termination.png "Figure 21 - Manage Product: Terminate")

---

* **Maintenance Zone**  
  Use this tab to restart selected node and deploy RavenDB tools.

!["Figure 22 - Manage Product: Storage"](images\portal-product-details-maintenance-zone.png "Figure 22 - Manage Product: Maintenance Zone")

---

* **Danger Zone**  
  Use this tab's **Terminate** button to eliminate your cluster.

!["Figure 23 - Manage Product: Terminate"](images\portal-product-details-terminate-product.png "Figure 23 - Manage Product: Danger Zone")

  {DANGER: }
  Terminating your instance is **irreversible**. Your data and cluster properties will be permanently lost.  
  {DANGER/}

---

{PANEL/}

##Related Articles

[Overview](../../cloud/cloud-overview)  
  
[The Backups Tab](../../cloud/portal/cloud-portal-backups-tab)  
[The Billing & Costs Tab](../../cloud/portal/cloud-portal-billing-tab)  
[The Support Tab](../../cloud/portal/cloud-portal-support-tab)  
[The Account Tab](../../cloud/portal/cloud-portal-account-tab)  
  
**Links**  
[Register]( https://cloud.ravendb.net/user/register)  
[Login]( https://cloud.ravendb.net/user/login)  
