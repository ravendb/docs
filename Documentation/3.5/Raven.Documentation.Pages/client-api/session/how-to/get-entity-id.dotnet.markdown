# Session: How to get entity Id?

Entities does not have to contain Id property or field. In case of such an entity and a need of knowing under what Id it is stored on server `GetDocumentId` method has been created.

## Syntax

{CODE get_document_id_1@ClientApi\Session\HowTo\GetDocumentId.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | object | Instance of an entity for which Id will be returned |

| Return Value | |
| ------------- | ----- |
| string | Returns Id for specified entity. Method may return `null` if `entity` is **null, isn't tracked or Id will be generated on the server**. |

## Example

{CODE get_document_id_3@ClientApi\Session\HowTo\GetDocumentId.cs /}

{CODE get_document_id_2@ClientApi\Session\HowTo\GetDocumentId.cs /}

## Related articles

 - [Working with document identifiers](../../document-identifiers/working-with-document-ids)
