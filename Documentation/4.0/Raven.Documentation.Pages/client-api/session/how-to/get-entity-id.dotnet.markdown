# Session: How to Get Entity ID

Entities does not have to contain an ID property or field. In the case of such an entity, and a need for knowing under what ID it is stored on the server, the `GetDocumentId` method was created.

## Syntax

{CODE get_document_id_1@ClientApi\Session\HowTo\GetDocumentId.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | object | Instance of an entity for which an ID will be returned |

| Return Value | |
| ------------- | ----- |
| string | Returns the ID for a specified entity. The method may return `null` if `entity` is **null, isn't tracked, or the ID will be generated on the server**. |

## Example

{CODE get_document_id_3@ClientApi\Session\HowTo\GetDocumentId.cs /}

{CODE get_document_id_2@ClientApi\Session\HowTo\GetDocumentId.cs /}

## Related Articles

### Document Identifiers

- [Working with Document Identifiers](../../../client-api/document-identifiers/working-with-document-identifiers)
- [Global ID Generation Conventions](../../../client-api/configuration/identifier-generation/global)
- [Type-specific ID Generation Conventions](../../../client-api/configuration/identifier-generation/type-specific)

### Knowledge Base

- [Document Identifier Generation](../../../server/kb/document-identifier-generation)
