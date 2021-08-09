# Operations: Server: How to Generate a Client Certificate

You can generate a client certificate using **CreateClientCertificateOperation**. 

## Syntax

{CODE:nodejs cert_1_1@ClientApi\Operations\Server\ClientCertificate.js /}

{CODE:nodejs cert_1_2@ClientApi\Operations\Server\ClientCertificate.js /}

{CODE:nodejs cert_1_3@ClientApi\Operations\Server\ClientCertificate.js /}

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

{CODE:nodejs cert_1_4@ClientApi\Operations\Server\ClientCertificate.js /}

## Example II

{CODE:nodejs cert_1_5@ClientApi\Operations\Server\ClientCertificate.js /}

## Related articles

- [How to **delete** certificate?](../../../../client-api/operations/server-wide/certificates/delete-certificate) 
- [How to **put** client certificate?](../../../../client-api/operations/server-wide/certificates/put-client-certificate) 
