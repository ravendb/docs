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
                //a client-side copy of our entity Id
                string companyId;

                //first session: store new document
                using (IDocumentSession session = store.OpenSession())
                {
                    Company entity = new Company { Name = "Company" };
                    session.Store(entity);
                    session.SaveChanges();

                    //calling Store() has generated an Id for our entity
                    //let's save it for use by another session
                    companyId = entity.Id;
                }

                //second session: load document by Id
                using (IDocumentSession session = store.OpenSession())
                {
                    Company entity = session.Load<Company>(companyId);

                    //do something with the loaded entity
                }
                #endregion

                #region session_usage_2
                using (IDocumentSession session = store.OpenSession())
                {
                    Company entity = session.Load<Company>(companyId);
                    entity.Name = "Another Company";

                    //when a document is loaded, the session tracks 
                    //all changes made to it (by default)
                    //a call to SaveChanges() sends all accumulated changes to the server
                    session.SaveChanges(); 
                    //note that no call to Store() is required
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
