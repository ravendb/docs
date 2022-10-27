using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using Xunit;
using System.Threading.Tasks;

namespace Raven.Documentation.Samples.ClientApi.Session
{
    public class WhatIsSession
    {
        public async Task Samples()
        {
            using (var store = new DocumentStore())
            {
                // A client-side copy of the document Id.
                string companyId = null;

                #region session_usage_1
                // Obtain a Session from your Document Store
                using (IDocumentSession session = store.OpenSession())
                {
                    // Create a new entity
                    Company entity = new Company { Name = "CompanyName" };
                    
                    // Store the entity in the Session's internal map
                    session.Store(entity);
                    // From now on, any changes that will be made to the entity will be tracked by the Session.
                    // However, the changes will be persisted to the server only when 'SaveChanges()' is called.
                    
                    session.SaveChanges();
                    // At this point the entity is persisted to the database as a new document.
                    // Since no database was specified when opening the Session, the Default Database is used.
                }
                #endregion
                
                #region session_usage_1_async
                // Obtain a Session from your Document Store
                using (IAsyncDocumentSession asyncSession = store.OpenAsyncSession())
                {
                    // Create a new entity
                    Company entity = new Company { Name = "CompanyName" };
                    
                    // Store the entity in the Session's internal map
                    asyncSession.StoreAsync(entity);
                    // From now on, any changes that will be made to the entity will be tracked by the Session.
                    // However, the changes will be persisted to the server only when 'SaveChanges()' is called.
                    
                    asyncSession.SaveChangesAsync();
                    // At this point the entity is persisted to the database as a new document.
                    // Since no database was specified when opening the Session, the Default Database is used.
                }
                #endregion

                #region session_usage_2
                // Open a session
                using (IDocumentSession session = store.OpenSession())
                {
                    // Load an existing document to the Session using its ID
                    // The loaded entity will be added to the session's internal map
                    Company entity = session.Load<Company>(companyId);
                    
                    // Edit the entity, the Session will track this change
                    entity.Name = "NewCompanyName";

                    session.SaveChanges();
                    // At this point, the change made is persisted to the existing document in the database
                }
                #endregion

                #region session_usage_2_async
                // Open a Session
                using (IAsyncDocumentSession asyncSession = store.OpenAsyncSession())
                {
                    // Load an existing document to the Session using its ID
                    // The loaded entity will be added to the session's internal map
                    Company entity = await asyncSession.LoadAsync<Company>(companyId);
                    
                    // Edit the entity, the Session will track this change
                    entity.Name = "NewCompanyName";

                    asyncSession.SaveChangesAsync();
                    // At this point, the change made is persisted to the existing document in the database
                }
                #endregion

                using (var session = store.OpenSession())
                {
                    #region session_usage_3
                    // A document is fetched from the server
                    Company entity1 = session.Load<Company>(companyId);
                    
                    // Loading the same document will now retrieve its entity from the Session's map
                    Company entity2 = session.Load<Company>(companyId);
                    
                    // This command will Not throw an exception.
                    Assert.Same(entity1, entity2);
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region session_usage_3_async
                    // A document is fetched from the server
                    Company entity1 = await asyncSession.LoadAsync<Company>(companyId);
                    
                    // Loading the same document will now retrieve its entity from the Session's map
                    Company entity2 = await asyncSession.LoadAsync<Company>(companyId);
                    
                    // This command will Not throw an exception.
                    Assert.Same(entity1, entity2);
                    #endregion
                }
            }
        }
    }
}
