using System.IO;
using System.Threading.Tasks;
using Raven.Client.Documents;

namespace Raven.Documentation.Samples.ClientApi.Session.Attachments
{
	public class Attachments
	{
		private interface IFoo
		{
            #region StoreSyntax
		    void Store(string documentId, string name, Stream stream, string contentType = null);
            void Store(object entity, string name, Stream stream, string contentType = null);
            #endregion

            #region delete_1
            void DeleteAttachment(string documentId, string name);
		    void DeleteAttachment(object entity, string name);
            #endregion
        }

        public void StoreAttachment()
		{
			using (var store = new DocumentStore())
			{
                #region StoreAttachment
                using (var session = store.OpenSession())
                using (var file1 = File.Open("001.jpg", FileMode.Open))
                using (var file2 = File.Open("002.jpg", FileMode.Open))
                using (var file3 = File.Open("003.jpg", FileMode.Open))
                using (var file4 = File.Open("004.mp4", FileMode.Open))
                {
                    var album = new Album
                    {
                        Name = "Holidays",
                        Description = "Holidays travel pictures of the all family",
                        Tags = new[] {"Holidays Travel", "All Family"},
                    };
                    session.Store(album, "albums/1");

                    session.Advanced.Attachments.Store("albums/1", "001.jpg", file1, "image/jpeg");
                    session.Advanced.Attachments.Store("albums/1", "002.jpg", file2, "image/jpeg");
                    session.Advanced.Attachments.Store("albums/1", "003.jpg", file3, "image/jpeg");
                    session.Advanced.Attachments.Store("albums/1", "004.mp4", file4, "video/mp4");

                    session.SaveChanges();
                }
				#endregion
			}
		}

        public async Task StoreAttachmentAsync()
		{
			using (var store = new DocumentStore())
			{
                #region StoreAttachmentAsync
                using (var session = store.OpenAsyncSession())
                using (var file1 = File.Open("001.jpg", FileMode.Open))
                using (var file2 = File.Open("002.jpg", FileMode.Open))
                using (var file3 = File.Open("003.jpg", FileMode.Open))
                using (var file4 = File.Open("004.mp4", FileMode.Open))
                {
                    var album = new Album
                    {
                        Name = "Holidays",
                        Description = "Holidays travel pictures of the all family",
                        Tags = new[] {"Holidays Travel", "All Family"},
                    };
                    await session.StoreAsync(album, "albums/1");

                    session.Advanced.Attachments.Store("albums/1", "001.jpg", file1, "image/jpeg");
                    session.Advanced.Attachments.Store("albums/1", "002.jpg", file2, "image/jpeg");
                    session.Advanced.Attachments.Store("albums/1", "003.jpg", file3, "image/jpeg");
                    session.Advanced.Attachments.Store("albums/1", "004.mp4", file4, "video/mp4");

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
