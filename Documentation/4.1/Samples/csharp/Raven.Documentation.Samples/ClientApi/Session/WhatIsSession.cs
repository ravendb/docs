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
                string companyId;

                //story new object
                using (IDocumentSession session = store.OpenSession())
                {
                    Company entity = new Company { Name = "Company" };
                    session.Store(entity);
                    session.SaveChanges();

                    //after calling SaveChanges(), an Id property if exists
                    //is filled by the entity's Id
                    companyId = entity.Id;
                }

                using (IDocumentSession session = store.OpenSession())
                {
                    //load by Id
                    Company entity = session.Load<Company>(companyId);
                    
                    //do something with the loaded entity
                }
                #endregion

                #region session_usage_2
                using (IDocumentSession session = store.OpenSession())
                {
                    Company entity = session.Load<Company>(companyId);
                    entity.Name = "Another Company";

                    //When a document is loaded by Id (or by query),
                    //its changes are tracked (by default).
                    //A call to SaveChanges() sends all accumulated changes to the server
                    session.SaveChanges(); 
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
