using System;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class ProjectQueryResults
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region projections_1
                    var projectedResults = session
                         // Make a dynamic query on the Companies collection
                        .Query<Company>()
                         // Call Select to define the new structure that will be returned per Company document
                        .Select(x => new
                        {
                            Name = x.Name,
                            City = x.Address.City,
                            Country = x.Address.Country
                        })
                        .ToList();

                    // Each resulting object in the list is Not a 'Company' entity,
                    // it is a new object containing ONLY the fields specified in the Select.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_1_async
                    var projectedResults = await asyncSession
                        // Make a dynamic query on the Companies collection
                        .Query<Company>()
                        // Call Select to define the new structure that will be returned per Company document
                        .Select(x => new {Name = x.Name, City = x.Address.City, Country = x.Address.Country})
                        .ToListAsync();

                    // Each resulting object in the list is Not a 'Company' entity,
                    // it is a new object containing ONLY the fields specified in the Select.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_2
                    var projectedResults = session
                        .Query<Order>()
                        .Select(x => new
                        {
                            ShipTo = x.ShipTo,
                            // Retrieve all product names from the Lines array in an Order document
                            ProductNames = x.Lines.Select(y => y.ProductName)
                        })
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_2_async
                    var projectedResults = await asyncSession
                        .Query<Order>()
                        .Select(x => new
                        {
                            ShipTo = x.ShipTo,
                            // Retrieve all product names from the Lines array in an Order document
                            ProductNames = x.Lines.Select(y => y.ProductName)
                        })
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_3
                    var projectedResults = session
                        .Query<Employee>()
                        .Select(x => new
                        {
                            // Any expression can be provided for the projected content
                            FullName = x.FirstName + " " + x.LastName
                        })
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_3_async
                    var projectedResults = await asyncSession
                        .Query<Employee>()
                        .Select(x => new
                        {
                            // Any expression can be provided for the projected content
                            FullName = x.FirstName + " " + x.LastName
                        })
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_4
                    var projectedResults = session
                        .Query<Order>()
                        .Select(x => new
                        {
                            // Any calculations can be done within a projection
                            TotalProducts = x.Lines.Count,
                            TotalDiscountedProducts = x.Lines.Count(x => x.Discount > 0),
                            TotalPrice = x.Lines.Sum(l => l.PricePerUnit * l.Quantity)
                        })
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_4_async
                    var projectedResults = await asyncSession
                        .Query<Order>()
                        .Select(x => new
                        {
                            // Any calculations can be done within a projection
                            TotalProducts = x.Lines.Count,
                            TotalDiscountedProducts = x.Lines.Count(x => x.Discount > 0),
                            TotalPrice = x.Lines.Sum(l => l.PricePerUnit * l.Quantity)
                        })
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_5
                    // Use LINQ query syntax notation
                    var projectedResults = (from e in session.Query<Employee>()
                        // Define a function
                        let format = (Func<Employee, string>)(p => p.FirstName + " " + p.LastName)
                        select new
                        {
                            // Call the function from the projection
                            FullName = format(e)
                        })
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_5_async
                    // Use LINQ query syntax notation
                    var projectedResults = await (from e in asyncSession.Query<Employee>()
                        // Define a function
                        let format = (Func<Employee, string>)(p => p.FirstName + " " + p.LastName)
                        select new
                        {
                            // Call the function from the projection
                            FullName = format(e)
                        })
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_6
                    // Use LINQ query syntax notation
                    var projectedResults = (from o in session.Query<Order>()
                        // Use RavenQuery.Load to load the related Company document
                        let c = RavenQuery.Load<Company>(o.Company)
                        select new
                        {
                            CompanyName = c.Name, // info from the related Company document
                            ShippedAt = o.ShippedAt // info from the Order document
                        })
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_6_async
                    // Use LINQ query syntax notation
                    var projectedResults = (from o in asyncSession.Query<Order>()
                        // Use RavenQuery.Load to load the related Company document
                        let c = RavenQuery.Load<Company>(o.Company)
                        select new
                        {
                            CompanyName = c.Name, // info from the related Company document
                            ShippedAt = o.ShippedAt // info from the Order document
                        })
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_7
                    var projectedResults = session
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
                    #region projections_7_async
                    var projectedResults = await asyncSession
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
                    #region projections_8
                    var projectedResults = session.Query<Employee>()
                        .Select(e => new
                        {
                            // Provide a JavaScript expression to the RavenQuery.Raw method
                            Date = RavenQuery.Raw<DateTime>("new Date(Date.parse(e.Birthday))"),
                            Name = RavenQuery.Raw(e.FirstName, "substr(0, 3)")
                        })
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_8_async
                    var projectedResults = await asyncSession.Query<Employee>()
                        .Select(e => new
                        {
                            // Provide a JavaScript expression to the RavenQuery.Raw method
                            Date = RavenQuery.Raw<DateTime>("new Date(Date.parse(e.Birthday))"),
                            Name = RavenQuery.Raw(e.FirstName, "substr(0, 3)")
                        })
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_9
                    var projectedResults = session.Query<Employee>()
                        .Select(e => new
                        {
                            Name = e.FirstName, Metadata = RavenQuery.Metadata(e) // Get the metadata
                        })
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_9_async
                    var projectedResults = await asyncSession.Query<Employee>()
                        .Select(e => new
                        {
                            Name = e.FirstName, Metadata = RavenQuery.Metadata(e) // Get the metadata
                        })
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_10
                    var projectedResults = session
                        .Query<Company>()
                        // Call 'ProjectInto' instead of using 'Select'
                        // Pass the projection class
                        .ProjectInto<ContactDetails>()
                        .ToList();

                    // Each resulting object in the list is Not a 'Company' entity,
                    // it is an object of type 'ContactDetails'.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_10_async
                    var projectedResults = await asyncSession
                        .Query<Company>()
                        // Call 'ProjectInto' instead of using 'Select'
                        // Pass the projection class
                        .ProjectInto<ContactDetails>()
                        .ToListAsync();

                    // Each resulting object in the list is Not a 'Company' entity,
                    // it is an object of type 'ContactDetails'.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_11
                    // Make a dynamic DocumentQuery
                    var projectedResults = session.Advanced
                        .DocumentQuery<Company>()
                        // Call 'SelectFields'
                        // Pass the projection class type
                        .SelectFields<ContactDetails>()
                        .ToList();

                    // Each resulting object in the list is Not a 'Company' entity,
                    // it is an object of type 'ContactDetails'.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_11_async
                    // Make a dynamic DocumentQuery
                    var projectedResults = await asyncSession.Advanced
                        .AsyncDocumentQuery<Company>()
                        // Call 'SelectFields'
                        // Pass the projection class type
                        .SelectFields<ContactDetails>()
                        .ToListAsync();

                    // Each resulting object in the list is Not a 'Company' entity,
                    // it is an object of type 'ContactDetails'.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_12
                    // Define an array with the field names that will be projected
                    var projectionFields = new string[]
                    {
                        // Fields from 'ContactDetails' class:
                        "Name", "Phone"
                    };

                    // Make a dynamic DocumentQuery
                    var projectedResults = session.Advanced
                        .DocumentQuery<Company>()
                        // Call 'SelectFields'
                        // Pass the projection class type & the fields to be projected from it
                        .SelectFields<ContactDetails>(projectionFields)
                        .ToList();

                    // Each resulting object in the list is Not a 'Company' entity,
                    // it is an object of type 'ContactDetails' containing data ONLY for the specified fields.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_12_async
                    // Define an array with the field names that will be projected
                    var projectionFields = new string[]
                    {
                        // Fields from 'ContactDetails' class:
                        "Name", "Phone"
                    };

                    // Make a dynamic DocumentQuery
                    var projectedResults = await asyncSession.Advanced
                        .AsyncDocumentQuery<Company>()
                        // Call 'SelectFields'
                        // Pass the projection class type & the fields to be projected from it
                        .SelectFields<ContactDetails>(projectionFields)
                        .ToListAsync();

                    // Each resulting object in the list is Not a 'Company' entity,
                    // it is an object of type 'ContactDetails' containing data ONLY for the specified fields.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_13
                    // For example:
                    try
                    {
                        var projectedResults = session
                            .Query<Company>()
                            // Make first projection
                            .ProjectInto<ContactDetails>()
                            // A second projection is not supported and will throw
                            .Select(x => new {Name = x.Name})
                            .ToList();
                    }
                    catch (Exception e)
                    {
                        // The following exception will be thrown:
                        // "Projection is already done. You should not project your result twice."
                    }
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region projections_14
                    var projectedResults = session
                        .Query<Product>()
                         // NOTE:
                         // While the following 'Include' line compiles,
                         // the related Supplier document will NOT BE INCLUDED in the query results,
                         // because 'Supplier' is not one of the projected fields in the 'Select' clause.
                        .Include(x => x.Supplier)
                        .Select(x => new
                        {
                            Name = x.Name,
                            ProductCategory = x.Category
                        })
                         // The related Category document WILL BE INCLUDED in the query results,
                         // since 'ProductCategory' is one of the projected fields.
                        .Include(x => x.ProductCategory)
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_14_async
                    var projectedResults = await asyncSession
                        .Query<Product>()
                         // NOTE:
                         // While the following 'Include' line compiles,
                         // the related Supplier document will NOT BE INCLUDED in the query results,
                         // because 'Supplier' is not one of the projected fields in the 'Select' clause.
                        .Include(x => x.Supplier)
                        .Select(x => new
                        {
                            Name = x.Name,
                            ProductCategory = x.Category
                        })
                         // The related Category document WILL BE INCLUDED in the query results,
                         // since 'ProductCategory' is one of the projected fields.
                        .Include(x =>x.ProductCategory)
                        .ToListAsync();
                    #endregion
                }
            }
        }

        #region projections_class
        public class ContactDetails
        {
            // The projection class contains field names from the 'Company' document
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Fax { get; set; }
        }
        #endregion
    }
}
