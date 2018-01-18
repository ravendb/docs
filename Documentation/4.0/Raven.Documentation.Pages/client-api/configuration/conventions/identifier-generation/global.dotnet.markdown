#Global identifier generation conventions

###FindCollectionName and FindCollectionNameForDynamic

Entity objects that share a common tag name belong to the same [collection](../../../../client-api/faq/what-is-a-collection) on the server side. Collection names are also used to build document Identifiers. There are two functions that the client uses to determine the collection name. The first one is used for standard objects with well defined type:

{CODE find_type_collection_name@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

The second one is dedicated for dynamic objects:

{CODE find_dynamic_collection_name@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

###TransformTypeCollectionNameToDocumentIdPrefix

Actually, collection names determined by recently described convention functions aren't directly used as prefixes in document Identifiers. There is a convention function called `TransformTypeCollectionNameToDocumentIdPrefix` which takes the collection name and produces the prefix:

{CODE transform_collection_name_to_prefix@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

Its default behavior is that for a collection which contains one upper character it simply converts it to lower case string, e.g. `Users` would be transformed into `users`. For collection names containing more upper characters there will be no change, e.g. the collection name: `LineItems` would output the following prefix: `LineItems`.

###FindClrTypeName and FindClrType

In metadata of all documents stored in a database you can find the following property which specifies the client-side type. For instance:

{CODE-BLOCK:json}
{
    "Raven-Clr-Type": "Orders.Shipper, Northwind"
}
{CODE-BLOCK/}

This property is used by RavenDB client to perform a conversion between .NET object and JSON document stored in a database. A function responsible for retrieving the CLR type of an entity is defined by `FindClrTypeName` convention:

{CODE find_type_name@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

To properly perform the revert conversion that is from a JSON result into a .NET object we need to retrieve the CLR type from `Raven-Clr-Type` metadata:

{CODE find_clr_type@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}


###FindIdentityProperty

The client must know where in your entity an identifier is stored to be property able to transform it into JSON document. It uses `FindIdentityProperty` convention for that. The default and very common convention is that a property named `Id` is the identifier, so it is the default one:

{CODE find_identity_property@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

You can provide a customization based on the `MemberInfo` parameter to indicate which property or field keeps the identifier. The client will iterate over all object properties and take the first one according to the defined predicate.

###FindIdentityPropertyNameFromCollectionName

It can happen that sometimes the results returned by the server doesn't have identifiers defined (for example if you run a projection query) however they have `@collection` in metadata.
Then to perform the conversion into .NET object a function that finds the identity property name for a given entity name is applied:

{CODE find_identity_property_name_from_collection_name@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

###IdentityPartsSeparator

According to the default, convention document identifiers have the following format: `[collectionName]/[identityValue]-[nodeName]`. The slash character (`/`) separates the two parts of an identifier.
You can overwrite it by using `IdentityPartsSeparator` convention. Its default definition is:

{CODE identity_part_separator@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

##Related articles

- [Document identifier generatation](../../../../server/kb/document-identifier-generation)
- [Working with document identifiers](../../../document-identifiers/working-with-document-identifiers)
- [What is a collection?](../../../faq/what-is-a-collection)
- [Type-specific identifier generation](./type-specific)
