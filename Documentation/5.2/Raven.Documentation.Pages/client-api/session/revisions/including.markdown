# Revisions: Including Revisions
---

{NOTE: }

* A document's **Revisions** can be 
  [Included](../../../client-api/how-to/handle-document-relationships#includes) 
  with the document.  
  The included revisions are retrieved when the document is loaded, without 
  requiring additional trips to the server.  
* Revisions can be Included when documents are -  
   * Loaded using `session.Load`  
   * Queried using `session.Query`  
   * Queried using `session.Advanced.RawQuery`  
* You can include revisions by their **Creation Date** or by the document's **Change Vector**.  

- [Including Revisions](../../../client-api/session/revisions/including#including-revisions)  
- [Syntax](../../../client-api/session/revisions/including#syntax)  
- [Including Revisions With `session.Load`](../../../client-api/session/revisions/including#including-revisions-with-session.load)  
   - [By Date](../../../client-api/session/revisions/including#load-include-revisions-by-date)  
   - [By Change Vector](../../../client-api/session/revisions/including#load-include-revisions-by-change-vector)  
- [Including Revisions With `session.Query`](../../../client-api/session/revisions/including#including-revisions-with-session.query)  
   - [By Date](../../../client-api/session/revisions/including#query-include-revisions-by-date)  
   - [By Change Vector](../../../client-api/session/revisions/including#query-include-revisions-by-change-vector)  
- [Including Revisions with `session.Advanced.RawQuery`](../../../client-api/session/revisions/including#including-revisions-with-session.advanced.rawquery)  
   - [By Date](../../../client-api/session/revisions/including#raw-query-include-revisions-by-date)  
   - [By Change Vector](../../../client-api/session/revisions/including#raw-query-include-revisions-by-change-vector)  

{NOTE/}

---

{PANEL: Including Revisions}

When it is known prior to the retrieval of a document that its revisions may 
be needed, the revisions can be **Included** so they'd be loaded along with the 
document without requiring additional trips to the server.  

When the document is loaded, [Loading](../../../client-api/session/revisions/loading) 
any of its included revisions would retrieve this revision from memory rather than 
from the server.  

This may be useful when, for example, a document that contains financial data 
is loaded by an auditing application. The document's past revisions can be Included 
with the document, to make the document's history available for instant inspection.  

---

#### Including Revisions By Date

To include a revision by its **creation date**, we simply pass a `DateTime` 
parameter with the relevant date to the relevant method 
([session.Load](../../../client-api/session/revisions/including#including-revisions-with-session.load), 
[session.Query](../../../client-api/session/revisions/including#including-revisions-with-session.query), 
or [session.Advanced.RawQuery](../../../client-api/session/revisions/including#including-revisions-with-session.advanced.rawquery)).  
The included revision is the closest revision to precede the given date.  

---

#### Including Revisions By Change Vector

When a document is updated, its [change vector](../../../server/clustering/replication/change-vector) 
is updated as well. If the Revisions feature is enabled, the update will also be reflected by the 
creation of a new document revision.  
We can include a revision **by the change vector of the document it contains**.  

To include a revision by its document's change vector, we do **not** provide the change vector directly.  
Instead, we provide a path to a **property** of the document whose revision it is, i.e. the document that 
we intend to load, that contains the wanted change vector.  

Keeping the change vectors of wanted revisions within properties of the documents they relate to, 
keeps them in context and clarifies their role.  
{NOTE: }
We can, for example, add change vectors that indicate changes in an employee's
payroll, to a "SalaryUpdates" property of the employee's payroll document.  
Each time we need to re-evaluate the employee's salary, we'd be able to instantly 
locate past salary revisions by the change vectors we kept, and load these revisions 
with the document to help our evaluation.  
{NOTE/}

{PANEL/}

{PANEL: Syntax}

Revisions can be included with documents retrieved via `session.Load` 
and `session.Query`, using one of the `IncludeRevisions` methods.  

{CODE IncludeRevisions_1_IncludeRevisions@ClientApi\Session\Revisions\Including.cs /}

| Parameters | Type | Description |
| ------------- | --------------------------- | ----- |
| **path** | `Expression<Func<T, string>>` | A path to a document property that contains **a single change vector**. <br> The revision whose change vector is contained in the document property will be Included. |
| **path** | `Expression<Func<T, IEnumerable<string>>>` | A path to a document property that contains **an array of change vectors**. <br> The revisions whose change vectors are contained in the array will be Included. |
| **before** | `DateTime` | The revision that precedes the given date will be included. |

{PANEL/}


{PANEL: Including Revisions With `session.Load`}

#### Load: Include Revisions by Date

To include a revision by its **creation date**, pass `IncludeRevisions` the date.  
The revision that immediately precedes the given date will be included.  

* **Sample**:  
  {CODE IncludeRevisions_2_LoadByDate@ClientApi\Session\Revisions\Including.cs /}

---

#### Load: Include Revisions by Change Vector

* To include a revision by a document's change vector:  
  Pass `IncludeRevisions` the path to a property of the document you load, 
  that contains the change vector.  

* To include **a Group of revisions** by their change vectors:  
  Pass `IncludeRevisions` the path to a property of the document you load, 
  that contains an array of change vectors.  

* **Example for a user defined class with properties for change vectors**:  
  {CODE IncludeRevisions_3_UserDefinition@ClientApi\Session\Revisions\Including.cs /}
* **Sample**:  
  {CODE IncludeRevisions_4_LoadByChangeVector@ClientApi\Session\Revisions\Including.cs /}

{PANEL/}

{PANEL: Including Revisions With `session.Query`}

Including revisions for `session.Query` follows the same principles shown 
for [session.Load](../../../client-api/session/revisions/including#including-revisions-with-session.load) 
above:  

* To include a revision by date, the date is passed to `IncludeRevisions` in `DateTime` format.  
* To include a single revision or a group of revisions by their change vectors, 
  a path to the loaded document's property that holds these change vectors is passed 
  to `IncludeRevisions`.  

#### Query: Include Revisions by Date

* **Sample**:  
  {CODE IncludeRevisions_5_QueryByDate@ClientApi\Session\Revisions\Including.cs /}

---

#### Query: Include Revisions by Change Vector

* **Sample**:  
  {CODE IncludeRevisions_6_QueryByChangeVectors@ClientApi\Session\Revisions\Including.cs /}

{PANEL/}

{PANEL: Including Revisions with `session.Advanced.RawQuery`}

#### Raw Query: Include Revisions by Date

* **Sample**:  
  {CODE IncludeRevisions_7_RawQueryByDate@ClientApi\Session\Revisions\Including.cs /}

---

#### Raw Query: Include Revisions by Change Vector

* **Sample**:  
  {CODE IncludeRevisions_8_RawQueryByChangeVector@ClientApi\Session\Revisions\Including.cs /}

{PANEL/}

## Related Articles

### Revisions

- [What are Revisions](../../../client-api/session/revisions/what-are-revisions)
- [Revisions in Data Subscriptions](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning)

### Server

- [Revisions](../../../server/extensions/revisions)
