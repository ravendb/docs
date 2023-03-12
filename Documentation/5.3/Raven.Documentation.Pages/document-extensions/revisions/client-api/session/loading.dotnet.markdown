# Get Revisions

---

{NOTE: }

* Using the Advanced Session methods you can __retrieve revisions and their metadata__  
  from the database for the specified document:  

* These methods can also be executed lazily, see [get revisions lazily](../../../../client-api/session/how-to/perform-operations-lazily#getRevisons).   
  
* In this page:  
  
  * [Get all revisions](../../../../document-extensions/revisions/client-api/session/loading#get-all-revisions)  

  * [Get revisions metadata](../../../../document-extensions/revisions/client-api/session/loading#get-revisions-metadata)  

  * [Get revisions by change vector or by creation time](../../../../document-extensions/revisions/client-api/session/loading#get-revisions-by-change-vector-or-by-creation-time)  


{NOTE/}

---

{PANEL: Get all revisions}

* Use `GetFor` to retrieve all of the revisions currently kept for the specified document.

---

__Example__:

{CODE-TABS}
{CODE-TAB:csharp:Sync example_1_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_1_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

__Syntax__:

{CODE syntax_1@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}

| Parameters | Type | Description |
| - | - |- |
| **id** | `string` | Document ID for which to retrieve revisions |
| **start** | `int` | First revision to retrieve |
| **pageSize** | `int` | Number of revisions to retrieve per page |

{PANEL/}

{PANEL: Get revisions metadata}

* Use `GetMetadataFor` to retrieve the metadata for all the revisions currently kept for the specified document.

---

__Example__:

{CODE-TABS}
{CODE-TAB:csharp:Sync example_2_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_2_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

__Syntax__:

{CODE syntax_2@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}

| Parameters | Type | Description |
| - | - |- |
| **id** | `string` | Document ID for which to retrieve revisions' metadata |
| **start** | `int` | First revision to retrieve metadata for |
| **pageSize** | `int` | Number of revisions to retrieve per page |

{PANEL/}

{PANEL: Get revisions by change vector or by creation time}

* Use `Get` to:  
  * Retrieve a revision or multiple revisions by their **change vectors**  
  * Retrieve a revision by its **creation time**  

---

__By change vector__:

{CODE-TABS}
{CODE-TAB:csharp:Sync example_3.1_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_3.1_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

__By creation time__:  

{CODE-TABS}
{CODE-TAB:csharp:Sync example_3.2_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_3.2_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

---

__Syntax__:

{CODE syntax_3@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}

| Parameter | Type | Description |
| - | - | - |
| **changeVector** | `string` | A revision's change vector |
| **changeVectors**| `IEnumerable<string>` | revisions' change vectors |
| **date**| `DateTime ` | A revision's creation time |
| **id** | `string` | Document ID for which to retrieve the revision by creation time |

{PANEL/}

## Related Articles

### Document Extensions

* [Document Revisions Overview](../../../../document-extensions/revisions/overview)  
* [Revert Revisions](../../../../document-extensions/revisions/revert-revisions)  
* [Revisions and Other Features](../../../../document-extensions/revisions/revisions-and-other-features)  

### Client API

* [Revisions: API Overview](../../../../document-extensions/revisions/client-api/overview)  
* [Operations: Configuring Revisions](../../../../document-extensions/revisions/client-api/operations/configure-revisions)  
* [Session: Including Revisions](../../../../document-extensions/revisions/client-api/session/including)  
* [Session: Counting Revisions](../../../../document-extensions/revisions/client-api/session/counting)  

### Studio

* [Settings: Document Revisions](../../../../studio/database/settings/document-revisions)  
* [Document Extensions: Revisions](../../../../studio/database/document-extensions/revisions)  
