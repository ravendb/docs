# Commands : Patches: How to work with patch requests?

**Patch** command is used to perform partial document updates without having to load, modify, and save a full document. This is usefull for performance enhancement or for updating denormalized data in entities.

## Syntax
Patching has three possible interfaces:

1. A generic session interface, that is used for the most common cases, and uses the session facilities to perform multiple operations in one request.

{CODE patch_generic_interfact@ClientApi\Commands\Patches\PatchRequests.cs /}

2. A non-generic session interface that exposes the full functionality and uses the session facilities to perform multiple operations in one request.

{CODE patch_non_generic_interface_in_session@ClientApi\Commands\Patches\PatchRequests.cs /}

**Parameters**

{CODE patch_command_data@Common.cs /}

{CODE patch_request@Common.cs /}

3. A store interface that exposes the full functionality and allows performing ad-hoc patch operations, without the need to create a new session

{CODE patch_non_generic_interface_in_store@ClientApi\Commands\Patches\PatchRequests.cs /}

**Parameters**

{CODE patch_operation@Common.cs /}

## Eexamples

{PANEL: Change field's value }
{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax patch_firstName_generic@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Non-Generic-session-syntax patch_firstName_non_generic_session@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Store-syntax patch_firstName_non_generic_store@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL: Change two fields values }
{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax patch_firstName_and_lastName_generic@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Non-Generic-session-syntax pathc_firstName_and_lastName_non_generic_session@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Store-syntax pathc_firstName_and_lastName_store@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL: Increment value}
{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax increment_age_generic@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Non-Generic-session-syntax increment_age_non_generic_session@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Store-syntax increment_age_non_generic_store@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL: Add item to array}
{CODE-TABS}
{CODE-TAB:csharp:Generic-syntax add_new_comment_to_comments_generic_session@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Non-Generic-session-syntax add_new_comment_to_comments_non_generic_session@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Store-syntax add_new_comment_to_comments_store@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL: Insert item into specific position in array}
Inserting item into specific position is supported only by the non-generic APIs
{CODE-TABS}
{CODE-TAB:csharp:Non-Generic-session-syntax insert_new_comment_at_position_1_session@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Store-syntax insert_new_comment_at_position_1_store@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL: Modify item in specific position in array}
Inserting item into specific position is supported only by the non-generic APIs
{CODE-TABS}
{CODE-TAB:csharp:Non-Generic-session-syntax modify_a_comment_at_position_3_in_comments_session@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Store-syntax modify_a_comment_at_position_3_in_comments_store@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL: Remove property}
Removing property supported only by the non-generic APIs
{CODE-TABS}
{CODE-TAB:csharp:Non-Generic-session-syntax remove_property_age_non_generic_session@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Store-syntax remove_property_age_store@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL: Remove property}
Renaming property supported only by the non-generic APIs
{CODE-TABS}
{CODE-TAB:csharp:Non-Generic-session-syntax rename_property_age_non_generic_session@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TAB:csharp:Store-syntax rename_property_age_store@ClientApi\Commands\Patches\PatchRequests.cs /}
{CODE-TABS/}
{PANEL/}

## Related articles

- [How to use **JavaScript** to **patch** your documents?](../../../client-api/commands/patches/how-to-use-javascript-to-patch-your-documents)  
