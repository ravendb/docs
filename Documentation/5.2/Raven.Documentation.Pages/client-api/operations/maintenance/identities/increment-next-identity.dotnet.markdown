# Increment Next Identity Operation

---

{NOTE: }

* Use `NextIdentityForOperation` to increment by one the latest identity number set on the server for the specified collection in the database.

* The next document that will be created using an identity for the collection will get the incremented value.

* In this page:

  * [Increment the next identity operation](../../../../client-api/operations/maintenance/identities/get-identities#get-identities-operation)
  * [Syntax](../../../../client-api/operations/maintenance/identities/get-identities#syntax)

{NOTE/}

---

{PANEL: Increment the next identity operation }

{CODE-TABS}
{CODE-TAB:csharp:Sync increment_identity@ClientApi\Operations\Maintenance\Identities\IncrementIdentity.cs /}
{CODE-TAB:csharp:Async increment_identity_async@ClientApi\Operations\Maintenance\Identities\IncrementIdentity.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax }

{CODE syntax@ClientApi\Operations\Maintenance\Identities\IncrementIdentity.cs /}

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
- [Get identities](../../../../client-api/operations/get-identities)
- [Seed identity](../../../../client-api/operations/seed-identity)
