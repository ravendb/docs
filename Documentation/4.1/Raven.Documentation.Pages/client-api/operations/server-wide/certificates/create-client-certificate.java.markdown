# Operations: Server: How to Generate a Client Certificate

You can generate a client certificate using **CreateClientCertificateOperation**. 

## Syntax

{CODE:java cert_1_1@ClientApi\Operations\Server\ClientCertificate.java /}

{CODE:java cert_1_2@ClientApi\Operations\Server\ClientCertificate.java /}

{CODE:java cert_1_3@ClientApi\Operations\Server\ClientCertificate.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | String | Name of a certificate |
| **permissions** | Map&lt;String, DatabaseAccess&gt; | Map with database to access level mapping |
| **clearance** | SecurityClearance | Access level |
| **password** | String | Optional certificate password, default: no password |

| Return Value | |
| ------------- | ----- |
| **RawData** | client certificate raw data |

## Example I

{CODE:java cert_1_4@ClientApi\Operations\Server\ClientCertificate.java /}

## Example II

{CODE:java cert_1_5@ClientApi\Operations\Server\ClientCertificate.java /}

## Related articles

- [How to **delete** certificate?](../../../../client-api/operations/server-wide/certificates/delete-certificate) 
- [How to **put** client certificate?](../../../../client-api/operations/server-wide/certificates/put-client-certificate) 
