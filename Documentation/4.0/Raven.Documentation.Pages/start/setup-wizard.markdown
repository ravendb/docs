# RavenDB Setup Walkthrough

We at Hibernating Rhinos understand that for programmers who are not experts in database or security, setting up and securing your database can be a strenuous task, especially if you need to set up a secured cluster.

To make the process as painless as possible, we came up with a wizard UI that guides you through the setup process. 

You have 4 options to choose from:

1. [Secure Setup with a Let's Encrypt certificate](#section1)
2. [Secure Setup with your own certificate](#section2)
3. [Unsecured Setup](#section3)
4. [Manual Setup](#section4)

When you download RavenDB 4 and start the server for the first time, you will be redirected to the setup wizard welcome screen where you can choose your preferred option.

![Figure 1. Welcome Screen](images/setup/2.png)

This section explains how to follow the setup wizard. It does not go into detail about security concerns. If you wish to learn about how authentication and authorization work in RavenDB or more about security in general, please read the relevant security section. 

<hr />

<a name="section1"></a>
# Secure Setup with a Let's Encrypt Certificate

Let's Encrypt is a free, automated, and non-profit certificate authority. It will generate a certificate for your domain (or website) as long as you can prove that you own it.

One of our goals is to make it as easy as possible for you to start RavenDB with a valid trusted certificate from the very beginning, and to stay secured from the development stage through  production deployment. 

During the wizard, RavenDB will give you a free subdomain under "dbs.local.ravendb.net". It lets you configure the DNS records for this subdomain to point to the IP addresses your server will listen to. The subdomain is owned by RavenDB, but you can always update the DNS records by running the setup wizard.

The free subdomain is given to you only for the purpose of proving ownership to Let's Encrypt.
If you wish to use your own domain, you are welcome to acquire a certificate by yourself and manually configure the server with it.

After choosing the Let's Encrypt Secure Setup option, you are required to enter your license. We need the license key here because we need to associate the subdomain with a single owner to ensure that you are the only one who can generate a valid certificate for that domain name.

![Figure 2. Enter License](images/setup/3.png)

The next step is to claim your domain.

![Figure 3. Claim Domain](images/setup/4.png)

In the next screen, you get to choose the IP address and port that your server will listen to.

**Important!** If you wish to setup a cluster, this is the place to add nodes to the cluster and choose their addresses. You should run the setup wizard only on the first node in the cluster and **not** on each of them separately. The first node will generate the required configuration for the entire cluster, and will provide detailed guidance on how to setup the additional nodes.

In the following screenshot, we show an example of constructing a cluster for local development:

![Figure 4. Configure Cluster](images/setup/5.png)

All 3 nodes will run on the local machine as 3 different processes:

- Node A (https://a.3cpo.dbs.local.ravendb.net) will listen to 127.0.0.1 on port 8080.
- Node B (https://b.3cpo.dbs.local.ravendb.net) will listen to 127.0.0.2 on port 8080.
- Node C (https://c.3cpo.dbs.local.ravendb.net) will listen to 127.0.0.3 on port 8080.

A real life scenario will be to have each node on its own machine. In that case, you would enter the actual IP address the node will listen to on that machine. IP addresses may be changed at a later time by running the setup wizard again which will update the DNS records at "dbs.local.ravendb.net".

A common scenario for running an internal cluster will be:  

- Node A (https://a.3cpo.dbs.local.ravendb.net) will listen to 10.0.0.84 on port 443.
- Node B (https://b.3cpo.dbs.local.ravendb.net) will listen to 10.0.0.75 on port 443.
- Node C (https://c.3cpo.dbs.local.ravendb.net) will listen to 10.0.0.91 on port 443.

In this manner, you can deploy a cluster that is completely internal to your network, but still gain all the benefits of using certificates and SSL with full trust and complete support from all the standard tooling. RavenDB also ensures that the Let's Encrypt certificate is refreshed automatically as needed.

**Important!** You need to make sure that the IP/port is available. On the local machine, the setup will ensure the port is available and the IP address is valid. However, on the other machines in the cluster, you'll need to verify that beforehand. When using port 443, you need to ensure that it hasn't already been taken by other applications like Skype, IIS, Apache, etc.

When you click next, the wizard will communicate with the Let's Encrypt servers, answer its challenge, and obtain a valid certificate for the cluster. The entire process usually takes one to two minutes.

![Figure 5. Finishing Up](images/setup/6.png)

When finished, you will receive a ZIP file containing all of the cluster configuration files including the server and client certificates, and a settings.json file for each node. We will show you what to do with these files shortly.

![Figure 6. Configuration Completed](images/setup/7.png)

At this point you will click the "Restart Server" button, and wait until the browser redirects you to the new url (in the example it's "https://a.3cpo.dbs.local.ravendb.net").

If you checked the relevant box in the previous stage, a client certificate is registered in the OS trusted store during setup. The Chrome and Edge browsers use the OS store, so they will let you choose your certificate right before you are redirected. Firefox users will have to manually import the certificate to the browser via Tools > Options > Advanced > Certificates > View Certificates.

If you didn't check the box, before you continue please register the client certificate in the OS store or import it to the browser.

![Figure 7. Restart and choose certificate](images/setup/8.png)

When you access the studio please navigate to: Manage Server > Cluster. You will see something similar to this:

![Figure 8. Incomplete Cluster](images/setup/9.png)

Nodes B and C are not running yet. As soon as we start them, node A will detect it and add them to the cluster.

Now, let's configure nodes B and C.

In the ZIP file you have a folder for each node which includes a settings.json and a server certificate. For each node, simply copy these 2 files (overwrite existing) to that node's RavenDB server folder and start the server. 

When nodes B and C are up, go back to the studio and see that the topology of the cluster was updated.

![Figure 9. Complete Cluster](images/setup/10.png)

You have successfully finished setting up a secure cluster of RavenDB servers using a Let's Encrypt certificate.

<hr />

<a name="section2"></a>
# Secure Setup with your own certificate
