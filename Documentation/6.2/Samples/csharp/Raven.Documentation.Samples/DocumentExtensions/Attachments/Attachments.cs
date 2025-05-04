using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Operations.Attachments;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Xunit;

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
            IEnumerator<AttachmentEnumeratorResult> Get(IEnumerable<AttachmentRequest> attachments);
            AttachmentName[] GetNames(object entity);
            AttachmentResult GetRevision(string documentId, string name, string changeVector);
            bool Exists(string documentId, string name);
            #endregion

            #region GetSyntaxAsync
            Task<AttachmentResult> GetAsync(string documentId, string name, CancellationToken token = default);
            Task<AttachmentResult> GetAsync(object entity, string name, CancellationToken token = default);
            Task<IEnumerator<AttachmentEnumeratorResult>> GetAsync(IEnumerable<AttachmentRequest> attachments, CancellationToken token = default);
            Task<AttachmentResult> GetRevisionAsync(string documentId, string name, string changeVector, CancellationToken token = default);
            Task<bool> ExistsAsync(string documentId, string name, CancellationToken token = default);
            #endregion

            #region DeleteSyntax
            void Delete(string documentId, string name);
            void Delete(object entity, string name);
            #endregion

            #region GetRngSyntax
            // Returns a range of the attachment by the document id and attachment name.
            AttachmentResult GetRange(string documentId, string name, long? from, long? to);

            // Returns a range of the attachment by the document id and attachment name.
            AttachmentResult GetRange(object entity, string name, long? from, long? to);
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
                        Tags = new[] { "Holidays Travel", "All Family" },
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
                using (var asyncSession = store.OpenAsyncSession())
                using (var file1 = File.Open("001.jpg", FileMode.Open))
                using (var file2 = File.Open("002.jpg", FileMode.Open))
                using (var file3 = File.Open("003.jpg", FileMode.Open))
                using (var file4 = File.Open("004.mp4", FileMode.Open))
                {
                    var album = new Album
                    {
                        Name = "Holidays",
                        Description = "Holidays travel pictures of the all family",
                        Tags = new[] { "Holidays Travel", "All Family" },
                    };
                    await asyncSession.StoreAsync(album, "albums/1");

                    asyncSession.Advanced.Attachments.Store("albums/1", "001.jpg", file1, "image/jpeg");
                    asyncSession.Advanced.Attachments.Store("albums/1", "002.jpg", file2, "image/jpeg");
                    asyncSession.Advanced.Attachments.Store("albums/1", "003.jpg", file3, "image/jpeg");
                    asyncSession.Advanced.Attachments.Store("albums/1", "004.mp4", file4, "video/mp4");

                    await asyncSession.SaveChangesAsync();
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

                    bool exists = session.Advanced.Attachments.Exists("albums/1", "003.jpg");
                }
                #endregion
            }
        }

        public async Task GetAttachmentAsync()
        {
            using (var store = new DocumentStore())
            {
                #region GetAttachmentAsync
                using (var asyncSession = store.OpenAsyncSession())
                {
                    Album album = await asyncSession.LoadAsync<Album>("albums/1");

                    using (AttachmentResult file1 = await asyncSession.Advanced.Attachments.GetAsync(album, "001.jpg"))
                    using (AttachmentResult file2 = await asyncSession.Advanced.Attachments.GetAsync("albums/1", "002.jpg"))
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

                    AttachmentName[] attachmentNames = asyncSession.Advanced.Attachments.GetNames(album);
                    foreach (AttachmentName attachmentName in attachmentNames)
                    {
                        string name = attachmentName.Name;
                        string contentType = attachmentName.ContentType;
                        string hash = attachmentName.Hash;
                        long size = attachmentName.Size;
                    }

                    bool exists = await asyncSession.Advanced.Attachments.ExistsAsync("albums/1", "003.jpg");
                }
                #endregion
            }
        }

        public void GetRange()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region GetRange
                    Album album = session.Load<Album>("albums/1");

                    AttachmentResult attachmentPart = session.Advanced.Attachments.GetRange(
                                                                album, "track1.mp3", 101, 200);
                    #endregion
                }
            }
        }

        public async Task GetRangeAsync()
        {
            using (var store = new DocumentStore())
            {
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region GetRangeAsync
                    Album album = await asyncSession.LoadAsync<Album>("albums/1");

                    AttachmentResult file1 = await asyncSession.Advanced.Attachments.GetRangeAsync(
                                                                album, "track1.mp3", 101, 200);
                    #endregion
                }
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
                using (var asyncSession = store.OpenAsyncSession())
                {
                    Album album = await asyncSession.LoadAsync<Album>("albums/1");
                    asyncSession.Advanced.Attachments.Delete(album, "001.jpg");
                    asyncSession.Advanced.Attachments.Delete("albums/1", "002.jpg");

                    await asyncSession.SaveChangesAsync();
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


        // attachments multi-get
        // BulkInsert.a few attachments and then get them with a single request
        public async void AttachmentsMultiGet()
        {
            using (var store = getDocumentStore())
            {
                // Create documents to add attachments to
                using (var session = store.OpenSession())
                {
                    var user1 = new User
                    {
                        Name = "Lilly",
                        Age = 20
                    };
                    session.Store(user1);

                    var user2 = new User
                    {
                        Name = "Betty",
                        Age = 25
                    };
                    session.Store(user2);

                    var user3 = new User
                    {
                        Name = "Robert",
                        Age = 29
                    };
                    session.Store(user3);

                    session.SaveChanges();
                }

                List<User> result;

                using (var session = store.OpenSession())
                {
                    IRavenQueryable<User> query = session.Query<User>()
                        .Where(u => u.Age < 30);

                    result = query.ToList();
                }

                // Query for users younger than 30, add an attachment
                using (var bulkInsert = store.BulkInsert())
                {
                    for (var user = 0; user < result.Count; user++)
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes("some contents here");
                        var stream = new MemoryStream(byteArray);

                        string userId = result[user].Id;
                        var attachmentsFor = bulkInsert.AttachmentsFor(userId);

                        for (var attNum = 0; attNum < 10; attNum++)
                        {
                            stream.Position = 0;
                            await attachmentsFor.StoreAsync(result[user].Name + attNum, stream);
                        }

                    }
                }

                // attachments multi-get (sync)
                using (var session = store.OpenSession())
                {
                    for (var userCnt = 0; userCnt < result.Count; userCnt++)
                    {
                        string userId = result[userCnt].Id;
                        #region GetAllAttachments
                        // Load a user profile
                        var user = session.Load<User>(userId);

                        // Get the names of files attached to this document
                        IEnumerable<AttachmentRequest> attachmentNames = session.Advanced.Attachments.GetNames(user).Select(x => new AttachmentRequest(userId, x.Name));

                        // Get the attached files
                        IEnumerator<AttachmentEnumeratorResult> attachmentsEnumerator = session.Advanced.Attachments.Get(attachmentNames);

                        // Go through the document's attachments
                        while (attachmentsEnumerator.MoveNext())
                        {
                            AttachmentEnumeratorResult res = attachmentsEnumerator.Current;

                            AttachmentDetails attachmentDetails = res.Details; // attachment details

                            Stream attachmentStream = res.Stream; // attachment contents

                            // In this case it is a string attachment, that can be decoded back to text 
                            var ms = new MemoryStream();
                            attachmentStream.CopyTo(ms);
                            string decodedStream = Encoding.UTF8.GetString(ms.ToArray());
                        }
                        #endregion
                    }
                }

                // attachments multi-get (Async)
                using (var session = store.OpenAsyncSession())
                {
                    for (var userCnt = 0; userCnt < result.Count; userCnt++)
                    {
                        string userId = result[userCnt].Id;
                        #region GetAllAttachmentsAsync
                        // Load a user profile
                        var user = await session.LoadAsync<User>(userId);

                        // Get the names of files attached to this document
                        IEnumerable<AttachmentRequest> attachmentNames = session.Advanced.Attachments.GetNames(user).Select(x => new AttachmentRequest(userId, x.Name));

                        // Get the attached files
                        IEnumerator<AttachmentEnumeratorResult> attachmentsEnumerator = await session.Advanced.Attachments.GetAsync(attachmentNames);

                        // Go through the document's attachments
                        while (attachmentsEnumerator.MoveNext())
                        {
                            AttachmentEnumeratorResult res = attachmentsEnumerator.Current;

                            AttachmentDetails attachmentDetails = res.Details; // attachment details

                            Stream attachmentStream = res.Stream; // attachment contents

                            // In this case it is a string attachment, that can be decoded back to text 
                            var ms = new MemoryStream();
                            attachmentStream.CopyTo(ms);
                            string decodedStream = Encoding.UTF8.GetString(ms.ToArray());
                        }
                        #endregion
                    }
                }

            }
        }

        public DocumentStore getDocumentStore()
        {
            DocumentStore store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "TestDatabase"
            };
            store.Initialize();

            var parameters = new DeleteDatabasesOperation.Parameters
            {
                DatabaseNames = new[] { "TestDatabase" },
                HardDelete = true,
            };
            store.Maintenance.Server.Send(new DeleteDatabasesOperation(parameters));
            store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord("TestDatabase")));

            return store;
        }

        private class User
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string AddressId { get; set; }
            public int Count { get; set; }
            public int Age { get; set; }
        }
    }
}
