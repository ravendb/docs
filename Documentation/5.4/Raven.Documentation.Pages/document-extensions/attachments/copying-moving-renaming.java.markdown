# Attachments: Copy, Move, Rename

Attachments can be copied, moved, or renamed using built-in session methods.  
All of those actions are executed when `saveChanges` is called and take place on the server-side,  
removing the need to transfer the entire attachment binary data over the network in order to perform the action.

{PANEL: Copy attachment}

Attachment can be copied using one of the `session.advanced().attachments().copy` methods:

### Syntax

{CODE:java copy_0@DocumentExtensions\Attachments\CopyMoveRename.java /}

### Example

{CODE:java copy_1@DocumentExtensions\Attachments\CopyMoveRename.java /}

{PANEL/}

{PANEL: Move attachment}

Attachment can be moved using one of the `session.advanced().attachments().move` methods:

### Syntax

{CODE:java move_0@DocumentExtensions\Attachments\CopyMoveRename.java /}

### Example

{CODE:java move_1@DocumentExtensions\Attachments\CopyMoveRename.java /}

{PANEL/}

{PANEL: Rename attachment}

Attachment can be renamed using one of the `session.advanced().attachments().rename` methods:

### Syntax

{CODE:java rename_0@DocumentExtensions\Attachments\CopyMoveRename.java /}

### Example

{CODE:java rename_1@DocumentExtensions\Attachments\CopyMoveRename.java /}

{PANEL/}

## Related Articles

### Attachments

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)  
- [Storing](../../document-extensions/attachments/storing)  
- [Loading](../../document-extensions/attachments/loading)  
- [Deleting](../../document-extensions/attachments/deleting)  
