using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class AttachmentExists
	{
		private interface IExists
		{
            #region syntax
            bool Exists(string documentId, string attachmentName);
            #endregion

            #region syntax_async
            Task<bool> ExistsAsync(string documentId, string attachmentName, CancellationToken token = default);
            #endregion
        }

        public async Task Exist()
		{
			using (var store = new DocumentStore())
			{
                using (var session = store.OpenSession())
                {
                    #region exists
                    bool exists = session
                        .Advanced
                        .Attachments
                        .Exists("categories/1-A", "image.jpg");

                    if (exists)
                    {
                        // attachment 'image.jpg' exists on document 'categories/1-A'
                    }
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region exists_async
                    bool exists = await asyncSession
                        .Advanced
                        .Attachments
                        .ExistsAsync("categories/1-A", "image.jpg");

                    if (exists)
                    {
                        // attachment 'image.jpg' exists on document 'categories/1-A'
                    }
                    #endregion
                }
            }
		}
	}
}
