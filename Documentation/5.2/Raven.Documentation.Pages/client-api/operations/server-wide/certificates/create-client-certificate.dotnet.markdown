# Operations: Server: How to Generate a Client Certificate
---

{NOTE: }

* You can generate a client certificate using **CreateClientCertificateOperation**.  

* Learn the rationale needed to properly define client certificates in [The RavenDB Security Authorization Approach](../../../../server/security/authentication/certificate-management#the-ravendb-security-authorization-approach)

{NOTE/}

## Syntax

{CODE cert_1_1@ClientApi\Operations\Server\ClientCertificate.cs /}

{CODE cert_1_2@ClientApi\Operations\Server\ClientCertificate.cs /}

{CODE cert_1_3@ClientApi\Operations\Server\ClientCertificate.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | string | Name of a certificate |
| **permissions** | Dictionary&lt;string, DatabaseAccess&gt; | Dictionary mapping databases to access level |
| **clearance** | SecurityClearance | Access level |
| **password** | string | Optional certificate password, default: no password |

| Return Value | |
| ------------- | ----- |
| **RawData** | client certificate raw data |

## Example I

{CODE cert_1_4@ClientApi\Operations\Server\ClientCertificate.cs /}

## Example II

{CODE cert_1_5@ClientApi\Operations\Server\ClientCertificate.cs /}

## Related articles

**Client API Articles**:  
- [How to delete certificate?](../../../../client-api/operations/server-wide/certificates/delete-certificate)  
- [How to put client certificate?](../../../../client-api/operations/server-wide/certificates/put-client-certificate)  

**Server Articles**:  
- [Certificates Management](../../../../server/security/authentication/certificate-management#enabling-communication-between-servers-importing-and-exporting-certificates)  
- [Client Certificate Usage](../../../../server/security/authentication/client-certificate-usage)  

