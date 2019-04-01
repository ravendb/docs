# Operations: Server: How to Put a Client Certificate

You can register an existing client certificate using **PutClientCertificateOperation**. 

## Syntax

{CODE:java cert_put_1@ClientApi\Operations\Server\ClientCertificate.java /}

{CODE:java cert_1_2@ClientApi\Operations\Server\ClientCertificate.java /}

{CODE:java cert_1_3@ClientApi\Operations\Server\ClientCertificate.java /}


| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | String | Name of a certificate |
| **certificate** | string | Certificate to register |
| **permissions** | Map&lt;String, DatabaseAccess&gt; | Map with database to access level mapping |
| **clearance** | SecurityClearance | Access level |

## Example

{CODE:java cert_put_2@ClientApi\Operations\Server\ClientCertificate.java /}

## Related Articles

- [How to **delete** client certificate?](../../../../client-api/operations/server-wide/certificates/delete-certificate) 
- [How to **generate** client certificate?](../../../../client-api/operations/server-wide/certificates/create-client-certificate) 

