# Attachments: Copying, Moving & Renaming

Attachments can be copied, moved or renamed using appropriate built-in session methods. All of those actions are executed when `SaveChanges` is called and are taking place on the server-side, removing the need to transfer whole attachment binary data over the network in order to perform the action.

{PANEL:Copying}

Attachment can be copied using one of the `session.Advanced.Attachments.Copy` methods:

### Syntax

{CODE copy_0@ClientApi\Session\Attachments\CopyMoveRename.cs /}

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync copy_1@ClientApi\Session\Attachments\CopyMoveRename.cs /}
{CODE-TAB:csharp:Async copy_2@ClientApi\Session\Attachments\CopyMoveRename.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:Moving}

Attachment can be moved using one of the `session.Advanced.Attachments.Move` methods:

### Syntax

{CODE move_0@ClientApi\Session\Attachments\CopyMoveRename.cs /}

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync move_1@ClientApi\Session\Attachments\CopyMoveRename.cs /}
{CODE-TAB:csharp:Async move_2@ClientApi\Session\Attachments\CopyMoveRename.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:Renaming}

Attachment can be renamed using one of the `session.Advanced.Attachments.Rename` methods:

### Syntax

{CODE rename_0@ClientApi\Session\Attachments\CopyMoveRename.cs /}

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync rename_1@ClientApi\Session\Attachments\CopyMoveRename.cs /}
{CODE-TAB:csharp:Async rename_2@ClientApi\Session\Attachments\CopyMoveRename.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Attachments

- [What are Attachments](../../../client-api/session/attachments/what-are-attachments)
- [Storing](../../../client-api/session/attachments/storing)
- [Loading](../../../client-api/session/attachments/loading)
- [Deleting](../../../client-api/session/attachments/deleting)
