# Changes API: How to Subscribe to Document Changes

Following methods allow you to subscribe to document changes:

- [forDocument](../../client-api/changes/how-to-subscribe-to-document-changes#fordocument)
- [forDocumentsInCollection](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsincollection)
- [forDocumentsStartingWith](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsstartingwith)
- [forAllDocuments](../../client-api/changes/how-to-subscribe-to-document-changes#foralldocuments)

{PANEL:forDocument}

Single document changes can be observed using `forDocument` method.

### Syntax

{CODE:java document_changes_1@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **docId** | String | ID of a document for which notifications will be processed. |

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[DocumentChange](../../client-api/changes/how-to-subscribe-to-document-changes#documentchange)> | Observable that allows to add subscriptions to notifications for given document. |

### Example

{CODE:java document_changes_2@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

{PANEL/}

{PANEL:forDocumentsInCollection}

To observe all document changes in particular collection use `forDocumentInCollection` method. This method filters documents by `@collection` metadata property value.

### Syntax

{CODE:java document_changes_3@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **collectionName** | String | Name of document collection for which notifications will be processed. |

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[DocumentChange](../../client-api/changes/how-to-subscribe-to-document-changes#documentchange)> | Observable that allows to add subscriptions to notifications for given document collection name. |

{INFO Overload with `TEntity` type uses `conventions.GetCollectionName` to get collection name. /}

### Example

{CODE:java document_changes_4@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

or

{CODE:java document_changes_5@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

{PANEL/}

{PANEL:forDocumentsStartingWith}

To observe all document changes for documents with ID that contains given prefix use `forDocumentsStartingWith` method.

### Syntax

{CODE:java document_changes_9@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **docIdPrefix** | String | Document ID prefix for which notifications will be processed. |

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[DocumentChange](../../client-api/changes/how-to-subscribe-to-document-changes#documentchange)> | Observable that allows to add subscriptions to notifications for given document ID prefix. |

### Example

{CODE:java document_changes_1_0@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

{PANEL/}

{PANEL:forAllDocuments}

To observe all document changes use `forAllDocuments` method.

### Syntax

{CODE:java document_changes_1_1@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[DocumentChange](../../client-api/changes/how-to-subscribe-to-document-changes#documentchange)> | Observable that allows to add subscriptions to notifications for all documents. |

### Example

{CODE:java document_changes_1_2@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

{PANEL/}

{PANEL:DocumentChange}

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Type** | [DocumentChangeTypes](../../client-api/changes/how-to-subscribe-to-document-changes#documentchangetypes) | Document change type enum |
| **Id** | String | Document identifier |
| **CollectionName** | String | Document's collection name |
| **TypeName** | String | Type name |
| **ChangeVector** | String | Document's ChangeVector|

{PANEL/}

{PANEL:DocumentChangeTypes}

| Name |
| ---- |
| **NONE** |
| **PUT** |
| **DELETE** |
| **BULK_INSERT_STARTED** |
| **BULK_INSERT_ENDED** |
| **BULK_INSERT_ERROR** |
| **DELETE_ON_TOMBSTONE_REPLICATION** |
| **CONFLICT** |
| **COMMON** |

{PANEL/}

## Related Articles

### Changes API

- [What is Changes API](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)
- [How to Subscribe to Operation Changes](../../client-api/changes/how-to-subscribe-to-operation-changes)
