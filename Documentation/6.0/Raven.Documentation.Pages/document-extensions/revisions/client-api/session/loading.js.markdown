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

* Use `getFor` to retrieve all of the revisions currently kept for the specified document.

---

**Example**:

{CODE:nodejs example_1@document-extensions\revisions\client-api\session\loading.js /}

**Syntax**:

{CODE:nodejs syntax_1@document-extensions\revisions\client-api\session\loading.js /}

| Parameters | Type | Description |
| - | - |- |
| **id** | string | Document ID for which to retrieve revisions |
| **options** | options object | Used for paging |

{CODE:nodejs syntax_5@document-extensions\revisions\client-api\session\loading.js /}

| Return value | |
| - | - |
| `Promise` | A `Promise` resolving to the document's revisions. <br>Revisions will be ordered by most recent revision first. |

{PANEL/}

{PANEL: Get revisions metadata}

* Use `getMetadataFor` to retrieve the metadata for all the revisions currently kept for the specified document.

---

**Example**:

{CODE:nodejs example_2@document-extensions\revisions\client-api\session\loading.js /}

**Syntax**:

{CODE:nodejs syntax_2@document-extensions\revisions\client-api\session\loading.js /}

| Parameters | Type | Description |
| - | - |- |
| **id** | string | Document ID for which to retrieve revisions' metadata |
| **options** | options object | Used for paging |

{CODE:nodejs syntax_5@document-extensions\revisions\client-api\session\loading.js /}

| Return value | |
| - | - |
| `Promise` | A `Promise` resolving to a list of the revisions metadata. |

{PANEL/}

{PANEL: Get revisions by creation time}

* Use `get` to retrieve a revision by its **creation time**.

---

**Example**:

{CODE:nodejs example_3@document-extensions\revisions\client-api\session\loading.js /}

**Syntax**:

{CODE:nodejs syntax_3@document-extensions\revisions\client-api\session\loading.js /}

| Parameter | Type | Description |
| - | - | - |
| **id** | string | Document ID for which to retrieve the revision by creation time |
| **date** | string | The revision's creation time |

| Return value | |
| - | - |
| `Promise` | A `Promise` resolving to the revision.<br>If no revision was created at the specified time, then the first revision that precedes it will be returned. |

{PANEL/}

{PANEL: Get revisions by change vector}

* Use `get` to retrieve a revision or multiple revisions by their **change vectors**.

---

**Example**:

{CODE:nodejs example_4@document-extensions\revisions\client-api\session\loading.js /}

**Syntax**:

{CODE:nodejs syntax_4@document-extensions\revisions\client-api\session\loading.js /}

| Parameter | Type | Description |
| - | - | - |
| **changeVector** | string | The revision's change vector |
| **changeVectors** | string[] | Change vectors of multiple revisions |

| Return value | |
| - | - |
| `Promise` | A `Promise` resolving to the matching revision(s). |

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
