# Patches: How to Perform Single Document Patch Operations

The **Patch** operation is used to perform partial document updates without having to load, modify, and save a full document. The whole operation is executed on the server-side and is useful as a performance enhancement or for updating denormalized data in entities.

The current page deals with patch operations on single documents.

Patching has three possible interfaces: typed session, non-typed session, and operation.

## Typed Session API
A type safe session interface that allows performing the most patch operations and uses the session facilities to perform multiple operations in one request.

{CODE patch_generic_interfact@ClientApi\Operations\Patches\PatchRequests.cs /}

## Non-Typed Session API
A non-typed session interface that exposes the full functionality and uses the session facilities to perform multiple operations in one request.

{CODE patch_non_generic_interface_in_session@ClientApi\Operations\Patches\PatchRequests.cs /}

{CODE patch_command_data@Common.cs /}

{INFO:Parameters}

We highly recommend using scripts with parameters. This allows RavenDB to cache scripts and boost performance. Parameters can be accessed in the script through the "args" object, and passed using PatchRequest's "Values" parameter.

{CODE patch_request@Common.cs /}

{INFO/}

## Operations API

An operations interface that exposes the full functionality and allows performing ad-hoc patch operations without creating a session.

{CODE patch_non_generic_interface_in_store@ClientApi\Operations\Patches\PatchRequests.cs /}

{CODE patch_operation@Common.cs /}

## Built-in JavaScript Extensions

In addition to ECMAScript 5.1 API, RavenDB introduces a few built-in functions and members:

| ------ |:------:| ------ |
| `id(document)` | function | returns the ID of a document|
| `this` | object | Current document (with metadata) |
| `args` | object | Object containing arguments passed to the script |
| `load(id)` | method | Allows document loading, increases the maximum number of allowed steps in a script. |
| `put(id, data, metadata)` | method | Allows document putting, returns generated id |
| `output(...)` | method | Allows debug on your patch, prints passed messages in output tab |

## Examples

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

## Related Articles

- [Set Based Patch Operation](../../../client-api/operations/patching/set-based)
