using System.Threading.Tasks;
using Raven.Client.Documents;

namespace Raven.Documentation.Samples.DocumentExtensions.Revisions.ClientAPI.Session
{
    public class getCount
    {
        public async Task Samples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region getCount
                    // Get the number of revisions for document 'companies/1-A"
                    var revisionsCount = session.Advanced.Revisions.GetCountFor("companies/1-A");
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region getCount_async
                    // Get the number of revisions for document 'companies/1-A"
                    var revisionsCount = await asyncSession.Advanced.Revisions.GetCountForAsync("companies/1-A");
                    #endregion
                }
            }
        }
        
        private interface IFoo
        {
            #region syntax
            long GetCountFor(string id);
            #endregion
        }
    }
}
