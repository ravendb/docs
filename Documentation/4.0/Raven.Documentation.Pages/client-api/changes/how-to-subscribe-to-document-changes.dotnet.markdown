# Changes API: How to Subscribe to Document Changes

Following methods allow you to subscribe to document changes:

- [ForDocument](../../client-api/changes/how-to-subscribe-to-document-changes#fordocument)
- [ForDocumentsInCollection](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsincollection)
- [ForDocumentsStartingWith](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsstartingwith)
- [ForAllDocuments](../../client-api/changes/how-to-subscribe-to-document-changes#foralldocuments)

{PANEL:ForDocument}

Single document changes can be observed using `ForDocument` method.

### Syntax

{CODE document_changes_1@ClientApi\Changes\HowToSubscribeToDocumentChanges.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **docId** | string | ID of a document for which notifications will be processed. |

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[DocumentChange](../../client-api/changes/how-to-subscribe-to-document-changes#documentchange)> | Observable that allows to add subscriptions to notifications for given document. |

### Example

{CODE document_changes_2@ClientApi\Changes\HowToSubscribeToDocumentChanges.cs /}

{PANEL/}

{PANEL:ForDocumentsInCollection}

To observe all document changes in particular collection use `ForDocumentInCollection` method. This method filters documents by `@collection` metadata property value.

### Syntax

{CODE document_changes_3@ClientApi\Changes\HowToSubscribeToDocumentChanges.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **collectionName** | string | Name of document collection for which notifications will be processed. |

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[DocumentChange](../../client-api/changes/how-to-subscribe-to-document-changes#documentchange)> | Observable that allows to add subscriptions to notifications for given document collection name. |

{INFO Overload with `TEntity` type uses `Conventions.GetCollectionName` to get collection name. /}

### Example

{CODE document_changes_4@ClientApi\Changes\HowToSubscribeToDocumentChanges.cs /}

or

{CODE document_changes_5@ClientApi\Changes\HowToSubscribeToDocumentChanges.cs /}

{PANEL/}

{PANEL:ForDocumentsStartingWith}

To observe all document changes for documents with ID that contains given prefix use `ForDocumentsStartingWith` method.

### Syntax

{CODE document_changes_9@ClientApi\Changes\HowToSubscribeToDocumentChanges.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **docIdPrefix** | string | Document ID prefix for which notifications will be processed. |

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[DocumentChange](../../client-api/changes/how-to-subscribe-to-document-changes#documentchange)> | Observable that allows to add subscriptions to notifications for given document ID prefix. |

### Example

{CODE document_changes_1_0@ClientApi\Changes\HowToSubscribeToDocumentChanges.cs /}

{PANEL/}

{PANEL:ForAllDocuments}

To observe all document changes use `ForAllDocuments` method.

### Syntax

{CODE document_changes_1_1@ClientApi\Changes\HowToSubscribeToDocumentChanges.cs /}

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[DocumentChange](../../client-api/changes/how-to-subscribe-to-document-changes#documentchange)> | Observable that allows to add subscriptions to notifications for all documents. |

### Example

{CODE document_changes_1_2@ClientApi\Changes\HowToSubscribeToDocumentChanges.cs /}

{PANEL/}

{PANEL:DocumentChange}

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Type** | [DocumentChangeTypes](../../client-api/changes/how-to-subscribe-to-document-changes#documentchangetypes) | Document change type enum |
| **Id** | string | Document identifier |
| **CollectionName** | string | Document's collection name |
| **TypeName** | string | Type name |
| **ChangeVector** | string | Document's ChangeVector|

{PANEL/}

{PANEL:DocumentChangeTypes}

| Name | Value |
| ---- | ----- |
| **None** | `0` |
| **Put** | `1` |
| **Delete** | `2` |
| **BulkInsertStarted** | `4` |
| **BulkInsertEnded** | `8` |
| **BulkInsertError** | `16` |
| **DeleteOnTombstoneReplication** | `32` |
| **Conflict** | `64` |
| **Common** | `Put & Delete` |

{PANEL/}

## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions Core](https://www.nuget.org/packages/System.Reactive.Core/) package to your project. /}

## Related Articles

### Changes API

- [What is Changes API](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)
- [How to Subscribe to Operation Changes](../../client-api/changes/how-to-subscribe-to-operation-changes)
