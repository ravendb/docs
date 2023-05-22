# Security: DeSerialization
---

{NOTE: }

* RCE (Remote Code Execution) intruders exploit security vulnerabilities 
  in their target systems to inject these systems with malicious code.  
* A routine client event that may present exploitable vulnerabilities 
  is the [deserialization](../../client-api/configuration/deserialization) 
  of `.Net` objects to `JSON` objects as data is received by the client.  
  E.g., deserialized objects may activate gadgets, and these gadgets may 
  run pre-loaded code on the client machine.  
   * See a list of known .Net RCE gadgets [here](https://cheatsheetseries.owasp.org/cheatsheets/Deserialization_Cheat_Sheet.html).  
   * See an example for using a gadget to exploit JSON deserialization [here](https://book.hacktricks.xyz/pentesting-web/deserialization/basic-.net-deserialization-objectdataprovider-gadgets-expandedwrapper-and-json.net#abusing-json.net).  
* RavenDB can prevent such attacks by blocking deserialization from 
  a known list of name spaces and object types.  

* In this page:  
  * [](../../)  
  * [](../../)  
  * [](../../)  
  * [](../../)  

{NOTE/}

---

{PANEL: `DefaultRavenSerializationBinder`}

To block the deserialization of name spaces or object types, register the 
`DefaultRavenSerializationBinder` convention and use its following methods:  

* `RegisterForbiddenNamespace`  
  Use this method to forbid the deserialization of given name spaces.  

* `RegisterForbiddenType`  
  Use this method to forbid the deserialization of given object types.  

* `RegisterSafeType`  
  Use this method to allow the deserialization of given object types.  

{CODE RegisterForbiddenNamespace@ClientApi\Security\DeSerializationSecurity.cs /}

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
