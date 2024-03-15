# Attachments: Copy, Move, Rename

Attachments can be copied, moved, or renamed using built-in session methods.  
All of those actions are executed when `SaveChanges` is called and take place on the server-side,  
removing the need to transfer the entire attachment binary data over the network in order to perform the action.

{PANEL: Copy attachment}

Attachment can be copied using one of the `session.Advanced.Attachments.Copy` methods:

### Syntax

{CODE copy_0@DocumentExtensions\Attachments\CopyMoveRename.cs /}

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync copy_1@DocumentExtensions\Attachments\CopyMoveRename.cs /}
{CODE-TAB:csharp:Async copy_2@DocumentExtensions\Attachments\CopyMoveRename.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Move attachment}

Attachment can be moved using one of the `session.Advanced.Attachments.Move` methods:

### Syntax

{CODE move_0@DocumentExtensions\Attachments\CopyMoveRename.cs /}

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync move_1@DocumentExtensions\Attachments\CopyMoveRename.cs /}
{CODE-TAB:csharp:Async move_2@DocumentExtensions\Attachments\CopyMoveRename.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Rename attachment}

Attachment can be renamed using one of the `session.Advanced.Attachments.Rename` methods:

### Syntax

{CODE rename_0@DocumentExtensions\Attachments\CopyMoveRename.cs /}

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync rename_1@DocumentExtensions\Attachments\CopyMoveRename.cs /}
{CODE-TAB:csharp:Async rename_2@DocumentExtensions\Attachments\CopyMoveRename.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Attachments

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)
- [Storing](../../document-extensions/attachments/storing)
- [Loading](../../document-extensions/attachments/loading)
- [Deleting](../../document-extensions/attachments/deleting)
