# Client API: How to Use Low-Level Commands

Low-level commands are the mechanism that [Operations](../../client-api/operations/what-are-operations) are built on top of.  
When performing an operation, there is an underlying low-level command that is in charge of sending the appropriate request to the server 
(via the **Request Executor**) and parsing the server reply.

In order to use low-level commands directly, you will need to use the `execute` method of a **Request Executor**.

{CODE:java Execute@ClientApi\HowTo\UseLowLevelCommands.java /}

## Examples

### GetDocumentsCommand

{CODE:java commands_1@ClientApi\HowTo\UseLowLevelCommands.java /}

### DeleteDocumentCommand

{CODE:java commands_2@ClientApi\HowTo\UseLowLevelCommands.java /}

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
* GetSubscriptionsCommand  
* GetSubscriptionStateCommand  
* GetStatisticsCommand  
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
* StreamCommand   

## Related articles

### Commands

- [How to Send Multiple Commands Using a Batch](../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
