# Client API: Setting up Authentication and Authorization

Authentication and authorization is based on [client X.509 certificates](../server/security/authorization/security-clearance-and-permissions).

The authentication options argument in `DocumentStore` constructor property allows you to pass a certificate which will be used by the RavenDB client to connect to a server. 

{NOTE If your RavenDB server instance is served using `https`, then your application is required to use a client certificate in order to be able to access the server. You can find more information [here](../server/security/overview). /}

## Example

{CODE:nodejs client_cert@client-api\certificate.js /}

## Related Articles

### Security

- [How to Create **Secured Server**](../server/security/overview)
- [Security Clearance & Permissions](../server/security/authorization/security-clearance-and-permissions)

### Document Store

- [What is a Document Store](../client-api/what-is-a-document-store)
- [Creating a Document Store](../client-api/creating-document-store)
- [Setting up Default Database](../client-api/setting-up-default-database)
