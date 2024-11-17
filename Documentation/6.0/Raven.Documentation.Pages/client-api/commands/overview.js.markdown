# Commands Overview
---

{NOTE: }

* RavenDB's Client API is structured in layers.  
  At the highest layer, you interact with the [document store](../../client-api/what-is-a-document-store) and the [document session](../../client-api/session/what-is-a-session-and-how-does-it-work),  
  which handle most common database tasks like loading, saving, and querying documents.
* Beneath this high-level interface are Operations and Commands:
   * **Operations**:  
     Operations provide management functionality outside the session's context,  
     like creating a database, performing bulk actions, or managing server-wide configurations.  
     Learn more about Operations in [what are Operations](../../client-api/operations/what-are-operations).
   * **Commands**:  
     Commands are the lowest-level operations that communicate directly with the server.  
     For example, a session’s _load_ method internally translates to a _loadOperation_, which eventually relies on a _getDocumentCommand_ to fetch data from the server.
* This layered structure lets you work at any level, depending on your needs.

* In this page:
    * [Execute command - using the Store context](../../../client-api/commands/documents/get#using-the-store-context)  
    * [Execute command - using the Session context](../../../client-api/commands/documents/get#using-the-session-context)  

{NOTE/}

---

{PANEL: Execute command - using the Store context}

This example shows executing a command using the **store context**.  
The store context is suitable for actions that are outside the scope of a specific session,  
e.g. creating a subscription task.

{CODE:nodejs execute_1@client-api\commands\documents\overview.js /}

{PANEL/}

{PANEL: Execute command - using the Session context}

This example shows executing a command using the **session context**.  

{CODE:nodejs execute_2@client-api\commands\documents\overview.js /}

{PANEL/}

## Related Articles

### Commands 

- [Put document command](../../../client-api/commands/documents/put)  
- [Get document command](../../../client-api/commands/documents/get)  
- [Delete document command](../../../client-api/commands/documents/delete)
- [Send multiple commands using batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
