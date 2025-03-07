# Revisions Client API Overview

---

{NOTE: }

* For a general revisions overview see: [Revisions Overview](../../../document-extensions/revisions/overview).  

* Document revisions can be managed using [Studio](../../../studio/database/settings/document-revisions) or from the **Client API**.  

* Find below a list of methods that revisions can be configured and managed by.  
    * [Store Operations](../../../document-extensions/revisions/client-api/overview#revisions-store-operations)  
    * [Session Methods](../../../document-extensions/revisions/client-api/overview#revisions-session-methods)  

{NOTE/}

---

{PANEL: Revisions `store` Operations}

* [ConfigureRevisionsOperation](../../../document-extensions/revisions/client-api/operations/configure-revisions) - apply a revision configuration
* [GetRevisionsOperation](../../../document-extensions/revisions/client-api/operations/get-revisions) - get revisions

{PANEL/}

{PANEL: Revisions `session` methods}

#### Get revisions:  

* Use [getFor](../../../document-extensions/revisions/client-api/session/loading#get-all-revisions) 
  to retrieve **all** revisions kept for a document.  
* Use [getMetadataFor](../../../document-extensions/revisions/client-api/session/loading#get-revisions-metadata) 
  to retrieve **metadata** for all revisions kept for a document.  
* Use [getBeforeDate](../../../document-extensions/revisions/client-api/session/loading#get-revisions-by-creation-time) 
  to retrieve revisions by **change vector** or **creation time**.  
* Read [here](../../../client-api/session/how-to/perform-operations-lazily#get-revisions) 
  about **lazy versions** of revision methods.  

---

#### Count revisions:  

Use [getCountFor](../../../document-extensions/revisions/client-api/session/counting#getcountfor) 
to get the number of revisions kept for a document.  

---

#### Force revision creation:

Read [here](../../../document-extensions/revisions/overview#force-revision-creation-via-api) 
about creating a revision even if revision configuration is disabled.  

{PANEL/}

## Related Articles

### Document Extensions

* [Document Revisions Overview](../../../document-extensions/revisions/overview)  
* [Revert Revisions](../../../document-extensions/revisions/revert-revisions)  
* [Revisions and Other Features](../../../document-extensions/revisions/revisions-and-other-features)  

### Studio

* [Settings: Document Revisions](../../../studio/database/settings/document-revisions)  
* [Document Extensions: Revisions](../../../studio/database/document-extensions/revisions)  
