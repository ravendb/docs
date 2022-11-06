using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class Exists
	{
		private interface IExists
		{
            #region exists_1
		    bool Exists(string id);
            #endregion

            #region exists_1_async
            Task<bool> ExistsAsync(string documentId, CancellationToken token = default);
            #endregion
        }

        public async Task Existence()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
				    #region exists_2
				    bool exists = session.Advanced.Exists("employees/1-A");

				    if (exists)
				    {
				        // document 'employees/1-A exists
				    }
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region exists_2_async
                    bool exists = await asyncSession.Advanced.ExistsAsync("employees/1-A");

                    if (exists)
                    {
                        // document 'employees/1-A exists
                    }
                    #endregion
                }
            }
		}
	}
}
