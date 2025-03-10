# Attachments: Copy, Move, Rename

Attachments can be copied, moved, or renamed using built-in session methods.  
All of those actions are executed when `saveChanges` is called and take place on the server-side,  
removing the need to transfer the entire attachment binary data over the network in order to perform the action.

{PANEL: Copy attachment}

Attachment can be copied using one of the `session.advanced.attachments.copy` methods:

### Syntax

{CODE:php copy_0@DocumentExtensions\Attachments\CopyMoveRename.php /}

### Example

{CODE:php copy_1@DocumentExtensions\Attachments\CopyMoveRename.php /}

{PANEL/}

{PANEL: Move attachment}

Attachment can be moved using one of the `session.advanced.attachments.move` methods:

### Syntax

{CODE:php move_0@DocumentExtensions\Attachments\CopyMoveRename.php /}

### Example

{CODE:php move_1@DocumentExtensions\Attachments\CopyMoveRename.php /}

{PANEL/}

{PANEL: Rename attachment}

Attachment can be renamed using one of the `session.advanced.attachments.rename` methods:

### Syntax

{CODE:php rename_0@DocumentExtensions\Attachments\CopyMoveRename.php /}

### Example

{CODE:php rename_1@DocumentExtensions\Attachments\CopyMoveRename.php /}

{PANEL/}

## Related Articles

### Attachments

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)
- [Storing](../../document-extensions/attachments/storing)
- [Loading](../../document-extensions/attachments/loading)
- [Deleting](../../document-extensions/attachments/deleting)
