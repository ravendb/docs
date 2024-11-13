# Revert Document to Revision Operation
---

{NOTE: }

* This article describes how to revert specific documents to specific revisions using the `RevertRevisionsByIdOperation` operation.

* To revert documents from all collections (or from selected collections) to a specified point in time,  
  see [Revert documents to revisions](../../../../document-extensions/revisions/revert-revisions). 

* By default, the operation will be applied to the [default database](../../../../client-api/setting-up-default-database).  
  To operate on a different database see [switch operations to different database](../../../../client-api/operations/how-to/switch-operations-to-a-different-database).  

* In this page:  
  * [Overview](../../../../document-extensions/revisions/client-api/operations/revert-document-to-revision#overview)
  * [Revert single document](../../../../document-extensions/revisions/client-api/operations/revert-document-to-revision#revert-single-document)
  * [Revert multiple documents](../../../../document-extensions/revisions/client-api/operations/revert-document-to-revision#revert-multiple-documents)
  * [Syntax](../../../../document-extensions/revisions/client-api/operations/revert-document-to-revision#syntax)  

{NOTE/}

---

{PANEL: Overview}

* To revert a document to a specific revision, provide the document ID and the change-vector of the target revision to the `RevertRevisionsByIdOperation` operation.  
  The document content will be overwritten by the content of the specified revision.

* An exception will be thrown if the revision's change-vector is not found, does not exist for the specified document, or belongs to a different document.

* Reverting a document with this operation can be executed even if the revisions configuration is disabled:
  * When revisions are **enabled**:  
    Reverting the document creates a new revision containing the content of the target revision.  
  * When revisions are **disabled**:  
    The document is reverted to the target revision without creating a new revision.  

* In addition to the document itself, reverting will impact Document Extensions as follows:  
  * **Attachments**:  
    If the target revision owns attachments, they are restored to their state when the revision was created.  
  * **Counters**:  
    If the target revision owns counters, they are restored to functionality with their values at the time the revision was created.  
  * **Time series**:  
    Time series data is Not reverted. Learn more [here](../../../../document-extensions/revisions/revisions-and-other-features#reverted-data-1).

* When executing this operation on a document that had revisions and was deleted, placing it in the Revisions Bin,
  the document will be **recreated** with the content of the specified target revision and will be removed from the Revisions Bin.

---

##### How to obtain a revision's change-vector:  

The change-vector of a revision can be obtained via:  

  * The Client API - follow the code in the examples [below](../../../../document-extensions/revisions/client-api/operations/revert-document-to-revision#revert-single-document)  
  * Or from the document view in the Studio  

![Get revision CV](images/get-cv-for-revision.png "Get the revision's change-vector")

1. Go to the Revisions tab in the document view.
2. Click a revision to view
3. The document view will display the content of the revision.  
   This top label indicates that you are viewing a revision and not the current document.
4. Click the copy button in the Properties pane to copy this revision's change-vector to your clipboard.

{PANEL/}

{PANEL: Revert single document}

Using RavenDB's sample data, document _orders/1-A_ has a total of 7 revisions.  
In this example, we revert document _orders/1-A_ to its very first revision.

{CODE-TABS}
{CODE-TAB:csharp:Sync revert_document_1@DocumentExtensions\Revisions\ClientAPI\Operations\RevertDocumentToRevisionOperation.cs /}
{CODE-TAB:csharp:Async revert_document_1_async@DocumentExtensions\Revisions\ClientAPI\Operations\RevertDocumentToRevisionOperation.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Revert multiple documents}

You can use the operation to revert multiple documents.  
Note: The documents do not need to belong to the same collection.

{CODE-TABS}
{CODE-TAB:csharp:Sync revert_document_2@DocumentExtensions\Revisions\ClientAPI\Operations\RevertDocumentToRevisionOperation.cs /}
{CODE-TAB:csharp:Async revert_document_2_async@DocumentExtensions\Revisions\ClientAPI\Operations\RevertDocumentToRevisionOperation.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:csharp syntax_1@DocumentExtensions\Revisions\ClientAPI\Operations\RevertDocumentToRevisionOperation.cs /}

| Parameter            | Type                         | Description                                                                                                                                 |
|----------------------|------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------|
| **id**               | `string`                     | The ID of the document to revert.                                                                                                           |
| **cv**               | `string`                     | The change vector of the revision to which the document should be reverted.                                                                 |
| **idToChangeVector** | `Dictionary<string, string>` | A dictionary where each key is a document ID, and each value is the change-vector of the revision to which the document should be reverted. |

{PANEL/}

## Related Articles

### Document Extensions

* [Revisions Overview](../../../../document-extensions/revisions/overview)  
* [Revert Documents to Revisions](../../../../document-extensions/revisions/revert-revisions)  
* [Revisions and Other Features](../../../../document-extensions/revisions/revisions-and-other-features)  
* [Revisions: API Overview](../../../../document-extensions/revisions/client-api/overview)

### Client API

* [What Are Operations](../../../../client-api/operations/what-are-operations)
* [Switch Operation Database](../../../../client-api/operations/how-to/switch-operations-to-a-different-database)
* [Setting Up a Default Database](../../../../client-api/setting-up-default-database)

### Studio

* [Settings: Document Revisions](../../../../studio/database/settings/document-revisions)  
* [Document Extensions: Revisions](../../../../studio/database/document-extensions/revisions)  
