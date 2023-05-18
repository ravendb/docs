# Conventions: Serialization

## CustomizeJsonSerializer

If you need to modify `JsonSerializer` object used by the client when sending entities to the server you can register a customization action:

{CODE customize_json_serializer@ClientApi\Configuration\Serialization.cs /}

## CustomizeJsonDeserializer

To modify the `JsonSerializer` object used by the client to deserialize entities loaded from the server, you can register a customization action:

{CODE customize_json_deserializer@ClientApi\Configuration\Serialization.cs /}

## DeserializeEntityFromBlittable

In order to customize the entity deserialization from blittable JSON you can use define `DeserializeEntityFromBlittable` implementation:

{CODE DeserializeEntityFromBlittable@ClientApi\Configuration\Serialization.cs /}

## JsonContractResolver

The default `JsonContractResolver` used by RavenDB will serialize all properties and all public fields. You can change it by providing own implementation of `IContractResolver` interface:

{CODE json_contract_resolver@ClientApi\Configuration\Serialization.cs /}

{CODE custom_json_contract_resolver@ClientApi\Configuration\Serialization.cs /}

You can also customize behavior of the default resolver by inheriting from `DefaultRavenContractResolver` and overriding specific methods.

{CODE custom_json_contract_resolver_based_on_default@ClientApi\Configuration\Serialization.cs /}

## PreserveDocumentPropertiesNotFoundOnModel

Controls whatever properties that were not de-serialized to an object properties will be preserved 
during saving a document again. If `false`, those properties will be removed when the document will be saved. Default: `true`.

{CODE preserve_doc_props_not_found_on_model@ClientApi\Configuration\Serialization.cs /}

## BulkInsert.TrySerializeEntityToJsonStream

For the bulk insert you can configure custom serialization implementation by providing `TrySerializeEntityToJsonStream`:

{CODE TrySerializeEntityToJsonStream@ClientApi\Configuration\Serialization.cs /}

## Numbers (de)serialization

RavenDB client supports out of the box all common numeric value types: `int`, `long`, `double`, `decimal` etc.  
Note that although the (de)serialization of `decimals` is fully supported, there are [server side limitations](../../server/kb/numbers-in-ravendb) to numbers in that range.  
Other number types like `BigInteger` must be treated using custom (de)serialization.

## IgnoreByRefMembers and IgnoreUnsafeMembers

By default, if you try to store an entity that has `ref` or unsafe members, the 
Client will throw an exception when [`session.SaveChanges()` is called](../../client-api/session/saving-changes).  

If `IgnoreByRefMembers` is set to `true` and you try to store an entity that has 
`ref` members, those members will simply be ignored. The entity will be uploaded 
to the server with all non-`ref` members without throwing an exception. The 
document structure on the server-side will not contain fields for those `ref` 
members.  

If `IgnoreUnsafeMembers` is set to `true`, all pointer members will be ignored 
in the same manner.  

The default value of both these conventions is `false`.  

## Related articles

### Conventions

- [Conventions](../../client-api/configuration/conventions)
- [Querying](../../client-api/configuration/querying)
- [Load Balance & Failover](../../client-api/configuration/load-balance/overview)

### Document Identifiers

- [Working with Document Identifiers](../../client-api/document-identifiers/working-with-document-identifiers)
- [Global ID Generation Conventions](../../client-api/configuration/identifier-generation/global)
- [Type-specific ID Generation Conventions](../../client-api/configuration/identifier-generation/type-specific)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
