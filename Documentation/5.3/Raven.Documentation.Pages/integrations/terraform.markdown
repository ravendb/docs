# Terraform

---

{NOTE: }

* [Terraform](https://www.terraform.io/intro/index.html) is an 
  [IaC](https://en.wikipedia.org/wiki/Infrastructure_as_code) (infrastructure 
  as Code) **provisioning tool**.  
  
* To provision services using Terraform, you need to provide the 
  Terraform application with a [configuration file](https://www.terraform.io/docs/language/index.html) 
  that defines the services topology you want to apply.  

* The Terraform configuration file is Declerative (rather than 
  Imperative), meaning you are **not** required to state any 
  actions, but only define the desired topology.  
  Terraform will check the current infrastructure state, find 
  what differs it from your configuration, and create, update, 
  or destroy services as needed to match the topology you requested.  

* RavenDB nodes can be installed and removed via Terraform both on-premise 
  and on the cloud.  

* Deploying RavenDB clusters via Terraform can automatize the deployment, 
  saving you the effort of manual management and keeping your infrastructure 
  configuration concise and easy to read.  

* In this page:  
  * [Preperations](../integrations/terraform#preparations)  
  * [Prepare a Terraform Configuration File](../integrations/terraform#prepare-a-terraform-configuration-file)  
{NOTE/}

---

{PANEL: Preparations}

To install RavenDB via Terraform:  

---

### Acquire a RavenDB license

To install RavenDB via Terraform, you need to have a RavenDB license.  

Find [here](https://ravendb.net/buy) what features are supported by 
each RavenDB license, and acquire the product that suits your needs.  

--- 

### Prepare the Destination

RavfenDB nodes can be installed via Terraform either on-premise or on the cloud.  

* **On-Premise**  
   * Prepare the servers you want to install the new nodes at.  
   * Verify that the servers can be accessed via [SSH](https://en.wikipedia.org/wiki/Secure_Shell).  

* **On the Cloud**  
   * Create cloud instances for the new RavenDB nodes on 
     a [cloud platform that supports Terraform](https://registry.terraform.io/browse/providers).  

--- 

### Download and Run the Terraform Application

The Terraform application is a CLI tool, that implements the plan 
provided to it by your configuration file.  

* Download the Terraform application [here](https://www.terraform.io/downloads.html).  
* Define a configuration file (see more below).  
* Run the application via CLI, using -  
   * [terraform plan](https://www.terraform.io/docs/cli/commands/plan.html) 
     to parse the configuration file and **create an execution plan**  
   * [terraform apply](https://www.terraform.io/docs/cli/commands/apply.html) 
     to **execute the actions proposed in the execution plan**.  
   * [terraform destroy](https://www.terraform.io/docs/cli/commands/destroy.html) 
     to **destroy the resources defined in your configuration file**.  

{PANEL/}

{PANEL: Prepare a Terraform Configuration File}

The Terraform code that is used in a configuration file is **Declerative** 
rather than **Imperative**.  
The significance of this is that you need only to define the final topology 
you're interested in, without stating what operations are needed to get there.  
Terraform will take it from there, and define, update, or remove services 
as needed to match your plan.  






{PANEL/}

##Related Articles

**Cloud**  
[Overview](cloud-overview)  
[Migration](cloud-migration)  
[Security](cloud-security)  
  
[Portal](../cloud/portal/cloud-portal)  
[Backup Tab](../cloud/portal/cloud-portal-backups-tab)  
  
[RavenDB on Burstable Instances](https://ayende.com/blog/187681-B/running-ravendb-on-burstable-cloud-instances)  
[AWS CPU Credits](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/burstable-credits-baseline-concepts.html)  

**Client-API**  
[Backup](../client-api/operations/maintenance/backup/backup)  
[Restore](../client-api/operations/maintenance/backup/restore)  

**Server**  
[Backup Overview](../server/ongoing-tasks/backup-overview)  

**Studio**  
[Backup Task](../studio/database/tasks/backup-task)  
