# Operations: Get and Count Revisions

---

{NOTE: }

* **Get** and **Count** a document's revisions using the `GetRevisionsOperation` store operation.  

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

// Start from a chosen revision and divide the retrieval of revisions to pages
public GetRevisionsOperation(string id, int start, int pageSize);
{CODE-BLOCK/}

| Parameter | Type | Description |
| - | - | - |
| **id** | `string` | ID of the document whose revisions you want to get |
| **start** | `int` | The number of revisions to skip |
| **pageSize** | `int` | Maximum number of revisions to get in each page |

---

#### Return Value: `RevisionsResult<T>`

{CODE-BLOCK: csharp}
public class RevisionsResult<T>
{
  // a list of retrieved revisions
  public List<T> Results { get; set; }

  // The number of retrieved revisions
  public int TotalResults { get; set; }
}
{CODE-BLOCK/}

{PANEL/}

{PANEL: Usage Samples}

#### Get all the revisions created for a document  
{CODE getAllRevisions@ClientApi/Operations/Revisions/GetRevisions.cs /}

---

#### Skip the first 50 revisions and then use Paging to get 10 revisions at a time  
{CODE getRevisionsWithPaging@ClientApi/Operations/Revisions/GetRevisions.cs /}


{PANEL/}

## Related Articles

### Operations

- [Configure Revisions](../../../client-api/operations/revisions/configure-revisions)

### Session

- [What Are Revisions](../../../client-api/session/revisions/what-are-revisions)

### Server

- [Revisions](../../../server/extensions/revisions)
