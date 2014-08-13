# Querying : How to use highlighting?

**Highlighting** can be a great feature for increasing search UX. To take leverage of it use `Highlight` method which is a part of query customizations available from `Customize`.

## Syntax

{CODE highlight_1@ClientApi\Session\Querying\HowToUseHighlighting.cs /}

**Parameters**   

fieldName
:   Type: string   
Name of a field to highlight.

fragmentLength
:   Type: int   
Maximum length of text fragments that will be returned.

fragmentCount
:   Type: int   
Maximum number of fragments that will be returned.

fragmentsField
:   Type: string   
Field in returned results containing highlight fragments (mutually exclusive with 'highlightings').

highlightings
:   Type: [FieldHighlightings](../../../glossary/client-api/querying/field-highlightings)     
Instance of a FieldHighlightings that contains the highlight fragments for each returned result (mutually exclusive with 'fragmentsField').

**Return Value**

Type: IDocumentQueryCustomization   
Returns self for easier method chaining.

highlightings
:   Type: [FieldHighlightings](../../../glossary/client-api/querying/field-highlightings)   
Instance of a FieldHighlightings that contains the highlight fragments for each returned result (mutually exclusive with 'fragmentsField').

## Example

{CODE highlight_2@ClientApi\Session\Querying\HowToUseHighlighting.cs /}

#### Related articles

TODO