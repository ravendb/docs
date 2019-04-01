# Patching: How to Perform Single Document Patch Operations

{NOTE: }
The **Patch** operation is used to perform partial document updates without having to load, modify, and save a full document. The whole operation is executed on the server-side and is useful as a performance enhancement or for updating denormalized data in entities.

The current page deals with patch operations on single documents.

Patching has three possible interfaces: [Session API](../../../client-api/operations/patching/single-document#session-api), [Session API using defer](../../../client-api/operations/patching/single-document#session-api-using-defer), and [Operations API](../../../client-api/operations/patching/single-document#operations-api).

In this page:  
[API overview](../../../client-api/operations/patching/single-document#api-overview)  
[Examples](../../../client-api/operations/patching/single-document#examples)  
{NOTE/}

## API overview

{PANEL: Session API}

A session interface that allows performing the most common patch operations.  
The patch request will be sent to server only after the call to `saveChanges`, this way it's possible to perform multiple operations in one request to the server.  

### Increment Field Value
`session.advanced().increment`
{CODE:java patch_generic_interface_increment@ClientApi\Operations\Patches\PatchRequests.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **T** | `Class` | Entity class |
| **U** | `Class` | Field class, must be of numeric type, or a `String` of `char` for string concatenation |
| **entity** | `T` | Entity on which the operation should be performed. The entity should be one that was returned by the current session in a `load` or `query` operation, this way, the session can track down the entity's ID |
| **entity id** | `String` | Entity ID on which the operation should be performed. |
| **delta** | `U` | Value to be added. |

* Note the numeric values [restrictions](../../../server/kb/numbers-in-ravendb) in JavaScript

### Set Field Value
`session.advanced().patch`
{CODE:java patch_generic_interface_set_value@ClientApi\Operations\Patches\PatchRequests.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **T** | `Class` | Entity Class |
| **U** | `Class` | Field class |
| **entity** | `T` | Entity on which the operation should be performed. The entity should be one that was returned by the current session in a `load` or `query` operation, this way, the session can track down the entity's ID |
| **entity id** | `String` | Entity ID on which the operation should be performed. |
| **delta** | `U` | Value to set. |

### Array Manipulation
`session.advanced().patch`
{CODE:java patch_generic_interface_array_modification_lambda@ClientApi\Operations\Patches\PatchRequests.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **T** | `Class` | Entity class |
| **U** | `Class` | Field class |
| **entity** | `T` | Entity on which the operation should be performed. The entity should be one that was returned by the current session in a `Load` or `Query` operation, this way, the session can track down the entity's ID |
| **entity id** | `String` | Entity ID on which the operation should be performed. |
| **arrayAdder** | `Consumer<JavaScriptArray<U>>` | Lambda that modifies the array, see `JavaScriptArray` below. |

{INFO:JavaScriptArray}
`JavaScriptArray` allows building lambdas representing array manipulations for patches.  

| Method Signature| Return Type | Description |
|--------|:-----|-------------| 
| **put(T item)** | `JavaScriptArray` | Allows adding `item` to an array. |
| **put(T... items)** | `JavaScriptArray` | Items to be added to the array. |
| **put(Collection<T> items)** | `JavaScriptArray` | Items to be added to the array. |
| **removeAt(int index)** | `JavaScriptArray` | Removes item in position `index` in array. |

{INFO/}

{PANEL/}

{PANEL:Session API using defer}
The low level session api for patches uses the `session.advanced().defer` function that allows registering single or several commands.  
One of the possible commands is the `PatchCommandData`, describing single document patch command.  
The patch request will be sent to server only after the call to `saveChanges`, this way it's possible to perform multiple operations in one request to the server.  

`session.advanced().defer`
{CODE:java patch_non_generic_interface_in_session@ClientApi\Operations\Patches\PatchRequests.java /}

{INFO: PatchCommandData}

| Constructor|  | |
|--------|:-----|-------------|
| **id** | `String` | ID of the document to be patched. |
| **changeVector** | `String` | [Can be null] Change vector of the document to be patched, used to verify that the document was not changed before the patch reached it. |
| **patch** | `PatchRequest` | Patch request to be performed on the document. |
| **patchIfMissing** | `PatchRequest` | [Can be null] Patch request to be performed if no document with the given ID was found. |

{INFO/}

{INFO: PatchRequest}

We highly recommend using scripts with parameters. This allows RavenDB to cache scripts and boost performance. Parameters can be accessed in the script through the "args" object, and passed using PatchRequest's "Values" parameter.

| Members | | |
| ------------- | ------------- | ----- |
| **Script** | `String` | JavaScript code to be run. |
| **Values** | `Map<String, Object>` | Parameters to be passed to the script. The parameters can be accessed using the '$' prefix. Parameter starting with a '$' will be used as is, without further concatenation . |

{INFO/}

{PANEL/}


{PANEL: Operations API}
An operations interface that exposes the full functionality and allows performing ad-hoc patch operations without creating a session.  

{CODE:java patch_non_generic_interface_in_store@ClientApi\Operations\Patches\PatchRequests.java /}

{INFO: PatchOperation}

| Constructor|  | |
|--------|:-----|-------------| 
| **id** | `String` | ID of the document to be patched. |
| **changeVector** | `String` | [Can be null] Change vector of the document to be patched, used to verify that the document was not changed before the patch reached it. |
| **patch** | `PatchRequest` | Patch request to be performed on the document. |
| **patchIfMissing** | `PatchRequest` | [Can be null] Patch request to be performed if no document with the given ID was found. Will run only if no `changeVector` was passed. |   
| **skipPatchIfChangeVectorMismatch** | `boolean` | If false and `changeVector` has value, and document with that ID and change vector was not found, will throw exception. |   

{INFO/}

{PANEL/}

{PANEL: Examples}

###Change Field's Value

{CODE-TABS}
{CODE-TAB:java:Session-syntax patch_firstName_generic@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TAB:java:Session-defer-syntax patch_firstName_non_generic_session@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TAB:java:Operations-syntax patch_firstName_non_generic_store@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TABS/}

###Change Values of Two Fields

{CODE-TABS}
{CODE-TAB:java:Session-syntax patch_firstName_and_lastName_generic@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TAB:java:Session-defer-syntax pathc_firstName_and_lastName_non_generic_session@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TAB:java:Operations-syntax pathc_firstName_and_lastName_store@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TABS/}

###Increment Value

{CODE-TABS}
{CODE-TAB:java:Session-syntax increment_age_generic@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TAB:java:Session-defer-syntax increment_age_non_generic_session@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TAB:java:Operations-syntax increment_age_non_generic_store@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TABS/}

###Add Item to Array

{CODE-TABS}
{CODE-TAB:java:Session-syntax add_new_comment_to_comments_generic_session@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TAB:java:Session-defer-syntax add_new_comment_to_comments_non_generic_session@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TAB:java:Operations-syntax add_new_comment_to_comments_store@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TABS/}

###Insert Item into Specific Position in Array

Inserting item into specific position is supported only by the non-typed APIs
{CODE-TABS}
{CODE-TAB:java:Session-defer-syntax insert_new_comment_at_position_1_session@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TAB:java:Operations-syntax insert_new_comment_at_position_1_store@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TABS/}

###Modify Item in Specific Position in Array

Inserting item into specific position is supported only by the non-typed APIs
{CODE-TABS}
{CODE-TAB:java:Session-defer-syntax modify_a_comment_at_position_3_in_comments_session@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TAB:java:Operations-syntax modify_a_comment_at_position_3_in_comments_store@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TABS/}

###Filter out Items from an Array

Filtering items from an array supported only by the non-typed APIs
{CODE-TABS}
{CODE-TAB:java:Session-defer-syntax filter_items_from_array_session@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TAB:java:Operations-syntax filter_items_from_array_store@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TABS/}

###Loading Documents in a Script

Loading documents supported only by non-typed APIs
{CODE-TABS}
{CODE-TAB:java:Session-defer-syntax update_product_name_in_order_session@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TAB:java:Operations-syntax update_product_name_in_order_store@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TABS/}

###Remove Property

Removing property supported only by the non-typed APIs
{CODE-TABS}
{CODE-TAB:java:Session-defer-syntax rename_property_age_non_generic_session@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TAB:java:Operations-syntax rename_property_age_store@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TABS/}

###Rename Property

Renaming property supported only by the non-typed APIs
{CODE-TABS}
{CODE-TAB:java:Session-defer-syntax rename_property_age_non_generic_session@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TAB:java:Operations-syntax rename_property_age_store@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TABS/}

###Add Document

Adding a new document is supported only by the non-typed APIs
{CODE-TABS}
{CODE-TAB:java:Session-defer-syntax add_document_session@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TAB:java:Operations-syntax add_document_store@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TABS/}

###Clone Document

In order to clone a document use put method as follows
{CODE-TABS}
{CODE-TAB:java:Session-defer-syntax clone_document_session@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TAB:java:Operations-syntax clone_document_store@ClientApi\Operations\Patches\PatchRequests.java /}
{CODE-TABS/}

{INFO:Cloning & Attachments} 
The attachments from source document will not be copied to the new one automatically.
{INFO/}

{PANEL/}

## Related Articles

### Patching

- [Set Based Patch Operation](../../../client-api/operations/patching/set-based)  

### Knowledge Base

- [JavaScript Engine](../../../server/kb/javascript-engine)
- [Numbers in JavaScript Engine](../../../server/kb/numbers-in-ravendb#numbers-in-javascript-engine)
