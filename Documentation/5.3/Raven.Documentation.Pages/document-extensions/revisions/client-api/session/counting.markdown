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

### Session

- [What are Revisions](../../../../client-api/session/revisions/what-are-revisions)  
- [Loading Revisions](../../../../client-api/session/revisions/loading)  

### Operations

- [Configure Revisions](../../../../client-api/operations/revisions/configure-revisions)
- [Get Revisions](../../../../client-api/operations/revisions/get-revisions)

### Server

- [Revisions](../../../../server/extensions/revisions)
