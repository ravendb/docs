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
        public async void AppendUsingBulkInsert()
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

                #region bulk_insert_attachment
                List<User> users;

                // Choose user profiles for which to attach a file
                using (var session = store.OpenSession())
                {
                    users = session.Query<User>()
                        .Where(u => u.Age < 30)
                        .ToList();
                }

                // Prepare content to attach
                byte[] byteArray = Encoding.UTF8.GetBytes("some contents here");
                var stream = new MemoryStream(byteArray);
                
                // Create a BulkInsert instance
                using (var bulkInsert = store.BulkInsert())
                {
                    for (var i = 0; i < users.Count; i++)
                    {
                        string userId = users[i].Id;
                        
                        // Call AttachmentsFor, pass the document ID for which to attach the file
                        var attachmentsBulkInsert = bulkInsert.AttachmentsFor(userId);

                        // Call Store to add the file stream to the BulkInsert instance
                        attachmentsBulkInsert.Store("AttachmentName", stream);
                    }
                }
                #endregion
            }
        }

        #region user_class
        public class User
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string AddressId { get; set; }
            public int Count { get; set; }
            public int Age { get; set; }
        }
        #endregion

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

