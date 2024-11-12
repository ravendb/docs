# Increment Next Identity Operation

---

{NOTE: }

* Use `NextIdentityForOperation` to increment the latest identity value set on the server for the specified collection in the database.

* The next document that will be created using an identity for the collection will receive the consecutive integer value.

* In this page:

  * [Increment the next identity value](../../../../client-api/operations/maintenance/identities/increment-next-identity#increment-the-next-identity-value)
  * [Syntax](../../../../client-api/operations/maintenance/identities/increment-next-identity#syntax)

{NOTE/}

---

{PANEL: Increment the next identity value }

{CODE:php increment_identity@ClientApi\Operations\Maintenance\Identities\IncrementIdentity.php /}

{PANEL/}

{PANEL: Syntax }

{CODE:php syntax@ClientApi\Operations\Maintenance\Identities\IncrementIdentity.php /}

| Parameter | Type   | Description                                     |
|-----------|--------|-------------------------------------------------|
| **$name** | `?string` | The name of a collection to create an ID for |

{PANEL/}

## Related Articles

### Document Identifiers

- [Working with Document Identifiers](../../../../client-api/document-identifiers/working-with-document-identifiers)
- [Global ID Generation Conventions](../../../../client-api/configuration/identifier-generation/global)
- [Type-specific ID Generation Conventions](../../../../client-api/configuration/identifier-generation/type-specific)

### Knowledge Base

- [Document Identifier Generation](../../../../server/kb/document-identifier-generation)

### Operations

- [What are operations](../../../../client-api/operations/what-are-operations)
- [Get identities](../../../../client-api/operations/maintenance/identities/get-identities)
- [Seed identity](../../../../client-api/operations/maintenance/identities/seed-identity)
