# Revisions API Overview

---

{NOTE: }

The Revisions client API includes a set of session methods and store operations 
that you can use to [configure](../../../document-extensions/revisions/overview#revisions-configurations), 
manage and use document revisions.  

* In this page:  
  * [Revisions Store Operations](../../../document-extensions/revisions/client-api/overview#revisions-store-operations)  
  * [Revisions Session methods](../../../document-extensions/revisions/client-api/overview#revisions-session-methods)  

{NOTE/}

---

{PANEL: Revisions Store Operations}

* [Creating configurations](../../../document-extensions/revisions/client-api/operations/configure-revisions)  
   * [ConfigureRevisionsOperation](../../../document-extensions/revisions/client-api/operations/configure-revisions#configurerevisionsoperation)  
     Use this operation to apply 
     [Revisions configurations](../../../document-extensions/revisions/overview#revisions-configurations) 
     to your collections.  
* [Getting and Counting Revisions](../../../document-extensions/revisions/client-api/operations/get-revisions)  
   * [GetRevisionsOperation](../../../document-extensions/revisions/client-api/operations/get-revisions#getrevisionsoperation)  
     Use this operation to Get and Count Revisions.  

{PANEL/}

{PANEL: Revisions Session methods}

* [Loading revisions and their metadata](../../../document-extensions/revisions/client-api/session/loading)  
   * [GetFor](../../../document-extensions/revisions/client-api/session/loading#getfor)  
     Use this method to retrieve all the revisions that are kept for a specified document.  
   * [GetMetadataFor](../../../document-extensions/revisions/client-api/session/loading#getmetadatafor)  
     Use this method to retrieve metadata for all revisions kept for a specified document.  
   * [Get](../../../document-extensions/revisions/client-api/session/loading#get)  
     Use this method to retrieve revisions by change vector or creation time.  
* [Counting Revisions](../../../document-extensions/revisions/client-api/session/counting)  
   * [GetCountFor](../../../document-extensions/revisions/client-api/session/counting#getcountfor)  
     Use this method to count the revisions kept for a document.  
* [Including revisions](../../../document-extensions/revisions/client-api/session/including)  
   * [IncludeRevisions](../../../document-extensions/revisions/client-api/session/including#section)  
     Use this method to include document revisions when retrieving documents via `Session.Load` or `Session.Query`.  
   * [RawQuery](../../../document-extensions/revisions/client-api/session/including#including-revisions-with-session.advanced.rawquery)  
     Learn here how to include revisions with documents retrieved via raw queries.  

{PANEL/}

## Related Articles

### Client API

- [Session: What are Revisions](../../../document-extensions/revisions/client-api/session/what-are-revisions)  
- [Session: Loading Revisions](../../../document-extensions/revisions/client-api/session/loading)  
- [Operations: How to Configure Revisions](../../../document-extensions/revisions/client-api/operations/configure-revisions)  
- [Revisions in Data Subscriptions](../../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning)  
