# Update Entities
---

{NOTE: }

* To modify existing documents:

    * **Retrieve** documents from the database using [Load](../../client-api/session/loading-entities#load) or by a [Query](../../client-api/session/querying/how-to-query#session.query).  
      The entities loaded from the documents are added to the internal entities map that the Session manages.
  
    * **Edit** the properties you wish to change.  
      The session will track all changes made to the loaded entities.

    * **Save** to apply the changes.  
      Once `SaveChanges()` returns it is guaranteed that the data is persisted in the database.
      


* In this page:
    * [Load a document & update](../../client-api/session/updating-entities#load-a-document-&-update)
    * [Query for documents & update](../../client-api/session/updating-entities#query-for-documents-&-update)
    
{NOTE/}

---

{PANEL: Load a document & update}

* In this example we `Load` a company document and update its **PostalCode** property.  

{CODE-TABS}
{CODE-TAB:csharp:Sync load-company-and-update@ClientApi\Session\UpdateDocuments.cs /}
{CODE-TAB:csharp:Async load-company-and-update-async@ClientApi\Session\UpdateDocuments.cs /}
{CODE-TABS/} 

{PANEL/}

{PANEL: Query for documents & update}

* In this example we `Query` for company documents whose **PostalCode** property is _12345_,  
  and modify this property for the matching documents.  

{CODE-TABS}
{CODE-TAB:csharp:Sync query-companies-and-update@ClientApi\Session\UpdateDocuments.cs /}
{CODE-TAB:csharp:Async query-companies-and-update-async@ClientApi\Session\UpdateDocuments.cs /}
{CODE-TABS/} 

{PANEL/}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Opening a Session](../../client-api/session/opening-a-session)
- [Deleting Entities](../../client-api/session/deleting-entities)
- [Saving Changes](../../client-api/session/saving-changes)

### Querying

- [Query Overview](../../client-api/session/querying/how-to-query)
- [Querying an Index](../../indexes/querying/query-index)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
