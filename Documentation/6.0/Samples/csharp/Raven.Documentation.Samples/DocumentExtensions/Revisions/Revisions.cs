using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.Documents.Operations.Revisions;
using Raven.Client.Documents.Session;
using Raven.Client.Json;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Server
{
    public class Revisions
    {
        private class User
        {
            public string Name { get; set; }
        }

        private class Loan
        {
            public string Id { get; set; }
        }

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                #region configuration
                await store.Maintenance.SendAsync(new ConfigureRevisionsOperation(new RevisionsConfiguration
                {
                    Default = new RevisionsCollectionConfiguration
                    {
                        Disabled = false,
                        PurgeOnDelete = false,
                        MinimumRevisionsToKeep = 5,
                        MinimumRevisionAgeToKeep = TimeSpan.FromDays(14),
                    },
                    Collections = new Dictionary<string, RevisionsCollectionConfiguration>
                    {
                        {"Users", new RevisionsCollectionConfiguration {Disabled = true}},
                        {"Orders", new RevisionsCollectionConfiguration {Disabled = false}}
                    }
                }));
                #endregion

                #region store
                using (var session = store.OpenSession())
                {
                    session.Store(new User
                    {
                        Name = "Ayende Rahien"
                    });

                    session.SaveChanges();
                }
                #endregion

                Loan loan = new Loan { Id = "loans/1" };

                using (var session = store.OpenSession())
                {
                    #region get_revisions
                    // Get all the revisions that were created for a document, by document ID
                    List<User> revisions = session
                        .Advanced
                        .Revisions
                        .GetFor<User>("users/1", start: 0, pageSize: 25);

                    // Get revisions metadata 
                    List<IMetadataDictionary> revisionsMetadata = session
                        .Advanced
                        .Revisions
                        .GetMetadataFor("users/1", start: 0, pageSize: 25);

                    // Get revisions by their change vectors
                    User revison = session
                        .Advanced
                        .Revisions
                        .Get<User>(revisionsMetadata[0].GetString(Constants.Documents.Metadata.ChangeVector));

                    // Get a revision by its creation time
                    // If no revision was created at that precise time, get the first revision to precede it
                    User revisonAtYearAgo = session
                        .Advanced
                        .Revisions
                        .Get<User>("users/1", DateTime.Now.AddYears(-1));
                    #endregion
                }
            }
        }

        public class Company
        {
            public string Id { get; set; }
            public string ExternalId { get; set; }
            public string Name { get; set; }
            public Contact Contact { get; set; }
            public Address Address { get; set; }
            public string Phone { get; set; }
            public string Fax { get; set; }
        }

    }
}
