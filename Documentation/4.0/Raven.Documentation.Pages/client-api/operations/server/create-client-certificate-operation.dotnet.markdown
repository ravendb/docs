# Operations : Server : How to generate client certificate?

You can generate client certificate using **CreateClientCertificateOperation**. 

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

- [How to **delete** client certificate?](../../../client-api/operations/server/delete-client-certificate-operation) 

