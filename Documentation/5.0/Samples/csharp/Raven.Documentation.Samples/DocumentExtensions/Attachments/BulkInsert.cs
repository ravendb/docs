using System.Linq;
using Raven.Client.Documents;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using Raven.Client.Documents.Linq;
using Raven.Client.ServerWide.Operations;
using Raven.Client.ServerWide;
using System.IO;
using System.Text;

namespace Documentation.Samples.DocumentExtensions.TimeSeries
{
    public class BulkInsertAttachments
    {
        private BulkInsertAttachments(ITestOutputHelper output)
        {
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

        [Fact]
        public async void ReebAppendUsingBulkInsert()
        {
            using (var store = getDocumentStore())
            {
                // Create documents to bulk-insert to
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

                #region bulk-insert-attachment
                List<User> result;

                // Choose user profiles to attach files to
                using (var session = store.OpenSession())
                {
                    IRavenQueryable<User> query = session.Query<User>()
                        .Where(u => u.Age < 30);

                    result = query.ToList();
                }

                // Bulk-insert an attachment to the chosen users
                using (var bulkInsert = store.BulkInsert())
                {
                    for (var user = 0; user < result.Count; user++)
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes("some contents here");
                        var stream = new MemoryStream(byteArray);

                        string userId = result[user].Id;
                        
                        // Choose the document to attach to
                        var attachmentsFor = bulkInsert.AttachmentsFor(userId);

                        // Attach the stream
                        await attachmentsFor.StoreAsync("attName", stream);
                    }
                }
                #endregion
            }
        }

        public class User
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string AddressId { get; set; }
            public int Count { get; set; }
            public int Age { get; set; }
        }

        #region AttachmentsFor-definition
        public AttachmentsBulkInsert AttachmentsFor(string id)
        #endregion
        {
            return new AttachmentsBulkInsert();
        }

        public struct AttachmentsBulkInsert
        {
        }

        #region AttachmentsFor.Store-definition
        public void Store(string name, Stream stream, string contentType = null)
        #endregion
        {
        }

    }
}

