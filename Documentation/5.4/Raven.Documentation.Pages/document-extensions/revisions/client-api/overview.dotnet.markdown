# Revisions Client API Overview

---

{NOTE: }

* For a general revisions overview see: [Revisions Overview](../../../document-extensions/revisions/overview).  

* Document revisions can be managed using [Studio](../../../studio/database/settings/document-revisions) or from the **Client API**.  

* From the **Client API**, revisions can be configured and managed by:  
    * [Store Operations](../../../document-extensions/revisions/client-api/overview#revisions-store-operations)  
    * [Session Methods](../../../document-extensions/revisions/client-api/overview#revisions-session-methods)  

{NOTE/}

---

{PANEL: Revisions Store Operations}

* [ConfigureRevisionsOperation](../../../document-extensions/revisions/client-api/operations/configure-revisions) - apply a revision configuration
* [ConfigureRevisionsForConflictsOperation](../../../document-extensions/revisions/client-api/operations/conflict-revisions-configuration) - manage conflicting documents revisions
* [GetRevisionsOperation](../../../document-extensions/revisions/client-api/operations/get-revisions) - get revisions

{PANEL/}

{PANEL: Revisions Session methods}

* **Get revisions**:  

  * [GetFor](../../../document-extensions/revisions/client-api/session/loading#get-all-revisions) - retrieve all revisions kept for a document.
  * [GetMetadataFor](../../../document-extensions/revisions/client-api/session/loading#get-revisions-metadata) - retrieve metadata for all revisions kept for a document.
  * [Get](../../../document-extensions/revisions/client-api/session/loading#get-revisions-by-creation-time) - retrieve revisions by change vector or creation time.
  * Read [here](../../../client-api/session/how-to/perform-operations-lazily#getRevisions) about **lazy versions** of `GetFor`, `GetMetadataFor`, and `Get`.

---

* **Include revisions**:  

  * [IncludeRevisions](../../../document-extensions/revisions/client-api/session/including#section) - include revisions when retrieving documents via `Session.Load` or `Session.Query`.  
  * [RawQuery](../../../document-extensions/revisions/client-api/session/including#including-revisions-with-session.advanced.rawquery) - Learn how to include revisions when retrieving documents via raw queries.

---

* **Count revisions**:  

  * [GetCountFor](../../../document-extensions/revisions/client-api/session/counting#getcountfor) - get the number of revisions kept for a document.  

---

* **Force revision creation**:

    * [ForceRevisionCreationFor](../../../document-extensions/revisions/overview#force-revision-creation-via-api) - create a revision even if revision configuration is disabled.

{PANEL/}

## Related Articles

### Document Extensions

* [Document Revisions Overview](../../../document-extensions/revisions/overview)  
* [Revert Revisions](../../../document-extensions/revisions/revert-revisions)  
* [Revisions and Other Features](../../../document-extensions/revisions/revisions-and-other-features)  

### Studio

* [Settings: Document Revisions](../../../studio/database/settings/document-revisions)  
* [Document Extensions: Revisions](../../../studio/database/document-extensions/revisions)  
