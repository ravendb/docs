# Patching: How to Perform Single Document Patch Operations

{NOTE: }
The **Patch** operation is used to perform partial document updates without having to load, modify, and save a full document. The whole operation is executed on the server-side and is useful as a performance enhancement or for updating denormalized data in entities.

The current page deals with patch operations on single documents.

Patching has three possible interfaces: [Typed Session API](../../../client-api/operations/patching/single-document#typed-session-api), [Non-Typed Session API](../../../client-api/operations/patching/single-document#non-typed-session-api), and [Operations API](../../../client-api/operations/patching/single-document#operations-api).

In this page:  
[API overview](../../../client-api/operations/patching/single-document#api-overview)  
[Examples](../../../client-api/operations/patching/single-document#examples)  
{NOTE/}

## API Overview

{PANEL: Typed Session API}

A type safe session interface that allows performing the most common patch operations.  
The patch request will be sent to server only after the call to `SaveChanges`. This way it's possible to perform multiple operations in one request to the server.  

### Increment Field Value
`Session.Advanced.Increment`
{CODE patch_generic_interface_increment@ClientApi\Operations\Patches\PatchRequests.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **T** | `Type` | Entity type |
| **U** | `Type` | Field type, must be of numeric type or a `string` of `char` for string concatenation |
| **entity** | `T` | Entity on which the operation should be performed. The entity should be one that was returned by the current session in a `Load` or `Query` operation, this way, the session can track down the entity's ID |
| **entity id** | `string` | Entity ID on which the operation should be performed. |
| **fieldPath** | `Expression<Func<T, U>>` | Lambda describing the path to the field. |
| **delta** | `U` | Value to be added. |

* Note the numeric values [restrictions](../../../server/kb/numbers-in-ravendb) in JavaScript

### Set Field Value
`Session.Advanced.Patch`
{CODE patch_generic_interface_set_value@ClientApi\Operations\Patches\PatchRequests.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **T** | `Type` | Entity type |
| **U** | `Type` | Field type|
| **entity** | `T` | Entity on which the operation should be performed. The entity should be one that was returned by the current session in a `Load` or `Query` operation. This way the session can track down the entity's ID. |
| **entity id** | `string` | Entity ID on which the operation should be performed. |
| **fieldPath** | `Expression<Func<T, U>>` | Lambda describing the path to the field. |
| **delta** | `U` | Value to set. |

### Array Manipulation
`Session.Advanced.Patch`
{CODE patch_generic_interface_array_modification_lambda@ClientApi\Operations\Patches\PatchRequests.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **T** | `Type` | Entity type |
| **U** | `Type` | Field type|
| **entity** | `T` | Entity on which the operation should be performed. The entity should be one that was returned by the current session in a `Load` or `Query` operation. This way the session can track down the entity's ID. |
| **entity id** | `string` | Entity ID on which the operation should be performed. |
| **fieldPath** | `Expression<Func<T, U>>` | Lambda describing the path to the field. |
| **arrayMofificationLambda** | `Expression<Func<JavaScriptArray<U>, object>>` | Lambda that modifies the array, see `JavaScriptArray` below. |

{INFO:JavaScriptArray}
`JavaScriptArray` allows building lambdas representing array manipulations for patches.  

| Method Signature| Return Type | Description |
|--------|:-----|-------------| 
| **Put(T item)** | `JavaScriptArray` | Allows adding `item` to an array. |
| **Put(params T[] items)** | `JavaScriptArray` | Items to be added to the array. |
| **RemoveAt(int index)** | `JavaScriptArray` | Removes item in position `index` in array. |
| **RemoveAll(Func<T, bool> predicate)** | `JavaScriptArray` | Removes all the items in the array that satisfy the given predicate. |

{INFO/}

{PANEL/}

{PANEL:Non-Typed Session API}
The non-typed Session API for patches uses the `Session.Advanced.Defer` function that allows registering single or several commands.  
One of the possible commands is the `PatchCommandData`, describing single document patch command.  
The patch request will be sent to server only after the call to `SaveChanges`, this way it's possible to perform multiple operations in one request to the server.  

`Session.Advanced.Defer`
{CODE patch_non_generic_interface_in_session@ClientApi\Operations\Patches\PatchRequests.cs /}

{INFO: PatchCommandData}

| Constructor|  | |
|--------|:-----|-------------| 
| **id** | `string` | ID of the document to be patched. |
| **changeVector** | `string` | [Can be null] Change vector of the document to be patched, used to verify that the document was not changed before the patch reached it. |
| **patch** | `PatchRequest` | Patch request to be performed on the document. |
| **patchIfMissing** | `PatchRequest` | [Can be null] Patch request to be performed if no document with the given ID was found. |   

{INFO/}

{INFO: PatchRequest}

We highly recommend using scripts with parameters. This allows RavenDB to cache scripts and boost performance. Parameters can be accessed in the script through the "args" object, and passed using PatchRequest's "Values" parameter.

| Members | | |
| ------------- | ------------- | ----- |
| **Script** | `string` | JavaScript code to be run. |
| **Values** | `Dictionary<string, object>` | Parameters to be passed to the script. The parameters can be accessed using the '$' prefix. Parameter starting with a '$' will be used as is, without further concatenation . |

{INFO/}

{PANEL/}


{PANEL: Operations API}
An operations interface that exposes the full functionality and allows performing ad-hoc patch operations without creating a session.  

`Raven.Client.Documents.Operations.Send`
`Raven.Client.Documents.Operations.SendAsync`

{CODE patch_non_generic_interface_in_store@ClientApi\Operations\Patches\PatchRequests.cs /}

{INFO: PatchOperation}

| Constructor|  | |
|--------|:-----|-------------| 
| **id** | `string` | ID of the document to be patched. |
| **changeVector** | `string` | [Can be null] Change vector of the document to be patched, used to verify that the document was not changed before the patch reached it. |
| **patch** | `PatchRequest` | Patch request to be performed on the document. |
| **patchIfMissing** | `PatchRequest` | [Can be null] Patch request to be performed if no document with the given ID was found. Will run only if no `changeVector` was passed. |   
| **skipPatchIfChangeVectorMismatch** | `bool` | If false and `changeVector` has value, and document with that ID and change vector was not found, will throw exception. |   

{INFO/}

{PANEL/}

{PANEL: Examples}

###Change Field's Value

{CODE-TABS}
{CODE-TAB:csharp:Session-typed-syntax patch_firstName_generic@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session-syntax-untyped patch_firstName_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax patch_firstName_non_generic_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Change Values of Two Fields

{CODE-TABS}
{CODE-TAB:csharp:Session-typed-syntax patch_firstName_and_lastName_generic@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session-syntax-untyped pathc_firstName_and_lastName_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax pathc_firstName_and_lastName_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Increment Value

{CODE-TABS}
{CODE-TAB:csharp:Session-typed-syntax increment_age_generic@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session-syntax-untyped increment_age_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax increment_age_non_generic_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Add Item to Array

{CODE-TABS}
{CODE-TAB:csharp:Session-typed-syntax add_new_comment_to_comments_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session-syntax-untyped add_new_comment_to_comments_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax add_new_comment_to_comments_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Insert Item into Specific Position in Array

Inserting item into specific position is supported only by the non-typed APIs
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped insert_new_comment_at_position_1_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax insert_new_comment_at_position_1_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Modify Item in Specific Position in Array

Inserting item into specific position is supported only by the non-typed APIs
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped modify_a_comment_at_position_3_in_comments_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax modify_a_comment_at_position_3_in_comments_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Filter out Items from an Array

{CODE-TABS}
{CODE-TAB:csharp:Session-typed-syntax filter_items_from_array_session_generic@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session-syntax-untyped filter_items_from_array_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax filter_items_from_array_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Loading Documents in a Script

Loading documents supported only by non-typed APIs
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped update_product_name_in_order_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax update_product_name_in_order_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Remove Property

Removing property supported only by the non-typed APIs
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped remove_property_age_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax remove_property_age_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Rename Property

Renaming property supported only by the non-typed APIs
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped rename_property_age_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax rename_property_age_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Add Document

Adding a new document is supported only by the non-typed APIs
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped add_document_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax add_document_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Clone Document

In order to clone a document use put method as follows
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped clone_document_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax clone_document_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

{INFO:Cloning, Attachments & Counters} 
The attachments and/or counters from source document will not be copied to the new one automatically.
{INFO/}

###Increment Counter

In order to increment or create a counter use <code>incrementCounter</code> method as follows
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-typed increment_counter_by_document_reference_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session-syntax-untyped increment_counter_by_document_id_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax increment_counter_by_document_id_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

{INFO:Method Overloading & Value restrictions}
The method can be called by document ID or by document reference and the value can be negative
{INFO/}

###Delete Counter

In order to delete a counter use <code>deleteCounter</code> method as follows
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-typed delete_counter_by_document_id_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session-syntax-untyped delete_counter_by_document_refference_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax delete_counter_by_document_refference_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

{INFO:Method Overloading}
The method can be called by document ID or by document reference
{INFO/}

###Get Counter

In order to get a counter while patching use <code>counter</code> method as follows
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-typed get_counter_by_document_id_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session-syntax-untyped get_counter_by_document_id_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax get_counter_by_document_id_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

{INFO:Method Overloading}
The method can be called by document ID or by document reference
{INFO/}

{PANEL/}

## Related Articles

### Patching

- [Set Based Patch Operation](../../../client-api/operations/patching/set-based)  

### Knowledge Base

- [JavaScript Engine](../../../server/kb/javascript-engine)
- [Numbers in JavaScript Engine](../../../server/kb/numbers-in-ravendb#numbers-in-javascript-engine)
