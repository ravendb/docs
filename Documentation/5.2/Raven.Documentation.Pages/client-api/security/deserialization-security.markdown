# Security: DeSerialization
---

{NOTE: }

* Data deserialization can run gadgets that may initiate RCE attack 
  on the client machine.  
* To prevent this risk, RavenDB's default deserializer blocks the 
  deserialization of known [`.Net` RCE gadgets](https://cheatsheetseries.owasp.org/cheatsheets/Deserialization_Cheat_Sheet.html#known-net-rce-gadgets).  
* Users can easily modify the list of namespaces and object types 
  for which deserialization is forbidden or allowed.  

* In this page:  
  * [Securing Deserialization](../../client-api/security/deserialization-security#securing-deserialization)  
  * [Invoking a Gadget](../../client-api/security/deserialization-security#invoking-a-gadget)  
  * [DefaultRavenSerializationBinder](../../client-api/security/deserialization-security#defaultravenserializationbinder)  
     * [RegisterForbiddenNamespace](../../client-api/security/deserialization-security#section)  
     * [RegisterForbiddenType](../../client-api/security/deserialization-security#section-1)  
     * [RegisterSafeType](../../client-api/security/deserialization-security#section-2)  
  * [Example](../../client-api/security/deserialization-security#example)  

{NOTE/}

---

{PANEL: Securing Deserialization}

* When a RavenDB client uses the [Newtonsoft library](https://www.newtonsoft.com/json/help/html/SerializingJSON.htm) 
  to deserialize a JSON string to a `.Net` object, the object may include 
  a reference to a **gadget** (a code segment) and the deserialization 
  process may execute this gadget.  
* Some gadgets attempt to exploit the deserialization process and initiate 
  an RCE (Remote Code Execution) attack that may, for example, inject the 
  system with malicious code. RCE attacks may sabotage the system, gain 
  control over it, steal information, and so on.  
* To prevent such exploitation, RavenDB's default deserializer blocks the 
  deserialization of [known `.Net` RCE gadgets](https://cheatsheetseries.owasp.org/cheatsheets/Deserialization_Cheat_Sheet.html#known-net-rce-gadgets):  
    `System.Configuration.Install.AssemblyInstaller`  
    `System.Activities.Presentation.WorkflowDesigner`  
    `System.Windows.ResourceDictionary`  
    `System.Windows.Data.ObjectDataProvider`  
    `System.Windows.Forms.BindingSource`  
    `Microsoft.Exchange.Management.SystemManager.WinForms.ExchangeSettingsProvider`  
    `System.Data.DataViewManager, System.Xml.XmlDocument/XmlDataDocument`  
    `System.Management.Automation.PSObject`  

* Users can easily [modify](../../client-api/security/deserialization-security#defaultravenserializationbinder) 
  the list of namespaces and object types for which deserialization is forbidden 
  or allowed.  

{PANEL/}  

{PANEL: Invoking a Gadget}

* **Direct gadget loading is PERMITTED**  
  Deserialization of directly-loaded objects **is permitted** 
  regardless of the content of the default deserializer list.  
  E.g., the following segment **will** be executed,  
  {CODE DeSerializationSecurity_load-object@ClientApi\Security\DeSerializationSecurity.cs /}

* **Indirect gadget loading CAN BE PREVENTED**  
  Gadgets loading and execution during deserialization **can** 
  be prevented using the default deserializer list 
  when the gadgets are loaded indirectly.  
  E.g., in the following sample, taken [from here](https://book.hacktricks.xyz/pentesting-web/deserialization/basic-.net-deserialization-objectdataprovider-gadgets-expandedwrapper-and-json.net#abusing-json.net), 
  the gadget name is included as a value, and will only take 
  its place and be used to execute the gadget during deserialization.  
  Including this type in the default deserialization list **will** 
  prevent the gadget's deserialization and execution.  
  {CODE DeSerializationSecurity_define-type@ClientApi\Security\DeSerializationSecurity.cs /}

{PANEL/}


{PANEL: `DefaultRavenSerializationBinder`}

Use the `DefaultRavenSerializationBinder` convention and its methods to 
block the deserialization of suspicious namespaces and object types and 
to allow the deserialization of trusted object types.  

Define a `DefaultRavenSerializationBinder` instance, use the dedicated 
methods to forbid or allow the deserialization of entities, and register 
the defined instance as a serialization convention as shown 
[below](../../client-api/security/deserialization-security#example).  

{NOTE: }
Be sure to update the default deserializer list **before** the document 
that you want the list to apply to is initialized.  
{NOTE/}

---

### `RegisterForbiddenNamespace`  
Use `RegisterForbiddenNamespace` to prevent the deserialization of objects loaded from a given namespace.

* {CODE RegisterForbiddenNamespace_definition@ClientApi\Security\DeSerializationSecurity.cs /}

     | Parameter | Type | Description |
     |:-------------:|:-------------:|-------------|
     | **@namespace** | `string` | The name of a namespace from which deserialization won't be allowed. |

     {NOTE: Exception}
     Attempting to deserialize a forbidden namespace will throw an 
     `InvalidOperationException` exception with the following details:  
     _"Cannot resolve type" + `type.FullName` + "because the namespace is on a blacklist due to 
     security reasons. Please customize json deserializer in the conventions and override SerializationBinder 
     with your own logic if you want to allow this type."_
     {NOTE/}

---

### `RegisterForbiddenType`  
Use `RegisterForbiddenType` to prevent the deserialization of a given object type.

* {CODE RegisterForbiddenType_definition@ClientApi\Security\DeSerializationSecurity.cs /}

     | Parameter | Type | Description |
     |:-------------:|:-------------:|-------------|
     | **type** | `Type` | An object type whose deserialization won't be allowed. |

     {NOTE: Exception}
     Attempting to deserialize a forbidden object type will throw an 
     `InvalidOperationException` exception with the following details:  
     _"Cannot resolve type" + `type.FullName` + "because the type is on a blacklist due to 
     security reasons. 
     Please customize json deserializer in the conventions and override SerializationBinder 
     with your own logic if you want to allow this type."_
     {NOTE/}

---

### `RegisterSafeType`  
Use `RegisterSafeType` to **allow** the deserialization of a given object type.

* {CODE RegisterSafeType_definition@ClientApi\Security\DeSerializationSecurity.cs /}

     | Parameter | Type | Description |
     |:-------------:|:-------------:|-------------|
     | **type** | `Type` | An object type whose deserialization **will** be allowed. |

## Example

{CODE DefaultRavenSerializationBinder@ClientApi\Security\DeSerializationSecurity.cs /}

{PANEL/}

## Related articles

### Conventions
- [Conventions](../../client-api/configuration/conventions)
- [Deserialization](../../client-api/configuration/deserialization)  
- [Serialization](../../client-api/configuration/serialization)  
