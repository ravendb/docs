# RavenDB on the Cloud: Portal

{NOTE: }

* Your Portal gives you access to different aspects of your RavenDB Cloud account.  
   * Use the **Products** tab to provision a new product or manage an existing one.  
     A "product" is a RavenDB cloud node or cluster.  
   * Use the **Billing** tab to view your billing history and expected payments.  
   * Use the **Support** tab to contact Support regarding a general issue or a specific question or request.  
   * Use the **Account** tab to view and edit your invoices and credit card details.  

* In this page:  
  * [The Products Tab](../cloud/cloud-portal#the-products-tab)  
     - [Provisioning a New Product](../cloud/cloud-portal#provisioning-a-new-product)  
     - [Managing an Existing Product](../cloud/cloud-portal#managing-an-existing-product)  
  * [The Billing Tab](../cloud/cloud-portal#the-billing-tab)  
  * [The Support Tab](../cloud/cloud-portal#the-support-tab)  
  * [The Account Tab](../cloud/cloud-portal#the-account-tab)  
{NOTE/}

---

{PANEL: The Products Tab }

The Products tab lets you provision a new cloud product, or manage an existing one.  
We use the term "provision" to indicate that cloud resources are allocated for your 
new node or cluster.  

You can provision products of three types:  [Free](../cloud/cloud-overview#the-free-tier), 
[Development](../cloud/cloud-overview#the-development-tier) 
and [Production](../cloud/cloud-overview#the-production-tier).  

{PANEL/}

---

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
!["Products Tab Plan"](images\products-tab-plan-001.png "Products Tab Plan")  
  

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


{PANEL: The Billing Tab}

!["Billing Tab"](images\billing-tab.png "Billing Tab")  
Use your account's Billing tab to view your present costs, past invoices and remaining credit.  

* **Costs**  
  Sums up your current expenses over your products.  
* **Past Invoices**  
  Collects your invoices up to date.  
* **Contracts**  
  Shows what remains of services you pre-paid for, e.g. the 10 months left of a yearly contract you've started two months ago.  

{PANEL/}


{PANEL: The Support Tab}

!["Support Tab"](images\support-001-selection.png "Support Tab")  
Use this tab to contact RavenDB's support personnel.  
Support can help you assemble a production cluster that's correct for your needs, 
solve technical issues, or understand more about product-specific and general issues.  
You can also use the support tab to write us your feedback and suggestions.  

The root selection is between a general topic you want to consult about, and an issue you have 
with one of your products.  

1. **General Support Call**  
2. **Product-Specific Support Call**  

---

#### 1. Submit a General Support Call  

!["Support General Message"](images\support-002-general.png "Support General Message")  

A. Choose to raise a general question  
B. Choose the issue's severity: Minor, Normal, or Critical.  
C. Write your question in your own words  
D. Send us your support call


{NOTE: }
An explanation regarding each severity level (minor, normal or critical) appears as you select the level.  
Be sure you understand what each of the three stands for, so your call would be properly prioritized.  
{NOTE/}

---

#### 2. Submit a Product-Specific Support Call  

!["Support Product-Specific Message"](images\support-003-product-specific.png "Support Product-Specific Message")  

A. Choose to raise a Product-specific question  
B. Choose the product you relate to  
C. Choose the issue's severity: Minor, Normal, or Critical.  
D. Write your question in your own words  
E. Send us your support call

---

####Support Entitlement  

Your support requests will be prioritized by their severity and your product plan.  

* Support for **Free** users  
  Our Free support includes full availability of your free cloud instance services, 
  response to any connectivity issues, and your participation in mailing lists and forums.  
* **Professional**  
  We also support bug fixes, email and phone support, and access to RavenDB's core developers.  
* **Production**  
  Support is also 24/7 available for you.  

| **Tier** | **SLA** |
| -- | -- |
| Free | No |
| Professional | Sun-Thu, 8:00-18:00 (GMT+2), Reply within a day |
| Production | Reply within 2 hours |

{PANEL/}

{PANEL: The Account Tab}

!["Account Tab"](images\account-tab.png "Account Tab")  

A. The **Account** tab lets you view and edit your **Invoice** and **Payment** information.  
B. Information you add/edit here appears on the payment invoices we provide you, normally at the end of the month.  
C. Add/Edit payment methods here. The first we try to charge is always the one you choose as 
   [Active](../cloud/cloud-pricing-payment-billing#credit-card).  
D. Should charging the active payment method fail, we'll continue trying through your list.  

{PANEL/}


##Related Articles

**General**  
[RavenDB on Burstable Instances](https://ayende.com/blog/187681-B/running-ravendb-on-burstable-cloud-instances)  
[AWS CPU Credits](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/burstable-credits-baseline-concepts.html)  
  
**Cloud**  
[Overview](../cloud/cloud-overview)  
[Tiers and Instances](../cloud/cloud-instances)  
[Pricing, Payment and Billing](../cloud/cloud-pricing-payment-billing)  
[Backup](../cloud/cloud-backup)  
[Migration](../cloud/cloud-migration)  
[Scaling](../cloud/cloud-scaling)  
[Security](../cloud/cloud-security)  
