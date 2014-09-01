#Global identifier generation conventions

###DocumentKeyGenerator

You can setup a custom key generation strategy by supplying a custom function `DocumentKeyGenerator`. By default a session uses [the HiLo algorithm](../../document-identifiers/hilo-algorithm) for both sync and async API usage:

{CODE key_generator_hilo@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

However if you want the session to use [an identity key generation strategy](../../document-identifiers/working-with-document-ids#identity-ids) by default you can overwrite this convention in the following manner:

{CODE key_generator_identityKeys@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

###FindClrTypeName and FindClrType

In metadata of all documents stored in a database you can find the following property which specifies the client-side type. For instance:

{CODE-BLOCK:json}
{
    "Raven-Clr-Type": "Orders.Shipper, Northwind"
}
{CODE-BLOCK/}

This property is used by RavenDB client to perform a conversion between .NET object and JSON document stored in a database. A function responsible for retrieving
the CLR type of an entity is defined by `FindClrTypeName` convention:

{CODE find_type_name@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

To properly perform the revert conversion that is from a JSON result into a .NET object we need to retrieve the CLR type from `Raven-Clr-Type` medatada:

{CODE find_clr_type@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

###FindTypeTagName and FindDynamicTagName

Entity objects that share a common tag name belong to the same [collection](../../faq/what-is-a-collection) on the server side. Tag names (collection names) are also used
to build document keys. There are two functions that the client uses to determine the collection name. The fist one is used for standard objects with well defined type:

{CODE find_type_tagname@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

The second one is dedicated for dynamic objects:

{CODE find_dynamic_tag_name@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

###FindIdentityProperty

The client must know where in your entity an identifier is stored to be properlt able to transform it into JSON document. It uses `FindIdentityProperty` convention for that. The default and very common convention is that a property
named `Id` is the indentifier, so it is the default one:

{CODE find_identity_property@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

You can provide a customization based on the `MemberInfo` parameter to indicate which property or field keeps the identifier. The client will iterate over all object properties and
will take the first one according to the defined predicate.

###FindIdentityPropertyNameFromEntityName

It can happen that sometimes the results returned by the server can haven't identifiers defined (for example if you run a projection query) however they have `Raven-Entity-Name` in metadata.
Then to perform the conversion into .NET object a function that finds the identity property name for a given entity name is applied:

{CODE find_iden_propn_name_from_entity_name@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

###IdentityPartsSeparator

According to the default convention document keys have the following format: `[collectionName]/[identityValue]`. The slash character (`/`) separates the two parts of an identifier.
You can overwrite that by using `IdentityPartsSeparator` convention. Its default definition is:

{CODE identity_part_separator@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

###IdentityTypeConvertors

RavenDB is designed to work with string identifiers. However it has the support for numeric and Guid ids. To be more exact it is able to work with `Int32`, `Int64` and `Guid` identifiers,
because it has dedicated converters for them:

{CODE identity_type_convertors@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

If you need to have the support for different types of identifier properties then you can add a custom converter that implements `ITypeConverter` interface:

{CODE custom_converter@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}
{CODE identity_type_convertors_2@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

###FindIdValuePartForValueTypeConversion

You need to remember that even if the identity property of your entry isn't a string, a document in a database always have a string key. E.g. for the `Orders` entity that has
a numeric ID and its value is `3`, the server side key will be `orders/3`. If such document is fetched by RavenDB client then the key needs to be converted into number. By default
we use approach that we look for the last part of the identifier after `IdentityPartsSeparator`:

{CODE find_id_value_part_for_value_type_conversion@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

###FindFullDocumentKeyFromNonStringIdentifier

Sometimes the client needs to know the full document id what will be stored for an entity which has non-sting identifier. We have the following function:

{CODE find_full_doc_key_from_non_string_identifier@ClientApi\Configuration\Conventions\IdentifierGeneration\Global.cs /}

to find the full key based on the type of a document and the value type identifier.