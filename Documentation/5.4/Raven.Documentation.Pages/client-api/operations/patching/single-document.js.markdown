# Single Document Patch Operations
---

{NOTE: }

* Patching allows __updating only parts of a document__ in a single trip to the server,  
  without having to load, modify, and save the entire document back to the database.
 
* This is particularly efficient for large documents or when only a small portion of the document needs to be changed, 
  reducing the amount of data transferred over the network.

* The patching operation is executed on the server-side within a [Single write transaction](../../../client-api/faq/transaction-support).

* This article covers patch operations on single documents from the Client API.  
  To patch multiple documents that match certain criteria see [Set based patching](../../../client-api/operations/patching/set-based).  
  Patching can also be done from the [Studio](../../../studio/database/documents/patch-view).  

* In this page:  

  * [API overview](../../../client-api/operations/patching/single-document#api-overview)    
  
  * [Examples](../../../client-api/operations/patching/single-document#examples)  
      * [Change value of single field](../../../client-api/operations/patching/single-document#change-value-of-single-field)  
      * [Change values of two fields](../../../client-api/operations/patching/single-document#change-values-of-two-fields)  
      * [Increment value](../../../client-api/operations/patching/single-document#increment-value)  
      * [Add or increment](../../../client-api/operations/patching/single-document#add-or-increment)  
      * [Add or patch](../../../client-api/operations/patching/single-document#add-or-patch)
      * [Add item to array](../../../client-api/operations/patching/single-document#add-item-to-array)
      * [Add or patch an existing array](../../../client-api/operations/patching/single-document#add-or-patch-an-existing-array)  
      * [Insert item into specific position in array](../../../client-api/operations/patching/single-document#insert-item-into-specific-position-in-array)  
      * [Modify item in specific position in array](../../../client-api/operations/patching/single-document#modify-item-in-specific-position-in-array)  
      * [Remove items from array](../../../client-api/operations/patching/single-document#remove-items-from-array)  
      * [Load documents in a script](../../../client-api/operations/patching/single-document#load-documents-in-a-script)  
      * [Remove property](../../../client-api/operations/patching/single-document#remove-property)  
      * [Rename property](../../../client-api/operations/patching/single-document#rename-property)  
      * [Add document](../../../client-api/operations/patching/single-document#add-document)  
      * [Clone document](../../../client-api/operations/patching/single-document#clone-document)  
      * [Create/Increment counter](../../../client-api/operations/patching/single-document#createincrement-counter)  
      * [Delete counter](../../../client-api/operations/patching/single-document#delete-counter)  
      * [Get counter](../../../client-api/operations/patching/single-document#get-counter)  
  
  * [Syntax](../../../client-api/operations/patching/single-document#syntax)
      * [Session API syntax](../../../client-api/operations/patching/single-document#session-api-syntax)
      * [Session API using defer syntax](../../../client-api/operations/patching/single-document#session-api-using-defer-syntax)
      * [Operations API syntax](../../../client-api/operations/patching/single-document#operations-api-syntax)
      * [List of script methods syntax](../../../client-api/operations/patching/single-document#list-of-script-methods-syntax)

{NOTE/}

---

{PANEL: API overview}

Patching can be performed using either of the following interfaces (detailed syntax is provided [below](../../../client-api/operations/patching/single-document#syntax)):

* __Session API__
* __Session API using defer__ 
* __Operations API__

---

{NOTE: }

#### Session API

* This interface allows performing most common patch operations.

* Multiple patch methods can be defined on the [session](../../../client-api/session/what-is-a-session-and-how-does-it-work) 
  and are sent to the server for execution in a single batch (along with any other modified documents) only when calling [SaveChanges](../../../client-api/session/saving-changes).

* This API includes the following patching methods (see examples [below](../../../client-api/operations/patching/single-document#examples)):
  * `patch`
  * `addOrPatch`
  * `increment`
  * `addOrIncrement`
  * `patchArray`
  * `addOrPatchArray`

{NOTE/}
{NOTE: }

#### Session API using defer

* Use `defer` to manipulate the patch request directly without wrapper methods.  
  Define the patch request yourself with a __script__ and optional variables.  

* The patch request constructs the `PatchCommandData` command,  
  which is then added to the session using the `defer` function.

* Similar to the above Session API,  
  all patch requests done via `defer` are sent to the server for execution only when _saveChanges_ is called.

{NOTE/}
{NOTE: }

#### Operations API

* [Operations](../../../client-api/operations/what-are-operations) allow performing ad-hoc requests directly on the document store __without__ creating a session.

* Similar to the above _defer_ usage, define the patch request yourself with a script and optional variables.  

* The patch requests constructs the `PatchOperation`, which is sent to the server for execution only when _saveChanges_ is called.

{NOTE/}
{PANEL/}

{PANEL: Examples}

{NOTE: }

<a id="change-value-of-single-field" /> __Change value of single field__:  

---

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax patch_firstName_session@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Session_defer_syntax patch_firstName_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax patch_firstName_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="change-values-of-two-fields" /> __Change values of two fields__:

---

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax patch_firstName_and_lastName_session@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Session_defer_syntax patch_firstName_and_lastName_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax patch_firstName_and_lastName_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="increment-value" /> __Increment value__:

---

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax increment_value_session@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Session_defer_syntax increment_value_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax increment_value_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="add-or-increment" /> __Add or increment__:

---

`addOrIncrement` behavior:

* If document exists + has the specified field =>
    * A numeric field will be __incremented__ by the specified value.
    * A string field will be __concatenated__ with the specified value.
    * The entity passed is disregarded.
* If document exists + does Not contain the specified field =>
    * The field will be __added__ to the document with the specified value.
    * The entity passed is disregarded.
* If document does Not exist =>
    * A new document will be __created__ from the provided entity.
    * The value to increment by is disregarded.

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax add_or_increment@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:User_class user_class@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="add-or-patch" /> __Add or patch__:  

---

`addOrPatch` behavior:

* If document exists + has the specified field =>
    * The field will be __patched__, the specified value will replace the existing value.
    * The entity passed is disregarded.
* If document exists + does Not contain the specified field =>
    * The field will be __added__ to the document with the specified value.
    * The entity passed is disregarded.
* If document does Not exist =>
    * A new document will be __created__ from the provided entity.
    * The value to patch by is disregarded.

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax add_or_patch@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:User_class user_class@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="add-item-to-array" /> __Add item to array__:  

---

`patchArray` behavior:

* If document exists + has the specified array =>
    * Item will be __added__ to the array.
* If document exists + does Not contain the specified array field =>
    * No exception is thrown, no patching is done, a new array is Not created.
* If document does Not exist =>
    * No exception is thrown, no patching is done, a new document is Not created.

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax add_item_to_array_session@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Session_defer_syntax add_item_to_array_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax add_item_to_array_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Blog_classes Blog_classes@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="add-or-patch-an-existing-array" /> __Add or patch an existing array__:  

---

`addOrPatchArray` behavior:

* If document exists + has the specified array field =>
    * The specified values will be __added__ to the existing array values.
    * The entity passed is disregarded.
* If document exists + does Not contain the specified array field =>
    * The array field is Not added to the document, no patching is done.
    * The entity passed is disregarded.
* If document does Not exist =>
    * A new document will be __created__ from the provided entity.
    * The value to patch by is disregarded.

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax add_or_patch_array@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:User_class user_class@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="insert-item-into-specific-position-in-array" /> __Insert item into specific position in array__:  

---

* Inserting an item in a specific position is supported only by the _defer_ or the _operations_ syntax.  
* No exception is thrown if either the document or the specified array does not exist.

{CODE-TABS}
{CODE-TAB:nodejs:Session_defer_syntax insert_item_in_array_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax insert_item_in_array_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Blog_classes Blog_classes@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="modify-item-in-specific-position-in-array" /> __Modify item in specific position in array__:  

---

* Inserting an item in a specific position is supported only by the _defer_ or the _operations_ syntax.
* No exception is thrown if either the document or the specified array does not exist.

{CODE-TABS}
{CODE-TAB:nodejs:Session_defer_syntax modify_item_in_array_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax modify_item_in_array_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Blog_classes Blog_classes@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="remove-items-from-array" /> __Remove items from array__:  

---

* Removing all items that match some predicate from an array is supported only by the _defer_ or the _operations_ syntax.
* No exception is thrown if either the document or the specified array does not exist.

{CODE-TABS}
{CODE-TAB:nodejs:Session_defer_syntax filter_items_from_array_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax filter_items_from_array_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Blog_classes Blog_classes@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="load-documents-in-a-script" /> __Load documents in a script__:  

---

* Loading documents is supported only by the _defer_ or the _operations_ syntax.

{CODE-TABS}
{CODE-TAB:nodejs:Session_defer_syntax load_and_update_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax load_and_update_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="remove-property" /> __Remove property__:  

---

* Removing a property is supported only by the _defer_ or the _operations_ syntax.

{CODE-TABS}
{CODE-TAB:nodejs:Session_defer_syntax remove_property_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax remove_property_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="rename-property" /> __Rename property__:  

---

* Renaming a property is supported only by the _defer_ or the _operations_ syntax.

{CODE-TABS}
{CODE-TAB:nodejs:Session_defer_syntax rename_property_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax rename_property_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="add-document" /> __Add document__:  

---

* Adding a new document is supported only by the _defer_ or the _operations_ syntax.

{CODE-TABS}
{CODE-TAB:nodejs:Session_defer_syntax add_document_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax add_document_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="clone-document" /> __Clone document__:  

---

* Cloning a new document is supported only by the _defer_ or the _operations_ syntax.

{CODE-TABS}
{CODE-TAB:nodejs:Session_defer_syntax clone_document_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax clone_document_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{INFO: }

__Attachments, Counters, Time Series, and Revisions:__

Attachments, counters, time series data, and revisions from the source document will Not be copied to the new document automatically.

{INFO/}

{NOTE/}
{NOTE: }

<a id="createincrement-counter" /> __Create/Increment counter__:  

---

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax increment_counter_session@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Session_defer_syntax increment_counter_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax increment_counter_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{INFO: }

Learn more about Counters in this [Counters Overview](../../../document-extensions/counters/overview).

{INFO/}

{NOTE/}
{NOTE: }

<a id="delete-counter" /> __Delete counter__:  

---

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax delete_counter_session@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Session_defer_syntax delete_counter_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax delete_counter_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{NOTE/}
{NOTE: }

<a id="get-counter" /> __Get counter__:  

---

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax get_counter_session@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Session_defer_syntax get_counter_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax get_counter_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{NOTE/}
{PANEL/}

{PANEL: Syntax}

---

### Session API syntax


{CODE:nodejs patch_syntax@client-api\operations\patches\patchRequests.js /}

| Parameter  | Type     | Description                                                                                                                                       |
|------------|----------|---------------------------------------------------------------------------------------------------------------------------------------------------|
| __id__     | `string` | Document ID on which patching should be performed.                                                                                                |
| __entity__ | `object` | Entity on which patching should be performed. The entity should be one that was returned by the current session in a `load` or `query` operation. |
| __Path__   | `string` | The path to the field.                                                                                                                            |
| __value__  | `object` | Value to set.                                                                                                                                     |

{CODE:nodejs add_or_patch_syntax@client-api\operations\patches\patchRequests.js /}

| Parameter  | Type     | Description                                                                              |
|------------|----------|------------------------------------------------------------------------------------------|
| __id__     | `string` | Document ID on which patching should be performed.                                       |
| __entity__ | `object` | If the specified document is Not found, a new document will be created from this entity. |
| __Path__   | `string` | The path to the field.                                                                   |
| __value__  | `object` | Value to set.                                                                            |

{CODE:nodejs increment_syntax@client-api\operations\patches\patchRequests.js /}

| Parameter        | Type     | Description                                                                                                                                       |
|------------------|----------|---------------------------------------------------------------------------------------------------------------------------------------------------|
| __id__           | `string` | Document ID on which patching should be performed.                                                                                                |
| __entity__       | `object` | Entity on which patching should be performed. The entity should be one that was returned by the current session in a `load` or `query` operation. |
| __path__         | `string` | The path to the field.                                                                                                                            |
| __valueToAdd__   | `object` | Value to increment by.<br>Note how numbers are handled with the [JavaScript engine](../../../server/kb/numbers-in-ravendb) in RavenDB.            |

{CODE:nodejs add_or_increment_syntax@client-api\operations\patches\patchRequests.js /}

| Parameter        | Type     | Description                                                                              |
|------------------|----------|------------------------------------------------------------------------------------------|
| __id__           | `string` | Document ID on which patching should be performed.                                       |
| __entity__       | `object` | If the specified document is Not found, a new document will be created from this entity. |
| __path__         | `string` | The path to the field.                                                                   |
| __valueToAdd__   | `object` | Value to increment by.                                                                   |

{CODE:nodejs patch_array_syntax@client-api\operations\patches\patchRequests.js /}

| Parameter              | Type                        | Description                                                                                                                                       |
|------------------------|-----------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------|
| __id__                 | `string`                    | Document ID on which patching should be performed.                                                                                                |
| __entity__             | `object`                    | Entity on which patching should be performed. The entity should be one that was returned by the current session in a `load` or `query` operation. |
| __pathToArray__        | `string`                    | The path to the array field.                                                                                                                      |
| __arrayAdder__         | `(JavaScriptArray) => void` | Function that modifies the array.                                                                                                                 |

{CODE:nodejs add_or_patch_array_syntax@client-api\operations\patches\patchRequests.js /}

| Parameter              | Type                        | Description                                                                              |
|------------------------|-----------------------------|------------------------------------------------------------------------------------------|
| __id__                 | `string`                    | Document ID on which patching should be performed.                                       |
| __entity__             | `object`                    | If the specified document is Not found, a new document will be created from this entity. |
| __pathToArray__        | `string`                    | The path to the array field.                                                             |
| __arrayAdder__         | `(JavaScriptArray) => void` | Function that modifies the array.                                                        |                                                                                                                                 |

{CODE:nodejs class_JavaScriptArray@client-api\operations\patches\patchRequests.js /}

---

### Session API using defer syntax

{CODE:nodejs defer_syntax@client-api\operations\patches\patchRequests.js /}

| Parameter      | Type       | Description                                                                                               |
|----------------|------------|-----------------------------------------------------------------------------------------------------------|
| __commands__   | `object[]` | List of commands that will be executed on the server.<br>Use the `PatchCommandData` command for patching. |

{CODE:nodejs class_PatchCommandData@client-api\operations\patches\patchRequests.js /}

{CODE:nodejs class_PatchRequest@client-api\operations\patches\patchRequests.js /}

---

### Operations API syntax

* Learn more about using operations in this [Operations overview](../../../client-api/operations/what-are-operations).

{CODE:nodejs operations_syntax@client-api\operations\patches\patchRequests.js /}

| Constructor                         | Type           | Description                                                                                                                                                                                                                                                                          |
|-------------------------------------|----------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __id__                              | `string`       | ID of the document to be patched.                                                                                                                                                                                                                                                    |
| __changeVector__                    | `string`       | Change vector of the document to be patched.<br>Used to verify that the document was not modified before the patch is executed. Can be null.                                                                                                                                         |
| __patch__                           | `PatchRequest` | Patch request to perform on the document.                                                                                                                                                                                                                                            |
| __patchIfMissing__                  | `PatchRequest` | Patch request to perform if the specified document is not found.<br>Will run only if no `changeVector` was passed.<br>Can be null.                                                                                                                                                   |
| __skipPatchIfChangeVectorMismatch__ | `boolean`      | `true` - do not patch if the document has been modified.<br>`false` (Default) - execute the patch even if document has been modified.<br><br>An exception is thrown if:<br>this param is `false` + `changeVector` has value + document with that ID and change vector was not found. |

---

### List of script methods syntax

* This is a partial list of some javascript methods that can be used in patch scripts.  
* For a more comprehensive list, please refer to [Knowledge Base: JavaScript Engine](../../../server/kb/javascript-engine#predefined-javascript-functions).  

| Method               | Arguments                                           | Description                                                                                                                                                                                                                                            |
|----------------------|-----------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __load__             | `string` / `string[]`                               | Loads one or more documents into the context of the script by their document IDs.                                                                                                                                                                      |
| __loadPath__         | A document and a path to an ID within that document | Loads a related document by the path to its ID.                                                                                                                                                                                                        |
| __del__              | Document ID; change vector;                         | Delete the given document by its ID. If you add the expected change vector and the document's current change vector does not match, the document will _not_ be deleted.                                                                                |
| __put__              | Document ID; document; change vector;               | Create or overwrite a document with a specified ID and entity. If you try to overwrite an existing document and pass the expected change vector, the put will fail if the specified change vector does not match the document's current change vector. |
| __cmpxchg__          | Key                                                 | Load a compare exchange value into the context of the script using its key.                                                                                                                                                                            |
| __getMetadata__      | Document                                            | Returns the document's metadata.                                                                                                                                                                                                                       |
| __id__               | Document                                            | Returns the document's ID.                                                                                                                                                                                                                             |
| __lastModified__     | Document                                            | Returns the `DateTime` of the most recent modification made to the given document.                                                                                                                                                                     |
| __counter__          | Document; counter name;                             | Returns the value of the specified counter in the specified document.                                                                                                                                                                                  |
| __counterRaw__       | Document; counter name;                             | Returns the specified counter in the specified document as a key-value pair.                                                                                                                                                                           |
| __incrementCounter__ | Document; counter name;                             | Increases the value of the counter by one.                                                                                                                                                                                                             |
| __deleteCounter__    | Document; counter name;                             | Deletes the counter.                                                                                                                                                                                                                                   |
| __timeseries__       | Document; the time series' name;                    | Returns the specified time series object.                                                                                                                                                                                                              |

{PANEL/}

## Related Articles

### Patching

- [Set based patch operation](../../../client-api/operations/patching/set-based)  

### Knowledge Base

- [JavaScript engine](../../../server/kb/javascript-engine)
- [Numbers in JavaScript engine](../../../server/kb/numbers-in-ravendb#numbers-in-javascript-engine)
