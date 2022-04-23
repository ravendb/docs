# Revisions: Loading Revisions

---

{NOTE: }

You can retrieve revisions and their metadata from the database using these Session methods:  

* `session.Advanced.Revisions.GetFor`  
  Retrieves all the revisions currently kept for a specified document  
* `session.Advanced.Revisions.GetMetadataFor`  
  Retrieves the metadata for all the revisions currently kept for a specified document  
* `session.Advanced.Revisions.Get`  
  Retrieves a revision or multiple revisions by their change vectors  
  Retrieves a revision by its creation time  

* In this page:  
  * [`GetFor`](../../../../document-extensions/revisions/client-api/session/loading#getfor)  
  * [`GetMetadataFor`](../../../../document-extensions/revisions/client-api/session/loading#getmetadatafor)  
  * [`Get`](../../../../document-extensions/revisions/client-api/session/loading#get)  

{NOTE/}

---

{PANEL: `GetFor`}

Use `GetFor` to retrieve all of the revisions currently kept for a specified document.  

### Syntax

{CODE syntax_1@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | ID of the document whose revisions are retrieved |
| **start** | int | First revision to retrieve |
| **pageSize** | int | How many revisions to retrieve per page |

#### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync example_1_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_1_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: `GetMetadataFor`}

Use `GetMetadataFor` to retrieve the metadata for all the revisions currently kept 
for a specified document.  

### Syntax

{CODE syntax_2@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | ID of the document whose revisions' metadata is retrieved |
| **start** | int | First revision to retrieve metadata for |
| **pageSize** | int | The metadata for how many revisions to retrieve per page |

#### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync example_2_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_2_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: `Get`}

Use `Get` to -  

* Retrieve a revision or multiple revisions by their **change vectors**  
* Retrieve a revision by its **creation time**  

### Syntax

{CODE syntax_3@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}

| Parameter | Type | Description |
| ------------- | ------------- | ----- |
| **changeVector** | `string` | a revision's change vectors |
| **changeVectors**| `IEnumerable<string>` | revisions' change vectors |
| **date**| `DateTime ` | a revision's creation time |

#### Example I
Get a revision by its change vector  

{CODE-TABS}
{CODE-TAB:csharp:Sync example_3.1_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_3.1_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

#### Example II
Get the metadata kept for a document's revisions, use it to find a revision's 
change vector, and retrieve the revision using the change vector.  

{CODE-TABS}
{CODE-TAB:csharp:Sync example_3.2_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_3.2_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

#### Example III
Get a revision by its creation time  

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
