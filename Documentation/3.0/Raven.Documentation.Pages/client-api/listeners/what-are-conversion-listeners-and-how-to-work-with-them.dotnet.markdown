# Client API : What are conversion listeners and how to work with them?

Conversion listeners provide users with hook for additional logic when converting an entity to a document/metadata pair and backwards.

There are two conversion listeners available:

* `IDocumentConversionListener` contains methods called at the moment of performing the actual conversion,

{CODE document_conversion_interface@ClientApi\Listeners\Conversion.cs /}

* `IExtendedDocumentConversionListener` provides methods to inject custom actions before or after actual document/entity conversion.

{CODE document_extended_conversion_interface@ClientApi\Listeners\Conversion.cs /}

##Example

Let's consider a case that we want to convert a metadata value provided by RavenDB server to a property of our class. 
To achieve this we are going to ignore the property when the entity is converted to document and set the value of this property when the entity is created from the document/metadata pair.

{CODE document_conversion_example@ClientApi\Listeners\Conversion.cs /}


