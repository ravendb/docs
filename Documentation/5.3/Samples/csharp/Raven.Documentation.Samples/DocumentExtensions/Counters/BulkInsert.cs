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
    public class BulkInsertCounters
    {
        private BulkInsertCounters(ITestOutputHelper output)
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

        // bulk insert Counters
        // Use BulkInsert.TimeSeriesBulkInsert.Append with doubles
        [Fact]
        public async void BulkInsertCounters2()
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

                List<User> result;

                #region bulk-insert-counters
                // Choose user profiles to add counters to
                using (var session = store.OpenSession())
                {
                    IRavenQueryable<User> query = session.Query<User>()
                        .Where(u => u.Age < 30);

                    result = query.ToList();
                }

                using (var bulkInsert = store.BulkInsert())
                {
                    for (var user = 0; user < result.Count; user++)
                    {
                        string userId = result[user].Id;

                        // Choose document
                        var countersFor = bulkInsert.CountersFor(userId);

                        // Add or Increment a counter
                        await bulkInsert.CountersFor(userId).IncrementAsync("downloaded", 100);
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

        #region CountersFor-definition
        public CountersBulkInsert CountersFor(string id)
        #endregion
        {
            return new CountersBulkInsert();
        }

        public struct CountersBulkInsert
        {
        }

        #region Increment-definition
        public void Increment(string name, long delta = 1L)
        #endregion
        {
        }

    }
}

