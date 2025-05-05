# Certificates View
---

{NOTE: }

* Use the **Certificates** view to create, customize, import and export certificates.  

* In this article:
   * [The certificates view](../../../studio/server/certificates/server-management-certificates-view#the-certificates-view)  
   * [View and edit certificates](../../../studio/server/certificates/server-management-certificates-view#view-and-edit-certificates)  
   * [Generate a client certificate](../../../studio/server/certificates/server-management-certificates-view#generate-a-client-certificate)  
   * [Enable communication between secure servers](../../../studio/server/certificates/server-management-certificates-view#enable-communication-between-secure-servers)  
      * [Upload an existing client certificate](../../../studio/server/certificates/server-management-certificates-view#upload-an-existing-client-certificate)
      * [Export server certificates](../../../studio/server/certificates/server-management-certificates-view#export-server-certificates)
   * [Certificate collections](../../../studio/server/certificates/server-management-certificates-view#certificate-collections)

{NOTE/}

---

{PANEL: The certificates view}

![Studio Certificates Management View](images/studio-certificates-overview.png "Studio Certificates Management View")

Client certificates are managed by RavenDB directly and not through any PKI infrastructure.  
To remove or reduce permissions on a certificate handed to a client, you can edit or remove 
the permissions entirely using this view.

1. Click **Manage Server** tab.
2. Select **Certificates**.
3. **Client certificate**  
   * [Generate client certificate](../../../server/security/authentication/certificate-management#generate-client-certificate) a new client certificate  
   * [Upload a client certificate](../../../server/security/authentication/certificate-management#upload-an-existing-certificate) that was exported from another server so that they can communicate.  
4. **Server certificates**  
   * [Export server certificates](../../../server/security/authentication/certificate-management#export-server-certificates) so that you can import them into another server.  
   * [Replace server certificates](../../../server/security/authentication/certificate-renewal-and-rotation) by uploading another `.pfx` certificate.  
5. **Well known admin certificate**  
   This is a trusted certificate, defined by a system administrator and given admin permissions.  
6. **Server certificate**  
   You can click [Renew now](../../../server/security/authentication/certificate-renewal-and-rotation) here.  
7. **Client certificate currently active in this server**  
   You can remove client certificates here or [edit](../../../server/security/authentication/certificate-management#edit-certificate) their settings, 
   e.g. databases permissions and [authorization](../../../server/security/authorization/security-clearance-and-permissions#authorization-security-clearance-and-permissions) 
   (security clearance) levels.  

{PANEL/}

{PANEL: View and edit certificates}

In the image below, the client certificates (`sampledom.client.certificate` and `Booking`) have different 
**security clearance** and **database permissions** configurations.  
This is done to give admins the ability to protect the contents of their databases by **customizing permissions**.  

For example, an application user can be given read/write access to the HR database, while project managers 
receive operator permissions on all databases.  

You can grant different [access levels](../../../server/security/authorization/security-clearance-and-permissions#authorization-security-clearance-and-permissions) 
by using different client certificates, each with its own set of permissions.  

![Client Certificate Settings](images/registered.png "Client Certificate Settings")

1. **Name**  
   Client certificate name.  
2. **Thumbprint**  
   Unique key for each certificate.  
3. **Security Clearance**  
   [Authorization level](../../../server/security/authorization/security-clearance-and-permissions#authorization-security-clearance-and-permissions) 
   that determines types of actions that can be done with this certificate.  
4. **Valid From**  
   Indicates when the certificate became valid. 
5. **Expiration date**  
   Client certificates are given 5 year expiration period by default.  
6. **Database Permissions**  
   The databases in this cluster that this client certificate has access to.  
7. **Edit Certificate**  
   Configure which databases it can access (applicable for User level) and its authorization clearance level.  
8. **Delete Certificate**  

{PANEL/}

{PANEL: Generate a client certificate} 

Use this view to generate a client certificate directly via RavenDB.  
Newly generated certificates will be added to the list of registered certificates.  

![Generate Client Certificate](images/generate.png "Generate Client Certificate")

1. **Certificate Name**
2. **Security Clearance Level**  
   Read more [here](../../../server/security/authorization/security-clearance-and-permissions#authorization-security-clearance-and-permissions) 
   about available clearance levels.  
3. **Certificate Passphrase**  
4. **Expiration Period**  
   Client certificates expiration is set to 5 years by default.  
5. **Database Permissions**  
   Select the **databases** that this certificate gives access to,  
   and the allowed **access level** for each database.  
6. **Require two-factor authentication**  
   Use this setting to add a two-factor authentication security layer to your certificate.  
    - Enabling two-factor authentication will display the certificate's **authentication key** 
      and **QR code**.  
    - You can then scan the QR code or copy the key by an external authentication application 
      of your choice, e.g. Google Authenticator or 2FAS.  
    - A client that connects Studio with a certificate that requires two-factor authentication, 
      will be granted access only after providing a code generated by the external authentication 
      service.  
    - This is what Studio's clearance screen looks like when 2-factor authentication is used:
      
         ![Entering Studio with Two-Factor Authentication](images/two-factor-auth.png "Entering Studio with Two-Factor Authentication")
     
         * **A. Authentication Code**  
           Provide a code generated by your 2-Factor authentication service.  
         * **B. Additional Settings**  
           You can limit the session duration here.  
           You can also grant access only to this browser, or use this 
           clearance screen to open Studio for other clients as well.  
7. **Generate** the certificate or **Cancel**.  
   
{NOTE: }
The information collected in this view is used by RavenDB internally, 
and will not be stored in the certificate itself.
{NOTE/}

{PANEL/}

{PANEL: Enable communication between secure servers} 

To enable communication between two secure databases, you need to:

1. **Export** ([download](../../../server/security/authentication/certificate-management#export-server-certificates)) 
   the `.pfx` certificate from the destination cluster.  
2. **Upload** (import) the downloaded certificate into the source server.  

---

### Upload an existing client certificate

Use this option to upload an existing client certificate.  
Uploaded certificates will be added to the list of registered certificates.  

![Upload client certificate](images/upload.png "Upload client certificate")

While uploading the client certificate you can modify its settings.  

![Modify uploaded certificate settings](images/upload-settings.png "Modify uploaded certificate settings")

See the [Generate a client certificate](../../../studio/server/certificates/server-management-certificates-view#generate-a-client-certificate) 
section to learn about the available settings.  

---

### Export server certificates

![Export Server Certificates](images/export-server-certificates.png "Export Server Certificates")

This option allows you to export the server certificate as a `.pfx` file.  
In the case of a cluster that contains several different server certificates, 
a `.pfx` [collection](../../../server/security/authentication/certificate-management#certificate-collections) 
will be exported.

{PANEL/}

{PANEL: Certificate collections} 

`.pfx` files may contain a single certificate or a collection of certificates.

When uploading a `.pfx` file with multiple certificates, RavenDB will add all certificates 
to the list of registered certificates as a single entry, and explicitly allow access to 
all certificates by their thumbprint.

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


