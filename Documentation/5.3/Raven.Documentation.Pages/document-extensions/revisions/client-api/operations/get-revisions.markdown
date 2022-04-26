# Revisions: Getting and Counting Revisions

---

{NOTE: }

* **Get** and **Count** a document's revisions using the `GetRevisionsOperation` store operation.  
* To **Count** a document's revisions without getting them, use the 
  [GetCountFor](../../../../document-extensions/revisions/client-api/session/counting) session method.  

* In this page:  
  * [`GetRevisionsOperation`](../../../../document-extensions/revisions/client-api/operations/get-revisions#getrevisionsoperation)  
  * [Usage Samples](../../../../document-extensions/revisions/client-api/operations/get-revisions#usage-samples)  

{NOTE/}

---

{PANEL: `GetRevisionsOperation`}


#### Overloads

{CODE-BLOCK: csharp}

// Get all the revisions of the document whose ID is provided
public GetRevisionsOperation(string id);

// Start from a specified revision and Get a specified number of revisions
public GetRevisionsOperation(string id, int start, int pageSize);

// Start from a specified revision and Get a specified number of revisions
public GetRevisionsOperation(Parameters parameters)

{CODE-BLOCK/}

| Parameter | Type | Description |
| - | - | - |
| **id** | `string` | ID of the document whose revisions you want to get |
| **start** | `int` | Revision number to start from |
| **pageSize** | `int` | Number of revisions to get |
| **parameters** | `Parameters` | an object that wraps `Id`, `Start`, and `PageSize` (see below) |

{CODE-BLOCK: csharp}
public class Parameters
{
    // ID of the document whose revisions you want to get
    public string Id { get; set; }
    // Revision number to start from
    public int? Start { get; set; }
    // Number of revisions to get
    public int? PageSize { get; set; }
}
{CODE-BLOCK/}

---

#### Return Value: `RevisionsResult<T>`

{CODE-BLOCK: csharp}
public class RevisionsResult<T>
{
  // The retrieved revisions
  public List<T> Results { get; set; }

  // Total number of revisions that exist for this document
  public int TotalResults { get; set; }
}
{CODE-BLOCK/}

{PANEL/}

{PANEL: Usage Samples}

#### Get all the revisions created for a document  
{CODE getAllRevisions@DocumentExtensions\Revisions\ClientAPI\Operations\GetRevisions.cs /}

---

#### Get and process revisions, one page at a time
{CODE getRevisionsWithPaging@DocumentExtensions\Revisions\ClientAPI\Operations\GetRevisions.cs /}

You can also pass the method the `ID`, `Start`, and `PageSize` arguments 
wrapped in a `Parameters` object:  
{CODE getRevisionsWithPaging_wrappedObject@DocumentExtensions\Revisions\ClientAPI\Operations\GetRevisions.cs /}

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
