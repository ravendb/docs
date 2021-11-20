# Terraform

---

{NOTE: }

* [Terraform](https://www.terraform.io/intro/index.html) is an 
  [IaC](https://en.wikipedia.org/wiki/Infrastructure_as_code) (infrastructure 
  as Code) **provisioning tool**.  
  You can create a configuration file that defines your desired infrastructure, 
  and apply your design using Terraform.  

* RavenDB nodes can be installed and removed via Terraform both **on-premise** 
  and **on the cloud**.  

* Using Terraform to automatize the deployment of RavenDB nodes and clusters 
  can save you a lot of manual supervision effort overtime.  
  Your Terraform configuration represent your infrastructure clearly 
  and concisely, and can always be used to review and easily modify it.  

* In this page:  
  * [Terraform Preperations and Execution](../integrations/terraform#terraform-preperations-and-execution)  
     * [Acquire a RavenDB license](../integrations/terraform#acquire-a-ravendb-license)  
     * [Prepare Destination/s](../integrations/terraform#prepare-destinations)  
     * [Download and Run the Terraform Application](../integrations/terraform#download-and-run-the-terraform-application)  
  * [Prepare Terraform Configuration](../integrations/terraform#prepare-terraform-configuration)  
{NOTE/}

---

{PANEL: Terraform Preperations and Execution}

To deploy RavenDB via Terraform, follow these steps:  

* **Acquire a RavenDB license**  
  To install RavenDB via Terraform, you need to provide Terraform with 
  a RavenDB license that validates your product and determines which of 
  its features would be enabled.  
  Find [here](https://ravendb.net/buy) what features are supported by 
  each RavenDB license, and acquire the product that suits your needs.  

* **Prepare Destination/s**  
  You can install RavenDB via Terraform both on-premise and on the cloud.  

* **Download and Run the Terraform Application**  
   * Download the Terraform application [here](https://www.terraform.io/downloads.html).  
   * Define a Terraform configuration file (see more below).  
   * Apply your configuration using the Terraform application via CLI.  
     Learn [here](https://www.terraform.io/docs/cli/commands/plan.html) 
     to make the application parse your configuration and create an execution plan.  
     Learn [Here](https://www.terraform.io/docs/cli/commands/apply.html) 
     to execute the actions proposed in the execution plan.  
     Learn [Here](https://www.terraform.io/docs/cli/commands/destroy.html) 
     to destroy an infrastructure by your configuration.  

{PANEL/}

{PANEL: Prepare Terraform Configuration}

* A Terraform configuration file is defined in a simple [declarative language](https://www.terraform.io/docs/language/index.html).  
   * The configuration file defines your **desired infrastructure topology**.  
     It does **not** specify the **actions** required to apply this infrastructure.  

* When the configuration file is applied, Terraform will -  
   * Compare the current infrastructure state with your design.  
   * Figure out what actions need to be taken, and create an **execution plan** 
     with these actions.  
   * **Create, Update, and Destroy** infrastructure resources as 
     instructed by the execution plan, until the infrastructure state 
     matches your requested configuration.  

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
