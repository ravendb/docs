# Conventions: Serialization
---

{NOTE: }

* **Serialization**

* In this page:  
  * [CustomizeJsonSerializer](../../client-api/configuration/serialization#customizejsonserializer)  
  * [JsonContractResolver](../../client-api/configuration/serialization#jsoncontractresolver)  
  * [BulkInsert.TrySerializeEntityToJsonStream](../../client-api/configuration/serialization#bulkinsert.tryserializeentitytojsonstream)  
  * [IgnoreByRefMembers and IgnoreUnsafeMembers](../../client-api/configuration/serialization#ignorebyrefmembers-and-ignoreunsafemembers)  

{NOTE/}

---

{{PANEL: Serialization}

## CustomizeJsonSerializer

If you need to modify `JsonSerializer` object used by the client when sending entities to the server you can register a customization action:

{CODE customize_json_serializer@ClientApi\Configuration\DeSerialization.cs /}

## JsonContractResolver

The default `JsonContractResolver` used by RavenDB will serialize all properties and all public fields. You can change it by providing own implementation of `IContractResolver` interface:

{CODE json_contract_resolver@ClientApi\Configuration\DeSerialization.cs /}

{CODE custom_json_contract_resolver@ClientApi\Configuration\DeSerialization.cs /}

You can also customize behavior of the default resolver by inheriting from `DefaultRavenContractResolver` and overriding specific methods.

{CODE custom_json_contract_resolver_based_on_default@ClientApi\Configuration\DeSerialization.cs /}

## BulkInsert.TrySerializeEntityToJsonStream

For the bulk insert you can configure custom serialization implementation by providing `TrySerializeEntityToJsonStream`:

{CODE TrySerializeEntityToJsonStream@ClientApi\Configuration\DeSerialization.cs /}

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

{PANEL/}

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
