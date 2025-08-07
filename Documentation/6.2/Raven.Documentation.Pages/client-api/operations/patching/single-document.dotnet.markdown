# Single Document Patch Operations

{NOTE: }

* The __Patch__ operation is used to perform _partial_ document updates with __one trip to the server__, 
  instead of loading, modifying, and saving a full document.  
  The whole operation is executed on the server-side and is useful as a performance enhancement or for 
  updating denormalized data in entities.

* Since the operation is executed in a single request to the database,  
  the patch command is performed in a single write [transaction](../../../client-api/faq/transaction-support).  

* The current page covers patch operations on single documents.

* Patching has three possible interfaces: [Session API](../../../client-api/operations/patching/single-document#session-api), 
[Session API using Defer](../../../client-api/operations/patching/single-document#session-api-using-defer), 
and [Operations API](../../../client-api/operations/patching/single-document#operations-api).

* Patching can be done from the [client API](../../../client-api/operations/patching/single-document#examples) as well as in the [studio](../../../studio/database/documents/patch-view).  

In this page:  

* [API overview](../../../client-api/operations/patching/single-document#api-overview)  
  * [Session API](../../../client-api/operations/patching/single-document#session-api)
  * [Session API using Defer](../../../client-api/operations/patching/single-document#session-api-using-defer)
  * [Operations API](../../../client-api/operations/patching/single-document#operations-api)
  * [List of script methods](../../../client-api/operations/patching/single-document#list-of-script-methods)
* [Examples](../../../client-api/operations/patching/single-document#examples)  
  * [Change value of single field](../../../client-api/operations/patching/single-document#change-value-of-single-field)  
  * [Change values of two fields](../../../client-api/operations/patching/single-document#change-values-of-two-fields)  
  * [Increment value](../../../client-api/operations/patching/single-document#increment-value)  
  * [Add or increment](../../../client-api/operations/patching/single-document#add-or-increment)  
  * [Add or patch](../../../client-api/operations/patching/single-document#add-or-patch)  
  * [Add or patch to an existing array](../../../client-api/operations/patching/single-document#add-or-patch-to-an-existing-array)  
  * [Add item to array](../../../client-api/operations/patching/single-document#add-item-to-array)  
  * [Insert item into specific position in array](../../../client-api/operations/patching/single-document#insert-item-into-specific-position-in-array)  
  * [Modify item in specific position in array](../../../client-api/operations/patching/single-document#modify-item-in-specific-position-in-array)  
  * [Remove items from array](../../../client-api/operations/patching/single-document#remove-items-from-array)  
  * [Loading documents in a script](../../../client-api/operations/patching/single-document#loading-documents-in-a-script)  
  * [Remove property](../../../client-api/operations/patching/single-document#remove-property)  
  * [Rename property](../../../client-api/operations/patching/single-document#rename-property)  
  * [Add document](../../../client-api/operations/patching/single-document#add-document)  
  * [Clone document](../../../client-api/operations/patching/single-document#clone-document)  
  * [Increment counter](../../../client-api/operations/patching/single-document#increment-counter)  
  * [Delete counter](../../../client-api/operations/patching/single-document#delete-counter)  
  * [Get counter](../../../client-api/operations/patching/single-document#get-counter)  
  * [Patching using inline string compilation](../../../client-api/operations/patching/single-document#patching-using-inline-string-compilation)  

{NOTE/}

## API Overview

{PANEL: Session API}

A type-safe session interface that allows performing the most common patch operations.  
The patch request will be sent to the server only when calling `SaveChanges`.  
This way it's possible to perform multiple operations in one request to the server.  

{NOTE: }

### Increment field value

---

`Session.Advanced.Increment`
{CODE patch_generic_interface_increment@ClientApi\Operations\Patches\PatchRequests.cs /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **T** | `Type` | Entity type |
| **U** | `Type` | Field type, must be of numeric type or a `string` of `char` for string concatenation |
| **entity** | `T` | Entity on which the operation should be performed. The entity should be one that was returned by the current session in a `Load` or `Query` operation, this way, the session can track down the entity's ID |
| **entity id** | `string` | Entity ID on which the operation should be performed. |
| **fieldPath** | `Expression<Func<T, U>>` | Lambda describing the path to the field. |
| **delta** | `U` | Value to be added. |

* Note how numbers are handled with the [JavaScript engine](../../../server/kb/numbers-in-ravendb) in RavenDB.

---

`Session.Advanced.AddOrIncrement`
{CODE add_or_increment_generic@ClientApi\Operations\Patches\PatchRequests.cs /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **T** | `Type` | Entity type |
| **TU** | `Type` | Field type, must be of numeric type or a `string` of `char` for string concatenation |
| **entity** | `T` | Entity on which the operation should be performed. The entity should be one that was returned by the current session in a `Load` or `Query` operation, this way, the session can track down the entity's ID |
| **entity id** | `string` | Entity ID on which the operation should be performed. |
| **path** | `Expression<Func<T, TU>>` | Lambda describing the path to the field. |
| **valToAdd** | `U` | Value to be added. |

{NOTE/}

{NOTE: }

### Set field value

---

`Session.Advanced.Patch`
{CODE patch_generic_interface_set_value@ClientApi\Operations\Patches\PatchRequests.cs /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **T** | `Type` | Entity type |
| **U** | `Type` | Field type|
| **entity** | `T` | Entity on which the operation should be performed. The entity should be one that was returned by the current session in a `Load` or `Query` operation. This way the session can track down the entity's ID. |
| **entity id** | `string` | Entity ID on which the operation should be performed. |
| **fieldPath** | `Expression<Func<T, U>>` | Lambda describing the path to the field. |
| **delta** | `U` | Value to set. |

---

`Session.Advanced.AddOrPatch`

{CODE add_or_patch_generic@ClientApi\Operations\Patches\PatchRequests.cs /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **T** | `Type` | Entity type |
| **TU** | `Type` | Field type|
| **entity** | `T` | Entity on which the operation should be performed. The entity should be one that was returned by the current session in a `Load` or `Query` operation. This way the session can track down the entity's ID. |
| **entity id** | `string` | Entity ID on which the operation should be performed. |
| **fieldPath** | `Expression<Func<T, TU>>` | Lambda describing the path to the field. |
| **value** | `U` | Value to set. |

{NOTE/}

{NOTE: }

### Array manipulation

---

`Session.Advanced.Patch`
{CODE patch_generic_interface_array_modification_lambda@ClientApi\Operations\Patches\PatchRequests.cs /}

| Parameters                   | Type | Description |
|------------------------------| ------------- | ----- |
| **T**                        | `Type` | Entity type |
| **U**                        | `Type` | Field type|
| **entity**                   | `T` | Entity on which the operation should be performed. The entity should be one that was returned by the current session in a `Load` or `Query` operation. This way the session can track down the entity's ID. |
| **entity id**                | `string` | Entity ID on which the operation should be performed. |
| **fieldPath**                | `Expression<Func<T, U>>` | Lambda describing the path to the field. |
| **arrayModificationLambda** | `Expression<Func<JavaScriptArray<U>, object>>` | Lambda that modifies the array, see `JavaScriptArray` below. |

---

`Session.Advanced.AddOrPatch`
{CODE add_or_patch_array_generic@ClientApi\Operations\Patches\PatchRequests.cs /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **T** | `Type` | Entity type |
| **TU** | `Type` | Field type|
| **entity** | `T` | Entity on which the operation should be performed. The entity should be one that was returned by the current session in a `Load` or `Query` operation. This way the session can track down the entity's ID. |
| **entity id** | `string` | Entity ID on which the operation should be performed. |
| **path** | `Expression<Func<T, TU>>` | Lambda describing the path to the field. |
| **Expression<Func<JavaScriptArray>** | `Expression<Func<JavaScriptArray<TU>, object>>` | Lambda that modifies the array, see `JavaScriptArray` below. |
| **arrayAdder** | `Add()` | Values to add to array. |

{INFO: JavaScriptArray}

`JavaScriptArray` allows building lambdas representing array manipulations for patches.  

| Method Signature| Return Type | Description |
|--------|:-----|-------------| 
| **Put(T item)** | `JavaScriptArray` | Allows adding `item` to an array. |
| **Put(params T[] items)** | `JavaScriptArray` | Items to be added to the array. |
| **RemoveAt(int index)** | `JavaScriptArray` | Removes item in position `index` in array. |
| **RemoveAll(Func<T, bool> predicate)** | `JavaScriptArray` | Removes all the items in the array that satisfy the given predicate. |

{INFO/}

{NOTE/}

{PANEL/}

{PANEL: Session API using Defer}

The non-typed Session API for patches uses the `Session.Advanced.Defer` function which allows registering one or more commands.  
One of the possible commands is the `PatchCommandData`, describing single document patch command.  
The patch request will be sent to the server only when calling `SaveChanges`, this way it's possible to perform multiple operations in one request to the server.  

`Session.Advanced.Defer`
{CODE patch_non_generic_interface_in_session@ClientApi\Operations\Patches\PatchRequests.cs /}

{INFO: }

#### PatchCommandData

| Constructor        | Type           | Description                                                                                                                              |
|--------------------|----------------|------------------------------------------------------------------------------------------------------------------------------------------|
| **id**             | `string`       | ID of the document to be patched.                                                                                                        |
| **changeVector**   | `string`       | [Can be null] Change vector of the document to be patched, used to verify that the document was not changed before the patch reached it. |
| **patch**          | `PatchRequest` | Patch request to be performed on the document.                                                                                           |
| **patchIfMissing** | `PatchRequest` | [Can be null] Patch request to be performed if no document with the given ID was found.                                                  |

{INFO/}

{INFO: }

#### PatchRequest

We highly recommend using scripts with parameters. This allows RavenDB to cache scripts and boost performance. 
Parameters can be accessed in the script through the `args` object and passed using PatchRequest's "Values" parameter.

| Property   | Type                         | Description                                                                                                                                                                        |
|------------|------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Script** | `string`                     | The patching script, written in JavaScript.                                                                                                                                        |
| **Values** | `Dictionary<string, object>` | Parameters to be passed to the script.<br>The parameters can be accessed using the '$' prefix.<br>Parameter starting with a '$' will be used as is, without further concatenation. |

{INFO/}

{PANEL/}

{PANEL: Operations API}

An operations interface that exposes the full functionality and allows performing ad-hoc patch operations without creating a session.  

`Raven.Client.Documents.Operations.Send`  
`Raven.Client.Documents.Operations.SendAsync`  

{CODE patch_non_generic_interface_in_store@ClientApi\Operations\Patches\PatchRequests.cs /}

{INFO: PatchOperation}

| Constructor                         | Type           | Description                                                                                                                                                                                                                                                                          |
|-------------------------------------|----------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **id**                              | `string`       | ID of the document to be patched.                                                                                                                                                                                                                                                    |
| **changeVector**                    | `string`       | Change vector of the document to be patched.<br>Used to verify that the document was not modified before the patch reached it.<br>Can be `null`.                                                                                                                                     |
| **patch**                           | `PatchRequest` | Patch request to perform on the document.                                                                                                                                                                                                                                            |
| **patchIfMissing**                  | `PatchRequest` | Patch request to perform if the specified document is not found.<br>Will run only if no `changeVector` was passed.<br>Can be `null`.                                                                                                                                                 |
| **skipPatchIfChangeVectorMismatch** | `bool`         | `true` - do not patch if the document has been modified.<br>`false` (Default) - execute the patch even if document has been modified.<br><br>An exception is thrown if:<br>this param is `false` + `changeVector` has value + document with that ID and change vector was not found. |

{INFO/}

{PANEL/}

{PANEL: List of script methods}

This is a list of a few of the javascript methods that can be used in patch scripts.  
See the more comprehensive list at [Knowledge Base: JavaScript Engine](../../../server/kb/javascript-engine#predefined-javascript-functions).  

| Method               | Arguments                                           | Description                                                                                                                                                                                                                                            |
|----------------------|-----------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **load**             | `string` or `string[]`                              | Loads one or more documents into the context of the script by their document IDs                                                                                                                                                                       |
| **loadPath**         | A document and a path to an ID within that document | Loads a related document by the path to its ID                                                                                                                                                                                                         |
| **del**              | Document ID; change vector                          | Delete the given document by its ID. If you add the expected change vector and the document's current change vector does not match, the document will _not_ be deleted.                                                                                |
| **put**              | Document ID; document; change vector                | Create or overwrite a document with a specified ID and entity. If you try to overwrite an existing document and pass the expected change vector, the put will fail if the specified change vector does not match the document's current change vector. |
| **cmpxchg**          | Key                                                 | Load a compare exchange value into the context of the script using its key                                                                                                                                                                             |
| **getMetadata**      | Document                                            | Returns the document's metadata                                                                                                                                                                                                                        |
| **id**               | Document                                            | Returns the document's ID                                                                                                                                                                                                                              |
| **lastModified**     | Document                                            | Returns the `DateTime` of the most recent modification made to the given document                                                                                                                                                                      |
| **counter**          | Document; counter name                              | Returns the value of the specified counter in the specified document                                                                                                                                                                                   |
| **counterRaw**       | Document; counter name                              | Returns the specified counter in the specified document as a key-value pair                                                                                                                                                                            |
| **incrementCounter** | Document; counter name                              | Increases the value of the counter by one                                                                                                                                                                                                              |
| **deleteCounter**    | Document; counter name                              | Deletes the counter                                                                                                                                                                                                                                    |
| **spatial.distance** | Two points by latitude and longitude; spatial units | Find the distance between to points on the earth                                                                                                                                                                                                       |
| **timeseries**       | Document; the time series' name                     | Returns the specified time series object                                                                                                                                                                                                               |

{PANEL/}

{PANEL: Examples}

### Change value of single field

{CODE-TABS}
{CODE-TAB:csharp:Session_syntax patch_firstName_generic@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session_defer_syntax patch_firstName_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations_syntax patch_firstName_non_generic_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

---

### Change values of two fields

{CODE-TABS}
{CODE-TAB:csharp:Session_syntax patch_firstName_and_lastName_generic@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session_defer_syntax pathc_firstName_and_lastName_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations_syntax pathc_firstName_and_lastName_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

---

### Increment value

{CODE-TABS}
{CODE-TAB:csharp:Session_syntax increment_age_generic@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session_defer_syntax increment_age_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations_syntax increment_age_non_generic_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

---

### Add or increment

`AddOrIncrement` increments an existing field or adds a new one in documents where they didn't exist.

{CODE Add_Or_Increment_Sample@ClientApi\Operations\Patches\PatchRequests.cs /}

---

### Add or patch

`AddOrPatch` adds or edits field(s) in a single document.  

If the document doesn't yet exist, this operation adds the document but doesn't patch it.  

{CODE Add_Or_Patch_Sample@ClientApi\Operations\Patches\PatchRequests.cs /}

---

### Add or patch to an existing array

This sample shows how to patch an existing array or add it to documents where it doesn't yet exist.

{CODE Add_Or_Patch_Array_Sample@ClientApi\Operations\Patches\PatchRequests.cs /}

---

### Add item to array

{CODE-TABS}
{CODE-TAB:csharp:Session_syntax add_new_comment_to_comments_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session_defer_syntax add_new_comment_to_comments_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations_syntax add_new_comment_to_comments_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

---

### Insert item into specific position in array

Inserting item into specific position is supported only by the non-typed APIs.

{CODE-TABS}
{CODE-TAB:csharp:Session_defer_syntax insert_new_comment_at_position_1_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations_syntax insert_new_comment_at_position_1_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

---

### Modify item in specific position in array

Inserting item into specific position is supported only by the non-typed APIs.

{CODE-TABS}
{CODE-TAB:csharp:Session_defer_syntax modify_a_comment_at_position_3_in_comments_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations_syntax modify_a_comment_at_position_3_in_comments_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

---

### Remove items from array

{CODE-TABS}
{CODE-TAB:csharp:Session_syntax filter_items_from_array_session_generic@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session_defer_syntax filter_items_from_array_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations_syntax filter_items_from_array_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

---

### Loading documents in a script

Loading documents is supported only by the non-typed APIs.

{CODE-TABS}
{CODE-TAB:csharp:Session_defer_syntax update_product_name_in_order_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations_syntax update_product_name_in_order_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

---

### Remove property

Removing property supported only by the non-typed APIs.

{CODE-TABS}
{CODE-TAB:csharp:Session_defer_syntax remove_property_age_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations_syntax remove_property_age_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

---

### Rename property

Renaming property supported only by the non-typed APIs.

{CODE-TABS}
{CODE-TAB:csharp:Session_defer_syntax rename_property_age_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations_syntax rename_property_age_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

---

### Add document

Adding a new document is supported only by the non-typed APIs.

{CODE-TABS}
{CODE-TAB:csharp:Session_defer_syntax add_document_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations_syntax add_document_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

---

### Clone document

To clone a document via patching, use the `put` method within the patching script as follows:

{CODE-TABS}
{CODE-TAB:csharp:Session_defer_syntax clone_document_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations_syntax clone_document_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

{INFO: }

**Attachments, Counters, Time Series, and Revisions:**

  * When cloning a document via patching, only the document's fields are copied to the new document.  
    Attachments, counters, time series data, and revisions from the source document will Not be copied automatically. 
  * To manage time series & counters via patching, you can use the pre-defined JavaScript methods listed here:
    [Counters methods](../../../server/kb/javascript-engine#counter-operations) & [Time series methods](../../../server/kb/javascript-engine#time-series-operations).  
  * Note: When [Cloning a document via the Studio](../../../studio/database/documents/create-new-document#clone-an-existing-document),
    attachments, counters, time Series, and revisions will be copied automatically.

**Archived documents:**

  * If the source document is archived, the cloned document will Not be archived.  
    To learn more about archived documents, see [Data archival overview](../../../data-archival/overview).

{INFO/}

---

### Increment counter

In order to increment or create a counter use <code>incrementCounter</code> method as follows:

{CODE-TABS}
{CODE-TAB:csharp:Session_syntax increment_counter_by_document_reference_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session_defer_syntax increment_counter_by_document_id_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations_syntax increment_counter_by_document_id_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

{INFO: Method overloading & value restrictions}

The method can be called by document ID or by document reference and the value can be negative.

{INFO/}

---

### Delete counter

In order to delete a counter use <code>deleteCounter</code> method as follows:

{CODE-TABS}
{CODE-TAB:csharp:Session_syntax delete_counter_by_document_id_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session_defer_syntax delete_counter_by_document_refference_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operation_syntax delete_counter_by_document_refference_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

{INFO: Method overloading}

The method can be called by document ID or by document reference

{INFO/}

---

### Get counter

In order to get a counter while patching use <code>counter</code> method as follows:

{CODE-TABS}
{CODE-TAB:csharp:Session_syntax get_counter_by_document_id_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session_defer_syntax get_counter_by_document_id_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations_syntax get_counter_by_document_id_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

{INFO: Method overloading}

The method can be called by document ID or by document reference.

{INFO/}

---

### Patching using inline string compilation

* When using a JavaScript script with the _defer_ or _operations_ syntax,  
  you can apply logic using **inline string compilation**.

* To enable this, set the [Patching.AllowStringCompilation](../../../server/configuration/patching-configuration#patching.allowstringcompilation) configuration key to _true_.

{CODE-TABS}
{CODE-TAB:csharp:Patching_usingFunction patch_using_inline_compilation_with_function@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Patching_usingEval patch_using_inline_compilation_with_eval@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Patching

- [Set based patch operation](../../../client-api/operations/patching/set-based)  

### Knowledge Base

- [JavaScript engine](../../../server/kb/javascript-engine)
- [Numbers in JavaScript engine](../../../server/kb/numbers-in-ravendb#numbers-in-javascript-engine)
