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
* You can include revisions by their **Creation Date** and by documents' **Change Vectors**.  

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

To include a revision by its **creation date**, pass ([session.Load](../../../client-api/session/revisions/including#including-revisions-with-session.load), 
[session.Query](../../../client-api/session/revisions/including#including-revisions-with-session.query), 
or your [Raw Query](../../../client-api/session/revisions/including#including-revisions-with-session.advanced.rawquery) 
a `DateTime` value.  

* If you provide the exact creation date of a document revision, this revision will be included.  
* If no exact match is found, the revision whose date precedes the date you provided will be included.  
  E.g., If the date you provided is between revision #499 and revision #500, revision #499 will be included.  
* If no revisions exist, an empty `IncludeRevisions` object will be returned.  

---

#### Including Revisions By Change Vector

When a document is modified, its [Change Vector](../../../server/clustering/replication/change-vector) 
is revised as well to trigger the document's replication, backup, etc.  

Old change vectors are not normally kept.  
If the **Revisions** feature is enabled, however, a new document revision is also created 
each time a document is modified, and the updated change vector marks the new revision.  

We can therefore track and include document revisions by their change vectors.  
To do so, we need to:  

1. Store the change vector whose revision we seek, in a property of the document.  
2. Provide the seeking method or query with a path to this document property.  

This way, the change vectors always remain in the context of their document.  

{NOTE: }
We can, for example, add a change vector to the "SalaryUpdates" property 
of an employee's payroll document, each time the employee's salary is raised.  
Then, when we need to re-evaluate the employee's salary, we can instantly 
track their past salary raises by the kept change vectors, and load these 
revisions along with the payroll document to help our evaluation.  
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

* To include a revision by date, pass `IncludeRevisions` a 
  [DateTime value](../../../client-api/session/revisions/including#including-revisions-by-date).  
* To include a single revision or a group of revisions by change vectors, pass 
  `IncludeRevisions` a [path](../../../client-api/session/revisions/including#including-revisions-by-change-vector) 
  to a property of the loaded document, that stores change vectors.  

#### Query: Include Revisions by Date

* **Sample**:  
  {CODE IncludeRevisions_5_QueryByDate@ClientApi\Session\Revisions\Including.cs /}

---

#### Query: Include Revisions by Change Vector

* **Sample**:  
  {CODE IncludeRevisions_6_QueryByChangeVectors@ClientApi\Session\Revisions\Including.cs /}

{PANEL/}

{PANEL: Including Revisions with `session.Advanced.RawQuery`}

* To include revisions by date or change vector, pass the raw query `include revisions` 
  command either a `DateTime` value or a `path` to a  change vector document property.  
* RavenDB will figure out by itself whether the parameter you passed it was a date or 
  a path, and include revisions accordingly.  
* Aliases (e.g. `from Users as U`) are not supported by raw queries that includes revisions.  

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
