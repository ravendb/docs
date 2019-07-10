#Cloud: Security
---

{NOTE: }

* All RavenDB cloud instances run securely on HTTPS.  
* RavenDB servers and client applications authenticate each other using X.509 certificates.  
* A RavenDB cloud instance comes with an initial client certificate that you will need in order to access it.  
* In this page:  
    * [Initial Client Certificate](cloud-security#initial-client-certificate)  
    * [Additional Certificates](cloud-security#additional-certificates)  

{NOTE/}

---

{PANEL: Initial Client Certificate}

A client certificate is automatically generated during the creation of your RavenDB cloud product. You will need to import 
this certificate to your browser to access your cloud instances.  

**1.** Go to the [Products tab](../cloud/portal/cloud-portal-products-tab) in the cloud portal and click on the `Download 
Certificate` button.  

!["Download Certificate"](images\security-001-download-certificate.png "Download Certificate")  
  
**2.** Extract the certificate package and open it. Double-click the `.pfx` file and the certificate import wizard will launch: 

!["Certificate Import Wizard"](images\security-002-wizard.png "Certificate Import Wizard")  
  
**3.** Click `Next` repeatedly until the wizard completes.  

**4.** If you're using Chrome on Windows, you will now be able to access your RavenDB cloud instance. 
In other cases (e.g. if you use Firefox or run Linux) you will have to import the certificate to your browser manually.  

**5.** Once the certificate is imported, go to your cloud instance's URL: 

!["Server URLs"](images\migration-001-urls.png "Server URLs")  
  
Your browser will prompt you to select a certificate. When you select the client certificate, the RavenDB [management studio]() will launch.  
{PANEL/}

{PANEL: Additional Certificates}

The initial client certificate grants you [operator](../server/security/authorization/security-clearance-and-permissions#operator) 
clearance to the server. In RavenDB cloud, unlike in an on-premises instance of RavenDB, you don't have access to the highest clearance level: 
[Cluster Admin](../server/security/authorization/security-clearance-and-permissions#cluster-admin). Operator clearance is very similar to 
the Cluster Admin, except for operations related to the [cluster](../server/clustering/overview), which we manage for you as 
part of the service we provide.  

To generate additional operator or [user](../server/security/authorization/security-clearance-and-permissions#user) clearance certificates:  

**1.** go to the server management studio, click on `Manage Certificates`:  

!["Manage Certificates"](images\migration-002-manage-certificates.png "Manage Certificates")  
  
**2.** And then on `generate client certificate`:  
  
!["Generate Client Certificate"](images\security-003-generate-client-certificate.png "Generate Client Certificate")  

{INFO: }
We recommend that you generate and use **different certificates** for your client applications, for maximum security.  
{INFO/}

{INFO: }
If your instance runs on a [burstable CPU](../cloud/cloud-overview#burstable-vs.-reserved-clusters), especially if it is a low-end one, 
RavenDB may take a while to generate certificates - spending a lot of your [CPU credits](cloud-overview#credits) in the 
process. We therefore recommend that you generate your certificates off-cloud and import them to your cloud instance instead 
of using your cloud instance generate them. We generate the initial client certificate off-cloud for the same reason.  
{INFO/}

{PANEL/}

##Related Articles

**Cloud**  
[Overview](cloud-overview)  
[Pricing, Payment and Billing](cloud-pricing-payment-billing)  
[Backup](cloud-backup)  
[Migration](cloud-migration)  

**Server**  
[Security Overview](../server/security/overview)  
