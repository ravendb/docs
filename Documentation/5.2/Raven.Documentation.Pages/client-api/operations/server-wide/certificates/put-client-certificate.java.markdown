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

{CODE:java cert_put_2@ClientApi\Operations\Server\ClientCertificate.java /}

{PANEL/}

{PANEL: Syntax}

{CODE:java cert_put_1@ClientApi\Operations\Server\ClientCertificate.java /}

| Parameter       | Type                          | Description                                                                                          |
|-----------------|-------------------------------|------------------------------------------------------------------------------------------------------|
| **name**        | `String`                      | A name for the certificate.                                                                          |
| **certificate** | `String`                      | The certificate to register.                                                                         |
| **permissions** | `Map<String, DatabaseAccess>` | A dictionary mapping database name to access level.<br>Relevant only when clearance is `VALID_USER`. |
| **clearance**   | `SecurityClearance`           | Access level (role) assigned to the certificate.                                                     |

{CODE:java cert_1_2@ClientApi\Operations\Server\ClientCertificate.java /}
{CODE:java cert_1_3@ClientApi\Operations\Server\ClientCertificate.java /}

{PANEL/}

## Related Articles

- [Delete client certificate](../../../../client-api/operations/server-wide/certificates/delete-certificate)
- [Generate client certificate](../../../../client-api/operations/server-wide/certificates/create-client-certificate)
