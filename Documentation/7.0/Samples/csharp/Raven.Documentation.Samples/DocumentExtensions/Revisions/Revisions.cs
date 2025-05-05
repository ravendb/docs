using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Revisions;
using Raven.Client.Documents.Session;
using Raven.Client.Json;
using Raven.Documentation.Samples.Orders;
using Xunit;

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
                    User revision = session
                        .Advanced
                        .Revisions
                        .Get<User>(revisionsMetadata[0].GetString(Constants.Documents.Metadata.ChangeVector));

                    // Get a revision by its creation time
                    // If no revision was created at that precise time, get the first revision to precede it
                    User revisionAtYearAgo = session
                        .Advanced
                        .Revisions
                        .Get<User>("users/1", DateTime.Now.AddYears(-1));
                    #endregion
                }
            }
        }

        public async Task ForceRevisionCreationForSample()
        {
            using (var store = new DocumentStore())
            {
                string companyId;
                using (var session = store.OpenSession())
                {
                    #region ForceRevisionCreationByEntity
                    // Force revision creation by entity
                    // =================================
                    
                    var company = new Company { 
                            Name = "CompanyName" 
                        };
                    
                    session.Store(company);
                    companyId = company.Id;
                    session.SaveChanges();

                    // Forcing the creation of a revision by entity can be performed 
                    // only when the entity is tracked, after the document is stored.
                    session.Advanced.Revisions.ForceRevisionCreationFor<Company>(company);
                    
                    // Call SaveChanges for the revision to be created
                    session.SaveChanges();
                    
                    var revisionsCount = session.Advanced.Revisions.GetFor<Company>(companyId).Count;
                    Assert.Equal(1, revisionsCount);
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region ForceRevisionCreationByID
                    // Force revision creation by ID
                    // =============================
                    
                    session.Advanced.Revisions.ForceRevisionCreationFor(companyId);
                    session.SaveChanges();

                    var revisionsCount = session.Advanced.Revisions.GetFor<Company>(companyId).Count;
                    Assert.Equal(1, revisionsCount);
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
        
        private interface IFoo
        {
            #region syntax_1
            // Available overloads:
            // ====================

            // Force revision creation by entity.
            // Can be used with tracked entities only.
            void ForceRevisionCreationFor<T>(T entity,
                ForceRevisionStrategy strategy = ForceRevisionStrategy.Before);

            // Force revision creation by document ID.
            void ForceRevisionCreationFor(string id,
                ForceRevisionStrategy strategy = ForceRevisionStrategy.Before);
            #endregion
            
            #region syntax_2
            public enum ForceRevisionStrategy
            {
                // Do not force a revision
                None,
                
                // Create a forced revision from the document currently in store
                // BEFORE applying any changes made by the user.
                // The only exception is for a new document,
                // where a revision will be created AFTER the update.
                Before
            }
            #endregion
        }
    }
}
