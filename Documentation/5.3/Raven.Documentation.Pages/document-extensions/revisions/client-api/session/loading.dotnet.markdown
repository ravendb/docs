# Revisions: Loading Revisions

There are a few methods that allow you to download revisions from a database:  

- **session.Advanced.Revisions.GetFor**  
    - Retrieve all the revisions that exist for a specified document  
- **session.Advanced.Revisions.GetMetadataFor** 
    - Retrieve the metadata for all the revisions that exist for a specified document  
- **session.Advanced.Revisions.Get**
    - Retrieve one or multiple revisions by their change vectors  
    - Retrieve a revision by its creation time  

{PANEL:GetFor}

### Syntax

{CODE syntax_1@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | document ID for which the revisions will be returned for |
| **start** | int | used for paging |
| **pageSize** | int | used for paging |

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync example_1_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_1_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:GetMetadataFor}

### Syntax

{CODE syntax_2@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | document ID for which the revisions will be returned for |
| **start** | int | used for paging |
| **pageSize** | int | used for paging |

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync example_2_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_2_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:Get}

### Syntax

{CODE syntax_3@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **changeVector** | `string` | revision change vectors |
| **changeVectors**| `IEnumerable<string>` | revisions change vectors |
| **date** or **changeVectors**| `DateTime ` | revision creation time |

### Example I
Get a revision by its change vector:  

{CODE-TABS}
{CODE-TAB:csharp:Sync example_3.1_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_3.1_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

### Example II
Get revisions metadata, extract a revision's change vector from the metadata, 
and get the revision by the change vector:  

{CODE-TABS}
{CODE-TAB:csharp:Sync example_3.2_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_3.2_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

### Example III
Get a revision by its creation time:  

{CODE-TABS}
{CODE-TAB:csharp:Sync example_3.3_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_3.3_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Revisions

- [What are Revisions](../../../client-api/session/revisions/what-are-revisions)
- [Revisions in Data Subscriptions](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning)

### Server

- [Revisions](../../../server/extensions/revisions)
