
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Session.Loaders;
using Raven.Client.Util;
using Raven.Documentation.Samples.Orders;

using System.Linq;
using Raven.Client.Documents.Linq;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Session.UpdatingEntities
{
    public class UpdatingEntities
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

        public void updateEntitiesSyns()
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
                        Type = Company.CompanyType.Public
                    }, "companies/KitchenAppliances");

                    session.Store(new Company
                    {
                        Name = "ShoeAppliances",
                        Type = Company.CompanyType.Public
                    }, "companies/ShoeAppliances");

                    session.Store(new Company
                    {
                        Name = "CarAppliances",
                        Type = Company.CompanyType.Public
                    }, "companies/CarAppliances");

                    session.SaveChanges();
                }

                #region load-company-and-update-its-type-sync
                using (var session = store.OpenSession())
                {
                    // Load a document
                    Company company = session.Load<Company>("companies/KitchenAppliances");

                    // Update the company's type
                    company.Type = Company.CompanyType.Private;

                    // Apply changes
                    session.SaveChanges();
                }
                #endregion

                #region query-companies-and-update-their-type-sync
                using (var session = store.OpenSession())
                {
                    // Query: find public companies
                    IRavenQueryable<Company> query = session.Query<Company>()
                        .Where(c => c.Type == Company.CompanyType.Public);

                    var result = query.ToList();

                    // Change company type from public to private
                    for (var l = 0; l < result.Count; l++)
                    {
                        result[l].Type = Company.CompanyType.Private;
                    }

                    // Apply changes
                    session.SaveChanges();
                }
                #endregion

            }
        }

        async public void updateEntitiesAsyns()
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
                        Type = Company.CompanyType.Public
                    }, "companies/KitchenAppliances");

                    session.Store(new Company
                    {
                        Name = "ShoeAppliances",
                        Type = Company.CompanyType.Public
                    }, "companies/ShoeAppliances");

                    session.Store(new Company
                    {
                        Name = "CarAppliances",
                        Type = Company.CompanyType.Public
                    }, "companies/CarAppliances");

                    session.SaveChanges();
                }

                #region load-company-and-update-its-type-async
                using (var asyncSession = store.OpenAsyncSession())
                {
                    // Load a document
                    Company company = await asyncSession.LoadAsync<Company>("companies/KitchenAppliances");

                    // Update the company's type
                    company.Type = Company.CompanyType.Private;

                    // Apply changes
                    await asyncSession.SaveChangesAsync();
                }
                #endregion

                #region query-companies-and-update-their-type-async
                using (var asyncSession = store.OpenAsyncSession())
                {
                    // Query: find public companies
                    IRavenQueryable<Company> query = asyncSession.Query<Company>()
                        .Where(c => c.Type == Company.CompanyType.Public);

                    var result = await query.ToListAsync();

                    // Change company type from public to private
                    for (var l = 0; l < result.Count; l++)
                    {
                        result[l].Type = Company.CompanyType.Private;
                    }

                    // Apply changes
                    await asyncSession.SaveChangesAsync();
                }
                #endregion

            }
        }


        private class Company
        {
            public decimal AccountsReceivable { get; set; }
            public string Id { get; set; }
            public string Name { get; set; }
            public string Desc { get; set; }
            public string Email { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string Address3 { get; set; }
            public List<Contact> Contacts { get; set; }
            public int Phone { get; set; }
            public CompanyType Type { get; set; }
            public List<string> EmployeesIds { get; set; }

            public enum CompanyType
            {
                Public,
                Private
            }
        }
    }
}
