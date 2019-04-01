# Client API: How to Use Low-Level Commands

Low-level commands are the mechanism that [Operations](../../client-api/operations/what-are-operations) are built on top of.  
When performing an operation, there is an underlying low-level command that is in charge of sending the appropriate request to the server 
(via the **Request Executor**) and parsing the server reply.

In order to use low-level commands directly, you will need to use the `Execute` or `ExectueAsync` method of a **Request Executor**.

{CODE-TABS}
{CODE-TAB:csharp:Execute Execute@ClientApi\HowTo\UseLowLevelCommands.cs /}
{CODE-TAB:csharp:ExecuteAsync Execute_async@ClientApi\HowTo\UseLowLevelCommands.cs /}
{CODE-TABS/}

## Examples

### GetDocumentsCommand

{CODE-TABS}
{CODE-TAB:csharp:Sync commands_1@ClientApi\HowTo\UseLowLevelCommands.cs /}
{CODE-TAB:csharp:Async commands_1_async@ClientApi\HowTo\UseLowLevelCommands.cs /}
{CODE-TABS/}

### DeleteDocumentCommand

{CODE-TABS}
{CODE-TAB:csharp:Sync commands_2@ClientApi\HowTo\UseLowLevelCommands.cs /}
{CODE-TAB:csharp:Async commands_2_async@ClientApi\HowTo\UseLowLevelCommands.cs /}
{CODE-TABS/}

## The Following Low-Level Commands are Available:

* BatchCommand  
* CreateSubscriptionCommand  
* DeleteDocumentCommand   
* DeleteSubscriptionCommand  
* DropSubscriptionConnectionCommand  
* ExplainQueryCommand  
* GetConflictsCommand  
* GetDocumentsCommand  
* GetIdentitiesCommand
* GetNextOperationIdCommand  
* GetOperationStateCommand  
* GetRevisionsBinEntryCommand  
* GetRevisionsCommand  
* GetStatisticsCommand  
* GetSubscriptionsCommand  
* GetSubscriptionStateCommand  
* HeadAttachmentCommand
* HeadDocumentCommand  
* HiLoReturnCommand  
* KillOperationCommand  
* NextHiLoCommand   
* NextIdentityForCommand 
* PatchCommand
* PutDocumentCommand   
* QueryCommand   
* QueryStreamCommand   
* SeedIdentityForCommand   
* StartBackupCommand   
* StreamCommand   

## Related articles

### Commands

- [How to Send Multiple Commands Using a Batch](../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
