# Seed Identity Operation

---

{NOTE: }

* Use `SeedIdentityForOperation` to set the latest identity value for the specified collection.
  
* The next document that will be created using an identity for the collection will receive the consecutive integer value.

* Identity values can also be managed from the Studio [identities view](../../../../studio/database/documents/identities-view).

* In this page:
  * [Set a higher identity value](../../../../client-api/operations/maintenance/identities/seed-identity#set-a-higher-identity-value)
  * [Force a lower identity value](../../../../client-api/operations/maintenance/identities/seed-identity#force-a-lower-identity-value)
  * [Syntax](../../../../client-api/operations/maintenance/identities/seed-identity#syntax)

{NOTE/}

---

{PANEL: Set a higher identity value }

You can replace the latest identity value on the server with a new, **higher** number. 
 

{CODE-TABS}
{CODE-TAB:csharp:Sync seed_identity_1@ClientApi\Operations\Maintenance\Identities\SeedIdentity.cs /}
{CODE-TAB:csharp:Async seed_identity_1_async@ClientApi\Operations\Maintenance\Identities\SeedIdentity.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Force a lower identity value }

* You can set the latest identity value to a number that is **lower** than the current latest value.

* Before proceeding, first ensure that documents with an identity value higher than the new number do not exist.

{CODE-TABS}
{CODE-TAB:csharp:Sync seed_identity_2@ClientApi\Operations\Maintenance\Identities\SeedIdentity.cs /}
{CODE-TAB:csharp:Async seed_identity_2_async@ClientApi\Operations\Maintenance\Identities\SeedIdentity.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax }

{CODE syntax@ClientApi\Operations\Maintenance\Identities\SeedIdentity.cs /}

| Parameter       | Type     | Description                                                                                                                          |
|-----------------|----------|--------------------------------------------------------------------------------------------------------------------------------|
| **name**        | `string` | The collection name to seed the identity value for.<br>Can be ended with or without a pipe (e.g. "companies" or "companies\|". |
| **value**       | `long`   | The number to set as the latest identity value.                                                                                |
| **forceUpdate** | `bool`   | `true` - force a new value that is lower than the latest.<br>`false` - only a higher value can be set.                         |

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
- [Increment next identity](../../../../client-api/operations/maintenance/identities/increment-next-identity)
