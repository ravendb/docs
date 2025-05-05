# Put Client Certificate Operation
---

{NOTE: }

* Use `PutClientCertificateOperation` to register an existing client certificate.

* To register an existing client certificate from the Studio,
  see [Upload an existing client certificate](../../../../studio/server/certificates/server-management-certificates-view#upload-an-existing-client-certificate).

* In this article:
  * [Put client certificate example](../../../../client-api/operations/server-wide/certificates/put-client-certificate#put-client-certificate-example)
  * [Syntax](../../../../client-api/operations/server-wide/certificates/put-client-certificate#syntax)

{NOTE/}

---

{PANEL: Put client certificate example}

{CODE-TABS}
{CODE-TAB:csharp:Sync put_client_certificate@ClientApi\Operations\Server\ClientCertificate.cs /}
{CODE-TAB:csharp:Async put_client_certificate_async@ClientApi\Operations\Server\ClientCertificate.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE put_syntax@ClientApi\Operations\Server\ClientCertificate.cs /}

| Parameter       | Type                                 | Description                                                                                         |
|-----------------|--------------------------------------|-----------------------------------------------------------------------------------------------------|
| **name**        | `string`                             | A name for the certificate.                                                                         |
| **certificate** | `X509Certificate2`                   | The certificate to register.                                                                        |
| **permissions** | `Dictionary<string, DatabaseAccess>` | A dictionary mapping database name to access level.<br>Relevant only when clearance is `ValidUser`. |
| **clearance**   | `SecurityClearance`                  | Access level (role) assigned to the certificate.                                                    |

{CODE cert_1_2@ClientApi\Operations\Server\ClientCertificate.cs /}
{CODE cert_1_3@ClientApi\Operations\Server\ClientCertificate.cs /}

{PANEL/}

## Related Articles

- [Delete client certificate](../../../../client-api/operations/server-wide/certificates/delete-certificate) 
- [Generate client certificate](../../../../client-api/operations/server-wide/certificates/create-client-certificate)
