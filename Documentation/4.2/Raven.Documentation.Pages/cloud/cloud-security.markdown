# RavenDB on the Cloud: Security
---

{NOTE: }

RavenDB cloud products use several layers of security.  

* All instances run over HTTPS.  

* Servers and client applications authenticate each other using X.509 certificates.  
  {INFO: }
  Note that a RavenDB cloud product **comes with an initial client certificate**.  
  You need this certificate in order to access the product.  
  {INFO/}

* You can choose [which IP addresses](../cloud/portal/cloud-portal-products-tab#manage-product-the-security-tab) your server can be contacted by.  

* Your [automated backup](../cloud/cloud-backup-and-restore) routines produce encrypted backup files.  

* In this page:  
    * [Using The Initial Client Certificate](cloud-security#using-the-initial-client-certificate)  
    * [Using Additional Certificates](cloud-security#using-additional-certificates)  

{NOTE/}

---

{PANEL: Using The Initial Client Certificate}

A client certificate is automatically generated during the creation of your RavenDB cloud product.  
You will need to import this certificate to your browser in order to access your cloud instances.  

---

####Download Certificate  

Go to the [Products tab](../cloud/portal/cloud-portal-products-tab) in the cloud [portal](../cloud/portal/cloud-portal) 
and click the **Download Certificate** button.  

!["Download Certificate"](images\security-001-download-certificate.png "Download Certificate")  

---

####Install the certificate  

Extract the certificate package, open it and double-click the **.pfx** file to launch the certificate import wizard. 
Simply confirm all the stages using the **Next** button until the wizard completes.  

!["Certificate Import Wizard"](images\security-002-wizard.png "Certificate Import Wizard")  

---

####Access your product  

If you're using Chrome on Windows, you will now be able to access your RavenDB cloud instance. You may need to restart your browser.  
In other cases (e.g. if you're using Firefox or run Linux) you will have to import the certificate to your browser manually.  

!["Server URLs"](images\migration-001-urls.png "Server URLs")  
  
Once the certificate is imported, click your cloud instance's URL.  
Your browser will prompt you to select a certificate. When you select the client certificate, RavenDB's [management studio]() will launch.  

{PANEL/}

{PANEL: Using Additional Certificates}

Your initial [operator-level](../server/security/authorization/security-clearance-and-permissions#operator) 
certificate allows you to perform operations like creating and deleting databases, managing access to the cluster, and inspecting the cluster's state.  
Operations like adding and removing cluster nodes are left for your [products administrators](../cloud/cloud-overview#ravendb-on-the-cloud-overview).  

{INFO: }
We recommend that you generate and use **different certificates** for your client applications, for maximum security.  
{INFO/}

{INFO: }
If your instance runs on a [burstable CPU](../cloud/cloud-overview#burstable-vs.-reserved-clusters), especially if it is a low-end one, 
RavenDB may take a while to generate certificates and spend a lot of your [CPU credits](../cloud/cloud-overview#budget-credits-and-throttling) in the 
process.  
We therefore recommend that you generate your certificates off-cloud and import them to your cloud instance.  
{INFO/}

**To generate additional [operator](../server/security/authorization/security-clearance-and-permissions#operator) 
  or [user](../server/security/authorization/security-clearance-and-permissions#user) certificates**:  
 
* Go to the server management studio, and click The `Manage Certificates` button.  
  !["Manage Certificates"](images\migration-002-manage-certificates.png "Manage Certificates")  
* Click **Generate client certificate**  
  !["Generate Client Certificate"](images\security-003-generate-client-certificate.png "Generate Client Certificate")  

{PANEL/}

##Related Articles

**Cloud**  
[Overview](../cloud/cloud-overview)  
[Pricing, Payment and Billing](../cloud/cloud-pricing-payment-billing)  
[Backup And Restore](../cloud/cloud-backup-and-restore)  
[Migration](../cloud/cloud-migration)  

  
[Portal](../cloud/portal/cloud-portal)  
  
[RavenDB on Burstable Instances](https://ayende.com/blog/187681-B/running-ravendb-on-burstable-cloud-instances)  
[AWS CPU Credits](https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/burstable-credits-baseline-concepts.html)  

**Server**  
[Security Overview](../server/security/overview)  
