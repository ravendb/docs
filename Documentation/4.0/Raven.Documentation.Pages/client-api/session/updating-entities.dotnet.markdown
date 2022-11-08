# Session: Updating Entities

Update an entity by retrieving it, editing its properties, and applying your changes using `SaveChanges()`.  

- [Update](../../client-api/session/updating-entities#update)

---

{PANEL:Update}

* Retrieve the entity you wish to update, using a [load](../../client-api/session/loading-entities#load) 
  method or a [query](../../client-api/session/querying/how-to-query#session.query).  
* Edit the properties you wish to change.  
* Call `SaveChanges()` to apply the update.  

---

### Example I

In this example we load a company profile and change its **type** property to **private**.  
{CODE-TABS}
{CODE-TAB:csharp:Sync load-company-and-update-its-type-sync@ClientApi\Session\UpdatingEntities.cs /}
{CODE-TAB:csharp:Async load-company-and-update-its-type-async@ClientApi\Session\UpdatingEntities.cs /}
{CODE-TABS/} 

---

### Example II

Here we run a query to find companies whose type is **public**, and change 
the type of matching companies to **private**.  
{CODE-TABS}
{CODE-TAB:csharp:Sync query-companies-and-update-their-type-sync@ClientApi\Session\UpdatingEntities.cs /}
{CODE-TAB:csharp:Async query-companies-and-update-their-type-async@ClientApi\Session\UpdatingEntities.cs /}
{CODE-TABS/} 

{PANEL/}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Opening a Session](../../client-api/session/opening-a-session)
- [Deleting Entities](../../client-api/session/deleting-entities)
- [Saving Changes](../../client-api/session/saving-changes)

### Querying

- [Basics](../../indexes/querying/basics)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
