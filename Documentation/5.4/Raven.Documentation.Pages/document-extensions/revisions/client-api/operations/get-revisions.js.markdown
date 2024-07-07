# Get Revisions Operation

---

{NOTE: }

* Use `GetRevisionsOperation` to GET the document's revisions.

* To only COUNT the number of revisions without getting them, use the [getCountFor](../../../../document-extensions/revisions/client-api/session/counting) session method.

* In this page:  
  * [Get all revisions](../../../../document-extensions/revisions/client-api/operations/get-revisions#get-all-revisions)  
  * [Paging results](../../../../document-extensions/revisions/client-api/operations/get-revisions#paging-results)  
  * [Syntax](../../../../document-extensions/revisions/client-api/operations/get-revisions#syntax)  

{NOTE/}

---

{PANEL: Get all revisions}

{CODE:nodejs getAllRevisions@documentExtensions\revisions\client-api\operations\getRevisions.js /}

{PANEL/}

{PANEL: Paging results}

* Get and process revisions, one page at a time:

{CODE:nodejs getRevisionsWithPaging@documentExtensions\revisions\client-api\operations\getRevisions.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@documentExtensions\revisions\client-api\operations\getRevisions.js /}

| Parameter | Type | Description |
| - | - | - |
| **id** | `string` | Document ID for which to get revisions |
| **parameters** | `object` | An object that wraps `start` and `pageSize` (see below) |

{CODE:nodejs syntax_2@documentExtensions\revisions\client-api\operations\getRevisions.js /}

| Return value of `store.operations.send(getRevisionsOp)` | |
| - | - |
| `RevisionsResult` | Object with revisions results |

{CODE:nodejs syntax_3@documentExtensions\revisions\client-api\operations\getRevisions.js /}

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
