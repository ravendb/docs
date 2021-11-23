# Terraform

---

{NOTE: }

* [Terraform](https://www.terraform.io/intro/index.html) is an 
  [IaC](https://en.wikipedia.org/wiki/Infrastructure_as_code) (infrastructure 
  as Code) **provisioning tool**.  
  You can create a Terraform Configuration file that defines your desired infrastructure, 
  and apply your design using Terraform.  

* RavenDB nodes can be installed and removed via Terraform both **on-premise** 
  and **on the cloud**.  

* Using Terraform to automatize the deployment of RavenDB nodes and clusters, 
  can save you a lot of manual supervision effort overtime.  
  Your Terraform configuration represent your infrastructure clearly 
  and concisely, and can always be used to review and easily modify it.  

* In this page:  
  * [Prerequisites](../integrations/terraform#prerequisites)  
  * [Prepare Terraform Configuration](../integrations/terraform#prepare-terraform-configuration)  
     * [Provider](../integrations/terraform#provider)  
     * [Terraform Local Parameters](../integrations/terraform#terraform-local-parameters)  
     * [RavenDB Resource](../integrations/terraform#ravendb-resource)  
     * [Terraform Output Values](../integrations/terraform#terraform-output-values)  
  * [Apply Terraform Configuration](../integrations/terraform#apply-terraform-configuration)  
{NOTE/}

---

{PANEL: Prerequisites}

To deploy RavenDB using Terraform, you need:  

* **RavenDB license**  
   * Find [here](https://ravendb.net/buy) what features are supported by 
     each RavenDB license, and acquire the product that suits your needs.  
   * The license you acquire will be provided in the 
     [Terraform Configuration file](../integrations/terraform#prepare-terraform-configuration) 
     to validate your product and determine which of its features would be 
     enabled.  

* **Hosts**  
  RavenDB can be hosted by both **on-premise servers** and **cloud instances**.  
   * Make sure you have the IPs of the servers or cloud instances that are 
     intended to host RavenDB nodes.  
   * Cloud providers that support Terraform are listed in Terraform's 
     [providers list](https://registry.terraform.io/browse/providers).  

* **Terraform Configuration File**  
   * Learn [here](https://www.terraform.io/docs/language/index.html) the 
     basics of Terraform's configuration language.  
   * Read [below](../integrations/terraform#prepare-terraform-configuration) 
     how to set the configuration file to provision RavenDB.  

* **Terraform Application**  
  The Terraform application is executed via CLI to apply your infrastructure configuration.  
   * Download the application [here](https://www.terraform.io/downloads.html).  
   * Learn to use it [here](https://learn.hashicorp.com/tutorials/terraform/install-cli).  

{PANEL/}

{PANEL: Prepare Terraform Configuration}

{NOTE: }
This section explains how Terraform handles configuration files 
in general, and how to create a RavenDB configuration file.  
For a more comprehensive understanding of Terraform and its various 
options, please consult the official Terraform documentation.  
{NOTE/}

* A Terraform configuration file is defined in a simple [declarative language](https://www.terraform.io/docs/language/index.html).  
   * The configuration file defines your **desired infrastructure topology**.  
     It does **not** specify the **actions** required to apply this infrastructure.  

* When the configuration file is applied, Terraform will -  
   * Compare the current infrastructure state with your design.  
   * Figure out what actions need to be taken, and create an **execution plan**.  
   * **Create, Update, and Destroy** infrastructure resources by 
     the execution plan, until the infrastructure state matches your 
     requested configuration.  

---

### Provider
Use the `provider` object to **set RavenDB as the provider**.  

Note that the **version number** relates to the RavenDB Terraform 
Provider version, and **not** to the RavenDB Product version (that 
is defined in the [resource](../integrations/terraform#ravendb-resource) 
block's **package** field).  

{CODE-BLOCK: json}
provider "ravendb" {
  version = "1.0.0"
}
{CODE-BLOCK/}

---

### Terraform Local Parameters
Use the `locals` block to **define local configuration properties** 
that your resources definitions will be able to refer, including -  

{CODE-BLOCK: json}
locals {
    # Node Tags
    # The tags that will be given to cluster nodes.
    nodes = toset([
        "a", "b", "c"
    ])

    # Hosts IPs
    # IP addresses of host servers that RavenDB cluster nodes will be deployed to
    hosts = [
        "3.95.238.149", 
        "3.87.248.150", 
        "3.95.220.189" 
    ]

    # Node IPs
    # For an Unsecure Setup
    ravendb_nodes_urls_unsecure = [
        "http://3.95.238.149:8080", 
        "http://3.87.248.150:8080", 
        "http://3.95.220.189:8080"
    ]
  
    # Node Addresses 
    # For a Secure Setup
    ravendb_nodes_urls_secure = [
        "https://a.domain.development.run", 
        "https://b.domain.development.run", 
        "https://c.domain.development.run" 
    ]
}
{CODE-BLOCK/}

---

### RavenDB Resource
Use the `resource` block to define your RavenDB node, its hosts, and its properties.  
Terraform will use this resource to create your cluster nodes.  

{CODE-BLOCK: json}
resource "ravendb_server" "server" {

    # Host IP addresses (see Terraform Local Parameters above) (Required)
    # Type: list
    hosts = local.hosts

    # RavenDB Database Name. If the database doesn't exist, it will be created (Optional)
    # Type: string
    database = "sampleDB"

    # Setup Type (Optional)
    # false => Secure RavenDB server (Recommended)
    # true => Unsecure RavenDB server (Not Recommended)
    # Type: bool
    unsecured = false

    # Path to server-side authentication certificate (Optional, for Secure setup only)
    # The certificate has to be in pfx format
    # Type: filebase64
    certificate = filebase64("/path/to/cert.pfx")

    package {
        # RavenDB version (Required)
        # Type: string
        version = "5.2.2"

        # Processor Architecture (Optional)
        # Type: string
        arch = "arm64"
    }

    url {
        # Nodes URLs (see Terraform Local Parameters above) (Required)
        # Type: list(string)
        list = local.ravendb_nodes_urls_secure

        # HTTP port (Optional)
        # Type: int
        http_port = 8080

        # TCP port (Optional)
        # Type: int
        tcp_port = 38880
    }
  
    # Path to RavenDB product license (Required)
    # Type: filebase64
    license = filebase64("/path/to/license.json")
  
    # Settings defined here will override settings set by settings.json
    # Optional
    settings_override = {
        "Indexing.MapBatchSize": 16384
    }
  
    # Paths to files you want to upload to the server for future usage (Optional)
    # Left side of the equation: Server path (absolute) to load to 
    # Right side of the equation: Original file path (absolute)
    assets = {
        "/path/to/file/file_name.extension" = filebase64("/path/to/file_name.extension")
    }
  
    # A User name and a path to an Access Key to your server (Optional)
    ssh {
        user = "ubuntu"
        pem  = filebase64("/path/to/server.pem")
    }
}
  {CODE-BLOCK/}

---

### Terraform Output Values
Defining [Output Values](https://www.terraform.io/docs/language/values/outputs.html) 
makes Terraform return values you're interested in, after applying your configuration.  

{CODE-BLOCK: json}

# Return a list of installed RavenDB instances  
output "public_instance_ips" {
    value = local.list
}

# Verify that a database with the defined name exists 
output "database_name" {
    value = ravendb_server.server.database
}

{CODE-BLOCK/}

{PANEL/}

{PANEL: Apply Terraform Configuration}

To apply your configuration, pass your configuration file to the 
Terraform application via CLI.  

* Learn [here](https://www.terraform.io/docs/cli/commands/plan.html) 
  to make the application parse your configuration and create an execution plan.  
* Learn [Here](https://www.terraform.io/docs/cli/commands/apply.html) 
  to execute the actions proposed in the execution plan.  
* Learn [Here](https://www.terraform.io/docs/cli/commands/destroy.html) 
  to destroy your infrastructure (e.g. if you created it as 
  a temporary testing environment).  

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
