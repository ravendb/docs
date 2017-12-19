# How to use operations?

The RavenDB client API is built with the notion of layers. At the top,
and what you will usually interact with, are the **[DocumentStore](../../client-api/what-is-a-document-store)** and the **[DocumentSession](../../client-api/session/what-is-a-session-and-how-does-it-work)**.
They, in turn, are built on top of the notion of Operations and Commands.

Operations are an encapsulation of a set of low level commands which are used to manipulate data, execute administrative tasks and change configuration on a server.  
They are available in DocumentStore under **Operations**, **Maintenance** and **Maintenance.Server** properties.

### When to use an operation?
Occasions to use an Operation directly usually arise when you are writing some
sort of management code (creating and deleting databases, assigning permissions, changing configuration, management of indexes, etc)

In other cases, you can use an Operation to run something that does not make
sense in the context of a session.  
For example, let's say we want to delete all
all of the People documents from the server where Name == Bob and Age >= 29.  
We could do it with the following code:
{CODE Client_Operations_0@ClientApi\HowTo\UseOperations.cs /}

###Using Common Operations

Common operations include set based operations (Patch, Delete-by-query), distributed compare-exchange operation, and management of attachments operations.

{PANEL:Operations.Send}
In order to excecute an Operation, you will need to use the `Send` or `SendAsync` methods. Avaliable overloads are:
{CODE-TABS}
{CODE-TAB:csharp:Sync Client_Operations_api@ClientApi\HowTo\UseOperations.cs /}
{CODE-TAB:csharp:Async Client_Operations_api_async@ClientApi\HowTo\UseOperations.cs /}
{CODE-TABS/}
{PANEL/}

####Example I - Get Attachment
{CODE-TABS}
{CODE-TAB:csharp:Sync Client_Operations_1@ClientApi\HowTo\UseOperations.cs /}
{CODE-TAB:csharp:Async Client_Operations_1_async@ClientApi\HowTo\UseOperations.cs /}
{CODE-TABS/}

####Example II - Delete Attachment
{CODE-TABS}
{CODE-TAB:csharp:Sync Client_Operations_2@ClientApi\HowTo\UseOperations.cs /}
{CODE-TAB:csharp:Async Client_Operations_2_async@ClientApi\HowTo\UseOperations.cs /}
{CODE-TABS/}

### Using Maintenance Operations

Maintenance operations include operations for changing configuration at runtime and for management of index operations.

{PANEL:Maintenance.Send}
{CODE-TABS}
{CODE-TAB:csharp:Sync Maintenance_Operations_api@ClientApi\HowTo\UseOperations.cs /}
{CODE-TAB:csharp:Async Maintenance_Operations_api_async@ClientApi\HowTo\UseOperations.cs /}
{CODE-TABS/}
{PANEL/}

####Example III - Get Indexes

{CODE-TABS}
{CODE-TAB:csharp:Sync Maintenance_Operations_1@ClientApi\HowTo\UseOperations.cs /}
{CODE-TAB:csharp:Async Maintenance_Operations_1_async@ClientApi\HowTo\UseOperations.cs /}
{CODE-TABS/}

####Example IV - Stop Index
{CODE-TABS}
{CODE-TAB:csharp:Sync Maintenance_Operations_2@ClientApi\HowTo\UseOperations.cs /}
{CODE-TAB:csharp:Async Maintenance_Operations_2_async@ClientApi\HowTo\UseOperations.cs /}
{CODE-TABS/}

{NOTE By default, operations available directly in store are working on a default database that was setup for that store. To switch operations to a different database that is available on that server use **[ForDatabase](../../client-api/operations/how-to/switch-operations-to-a-different-database)** method. /}

###Using Server Operations

This type of operations contain various administrative and miscellaneous configuration operations.

{PANEL:Maintenance.Server.Send}
{CODE-TABS}
{CODE-TAB:csharp:Sync Server_Operations_api@ClientApi\HowTo\UseOperations.cs /}
{CODE-TAB:csharp:Async Server_Operations_api_async@ClientApi\HowTo\UseOperations.cs /}
{CODE-TABS/}
{PANEL/}

####Example V - Create Database 
{CODE-TABS}
{CODE-TAB:csharp:Sync Server_Operations_2@ClientApi\HowTo\UseOperations.cs /}
{CODE-TAB:csharp:Async Server_Operations_2_async@ClientApi\HowTo\UseOperations.cs /}
{CODE-TABS/}

####Example VI - Get Build Number
{CODE-TABS}
{CODE-TAB:csharp:Sync Server_Operations_1@ClientApi\HowTo\UseOperations.cs /}
{CODE-TAB:csharp:Async Server_Operations_1_async@ClientApi\HowTo\UseOperations.cs /}
{CODE-TABS/}

###More Information
You can read more about operations [here](../../client-api/operations/what-are-operations)
