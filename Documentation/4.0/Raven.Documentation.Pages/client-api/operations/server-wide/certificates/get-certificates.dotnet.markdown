# Operations: Server: How to Get Certificates

To get client certificates use **GetCertificatesOperation**.

## Syntax

{CODE:csharp get_certs_1@ClientApi\Operations\Server\ClientCertificate.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **pageSize** | int | Maximum number of records that will be downloaded |
| **start** | int | Number of records that should be skipped |

| Return Value | |
| ------------- | ----- |
| `CertificateDefinition[]` | Array of certificate definitions |

##Example

{CODE:csharp get_certs_2@ClientApi\Operations\Server\ClientCertificate.cs /}
