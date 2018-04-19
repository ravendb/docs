# Patching : How to Perform Single Document Patch Operations

{NOTE: }
The **Patch** operation is used to perform partial document updates without having to load, modify, and save a full document. The whole operation is executed on the server-side and is useful as a performance enhancement or for updating denormalized data in entities.

The current page deals with patch operations on single documents.

Patching has three possible interfaces: [Typed Session API](../../../client-api/operations/patching/single-document#typed-session-api), [Non-Typed Session API](../../../client-api/operations/patching/single-document#non-typed-session-api), and [Operations API](../../../client-api/operations/patching/single-document#operations-api).

In this page:  
[API overview](../../../client-api/operations/patching/single-document#api-overview)  
[Examples](../../../client-api/operations/patching/single-document#examples)  
{NOTE/}

## API overview

{PANEL: Typed Session API}

A type safe session interface that allows performing the most common patch operations.  
The patch request will be sent to server only after the call to `SaveChanges`, this way it's possile to perform multiple operations in one request to the server.  

### Increment field value
`Session.Advanced.Increment`
{CODE patch_generic_interface_increment@ClientApi\Operations\Patches\PatchRequests.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **T** | `Type` | Entity type |
| **U** | `Type` | Field type, must be of numeric type, or a `string` of `char` for string concatination |
| **entity** | `T` | Entity on which the operation should be performed. The entity should be one that was returned by the current session in a `Load` or `Query` operation, this way, the session can track down the entity's ID |
| **entity id** | `string` | Entity ID on which the operation should be performed. |
| **fieldPath** | `Expression<Func<T, U>>` | Lambda describing the path to the field. |
| **delta** | `U` | Value to be added. |

* Note the numeric values [restrictions](../../../server/kb/numbers-in-ravendb) in JavaScript

### Set field value
`Session.Advanced.Patch`
{CODE patch_generic_interface_set_value@ClientApi\Operations\Patches\PatchRequests.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **T** | `Type` | Entity type |
| **U** | `Type` | Field type|
| **entity** | `T` | Entity on which the operation should be performed. The entity should be one that was returned by the current session in a `Load` or `Query` operation, this way, the session can track down the entity's ID |
| **entity id** | `string` | Entity ID on which the operation should be performed. |
| **fieldPath** | `Expression<Func<T, U>>` | Lambda describing the path to the field. |
| **delta** | `U` | Value to set. |

### Array manipulation
`Session.Advanced.Patch`
{CODE patch_generic_interface_array_modification_lambda@ClientApi\Operations\Patches\PatchRequests.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **T** | `Type` | Entity type |
| **U** | `Type` | Field type|
| **entity** | `T` | Entity on which the operation should be performed. The entity should be one that was returned by the current session in a `Load` or `Query` operation, this way, the session can track down the entity's ID |
| **entity id** | `string` | Entity ID on which the operation should be performed. |
| **fieldPath** | `Expression<Func<T, U>>` | Lambda describing the path to the field. |
| **arrayMofificationLambda** | `Expression<Func<JavaScriptArray<U>, object>>` | Lambda that modifies the array, see `JavaScriptArray` below. |

{INFO:JavaScriptArray}
`JavaScriptArray` allows buildin lambdas representing array manipulations for patches.  

| Method Signature| Return Type | Description |
|--------|:-----|-------------| 
| **Put(T item)** | `JavaScriptArray` | Allows adding `item` to an array. |
| **Put(params T[] items)** | `JavaScriptArray` | Items to be added to the array. |
| **RemoveAt(int index)** | `JavaScriptArray` | Removes item in position `index` in array. |

{INFO/}

{PANEL/}

{PANEL:Non-Typed Session API}
The non-typed session api for patches uses the `Session.Advanced.Defer` function that allows registering single or several commands.  
One of the possible commands is the `PatchCommandData`, describing single document patch command.  
The patch request will be sent to server only after the call to `SaveChanges`, this way it's possile to perform multiple operations in one request to the server.  

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
| **Values** | `Dictionary<string, object>` | Parameters to be passed to the script. The parameters can be accessed using the '$' prefix. Parameter starting with a '$' will be used as is, without further concatination . |

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

###Change field's value

{CODE-TABS}
{CODE-TAB:csharp:Session-typed-syntax patch_firstName_generic@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session-syntax-untyped patch_firstName_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax patch_firstName_non_generic_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Change values of two fields

{CODE-TABS}
{CODE-TAB:csharp:Session-typed-syntax patch_firstName_and_lastName_generic@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session-syntax-untyped pathc_firstName_and_lastName_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax pathc_firstName_and_lastName_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Increment value

{CODE-TABS}
{CODE-TAB:csharp:Session-typed-syntax increment_age_generic@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session-syntax-untyped increment_age_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax increment_age_non_generic_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Add item to array

{CODE-TABS}
{CODE-TAB:csharp:Session-typed-syntax add_new_comment_to_comments_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session-syntax-untyped add_new_comment_to_comments_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax add_new_comment_to_comments_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Insert item into specific position in array

Inserting item into specific position is supported only by the non-typed APIs
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped insert_new_comment_at_position_1_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax insert_new_comment_at_position_1_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Modify item in specific position in array

Inserting item into specific position is supported only by the non-typed APIs
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped modify_a_comment_at_position_3_in_comments_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax modify_a_comment_at_position_3_in_comments_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Filter out items from an array

Filtering items from an array supported only by the non-typed APIs
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped filter_items_from_array_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax filter_items_from_array_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Loading documents in a script

Loading documents supported only by non-typed APIs
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped update_product_name_in_order_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax update_product_name_in_order_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Remove property

Removing property supported only by the non-typed APIs
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped rename_property_age_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax rename_property_age_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

###Rename property

Renaming property supported only by the non-typed APIs
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped rename_property_age_non_generic_session@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax rename_property_age_store@ClientApi\Operations\Patches\PatchRequests.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Patching

- [Set Based Patch Operation](../../../client-api/operations/patching/set-based)  

### Knowledge Base

- [JavaScript Engine](../../../server/kb/javascript-engine)
- [Numbers in JavaScript Engine](../../../server/kb/numbers-in-ravendb#numbers-in-javascript-engine)
