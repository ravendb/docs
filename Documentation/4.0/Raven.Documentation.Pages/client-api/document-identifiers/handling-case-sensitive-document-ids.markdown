# Handling case sensitive document IDs

RavenDB document IDs are handled by design not to be case sensitive. Load operations will execute regardless of the letter casing of the IDs.
The trick to handle this case is encode the case sensitive key into a unique representation that can be stored in a case insensitive way.

{CODE document_ids_1@ClientApi\DocumentIdentifiers\HandlingCaseSensitiveDocumentIds.cs /}

The token is the read id that a .Load operations would use. Therefore what we have to do is override the stored naming.   

In this example we will use a prefix but you can use the typical convention:

{CODE document_ids_2@ClientApi\DocumentIdentifiers\HandlingCaseSensitiveDocumentIds.cs /}

The GetSortKey will ensure that the representation of the string is unique. Therefore each time that we perform a .Load operation we are actually pointing to the proper entity.
