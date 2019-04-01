# Session: Querying: How to use highlighting?

**Highlighting** can be a great feature for increasing search UX. To take leverage of it use `highlight` method which is a part of query customizations available from `customize`.

## Syntax

{CODE:java highlight_1@ClientApi\Session\Querying\HowToUseHighlighting.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | String | Name of a field to highlight. |
| **fragmentLength** | int | Maximum length of text fragments that will be returned. |
| **fragmentCount** | int | Maximum number of fragments that will be returned. |
| **fragmentsField** | String | Field in returned results containing highlight fragments (mutually exclusive with 'highlightings'). |
| **highlightings** | [FieldHighlightings](../../../glossary/field-highlightings) | Instance of a FieldHighlightings that contains the highlight fragments for each returned result (mutually exclusive with 'fragmentsField'). |

| Return Value | |
| ------------- | ----- |
| IDocumentQueryCustomization | Returns self for easier method chaining. |
| [FieldHighlightings](../../../glossary/field-highlightings) | Instance of a FieldHighlightings that contains the highlight fragments for each returned result (mutually exclusive with 'fragmentsField'). |

## Example

{CODE:java highlight_2@ClientApi\Session\Querying\HowToUseHighlighting.java /}

## Related articles

- [Indexes : Querying : Highlights](../../../indexes/querying/highlights)
