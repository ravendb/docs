# Get Identities Operation

---

{NOTE: }

* Upon document creation, providing a collection name with a pipe symbol (`|`) 
  will cause the server to generate an ID for the new document called an **identity**.  
  E.g. `companies|`
 
* The identity document ID is unique across the entire cluster within the database scope.  
  It is composed of the provided collection name and an integer value that is continuously incremented.

* Identity values can also be managed from the Studio [identities](../../../../studio/database/documents/identities-view) view.

* Use `GetIdentitiesOperation` to get the dictionary that maps collection names to their corresponding latest identity values.

{INFO: }
Learn more about identities in:
[Document identifier generation](../../../../server/kb/document-identifier-generation#strategy--3)
{INFO/}

* In this page:

  * [Get identities operation](../../../../client-api/operations/maintenance/identities/get-identities#get-identities-operation)
  * [Syntax](../../../../client-api/operations/maintenance/identities/get-identities#syntax)

{NOTE/}

---

{PANEL: Get identities operation }

{CODE:nodejs get_identities@client-api\Operations\Maintenance\Identities\getIdentities.js /}

{PANEL/}

{PANEL: Syntax }

{CODE:nodejs syntax@client-api\Operations\Maintenance\Identities\getIdentities.js /}

{PANEL/}

## Related Articles

### Document Identifiers

- [Global ID Generation Conventions](../../../../client-api/configuration/identifier-generation/global)
- [Type-specific ID Generation Conventions](../../../../client-api/configuration/identifier-generation/type-specific)

### Knowledge Base

- [Document Identifier Generation](../../../../server/kb/document-identifier-generation#strategy--3)

### Operations

- [What are Operations](../../../../client-api/operations/what-are-operations)
- [Seed identity](../../../../client-api/operations/maintenance/identities/seed-identity)
- [Increment next identity](../../../../client-api/operations/maintenance/identities/increment-next-identity)
