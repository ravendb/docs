# Revisions: Loading Revisions

There are a few methods that allow you to download revisions from a database:   

- **session.Advanced.Revisions.GetFor** 
    - can be used to return all previous revisions for a specified document   
- **session.Advanced.Revisions.GetMetadataFor**
    - can be used to return metadata of all previous revisions for a specified document  
- **session.Advanced.Revisions.Get**
    - can be used to retrieve a revision(s) using a change vector(s)  

{PANEL:GetFor}

### Syntax

{CODE syntax_1@ClientApi\Session\Revisions\Loading.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | document ID for which the revisions will be returned for |
| **start** | int | used for paging |
| **pageSize** | int | used for paging |

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync example_1_sync@ClientApi\Session\Revisions\Loading.cs /}
{CODE-TAB:csharp:Async example_1_async@ClientApi\Session\Revisions\Loading.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:GetMetadataFor}

### Syntax

{CODE syntax_2@ClientApi\Session\Revisions\Loading.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | document ID for which the revisions will be returned for |
| **start** | int | used for paging |
| **pageSize** | int | used for paging |

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync example_2_sync@ClientApi\Session\Revisions\Loading.cs /}
{CODE-TAB:csharp:Async example_2_async@ClientApi\Session\Revisions\Loading.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:Get}

### Syntax

{CODE syntax_3@ClientApi\Session\Revisions\Loading.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **changeVector** or **changeVectors**| string or `IEnumerable<string>` | one or many revision change vectors |

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync example_3_sync@ClientApi\Session\Revisions\Loading.cs /}
{CODE-TAB:csharp:Async example_3_async@ClientApi\Session\Revisions\Loading.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Revisions

- [What are Revisions](../../../client-api/session/revisions/what-are-revisions)
- [Revisions in Data Subscriptions](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning)

### Server

- [Revisions](../../../server/extensions/revisions)
