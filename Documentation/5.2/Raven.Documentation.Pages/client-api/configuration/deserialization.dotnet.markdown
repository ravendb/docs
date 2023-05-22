# Conventions: DeSerialization

---

{NOTE: }

Use the methods described in this page to customize the [conventions](../../client-api/configuration/conventions) 
by which entities are deserialized as they are received by the client.  

* In this page:  
  * [CustomizeJsonDeserializer](../../client-api/configuration/deserialization#customizejsondeserializer)  
  * [DeserializeEntityFromBlittable](../../client-api/configuration/deserialization#deserializeentityfromblittable)  
  * [PreserveDocumentPropertiesNotFoundOnModel](../../client-api/configuration/deserialization#preservedocumentpropertiesnotfoundonmodel)  
  * [Numbers DeSerialization](../../client-api/configuration/deserialization#number-deserialization)  

{NOTE/}

---

{PANEL: DeSerialization}

## CustomizeJsonDeserializer

* The `JsonSerializer` object is used by the client to deserialize entities 
  loaded from the server.  
* Use the `CustomizeJsonDeserializer` convention to modify `JsonSerializer` 
  by registering a deserialization customization action.  

{CODE customize_json_deserializer@ClientApi\Configuration\DeSerialization.cs /}

## DeserializeEntityFromBlittable

* Use the `DeserializeEntityFromBlittable` convention to customize entity 
  deserialization from a blittable JSON.  

{CODE DeserializeEntityFromBlittable@ClientApi\Configuration\DeSerialization.cs /}

## PreserveDocumentPropertiesNotFoundOnModel

* Some document properties are not deserialized to an object.  
* Set the `PreserveDocumentPropertiesNotFoundOnModel` convention to `true` 
  to **preserve** such properties when the document is saved.  
* Set the `PreserveDocumentPropertiesNotFoundOnModel` convention to `false` 
  to **remove** such properties when the document is saved.  
* Default: `true`  

{CODE preserve_doc_props_not_found_on_model@ClientApi\Configuration\DeSerialization.cs /}

## Number DeSerialization

* RavenDB client supports all common numeric value types (including `int`, `long`, 
  `double`, `decimal`, etc.) out of the box.  
* Note that although deserialization of `decimals` is fully supported, there are 
  [server side limitations](../../server/kb/numbers-in-ravendb) to numbers in this range.  
* Other number types, like `BigInteger`, must be handled using custom deserialization.

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
