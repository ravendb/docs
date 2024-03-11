using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Raven.Documentation.Samples.ClientApi.Session.Configuration
{
    public class DisableCaching
    {
        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                #region disable_caching
                using (IDocumentSession Session = store.OpenSession(new SessionOptions
                {
                    NoCaching = true
                }))
                {
                    // code here
                }
                #endregion

                #region disable_caching_async
                using (IAsyncDocumentSession Session = store.OpenAsyncSession(new SessionOptions
                {
                    NoCaching = true
                }))
                {
                    // async code here
                }
                #endregion
            }
        }
    }
}
