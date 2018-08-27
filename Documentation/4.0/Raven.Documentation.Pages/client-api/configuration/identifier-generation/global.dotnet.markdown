#Global Identifier Generation Conventions

##FindCollectionName and FindCollectionNameForDynamic

Documents that have the same `@collection` metadata belong to the same [collection](../../../client-api/faq/what-is-a-collection) on the server side. Collection names are also used to build document identifiers. There are two functions that the client uses to determine a collection name for a given type. The first one is used for standard objects with a well-defined type:

{CODE find_type_collection_name@ClientApi\Configuration\IdentifierGeneration\Global.cs /}

The second one is dedicated for dynamic objects:

{CODE find_dynamic_collection_name@ClientApi\Configuration\IdentifierGeneration\Global.cs /}

{INFO: What is a Dynamic Object?}

The `FindCollectionNameForDynamic` only works on objects that inherit from [IDynamicMetaObjectProvider](https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.idynamicmetaobjectprovider) interface. In .NET there are two built-in types that implement that interface, the [ExpandoObject](https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.expandoobject) and [DynamicObject](https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.dynamicobject).

For example, if we want to determine a collection using a `Collection` property from a dynamic object, we need to set `FindCollectionNameForDynamic` as follows:

{CODE find_dynamic_collection_name_sample_1@ClientApi\Configuration\IdentifierGeneration\Global.cs /}

After that we can store our dynamic object as follows:

{CODE find_dynamic_collection_name_sample_2@ClientApi\Configuration\IdentifierGeneration\Global.cs /}

{INFO/}

##TransformTypeCollectionNameToDocumentIdPrefix

Collection names determined by recently described convention functions aren't directly used as prefixes in document identifiers. There is a convention function called `TransformTypeCollectionNameToDocumentIdPrefix` which takes the collection name and produces the prefix:

{CODE transform_collection_name_to_prefix@ClientApi\Configuration\IdentifierGeneration\Global.cs /}

Its default behavior for a collection which contains one upper character is to simply convert it to lower case string. `Users` would be transformed into `users`. For collection names containing more upper characters, there will be no change. The collection name: `LineItems` would output the following prefix: `LineItems`.

##FindClrTypeName and FindClrType

In the metadata of all documents stored in a database, you can find the following property which specifies the client-side type. For instance:

{CODE-BLOCK:json}
{
    "Raven-Clr-Type": "Orders.Shipper, Northwind"
}
{CODE-BLOCK/}

This property is used by RavenDB client to perform a conversion between a .NET object and a JSON document stored in a database. A function responsible for retrieving the CLR type of an entity is defined by `FindClrTypeName` convention:

{CODE find_type_name@ClientApi\Configuration\IdentifierGeneration\Global.cs /}

To properly perform the revert conversion that is from a JSON result into a .NET object, we need to retrieve the CLR type from the `Raven-Clr-Type` metadata:

{CODE find_clr_type@ClientApi\Configuration\IdentifierGeneration\Global.cs /}

##FindIdentityProperty

The client must know where in your entity an identifier is stored to be properly able to transform it into JSON document. It uses the `FindIdentityProperty` convention for that. The default and very common convention is that a property named `Id` is the identifier, so is the default one:

{CODE find_identity_property@ClientApi\Configuration\IdentifierGeneration\Global.cs /}

You can provide a customization based on the `MemberInfo` parameter to indicate which property or field keeps the identifier. The client will iterate over all object properties and take the first one according to the defined predicate.

##FindIdentityPropertyNameFromCollectionName

It can happen that sometimes the results returned by the server don't have identifiers defined (for example if you run a projection query). However, they have `@collection` in metadata.

To perform the conversion into a .NET object, a function that finds the identity property name for a given entity name is applied:

{CODE find_identity_property_name_from_collection_name@ClientApi\Configuration\IdentifierGeneration\Global.cs /}

##IdentityPartsSeparator

According to the default, convention document identifiers have the following format: `[collectionName]/[identityValue]-[nodeTag]`. The slash character (`/`) separates the two parts of an identifier.
You can overwrite it by using `IdentityPartsSeparator` convention. Its default definition is:

{CODE identity_part_separator@ClientApi\Configuration\IdentifierGeneration\Global.cs /}

##Related Articles

- [Document identifier generation](../../../server/kb/document-identifier-generation)
- [Working with document identifiers](../../../client-api/document-identifiers/working-with-document-identifiers)
- [What is a collection?](../../../client-api/faq/what-is-a-collection)
- [Type-specific identifier generation](../../../client-api/configuration/identifier-generation/type-specific)
