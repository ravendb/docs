# Include Revisions

---

{NOTE: }

* Document revisions can be included in results when:
    * **Making a query** (`session.query` / `session.advanced.rawQuery`)
    * **Loading a document** (`session.load`) from the server

* The revisions to include can be specified by:
    * **Creation time**
    * **Change vector**

* In this page:
  * [Overview:](../../../../document-extensions/revisions/client-api/session/including#overview)
      * [Why include revisions](../../../../document-extensions/revisions/client-api/session/including#why-include)
      * [Including by creation time](../../../../document-extensions/revisions/client-api/session/including#include-by-time)
      * [Including by change vector](../../../../document-extensions/revisions/client-api/session/including#include-by-change-vector)
  * [Include revisions:](../../../../document-extensions/revisions/client-api/session/including#include-revisions-when-loading-document)  
      * [When Loading document](../../../../document-extensions/revisions/client-api/session/including#include-revisions-when-loading-document)
      * [When making a Query](../../../../document-extensions/revisions/client-api/session/including#include-revisions-when-making-a-query)
      * [When making a Raw Query](../../../../document-extensions/revisions/client-api/session/including#include-revisions-when-making-a-raw-query)
  * [Syntax](../../../../document-extensions/revisions/client-api/session/including#syntax)
  * [Patching the revision change vector](../../../../document-extensions/revisions/client-api/session/including#patching-the-revision-change-vector)

{NOTE/}

---

{PANEL: Overview}

{NOTE: }

<a id="why-include" /> **Why include revisions**:

---

* Including revisions may be useful, for example, when an auditing application loads or queries for a document.  
  The document's past revisions can be included with the document to make the document's history available for instant inspection.  

* Once loaded to the session, there are no additional trips to the server when accessing the revisions.  
  [Getting](../../../../document-extensions/revisions/client-api/session/loading) a revision that was included with the document will retrieve it directly from the session.  
  This also holds true when attempting to include revisions but none are found.

{NOTE/}

{NOTE: }

<a id="include-by-time" /> **Including by Creation Time**:

---

* You can include a single revision by specifying its creation time, see examples below.

* You can pass local time or UTC, either way the server will  convert it to UTC.  

* **If the provided time matches** the creation time of a document revision, this revision will be included.

* **If no exact match is found**, then the first revision that precedes the specified time will be returned.

{NOTE/}

{NOTE: }

<a id="include-by-change-vector" /> **Including by Change Vector**:

---

* Each time a document is modified, its [Change Vector](../../../../server/clustering/replication/change-vector) is updated.  

* When a revision is created,  
  the revision's change vector is the change vector of the document at the time of the revision's creation.  

* To include single or multiple document revisions by their change vectors:   

  * When modifying the document, store its updated change vector in a property in the document.  
    Can be done by [patching](../../../../document-extensions/revisions/client-api/session/including#patching-the-revision-change-vector) the document from the Client API or from the Studio.

  * Specify the **path** to this property when including the revisions, see examples below.  
  
  * e.g.:  
    Each time an employee's contract document is modified (e.g. when their salary is raised),  
    you can add the current change vector of the document to a dedicated property in the document.  
    Whenever the time comes to re-evaluate an employee's terms and their contract is loaded,  
    its past revisions can be easily included with it by their change vectors.

{NOTE/}

{PANEL/}

{PANEL: Include revisions when Loading document}

**Include a revision by Time**

{CODE:nodejs include_1@documentextensions\revisions\client-api\session\including.js /}

---

**Include revisions by Change Vector**

{CODE:nodejs include_2@documentextensions\revisions\client-api\session\including.js /}

<a id="sample-document" />
{CODE:nodejs sample_document@documentextensions\revisions\client-api\session\including.js /}

{PANEL/}

{PANEL: Include revisions when making a Query}

**Include revisions by Time**

{CODE:nodejs include_3@documentextensions\revisions\client-api\session\including.js /}

---

**Include revisions by Change Vector**

{CODE:nodejs include_4@documentextensions\revisions\client-api\session\including.js /}

* See the _Contract_ class definition [above](../../../../document-extensions/revisions/client-api/session/including#sample-document). 

{PANEL/}

{PANEL: Include revisions when making a Raw Query}

* Use `include revisions` in your RQL when making a raw query.  

* Pass either the revision creation time or the path to the document property containing the change vector(s),  
  RavenDB will figure out the parameter type passed and include the revisions accordingly.    

* Aliases (e.g. `from Users as U`) are Not supported by raw queries that include revisions.

---

**Include revisions by Time**

{CODE:nodejs include_5@documentextensions\revisions\client-api\session\including.js /}

---

**Include revisions by Change Vector**

{CODE:nodejs include_6@documentextensions\revisions\client-api\session\including.js /}

* See the _Contract_ class definition [above](../../../../document-extensions/revisions/client-api/session/including#sample-document).

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@documentextensions\revisions\client-api\session\including.js /}

| Parameters | Type | Description |
| - | - | - |
| **before** | `string` | <ul><li>Creation time of the revision to be included.</li><li>Pass local time or UTC.<br>The server will convert the param to UTC.</li><li>If no revision was created at this time then the first revision that precedes it is returned.</li></ul> |
| **path** | `string` | <ul><li>The path to the document property that contains <br> **a single change vector** or **an array of change vectors** <br>of the revisions to be included.</li></ul> |

| Return value | |
| - | - |
| `object` | <ul><li>When **loading** a document:<br>A builder object that is used to build the include part in the Load request.</il><li>When **querying** for a document:<br>A builder object that is used to build the include part in the Query RQL expression.</li><li>Can be used in chaining.</li></ul> |

{PANEL/}

{PANEL: Patching the revision change vector}

* To include revisions when making a query or a raw query,  
  you need to specify the path to the document property that contains the revision change vector(s).

* The below example shows how to get and patch a revision change vector to a document property.

{CODE:nodejs include_7@documentextensions\revisions\client-api\session\including.js /}

* See the _Contract_ class definition [above](../../../../document-extensions/revisions/client-api/session/including#sample-document).

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
* [Session: Counting Revisions](../../../../document-extensions/revisions/client-api/session/counting)  

### Studio

* [Settings: Document Revisions](../../../../studio/database/settings/document-revisions)  
* [Document Extensions: Revisions](../../../../studio/database/document-extensions/revisions)  
