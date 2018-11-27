# Client API : Setting up Authentication and Authorization

Authentication and authorization is based on [client X.509 certificates](../server/security/authorization/security-clearance-and-permissions).

The authentication options argument in `DocumentStore` constructor property allows you to pass a certificate which will be used by the RavenDB client to connect to a server. 

{NOTE If your RavenDB server instance is served using `https`, then your application is required to use a client certificate in order to be able to access the server. You can find more information [here](../server/security/overview). /}

## Example

{CODE:nodejs client_cert@clientApi\certificate.js /}

## Related Articles

### Security

- [How to Create **Secured Server**](../server/security/overview)
- [Security Clearance & Permissions](../server/security/authorization/security-clearance-and-permissions)

### Document Store

- [What is a Document Store](../clientApi/what-is-a-document-store)
- [Creating a Document Store](../clientApi/creating-document-store)
- [Setting up Default Database](../clientApi/setting-up-default-database)
