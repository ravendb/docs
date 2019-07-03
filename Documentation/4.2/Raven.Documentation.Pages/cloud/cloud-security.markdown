#Cloud: Security
---

{NOTE: }

* All RavenDB cloud instances run securely with HTTPS.  
* Your RavenDB server and client identify each other using [X.509](https://docs.microsoft.com/en-us/windows/desktop/seccertenroll/about-x-509-public-key-certificates) 
certificates.  
{NOTE/}

---

{PANEL: }

A client certificate is automatically generated during the creation of your cloud cluster. You will need this certificate
to access your cluster. In the cloud [Control Panel](../cloud/cloud-control-panel), open the `Products` tab and click the 
`Download Certificate` button.  

![](images\CloudSecurity_1.png)  
  
Extract the certificate package and open it. On Windows double click the `.pfx` file and the `certificate import wizard` will launch: 

![](images\CloudSecurity_2.png)  
  
* Click next repeatedly until the wizard is through.  
* If you use Chrome, you will now be able to access your RavenDB cloud instance.  
  In other cases, e.g. if you use Firefox or run Linux, you will have to import the certificate to your browser manually.  
* Once the certificate is imported:  
  When you go to your cloud node's URL, your browser will prompt you to select a certificate.  
  When you select the appropriate client certificate, the RavenDB management studio will launch.  

This initial client certificate grants you [operator](../server/security/authorization/security-clearance-and-permissions#operator) 
level clearance to the server.  
To generate additional operator or [user](../server/security/authorization/security-clearance-and-permissions#user) 
certificates, go to the server management studio.  
Click on `Manage Certificates`:  

!["Manage Certificates"](images\CloudScaling_ManageCertificates.png "Manage Certificates")  
  
And then on `generate client certificate`:  
  
!["Generate Client Certificate"](images\CloudSecurity_3.png "Generate Client Certificate")  

{INFO: }
If your instance runs on a [burstable CPU](../cloud/cloud-overview#burstable-instances), 
especially if it is a low-end one, RavenDB may take a while to generate a certificate, 
spending a lot of your CPU credits in the process. We therefore recommend that you create your certificate 
locally and import it to your instance rather than let RavenDB generate it for you.  
{INFO/}

{INFO: }
We recommend that you generate and use **different certificates** for each of your client applications for maximum security.  
{INFO/}

{PANEL/}

