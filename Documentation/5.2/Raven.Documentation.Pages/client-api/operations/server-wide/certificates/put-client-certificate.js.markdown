# Operations: Server: How to Put a Client Certificate

You can register an existing client certificate using **PutClientCertificateOperation**. 

## Syntax

{CODE:nodejs cert_put_1@ClientApi\Operations\Server\ClientCertificate.js /}

{CODE:nodejs cert_1_2@ClientApi\Operations\Server\ClientCertificate.js /}

{CODE:nodejs cert_1_3@ClientApi\Operations\Server\ClientCertificate.js /}


| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | string | Name of a certificate |
| **certificate** | X509Certificate2 | Certificate to register |
| **permissions** | Dictionary&lt;string, DatabaseAccess&gt; | Dictionary mapping databases to access level |
| **clearance** | SecurityClearance | Access level |

## Example

{CODE:nodejs cert_put_2@ClientApi\Operations\Server\ClientCertificate.js /}

## Related Articles

- [How to **delete** client certificate?](../../../../client-api/operations/server-wide/certificates/delete-certificate) 
- [How to **generate** client certificate?](../../../../client-api/operations/server-wide/certificates/create-client-certificate) 

