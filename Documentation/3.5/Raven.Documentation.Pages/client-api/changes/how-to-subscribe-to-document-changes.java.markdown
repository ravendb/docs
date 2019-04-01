# Changes API: How to subscribe to document changes?

Following methods allow you to subscribe to document changes:

- [ForDocument](../../client-api/changes/how-to-subscribe-to-document-changes#fordocument)
- [ForDocumentsInCollection](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsincollection)
- [ForDocumentsOfType](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsoftype)
- [ForDocumentsStartingWith](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsstartingwith)
- [ForAllDocuments](../../client-api/changes/how-to-subscribe-to-document-changes#foralldocuments)

{PANEL:ForDocument}

Single document changes can be observed using `forDocument` method.

### Syntax

{CODE:java document_changes_1@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **docId** | String | Id of a document for which notifications will be processed. |

| Return Value | |
| ------------- | ----- |
| IObservable<[DocumentChangeNotification](../../glossary/document-change-notification)> | Observable that allows to add subscriptions to notifications for given document. |

### Example

{CODE:java document_changes_2@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

{PANEL/}

{PANEL:ForDocumentsInCollection}

To observe all document changes in particular collection use `forDocumentInCollection` method. This method filters documents by `Raven-Entity-Name` metadata property value.

### Syntax

{CODE:java document_changes_3@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **collectionName** | String | Name of document collection for which notifications will be processed. |

| Return Value | |
| ------------- | ----- |
| IObservable<[DocumentChangeNotification](../../glossary/document-change-notification)> | Observable that allows to add subscriptions to notifications for given document collection name. |

{INFO Overload with `Class` uses `Conventions.getTypeTagName` to get collection name. /}

### Example

{CODE:java document_changes_4@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

or

{CODE:java document_changes_5@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

{PANEL/}

{PANEL:ForDocumentsOfType}

To observe all document changes for given type use `forDocumentsOfType` method. This method filters documents by `Raven-Clr-Type` metadata property value.

## Syntax

{CODE:java document_changes_6@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **typeName** or **clazz** | String or Class | Name of class or class for which notifications will be processed. If default conventions are used then full type name without version information should be passed.

| Return Value | |
| ------------- | ----- |
| IObservable<[DocumentChangeNotification](../../glossary/document-change-notification)> | Observable that allows to add subscriptions to notifications for given document type name. |

{INFO Overload with `Class` uses `Conventions.findClrTypeName` to get type name. /}

## Example

{CODE:java document_changes_7@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

or 

{CODE:java document_changes_8@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

{PANEL/}

{PANEL:ForDocumentsStartingWith}

To observe all document changes for documents with Id that contains given prefix use `forDocumentsStartingWith` method.

## Syntax

{CODE:java document_changes_9@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **docIdPrefix** | String | Document Id prefix for which notifications will be processed. |

| Return Value | |
| ------------- | ----- |
| IObservable<[DocumentChangeNotification](../../glossary/document-change-notification)> | Observable that allows to add subscriptions to notifications for given document Id prefix. |

## Example

{CODE:java document_changes_1_0@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

{PANEL/}

{PANEL:ForAllDocuments}

To observe all document changes use `forAllDocuments` method.

## Syntax

{CODE:java document_changes_1_1@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

| Return Value | |
| ------------- | ----- |
| IObservable<[DocumentChangeNotification](../../glossary/document-change-notification)> | Observable that allows to add subscriptions to notifications for all documents. |

## Example

{CODE:java document_changes_1_2@ClientApi\Changes\HowToSubscribeToDocumentChanges.java /}

{PANEL/}



