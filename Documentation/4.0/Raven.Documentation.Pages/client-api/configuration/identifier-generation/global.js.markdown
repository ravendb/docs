#Global Identifier Generation Conventions

##FindCollectionName

Documents that have the same `@collection` metadata belong to the same [collection](../../../client-api/faq/what-is-a-collection) on the server side. Collection names are also used to build document identifiers. 

{CODE:nodejs find_type_collection_name@client-api\configuration\identifierGeneration\global.js /}

##TransformClassCollectionNameToDocumentIdPrefix

Collection names determined by recently described convention functions aren't directly used as prefixes in document identifiers. There is a convention function called `transformClassCollectionNameToDocumentIdPrefix()` which takes the collection name and produces the prefix:

{CODE:nodejs transform_collection_name_to_prefix@client-api\configuration\identifierGeneration\global.js /}

Its default behavior is that for a collection which contains one upper character it simply converts it to lower case string. `Users` would be transformed into `users`. For collection names containing more upper characters there will be no change. The collection name: `LineItems` would output the following prefix: `LineItems`.

##FindJsTypeName and FindJsType

In the metadata of all documents stored by RavenDB Node.js Client, you can find the following property which specifies the client-side type. For instance:

{CODE-BLOCK:json}
{
    "Raven-Node-Type": "Customer"
}
{CODE-BLOCK/}

This property is used by RavenDB client to perform a conversion between a JS object and a JSON document stored in a database. A function responsible for retrieving the JS type of an entity is defined by `findJsTypeName()` convention:

{CODE:nodejs find_type_name@client-api\configuration\identifierGeneration\global.js /}

To properly perform the reverse conversion that is from a JSON result into a JS object, we need to retrieve the JS type from the `Raven-Node-Type` metadata:

{CODE:nodejs find_clr_type@client-api\configuration\identifierGeneration\global.js /}


##FindIdentityPropertyNameFromCollectionName

It can happen that sometimes the results returned by the server don't have identifiers defined (for example if you run a projection query) however they have `@collection` in metadata.

To perform the conversion into a JS object, a function that finds the identity property name for a given collection name is applied:

{CODE:nodejs find_identity_property_name_from_collection_name@client-api\configuration\identifierGeneration\global.js /}

##IdentityPartsSeparator

By default, convention document identifiers have the following format: `[collectionName]/[identityValue]-[nodeTag]`. The slash character (`/`) separates the two parts of an identifier.
You can overwrite it by using `IdentityPartsSeparator` convention. Its default definition is:

{CODE:nodejs identity_part_separator@client-api\configuration\identifierGeneration\global.js /}

##FindCollectionNameForObjectLiteral

This convention is *not defined by default*. It's only useful when using object literals as entities. It defines how the client obtains a collection name for an object literal. If it's undefined object literals stored with `session.store()` are going to land up in `@empty` collection having a UUID for an ID. 

For instance here's mapping of the *category* field to collection name:
{CODE:nodejs find_collection_name_for_object_literal@client-api\configuration\identifierGeneration\global.js /}

##Related Articles

- [Document identifier generation](../../../server/kb/document-identifier-generation)
- [Working with document identifiers](../../../client-api/document-identifiers/working-with-document-identifiers)
- [What is a collection?](../../../client-api/faq/what-is-a-collection)
- [Type-specific identifier generation](../../../client-api/configuration/identifier-generation/type-specific)
