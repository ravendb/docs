# Get Identities Operation

---

{NOTE: }

* Upon document creation, providing a collection name with a pipe symbol (`|`)  
  will cause the server to generate an ID for the new document called an __identity__.
 
* The identity document ID is unique across the entire cluster within the database scope.  
  It is composed  of the collection name provided and an integer value that is continuously incremented.

* Identity values can also be managed from the Studio [identities view](../../../../studio/database/documents/identities-view).

* Use `GetIdentitiesOperation` to get the dictionary that maps collection names to their corresponding latest identity values.

* Learn more about identities in:

    * [Document identifier generation](../../../../server/kb/document-identifier-generation#identity)
    * [Working with document identifiers](../../../../client-api/document-identifiers/working-with-document-identifiers#identities)

* In this page:

  * [Get identities operation](../../../../client-api/operations/maintenance/identities/get-identities#get-identities-operation)
  * [Syntax](../../../../client-api/operations/maintenance/identities/get-identities#syntax)

{NOTE/}

---

{PANEL: Get identities operation }

{CODE-TABS}
{CODE-TAB:csharp:Sync get_identities@ClientApi\Operations\Maintenance\Identities\GetIdentities.cs /}
{CODE-TAB:csharp:Async get_identities_async@ClientApi\Operations\Maintenance\Identities\GetIdentities.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax }

{CODE syntax@ClientApi\Operations\Maintenance\Identities\GetIdentities.cs /}

{PANEL/}

## Related Articles

### Document Identifiers

- [Working with Document Identifiers](../../../../client-api/document-identifiers/working-with-document-identifiers)
- [Global ID Generation Conventions](../../../../client-api/configuration/identifier-generation/global)
- [Type-specific ID Generation Conventions](../../../../client-api/configuration/identifier-generation/type-specific)

### Knowledge Base

- [Document Identifier Generation](../../../../server/kb/document-identifier-generation)

### Operations

- [What are Operations](../../../../client-api/operations/what-are-operations)
- [Seed identity](../../../../client-api/operations/maintenance/identities/seed-identity)
- [Increment next identity](../../../../client-api/operations/maintenance/identities/increment-next-identity)
