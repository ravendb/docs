#Global identifier generation conventions

###DocumentKeyGenerator

You can setup a custom key generation strategy by supplying a custom function `DocumentKeyGenerator`. By default a session uses [the HiLo algorithm](../../../../client-api/document-identifiers/hilo-algorithm):

{CODE:java key_generator_hilo@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.java /}

However if you want the session to use [an identity key generation strategy](../../../../client-api/document-identifiers/working-with-document-ids#identity-ids) by default, you can overwrite this convention in the following manner:

{CODE:java key_generator_identityKeys@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.java /}

Function `getTypeTagName` will use either the convention specified in `findTypeTagName` (see below) or its default implementation.

###FindTypeTagName

Entity objects that share a common tag name belong to the same [collection](../../../../client-api/faq/what-is-a-collection) on the server side. Tag names are also used to build document keys.

{CODE:java find_type_tagname@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.java /}


###TransformTypeTagNameToDocumentKeyPrefix

Actually, tag names determined by recently described convention functions aren't directly used as prefixes in document keys. There is a convention function called `transformTypeTagNameToDocumentKeyPrefix` which takes the collection name and produces the prefix:

{CODE:java transform_tag_name_to_prefix@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.java /}

Its default behavior is that for a tag which contains one upper character it simply converts it to lower case string, e.g. `Users` would be transformed into `users`. For tag names containing more upper characters there will be no change, e.g. the tag name: `LineItems` would output the following prefix: `LineItems`.

###FindJavaClassName and FindJavaClass

In metadata of all documents stored in a database you can find the following property which specifies the client-side type. For instance:

{CODE-BLOCK:json}
{
    "Raven-Java-Class": "com.example.orders.Shipper"
}
{CODE-BLOCK/}

This property is used by RavenDB client to perform a conversion between java object and JSON document stored in a database. A function responsible for retrieving the java class of an entity is defined by `findJavaClassName` convention:

{CODE:java find_type_name@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.java /}

To properly perform the revert conversion that is from a JSON result into a java object we need to retrieve the java class from `Raven-Java-Class` metadata:

{CODE:java find_clr_type@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.java /}


###FindIdentityProperty

The client must know where in your entity an identifier is stored to be property able to transform it into JSON document. It uses `findIdentityProperty` convention for that. The default and very common convention is that a property named `Id` is the identifier, so it is the default one:

{CODE:java find_identity_property@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.java /}

You can provide a customization based on the `field` parameter to indicate which field keeps the identifier. The client will iterate over all object fields and take the first one according to the defined predicate.

###FindIdentityPropertyNameFromEntityName

It can happen that sometimes the results returned by the server can haven't identifiers defined (for example if you run a projection query) however they have `Raven-Entity-Name` in metadata.
Then to perform the conversion into java object a function that finds the identity property name for a given entity name is applied:

{CODE:java find_iden_propn_name_from_entity_name@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.java /}

###IdentityPartsSeparator

According to the default, convention document keys have the following format: `[collectionName]/[identityValue]`. The slash character (`/`) separates the two parts of an identifier.
You can overwrite it by using `identityPartsSeparator` convention. Its default definition is:

{CODE:java identity_part_separator@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.java /}

###IdentityTypeConvertors

RavenDB is designed to work with string identifiers. However it has the support for numeric and UUID ids. To be more exact it is able to work with `int`, `long` and `UUID` identifiers,
because it has dedicated converters for them:

{CODE:java identity_type_convertors@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.java /}

If you need to have the support for different types of identifier properties, you can add a custom converter that implements `ITypeConverter` interface:

{CODE:java custom_converter@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.java /}
{CODE:java identity_type_convertors_2@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.java /}

###FindIdValuePartForValueTypeConversion

You need to remember that even if the identity property of your entry isn't a string, a document in a database always has a string key. E.g. for the `Orders` entity that has
a numeric ID and its value is `3`, the server side key will be `orders/3`. If such document is fetched by RavenDB client, the key needs to be converted into a number. By default
we look for the last part of the identifier after `identityPartsSeparator`:

{CODE:java find_id_value_part_for_value_type_conversion@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.java /}

###FindFullDocumentKeyFromNonStringIdentifier

Sometimes the client needs to know the full id of the document that will be stored for an entity with non-sting identifier. We can use the following function:

{CODE:java find_full_doc_key_from_non_string_identifier@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.java /}

to find the full key based on the type of a document and the value type identifier.