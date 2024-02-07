# Patching: JSON Patch Syntax

{NOTE: }

* You can use the **JSON Patch Syntax** from your client to apply changes 
  to RavenDB documents via JSON objects.  

* A JSON Patch is a document constructed of JSON objects, each containing 
  the ID of a target (RavenDB) document and a patch operation to be applied 
  to this document.  

* Since the operation is executed in a single request to a database, the JSON Patch command is performed in a single write [transaction](../../../client-api/faq/transaction-support).

* JSON Patch operations include -  
   * **Add** a document property  
   * **Remove** a document property  
   * **Replace** the contents of a document property  
   * **Copy** the contents of one document property to another  
   * **Move** the contents of one document property to another  
   * **Test** whether the patching succeeded  
  
* In this page:  
   * [JSON Patches](../../../client-api/operations/patching/json-patch-syntax#json-patches)
   * [Running JSON Patches](../../../client-api/operations/patching/json-patch-syntax#running-json-patches)  
   * [Patch Operations](../../../client-api/operations/patching/json-patch-syntax#patch-operations)  
      * [Add Document Property](../../../client-api/operations/patching/json-patch-syntax#add-document-property)  
      * [Remove Document Property](../../../client-api/operations/patching/json-patch-syntax#remove-document-property)  
      * [Replace Document Property Contents](../../../client-api/operations/patching/json-patch-syntax#replace-document-property-contents)  
      * [Copy Document Property Contents to Another Property](../../../client-api/operations/patching/json-patch-syntax#copy-document-property-contents-to-another-property)  
      * [Move Document Property Contents to Another Property](../../../client-api/operations/patching/json-patch-syntax#move-document-property-contents-to-another-property)  
      * [Test Patching Operation](../../../client-api/operations/patching/json-patch-syntax#test-patching-operation)  
   * [Additional JSON Patching Options](../../../client-api/operations/patching/json-patch-syntax#additional-json-patching-options)  

{NOTE/}

---

{PANEL: JSON Patches }

* Similar to other forms of patching, JSON Patches can be used by a client to 
  swiftly change any number of documents without loading and editing the documents 
  locally first.  

* A series of JSON objects, each containing a patch operation and a document ID, 
  are added to an ASP `JsonPatchDocument` object that is sent to the server for 
  execution.  

---

### When are JSON Patches Used?

JSON Patches include no RQL or C# code, and offer a limited set of operations 
in relation to other patching methods.  
Users may still prefer them over other methods when, for example -  

   * A client of multiple databases of different brands prefers broadcasting patches 
     with a common syntax to all databases.  
   * It is easier for an automated process that builds and applies patches, 
     to send JSON patches.  

{PANEL/}

{PANEL: Running JSON Patches}

To run JSON patches -  

* Use the `Microsoft.AspNetCore.JsonPatch` namespace from your code.  
  E.g. `using Microsoft.AspNetCore.JsonPatch;`  
* Create a `JsonPatchDocument` instance and append your patches to it.  
* Pass your Json Patch Document to RavenDB's `JsonPatchOperation` operation to run the patches.  
    * `JsonPatchOperation` Parameters  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | id | `string` | The ID of the document we want to patch |
        | jsonPatchDocument | `JsonPatchDocument` | Patches document |



{PANEL/}

{PANEL: Patch Operations}

### Add Operation

Use the `Add` operation to add a document property or an array element.  

* **Method Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | path | `string` | Path to the property we want to add |
    | value | `object` | Property value |

* **Code Sample - Add a document property**  
  {CODE json_patch_Add_operation@ClientApi\Operations\Patches\JsonPatchSyntax.cs /}

---

### Remove Document Property  

Use the `Remove` operation to remove a document property or an array element.  

* **Method Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | path | `string` | Path to the property we want to remove |

* **Code Sample - Remove a document property**  
  {CODE json_patch_Remove_operation@ClientApi\Operations\Patches\JsonPatchSyntax.cs /}

---

### Replace Document Property Contents  

Use the `Replace` operation to replace the contents of a document property or an array element

* **Method Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | path | `string` | Path to the property whose contents we want to replace |
    | value | `object` | New contents |

* **Code Sample - Replace a document property**  
  {CODE json_patch_Replace_operation@ClientApi\Operations\Patches\JsonPatchSyntax.cs /}

---

### Copy Document Property Contents to Another Property  

Use the `Copy` operation to copy the contents of one document property array element to another

* **Method Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | from | `string` | Path to the property we want to copy |
    | path| `string` | Path to the property we want to copy to |

* **Code Sample - Copy document property contents**  
  {CODE json_patch_Copy_operation@ClientApi\Operations\Patches\JsonPatchSyntax.cs /}

---

### Move Document Property Contents to Another Property  

Use the `Move` operation to move the contents of one document property or array element to another  

* **Method Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | from | `string` | Path to the property whose contents we want to move |
    | path| `string` | Path to the property we want to move the contents to |

* **Code Sample - Move document property contents**  
  {CODE json_patch_Move_operation@ClientApi\Operations\Patches\JsonPatchSyntax.cs /}

---

### Test Patching Operation

Use the `Test` operation to verify patching operations.  
If the test fails, all patching operations included in the patches document will be revoked 
and a `RavenException` exception will be thrown.  

* **Method Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | path | `string` | Path to the property we want to test |
    | value | `object` | Value to compare `path` with |


* **Code Sample - Test Patching**  

  {CODE json_patch_Test_operation@ClientApi\Operations\Patches\JsonPatchSyntax.cs /}

{PANEL/}

{PANEL: Additional JSON Patching Options}

The samples given above remain simple, showing how to manipulate document properties.  
Note that JSON Patches have additional options, like the manipulation of array or list elements:  

* **Add an array element**  
  {CODE json_patch_Add_array_element@ClientApi\Operations\Patches\JsonPatchSyntax.cs /}

You can learn more about additional JSON patching options in the [JSON Patch RFC](https://datatracker.ietf.org/doc/html/rfc6902), 
among other resources.  

{PANEL/}

## Related Articles

### Patching

- [Single Document Patch Operation](../../../client-api/operations/patching/single-document)  
- [Set Based Patch Operation](../../../client-api/operations/patching/set-based)  

### Knowledge Base

- [JavaScript Engine](../../../server/kb/javascript-engine)
- [Numbers in JavaScript Engine](../../../server/kb/numbers-in-ravendb#numbers-in-javascript-engine)

### Additional resources
- [JsonPatchDocument Class](http://docs.microsoft.com/en-us/dotnet/api/Microsoft.aspnetcore.jsonpatch.jsonpatchdocument?view=aspnetcore=5.0)  
- [JSON Patch RFC](https://datatracker.ietf.org/doc/html/rfc6902)  
