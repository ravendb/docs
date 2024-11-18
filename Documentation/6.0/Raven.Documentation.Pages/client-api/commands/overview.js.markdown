# Commands Overview
---

{NOTE: }

* RavenDB's Client API is structured in layers.  
  At the highest layer, you interact with the [document store](../../client-api/what-is-a-document-store) and the [document session](../../client-api/session/what-is-a-session-and-how-does-it-work),  
  which handle most common database tasks like loading, saving, and querying documents.

* Beneath this high-level interface are Operations and Commands:

    * **Operations**:

        * Operations provide management functionality outside the session's context,  
          like creating a database, performing bulk actions, or managing server-wide configurations.

        * Learn more about Operations in [what are Operations](../../client-api/operations/what-are-operations).

    * **Commands**:

        * All high-level methods and Operations are built on top of Commands.  
          Commands form the lowest-level operations that directly communicate with the server.

        * For example, a session’s _Load_ method translates internally to a _LoadOperation_,  
          which ultimately relies on a _GetDocumentsCommand_ to fetch data from the server.

        * Commands are responsible for sending the appropriate request to the server using a `Request Executor`,  
          and parsing the server's response.

        * All commands can be executed using either the Store's _Request Executor_ or the Session's _Request Executor_,  
          regardless of whether the command is session-related or not.

* This layered structure lets you work at any level, depending on your needs.

* In this page:
    * [Execute command - using the Store](../../client-api/commands/overview#execute-command---using-the-store)
    * [Execute command - using the Session](../../client-api/commands/overview#execute-command---using-the-session)
    * [Available commands](../../client-api/commands/overview#available-commands)

{NOTE/}
---

{PANEL: Execute command - using the Store}

This example shows how to execute the low-level `CreateSubscriptionCommand` using the **Store**.  
(For examples of creating a subscription using higher-level methods, see [subscription creation examples](../../client-api/data-subscriptions/creation/examples)).

{CODE:nodejs execute_1@client-api\commands\documents\overview.js /}

{PANEL/}

{PANEL: Execute command - using the Session}

This example shows how to execute the low-level `GetDocumentsCommand` using the **Session**.  
(For loading a document using higher-level methods, see [loading entities](../../client-api/session/loading-entities)).

{CODE:nodejs execute_2@client-api\commands\documents\overview.js /}

{PANEL/}

{PANEL: Available commands}

* The Following low-level commands are available:
    * ConditionalGetDocumentsCommand
    * CreateSubscriptionCommand
    * [DeleteDocumentCommand](../../client-api/commands/documents/delete)
    * DeleteSubscriptionCommand
    * DropSubscriptionConnectionCommand
    * ExplainQueryCommand
    * GetClusterTopologyCommand
    * GetConflictsCommand
    * GetDatabaseTopologyCommand
    * [GetDocumentsCommand](../../client-api/commands/documents/get)
    * GetIdentitiesCommand
    * GetNextOperationIdCommand
    * GetNodeInfoCommand
    * GetOperationStateCommand
    * GetRawStreamResultCommand
    * GetRevisionsBinEntryCommand
    * GetRevisionsCommand
    * GetSubscriptionsCommand
    * GetSubscriptionStateCommand
    * GetTcpInfoCommand
    * GetTrafficWatchConfigurationCommand
    * HeadAttachmentCommand
    * HeadDocumentCommand
    * HiLoReturnCommand
    * IsDatabaseLoadedCommand
    * KillOperationCommand
    * MultiGetCommand
    * NextHiLoCommand
    * NextIdentityForCommand
    * [PutDocumentCommand](../../client-api/commands/documents/put)
    * PutSecretKeyCommand
    * QueryCommand
    * QueryStreamCommand
    * SeedIdentityForCommand
    * SingleNodeBatchCommand
    * WaitForRaftIndexCommand

{PANEL/}

## Related Articles

### Commands 

- [Put document command](../../../client-api/commands/documents/put)  
- [Get document command](../../../client-api/commands/documents/get)  
- [Delete document command](../../../client-api/commands/documents/delete)
- [Send multiple commands using batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
