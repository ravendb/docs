# Installation: Setup Wizard Walkthrough

* The **Setup Wizard** guides you through a step-by-step installation of a RavenDB server.  

* You can use the wizard to install either a **secure** or an **unsecure** server.  
   * An **unsecure server** can be used for **trial and development**, providing there 
     is no issue with unauthorized access to the server and the data stored in it.  
   * It is **highly recommended** to use a **secure server** wherever access to the 
     server, its management, or its data should be restricted.  
   * You will also need a secure server in a development environment that requires 
     the usage of security related features like encryption and client certificates.  

* When installing a secure server, connecting it will thereafter be possible only 
  for clients that own a valid, trusted certificate.  
  To secure the server throughout its lifecycle, you can provide a suitable certificate 
  during setup. The setup wizard allows you to either -  
   * Generate and use a **Let's Encrypt certificate**,  
     saving you the bother of providing your own certificate, and making it possible 
     for RavenDB to renewal the certificate automatically from now on,  
   * -or- Provide a **self-obtained certificate**,  
     leaving you responsible for periodic certificate renewals.  

{INFO: Additional resources}

* This page explains how to follow the setup wizard, without going into security concerns details.  
  Learn more about _authentication_, _authorization_, and _security_ in RavenDB in: 
  [Security overview](../../server/security/overview)  

* Another helpful resource about setup and security: [Security common errors & FAQ](../../Server/Security/common-errors-and-faq)  

* To learn how to **install RavenDB manually** rather than via the setup wizard: [Manual setup](../../start/installation/manual)  
{INFO/}

