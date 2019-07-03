#Cloud: Overview
---

{NOTE: }

Running RavenDB as a cloud service takes much of its administration from your hands and passes it to 
RavenDB's developers, allowing them to maintain and optimize your instances for you, take care of cluster 
operations like adding or removing nodes, and run daily tasks like automatic data backup.  

Ridding you of administrative chores is complemented by giving you full control over your data and 
database management, using your account's [Control Panel](../cloud/cloud-control-panel) and a cloud 
version of our [Studio](../studio/overview) management GUI.  

* What do you get?
   * The high availability of a cloud service.  
   * We constantly monitor your products and equipment and take care of their health.  
   * We insulate you from the cloud infrastructure and optimize it for you.
   * High-level security
   * Regular backup
   * Our [support](../cloud/cloud-control-panel#the-support-tab)

* In this page:  
  * [Your Account](../cloud/cloud-overview#your-account)  
     - [Register Your Account](../cloud/cloud-overview#register-your-account)  
     - [Log-in To Your Account](../cloud/cloud-overview#log-in-to-your-account)  
  * [Burstable vs. Rerserved clusters](../cloud/cloud-overview#burstable-vs.-rerserved-clusters)
     - [Reserved Cluster](../cloud/cloud-overview#reserved-clusters)  
     - [Burstable Instance](../cloud/cloud-overview#burstable-instances)  
     - [Upgrading from Burstable to Reserved](../cloud/cloud-overview#upgrading-from-burstable-to-reserved)  
     - [Credits](../cloud/cloud-overview#credits)  
{NOTE/}

---

{PANEL: Your Account}

Your RavenDB-cloud account gathers your own details, information regarding your products, and billing data.  
We create your account and send you a link to its [Control Panel](../cloud/cloud-control-panel) as soon as you finish registering.  
You can use your Control Panel to create and manage [products](../cloud/cloud-control-panel#the-products-tab) (cloud nodes or clusters), 
view your billing info, and contact Support.  

---

### Register Your Account  

* To create a new account, enter [https://cloud.ravendb.net/user/register](https://cloud.ravendb.net/user/register).  
   - Provide your email address,  
     Click the checkbox to accept,  
     and click the **Next** button.  
     !["Registration: Email Address"](images\registration_001.png "Registration: Email Address")  
   - Enter an unoccupied domain name of your choice for your cloud nodes and clusters.  
    !["Registration: Domain Name"](images\registration_002.png "Registration: Domain Name")  
   - Enter your billing details and click Next,  
     or click "Skip Billing Information" if you prefer to evaluate the cloud service using a basic free product.  
    !["Registration: Billing"](images\registration_003.png "Registration: Billing")  
   - Examine your choices, and either confirm or return to fix them.  
    !["Registration: Link"](images\registration_004.png "Registration: Link")  
   - Check your email, and use the link the setup procedure sent you.  

---

### Log-in To Your Account  

*  To access your account, open [https://cloud.ravendb.net/user/login](https://cloud.ravendb.net/user/login) 
   and enter the email address you provided during registration.  
   !["Login"](images\registration-006-login.png "Login")  
   - Check your email box, and use the link the login routine sent you.  
     

  {NOTE: }
   The link is temporary, we use expirable links to make sure each visit is logged and to keep your activity 
   trackable and more secure.  
  {NOTE/}
  
  {NOTE: }
   Operating an account is currently limited to a single operator.  
   We will add multiple-operators access in the near future.  
  {NOTE/}

{PANEL/}

{PANEL: Burstable vs. Rerserved clusters}

####Reserved Clusters

Reserved clusters are production clusters of grade *Standard* or *Performance*. Their resources are 
pre-allocated, and can be used only by them. Designed to run production systems with demanding workloads, 
these are the workhorses of the RavenDB Cloud.

---

####Burstable Instances

Free instances, Development instances and low-end ("basic") Production clusters are operated by 
"burstable" CPUs. Such clusters are suitable for small to medium production loads but are limited 
in the total amount of resources that they can consume.  
RavenDB's burstable instance allows you to consume more resources for short amount of time, but will 
throttle operations if the cluster uses more than its fair share of system resources.  

Such clusters are useful because a RavenDB Cloud balances resource usage among the instances in the cluster.  
If you expect moderate usage (with some peaks), choosing a burstable option can allow you to save about 20% of your costs.  

---

{NOTE: }
####Upgrading from Burstable to Reserved
The RavenDB Cloud allows you to upgrade from a **Basic** production instance to a **Standard** or 
**Performance** cluster at will, with no impact on your system availability. 
{NOTE/}

---

####Credits

Each RavenDB instance in a burstable cluster is given **credits**, that are consumed when it uses-up its 
computing and I/O resources. In periods of low resource usage on the other hand, credits are accumulated 
(up to a certain limit).  

When an instance's credits are consumed, its services are throttled. E.g. indexing may be delayed and
requests may be denied. RavenDB will automatically shift work between nodes in the cluster as their 
credits are drained, but if the amount of work you're performing enough to drain the entire cluster's
budget, you may be throttled. 

{PANEL/}
