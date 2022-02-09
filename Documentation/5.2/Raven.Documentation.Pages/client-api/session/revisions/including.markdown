# Revisions: Including Revisions
---

{NOTE: }

* A document's **Revisions** can be 
  [Included](../../../client-api/how-to/handle-document-relationships#includes) 
  with the document.  
  The included revisions are retrieved when the document is loaded, without 
  requiring additional trips to the server.  
* Revisions can be Included by their **Creation Date** or **Change Vector**, 
  and retrieved when documents are -  
   * Loaded using `session.Load`  
   * Queried using `session.Query`  
   * Queried using `session.Advanced.RawQuery`  

- [Including Revisions](../../../client-api/session/revisions/including#including-revisions)  
- [`IncludeRevisions`](../../../client-api/session/revisions/including#includerevisions)  
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
any of its included revisions will retrieve them from memory rather than from the server.  

This may be useful when, for example, a document that contains financial data 
is loaded by an auditing application. The document's past revisions can be Included 
with the document, to make the document's history available for instant inspection.  

Revisions can be Included by their **Creation Date** or **Change Vector**.  

---

#### Including Revisions By Date

To include **a single revision** by its **creation date** -  
Pass a `DateTime` value to 
[session.Load](../../../client-api/session/revisions/including#including-revisions-with-session.load) 
or [session.Query](../../../client-api/session/revisions/including#including-revisions-with-session.query) 
using `IncludeRevisions` (see below),  
or to a [Raw Query](../../../client-api/session/revisions/including#including-revisions-with-session.advanced.rawquery) 
using `.AddParameter`.  

* If the provided date matches the creation date of a document revision, this revision will be included.  
* If no exact match is found, the nearest revision to precede it will be included.  
   * E.g. -  
     If the date you provided is `Aprl 7 2020`,  
     and it is located between two existing revisions dated `April 2 2020` and `April 11 2020`,  
     the `April 2` revision will be included.  
* If no revisions exist, an empty `IncludeRevisions` object will be returned.  

---

#### Including Revisions By Change Vector

Each time a document is modified, its [Change Vector](../../../server/clustering/replication/change-vector) 
is revised to trigger the document's replication, backup, etc.  
While the **Revisions** feature is enabled, each new document revision keeps the document's 
change vector at the time of its creation.  
We can use this to track and include **Single** or **Multiple** document revisions by their change vectors.  
To do so:  

1. Store helpful change vectors in advance, in a property of the document.  
2. When you want to include revisions by their change vector -  
   Pass the **path** to the document property to 
   [session.Load](../../../client-api/session/revisions/including#including-revisions-with-session.load) 
   or [session.Query](../../../client-api/session/revisions/including#including-revisions-with-session.query) 
   via `IncludeRevisions` (see below),  
   or to a [Raw Query](../../../client-api/session/revisions/including#including-revisions-with-session.advanced.rawquery) 
   using `.AddParameter`.  

{NOTE: }
Storing a document's change vector/s in one of the document's properties helps 
clarify their context and purpose.  
E.g. -  
A change vector can be added to an employee's contract document's "ContractRev" property 
each time the contract is revised (e.g. when their salary is raised).  
Whenever the time comes to re-evaluate this employee's terms and their contract 
is loaded, its past revisions can be easily included with it by their change vectors.  
{NOTE/}

{PANEL/}

{PANEL: `IncludeRevisions`}

Revisions can be included with documents retrieved via `session.Load` 
and `session.Query`, using one of the `IncludeRevisions` methods.  

{CODE IncludeRevisions_1_IncludeRevisions@ClientApi\Session\Revisions\Including.cs /}

| Parameters | Type | Description |
| ------------- | --------------------------- | ----- |
| **before** | `DateTime` | A **single revision** that matches or precedes the given date will be included. |
| **path** | `Expression<Func<T, string>>` | A path to a document property that contains **a single change vector**. <br> The revision whose change vector is contained in the document property will be Included. |
| **path** | `Expression<Func<T, IEnumerable<string>>>` | A path to a document property that contains **an array of change vectors**. <br> The revisions whose change vectors are contained in the array will be Included. |

{PANEL/}


{PANEL: Including Revisions With `session.Load`}

#### Load: Include Revisions by Date

To include a **single revision** by its **creation date**, pass `IncludeRevisions` the date.  
The revision whose date [matches or immediately precedes](../../../client-api/session/revisions/including#including-revisions-by-date) 
that of the given date will be included.  

* **Sample**:  
  {CODE IncludeRevisions_2_LoadByDate@ClientApi\Session\Revisions\Including.cs /}

---

#### Load: Include Revisions by Change Vector

* To include a **single revision** by a document's change vector:  
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
* RavenDB will figure out by itself whether the parameter you passed was a date or 
  a path, and include revisions accordingly.  
* Aliases (e.g. `from Users as U`) are not supported by raw queries that include revisions.  

#### Raw Query: Include Revisions by Date

* **Sample**:  
  {CODE IncludeRevisions_7_RawQueryByDate@ClientApi\Session\Revisions\Including.cs /}

---

#### Raw Query: Include Revisions by Change Vector

* **Sample**:  
  {CODE IncludeRevisions_8_RawQueryByChangeVector@ClientApi\Session\Revisions\Including.cs /}

{PANEL/}

## Related Articles

### Client API

- [What are Revisions](../../../client-api/session/revisions/what-are-revisions)
- [Loading Revisions](../../../client-api/session/revisions/loading))
- [Include](../../../client-api/how-to/handle-document-relationships#includes)

### Server

- [Revisions](../../../server/extensions/revisions)
