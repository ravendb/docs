# Get Revisions Operation

---

{NOTE: }

* Use `GetRevisionsOperation` to GET the document's revisions.

* To only COUNT the number of revisions without getting them, use the [GetCountFor](../../../../document-extensions/revisions/client-api/session/counting) session method.

* In this page:  
  * [Get all revisions](../../../../document-extensions/revisions/client-api/operations/get-revisions#get-all-revisions)  
  * [Paging results](../../../../document-extensions/revisions/client-api/operations/get-revisions#paging-results)  
  * [Syntax](../../../../document-extensions/revisions/client-api/operations/get-revisions#syntax)  

{NOTE/}

---

{PANEL: Get all revisions}

{CODE-TABS}
{CODE-TAB:csharp:Sync getAllRevisions@DocumentExtensions\Revisions\ClientAPI\Operations\GetRevisions.cs /}
{CODE-TAB:csharp:Async getAllRevisions_async@DocumentExtensions\Revisions\ClientAPI\Operations\GetRevisions.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Paging results}

* Get and process revisions, one page at a time:

{CODE-TABS}
{CODE-TAB:csharp:Sync getRevisionsWithPaging@DocumentExtensions\Revisions\ClientAPI\Operations\GetRevisions.cs /}
{CODE-TAB:csharp:Async getRevisionsWithPaging_async@DocumentExtensions\Revisions\ClientAPI\Operations\GetRevisions.cs /}
{CODE-TABS/}

* The document ID, start & page size can be wrapped in a `Parameter` object:

{CODE-TABS}
{CODE-TAB:csharp:Sync getRevisionsWithPagingParams@DocumentExtensions\Revisions\ClientAPI\Operations\GetRevisions.cs /}
{CODE-TAB:csharp:Async getRevisionsWithPagingParams_async@DocumentExtensions\Revisions\ClientAPI\Operations\GetRevisions.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

Available overloads:

{CODE-BLOCK: csharp}

// Get all revisions for the specified document:
public GetRevisionsOperation(string id);

// Page revisions:
public GetRevisionsOperation(string id, int start, int pageSize);
public GetRevisionsOperation(Parameters parameters)

{CODE-BLOCK/}

| Parameter | Type | Description |
| - | - | - |
| **id** | `string` | Document ID for which to get revisions |
| **start** | `int` | Revision number to start from |
| **pageSize** | `int` | Number of revisions to get |
| **parameters** | `Parameters` | An object that wraps `Id`, `Start`, and `PageSize` (see below) |

{CODE-BLOCK: csharp}
public class Parameters
{
    public string Id { get; set; }     // Document ID for which to get revisions
    public int? Start { get; set; }    // Revision number to start from
    public int? PageSize { get; set; } // Number of revisions to get
}
{CODE-BLOCK/}

| Return value of `store.Operations.Send(getRevisionsOp)` | |
| - | - |
| `RevisionsResult<T>` | Object with revisions results |


{CODE-BLOCK: csharp}
public class RevisionsResult<T>
{  
  public List<T> Results { get; set; }  // The retrieved revisions
  public int TotalResults { get; set; } // Total number of revisions that exist for the document
}
{CODE-BLOCK/}

{PANEL/}

## Related Articles

### Document Extensions

* [Document Revisions Overview](../../../../document-extensions/revisions/overview)  
* [Revert Revisions](../../../../document-extensions/revisions/revert-revisions)  
* [Revisions and Other Features](../../../../document-extensions/revisions/revisions-and-other-features)  

### Client API

* [Revisions: API Overview](../../../../document-extensions/revisions/client-api/overview)  
* [Operations: Configuring Revisions](../../../../document-extensions/revisions/client-api/operations/configure-revisions)  
* [Session: Loading Revisions](../../../../document-extensions/revisions/client-api/session/loading)  
* [Session: Including Revisions](../../../../document-extensions/revisions/client-api/session/including)  
* [Session: Counting Revisions](../../../../document-extensions/revisions/client-api/session/counting)  

### Studio
* [Settings: Document Revisions](../../../../studio/database/settings/document-revisions)  
* [Document Extensions: Revisions](../../../../studio/database/document-extensions/revisions)  
