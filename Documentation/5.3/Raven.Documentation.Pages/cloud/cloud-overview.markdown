# NoSQL Cloud Service: Overview

{NOTE: }

Running RavenDB as a cloud service frees you from operational overhead by passing much of the administration to the hands 
of RavenDB's developers and allowing them to maintain and optimize your instances for you, take care of cluster operations 
like adding or removing nodes, and run recurring tasks like backing up your data.  

* **Our and your administration**  
  Ridding you of administrative chores that don't require your attention is complemented by providing you with 
  comfortable control over your data and database management via your account's [Portal](../cloud/portal/cloud-portal) 
  and the cloud version of our [Studio](../studio/overview) management GUI.  

* **Resources**  
  Some of your products may share their resources with other applications and users, while other products may occupy machines 
  reserved for their usage only. Read about [reserved clusters](../cloud/cloud-overview#reserved-clusters), 
  [burstable instances](../cloud/cloud-overview#burstable-instances), and [credits](../cloud/cloud-overview#budget-credits-and-throttling) 
  to understand more about your choices.  

* In this page:  
  * [Your Account](../cloud/cloud-overview#your-account)  
     - [Register](../cloud/cloud-overview#register-your-account)  
     - [Login](../cloud/cloud-overview#login-to-your-account)  
  * [RavenDB on the Cloud](../cloud/cloud-overview#ravendb-on-the-cloud)  
      * [Instances, Provisioning and RavenDB Products](../cloud/cloud-overview#instances-provisioning-and-ravendb-products)  
      * [RavenDB Tiers: Free, Development and Production](../cloud/cloud-overview#ravendb-tiers)  
      * [Burstable vs. Reserved clusters](../cloud/cloud-overview#burstable-vs.-reserved-clusters)  
      * [Budget, Credits and Throttling](../cloud/cloud-overview#budget-credits-and-throttling)  
  * [Cloud Configurations](../cloud/cloud-overview#cloud-configurations)
      * [Indexing](../cloud/cloud-overview#indexing)

{NOTE/}

---

{PANEL: What do you get?}

Some of the key reasons for running RavenDB on the cloud are -

* The high availability of a cloud service.  
* The constant monitoring and healthcare of your products and their accommodating equipment.  
* Your insulation from the cloud infrastructure's administration as we handle and continuously optimize it.  
* The high [security](../cloud/cloud-security) level.  
* Regular [backups](../cloud/cloud-backup-and-restore) to your database and settings.  
* Our [support](../cloud/portal/cloud-portal-support-tab).  

{PANEL/}

{PANEL: Your Account}

Your cloud-RavenDB account gathers your products' information, your own details, and your contracts and 
billing data. We create your account and send you a link to its [Portal](../cloud/portal/cloud-portal) 
as soon as you finish registering, so you can immediately start creating and handling your 
[cloud products](../cloud/portal/cloud-portal-products-tab).  

{PANEL/}

{PANEL: Register Your Account}

To create a new account, enter [https://cloud.ravendb.net/user/register](https://cloud.ravendb.net/user/register) 
and allow the wizard to walk you through the 4 simple stages of the registration procedure:  

1. **E-Mail**
2. **Domain**  
3. **Billing**  
4. **Summary**  

---

#### 1. E-Mail  
  !["Registration: Email Address"](images\registration-001-email.png "Registration: Email Address")  

   - Provide your email address  
   - Read the **terms & conditions** and the **privacy policy**, and click the **accept** checkbox if you accept them.  
   - Click the **Next** button  

---

#### 2. Domain  
  !["Registration: Domain Name"](images\registration-002-domain.png "Registration: Domain Name")  

   - Enter an unoccupied domain name of your choice, that would be used for your cloud products.  
     The domain name is typically your organization or project name and will be used in all your products' URLs.
     The procedure will check this name's availability, and if it's free - let you continue using the **Next** button.  
    -or-  
     If you prefer **not** to currently provide a domain name, use the **Later** button.  
     You will still be required to provide a domain name, but it will happen later on when you create your first product.  

---

#### 3. Billing  
  !["Registration: Billing"](images\registration-003-billing.png "Registration: Billing")  
  !["Registration: Billing"](images\registration-004-payment.png "Registration: Billing")  

  - Enter your billing and payment information and click the **Next** button  
    -or-  
    Click the **Skip Billing Information** button if you prefer to evaluate the cloud service for now using the free product.  

---

#### 4. Summary  
  !["Registration: Link"](images\registration-005-summary.png "Registration: Link")  

  - Verify your choices, confirm you're not a robot and click the **Sign-up** button to register your cloud account.  

---

#### Signing up will send you a message with a link to your email.  

  - The sign-up process will send a registration link to your email.
  - Click the link to enter your account. No password required.  
  - The link will expire after a while, use the [login page](../cloud/cloud-overview#login-to-your-account) to re-visit your account.  

{PANEL/}

{PANEL: Login To Your Account}

To access your account, click [https://cloud.ravendb.net/user/login](https://cloud.ravendb.net/user/login).  

---

#### Login  
  !["Login"](images\registration-006-login.png "Login")  

  - Enter the same email address you provided during registration, confirm you're not a robot and 
    click the **Send Magic Link** button  

  

* The login procedure will send you a message with a link. Click the link to enter your account, no password required.  

  {NOTE: }
   The magic link is expirable to make sure each visit is logged and keep account activity trackable and more secure.  
  {NOTE/}
  
  {NOTE: }
   An account can currently be managed by a single operator, we will soon add multiple-operators management.  
  {NOTE/}

{PANEL/}

{PANEL: RavenDB on the Cloud}

##Instances, Provisioning and RavenDB Products  

---

#### Cloud Instances  

A cloud instance is a RavenDB server, allocated for you on a cloud provider like AWS or Azure.  
Your cloud instances are fully managed by RavenDB Cloud, including security, monitoring, backups and patches.  
You get operator-level access to your RavenDB instances, and can perform most admin operations on your own.  

---

####Provisioning  

Each RavenDB product is provisioned as many instances as it requires. A production cluster for example is 
allocated three instances at a minimum, while a development server's single node is allocated a single instance.  

---

####RavenDB Products  

Your RavenDB **product** can be a single server or a multi-node cluster.  
Each server or cluster-node occupies a single cloud instance.  

Different instances are equipped to your specifications to accommodate your different products and 
may be considerably different from each other in features, capabilities, and cost. The instances of 
a basic production cluster, for example, are [burstable](../cloud/cloud-overview#burstable-instances), 
while those of a higher-grade production cluster are [reserved](../cloud/cloud-overview#reserved-clusters).  

##RavenDB Tiers  

There are three types of RavenDB cloud products. They are explained in detail in the 
[Tiers and Instances ](../cloud/cloud-instances) section, in the meantime - here are 
a few words about each.  

---

####The Free Tier  
This tier offers a single product: a single-node RavenDB cloud server designated for experiments and evaluation.  
It is operated by a low-end instance that is shared with other users and is equipped with the community subset of RavenDB features.  
You can have only one free instance in your account. If you don't use your free instance for 30 days, it will be terminated.  

---

####The Development Tier  
The development tier offers a set of server configurations that can handle increasingly 
higher CPU load. All configurations are single-node servers, but a development server is equipped 
with RavenDB's full features set.  
Though superior to a free instance, a development instance still runs a single node on shared 
hardware and is designated for development, not production.  

---

####The Production Tier
A production configuration is a full RavenDB cluster of at least three nodes. You can choose its 
components using the Portal from a set of pre-selected templates, or contact 
us and we'll custom-tailor your product for your specific production requirements, i.e. with a specific 
HW selection or a multi-region cluster.  

---

| **Tier** | **Sub-tiers** | **CPU** | **Options** | **SLA** |
| -- | -- | -- | -- | -- |
| Free | - | Very low | None | No |
| Development | Dev10-Dev70 | Very low - High | All | [Yes](../cloud/portal/cloud-portal-support-tab#support-entitlement) |
| Production | **By CPU Priority** (Basic/Standard/Performance) <br> **By Cluster Size**| Up to extra performance, high network and reserved NVMe | All | [Yes](../cloud/portal/cloud-portal-support-tab#support-entitlement) |

##Burstable vs. Reserved clusters

####Reserved Clusters  
Reserved clusters are production clusters of grade *Standard* or *Performance*. Their resources are 
pre-allocated and can be used only by them. Designed to run production systems with demanding workloads, 
these are the workhorses of the RavenDB Cloud.

---

####Burstable Instances  
Free instances, Development instances and low-end ("basic") Production clusters are operated by 
"burstable" CPUs. Such clusters are suitable for small to medium production loads but are limited 
in the total amount of resources that they can consume.  
RavenDB's burstable instance allows you to consume more resources for a short amount of time but will 
throttle operations if the cluster uses more than its fair share of system resources.  

Such clusters are useful because a RavenDB Cloud balances resource usage among the instances in the cluster.  
If you expect moderate usage (with some peaks), choosing a burstable option can allow you to save about 20% of your costs.  

{NOTE: }
Upgrading from Burstable to Reserved
The RavenDB Cloud allows you to upgrade from a **Basic** production instance to a **Standard** or 
**Performance** cluster at will, with no impact on your system availability. 
{NOTE/}

##Budget, Credits and Throttling
Burstable instances are given a **budget** of CPU credits.  
Credits are **consumed** when the instance uses computing and I/O resources, and are **accumulated**, up to a certain limit, when resources usage is low.  

* **Burstable INSTANCES and throttling**  
  When an instance's budget is consumed, its services are throttled.  
  For example, **Indexing may be delayed** and **Requests may be denied**.  
  
    {INFO: }
    We throttle by denying resources, instead of charging you more.  
    RavenDB Cloud automatically attempts to balance the load between your cluster nodes.  
    {INFO/}

* **Burstable CLUSTERS and throttling**  
  [Basic-grade production clusters](../cloud/cloud-instances#basic-grade-production-cluster) are burstable: each node of such a cluster 
  is accommodated by a burstable instance.  
  When the budget of a burstable cluster is drained or nearly so, the cluster automatically shifts the workload to cluster nodes that still have 
  credits to work with.  
  If the amount of work you're performing is large enough to drain the entire cluster's budget, it may still be throttled.  

{NOTE: }
**Use a suitable configuration**.  
The stronger your burstable instances are, the less susceptible they are to budget drainage and throttling.  
If you are regularly warned or your product is actually throttled, consider [upscaling](../cloud/cloud-scaling) its configuration.  
{NOTE/}

---

####Throttling Warnings  
Your instances will warn you when their credits are about to drain up, and inform you when throttling is performed.  
They will also perform preemptive actions to lower resources usage.  
![Throttling Is Coming](images/throttling-001-nearly-exhausted-messages.png "Throttling Is Coming")  
![Throttling Is Here](images/throttling-002-indexing-paused.png "Throttling Is Here")  

---

####How much is a CPU credit, and what's in my budget?  
A single credit represents using 100% of the CPU for a full minute out of an hour.  

You can view what's currently in your instance's budget, by adding `/debug/cpu-credits` to its URL.  
![Current Budget](images/throttling-003-remaining-credits.png "Current Budget")  
Note that the figures relate to CPU seconds; for CPU minutes, divide them by 60.  
E.g., `"MaxCredits": 8640.0` means your instance can accumulate an entitlement for up to 144 minutes of full CPU usage.  

{PANEL/}

{PANEL: Cloud Configurations}

In most cases, default configurations inherit from RavenDB On Premise [Client API Configurations](../client-api/configuration/conventions) 
and [Server Configurations](../server/configuration/configuration-options).  
See the left-side-menu for various configuration categories to find ClinetAPI and Server configurations, or use the search bar.  

Whenever configuration defaults differ in RavenDB Cloud, these configurations will be discussed in this section.

#### Setting Cloud Configuration via Studio

!["Setting Cloud Configuration via Studio"](images\configuration-setting-new-config.png "Setting Cloud Configuration via Studio")  

1. **Indexes Tab**  
   Click to see indexing options.
2. **List of Indexes**  
   Select to see the list of your current indexes.
   You can only configure static indexes, not auto-indexes. 
   Select the index for which you want to change the default settings.
3. **Configuration**  
   Scroll down and select the Configuration tab.
4. **Add customized indexing configuration**  
   Click to select configuration and change the value.  
5. **Indexing Configuration Key**  
   Paste or select the configuration key that you want to change.
6. **Value**  
   Enter the new value for this configuration.
   * **Click Save** at the top of the interface when finished.

## Indexing

---

### Max Batch Size

The factors to consider if adjusting the default max indexing batch size:

* [Size of documents](https://ravendb.net/articles/dealing-with-large-documents-100-mb#:~:text=RavenDB%20can%20handle%20large%20documents,isn't%20a%20practical%20one.)
* [Complexity of calculations](../studio/database/indexes/indexing-performance#common-indexing-issues) that static index does
* [IOPS number](../cloud/cloud-instances#a-production-cloud-cluster) (IOPS - Input/Output Operations Per Second - can be adjusted according to your neeeds.)
  !["Find IOPS Number"](images\configuration-see-iops.png "Find IOPS Number")  
   1. **Products**  
      In the Cloud Portal, click the Products tab.
   2. **IOPS**  
      Your current IOPS number is written here.  
      It can be adjusted by clicking "Change Storage". 

#### Default Cloud Configuration 

The default configuration sets batch sizes with the following adjustable formula:  

`Indexing.MapBatchSize = max(Bits.PowerOf2(iops * 5), 1024);`

* Explanation of configuration value:
    * `max` = the larger of the two following options:
    * `Bits.PowerOf2(iops * 5)` = the power of two that's larger than (machine's iops speed * 5)
      e.g. in a machine with iops of 500...500 * 2 = 2,500, so the next power of two is 2 ** 12, which is 4,096 documents.  
    * `1024` = number of documents  

In this example, 4,096 is larger than 1024, so the maximum batch size will be 4,096 documents.

{PANEL/}

##Related Articles
  
[Portal](../cloud/portal/cloud-portal)  
  
**Links**  
[Register]( https://cloud.ravendb.net/user/register)  
[Login]( https://cloud.ravendb.net/user/login)  
  
