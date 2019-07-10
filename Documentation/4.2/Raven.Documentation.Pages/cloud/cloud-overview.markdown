# RavenDB on the Cloud: Overview

{NOTE: }

Running RavenDB as a cloud service takes much of its administration from your hands and passes it to those of 
RavenDB's developers, allowing them to maintain and optimize your instances for you, take care of cluster operations 
like adding or removing nodes, and run recurring tasks like backing up your data.  

* **Our and your administration**  
  Ridding you of administrative chores that don't require your attention is complemented by providing you with 
  comfortable control over your data and database management via your account's [Portal](../cloud/portal/cloud-portal) 
  and the cloud version of our [Studio](../studio/overview) management GUI.  

* **Resources**  
  Some of your products may share their resources with other applications and users, while other products may occupy machines 
  reserved for their usage only. Read about [reserved clusters](../cloud/cloud-overview#burstable-vs.-reserved-clusters), 
  [burstable instances](../cloud/cloud-overview#burstable-vs.-reserved-clusters), and [credits](../cloud/cloud-overview#credits) 
  to understand more about your choices.  

* In this page:  
  * [Your Account](../cloud/cloud-overview#your-account)  
     - [Register](../cloud/cloud-overview#register-your-account)  
     - [Login](../cloud/cloud-overview#login-to-your-account)  
  * [RavenDB on the Cloud](../cloud/cloud-overview#ravendb-on-the-cloud)  
      * [Instances, Provisioning and RavenDB Products](../cloud/cloud-overview#instances-provisioning-and-ravendb-products)  
      * [RavenDB Tiers: Free, Development and Production](../cloud/cloud-overview#ravendb-tiers)  
      * [Burstable vs. Reserved clusters](../cloud/cloud-overview#burstable-vs.-reserved-clusters)  
      * [Credits](../cloud/cloud-overview#credits)  
{NOTE/}

---

{PANEL: What do you get?}

Some of the key reasons for running RavenDB on the cloud are -

* The high availability of a cloud service.  
* The constant monitoring and healthcare of your products and their accommodating equipment.  
* Your insulation from the cloud infrastructure's administration as we handle and continuously optimize it.  
* The high [security](../cloud/cloud-security) level.  
* Regular [backups](../cloud/cloud-backup) to your database and settings.  
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

#### Signing up will send you a message with a link.  

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

A cloud instance is a virtual machine, allocated for us by a cloud provider like AWS or Azure.  
It is the role of your RavenDB Cloud provisioning procedure to approach the cloud provider you select, 
provision your instances according to your requirements, and install your RavenDB products on them.  

---

####Provisioning  

Each RavenDB product is provisioned as many instances as it requires. A production cluster, for 
example, is allocated three instances at least - one per cluster-node, while a development server's 
single node is allocated a single instance.  

---

####RavenDB Products  

Your RavenDB **product** can be a single server or a multi-node cluster.  
Each server or cluster-node occupies a single cloud instance.  

Different instances are equipped by your specifications to accommodate your different products and 
may be considerably different from each other in features, capabilities, and cost. The instances of 
a basic production cluster for example are [burstable](../cloud/cloud-overview#burstable-vs.-reserved-clusters), 
while those of a higher-grade production cluster are [reserved](../cloud/cloud-overview#burstable-vs.-reserved-clusters).  

##RavenDB Tiers  

There are three types of RavenDB cloud products. They are explained in detail in the 
[Tiers and Instances ](../cloud/cloud-instances) section, in the meantime - here are 
a few words about each.  

---

####The Free Tier  
This tier offers a single product: a single-node RavenDB cloud server designated for experiments and evaluation.  
It is operated by a low-end instance that is shared with other users and is equipped with a subset of RavenDB features.  
You can have only one free instance in your account.  

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
components using the Portal up to 2 TB of disk space, 192 GB of RAM and 48 cores, or contact 
us and we'll custom-tailor your product for your specific production requirements, i.e. with a specific 
HW selection or a multi-region cluster.  

##Burstable vs. Reserved clusters

**Reserved Clusters**  
Reserved clusters are production clusters of grade *Standard* or *Performance*. Their resources are 
pre-allocated and can be used only by them. Designed to run production systems with demanding workloads, 
these are the workhorses of the RavenDB Cloud.

**Burstable Instances**  
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

##Credits
Burstable instances are given **credits**, that are consumed when the instance consumes its computing and I/O resources.  
In periods of low resource usage on the other hand, credits are accumulated - up to a certain limit.  

{NOTE: }

When all credits of an instance are consumed, its services are throttled.  
For example, **Indexing may be delayed** and **Requests may be denied**.  

* One credit is the equivalent of a single minute per hour of 100% CPU-power.  
* We provide burstable instances with a 10$-worth monthly credit as a starter.  
* We do not charge you with money when your credits are consumed.  

{NOTE/}

* **Check your Studio warnings**  
  Do not turn your management studio's warnings off, and do check them regularly.  
  Your instances will warn you when their credits are about to drain up. They will also perform preemptive actions 
  to lower resources usage.  
* The stronger an instance is, the less prone it is to credits drainage and throttling.  
  If your instances are repeatedly throttled, consider [upscaling](../cloud/cloud-scaling).  
* **Cluster Credits**  
  Basic-grade production clusters are burstable, i.e. each of their nodes is accommodated by a burstable instance.  
  When the credits of a burstable cluster are consumed, the cluster automatically shifts the workload to nodes 
  that still have credits to work with.  
  If the amount of work you're performing is large enough to drain the entire cluster's budget, you may still be throttled.  
  
{PANEL/}

##Related Articles
  
[Portal](../cloud/portal/cloud-portal)  
  
**Links**  
[Register]( https://cloud.ravendb.net/user/register)  
[Login]( https://cloud.ravendb.net/user/login)  
  
