# Get Revisions

---

{NOTE: }

* Using the Advanced Session methods you can **retrieve revisions and their metadata**  
  from the database for the specified document.  

* These methods can also be executed lazily, see [get revisions lazily](../../../../client-api/session/how-to/perform-operations-lazily#getRevisions).   
  
* In this page:  
  
  * [Get all revisions](../../../../document-extensions/revisions/client-api/session/loading#get-all-revisions)  

  * [Get revisions metadata](../../../../document-extensions/revisions/client-api/session/loading#get-revisions-metadata)  

  * [Get revisions by creation time](../../../../document-extensions/revisions/client-api/session/loading#get-revisions-by-creation-time)  
  
  * [Get revisions by change vector](../../../../document-extensions/revisions/client-api/session/loading#get-revisions-by-change-vector)  


{NOTE/}

---

{PANEL: Get all revisions}

* Use `GetFor` to retrieve all of the revisions currently kept for the specified document.

---

**Example**:

{CODE-TABS}
{CODE-TAB:csharp:Sync example_1_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_1_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

**Syntax**:

{CODE syntax_1@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}

| Parameters | Type | Description |
| - | - |- |
| **id** | `string` | Document ID for which to retrieve revisions |
| **start** | `int` | First revision to retrieve, used for paging |
| **pageSize** | `int` | Number of revisions to retrieve per results page |

{PANEL/}

{PANEL: Get revisions metadata}

* Use `GetMetadataFor` to retrieve the metadata for all the revisions currently kept for the specified document.

---

**Example**:

{CODE-TABS}
{CODE-TAB:csharp:Sync example_2_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_2_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

**Syntax**:

{CODE syntax_2@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}

| Parameters | Type | Description |
| - | - |- |
| **id** | `string` | Document ID for which to retrieve revisions' metadata |
| **start** | `int` | First revision to retrieve metadata for, used for paging |
| **pageSize** | `int` | Number of revisions to retrieve per results page |

{PANEL/}

{PANEL: Get revisions by creation time}

* Use `Get` to retrieve a revision by its **creation time**.

---

**Example**:

{CODE-TABS}
{CODE-TAB:csharp:Sync example_3_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_3_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

---

**Syntax**:

{CODE syntax_3@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}

| Parameter | Type | Description |
| - | - | - |
| **id** | `string` | Document ID for which to retrieve the revision by creation time |
| **date** | `DateTime ` | The revision's creation time |

{PANEL/}

{PANEL: Get revisions by change vector}

* Use `Get` to retrieve a revision or multiple revisions by their **change vectors**.  

---

**Example**:

{CODE-TABS}
{CODE-TAB:csharp:Sync example_4_sync@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TAB:csharp:Async example_4_async@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}
{CODE-TABS/}

**Syntax**:

{CODE syntax_4@DocumentExtensions\Revisions\ClientAPI\Session\Loading.cs /}

| Parameter | Type | Description |
| - | - | - |
| **changeVector** | `string` | The revision's change vector |
| **changeVectors** | `IEnumerable<string>` | Change vectors of multiple revisions |

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
