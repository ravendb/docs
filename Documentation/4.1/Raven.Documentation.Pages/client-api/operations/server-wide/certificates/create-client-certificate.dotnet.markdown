# Operations: Server: How to Generate a Client Certificate

You can generate a client certificate using **CreateClientCertificateOperation**. 

## Syntax

{CODE cert_1_1@ClientApi\Operations\Server\ClientCertificate.cs /}

{CODE cert_1_2@ClientApi\Operations\Server\ClientCertificate.cs /}

{CODE cert_1_3@ClientApi\Operations\Server\ClientCertificate.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | string | Name of a certificate |
| **permissions** | Dictionary&lt;string, DatabaseAccess&gt; | Dictionary with database to access level mapping |
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

- [How to **delete** certificate?](../../../../client-api/operations/server-wide/certificates/delete-certificate) 
- [How to **put** client certificate?](../../../../client-api/operations/server-wide/certificates/put-client-certificate) 
