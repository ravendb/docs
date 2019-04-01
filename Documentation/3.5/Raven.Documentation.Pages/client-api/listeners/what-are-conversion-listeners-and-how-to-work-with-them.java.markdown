# Listeners: What are conversion listeners and how to work with them?

Conversion listeners provide users with hook for an additional logic when converting an entity to a document/metadata pair and backwards.

`IDocumentConversionListener` provides methods to inject custom actions before or after actual document/entity conversion.

{CODE:java document_conversion_interface@ClientApi\Listeners\Conversion.java /}

##Example

Let's consider a case that we want to convert a metadata value provided by RavenDB server to a property of our class. 
To achieve this we are going to ignore the property after the entity is converted to the document and set the value of this property after the entity is created from the document/metadata pair.

{CODE:java document_conversion_example@ClientApi\Listeners\Conversion.java /}


