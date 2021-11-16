# Installation: Setup Wizard Walkthrough
---

{NOTE: }

* We want to make it as easy as possible for you to start RavenDB with a valid, trusted certificate from the very beginning.  
  We want you to stay secure throughout your application lifecycle, starting with the early stages of development and all the way to production and day-to-day usage.  

* To make the setup process as smooth as possible, we have introduced the **Setup Wizard**,  
  a step-by-step guide to help you configure the desired security level and easily deploy a secure cluster.  

* Using the Setup Wizard you can set up a secure cluster with:  
  * **Let's Encrypt certificate**  
  * **Self-obtained certificate**  
  * -or- choose to continue in an **unsecure mode**.  

* Note: The RavenDB server can also be **set up manually** if choosing not to use the wizard.  
  See [Manual Setup](../../start/installation/manual)  

* Note: this page explains how to follow the Setup Wizard, without going into security concerns details.  
  To learn more about how _Authentication_ and _Authorization_ are implemented with RavenDB, and about _Security_ in general,  
  go to [Security Overview](../../server/security/overview).  

* If you are having trouble using the wizard, or with security in general, please visit the [Security Common Errors & FAQ](../../Server/Security/common-errors-and-faq) section.

 In this page:  

- [Select Setup Mode](../../start/installation/setup-wizard#select-setup-mode)  
- [Secure Setup with a Free Let's Encrypt Certificate](../../start/installation/setup-wizard#secure-setup-with-a-free-let)  

    * [Configuring The Server Addresses](../../start/installation/setup-wizard#configuring-the-server-addresses)  
    * [Installing The Certificate](../../start/installation/setup-wizard#installing-the-certificate)  
    * [Setting Up Other Nodes](../../start/installation/setup-wizard#setting-up-other-nodes)  

- [Secure Setup with a Self-Obtained Certificate](../../start/installation/setup-wizard#secure-setup-with-your-own-certificate)  
    * [Configuring The Server Addresses](../../start/installation/setup-wizard#configuring-the-server-addresses-1)  
    * [Setting Up Other Nodes](../../start/installation/setup-wizard#setting-up-other-nodes-1)  
- [Unsecure Setup](../../start/installation/setup-wizard#unsecure-setup)  

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

{NOTE/}

---

{PANEL:Select Setup Mode}

* When running the RavenDB server for the first time,  
  you will be redirected to the setup wizard welcome page where you can choose your preferred option.  

![Figure 1. Welcome Page](images/setup/setup-wizard-1.png "Select Mode in the Welcome Page")

{PANEL/}

{PANEL:Secure Setup with a Free Let's Encrypt Certificate}

[Let's Encrypt](https://letsencrypt.org/) is a free, automated, and non-profit certificate authority.  
It will generate a certificate for your domain as long as you can prove that you own it.  

During the wizard, RavenDB will give you a free subdomain. This will let you configure the DNS records for this subdomain to point to the IP addresses your server will listen to. The subdomain is owned by RavenDB, and you can manage it through our [Customer Portal](https://customers.ravendb.net). Login with your license key, and you can add/remove/update DNS records for your cluster.

The free subdomain is given to you only for the purpose of proving ownership to Let's Encrypt. If you wish to use your own domain, you are welcome to acquire your own certificate and use that instead.

{WARNING: Security consideration and ownership of certificates and domains} 

The automatic setup is designed to be as convenient and as easy as possible. It takes care of all the details of setting up DNS records, generating certificates, and performing their renewals. Because of these requirements, the ownership of the certificates and DNS records needs to stay within the Hibernating Rhinos company. This gives us the ability to generate valid certificates and modify DNS settings for your registered domains and should be a consideration to keep in mind while reviewing the security of your system.  

Hibernating Rhinos will **never** exploit these abilities and will never perform any modifications to the certificates and DNS records unless explicitly requested by the client.

The purpose of this feature is to make it easy for users to get set up and running with a minimum of fuss. We recommend that **for actual production deployments** and for the highest level of security and control, you'll [use your own certificates and domains](../../start/installation/setup-wizard#secure-setup-with-your-own-certificate), avoiding the need to rely on a third party for such a critical part of your security.

{WARNING/}

After choosing the Let's Encrypt Secure Setup option, you are required to enter your license key which was sent to the email address you provided. This process will associate your license with the chosen subdomain to ensure that valid certificates can only be generated by a single license holder.

![Figure 2. Enter License](images/setup/3.png "Enter License")  

The next step is to name and claim your subdomain.

![Figure 3. Claim Domain](images/setup/4.png "Claim Domain")  
<br/>

### Configuring The Server Addresses

In the next screen, you will choose the IP address and port that your server will bind to.

If you wish to setup a cluster of servers/nodes (for a more stable, robust, and available database), this is the place to add nodes to the cluster and choose their IP addresses.

For a smooth setup experience, please **make sure that the IP address and port are available in each machine**. The wizard will validate this and throw an error if they are being used. When using port 443, you need to ensure that it hasn't already been taken by other applications like Skype, IIS, Apache, etc. On Linux, you might need to [allow port 443 for non-root processes](https://superuser.com/questions/710253/allow-non-root-process-to-bind-to-port-80-and-443).  

For a list of IPs and ports already in use on your machine, run `netstat -a` in the command line.  

**IP addresses and ports may be changed at a later time** by running the setup wizard again which will update the DNS records. Another way is to configure the `settings.json` file in each node's server folder.  This process requires you to restart your server after configuring.  

### Example I - On one machine

In the following screenshot, we show an example of constructing a cluster for local development on one machine:

![Figure 4. Configure Cluster](images/setup/5.png "Configure Cluster")  

All 3 nodes will run on the local machine:

- Node A (https://a.raven.development.run) will listen to 127.0.0.1 on port 8080.
- Node B (https://b.raven.development.run) will listen to 127.0.0.2 on port 8080.
- Node C (https://c.raven.development.run) will listen to 127.0.0.3 on port 8080.

Each node will run in its own process and have its own data directory and [settings.json](../../server/configuration/configuration-options#json) file. You should have 3 separate RavenDB folders.

### Example II - On separate machines

Each node will run on its own machine in a network.

A common scenario for running an internal cluster will be:  

- Node A (https://a.raven.development.run) will listen to 10.0.0.84 on port 443.
- Node B (https://b.raven.development.run) will listen to 10.0.0.75 on port 443.
- Node C (https://c.raven.development.run) will listen to 10.0.0.91 on port 443.

You can deploy a cluster that is completely internal to your network and still gain all the benefits of using certificates and SSL with full trust and complete support from all the standard tooling.

{WARNING: A cluster of nodes on separate machines} 
To enable the nodes to communicate between machines, use the 10.0... IP addresses instead of the 127.0...
{WARNING/}


### Example III - Behind a firewall

A RavenDB server can run behind a firewall (in cloud environments for example).

RavenDB will bind to the **private** IP address. However, the DNS records must be updated to the **external** IP address which is reachable from the outside world. Requests made to the external IP address will be forwarded to the private IP address (which RavenDB listens on).

Check the box "Customize external IP and Ports" and supply the external IP address.  

![Figure 4a. Configure Cloud Node](images/setup/5a.png "Configure Cloud Node")  
<br/>

### Example IV - In a Docker container

In Docker, if you choose to use port mapping with the `-p` flag, You need to check the box "Customize external IP and Ports" and supply the external IP address as well as the exposed ports.  

So if a container was created using:

    sudo docker run -t -p 38889:38888 -p 443:8080 ravendb/ravendb

Then the following configuration should be applied:  

![Figure 4b. Configure Docker Node](images/setup/5b.png "Configure Docker Node")  
<br/>

### Installing The Certificate

When you click next, the wizard will establish a connection with Let's Encrypt to obtain a valid certificate for the entire cluster. 

It usually takes this process a couple of minutes to complete. The wizard validates that the DNS records updated successfully and that the server can run with the supplied addresses and certificate and is reachable using the new domain name.

{WARNING: Caching of Let's Encrypt Certificates} 
In some scenarios you will run the setup wizard again. In that case, if none of the cluster domains changed, the wizard will use the cached certificate and not request a new one from Let's Encrypt.
{WARNING/}

![Figure 5. Finishing Up](images/setup/6.png "Finishing Cluster IP Configuration")  

If the validation fails, you will receive a detailed error. You can go back in the wizard, change settings and try again.

A [common error](../../server/security/common-errors-and-faq) is that DNS records didn't yet update locally.  
You may wait a bit and try again. If you do not want to wait, you can configure your network card (just for the setup) to use Google's DNS server (8.8.8.8), to bypass caching of DNS records.

Tip:  use dns.google.com to see the DNS record of your domain.

<br/>

When finished you will receive a Zip file containing all of the cluster configuration files and certificates.  
Save this .zip file in your parent folder. It has the security certificate and settings and for each node.  
If you are setting up a cluster, you will use this Zip file to set up the other nodes.

![Figure 6. Configuration Completed](images/setup/7.png "Configuration Completed Image")  

Copy the downloaded `<YourDomainName>.Cluster.Settings.zip` folder into the Cluster Parent folder to use it later.  
![Figure 6a. Save Cluster Settings Zip in Parent Folder](images/setup/Cluster-Settings-Zip-In-Parent-Folder.png "Save Cluster Settings Zip in Parent Folder")  

If you left the "Automatically register the admin client..." box in the IP setup stage checked (it is checked by default), a client certificate is registered in the OS trusted store during setup. The Chrome and Edge browsers use the OS store, so they will let you choose your certificate right before you are redirected. Firefox users will have to manually import the certificate to the browser via Tools > Options > Advanced > Certificates > View Certificates.

If you unchecked the box, before you continue please register the client certificate in the OS store or import it to the browser.

### Run Certificate Import Wizard

A.  Extract the downloaded configuration .zip file `<YourDomainName>.Cluster.Settings.zip` to the parent folder.

B.  Run the `admin.client...pfx` file to start the certificate import wizard.  In most cases, it is configured to work best by clicking `next` every time.  
     ![Figure 7. Certificate Import Wizard](images/setup/Certificate-Import-Wizard.png "Certificate Import Wizard")

C.  In the main installation wizard on your browser, there should be a screen with a `restart` button.  
     ![Figure 7a. Restart server after IP setup](images/setup/Restart-server-after-IP-setup.png "Restart server after IP setup")

D.  After clicking restart, the wizard checks if you've run the certificate Import Wizard, which you've just done.
     ![Figure 7b. Restart Certificate Wizard Check](images/setup/Cert-Wizard-Check.png "Certificate Wizard Check")  

E.  You should see a window that asks which certificate you want to use.  
     ![Figure 7c. Choose certificate](images/setup/8.png "Choose Certificate")  
</br>

If you are setting up a single node, the setup is complete and you can start working.  
  
<br/>
  
### Setting Up Other Nodes

When you access the Studio please navigate to: Manage Server -> Cluster. You will see something similar to this:

![Figure 8. Incomplete Cluster](images/setup/9.png "Incomplete Cluster view")

Nodes B and C are not running yet. As soon as we start them, Node A will detect and add them to the cluster.

Now, let's bring Node B up.

A.  Copy the zipped configuration `...settings.zip` file to the Node B folder.  
B.  Extract it into the Node B folder.  
C.  Extract the downloaded server `RavenDB...zip` folder into the Node B folder.  
D.  In **Windows**, start the RavenDB setup wizard using the `run.ps1` script. In **Linux**, use the `start.sh` script.
  
E.  This time we will choose to `Continue the cluster setup for new node`.
     ![Figure 9. Choose Cluster Setup](images/setup/10.png "Choose Cluster Setup view")  

F.  Run the Certificate Import Wizard again with the `admin.client...pfx` that's in the Node B folder. Click `next` until done.  
G.  In the Setup Wizard (image below), `Browse` for and select the **zipped** `...Settings.zip` file from the Node B folder.  
H.  `Select node tag` (B in this case).  
I.  Then click `next`.  
     ![Figure 10. Complete Existing Cluster](images/setup/Complete-Existing-Cluster-Setup.png "Complete Existing Cluster view")  

J.  Click `Restart`.  A new tab with the Studio should open in your browser and when you navigate to: Manage Server -> Cluster you should see two green nodes with a green line between them.  
K.  Repeat the process for the remaining nodes. When all the nodes are up, you can view the updated topology in the Studio.  

{NOTE: A Healthy Cluster} 
All of the nodes in a healthy cluster should be green with green lines between them.  
If one of the nodes disconnects at any time, the RavenDB studio will show that it is red with a red line showing the disconnect.  
{NOTE/} 

![Figure 11. Healthy Cluster](images/setup/12.png "Healthy Cluster")
</br>

You have successfully finished setting up a secure cluster of RavenDB servers using a Let's Encrypt certificate.

{PANEL/}

{PANEL:Secure Setup with Your Own Certificate}

In RavenDB, users can provide their own server certificate. The certificate can be issued by a trusted SSL vendor or it can be a self-signed certificate. In the latter case, it's the user's responsibility to have the self-signed CA registered in the OS stores on all the relevant machines.

RavenDB will accept PFX server certificates which contain the private key, are not expired, and have the following fields:

- KeyUsage: DigitalSignature, KeyEncipherment
- ExtendedKeyUsage: Client Authentication, Server Authentication

If you wish to use the setup wizard to construct a cluster, you must use the same certificate for all nodes. If you wish to use a different certificate for each node, it's possible only through [manual setup](../../start/installation/manual). A wildcard certificate is probably the easiest way to go. Another option is to issue a certificate which contains all the domains of all the cluster nodes as "Subject Alternative Names" (SANs). 

After choosing the Secure Setup with your own certificate option, you are required to upload the certificate and click next. In the example, we will use the *.ravendb.example.com wildcard certificate.  

![Figure 1. Upload Certificate](images/setup/w1.png "Upload Certificate")

### Configuring The Server Addresses

In the next screen, you will choose the IP address and port that your server will bind to.

If you wish to setup a cluster of servers/nodes (for a more stable, robust, and available database), this is the place to add nodes to the cluster and choose their IP addresses.

For a smooth setup experience, please **make sure that the IP addresses and ports are available in each machine**. The wizard will validate this and throw an error if they are being used. When using port 443, you need to ensure that it hasn't already been taken by other applications like Skype, IIS, Apache, etc. On Linux, you might need to [allow port 443 for non-root processes](https://superuser.com/questions/710253/allow-non-root-process-to-bind-to-port-80-and-443).  For a list of IPs and ports already in use, run `netstat -a` in the command line.

{WARNING: Important} 
If you bring your own certificate, you must also take care of the DNS records. If you choose to bind to 127.0.0.1, and provide a certificate with CN=my.domain, then the DNS record of my.domain must point to 127.0.0.1.

If you are running behind a firewall, the DNS records must point to the **external** IP address.
{WARNING/}

### Example I

In the following screenshot, we show an example of constructing a cluster for local development on one machine:

![Figure 2. Configure Cluster](images/setup/w2.png "Configure Cluster")  

All 3 nodes will run on the local machine:

- Node A (https://a.ravendb.example.com) will listen to 127.0.0.1 on port 8080.
- Node B (https://b.ravendb.example.com) will listen to 127.0.0.2 on port 8080.
- Node C (https://c.ravendb.example.com) will listen to 127.0.0.3 on port 8080.

Each node will run in its own process and have its own data directory and [settings.json](../../server/configuration/configuration-options#json) file. You should have 3 separate RavenDB folders.

### Example II

Each node will run on its own machine in a network.

A common scenario for running an internal cluster will be:  

- Node A (https://a.ravendb.example.com) will listen to 10.0.0.84 on port 443.
- Node B (https://b.ravendb.example.com) will listen to 10.0.0.75 on port 443.
- Node C (https://c.ravendb.example.com) will listen to 10.0.0.91 on port 443.

You can deploy a cluster that is completely internal to your network and still gain all the benefits of using certificates and SSL with full trust and complete support from all the standard tooling.

### Example III

A RavenDB server can run behind a firewall (in cloud environments for example).

RavenDB will bind to the **private** IP address. However, the DNS records must be updated to the **external** IP address which is reachable from the outside world. Requests made to the external IP address will be forwarded to the private IP address (which RavenDB listens on).

It is your responsibility to update the DNS record of your domain to point to your external IP address.  

### Example IV

In Docker, if you choose to use port mapping with the -p flag, You need to check the box "Customize external ports" and supply the exposed ports.  

It is your responsibility to update the DNS record of your domain to point to your external IP address. 

So if a container was created using:

    sudo docker run -t -p 38889:38888 -p 443:8080 ravendb/ravendb

Then the following configuration should be applied:  

![Figure 2a. Configure Docker Node](images/setup/w2a.png "Configure Docker Node")  

When finished, you will receive a Zip file containing all of the cluster configuration files and certificates. In case you are setting up a cluster, you will use this Zip file to setup the other nodes.

![Figure 3. Configuration Completed](images/setup/w3.png "Configuration Completed")

At this point, click the "Restart Server" button, and wait until the browser redirects you to the new URL (in the example it's "https://a.ravendb.example.com").

If you checked the relevant box in the previous stage, a client certificate is registered in the OS trusted store during setup. The Chrome and Edge browsers use the OS store, so they will let you choose your certificate right before you are redirected. Firefox users will have to manually import the certificate to the browser via Tools > Options > Advanced > Certificates > View Certificates > Your Certificates Tab > Import.

If you didn't check the box, please register the client certificate in the OS store or import it to the browser before you continue.

![Figure 4. Restart and choose certificate](images/setup/w4.png "Restart and choose certificate")

If you are setting up a single node, the setup is complete and you can start working.

### Setting Up Other Nodes

When you access the Studio please navigate to: Manage Server > Cluster. You will see something similar to this:

![Figure 8. Incomplete Cluster](images/setup/w5.png "Incomplete Cluster")  

Nodes B and C are not running yet. As soon as we start them, node A will detect it and add them to the cluster.

Now, let's bring node B up.

First, copy the configuration Zip file to node B and download/copy a fresh RavenDB server folder. In **Windows**, start RavenDB using the `run.ps1` script. In **Linux**, use the `start.sh` script.

This time, we will choose to continue the cluster setup.

![Figure 9. Continue Setup](images/setup/10.png "Continue Setup")  

Now we will supply the downloaded Zip file and select the node we are currently setting up.

![Figure 10. Upload Zip File](images/setup/w6.png "Upload Zip File")  

Click restart when finished and repeat the process for more nodes. When all the nodes are up, you can view the updated topology in the studio.

![Figure 11. Complete Cluster](images/setup/w7.png "Complete Cluster")

You have successfully finished setting up a secure cluster of RavenDB servers using your own wildcard certificate.

{PANEL/}

{PANEL:Unsecure Setup}

In the **Unsecure Mode**, all you need to do is specify the **IP address** and **ports** that the server will listen to.  
{DANGER: Danger}

* We strongly recommend [setting up securely](../../start/installation/setup-wizard#secure-setup-with-a-free-let) from the start to prevent potential future vulnerability.  The process takes a few minutes and is free.  

* All security features (authentication, authorization and encryption) are **disabled** in the Unsecure Mode.  

* When choosing to listen to an outside network, the RavenDB server does not provide any security since Authentication is off.  
  Anyone who can access the server using the configured IP address will be granted **administrative privileges**.  
{DANGER/}

![Figure 1. Complete Cluster](images/setup/u0.png "Configuring a server on Node A, listening to 127.0.0.1 on port 8080 ")

1. **Http Port** - Enter the port that will be used by the clients and the Studio. Default is 8080.  
   **TCP Port** - Enter the port that will be used for inter-server communication and for subscriptions. Default is 38888.  

2. **IP Address**: Enter the server's IP address.  

3. **Create new cluster**  
   * **Checked** - The server will be created within a cluster with the specified Node Tag.  
                   This new cluster will only contain this node.  
   * **Unchecked** - The server will Not be created in a cluster.  
                     The server will be created in a [Passive State](../../studio/server/cluster/cluster-view#cluster-nodes-states-&-types-flow) and can later be added to an already existing cluster.  

4. **Environment**  
   This option only shows when creating a new cluster.  
   Select the label that will be shown in the Studio UI for this server.     
   If you select 'None' now, you can still configure this later from the Studio.  

5. **Next** - Click Next when done configuring.  

![Figure 2. Complete Cluster](images/setup/u1.png "Configuration has completed - Restart the Server")

Once configuration is completed, restart the server.  
After a few seconds, the server will be ready and accessible.  
Access the Studio by entering the URL in the browser: "http://127.0.0.1:8080" or "http://localhost:8080".  

![Figure 3. Complete Cluster](images/setup/u2.png "The server's dashboard")

### Continuing The Cluster Setup

To construct a cluster, unzip the downloaded RavenDB package to more machines (or local folders), 
as many as the number of nodes you want.  
In each node, start the RavenDB server and complete the Setup Wizard, entering a different IP address per server.

Once all the servers are up and running, building the cluster is simple.  
Access the studio, go to _Manage Server > Cluster_, and add nodes to the cluster by their URL.  
Learn more in [Adding a Node to a Cluster](../../studio/server/cluster/add-node-to-cluster).  
{PANEL/}

## Related Articles

### Server
- [Security in RavenDB - Overview](../../server/security/overview)
- [Common Setup Wizard Errors and FAQ](../../server/security/common-errors-and-faq#setup-wizard-issues) 

### Getting Started
- [Manual Setup](../../start/installation/manual)
- [Running as a Service](../../start/installation/running-as-service)
- [Setup Example - AWS Windows VM](../../start/installation/setup-examples/aws-windows-vm)
- [Setup Example - AWS Linux VM](../../start/installation/setup-examples/aws-linux-vm)
- [Setup Example - Docker on AWS Linux VM](../../start/installation/setup-examples/aws-docker-linux-vm)
