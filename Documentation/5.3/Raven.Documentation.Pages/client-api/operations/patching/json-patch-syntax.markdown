# Patching: JSON Patch Syntax

{NOTE: }

* You can use the **JSON Patch Syntax** from your client to apply changes 
  to RavenDB documents via JSON objects.  

* A JSON Patch is a document constructed of JSON objects, each containing 
  the ID of a target (RavenDB) document and a patch operation to be applied 
  to this document.  

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
{NOTE/}

---

{PANEL: JSON Patches }

Similar to other forms of patching, JSON Patches can be used by a client to 
swiftly change any number of documents without loading and editing the documents 
locally first.  

A series of JSON objects, each containing a patch operation and a document ID, 
is sent to the server, that performs the requested changes with no further 
involvement of the client.  

JSON patches include no RQL or c# code. They may be preferred by users in spite 
of the limited set of operations they offer, when, for example, the client that 
applies them maintains a common communications interface with multiple systems, 
of which RavenDB is one.  

{PANEL/}

{PANEL: Running JSON Patches}

To run JSON patches -  

* Install the ASP `Install-Package Microsoft.AspNetCore.JsonPatch` package
  to make its [JsonPatchDocument Class](http://docs.microsoft.com/en-us/dotnet/api/Microsoft.aspnetcore.jsonpatch.jsonpatchdocument?view=aspnetcore=5.0) 
  available for your client.  
* Use the `Microsoft.AspNetCore.JsonPatch` namespace from your code.  
* Create a `JsonPatchDocument` instance and append your patches to it.  
* Pass your Json Patch Document to RavenDB's `JsonPatchOperation` operation to run the patches.  
    * `JsonPatchOperation` Parameters  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | id | `string` | The ID of the document we want to patch |
        | jsonPatchDocument | `JsonPatchDocument` | Patches document |



{PANEL/}

{PANEL: Patch Operations}

### Add Document Property  

Add a document property using the `Add` patching operation.  

* **Method Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | path | `string` | Path to the property we want to add |
    | value | `object` | Property value |

* **Code Sample**  

    {CODE-BLOCK: JavaScript}
    var patchesDocument = new JsonPatchDocument();
patchesDocument.Add("/PropertyName", "NewValue");
store.Operations.Send(new JsonPatchOperation(documentId, patchesDocument));
    {CODE-BLOCK/}

---

### Remove Document Property  

Remove a document property using the `Remove` patching operation.  

* **Method Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | path | `string` | Path to the property we want to remove |

* **Code Sample**  

    {CODE-BLOCK: JavaScript}
    var patchesDocument = new JsonPatchDocument();
patchesDocument.Remove("/PropertyName");
store.Operations.Send(new JsonPatchOperation(documentId, patchesDocument));
    {CODE-BLOCK/}

---

### Replace Document Property Contents  

Replace the contents of a document property using the `Replace` patching operation.  

* **Method Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | path | `string` | Path to the property whose contents we want to replace |
    | value | `object` | New contents |

* **Code Sample**  

    {CODE-BLOCK: JavaScript}
    var patchesDocument = new JsonPatchDocument();
patchesDocument.Replace("/PropertyName", "NewValue");
store.Operations.Send(new JsonPatchOperation(documentId, patchesDocument));
    {CODE-BLOCK/}

---

### Copy Document Property Contents to Another Property  

Copy the contents of one document property to another using the `Replace` patching operation.  

* **Method Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | from | `string` | Path to the property we want to copy |
    | path| `string` | Path to the property we want to copy to |

* **Code Sample**  

    {CODE-BLOCK: JavaScript}
    var patchesDocument = new JsonPatchDocument();
patchesDocument.Copy("/PropertyName1", "PropertyName2"); // Copy PropertyName1 contents to PropertyName2
store.Operations.Send(new JsonPatchOperation(documentId, patchesDocument));
    {CODE-BLOCK/}

---

### Move Document Property Contents to Another Property  

Move the contents of one document property to another using the `Move` patching operation.  

* **Method Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | from | `string` | Path to the property whose contents we want to move |
    | path| `string` | Path to the property we want to move the contents to |

* **Code Sample**  

    {CODE-BLOCK: JavaScript}
    var patchesDocument = new JsonPatchDocument();
patchesDocument.Move("/PropertyName1", "PropertyName2"); // Move PropertyName1 contents to PropertyName2
store.Operations.Send(new JsonPatchOperation(documentId, patchesDocument));
    {CODE-BLOCK/}

---

### Test Patching Operation

Verify patching using the `Test` patching operation.  
If the test fails, all patching operations included in the patches document will be revoked.  

* **Method Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | path | `string` | Path to the property we want to test |
    | value | `object` | Value to compare `path` with |


* **Code Sample**  

    {CODE-BLOCK: JavaScript}
    var patchesDocument = new JsonPatchDocument();
patchesDocument.Test("/PropertyName", "Value"); // Compare property contents with the value 
                                                // Revoke all patch operations if the test fails 
store.Operations.Send(new JsonPatchOperation(documentId, patchesDocument));
    {CODE-BLOCK/}

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
