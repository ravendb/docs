# Changes API: How to Subscribe to Document Changes

Following methods allow you to subscribe to document changes:

- [forDocument()](../../client-api/changes/how-to-subscribe-to-document-changes#fordocument)
- [forDocumentsInCollection()](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsincollection)
- [forDocumentsStartingWith()](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsstartingwith)
- [forAllDocuments()](../../client-api/changes/how-to-subscribe-to-document-changes#foralldocuments)

{PANEL:forDocument}

Single document changes can be observed using `forDocument()` method.

### Syntax

{CODE:nodejs document_changes_1@client-api\changes\howToSubscribeToDocumentChanges.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **docId** | string | ID of a document for which notifications will be processed. |

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[DocumentChange](../../client-api/changes/how-to-subscribe-to-document-changes#documentchange)> | Observable that allows to add listeners for events for given document. |

### Example

{CODE:nodejs document_changes_2@client-api\changes\howToSubscribeToDocumentChanges.js /}

{PANEL/}

{PANEL:forDocumentsInCollection}

To observe all document changes in particular collection use `forDocumentInCollection()` method. This method filters documents by `@collection` metadata property value.

### Syntax

{CODE:nodejs document_changes_3@client-api\changes\howToSubscribeToDocumentChanges.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **collectionName** | string | Name of document collection for which notifications will be processed. |

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[DocumentChange](../../client-api/changes/how-to-subscribe-to-document-changes#documentchange)> | Observable that allows to add subscriptions to notifications for given document collection name. |

{INFO Overload with entity type uses `conventions.getCollectionNameForType()` to get collection name. /}

### Example

{CODE:nodejs document_changes_4@client-api\changes\howToSubscribeToDocumentChanges.js /}

or

{CODE:nodejs document_changes_5@client-api\changes\howToSubscribeToDocumentChanges.js /}

{PANEL/}

{PANEL:forDocumentsStartingWith}

To observe all document changes for documents with ID that contains given prefix use `forDocumentsStartingWith()` method.

### Syntax

{CODE:nodejs document_changes_9@client-api\changes\howToSubscribeToDocumentChanges.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **docIdPrefix** | string | Document ID prefix for which notifications will be processed. |

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[DocumentChange](../../client-api/changes/how-to-subscribe-to-document-changes#documentchange)> | Observable that allows to add subscriptions to notifications for given document ID prefix. |

### Example

{CODE:nodejs document_changes_1_0@client-api\changes\howToSubscribeToDocumentChanges.js /}

{PANEL/}

{PANEL:forAllDocuments}

To observe all document changes use `forAllDocuments()` method.

### Syntax

{CODE:nodejs document_changes_1_1@client-api\changes\howToSubscribeToDocumentChanges.js /}

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[DocumentChange](../../client-api/changes/how-to-subscribe-to-document-changes#documentchange)> | Observable that allows to add subscriptions to notifications for all documents. |

### Example

{CODE:nodejs document_changes_1_2@client-api\changes\howToSubscribeToDocumentChanges.js /}

{PANEL/}

{PANEL:DocumentChange}

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **type** | [DocumentChangeTypes](../../client-api/changes/how-to-subscribe-to-document-changes#documentchangetypes) | Document change type enum |
| **id** | string | Document identifier |
| **collectionName** | string | Document's collection name |
| **typeName** | string | Type name |
| **changeVector** | string | Document's ChangeVector|

{PANEL/}

{PANEL:DocumentChangeTypes}

| Name |
| ---- |
| **None** |
| **Put** |
| **Delete** |
| **BulkInsertStarted** |
| **BulkInsertEnded** |
| **BulkInsertError** |
| **DeleteOnTombstoneReplication** |
| **Conflict** |
| **Common** |

{PANEL/}

## Related Articles

### Changes API

- [What is Changes API](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)
- [How to Subscribe to Operation Changes](../../client-api/changes/how-to-subscribe-to-operation-changes)
