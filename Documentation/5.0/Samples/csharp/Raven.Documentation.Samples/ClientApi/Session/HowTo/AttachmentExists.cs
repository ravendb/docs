using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class AttachmentExists
	{
		private interface IExists
		{
            #region exists_1
            bool Exists(string documentId, string name);
            #endregion

            #region exists_1_async
            Task<bool> ExistsAsync(string documentId, string name, CancellationToken token = default);
            #endregion
        }

        public async Task Existence()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
				    #region exists_2
				    bool exists = session
                        .Advanced
                        .Attachments
                        .Exists("categories/1-A","image.jpg");

				    if (exists)
				    {
				        //do something
				    }
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region exists_2_async
                    bool exists = await asyncSession
                                            .Advanced
                                            .Attachments
                                            .ExistsAsync("categories/1-A", "image.jpg");

                    if (exists)
                    {
                        //do something
                    }
                    #endregion
                }
            }
		}
	}
}
