#Global Identifier Generation Conventions

##FindCollectionName

Documents that have the same `@collection` metadata belong to the same [collection](../../../client-api/faq/what-is-a-collection) on the server side. Collection names are also used to build document identifiers. 

{CODE:java find_type_collection_name@ClientApi\Configuration\IdentifierGeneration\Global.java /}

##TransformClassCollectionNameToDocumentIdPrefix

Collection names determined by recently described convention functions aren't directly used as prefixes in document identifiers. There is a convention function called `TransformClassCollectionNameToDocumentIdPrefix` which takes the collection name and produces the prefix:

{CODE:java transform_collection_name_to_prefix@ClientApi\Configuration\IdentifierGeneration\Global.java /}

Its default behavior is that for a collection which contains one upper character it simply converts it to lower case string. `Users` would be transformed into `users`. For collection names containing more upper characters there will be no change. The collection name: `LineItems` would output the following prefix: `LineItems`.

##FindJavaClassName and FindJavaClass

In the metadata of all documents stored by RavenDB Java Client, you can find the following property which specifies the client-side type. For instance:

{CODE-BLOCK:json}
{
    "Raven-Java-Type": "com.example.Customer"
}
{CODE-BLOCK/}

This property is used by RavenDB client to perform a conversion between a Java object and a JSON document stored in a database. A function responsible for retrieving the Java class of an entity is defined by `findJavaClassName` convention:

{CODE:java find_type_name@ClientApi\Configuration\IdentifierGeneration\Global.java /}

To properly perform the revert conversion that is from a JSON result into a Java object, we need to retrieve the Java class from the `Raven-Java-Type` metadata:

{CODE:java find_clr_type@ClientApi\Configuration\IdentifierGeneration\Global.java /}


##FindIdentityProperty

The client must know where in your entity an identifier is stored to be properly able to transform it into JSON document. It uses the `FindIdentityProperty` convention for that. The default and very common convention is that a property named `Id` is the identifier, so is the default one:

{CODE:java find_identity_property@ClientApi\Configuration\IdentifierGeneration\Global.java /}

You can provide a customization based on the `FieldInfo` parameter to indicate which property or field keeps the identifier. The client will iterate over all object properties and take the first one according to the defined predicate.

##FindIdentityPropertyNameFromCollectionName

It can happen that sometimes the results returned by the server don't have identifiers defined (for example if you run a projection query) however they have `@collection` in metadata.

To perform the conversion into a Java object, a function that finds the identity property name for a given entity name is applied:

{CODE:java find_identity_property_name_from_collection_name@ClientApi\Configuration\IdentifierGeneration\Global.java /}

##IdentityPartsSeparator

According to the default, convention document identifiers have the following format: `[collectionName]/[identityValue]-[nodeTag]`. The slash character (`/`) separates the two parts of an identifier.
You can overwrite it by using `IdentityPartsSeparator` convention. Its default definition is:

{CODE:java identity_part_separator@ClientApi\Configuration\IdentifierGeneration\Global.java /}

##Related Articles

- [Document identifier generation](../../../server/kb/document-identifier-generation)
- [Working with document identifiers](../../../client-api/document-identifiers/working-with-document-identifiers)
- [What is a collection?](../../../client-api/faq/what-is-a-collection)
- [Type-specific identifier generation](../../../client-api/configuration/identifier-generation/type-specific)
