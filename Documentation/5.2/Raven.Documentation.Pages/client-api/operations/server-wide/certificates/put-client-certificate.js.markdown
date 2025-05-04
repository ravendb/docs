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

{CODE:nodejs cert_put_2@ClientApi\Operations\Server\ClientCertificate.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@ClientApi\Operations\Server\ClientCertificate.js /}

| Parameter       | Type                             | Description                                                                                         |
|-----------------|----------------------------------|-----------------------------------------------------------------------------------------------------|
| **name**        | `string`                         | A name for the certificate.                                                                         |
| **certificate** | `string`                         | The certificate to register.                                                                        |
| **permissions** | `Record<string, DatabaseAccess>` | A dictionary mapping database name to access level.<br>Relevant only when clearance is `ValidUser`. |
| **clearance**   | `SecurityClearance`              | Access level (role) assigned to the certificate.                                                    |

* `SecurityClearance` options:
  * `ClusterAdmin`  
  * `ClusterNode`  
  * `Operator`  
  * `ValidUser`  

* `DatabaseAccess ` options:
  * `Read`
  * `ReadWrite`  
  * `Admin`

{PANEL/}

## Related Articles

- [Delete client certificate](../../../../client-api/operations/server-wide/certificates/delete-certificate)
- [Generate client certificate](../../../../client-api/operations/server-wide/certificates/create-client-certificate)
