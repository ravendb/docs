using System.IO;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;

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

            #region GetSyntax
		    AttachmentResult Get(string documentId, string name);
		    AttachmentResult Get(object entity, string name);
		    AttachmentName[] GetNames(object entity);
            AttachmentResult GetRevision(string documentId, string name, string changeVector);
		    bool Exists(string documentId, string name);
            #endregion

            #region DeleteSyntax
            void Delete(string documentId, string name);
		    void Delete(object entity, string name);
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

        public void GetAttachment()
		{
			using (var store = new DocumentStore())
			{
                #region GetAttachment
                using (var session = store.OpenSession())
                {
                    Album album = session.Load<Album>("albums/1");

                    using (AttachmentResult file1 = session.Advanced.Attachments.Get(album, "001.jpg"))
                    using (AttachmentResult file2 = session.Advanced.Attachments.Get("albums/1", "002.jpg"))
                    {
                        Stream stream = file1.Stream;

                        AttachmentDetails attachmentDetails = file1.Details;
                        string name = attachmentDetails.Name;
                        string contentType = attachmentDetails.ContentType;
                        string hash = attachmentDetails.Hash;
                        long size = attachmentDetails.Size;
                        string documentId = attachmentDetails.DocumentId;
                        string changeVector = attachmentDetails.ChangeVector;
                    }

                    AttachmentName[] attachmentNames = session.Advanced.Attachments.GetNames(album);
                    foreach (AttachmentName attachmentName in attachmentNames)
                    {
                        string name = attachmentName.Name;
                        string contentType = attachmentName.ContentType;
                        string hash = attachmentName.Hash;
                        long size = attachmentName.Size;
                    }

                    bool isExists = session.Advanced.Attachments.Exists("albums/1", "003.jpg");
                }
				#endregion
			}
		}

        public async Task GetAttachmentAsync()
		{
			using (var store = new DocumentStore())
			{
                #region GetAttachmentAsync
                using (var session = store.OpenAsyncSession())
                {
                    Album album = await session.LoadAsync<Album>("albums/1");

                    using (AttachmentResult file1 = await session.Advanced.Attachments.GetAsync(album, "001.jpg"))
                    using (AttachmentResult file2 = await session.Advanced.Attachments.GetAsync("albums/1", "002.jpg"))
                    {
                        Stream stream = file1.Stream;

                        AttachmentDetails attachmentDetails = file1.Details;
                        string name = attachmentDetails.Name;
                        string contentType = attachmentDetails.ContentType;
                        string hash = attachmentDetails.Hash;
                        long size = attachmentDetails.Size;
                        string documentId = attachmentDetails.DocumentId;
                        string changeVector = attachmentDetails.ChangeVector;
                    }

                    AttachmentName[] attachmentNames = session.Advanced.Attachments.GetNames(album);
                    foreach (AttachmentName attachmentName in attachmentNames)
                    {
                        string name = attachmentName.Name;
                        string contentType = attachmentName.ContentType;
                        string hash = attachmentName.Hash;
                        long size = attachmentName.Size;
                    }

                    bool isExists = await session.Advanced.Attachments.ExistsAsync("albums/1", "003.jpg");
                }
				#endregion
			}
		}

        public void DeleteAttachment()
		{
			using (var store = new DocumentStore())
			{
                #region DeleteAttachment
                using (var session = store.OpenSession())
                {
                    Album album = session.Load<Album>("albums/1");
                    session.Advanced.Attachments.Delete(album, "001.jpg");
                    session.Advanced.Attachments.Delete("albums/1", "002.jpg");

                    session.SaveChanges();
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
                    Album album = await session.LoadAsync<Album>("albums/1");
                    session.Advanced.Attachments.Delete(album, "001.jpg");
                    session.Advanced.Attachments.Delete("albums/1", "002.jpg");

                    await session.SaveChangesAsync();
                }
				#endregion
			}
		}

	    public class Album
	    {
	        public string Id { get; set; }
	        public string Name { get; set; }
	        public string Description { get; set; }
	        public string[] Tags { get; set; }
	    }
    }
}
