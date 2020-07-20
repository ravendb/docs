# Attachments: Copying, Moving & Renaming

Attachments can be copied, moved or renamed using appropriate built-in session methods. All of those actions are executed when `saveChanges` is called and are taking place on the server-side, removing the need to transfer whole attachment binary data over the network in order to perform the action.

{PANEL:Copying}

Attachment can be copied using one of the `session.advanced().attachments().copy` methods:

### Syntax

{CODE:java copy_0@DocumentExtensions\Attachments\CopyMoveRename.java /}

### Example

{CODE:java copy_1@DocumentExtensions\Attachments\CopyMoveRename.java /}

{PANEL/}

{PANEL:Moving}

Attachment can be moved using one of the `session.advanced().attachments().move` methods:

### Syntax

{CODE:java move_0@DocumentExtensions\Attachments\CopyMoveRename.java /}

### Example

{CODE:java move_1@DocumentExtensions\Attachments\CopyMoveRename.java /}

{PANEL/}

{PANEL:Renaming}

Attachment can be renamed using one of the `session.advanced().attachments().rename` methods:

### Syntax

{CODE:java rename_0@DocumentExtensions\Attachments\CopyMoveRename.java /}

### Example

{CODE:java rename_1@DocumentExtensions\Attachments\CopyMoveRename.java /}

{PANEL/}

## Related Articles

### Attachments

- [What are Attachments](../../../client-api/session/attachments/what-are-attachments)
- [Storing](../../../client-api/session/attachments/storing)
- [Loading](../../../client-api/session/attachments/loading)
- [Deleting](../../../client-api/session/attachments/deleting)
