# Revisions: Counting Revisions

---

{NOTE: }

* A document's revisions can be counted using the `session.Advanced.Revisions.GetCountFor` method.  

* In this page:  
   * [`GetCountFor`](../../../../document-extensions/revisions/client-api/session/counting#getcountfor)  
   * [Usage Sample](../../../../document-extensions/revisions/client-api/session/counting#usage-sample)  

{NOTE/}

---

{PANEL: `GetCountFor`}

{CODE syntax@DocumentExtensions\Revisions\ClientAPI\Session\Counting.cs /}

| Parameter | Type | Description |
| ------------- | ------------- | ------------- |
| **id** | string | ID of the document whose revisions are counted |

* **Return Value**: `long`  
  The number of revisions for this document  

{PANEL/}

{PANEL: Usage Sample}

Get the number of revisions created for a document:
{CODE-TABS}
{CODE-TAB:csharp:Sync example_sync@DocumentExtensions\Revisions\ClientAPI\Session\Counting.cs /}
{CODE-TAB:csharp:Async example_async@DocumentExtensions\Revisions\ClientAPI\Session\Counting.cs /}
{CODE-TABS/}

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

### Studio

* [Settings: Document Revisions](../../../../studio/database/settings/document-revisions)  
* [Document Extensions: Revisions](../../../../studio/database/document-extensions/revisions)  
