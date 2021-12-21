# Certificates Management View

---

* The built-in RavenDB Studio enables full customization of client certificates as well as importing and exporting of certificates.  

* This article gives brief guidance about the Studio certificate management GUI.  
  For detailed explanations including [the RavenDB Security Authorization Approach](../../../server/security/authentication/certificate-management#the-ravendb-security-authorization-approach), see the article [Certificate Management](../../../server/security/authentication/certificate-management).  

In this page:

* [Certificates Management Studio View](../../../studio/server/certificates/server-management-certificates-view#certificates-management-studio-view)  
* [Customizing Certificates: Database and Access Permissions](../../../studio/server/certificates/server-management-certificates-view#customizing-certificates-database-and-access-permissions)  
* [Enabling Commuinication Between Servers: Importing and Exporting Certificates](../../../studio/server/certificates/server-management-certificates-view#enabling-commuinication-between-servers-importing-and-exporting-certificates)  

### Certificates Management Studio View

![Figure 1. Certificates View](images/studio-certificates-overview.png "Studio Certificate Management View")

1. Click **Manage Server** tab.
2. Select **Certificates**.
3. To enable servers to communicate, **generate** or **import** client certificates and **export** server certificate with these buttons.
4. Status of current server certificate.
5. Status of current client certificates active in this server.  You can edit or remove client certificates, including authorization levels here. 

---

### Customizing Certificates: Database and Access Permissions

In the following certificate management view, notice that various certificates have different database and [security clearance permissions](../../../server/security/authorization/security-clearance-and-permissions). 
This is done to give admins the ability to protect the contents of their databases by customizing permissions. 
For example, if an application user should have read-only access, but application managers should have database admin permissions, 
you can grant different access levels by using different client certificates, each with its own set of permissions.  

![Figure 2. Registered Certificates](images/customized-client-certificates.png "Status of Registered Certificates")


{PANEL: Certificates Status and Configuration} 

In the Studio Certificates Management view, we can see a list of registered client certificates and their status.  
Notice that the two client certificates ("admin" and "AppClients" have different access configurations.)  
Each certificate contains the following:

![Figure 3. Client Certificates with Various Permissions](images/registered.png "Client Certificates with Various Permissions")

1. Name  
2. Thumbprint  
3. Security Clearance  
4. Expiration date  
5. Allowed Databases  
6. Edit Certificate  
7. Delete Certificate  


{PANEL/}

### Manage Certificates

{PANEL:Generate Client Certificate} 

Using this view, you can generate client certificates directly via RavenDB.  
Newly generated certificates will be added to the list of registered certificates.  

![Figure 4. Generate Client Certificate](images/generate.png "Generate Client Certificate")

When generating a certificate, you must complete the following fields:

1. Name
2. Security Clearance
3. Allowed databases and access level for each database

This information is used by RavenDB internally and is not stored in the certificate itself.

Expiration for client certificates is set to 5 years by default.

{PANEL/}



{PANEL:Edit Certificate} 

Every certificate in the list can be edited. The editable fields are:

![Figure 5. Edit Certificate](images/edit.png "Edit Certificate")

1. Name
2. Security Clearance
3. Allowed databases and access level for each database

{PANEL/}

---

### Enabling Communication Between Servers: Importing and Exporting Certificates

{PANEL:Upload an Existing Certificate} 

Using this view you can upload existing client certificates.  
Uploaded certificates will be added to the list of registered certificates.  

To connect two secure databases, you must

a. **Export** (download) the cluster certificate from the destination cluster.  
b. **Upload** (import) the downloaded certificate into the source server.  

![Figure 6. Upload Existing Certificate](images/upload.png "Upload Existing Certificate")

When uploading an existing certificate file, you must complete the following fields:

1. Name
2. Security Clearance
3. Upload the `.pfx` certificate file from the destination server installation folder.
4. Select databases and permission levels

This information is used by RavenDB internally and is not stored in the certificate itself.

{PANEL/}

{PANEL:Export Server Certificates} 

![Figure 7. Export Server Certificates](images/export-server-certificates.png "Export Server Certificates")

This option allows you to export the server certificate as a .pfx file. In the case of a cluster which contains several different server certificates, a .pfx collection will be exported.

{PANEL/}

## Related articles

### Setting Up a Secure Cluster

- [Secure Setup with a Let's Encrypt Certificate](../../../start/installation/setup-wizard#secure-setup-with-a-let)
- [Secure Setup with Your Own Certificate](../../../start/installation/setup-wizard#secure-setup-with-your-own-certificate)

### Server

- [Certificate Management in Studio](../../../server/security/authentication/certificate-management)  
- [Security Clearance and Permissions](../../../server/security/authorization/security-clearance-and-permissions)  
- [Common Errors and FAQ](../../../server/security/common-errors-and-faq)  
- [Manual Certificate Configuration](../../../server/security/authentication/certificate-configuration)  

### Authorization

- [Security Clearance and Permissions](../../../server/security/authorization/security-clearance-and-permissions)

### Client API

- [Setting up Authentication and Authorization](../../../client-api/setting-up-authentication-and-authorization)
- [How to create a client certificate](../../../client-api/operations/server-wide/certificates/create-client-certificate) 
- [How to delete a certificate](../../../client-api/operations/server-wide/certificates/delete-certificate)  
- [How to generate a client certificate](../../../client-api/operations/server-wide/certificates/create-client-certificate) 
- [How to put a client certificate](../../../client-api/operations/server-wide/certificates/put-client-certificate)  

### Security

- [Overview](../../../server/security/overview)
- [Manual Certificate Configuration](../../../server/security/authentication/certificate-configuration)
- [Client Certificate Usage](../../../server/security/authentication/client-certificate-usage)
- [Certificate Renewal & Rotation](../../../server/security/authentication/certificate-renewal-and-rotation)
- [Encryption](../../../server/security/encryption/encryption-at-rest)


