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
  can save you a lot of manual supervision effort over time.  
  Your Terraform configuration represents your infrastructure clearly 
  and concisely, and can always be used to review and easily modify it.  

* In this page:  
  * [Prerequisites](../integrations/terraform#prerequisites)  
  * [Prepare Terraform Configuration](../integrations/terraform#prepare-terraform-configuration)  
     * [Provider Configuration](../integrations/terraform#provider-configuration)  
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

### Provider Configuration
Use the `provider` object to define a provider, with the following properties:  

{CODE-BLOCK: json}
provider "ravendb" {
  version = "1.0.0"
}
{CODE-BLOCK/}

{NOTE: }
Note that the version number is RavenDB's current **Terraform Plugin** 
version (and **not** the RavenDB Product version, which is defined in 
the [RavenDB Resource](../integrations/terraform#ravendb-resource) 
block **package** field).  
{NOTE/}

---

### Terraform Local Parameters
Use [local values](https://www.terraform.io/docs/language/values/locals.html) 
to define entities you can refer to when you define your [RavenDB resource](../integrations/terraform#ravendb-resource).  

{CODE-BLOCK: json}
locals {
    # Node Tags
    # The tags that will be given to cluster nodes
    # Type: set
    nodes = toset([
        "a", "b", "c"
    ])

    # Hosts IPs
    # IP addresses of host servers that RavenDB cluster nodes will be deployed to
    # Type: list(strings)
    hosts = [
        "3.95.238.149", 
        "3.87.248.150", 
        "3.95.220.189" 
    ]

    # Node IPs (For an Unsecure Setup)
    # Type: list(string)
    ravendb_nodes_urls_unsecure = [
        "http://3.95.238.149:8080", 
        "http://3.87.248.150:8080", 
        "http://3.95.220.189:8080"
    ]
  
    # Node Addresses (For a Secure Setup)
    # Type: list(string)
    ravendb_nodes_urls_secure = [
        "https://a.domain.development.run", 
        "https://b.domain.development.run", 
        "https://c.domain.development.run" 
    ]
}
{CODE-BLOCK/}

---

### RavenDB Resource
Use a [resource block](https://www.terraform.io/docs/language/resources/syntax.html) 
to define a RavenDB server, its hosts, and its properties.  
Terraform will use this resource to install (or remove) your cluster nodes.  

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
        # Default for a Secure setup: 443
        # Default for an Unsecure setup: 8080
        http_port = 8080

        # TCP port (Optional)
        # Type: int
        # Default for a Secure setup: 38888
        # Default for an Unsecure setup: 38881
        tcp_port = 38880
    }
  
    # Path to RavenDB product license (Required)
    # Type: filebase64
    license = filebase64("/path/to/license.json")
  
    # Settings defined here will override settings set by settings.json (Optional)
    settings_override = {
        "Indexing.MapBatchSize": 16384
    }
  
    # Paths to files you want to upload to the server for future usage (Optional)
    # Left side of the equation: Server path (absolute) to load to 
    # Right side of the equation: Original file path (absolute)
    assets = {
        "/path/to/file/file_name.extension" = filebase64("/path/to/file_name.extension")
    }
  
    # Credentials for Terraform to authenticate with when accessing the host (Optional)
    ssh {
        # User name 
        # Type: string
        user = "ubuntu"

        # Path to a stored access key 
        # Type: filebase64
        pem  = filebase64("/path/to/server.pem")
    }
}
  {CODE-BLOCK/}

| Entry | Role | Type | Required/Optional |
| ------------- | ----- | ---- | ---- |
| **hosts** | Host IP addresses (see [Terraform Local Parameters](../integrations/terraform#terraform-local-parameters) above) | `list` | Required |
| **database** | RavenDB Database Name <br> If the database doesn't exist, it will be created | `string` | Optional |
| **unsecured** | Setup Type <br> false => Install a Secure RavenDB setup (**Recommended**) <br> true => Install an Unsecure RavenDB setup (**Not** recommended) | `bool` | Optional |
| **certificate** | Path to server-side authentication certificate <br> The certificate has to be in pfx format | `filebase64` | Optional, for Secure setup only |
| **package.version** | RavenDB version | `string` | Required |
| **package.arch** | Processor Architecture | `string` | Optional |
| **url.list** | Nodes URLs (see [Terraform Local Parameters](../integrations/terraform#terraform-local-parameters) above) | `string` | Required |
| **url.http_port** | HTTP port <br> Default for a Secure setup: 443 <br> Default for an Unsecure setup: 8080 | `int` | Optional |
| **url.tcp_port** | TCP port <br> Default for a Secure setup: 38888 <br> Default for an Unsecure setup: 38881 | `int` | Optional |
| **license** | Path to RavenDB product license | `filebase64` | Required |
| **settings_override** | Settings defined here will override settings set by settings.json | `` | Optional |
| **assets** | Paths to files you want to upload to the server for future usage <br> Left side of the equation: Server path (absolute) to load to <br> Right side of the equation: Original file path (absolute)| `filebase64` | Optional |
| **ssh.user** | **User Name** for Terraform to authenticate with when accessing the host| `string` | Optional |
| **ssh.pem** | Path to an **Access Key** for Terraform to authenticate with when accessing the host | `filebase64` | Optional |

---

### Terraform Output Values
Use the [Output Values](https://www.terraform.io/docs/language/values/outputs.html) 
block to define values that Terraform will return after applying your configuration.  

{CODE-BLOCK: json}
# Return a list of installed RavenDB instances  
# Type: list(string)
output "public_instance_ips" {
    value = local.list
}

# Verify that a database with the defined name exists 
# Type: string
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

**Server**  
[Cluster Overview](../server/clustering/overview)



**External Links**  
[Terraform Language Documentation](https://www.terraform.io/docs/language/index.html)  
[Terraform Output Values](https://www.terraform.io/docs/language/values/outputs.html)  
[Terraform Provisioning](https://www.terraform.io/docs/cli/run/index.html)
