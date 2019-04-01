# Operations: Server: How to Get a Certificate

To get a client certificate by thumbprint use **GetCertificateOperation**.

## Syntax

{CODE:csharp get_cert_1@ClientApi\Operations\Server\ClientCertificate.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **thumbprint** | string | Certificate thumbprint |

| Return Value | |
| ------------- | ----- |
| `CertificateDefinition` | Certificate definition |

##Example

{CODE:csharp get_cert_2@ClientApi\Operations\Server\ClientCertificate.cs /}
