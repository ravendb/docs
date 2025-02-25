# Delete Revisions Operation
---

{NOTE: }

* Use `DeleteRevisionsOperation` to delete document revisions.  
  The document itself is not deleted, only its revisions will be deleted, as specified by the operation's params.

* Existing revisions will be deleted regardless of the current [revisions settings](../../../../studio/database/settings/document-revisions),  
  even if these settings are disabled.

* When working with a secure server:  
  * The delete revisions action will be logged in the [audit log](../../../../server/security/audit-log/audit-log).  
  * This operation is only available for a client certificate with a [security clearance](../../../../server/security/authorization/security-clearance-and-permissions) of _DatabaseAdmin_ or higher.

* By default, the operation will be applied to the [default database](../../../../client-api/setting-up-default-database).  
  To operate on a different database see [switch operations to different database](../../../../client-api/operations/how-to/switch-operations-to-a-different-database).  

* In this page:  
  * [Delete all revisions - single document](../../../../document-extensions/revisions/client-api/operations/delete-revisions#delete-all-revisions---single-document)
  * [Delete revisions - multiple documents](../../../../document-extensions/revisions/client-api/operations/delete-revisions#delete-revisions---multiple-documents)
  * [Delete revisions by time frame](../../../../document-extensions/revisions/client-api/operations/delete-revisions#delete-revisions-by-time-frame)  
  * [Delete revisions by change vectors](../../../../document-extensions/revisions/client-api/operations/delete-revisions#delete-revisions-by-change-vectors)  
  * [Syntax](../../../../document-extensions/revisions/client-api/operations/delete-revisions#syntax)  

{NOTE/}

---

{PANEL: Delete all revisions - single document}

In this example, we delete ALL revisions of a document.  
Both types of revisions, those resulting from the [revisions settings](../../../../studio/database/settings/document-revisions) and those generated manually via  
[force revision creation](../../../../document-extensions/revisions/overview#force-revision-creation), will be deleted.

{CODE-TABS}
{CODE-TAB:csharp:Sync delete_revisions_1@DocumentExtensions\Revisions\ClientAPI\Operations\DeleteRevisions.cs /}
{CODE-TAB:csharp:Async delete_revisions_1_async@DocumentExtensions\Revisions\ClientAPI\Operations\DeleteRevisions.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Delete revisions - multiple documents}

You can specify multiple documents from which to delete revisions.

{CODE-TABS}
{CODE-TAB:csharp:Sync delete_revisions_2@DocumentExtensions\Revisions\ClientAPI\Operations\DeleteRevisions.cs /}
{CODE-TAB:csharp:Async delete_revisions_2_async@DocumentExtensions\Revisions\ClientAPI\Operations\DeleteRevisions.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Delete revisions by time frame}

You can specify a time frame from which to delete revisions.  
Only revisions that were created within that time frame (inclusive) will be deleted.  
The time should be specified in UTC.

{CODE-TABS}
{CODE-TAB:csharp:Sync delete_revisions_3@DocumentExtensions\Revisions\ClientAPI\Operations\DeleteRevisions.cs /}
{CODE-TAB:csharp:Async delete_revisions_3_async@DocumentExtensions\Revisions\ClientAPI\Operations\DeleteRevisions.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Delete revisions by change vectors}

Each revision has its own unique [change vector](../../../../document-extensions/revisions/client-api/session/loading#get-revisions-by-change-vector).  
You can specify which revisions to delete by providing their corresponding change vectors.  
No exception is thrown if a change vector doesn’t match any revision.

{CODE-TABS}
{CODE-TAB:csharp:Sync delete_revisions_4@DocumentExtensions\Revisions\ClientAPI\Operations\DeleteRevisions.cs /}
{CODE-TAB:csharp:Async delete_revisions_4_async@DocumentExtensions\Revisions\ClientAPI\Operations\DeleteRevisions.cs /}
{CODE-TABS/}

{CONTENT-FRAME: }

Avoid deleting a "Delete Revision" using the `DeleteRevisionsOperation` operation.  
Consider the following scenario:  
 
  1. A document that has revisions is deleted.
   
  2. A "Delete Revision" is created for the document, and it will be listed in the [Revisions Bin](../../../../studio/database/document-extensions/revisions/revisions-bin).

  3. The revisions of this deleted document remain accessible via the Revisions Bin.
   
  4. If you remove this "Delete Revision" by providing its change vector to `DeleteRevisionsOperation`,  
     the "Delete Revision" will be removed from the Revisions Bin, causing the associated revisions to become orphaned.  
     However, you will still be able to access these orphaned revisions from the [All Revisions](../../../../studio/database/document-extensions/revisions/all-revisions) view.

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Syntax}

{CODE:csharp syntax_1@DocumentExtensions\Revisions\ClientAPI\Operations\DeleteRevisions.cs /}

| Parameter                       | Type           | Description                                                                                                                                                                           |
|---------------------------------|----------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **documentId**                  | `string`       | The ID of the document whose revisions you want to delete.                                                                                                                            |
| **documentIds**                 | `List<string>` | A list of document IDs whose revisions you want to delete.                                                                                                                            |
| **removeForceCreatedRevisions** | `bool`         | `true` - Include [force-created revisions](../../../../document-extensions/revisions/overview#force-revision-creation) in the deletion.<br>`false` - Exclude force-created revisions. |
| **from**                        | `DateTime`     | The start of the date range for the revisions to delete (inclusive).                                                                                                                  |
| **to**                          | `DateTime`     | The end of the date range for the revisions to delete (inclusive).                                                                                                                    |
| **revisionsChangeVectors**      | `List<string>` | A list of change vectors corresponding to the revisions that you want to delete.                                                                                                      |

{PANEL/}

## Related Articles

### Document Extensions

* [Revisions Overview](../../../../document-extensions/revisions/overview)  
* [Revert Revisions](../../../../document-extensions/revisions/revert-revisions)  
* [Revisions and Other Features](../../../../document-extensions/revisions/revisions-and-other-features)  
* [Revisions: API Overview](../../../../document-extensions/revisions/client-api/overview)
* [Session: Loading Revisions](../../../../document-extensions/revisions/client-api/session/loading)
* [Session: Including Revisions](../../../../document-extensions/revisions/client-api/session/including)
* [Session: Counting Revisions](../../../../document-extensions/revisions/client-api/session/counting)

### Client API

* [What Are Operations](../../../../client-api/operations/what-are-operations)
* [Switch Operation Database](../../../../client-api/operations/how-to/switch-operations-to-a-different-database)
* [Setting Up a Default Database](../../../../client-api/setting-up-default-database)

### Studio

* [Settings: Document Revisions](../../../../studio/database/settings/document-revisions)  
* [Document Extensions: Revisions](../../../../studio/database/document-extensions/revisions)  
* [Manage Database Group](../../../../studio/database/settings/manage-database-group)
