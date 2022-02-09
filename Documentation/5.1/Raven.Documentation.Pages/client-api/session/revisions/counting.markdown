# Revisions: Counting Revisions

---

{NOTE: }
A document's revisions can be counted using the `session.Advanced.Revisions.GetCountFor` method.  

* In this page:  
   * [Syntax](../../../client-api/session/revisions/counting#syntax)  
   * [Example](../../../client-api/session/revisions/counting#example)  

{NOTE/}

---

{PANEL: Syntax}

{CODE syntax@ClientApi\Session\Revisions\Counting.cs /}

* **`GetCountFor` Parameters**  

    | Parameter | Type | Description |
    | ------------- | ------------- | ------------- |
    | **id** | string | ID of the document whose revisions are counted |

* **Return Value**: `long`  
  The number of revisions for this document.  

{PANEL/}

{PANEL: Example}

{CODE-TABS}
{CODE-TAB:csharp:Sync example_sync@ClientApi\Session\Revisions\Counting.cs /}
{CODE-TAB:csharp:Async example_async@ClientApi\Session\Revisions\Counting.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Client API

- [What are Revisions](../../../client-api/session/revisions/what-are-revisions)  
- [Loading Revisions](../../../client-api/session/revisions/loading)  

### Server

- [Revisions](../../../server/extensions/revisions)
