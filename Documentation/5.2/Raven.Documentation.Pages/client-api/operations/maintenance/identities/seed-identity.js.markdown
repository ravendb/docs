# Seed Identity Operation

---

{NOTE: }

* Use `SeedIdentityForOperation` to set the latest identity value for the specified collection.
  
* The next document that will be created using an identity for the collection will receive the consecutive integer value.

* Identity values can also be managed from the Studio [identities view](../../../../studio/database/documents/identities-view).

* In this page:
  * [Set a higher identity value](../../../../client-api/operations/maintenance/identities/seed-identity#set-a-higher-identity-valure)
  * [Force a smaller identity value](../../../../client-api/operations/maintenance/identities/seed-identity#force-a-smaller-identity-value)
  * [Syntax](../../../../client-api/operations/maintenance/identities/seed-identity#syntax)

{NOTE/}

---

{PANEL: Set a higher identity value }

You can replace the latest identity value on the server with a new, __higher__ number. 

{CODE:nodejs seed_identity_1@ClientApi\Operations\Maintenance\Identities\seedIdentity.js /}

{PANEL/}

{PANEL: Force a smaller identity value }

* You can set the latest identity value to a number that is __smaller__ than the current latest value.

* Before proceeding, first ensure that documents with an identity value higher than the new number do not exist.

{CODE:nodejs seed_identity_2@ClientApi\Operations\Maintenance\Identities\seedIdentity.js /}

{PANEL/}

{PANEL: Syntax }

{CODE:nodejs syntax@ClientApi\Operations\Maintenance\Identities\seedIdentity.js /}

| Parameter       | Type    | Description                                                                                                                               |
|-----------------|---------|-------------------------------------------------------------------------------------------------------------------------------------------|
| __name__        | string  | The collection name for which to seed the identity value.<br>Can be with or without a pipe in the end (e.g. "companies" or "companies\|". |
| __value__       | number  | The number to set as the latest identity value.                                                                                           |
| __forceUpdate__ | boolean | `true` - force a new value that is smaller than the latest.<br>`false` - only a higher value can be set.                                  |

{PANEL/}

## Related Articles

### Document Identifiers

- [Global ID Generation Conventions](../../../../client-api/configuration/identifier-generation/global)
- [Type-specific ID Generation Conventions](../../../../client-api/configuration/identifier-generation/type-specific)

### Knowledge Base

- [Document Identifier Generation](../../../../server/kb/document-identifier-generation)

### Operations

- [What are operations](../../../../client-api/operations/what-are-operations)
- [Get identities](../../../../client-api/operations/maintenance/identities/get-identities)
- [Increment next identity](../../../../client-api/operations/maintenance/identities/increment-next-identity)