* In this page:
   * [Run the Setup Wizard](../../start/installation/setup-wizard#run-the-setup-wizard)  
   * [Select Setup Mode](../../start/installation/setup-wizard#select-setup-mode)  
   * [Secure Setup with a Free Let's Encrypt Certificate](../../start/installation/setup-wizard#secure-setup-with-a-free-let)  
      * [Configuring The Server Addresses](../../start/installation/setup-wizard#configuring-the-server-addresses)  
      * [Installing The Certificate](../../start/installation/setup-wizard#installing-the-certificate)  
      * [Setting Up Other Nodes](../../start/installation/setup-wizard#setting-up-other-nodes)  
      * [Next Steps](../../start/installation/setup-wizard#next-steps)
   * [Secure Setup with Your Own Certificate](../../start/installation/setup-wizard#secure-setup-with-your-own-certificate)  
      * [Configuring The Server Addresses](../../start/installation/setup-wizard#configuring-the-server-addresses-1)  
      * [Setting Up Other Nodes](../../start/installation/setup-wizard#setting-up-other-nodes-1)  
      * [Next Steps](../../start/installation/setup-wizard#next-steps)
   * [Unsecure Setup](../../start/installation/setup-wizard#unsecure-setup)  

{INFO: Help Us Improve Prompt}
When you first launch RavenDB, you will see this prompt asking if you'd be willing to 
anonymously share some Studio usage data with us in order to help us improve RavenDB:  

![NoSQL Database Share Studio Usage](images/setup/help-us-improve.png "Help Us Improve")

Once you respond to this prompt, it should not appear again. However, in some scenarios, 
such as running RavenDB embedded, or working without browser cookies, the prompt may 
appear again.  

If necessary, you can add this flag to the Studio URL to prevent the prompt from 
appearing:  

`<Studio URL>#dashboard?disableAnalytics=true`

{INFO/}

---

{PANEL: Run the Setup Wizard}

To install RavenDB, you can:  

* Download the RavenDB version you want to install from the [Downloads](https://ravendb.net/download) page.  
* Extract the downloaded archive to a folder on your machine.  
* Open a command prompt (e.g. PowerShell) and navigate to the setup folder.  
* Run setup:  
   {CODE-BLOCK: plain}
   .\run.ps1
   {CODE-BLOCK/}

{PANEL/}

{PANEL:Select Setup Mode}

When running the RavenDB server for the first time, you will be redirected 
to the setup wizard welcome page where you can choose your preferred option.  

![Welcome Page](images/setup/setup-wizard-1.png "Select Mode in the Welcome Page")

{PANEL/}

{PANEL:Secure Setup with a Free Let's Encrypt Certificate}

[Let's Encrypt](https://letsencrypt.org/) is a free, automated, and non-profit certificate authority.  
It will generate a certificate for your domain as long as you can prove that you own it.  

During setup, RavenDB will provide you with a free subdomain and allow you to configure its DNS records 
with the IP addresses that your server will listen on.  
The subdomain is owned by RavenDB, and you can manage it through the [Customer Portal](https://customers.ravendb.net).  
Login with your license key, and you can add/remove/update DNS records for your cluster.

The free subdomain is given to you only for the purpose of proving ownership to Let's Encrypt.  
If you wish to use your own domain, you are welcome to acquire your own certificate and use it instead.

{WARNING: Security consideration and ownership of certificates and domains} 

The automatic setup is designed to be as convenient and as easy as possible. It takes care of all the details of setting up DNS 
records, generating certificates, and performing their renewals. Because of these requirements, the ownership of the certificates 
and DNS records needs to stay within the RavenDB company. This gives us the ability to generate valid certificates and 
modify DNS settings for your registered domains and should be a consideration to keep in mind while reviewing the security of your system.  

We will never exploit these abilities and never perform any modifications to the certificates and DNS 
records unless explicitly requested by the client.

The purpose of this feature is to make it easy for users to get set up and running with the minimum fuss.  
We recommend that **for actual production deployments** and for the highest level of security and control, 
you will [use your own certificates and domains](../../start/installation/setup-wizard#secure-setup-with-your-own-certificate), 
avoiding the need to rely on a third party for such a critical part of your security.

{WARNING/}

After choosing the Let's Encrypt Secure Setup option, you are required to enter your license key which was sent to the email 
address you provided. This process will associate your license with your subdomain to ensure that valid certificates can 
only be generated by a single license holder.

![Enter License](images/setup/3.png "Enter License")

The next step is to name and claim your subdomain.

![Claim Domain](images/setup/4.png "Claim Domain")

---

<br/>

### Configuring The Server Addresses

In the next screen, you will choose the IP address and port that your server will bind to.

If you wish to setup a cluster of servers/nodes (for a more stable, robust, and available database), this is the place to add 
nodes to the cluster and choose their IP addresses.

For a smooth setup experience, please **make sure that the IP address and port are available in each machine**. The wizard will 
validate this and throw an error if they are being used. When using port 443, you need to ensure that it hasn't already been 
taken by other applications like Skype, IIS, Apache, etc. On Linux, you might need to [allow port 443 for non-root processes](https://superuser.com/questions/710253/allow-non-root-process-to-bind-to-port-80-and-443).  

For a list of IPs and ports already in use on your machine, run `netstat -a` in the command line.  

**IP addresses and ports may be changed at a later time** by running the setup wizard again which will update the DNS records. 
Another way is to configure the `settings.json` file in each node's server folder.  This process requires you to restart your 
server after configuring.  

### Example I - On one machine

In the following screenshot, we show an example of constructing a cluster for local development on one machine:

![Configure Cluster](images/setup/5.png "Configure Cluster")

All 3 nodes will run on the local machine:

- Node A (https://a.raven.development.run) will listen to 127.0.0.1 on port 8080.
- Node B (https://b.raven.development.run) will listen to 127.0.0.2 on port 8080.
- Node C (https://c.raven.development.run) will listen to 127.0.0.3 on port 8080.

Each node will run in its own process and have its own data directory and [settings.json](../../server/configuration/configuration-options#settings.json) file. 
You should have 3 separate RavenDB node folders.

### Example II - On separate machines for higher availability

Each node will run on its own machine in a network.

A common scenario for running an internal cluster will be:  

- Node A (https://a.raven.development.run) will listen to 10.0.0.84 on port 443.
- Node B (https://b.raven.development.run) will listen to 10.0.0.75 on port 443.
- Node C (https://c.raven.development.run) will listen to 10.0.0.91 on port 443.

You can deploy a cluster that is completely internal to your network and still gain all the benefits of using certificates and SSL with 
full trust and complete support from all the standard tooling. 

{INFO: A cluster of nodes on separate machines} 
To enable the nodes to communicate between machines, use the 10.0... IP addresses instead of the 127.0...
{INFO/}


### Example III - Behind a firewall

A RavenDB server can run behind a firewall (in cloud environments for example).

RavenDB will bind to the **private** IP address. However, the DNS records must be updated to the **external** IP address which is reachable 
from the outside world. Requests made to the external IP address will be forwarded to the private IP address (which RavenDB listens on).

Check the box "Customize external IP and Ports" and supply the external IP address.  

![Configure Cloud Node](images/setup/5a.png "Configure Cloud Node")

<br/>

### Example IV - In a Docker container

In Docker, if you choose to use port mapping with the `-p` flag, You need to check the box "Customize external IP and Ports" 
and supply the external IP address as well as the exposed ports.  

So if a container was created using:

    sudo docker run -t -p 38889:38888 -p 443:8080 ravendb/ravendb

Then the following configuration should be applied:  

![Configure Docker Node](images/setup/5b.png "Configure Docker Node")

<br/>

---

### Installing The Certificate

When you click next, the wizard will establish a connection with Let's Encrypt to obtain a valid certificate for the entire cluster. 

It usually takes this process a couple of minutes to complete. The wizard validates that the DNS records updated successfully and that 
the server can run with the supplied addresses and certificate and is reachable using the new domain name.

{WARNING: Caching of Let's Encrypt Certificates} 
In some scenarios you will run the setup wizard again. In that case, if none of the cluster domains changed, the wizard will use the 
cached certificate and not request a new one from Let's Encrypt.
{WARNING/}

![Finishing Up](images/setup/6.png "Finishing Cluster IP Configuration")

{INFO: Configuration Failure}

If the validation fails, you will receive a detailed error. You can go back in the wizard, change the settings and try again.

A [common DNS error](../../server/security/common-errors-and-faq) is that **DNS records didn't yet update** locally.  
Usually, the solution is to wait a few minutes and try again. If you do not want to wait, you can configure your network card 
(just for the setup) to use Google's DNS server (8.8.8.8), to bypass caching of DNS (Domain Name System) records.

Tip:  use dns.google.com to see the DNS record of your domain.

{INFO/}


When finished you will receive a .zip file containing all of the cluster configuration files and certificates.  
Save this .zip file in each of your server folders. It has the security certificate and settings and for each node.  
You may need it in the future, so make sure it is saved in a permanent location.
If you are setting up a cluster, you will use this Zip file to set up the other nodes.

![Configuration Completed](images/setup/7.png "Configuration Completed Image")

Copy the downloaded `<YourDomainName>.Cluster.Settings.zip` folder into the Cluster Parent folder(s) to use it later. 
It contains the certificate and configurations of the server that you set up in the wizard.  

When you will run the .pfx installation wizard, 
it will set a file path for the certificate in the `settings.json` file.  
**Make sure not to relocate these files after installation** without also changing the `settings.json` because relocating them will 
cause a 'System.InvalidOperationException: Unable to start the server.' error. 
If you must move your folder at a later time, you can [reconfigure the certificate file path](../../server/security/authentication/certificate-configuration#standard-manual-setup-with-certificate-stored-locally) 
in the `settings.json` file.

![Save Cluster Settings Zip in Parent Folder](images/setup/Cluster-Settings-Zip-In-Parent-Folder.png "Save Cluster Settings Zip in Parent Folder")

If you left the "Automatically register the admin client..." box in the IP setup stage checked (it is checked by default), 
a client certificate is registered in the OS trusted store during setup. The Chrome and Edge browsers use the OS store, 
so they will let you choose your certificate right before you are redirected.  

Firefox users will have to manually import the certificate 
to the browser via Tools > Options > Advanced > Certificates > View Certificates.

If you unchecked the box, before you continue, please [register the client certificate](../../server/security/authentication/client-certificate-usage) 
in the OS store or import it to the browser.

---

### Install client certificate - Run Certificate Import Wizard

A.  Extract the downloaded configuration .zip file `<YourDomainName>.Cluster.Settings.zip` to the parent folder.

B.  Run the `admin.client...pfx` file to start the client certificate import wizard.  Make sure you use the `admin.client...pfx` and not the 
  `cluster.server...pfx`.  
    Unless you want to set a client certificate password or 
    define a different file path, you can use the default settings by clicking **next** every time.  

       ![Certificate Import Wizard](images/setup/Certificate-Import-Wizard.png "Certificate Import Wizard")

C.  In the main installation wizard on your browser, there should be a screen with a **restart** button.  
     
       ![Restart server after IP setup](images/setup/Restart-server-after-IP-setup.png "Restart server after IP setup")

D.  After clicking restart, the wizard checks if you've run the certificate Import Wizard, which you've just done.

       ![Restart Certificate Wizard Check](images/setup/Cert-Wizard-Check.png "Certificate Wizard Check")

E.  You should see a window that asks which certificate you want to use.  

       ![Choose certificate](images/setup/8.png "Choose Certificate")

---

If you are setting up a only single node, the setup is complete and you can start working on your secure server.  
  
---

<br/>
  
### Setting Up Other Nodes

When you access the Studio (automatically opens when starting a RavenDB server) check that Node A is running by clicking the 
**Manage Server** tab on the left side > select **Cluster**. 
You will see something similar to this:

![Incomplete Cluster](images/setup/9.png "Incomplete Cluster view")

Nodes B and C are not running yet. As soon as we start them, Node A will detect and add them to the cluster.

**Now, let's bring Node B up.**

1. Extract the downloaded server `RavenDB...zip` folder into the Node B folder.  
2. In **Windows**, start the RavenDB setup wizard using the `run.ps1` script via PowerShell. In **Linux**, use the `start.sh` script.  
3. **Continue the cluster setup for new node**  
   This time we will scroll down and click the "Continue the cluster setup for new node" button to connect other servers to this cluster.  

      ![Choose Cluster Setup](images/setup/10.png "Choose Cluster Setup view")

4. If on separate machines, run the `admin.cluster...pfx` file on the new machine to register the certificate in the OS.  
5. **Configuration package**  
   In the Setup Wizard (image below), **Browse** for and select the `<YourDomainName>.Cluster.Settings.zip` file from setup folder.  
6. **Node Tag**  
   Select node tag (B in this case) to designate which node in the cluster this server will be.  
7. Then click **next**.  

      ![Complete Existing Cluster](images/setup/Complete-Existing-Cluster-Setup.png "Complete Existing Cluster view")

8. Click **Restart**.  A new tab with the Studio should open in your browser and when you navigate to: Manage Server -> Cluster you should 
   see two green nodes with a green line between them.  
9. Repeat the process for the remaining nodes. When all the nodes are up, you can view the updated topology in the Studio.  

{NOTE: A Healthy Cluster} 

* All of the nodes in a healthy cluster should be green with green lines between them.  
* If one of the nodes disconnects at any time, the RavenDB studio will show that it is red with a red line showing the disconnect.  

{NOTE/} 

![Healthy Cluster](images/setup/12.png "Healthy Cluster")

</br>

You have successfully finished setting up a secure cluster of RavenDB servers using a Let's Encrypt certificate.  
By default, the server certificate will renew automatically because you set up the cluster with Let's Encrypt and RavenDB's Setup Wizard.
  
{INFO: Registering as a service to increase server availability}

You can now [register the cluster as a service](../../start/installation/running-as-service) in your OS (it will run in the background 
every time your machine starts).

{INFO/}

{PANEL/}

---

{PANEL: Next Steps}

Learning how to work with RavenDB correctly with the following resources will make your job easier and save time 
handling technical problems later.  

* **To learn how to fly with RavenDB**, here are a few options:
  * [Demo](https://demo.ravendb.net/) - Step-by-step code walkthrough  
  * [Bootcamp](https://ravendb.net/learn/bootcamp) - Lessons mailed to your inbox 
  * [Documentation Guide](https://ravendb.net/learn/docs-guide) - In addition to this guide and the [First Steps](../../start/installation/setup-wizard#first-steps-from-our-documentation) 
    section below, RavenDB's extensive documentation also has detailed explanations of server related operations, external replication, 
    cloud-based servers, and much more. Navigate to related articles on the left and right sides of your screen.  
  * [Workshops](https://workshops.ravendb.net/winter-2022)
  * [Webinars](https://ravendb.net/learn/webinars)

#### First Steps from our documentation:

* **Importing from Other Databases**
  * [From SQL Databases](../../studio/database/tasks/import-data/import-from-sql)
  * [From NoSQL Databases](../../studio/database/tasks/import-data/import-from-other)
  * [From CSV](../../studio/database/tasks/import-data/import-from-csv)
  * [From RavenDB file](../../studio/database/tasks/import-data/import-data-file) or from a [live RavenDB server](../../studio/database/tasks/import-data/import-from-ravendb)
* [Configure Client Certificates](../../server/security/authentication/certificate-management) to customize security access 
  permissions for each client.  
* [APIs to Integrate With Your Client](../../client-api/what-is-a-document-store) starting with the Document Store.  
* [Studio GUI Overview](../../studio/overview) to learn how the built-in RavenDB studio features can save you time and help you 
  keep things running smoothly.  
* [RavenDB Indexing](../../indexes/indexing-basics) is very sophisticated.  Auto-indexing is default and with agility in mind, it adjusts to changes immediately, 
  but you can set up static indexes manually as well.  
* [Backup Securely](../../studio/database/tasks/backup-task)  
* **Document Extensions**  
  * [Time Series](../../document-extensions/timeseries/overview)  
  * [Attachments](../../document-extensions/attachments/what-are-attachments)  
  * [Counters](../../document-extensions/counters/overview)  



{PANEL/}

---

{PANEL:Secure Setup with Your Own Certificate}

In RavenDB, users can provide their own server certificate. The certificate can be issued by a trusted SSL vendor or it can be 
a self-signed certificate. In the latter case, it's the user's responsibility to have the self-signed CA registered in the OS 
stores on all the relevant machines.

RavenDB will accept PFX server certificates which contain the private key, are not expired, and have the following fields:

- KeyUsage: DigitalSignature, KeyEncipherment
- ExtendedKeyUsage: Client Authentication, Server Authentication

If you wish to use the setup wizard to construct a cluster, you must use the same certificate for all nodes. If you wish to use 
a different certificate for each node, it's possible only through [manual setup](../../start/installation/manual). A wildcard 
certificate is probably the easiest way to go. Another option is to issue a certificate which contains all the domains of all the 
cluster nodes as "Subject Alternative Names" (SANs). 

After choosing the Secure Setup with your own certificate option, you are required to upload the certificate and click next. 
In the example, we will use the *.ravendb.example.com wildcard certificate.  

![Upload Certificate](images/setup/w1.png "Upload Certificate")

### Configuring The Server Addresses

In the next screen, you will choose the IP address and port that your server will bind to.

If you wish to setup a cluster of servers/nodes (for a more stable, robust, and available database), this is the place to add 
nodes to the cluster and choose their IP addresses.

For a smooth setup experience, please **make sure that the IP addresses and ports are available in each machine**. The wizard 
will validate this and throw an error if they are being used. When using port 443, you need to ensure that it hasn't already 
been taken by other applications like Skype, IIS, Apache, etc. On Linux, you might need to [allow port 443 for non-root processes](https://superuser.com/questions/710253/allow-non-root-process-to-bind-to-port-80-and-443).  
For a list of IPs and ports already in use, run `netstat -a` in the command line.

{WARNING: Important} 
If you bring your own certificate, you must also take care of the DNS records. If you choose to bind to 127.0.0.1, and provide a 
certificate with CN=my.domain, then the DNS record of my.domain must point to 127.0.0.1.

If you are running behind a firewall, the DNS records must point to the **external** IP address.
{WARNING/}

### Example I - On one machine

In the following screenshot, we show an example of constructing a cluster for local development on one machine:

![Configure Cluster](images/setup/w2.png "Configure Cluster")

All 3 nodes will run on the local machine:

- Node A (https://a.ravendb.example.com) will listen to 127.0.0.1 on port 8080.
- Node B (https://b.ravendb.example.com) will listen to 127.0.0.2 on port 8080.
- Node C (https://c.ravendb.example.com) will listen to 127.0.0.3 on port 8080.

Each node will run in its own process and have its own data directory and [settings.json](../../server/configuration/configuration-options#settings.json) file. 

### Example II - On separate machines

Each node will run on its own machine in a network.

A common scenario for running an internal cluster will be:  

- Node A (https://a.ravendb.example.com) will listen to 10.0.0.84 on port 443.
- Node B (https://b.ravendb.example.com) will listen to 10.0.0.75 on port 443.
- Node C (https://c.ravendb.example.com) will listen to 10.0.0.91 on port 443.

You can deploy a cluster that is completely internal to your network and still gain all the benefits of using certificates and SSL with 
full trust and complete support from all the standard tooling. 

{INFO: A cluster of nodes on separate machines} 
To enable the nodes to communicate between machines, use the 10.0... IP addresses instead of the 127.0...
{INFO/}

### Example III - Behind a firewall

A RavenDB server can run behind a firewall (in cloud environments for example).

RavenDB will bind to the **private** IP address. However, the DNS records must be updated to the **external** IP address which is reachable 
from the outside world. Requests made to the external IP address will be forwarded to the private IP address (which RavenDB listens on).

It is your responsibility to update the DNS record of your domain to point to your external IP address.  

### Example IV - In a Docker container

In Docker, if you choose to use port mapping with the -p flag, You need to check the box "Customize external ports" and 
supply the exposed ports.  

It is your responsibility to update the DNS record of your domain to point to your external IP address. 

So if a container was created using:

    sudo docker run -t -p 38889:38888 -p 443:8080 ravendb/ravendb

Then the following configuration should be applied:  

![Configure Docker Node](images/setup/w2a.png "Configure Docker Node")

{NOTE: }

A [common DNS error](../../server/security/common-errors-and-faq) is that **DNS records didn't yet update** locally.  
Usually, the solution is to wait a few minutes and try again. If you do not want to wait, you can configure your network card 
(just for the setup) to use Google's DNS server (8.8.8.8), to bypass caching of DNS (Domain Name System) records.


When finished, you will receive a Zip file containing all of the cluster configuration settings files.  
If you are setting up a cluster, you will use this Zip file to set up each of your nodes.

![Configuration Completed](images/setup/w3.png "Configuration Completed")


{NOTE/}

</br>

### Installing Your Certificate and Setting the File Path

RavenDB will accept `.pfx` server certificates which contain the private key, are not expired, and have the following fields:

**KeyUsage**: DigitalSignature, KeyEncipherment  
**ExtendedKeyUsage**: Client Authentication, Server Authentication


1. Place the `.pfx` file in a permanent location in each server/node folder.  
  {WARNING: }
  If this location changes without adjusting your `settings.json` file, the server won't find the certificate and will not run 
  unless you also reconfigure the settings.json.  
  {WARNING/}
2. Run the `.pfx` file and click next each time for default settings, or configure the path and optional certificate password.  
3. At this point, click the "Restart Server" button, and wait until the browser redirects you to the new URL 
  (in the example it's "https://a.ravendb.example.com").
4. If you left the "Automatically register the admin client..." box in the IP setup stage checked (it is checked by default), 
  a client certificate is registered in the OS trusted store during setup. The Chrome and Edge browsers use the OS store, 
  so they will let you choose your certificate right before you are redirected.  
   * Firefox users will have to manually import the certificate to the browser via Tools > Options > Advanced > Certificates > View Certificates.
   * If you unchecked the box, before you continue please [register the client certificate](../../server/security/authentication/client-certificate-usage) 
     in the OS store or import it to the browser.

![Restart and choose certificate](images/setup/w4.png "Restart and choose certificate")

If you are setting up a single node, the setup is complete and you can start working.  

<br/>

### Setting Up Other Nodes

When you access the Studio (automatically opens when starting a RavenDB server) check that Node A is running by clicking the 
**Manage Server** tab on the left side > select **Cluster**. 

![Incomplete Cluster](images/setup/w5.png "Incomplete Cluster")

Nodes B and C are not running yet. As soon as we start them, node A will detect it and add them to the cluster.

**Now, let's bring Node B up.**

1. Extract the downloaded server `RavenDB...zip` folder into the Node B folder.  
2. In **Windows**, start the RavenDB setup wizard using the `run.ps1` script via PowerShell. In **Linux**, use the `start.sh` script.  
3. **Continue the cluster setup for new node**  
   This time we will scroll down and click the "Continue the cluster setup for new node" button to connect other servers to this cluster.  
 
      ![Choose Cluster Setup](images/setup/10.png "Choose Cluster Setup view")

4. If on separate machines, run the `admin.cluster...pfx` file on the new machine to register the certificate in the OS.  
5. **Configuration package**  
   In the Setup Wizard (image below), **Browse** for and select the `<YourDomainName>.Cluster.Settings.zip` file from setup folder.  
6. **Node Tag**  
   Select node tag (B in this case) to designate which node in the cluster this server will be.  
7. Then click **next**.  

      ![Complete Existing Cluster](images/setup/Complete-Existing-Cluster-Setup.png "Complete Existing Cluster view")
 
8. Click **Restart**.  A new tab with the Studio should open in your browser and when you navigate to: Manage Server -> Cluster you should 
   see two green nodes with a green line between them.  
9. Repeat the process for the remaining nodes. When all the nodes are up, you can view the updated topology in the Studio.  

{NOTE: A Healthy Cluster} 

* All of the nodes in a healthy cluster should be green with green lines between them.  
* If one of the nodes disconnects at any time, the RavenDB studio will show that it is red with a red line showing the disconnect.  

{NOTE/} 

![Complete Cluster](images/setup/w7.png "Complete Cluster")

You have successfully finished setting up a secure cluster of RavenDB servers using your own wildcard certificate.

{WARNING: }

You are responsible to periodically renew your server certificate.

{WARNING/}  


{INFO: Registering as a service to increase server availability}

You can now [register the cluster as a service](../../start/installation/running-as-service) in your OS (it will run in the background 
every time your machine starts).

{INFO/}

{PANEL/}

{PANEL: Next Steps}

Learning how to work with RavenDB correctly with the following resources will make your job easier and save time 
handling technical problems later.  

* **To learn how to fly with RavenDB**, here are a few options:
  * [Demo](https://demo.ravendb.net/) - Step-by-step code walkthrough  
  * [Bootcamp](https://ravendb.net/learn/bootcamp) - Lessons mailed to your inbox 
  * [Documentation Guide](https://ravendb.net/learn/docs-guide) - In addition to this guide and the [First Steps](../../start/installation/setup-wizard#first-steps-from-our-documentation) 
    section below, RavenDB's extensive documentation also has detailed explanations of server related operations, external replication, 
    cloud-based servers, and much more. Navigate to related articles on the left and right sides of your screen.  
  * [Workshops](https://workshops.ravendb.net/winter-2022)
  * [Webinars](https://ravendb.net/learn/webinars)

#### First Steps from our documentation:

* **Importing from Other Databases**
  * [From SQL Databases](../../studio/database/tasks/import-data/import-from-sql)
  * [From NoSQL Databases](../../studio/database/tasks/import-data/import-from-other)
  * [From CSV](../../studio/database/tasks/import-data/import-from-csv)
  * [From RavenDB file](../../studio/database/tasks/import-data/import-data-file) or from a [live RavenDB server](../../studio/database/tasks/import-data/import-from-ravendb)
* [Configure Client Certificates](../../server/security/authentication/certificate-management) to customize security access 
  permissions for each client.  
* [APIs to Integrate With Your Client](../../client-api/what-is-a-document-store) starting with the Document Store.  
* [Studio GUI Overview](../../studio/overview) to learn how the built-in RavenDB studio features can save you time and help you 
  keep things running smoothly.  
* [RavenDB Indexing](../../indexes/indexing-basics) is very sophisticated.  Auto-indexing is default and with agility in mind, it adjusts to changes immediately, 
  but you can set up static indexes manually as well.  
* [Backup Securely](../../studio/database/tasks/backup-task)  
* **Document Extensions**  
  * [Time Series](../../document-extensions/timeseries/overview)  
  * [Attachments](../../document-extensions/attachments/what-are-attachments)  
  * [Counters](../../document-extensions/counters/overview)  



{PANEL/}

{PANEL:Unsecure Setup}

In the **Unsecure Mode**, all you need to do is specify the **IP address** and **ports** that the server will listen to.  
{DANGER: Danger}

* We strongly recommend [setting up securely](../../start/installation/setup-wizard#secure-setup-with-a-free-let) from the 
  start to prevent potential future vulnerability.  The process takes a few minutes and is free.  

* All security features (authentication, authorization and encryption) are **disabled** in the Unsecure Mode.  

* When choosing to listen to an outside network, the RavenDB server does not provide any security since Authentication is off.  

* Anyone who can access the server using the configured IP address will be granted **administrative privileges**.  
{DANGER/}

![Complete Cluster](images/setup/u0.png "Configuring a server on Node A, listening to 127.0.0.1 on port 8080")

1. **Http Port** - Enter the port that will be used by the clients and the Studio. Default is 8080.  
   **TCP Port** - Enter the port that will be used for inter-server communication and for subscriptions. Default is 38888.  

2. **IP Address**: Enter the server's IP address.  

3. **Create new cluster**  
   * **Checked** - The server will be created within a cluster with the specified Node Tag.  
                   This new cluster will only contain this node.  
   * **Unchecked** - The server will Not be created in a cluster.  
                     The server will be created in a 
                     [Passive State](../../studio/cluster/cluster-view#cluster-nodes-states-&-types-flow) 
                     and can later be added to an already existing cluster.  

4. **Environment**  
   This option only shows when creating a new cluster.  
   Select the label that will be shown in the Studio UI for this server.     
   If you select 'None' now, you can still configure this later from the Studio.  

5. **Next** - Click Next when done configuring.  

      ![Complete Cluster](images/setup/u1.png "Configuration has completed - Restart the Server")

      Once the configuration is completed, restart the server.  
      After a few seconds, the server will be ready and accessible.  
      Access the Studio by entering the URL in the browser: "http://127.0.0.1:8080" or "http://localhost:8080".  

      ![Complete Cluster](images/setup/u2.png "The server's dashboard")

### Continuing The Cluster Setup

To construct a cluster, unzip the downloaded RavenDB package to more machines (or local folders), 
as many as the number of nodes you want.  
In each node, start the RavenDB server and complete the Setup Wizard, entering a different IP address per server.

Once all the servers are up and running, building the cluster is simple.  
Access the studio, go to _Manage Server > Cluster_, and add nodes to the cluster by their URL.  
Learn more in [Adding a Node to a Cluster](../../studio/cluster/setting-a-cluster#add-another-node-to-the-cluster).  
{PANEL/}

## Related Articles

### Server
- [Security in RavenDB - Overview](../../server/security/overview)
- [Common Setup Wizard Errors and FAQ](../../server/security/common-errors-and-faq#setup-wizard-issues) 

### Getting Started
- [Manual Setup](../../start/installation/manual)
- [Running as a Service](../../start/installation/running-as-service)
- [Running in a Docker Container](../../start/installation/running-in-docker-container)
- [Setup Example - AWS Windows VM](../../start/installation/setup-examples/aws-windows-vm)
- [Setup Example - AWS Linux VM](../../start/installation/setup-examples/aws-linux-vm)
