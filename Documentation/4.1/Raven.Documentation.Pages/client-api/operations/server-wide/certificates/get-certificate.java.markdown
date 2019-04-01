# Operations: Server: How to Get a Certificate

To get a client certificate by thumbprint use **GetCertificateOperation**.

## Syntax

{CODE:java get_cert_1@ClientApi\Operations\Server\ClientCertificate.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **thumbprint** | String | Certificate thumbprint |

| Return Value | |
| ------------- | ----- |
| `CertificateDefinition` | Certificate definition |

##Example

{CODE:java get_cert_2@ClientApi\Operations\Server\ClientCertificate.java /}
