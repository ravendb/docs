using System;
using Raven.Client.Documents;
using System.Linq;
using Raven.Client.Documents.Linq;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Session.UpdateDocuments
{
    public class UpdateDocuments
    {
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

        public void UpdateDocumentsSync()
        {
            using (var store = getDocumentStore())
            {
                var baseline = DateTime.Today;

                // Open a session
                using (var session = store.OpenSession())
                {
                    // Use the session to create a document
                    session.Store(new Company
                    {
                        Name = "KitchenAppliances",
                        Address = new Address { PostalCode = "12345" }
                    }, "companies/KitchenAppliances");

                    session.Store(new Company
                    {
                        Name = "ShoeAppliances",
                        Address = new Address { PostalCode = "12345" }
                    }, "companies/ShoeAppliances");

                    session.Store(new Company
                    {
                        Name = "CarAppliances",
                        Address = new Address { PostalCode = "67890" }
                    }, "companies/CarAppliances");

                    session.SaveChanges();
                }

                #region load-company-and-update
                using (var session = store.OpenSession())
                {
                    // Load a company document
                    Company company = session.Load<Company>("companies/1-A");

                    // Update the company's PostalCode
                    company.Address.PostalCode = "TheNewPostalCode";

                    // Apply changes
                    session.SaveChanges();
                }
                #endregion

                #region query-companies-and-update
                using (var session = store.OpenSession())
                {
                    // Query: find companies with the specified PostalCode
                    IRavenQueryable<Company> query = session.Query<Company>()
                        .Where(c => c.Address.PostalCode == "12345");

                    var matchingCompanies = query.ToList();

                    // Update the PostalCode for the resulting company documents
                    for (var i = 0; i < matchingCompanies.Count; i++)
                    {
                        matchingCompanies[i].Address.PostalCode = "TheNewPostalCode";
                    }

                    // Apply changes
                    session.SaveChanges();
                }
                #endregion

            }
        }

        async public void UpdateDocumentsAsync()
        {
            using (var store = getDocumentStore())
            {
                var baseline = DateTime.Today;

                // Open a session
                using (var session = store.OpenSession())
                {
                    // Use the session to create a document
                    session.Store(new Company
                    {
                        Name = "KitchenAppliances",
                        Address = new Address { PostalCode = "12345" }
                    }, "companies/KitchenAppliances");

                    session.Store(new Company
                    {
                        Name = "ShoeAppliances",
                        Address = new Address { PostalCode = "12345" }
                    }, "companies/ShoeAppliances");

                    session.Store(new Company
                    {
                        Name = "CarAppliances",
                        Address = new Address { PostalCode = "67890" }
                    }, "companies/CarAppliances");

                    session.SaveChanges();
                }

                #region load-company-and-update-async
                using (var asyncSession = store.OpenAsyncSession())
                {
                    // Load a document
                    Company company = await asyncSession.LoadAsync<Company>("companies/KitchenAppliances");

                    // Update the company's PostalCode
                    company.Address.PostalCode = "TheNewPostalCode";

                    // Apply changes
                    await asyncSession.SaveChangesAsync();
                }
                #endregion

                #region query-companies-and-update-async
                using (var asyncSession = store.OpenAsyncSession())
                {
                    // Query: find companies with the specified PostalCode
                    IRavenQueryable<Company> query = asyncSession.Query<Company>()
                        .Where(c => c.Address.PostalCode == "12345");

                    var matchingCompanies = await query.ToListAsync();

                    // Update the PostalCode for the resulting company documents
                    for (var i = 0; i < matchingCompanies.Count; i++)
                    {
                        matchingCompanies[i].Address.PostalCode = "TheNewPostalCode";
                    }

                    // Apply changes
                    await asyncSession.SaveChangesAsync();
                }
                #endregion
            }
        }

        public class Company
        {
            public string Name { get; set; }
            public Address Address { get; set; }
        }

        public class Address
        {
            public string PostalCode { get; set; }
        }
    }
}
