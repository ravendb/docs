# Operations: Get and Count Revisions

---

{NOTE: }

* **Get** and **Count** a document's revisions using the `GetRevisionsOperation` store operation.  
* To **Count** a document's revisions without getting them, use the 
  [GetCountFor](../../../client-api/session/revisions/counting) session method.  

* In this page:  
  * [`GetRevisionsOperation`](../../../client-api/operations/revisions/get-revisions#getrevisionsoperation)  
  * [Usage Samples](../../../client-api/operations/revisions/get-revisions#usage-samples)  

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
{CODE getAllRevisions@ClientApi/Operations/Revisions/GetRevisions.cs /}

---

#### Get and process revisions, one page at a time
{CODE getRevisionsWithPaging@ClientApi/Operations/Revisions/GetRevisions.cs /}

You can also pass the method the `ID`, `Start`, and `PageSize` arguments 
wrapped in a `Parameters` object:  
{CODE getRevisionsWithPaging_wrappedObject@ClientApi/Operations/Revisions/GetRevisions.cs /}

{PANEL/}

## Related Articles

### Operations

- [Configure Revisions](../../../client-api/operations/revisions/configure-revisions)

### Session

- [What Are Revisions](../../../client-api/session/revisions/what-are-revisions)
- [Counting Revisions](../../../client-api/session/revisions/counting)

### Server

- [Revisions](../../../server/extensions/revisions)
