# Conventions: Serialization

---

{NOTE: }

Use the methods described in this page to customize the [conventions](../../client-api/configuration/conventions) 
by which entities are serialized as they are sent by the client to the server.  

* In this page:  
  * [CustomizeJsonSerializer](../../client-api/configuration/serialization#customizejsonserializer)  
  * [JsonContractResolver](../../client-api/configuration/serialization#jsoncontractresolver)  
  * [BulkInsert.TrySerializeEntityToJsonStream](../../client-api/configuration/serialization#bulkinsert.tryserializeentitytojsonstream)  
  * [IgnoreByRefMembers and IgnoreUnsafeMembers](../../client-api/configuration/serialization#ignorebyrefmembers-and-ignoreunsafemembers)  

{NOTE/}

---

{PANEL: Serialization}

## CustomizeJsonSerializer

* The `JsonSerializer` object is used by the client to serialize entities 
  sent by the client to the server.  
* Use the `CustomizeJsonSerializer ` convention to modify `JsonSerializer` 
  by registering a serialization customization action.  

{CODE customize_json_serializer@ClientApi\Configuration\Serialization.cs /}

## JsonContractResolver

* The default `JsonContractResolver` convention used by RavenDB will serialize 
  **all** properties and **all** public fields.  
* Change this behavior by providing your own implementation of the `IContractResolver` 
  interface.  

    {CODE json_contract_resolver@ClientApi\Configuration\Serialization.cs /}

    {CODE custom_json_contract_resolver@ClientApi\Configuration\Serialization.cs /}

* You can also customize the behavior of the **default resolver** by inheriting 
  from `DefaultRavenContractResolver` and overriding specific methods.  

    {CODE custom_json_contract_resolver_based_on_default@ClientApi\Configuration\Serialization.cs /}

## BulkInsert.TrySerializeEntityToJsonStream

* Adjust [Bulk Insert](../../client-api/bulk-insert/how-to-work-with-bulk-insert-operation) 
  behavior by using the `TrySerializeEntityToJsonStream` convention to register a custom 
  serialization implementation.  

{CODE TrySerializeEntityToJsonStream@ClientApi\Configuration\Serialization.cs /}

## IgnoreByRefMembers and IgnoreUnsafeMembers

* By default, if you try to store an entity with `ref` or unsafe members, 
  the Client will throw an exception when [`session.SaveChanges()`](../../client-api/session/saving-changes) 
  is called.  
* Set the `IgnoreByRefMembers` convention to `true` to simply ignore `ref` 
  members when an attempt to store an entity with `ref` members is made.  
  The entity will be uploaded to the server with all non-`ref` members without 
  throwing an exception.  
  The document structure on the server-side will not contain fields for those 
  `ref` members.  
* Set the `IgnoreUnsafeMembers` convention to `true` to ignore all pointer 
  members in the same manner.  
* `IgnoreByRefMembers` default value: `false`  
* `IgnoreUnsafeMembers` default value: `false`  

{PANEL/}

## Related articles

### Conventions

- [Conventions](../../client-api/configuration/conventions)  
- [Querying](../../client-api/configuration/querying)  
- [Load Balance & Failover](../../client-api/configuration/load-balance/overview)  
- [Deserialization](../../client-api/configuration/deserialization)  
- [Deserialization Security](../../client-api/security/deserialization-security)  

### Document Identifiers

- [Document Identifiers](../../client-api/document-identifiers/working-with-document-identifiers)  
- [Global ID Generation](../../client-api/configuration/identifier-generation/global)  
- [Type-specific ID](../../client-api/configuration/identifier-generation/type-specific)  

### Document Store

- [Document Store](../../client-api/what-is-a-document-store)  
