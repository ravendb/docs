using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries;
using Raven.Documentation.Samples.Orders;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToProjectQueryResults
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region projections_1
                    // request Name, City and Country for all entities from 'Companies' collection
                    var results = session
                        .Query<Company>()
                        .Select(x => new
                        {
                            Name = x.Name,
                            City = x.Address.City,
                            Country = x.Address.Country
                        })
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_1_async
                    // request Name, City and Country for all entities from 'Companies' collection
                    var results = await asyncSession
                        .Query<Company>()
                        .Select(x => new
                        {
                            Name = x.Name,
                            City = x.Address.City,
                            Country = x.Address.Country
                        })
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_2
                    var results = session
                        .Query<Order>()
                        .Select(x => new
                        {
                            ShipTo = x.ShipTo,
                            Products = x.Lines.Select(y => y.ProductName),
                        })
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_2_async
                    var results = await asyncSession
                        .Query<Order>()
                        .Select(x => new
                        {
                            ShipTo = x.ShipTo,
                            Products = x.Lines.Select(y => y.ProductName),
                        })
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_3
                    var results = (from e in session.Query<Employee>()
                                   select new
                                   {
                                       FullName = e.FirstName + " " + e.LastName,
                                   }).ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_3_async
                    var results = await (from e in asyncSession.Query<Employee>()
                                         select new
                                         {
                                             FullName = e.FirstName + " " + e.LastName,
                                         }).ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_4
                    var results = session
                        .Query<Order>()
                        .Select(x => new
                        {
                            Total = x.Lines.Sum(l => l.PricePerUnit * l.Quantity),
                        })
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_4_async
                    var results = await asyncSession
                        .Query<Order>()
                        .Select(x => new
                        {
                            Total = x.Lines.Sum(l => l.PricePerUnit * l.Quantity),
                        })
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_count_in_projection
                    var results = (from o in session.Query<Order>()
                                    let c = RavenQuery.Load<Company>(o.Company)
                        select new
                        {
                            CompanyName = c.Name,
                            ShippedAt = o.ShippedAt,
                            TotalProducts = o.Lines.Count(), //both empty syntax and with a predicate is supported
                            TotalDiscountedProducts = o.Lines.Count(x => x.Discount > 0)
                        }).ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_5
                    var results = (from o in session.Query<Order>()
                                   let c = RavenQuery.Load<Company>(o.Company)
                                   select new
                                   {
                                       CompanyName = c.Name,
                                       ShippedAt = o.ShippedAt
                                   }).ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_5_async
                    var results = (from o in asyncSession.Query<Order>()
                                   let c = RavenQuery.Load<Company>(o.Company)
                                   select new
                                   {
                                       CompanyName = c.Name,
                                       ShippedAt = o.ShippedAt
                                   }).ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_6
                    var results = session
                        .Query<Employee>()
                        .Select(e => new
                        {
                            DayOfBirth = e.Birthday.Day,
                            MonthOfBirth = e.Birthday.Month,
                            Age = DateTime.Today.Year - e.Birthday.Year
                        })
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_6_async
                    var results = await asyncSession
                        .Query<Employee>()
                        .Select(e => new
                        {
                            DayOfBirth = e.Birthday.Day,
                            MonthOfBirth = e.Birthday.Month,
                            Age = DateTime.Today.Year - e.Birthday.Year
                        })
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_7
                    var results = from e in session.Query<Employee>()
                                  select new
                                  {
                                      Date = RavenQuery.Raw<DateTime>("new Date(Date.parse(e.Birthday))"),
                                      Name = RavenQuery.Raw(e.FirstName, "substr(0,3)"),
                                  };
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_7_async
                    var results = await (from e in asyncSession.Query<Employee>()
                                         select new
                                         {
                                             Date = RavenQuery.Raw<DateTime>("new Date(Date.parse(e.Birthday))"),
                                             Name = RavenQuery.Raw(e.FirstName, "substr(0,3)"),
                                         }).ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_8
                    var results = session.Query<Company, Companies_ByContact>()
                        .ProjectInto<ContactDetails>()
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_8_async
                    var results = await asyncSession.Query<Company, Companies_ByContact>()
                        .ProjectInto<ContactDetails>()
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_10
                    // query index 'Products_BySupplierName' 
                    // return documents from collection 'Products' that have a supplier 'Norske Meierier'
                    // project them to 'Products'
                    List<Product> results = session
                        .Query<Products_BySupplierName.Result, Products_BySupplierName>()
                        .Where(x => x.Name == "Norske Meierier")
                        .OfType<Product>()
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_10_async
                    // query index 'Products_BySupplierName' 
                    // return documents from collection 'Products' that have a supplier 'Norske Meierier'
                    // project them to 'Products'
                    List<Product> results = await asyncSession
                        .Query<Products_BySupplierName.Result, Products_BySupplierName>()
                        .Where(x => x.Name == "Norske Meierier")
                        .OfType<Product>()
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_12
                    var results = (from e in session.Query<Employee>()
                                   let format = (Func<Employee, string>)(p => p.FirstName + " " + p.LastName)
                                   select new
                                   {
                                       FullName = format(e)
                                   }).ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_12_async
                    var results = await (from e in asyncSession.Query<Employee>()
                                         let format = (Func<Employee, string>)(p => p.FirstName + " " + p.LastName)
                                         select new
                                         {
                                             FullName = format(e)
                                         }).ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_13
                    var results = (from e in session.Query<Employee>()
                                   select new
                                   {
                                       Name = e.FirstName,
                                       Metadata = RavenQuery.Metadata(e),
                                   }).ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_13_async
                    var results = await (from e in asyncSession.Query<Employee>()
                                         select new
                                         {
                                             Name = e.FirstName,
                                             Metadata = RavenQuery.Metadata(e),
                                         }).ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region selectFields
                    var fields = new string[]{
                        "Name",
                        "Phone"
                    };

                    var results = session
                        .Advanced
                        .DocumentQuery<Company, Companies_ByContact>()
                        .SelectFields<ContactDetails>(fields)
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region selectFields_async
                    var fields = new string[]{
                        "Name",
                        "Phone"
                    };

                    var results = await asyncSession
                        .Advanced
                        .AsyncDocumentQuery<Company, Companies_ByContact>()
                        .SelectFields<ContactDetails>(fields)
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region selectFields_2
                    var results = session
                        .Advanced
                        .DocumentQuery<Company, Companies_ByContact>()
                        .SelectFields<ContactDetails>()
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region selectFields_2_async
                    var results = await asyncSession
                        .Advanced
                        .AsyncDocumentQuery<Company, Companies_ByContact>()
                        .SelectFields<ContactDetails>()
                        .ToListAsync();
                    #endregion
                }
            }
        }

        #region projections_9
        public class Companies_ByContact : AbstractIndexCreationTask<Company>
        {
            public Companies_ByContact()
            {
                Map = companies => companies
                    .Select(x => new
                    {
                        Name = x.Contact.Name,
                        x.Phone
                    });

                StoreAllFields(FieldStorage.Yes); // Name and Phone fields can be retrieved directly from index
            }
        }
        #endregion

        #region projections_9_class
        public class ContactDetails
        {
            public string Name { get; set; }

            public string Phone { get; set; }
        }
        #endregion

        #region projections_11
        public class Products_BySupplierName : AbstractIndexCreationTask<Product>
        {
            public class Result
            {
                public string Name { get; set; }
            }

            public Products_BySupplierName()
            {
                Map =
                    products =>
                        from product in products
                        let supplier = LoadDocument<Supplier>(product.Supplier)
                        select new
                        {
                            Name = supplier.Name
                        };
            }
        }
        #endregion

    }
}
