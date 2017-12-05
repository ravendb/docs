using System.IO;
using System.Threading.Tasks;
using Raven.Client.Documents;

namespace Raven.Documentation.Samples.ClientApi.Session.Attachments
{
	public class Attachments
	{
		private interface IFoo
		{
			#region put_1
			void StoreAttachment(string documentId, string name, Stream stream, string contentType = null);
            void StoreAttachment(object entity, string name, Stream stream, string contentType = null);
            #endregion

            #region delete_1
		    void DeleteAttachment(string documentId, string name);
		    void DeleteAttachment(object entity, string name);
            #endregion
        }

        public async Task StoreAttachmentAsync()
		{
			using (var store = new DocumentStore())
			{
                #region StoreAttachmentAsync
                using (var session = store.OpenAsyncSession())
                using (var file = File.Open("sea.png", FileMode.Open))
                {
                    var album = new Album
                    {
                        Name = "Holidays",
                        Description = "Holidays 2014"
                    };
                    await session.StoreAsync(album, "albums/1");

                    session.Advanced.Attachments.Store("albums/1", "sea.png", file, "image/png");

                    await session.SaveChangesAsync();
                }
				#endregion
			}
		}

        public async Task DeleteAttachmentAsync()
		{
			using (var store = new DocumentStore())
			{
                #region DeleteAttachmentAsync
                using (var session = store.OpenAsyncSession())
                {
                    session.Advanced.Attachments.Delete("albums/1", "sea.png");

                    await session.SaveChangesAsync();
                }
				#endregion
			}
		}
	}
}
