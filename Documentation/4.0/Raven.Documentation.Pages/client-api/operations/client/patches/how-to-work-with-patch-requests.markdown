# Patches: How to work with patch requests?

**Patch** operation is used to perform partial document updates without having to load, modify, and save a full document. This is usefull for performance enhancement or for updating denormalized data in entities.

    The current page deals with patch operations on single docuements, the next pages in this section will deal with patches on more than one docuemnt.

## Syntax
Patching has three possible interfaces:

1. A type safe session interface, that allows performing the most patch operations and uses the session facilities to perform multiple operations in one request.

{CODE patch_generic_interfact@ClientApi\Operations\Client\Patches\PatchRequests.cs /}

2. A non-generic session interface that exposes the full functionality and uses the session facilities to perform multiple operations in one request.

{CODE patch_non_generic_interface_in_session@ClientApi\Operations\Client\Patches\PatchRequests.cs /}

**Parameters**

{CODE patch_command_data@Common.cs /}

{CODE patch_request@Common.cs /}

3. An operations interface that exposes the full functionality and allows performing ad-hoc patch operations, without the need to create a new session

{CODE patch_non_generic_interface_in_store@ClientApi\Operations\Client\Patches\PatchRequests.cs /}

**Parameters**

{CODE patch_operation@Common.cs /}

## Eexamples

{PANEL: Change field's value }
{CODE-TABS}
{CODE-TAB:csharp:Session-typed-syntax patch_firstName_generic@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session-syntax-untyped patch_firstName_non_generic_session@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax patch_firstName_non_generic_store@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL: Change two fields values }
{CODE-TABS}
{CODE-TAB:csharp:Session-typed-syntax patch_firstName_and_lastName_generic@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session-syntax-untyped pathc_firstName_and_lastName_non_generic_session@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax pathc_firstName_and_lastName_store@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL: Increment value}
{CODE-TABS}
{CODE-TAB:csharp:Session-typed-syntax increment_age_generic@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session-syntax-untyped increment_age_non_generic_session@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax increment_age_non_generic_store@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL: Add item to array}
{CODE-TABS}
{CODE-TAB:csharp:Session-typed-syntax add_new_comment_to_comments_generic_session@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Session-syntax-untyped add_new_comment_to_comments_non_generic_session@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax add_new_comment_to_comments_store@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL: Insert item into specific position in array}
Inserting item into specific position is supported only by the non-generic APIs
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped insert_new_comment_at_position_1_session@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax insert_new_comment_at_position_1_store@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL: Modify item in specific position in array}
Inserting item into specific position is supported only by the non-generic APIs
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped modify_a_comment_at_position_3_in_comments_session@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax modify_a_comment_at_position_3_in_comments_store@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL: Remove property}
Removing property supported only by the non-generic APIs
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped remove_property_age_non_generic_session@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax remove_property_age_store@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL: Remove property}
Renaming property supported only by the non-generic APIs
{CODE-TABS}
{CODE-TAB:csharp:Session-syntax-untyped rename_property_age_non_generic_session@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Operations-syntax rename_property_age_store@ClientApi\Operations\Client\Patches\PatchRequests.cs /}
{CODE-TABS/}
{PANEL/}

## Related articles

- [How to use **JavaScript** to **patch** your documents?](../../../client-api/commands/patches/how-to-use-javascript-to-patch-your-documents)  
