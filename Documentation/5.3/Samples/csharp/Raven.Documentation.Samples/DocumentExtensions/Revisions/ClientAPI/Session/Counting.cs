using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Json;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Revisions
{
    public class Loading
    {
        private interface IFoo
        {
            #region syntax
            long GetCountFor(string id);
            #endregion
        }

        public async Task Samples()
        {
            using (var store = new DocumentStore())
            {
                var CompanyProfile = new Company { Name = "Persian Rugs" };
                using (var session = store.OpenSession())
                {
                    #region example_sync
                    var revisionsCount = session.Advanced.Revisions.GetCountFor(CompanyProfile.Id);
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region example_async
                    var revisionsCount = await asyncSession.Advanced.Revisions.GetCountForAsync(CompanyProfile.Id);
                    #endregion
                }
            }
        }
    }
}
