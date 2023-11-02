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

{CODE:nodejs increment_identity@ClientApi\Operations\Maintenance\Identities\incrementIdentity.js /}

{PANEL/}

{PANEL: Syntax }

{CODE:nodejs syntax@ClientApi\Operations\Maintenance\Identities\incrementIdentity.js /}

| Parameter | Type   | Description                                                                                                                                    |
|-----------|--------|------------------------------------------------------------------------------------------------------------------------------------------------|
| __name__  | string | The collection name for which to increment the identity value.<br>Can be with or without a pipe in the end (e.g. "companies" or "companies\|". |

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
