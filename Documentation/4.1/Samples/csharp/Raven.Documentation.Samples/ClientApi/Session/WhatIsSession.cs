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
                #region session_usage_1
                //A client-side copy of the document Id.
                string companyId;

                //Store new document.
                using (IDocumentSession session = store.OpenSession())
                {
                    Company entity = new Company { Name = "Company" };
                    session.Store(entity);
                    //The entity is now marked for storage,
                    //and changes made to it will be tracked,
                    //but it is not sent to the server until `SaveChanges()` is called.
                    session.SaveChanges();

                    //Calling `Store()` has generated an Id for our entity.
                    //Let's save it for use in the next example.
                    companyId = entity.Id;
                }
                #endregion

                #region session_usage_2
                //Load document from database, edit it, and save it.
                using (IDocumentSession session = store.OpenSession())
                {
                    //Use companyId from the previous example.
                    Company entity = session.Load<Company>(companyId); 
                    entity.Name = "Another Company";

                    //When a document is loaded, the session tracks 
                    //all changes made to it (by default).
                    //A call to `SaveChanges()` sends all accumulated changes to the server.
                    session.SaveChanges(); 
                    //Note that no call to `Store()` is required for a loaded document.
                }
                #endregion

                using (var session = store.OpenSession())
                {
                    #region session_usage_3
                    Assert.Same(session.Load<Company>(companyId), session.Load<Company>(companyId));
                    #endregion
                }
            }
        }
    }
}
