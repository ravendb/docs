using System;
using System.Collections.Generic;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Session
{
    public class WhatIsSession
    {
        public WhatIsSession()
        {
            using (var store = new DocumentStore())
            {
                // A client-side copy of the document Id.
                string companyId = null;

                #region session_usage_1
                // Obtain a Session from your Document Store.
                using (IDocumentSession session = store.OpenSession())
                {
                    // Create a new entity
                    Company entity = new Company { Name = "Company" };
                    // Mark the entity for storage in the Session.
                    session.Store(entity);
                    // Any changes made to the entity will now be tracked by the Session.
                    // However, the changes are persisted to the server only when 'SaveChanges()' is called.
                    session.SaveChanges();
                    // At this point the entity is persisted to the database as a new document.
                    // Since no database was specified when opening the Session, the Default Database is used.
                }
                #endregion

                #region session_usage_2
                // Open a session.
                using (IDocumentSession session = store.OpenSession())
                {
                    // Load an existing document to the Session using its Id.
                    Company entity = session.Load<Company>(companyId);
                    // Edit the entity. The Session will track this change.
                    entity.Name = "Another Company";

                    session.SaveChanges();
                    // At this point, the change made is persisted in the database.
                }
                #endregion

                using (var session = store.OpenSession())
                {
                    #region session_usage_3
                    //When a document is loaded for a second time, it is retrieved from the identity map.
                    Assert.Same(session.Load<Company>(companyId), session.Load<Company>(companyId));
                    //This command will not throw an exception.
                    #endregion
                }
            }
        }
    }
}
