# Conventions: DeSerialization
---

{NOTE: }

* **DeSerialization**

* In this page:  
  * [CustomizeJsonDeserializer](../../client-api/configuration/deserialization#customizejsondeserializer)  
  * [DeserializeEntityFromBlittable](../../client-api/configuration/deserialization#deserializeentityfromblittable)  
  * [PreserveDocumentPropertiesNotFoundOnModel](../../client-api/configuration/deserialization#preservedocumentpropertiesnotfoundonmodel)  
  * [Numbers DeSerialization](../../client-api/configuration/deserialization#numbers-deserialization)  

{NOTE/}

---

{PANEL: DeSerialization}

## CustomizeJsonDeserializer

To modify the `JsonSerializer` object used by the client to deserialize entities 
loaded from the server, register a customization action:

{CODE customize_json_deserializer@ClientApi\Configuration\DeSerialization.cs /}

## DeserializeEntityFromBlittable

Use `DeserializeEntityFromBlittable` to customize entity deserialization from a blittable JSON.  

{CODE DeserializeEntityFromBlittable@ClientApi\Configuration\DeSerialization.cs /}

## PreserveDocumentPropertiesNotFoundOnModel

Controls whatever properties that were not de-serialized to an object properties will be preserved 
during saving a document again. If `false`, those properties will be removed when the document will be saved. Default: `true`.

{CODE preserve_doc_props_not_found_on_model@ClientApi\Configuration\DeSerialization.cs /}

## Numbers DeSerialization

RavenDB client supports out of the box all common numeric value types: `int`, `long`, `double`, `decimal` etc.  
Note that although the de-serialization of `decimals` is fully supported, there are [server side limitations](../../server/kb/numbers-in-ravendb) to numbers in that range.  
Other number types like `BigInteger` must be treated using custom de-serialization.

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
