# Terraform

---

{NOTE: }

* [Terraform](https://www.terraform.io/) is an infrastructure deployment tool 
  that installs infrastructure components and modifies existing deployments by 
  applying [user-defined configuration files](https://www.terraform.io/docs/language/index.html).  

* This article explains how to deploy RavenDB nodes and clusters on-premise 
  or on the cloud via Terraform, and modify existing clusters.  

* Deploying RavenDB via Terraform allows you to automatize the deployment 
  process and keep its consistency.  
  
* In this page:  
  * [basic configuration](../cloud/cloud-backup-and-restore#the-mandatory-backup-routine)  
{NOTE/}

---

{PANEL: Basic Configuration}

### Acquiring a RavenDB license

To install RavenDB via Terraform, you need to [acquire RavenDB first](https://ravendb.net/buy).  
Having acquired it, you will be able to provide its license code 
in your Terraform configuration file (see below) and install the 
database in a location of your choice.  

### Where can RavenDB be installed

You can install RavenDB via Terraform either on-premise or on the cloud.  
In either case, you will need to provide the **destination URL** in you 
Terraform configuration file (see below).  

### RavenDB Terraform Repository

The RavenDB The respository from which 

create & run terraform script  

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
