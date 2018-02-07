# Setting up Authentication and Authorization

Authentication and authorization is based on [client X.509 certificates](../server/security/authorization/security-clearance-and-permissions).

The `Certificate` property allows you to pass a certificate which will be used by the RavenDB client to connect to a server. 

{NOTE If your RavenDB instance is running on 'https', then your application has to use a client certificate in order to be able to access the server. You can find more information [here](../server/security/overview). /}

## Example

{CODE client_cert@ClientApi\Certificate.cs /}

## Related Articles

- [How to create **secured server**?](../server/security/overview)
- [Security Clearance & Permissions](../server/security/authorization/security-clearance-and-permissions)
