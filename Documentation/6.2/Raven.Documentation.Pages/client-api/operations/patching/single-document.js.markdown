# Single Document Patch Operations
---

{NOTE: }

* Patching allows **updating only parts of a document** in a single trip to the server,  
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
      * [Modify value of single field](../../../client-api/operations/patching/single-document#modify-value-of-single-field)  
      * [Modify values of two fields](../../../client-api/operations/patching/single-document#modify-values-of-two-fields)  
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
      * [Patching using inline string compilation](../../../client-api/operations/patching/single-document#patching-using-inline-string-compilation)  
  
  * [Syntax](../../../client-api/operations/patching/single-document#syntax)
      * [Session API syntax](../../../client-api/operations/patching/single-document#session-api-syntax)
      * [Session API using defer syntax](../../../client-api/operations/patching/single-document#session-api-using-defer-syntax)
      * [Operations API syntax](../../../client-api/operations/patching/single-document#operations-api-syntax)
      * [List of script methods syntax](../../../client-api/operations/patching/single-document#list-of-script-methods-syntax)

{NOTE/}

---

{PANEL: API overview}

Patching can be performed using either of the following interfaces (detailed syntax is provided [below](../../../client-api/operations/patching/single-document#syntax)):

* **Session API**
* **Session API using defer** 
* **Operations API**

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
  Define the patch request yourself with a **script** and optional variables.  

* The patch request constructs the `PatchCommandData` command,  
  which is then added to the session using the `defer` function.

* Similar to the above Session API,  
  all patch requests done via `defer` are sent to the server for execution only when _saveChanges_ is called.

{NOTE/}
{NOTE: }

#### Operations API

* [Operations](../../../client-api/operations/what-are-operations) allow performing ad-hoc requests directly on the document store **without** creating a session.

* Similar to the above _defer_ usage, define the patch request yourself with a script and optional variables.  

* The patch requests constructs the `PatchOperation`, which is sent to the server for execution only when _saveChanges_ is called.

{NOTE/}
{PANEL/}

{PANEL: Examples}

{CONTENT-FRAME: }

#### Modify value of single field  

---

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax patch_firstName_session@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Session_defer_syntax patch_firstName_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax patch_firstName_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Modify values of two fields

---

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax patch_firstName_and_lastName_session@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Session_defer_syntax patch_firstName_and_lastName_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax patch_firstName_and_lastName_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Increment value

---

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax increment_value_session@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Session_defer_syntax increment_value_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax increment_value_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Add or increment

---

`addOrIncrement` behavior:

* If document exists + has the specified field =>
    * A numeric field will be **incremented** by the specified value.
    * A string field will be **concatenated** with the specified value.
    * The entity passed is disregarded.
* If document exists + does Not contain the specified field =>
    * The field will be **added** to the document with the specified value.
    * The entity passed is disregarded.
* If document does Not exist =>
    * A new document will be **created** from the provided entity.
    * The value to increment by is disregarded.

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax add_or_increment@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:User_class user_class@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Add or patch  

---

`addOrPatch` behavior:

* If document exists + has the specified field =>
    * The field will be **patched**, the specified value will replace the existing value.
    * The entity passed is disregarded.
* If document exists + does Not contain the specified field =>
    * The field will be **added** to the document with the specified value.
    * The entity passed is disregarded.
* If document does Not exist =>
    * A new document will be **created** from the provided entity.
    * The value to patch by is disregarded.

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax add_or_patch@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:User_class user_class@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Add item to array  

---

`patchArray` behavior:

* If document exists + has the specified array =>
    * Item will be **added** to the array.
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

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Add or patch an existing array  

---

`addOrPatchArray` behavior:

* If document exists + has the specified array field =>
    * The specified values will be **added** to the existing array values.
    * The entity passed is disregarded.
* If document exists + does Not contain the specified array field =>
    * The array field is Not added to the document, no patching is done.
    * The entity passed is disregarded.
* If document does Not exist =>
    * A new document will be **created** from the provided entity.
    * The value to patch by is disregarded.

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax add_or_patch_array@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:User_class user_class@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Insert item into specific position in array  

---

* Inserting an item in a specific position is supported only by the _defer_ or the _operations_ syntax.  
* No exception is thrown if either the document or the specified array does not exist.

{CODE-TABS}
{CODE-TAB:nodejs:Session_defer_syntax insert_item_in_array_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax insert_item_in_array_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Blog_classes Blog_classes@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Modify item in specific position in array  

---

* Inserting an item in a specific position is supported only by the _defer_ or the _operations_ syntax.
* No exception is thrown if either the document or the specified array does not exist.

{CODE-TABS}
{CODE-TAB:nodejs:Session_defer_syntax modify_item_in_array_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax modify_item_in_array_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Blog_classes Blog_classes@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Remove items from array  

---

* Removing all items that match some predicate from an array is supported only by the _defer_ or the _operations_ syntax.
* No exception is thrown if either the document or the specified array does not exist.

{CODE-TABS}
{CODE-TAB:nodejs:Session_defer_syntax filter_items_from_array_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax filter_items_from_array_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Blog_classes Blog_classes@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Load documents in a script  

---

* Loading documents is supported only by the _defer_ or the _operations_ syntax.

{CODE-TABS}
{CODE-TAB:nodejs:Session_defer_syntax load_and_update_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax load_and_update_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Remove property  

---

* Removing a property is supported only by the _defer_ or the _operations_ syntax.

{CODE-TABS}
{CODE-TAB:nodejs:Session_defer_syntax remove_property_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax remove_property_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Rename property  

---

* Renaming a property is supported only by the _defer_ or the _operations_ syntax.

{CODE-TABS}
{CODE-TAB:nodejs:Session_defer_syntax rename_property_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax rename_property_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Add document  

---

* Adding a new document is supported only by the _defer_ or the _operations_ syntax.

{CODE-TABS}
{CODE-TAB:nodejs:Session_defer_syntax add_document_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax add_document_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Clone document  

---

* Cloning a new document is supported only by the _defer_ or the _operations_ syntax.

{CODE-TABS}
{CODE-TAB:nodejs:Session_defer_syntax clone_document_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax clone_document_operation@client-api\operations\patches\patchRequests.js /}
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

{INFO/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Create/Increment counter  

---

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax increment_counter_session@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Session_defer_syntax increment_counter_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax increment_counter_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{INFO: }

Learn more about Counters in this [Counters Overview](../../../document-extensions/counters/overview).

{INFO/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Delete counter  

---

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax delete_counter_session@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Session_defer_syntax delete_counter_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax delete_counter_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Get counter  

---

{CODE-TABS}
{CODE-TAB:nodejs:Session_syntax get_counter_session@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Session_defer_syntax get_counter_defer@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Operations_syntax get_counter_operation@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Patching using inline string compilation  

---

* When using a JavaScript script with the _defer_ or _operations_ syntax,  
  you can apply logic using **inline string compilation**.
* To enable this, set the [Patching.AllowStringCompilation](../../../server/configuration/patching-configuration#patching.allowstringcompilation) configuration key to _true_.

{CODE-TABS}
{CODE-TAB:nodejs:Patching_usingFunction Patching_usingFunction@client-api\operations\patches\patchRequests.js /}
{CODE-TAB:nodejs:Patching_usingEval Patching_usingEval@client-api\operations\patches\patchRequests.js /}
{CODE-TABS/}

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Syntax}

---

### Session API syntax


{CODE:nodejs patch_syntax@client-api\operations\patches\patchRequests.js /}

| Parameter    | Type     | Description                                                                                                                                       |
|--------------|----------|---------------------------------------------------------------------------------------------------------------------------------------------------|
| **id**       | `string` | Document ID on which patching should be performed.                                                                                                |
| **entity**   | `object` | Entity on which patching should be performed. The entity should be one that was returned by the current session in a `load` or `query` operation. |
| **Path**     | `string` | The path to the field.                                                                                                                            |
| **value**    | `object` | Value to set.                                                                                                                                     |

{CODE:nodejs add_or_patch_syntax@client-api\operations\patches\patchRequests.js /}

| Parameter   | Type     | Description                                                                              |
|-------------|----------|------------------------------------------------------------------------------------------|
| **id**      | `string` | Document ID on which patching should be performed.                                       |
| **entity**  | `object` | If the specified document is Not found, a new document will be created from this entity. |
| **Path**    | `string` | The path to the field.                                                                   |
| **value**   | `object` | Value to set.                                                                            |

{CODE:nodejs increment_syntax@client-api\operations\patches\patchRequests.js /}

| Parameter       | Type     | Description                                                                                                                                       |
|-----------------|----------|---------------------------------------------------------------------------------------------------------------------------------------------------|
| **id**          | `string` | Document ID on which patching should be performed.                                                                                                |
| **entity**      | `object` | Entity on which patching should be performed. The entity should be one that was returned by the current session in a `load` or `query` operation. |
| **path**        | `string` | The path to the field.                                                                                                                            |
| **valueToAdd**  | `object` | Value to increment by.<br>Note how numbers are handled with the [JavaScript engine](../../../server/kb/numbers-in-ravendb) in RavenDB.            |

{CODE:nodejs add_or_increment_syntax@client-api\operations\patches\patchRequests.js /}

| Parameter      | Type     | Description                                                                              |
|----------------|----------|------------------------------------------------------------------------------------------|
| **id**         | `string` | Document ID on which patching should be performed.                                       |
| **entity**     | `object` | If the specified document is Not found, a new document will be created from this entity. |
| **path**       | `string` | The path to the field.                                                                   |
| **valueToAdd** | `object` | Value to increment by.                                                                   |

{CODE:nodejs patch_array_syntax@client-api\operations\patches\patchRequests.js /}

| Parameter       | Type                        | Description                                                                                                                                       |
|-----------------|-----------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------|
| **id**          | `string`                    | Document ID on which patching should be performed.                                                                                                |
| **entity**      | `object`                    | Entity on which patching should be performed. The entity should be one that was returned by the current session in a `load` or `query` operation. |
| **pathToArray** | `string`                    | The path to the array field.                                                                                                                      |
| **arrayAdder**  | `(JavaScriptArray) => void` | Function that modifies the array.                                                                                                                 |

{CODE:nodejs add_or_patch_array_syntax@client-api\operations\patches\patchRequests.js /}

| Parameter       | Type                        | Description                                                                              |
|-----------------|-----------------------------|------------------------------------------------------------------------------------------|
| **id**          | `string`                    | Document ID on which patching should be performed.                                       |
| **entity**      | `object`                    | If the specified document is Not found, a new document will be created from this entity. |
| **pathToArray** | `string`                    | The path to the array field.                                                             |
| **arrayAdder**  | `(JavaScriptArray) => void` | Function that modifies the array.                                                        |                                                                                                                                 |

{CODE:nodejs class_JavaScriptArray@client-api\operations\patches\patchRequests.js /}

---

### Session API using defer syntax

{CODE:nodejs defer_syntax@client-api\operations\patches\patchRequests.js /}

| Parameter    | Type       | Description                                                                                               |
|--------------|------------|-----------------------------------------------------------------------------------------------------------|
| **commands** | `object[]` | List of commands that will be executed on the server.<br>Use the `PatchCommandData` command for patching. |

{CODE:nodejs class_PatchCommandData@client-api\operations\patches\patchRequests.js /}

{CODE:nodejs class_PatchRequest@client-api\operations\patches\patchRequests.js /}

---

### Operations API syntax

* Learn more about using operations in this [Operations overview](../../../client-api/operations/what-are-operations).

{CODE:nodejs operations_syntax@client-api\operations\patches\patchRequests.js /}

| Constructor                          | Type           | Description                                                                                                                                                                                                                                                                          |
|--------------------------------------|----------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **id**                               | `string`       | ID of the document to be patched.                                                                                                                                                                                                                                                    |
| **changeVector**                     | `string`       | Change vector of the document to be patched.<br>Used to verify that the document was not modified before the patch is executed. Can be null.                                                                                                                                         |
| **patch**                            | `PatchRequest` | Patch request to perform on the document.                                                                                                                                                                                                                                            |
| **patchIfMissing**                   | `PatchRequest` | Patch request to perform if the specified document is not found.<br>Will run only if no `changeVector` was passed.<br>Can be null.                                                                                                                                                   |
| **skipPatchIfChangeVectorMismatch**  | `boolean`      | `true` - do not patch if the document has been modified.<br>`false` (Default) - execute the patch even if document has been modified.<br><br>An exception is thrown if:<br>this param is `false` + `changeVector` has value + document with that ID and change vector was not found. |

---

### List of script methods syntax

* For a complete list of JavaScript methods available in patch scripts,  
  refer to [Knowledge Base: JavaScript Engine](../../../server/kb/javascript-engine#predefined-javascript-functions).

{PANEL/}

## Related Articles

### Patching

- [Set based patch operation](../../../client-api/operations/patching/set-based)  

### Knowledge Base

- [JavaScript engine](../../../server/kb/javascript-engine)
- [Numbers in JavaScript engine](../../../server/kb/numbers-in-ravendb#numbers-in-javascript-engine)
