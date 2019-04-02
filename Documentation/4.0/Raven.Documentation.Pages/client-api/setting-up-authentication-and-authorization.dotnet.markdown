# Client API: Setting up Authentication and Authorization
---
{NOTE: }

* Authentication and authorization is based on [Client X.509 Certificates](../server/security/authorization/security-clearance-and-permissions).  

{INFO: Running On HTTPS}If your RavenDB instance runs on https, your application must have a certificate in order to access the server. Read more in [Security Overview](../server/security/overview).{INFO/}  

* X.509 certificates can be obtained for free through RavenDB. Alternatively, use your own certificate. Read more in the [Setup Wizard Walkthrough](../start/installation/setup-wizard#secure-setup-with-a-let).  

* Pass your certificate to the document store's `Certificate` property, as shown in the [example code](#example) below.
{NOTE/}
{PANEL:Example - Initializing Document Store With Certificate}<a name="example"></a>
{CODE client_cert@ClientApi\Certificate.cs /}
{PANEL/}
## Related Articles

### Getting Started

- [Installation : Setup Wizard Walkthrough](../start/installation/setup-wizard)

### Client API

- [What is a Document Store](../client-api/what-is-a-document-store)
- [Creating a Document Store](../client-api/creating-document-store)
- [Setting up Default Database](../client-api/setting-up-default-database)

### Server

- [Security Overview](../server/security/overview)
- [Security Clearance & Permissions](../server/security/authorization/security-clearance-and-permissions)
