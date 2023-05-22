# Security: DeSerialization
---

{NOTE: }

* An RCE (Remote Code Execution) attack is an attempt to exploit a vulnerability 
  on a remote machine through which code can be injected and allow the intruder 
  to perform various uninvited operations.  
* One such vulnerability is situated where `.Net` objects (e.g. ones transfered 
  from a server) are deserialized (e.g. on a client machine) to `JSON` objects 
  using the [Newtonsoft](https://www.newtonsoft.com/json) library.  
   {NOTE: }
   See [here](https://book.hacktricks.xyz/pentesting-web/deserialization/basic-.net-deserialization-objectdataprovider-gadgets-expandedwrapper-and-json.net#abusing-json.net) 
   how deserializing a `ObjectDataProvider` Gadget can cause an RCE.  

  



* In this page:  
  * [](../../)  
  * [](../../)  
  * [](../../)  
  * [](../../)  

{NOTE/}

---

{PANEL: DeSerialization Security}

{CODE customize_json_serializer@ClientApi\Security\DeSerializationSecurity.cs /}

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
