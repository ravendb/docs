# Operations : Server : How to Get a Certificate

To get a client certificate by thumbprint use **GetCertificateOperation**.

## Syntax

{CODE:csharp get_cert_1@ClientApi\Operations\Server\ClientCertificate.cs /}

### Return Value

The result of executing GetCertificateOperation is a [CertificateDefinition](../../../../glossary/CertificateDefinition) object.

###Example

{CODE:csharp get_cert_2@ClientApi\Operations\Server\ClientCertificate.cs /}
