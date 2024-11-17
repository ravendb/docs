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
     Learn more about Operations in [What are Operations](../../client-api/operations/what-are-operations).
   * **Commands**:  
     Commands are the lowest-level operations that communicate directly with the server.  
     For example, a session’s _Load_ method internally translates to a _LoadOperation_, which eventually relies on a _GetDocumentCommand_ to fetch data from the server.
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

{CODE-TABS}
{CODE-TAB:csharp:Execute_command execute_1@ClientApi\Commands\Documents\Overview.cs /}
{CODE-TAB:csharp:Execute_command_async execute_1_async@ClientApi\Commands\Documents\Overview.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Execute command - using the Session context}

This example shows executing a command using the **session context**.  

{CODE-TABS}
{CODE-TAB:csharp:Execute_command execute_2@ClientApi\Commands\Documents\Overview.cs /}
{CODE-TAB:csharp:Execute_command_async execute_2_async@ClientApi\Commands\Documents\Overview.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Commands 

- [Put document command](../../../client-api/commands/documents/put)  
- [Get document command](../../../client-api/commands/documents/get)  
- [Delete document command](../../../client-api/commands/documents/delete)
- [Send multiple commands using batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
