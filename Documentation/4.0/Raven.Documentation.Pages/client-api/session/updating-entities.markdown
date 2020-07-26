# Session: Updating Entities

To update an entity retrieve it ([explicitly](../../client-api/session/loading-entities#load) 
or by a [query](../../client-api/session/querying/how-to-query#session.query)), edit the properties 
you wish to change, and call `saveChanges()` to apply the changes.  

---

{PANEL:Samples}

* In this sample we explicitly load a company profile, and change its type to **private**.  
   {CODE load-company-and-update-its-type@ClientApi\Session\UpdatingEntities.cs /}  

* In this sample we find companies whose type is **public** using a query, and change it to **private**.  
   {CODE query-companies-and-update-their-type@ClientApi\Session\UpdatingEntities.cs /}  


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
