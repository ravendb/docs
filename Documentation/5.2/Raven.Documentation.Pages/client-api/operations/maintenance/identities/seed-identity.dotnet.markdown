# Seed Identity Operation

---

{NOTE: }

* Use `SeedIdentityForOperation` to set the latest identity value for the specified collection in the database.
  
* The next document that will be created using an identity for the collection will receive the consecutive integer value.

* Identity values can also be managed from the Studio [identities view](../../../../todo..).

* In this page:

  * [Seed identity operation](../../../../client-api/operations/maintenance/identities/seed-identities#seed-identity-operation)
  * [Syntax](../../../../client-api/operations/maintenance/identities/seed-identities#syntax)

{NOTE/}

---

{PANEL: Seed identity operation }

{CODE-TABS}
{CODE-TAB:csharp:Sync seed_identity@ClientApi\Operations\Maintenance\Identities\SeedIdentity.cs /}
{CODE-TAB:csharp:Async seed_identity_async@ClientApi\Operations\Maintenance\Identities\SeedIdentity.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax }

{CODE syntax@ClientApi\Operations\Maintenance\Identities\SeedIdentity.cs /}

| Parameter       | Type   | Description                                                                                                                               |
|-----------------|--------|-------------------------------------------------------------------------------------------------------------------------------------------|
| __name__        | string | The collection name for which to seed the identity value.<br>Can be with or without a pipe in the end (e.g. "companies" or "companies\|". |
| __value__       | long   | The number to set as the latest identity value.                                                                                           |
| __forceUpdate__ | bool   | ??? todo...                                                                                                                               |

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
- [Increment next identity](../../../../client-api/operations/increment-next-identity)
