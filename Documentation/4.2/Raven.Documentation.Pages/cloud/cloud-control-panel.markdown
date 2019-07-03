#Cloud: Control Panel
---

{NOTE: }

* Your Control Panel gives you access to different aspects of your RavenDB Cloud account.  
   * Use the **Products** tab to provision a new product or manage an existing one.  
     A "product" is a RavenDB cloud node or cluster.  
   * Use the **Billing** tab to view your billing history and expected payments.  
   * Use the **Support** tab to contact Support regarding a general issue or a specific question or request.  
   * Use the **Account** tab to view and edit your invoices and credit card details.  

* In this page:  
  * [The Products Tab](../cloud/cloud-control-panel#the-products-tab)  
     - [Provisioning a New Product](../cloud/cloud-control-panel#provisioning-a-new-product)  
     - [Managing an Existing Product](../cloud/cloud-control-panel#managing-an-existing-product)  
  * [The Billing Tab](../cloud/cloud-control-panel#the-billing-tab)  
  * [The Support Tab](../cloud/cloud-control-panel#the-support-tab)  
  * **The Account Tab**  
{NOTE/}

---

{PANEL: The Products Tab}

The Products tab lets you provision a new cloud product, or manage an existing one.  
We use the term "provision", because your new node or cluster are allocated cloud resources.  

* You can provision products of three types: **Free**, **Development** and **Production**.  
  - There is only one type of a **Free** product.  
  - **Development** and **Production** products can be selected from a variety of configurations.  

---

#### Provisioning a New Product  

To provision a new instance, open your Control Panel's Products tab and click Add Product.  

* Control Panel
    !["Control Panel"](images\control-panel-001.png "Add Product")  

    The New Product wizard will open, and walk you through four simple stages: Plan, Billing, Customize and Summary.  

* **Product Plan**  
  Use the Plan page to choose your instance **cloud provider**, **region** and **tier**.  
  !["Products Tab Plan"](images\products-tab-plan-001.png "Products Tab Plan")  
  
   - **Cloud Provider**  
      Choose your cloud host. It can currently be one of two providers:  
       - **Amazon AWS**  
       - **Microsoft Azure**  

    - **Region**  
      Select where your equipment would be physically located.  
      {NOTE: }
       You can currently locate Free Instances only in the **US East (N. Virginia) - us-east-1** region.  
      {NOTE/}
  
    - **Tier**  
      You can create either a [Free](../cloud/cloud-instances#a-free-instance), 
      a [Development](../cloud/cloud-instances#a-development-instance) or a 
      [Production](../cloud/cloud-instances#a-production-instance) instance.  
      Learn more about them in the [Tiers and Instances](../cloud/cloud-instances#ravendb-on-the-cloud-tiers-and-instances) page.  
      In short,  
       - A **Free instance** is great for experiments and evaluation. It runs a single RavenDB node on a low-end hardware shared 
         with other users, and is equipped with a subset of RavenDB features. You can have only one free instance in your account.  
       - A **Development instance** implements RavenDB's full feature set, and its hardware can be selected out of 7 configurations 
         and handle low to high CPU load. Though superior to free instances, a development instance still runs a single node on 
         a shared hardware and is unfit for production.  
       - A **Production instance** is a full RavenDB cluster with a minimum of three nodes, running on a highly customizable 
         HW configuration in order to fit your specific production requirements. You can choose your instance composition 
         from our components list, with up to 2 TB of disk space, 192 GB of RAM and 48 cores, or contact us and we will tailor 
         your instances for your special needs, i.e. a different HW selection or a multi-region cluster.  

* Product Billing  
  Enter your billing details and click Next,  
  or click "Later" if you chose to evaluate the cloud service using a basic free product.  
  !["New Product: Billing"](images\control-panel-products-001-billing.png "New Product: Billing")  

* Customize  
  Choose your product's display name and allowed IP addresses.  
  !["New Product: Customize"](images\control-panel-products-002-customize.png "New Product: Customize")  
   - The display name is simply the name by which this product would appear in your Products tab.  
   - The Allowed IPs selection is important.  
     It determines which addresses would be allowed to manage your database, adding an important layer to your database security.  

* Summary  
  The Summary stage shows you your choices and lets you edit them if you wish.  
  !["New Product: Summary"](images\control-panel-products-003-summary.png "New Product: Summary")  

---

#### Managing an Existing Product  

To modify an existing product, find it in the Products tab and click its Manage button.
!["Manage Product: Manage Button"](images\control-panel-products-004-manage-button.png "Manage Product: Manage Button")  

   * The **General** management tab shows this product's current configuration, and lets you **modify it**.  
     !["Manage Product tab"](images\control-panel-products-005-manage-general.png "Manage Product tab")  
      - **Scale** your product up or down  
        Click **Change Instance Type** to change your product's configuration.  
        !["Manage Product: Scale"](images\control-panel-products-0051-manage-General-scale.png "Manage Product: Scale")  
        You can pick your new product's configuration only out of its current tier.  
        Your databases and data will be automatically migrated into your new configuration.  
      - Modify your product's **storage** configuration  
        Click **Change Storage** to change your product's storage configuration.  
        !["Manage Product: Storage"](images\control-panel-products-0052-manage-general-storage.png "Manage Product: Storage")  
   * The **Security** management tab lets you download your product's certificate, 
     and choose which addresses are allowed to connect this database instance.  
     !["Manage Product: Security"](images\control-panel-products-006-manage-security.png "Manage Product: Security")  
      - **Download your certificate**  
        Your product will communicate only with trusted sources.  
        Download its certificate and install it on any machine you want it to trust.  
      - **Edit this product's Allowed IPs list**  
        All your RavenDB Cloud instances are secured using TLS 1.2 and X509 certificates.  
        Further increase your system's security using an in-depth security measure, 
        and allow only well known sources such as your application servers access your RavenDB instances.  
         - Click the Allowed IPs' **Edit** button to modify the list of allowed addresses.  
           !["Manage Product: Edit IPs"](images\control-panel-products-0061-manage-security-addresses.png "Manage Product: Edit IPs")  
           The default setting, 0.0.0.0/0, grants access to **all** IP addresses.  
           Cross instance communication inside the cluster is not subject to these limits.
   * **Danger Zone**  
     Use the Danger Zone tab's Terminate button to delete your cluster.  
     Be aware that this operation is irreversible. Your cluster's data and properties will be permanently lost.  
           !["Manage Product: Terminate"](images\control-panel-products-007-manage-terminate.png "Manage Product: Terminate")  

{PANEL/}


{PANEL: The Billing Tab}

Use your account's Billing tab to view your present costs, past invoices and remaining credit.  

* **Costs** sums up your current expenses over your products.  
* **Past Invoices** collects invoices that have been generated for you until today.  
* **Contracts** shows what remains of services you pre-paid for, e.g. the 10 months left of a yearly 
  contract that started two months ago.

{PANEL/}


{PANEL: The Support Tab}

You can use the Support tab to consult with Support regarding general issues, or issues 
specifically related to your needs, configurations or experience with your products.  

* Choose whether your issue is related to one of your products.  
   - If the issue is doesn't necessarily concern one of your products:  
      - Specify it  
        !["Support form"](images\support-001.png "Support form")  
      - Choose your support request level, according to each level's specifications.  
        !["Support Level"](images\support-002-level.png "Support Level")  
      -  Describe your support issue in your own words.  
        !["Support Message"](images\support-003-message.png "Support Message")  
      - And send us the message.  

   - If the issue specifically relates to one of your products:  
     !["Support: Product-related"](images\support-004-product-specific.png "Support: Product-related")  
      - Specify it  
      - Choose the relevant product  
      - Choose its support level  
      - Write your message in your own words  
      - And submit the form.  

Support requests are prioritized by their severity, and by the sender's support contract.  
  
* **Free**  
  The Free support package includes full availability of your cloud services, response to any connectivity issues, 
  and your participance in mailing lists and forums.  
* **Professional**  
  The Professional support level includes all the services provided for the free instance, and adds bug fixes, email 
  and phone support, and access to RavenDB’s core developers.  
* **Production**  
  Support for Production includes everything provided for lower levels and adds our 24/7 availability.  

| **Tier** | **SLA** |
| -- | -- |
| Free | No |
| Professional | Sun-Thu, 8:00-18:00 (GMT+2), Reply within a day |
| Production | Reply within 2 hours |

{PANEL/}
