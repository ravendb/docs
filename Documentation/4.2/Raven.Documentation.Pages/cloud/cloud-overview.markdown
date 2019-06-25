# RavenDB on the Cloud: Overview
---

{NOTE: }

Running RavenDB on the cloud provides you with cluster instances that are managed and taken care of by 
the same committed team that develops it, now making sure it would run properly and provide its best performance.  

* This is what you get.
   * Your cloud instances and their selected capabilities are always available for you and your users.  
   * You can manage RavenDB as you would off-cloud, e.g. create and remove databases.  
   * We monitor your instances and the machines that accomodate them and take care of their health.  
   * Your data is regularly backed up.  
   * You are provided with the support level you've subscribed for.  

* In this page:  
  * [Registering and Logging In](../cloud/cloud-overview#registering-and-logging-in)  
  * [Burstable and Dedicated CPUs and the Credits system](../cloud/cloud-overview#burstable-and-dedicated-cpus-and-the-credits-system)
{NOTE/}

---

{PANEL: Registering and Logging-in}

The first step in running RavenDB on the cloud, is creating your account.  
Then you can enter your Control Panel, create and approach your instances.  

* Create an account  
   - Enter [cloud.ravendb.net](cloud.ravendb.net)  
   - Provide your email address  
   - Provide a domain name for your cloud instances  
   - Provide Billing Payment details, or click "Skip Billing Information" if you want to evaluate the cloud service first.  
   - See your choices summary, and either confirm or return to previous screens to fix what you want.  

* Entering your account  
  When you complete the registration procedure, a link will be sent to the email address you have provided. 
  Click it to access your account and Control Panel.  
  Using a link to enter your account takes care that each visit would be logged, keeping your activity 
  trackable and more secure.  
  {NOTE: }
  Future feature: Multiple Account Administrators  
  In the near future multiple users will be able to log into the same account.  
  They will all be granted full access and administration permissions.  
  {NOTE/}

* Your Control Panel comprises four tabs, by which you can manage various respects of your cloud instances' activity. 
  They are -  
   * Products  
   * Billing  
   * Support  
   * Account  

{PANEL/}

{PANEL: Burstable and Dedicated CPUs and the Credits system}

* Burstable instances  
  Free instances, Development instances and low-end ("basic") Production instances are operated by 
  "burstable" CPUs, that are shared by multiple users. Each user can usually use only a part of the 
  CPU's time and power.  
* Credits  
  Each such instance is given **credits**, that are consumed when it uses the CPU and are renewed and 
  even multiplied (up to a certain limit) over time and during periods of low instance CPU usage. 
  When an instance's credits are consumed its services are throttled, e.g. indexing may be delayed and
  requests may be denied.  
* Dedicated CPUs  
  Production instances of higher grades (**Standard** and **Performance**) are operated by CPUs 
  100% dedicated to them. They do not share their resources with other applications and do not need 
  to use credits.  

{PANEL/}

## Related articles
**Studio Articles**:  
[xxx](../../../xxx)  

**Client-API Articles**:  
[xxx](../../../xxx)  
