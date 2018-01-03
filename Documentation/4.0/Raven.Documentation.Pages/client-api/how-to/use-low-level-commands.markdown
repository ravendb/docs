#How to use low-level commands?

Low-level commands are the mechanism that [Operations](../../client-api/operations/what-are-operations) are bult on top of.  
When preforming an operation, there is an underlying low-level command that is in charge of sending the appropriate request to the server 
(via the **Request Executor**) and parsing the server reply.

In order to use low-level commands directly, you will need to use the `Execute` or `ExectueAsync` method of a  **Request Executor**.

{CODE-TABS}
{CODE-TAB:csharp:Execute Execute@ClientApi\HowTo\UseLowLevelCommands.cs /}
{CODE-TAB:csharp:ExecuteAsync Execute_async@ClientApi\HowTo\UseLowLevelCommands.cs /}
{CODE-TABS/}

### Examples

{PANEL:GetDocumentsCommand}
{CODE-TABS}
{CODE-TAB:csharp:Sync commands_1@ClientApi\HowTo\UseLowLevelCommands.cs /}
{CODE-TAB:csharp:Async commands_1_async@ClientApi\HowTo\UseLowLevelCommands.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL:DeleteDocumentCommand}
{CODE-TABS}
{CODE-TAB:csharp:Sync commands_2@ClientApi\HowTo\UseLowLevelCommands.cs /}
{CODE-TAB:csharp:Async commands_2_async@ClientApi\HowTo\UseLowLevelCommands.cs /}
{CODE-TABS/}
{PANEL/}


### The following low-level commands are available:

#### Documents
* [GetDocumentsCommand](../../client-api/commands/documents/get)   
* [PutDocumentCommand](../../client-api/commands/documents/put)    
* [DeleteDocumentCommand](../../client-api/commands/documents/delete)    

