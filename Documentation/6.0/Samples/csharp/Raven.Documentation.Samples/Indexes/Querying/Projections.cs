using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Linq;
using Raven.Documentation.Samples.Orders;
using Raven.Client.Documents.Session;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class Projections
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region projections_1
                    var projectedResults = session
                         // Query the index
                        .Query<Employees_ByNameAndTitle.IndexEntry, Employees_ByNameAndTitle>()
                         // Can filter by index-field, e.g. query only for sales representatives
                        .Where(x => x.Title == "sales representative")
                         // Call 'Select' to return only the first and last name per matching document
                        .Select(x => new
                        {
                            EmployeeFirstName = x.FirstName,
                            EmployeeLastName = x.LastName
                        })
                        .ToList();
                    
                    // Each resulting object in the list is Not an 'Employee' entity,
                    // it is a new object containing ONLY the fields specified in the Select
                    // ('EmployeeFirstName' & 'EmployeeLastName').
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_1_async
                    var projectedResults = await asyncSession
                         // Query the index
                        .Query<Employees_ByNameAndTitle.IndexEntry, Employees_ByNameAndTitle>()
                         // Can filter by index-field, e.g. query only for sales representatives
                        .Where(x => x.Title == "sales representative")
                         // Call 'Select' to return only the first and last name per matching document
                        .Select(x => new 
                        {
                            EmployeeFirstName = x.FirstName,
                            EmployeeLastName = x.LastName
                        })
                        .ToListAsync();
                    
                    // Each resulting object in the list is Not an 'Employee' entity,
                    // it is a new object containing ONLY the fields specified in the Select
                    // ('EmployeeFirstName' & 'EmployeeLastName').
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region projections_1_stored
                    var projectedResults = session
                        .Query<Employees_ByNameAndTitleWithStoredFields.IndexEntry,
                            Employees_ByNameAndTitleWithStoredFields>()
                        .Select(x => new
                        {
                            // Project fields 'FirstName' and 'LastName' which are stored in the index
                            EmployeeFirstName = x.FirstName,
                            EmployeeLastName = x.LastName
                        })
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_1_stored_async
                    var projectedResults = await asyncSession
                        .Query<Employees_ByNameAndTitleWithStoredFields.IndexEntry,
                            Employees_ByNameAndTitleWithStoredFields>()
                        .Select(x => new
                        {
                            // Project fields 'FirstName' and 'LastName' which are stored in the index
                            EmployeeFirstName = x.FirstName,
                            EmployeeLastName = x.LastName
                        })
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region projections_2
                    var projectedResults = session
                        .Query<Orders_ByCompanyAndShipToAndLines.IndexEntry, Orders_ByCompanyAndShipToAndLines>()
                        .Where(x => x.Company == "companies/65-A")
                        .Select(x => new
                        {
                            // Retrieve a property from an object
                            ShipToCity = x.ShipTo.City,
                            // Retrieve all product names from the Lines array
                            Products = x.Lines.Select(y => y.ProductName)
                        })
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_2_async
                    var projectedResults = await asyncSession
                        .Query<Orders_ByCompanyAndShipToAndLines.IndexEntry, Orders_ByCompanyAndShipToAndLines>()
                        .Where(x => x.Company == "companies/65-A")
                        .Select(x => new
                        {
                            // Retrieve a property from an object
                            ShipToCity = x.ShipTo.City,
                            // Retrieve all product names from the Lines array
                            Products = x.Lines.Select(y => y.ProductName)
                        })
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region projections_3
                    var projectedResults = session
                        .Query<Employees_ByNameAndTitle.IndexEntry, Employees_ByNameAndTitle>()
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
                        .Query<Employees_ByNameAndTitle.IndexEntry, Employees_ByNameAndTitle>()
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
                        .Query<Orders_ByCompanyAndShipToAndLines.IndexEntry, Orders_ByCompanyAndShipToAndLines>()
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
                
                using (var asyncSession = store.OpenSession())
                {
                    #region projections_4_async
                    var projectedResults = await asyncSession
                        .Query<Orders_ByCompanyAndShipToAndLines.IndexEntry, Orders_ByCompanyAndShipToAndLines>()
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
                    var projectedResults =
                        // Use LINQ query syntax notation
                        (from x in session
                                .Query<Employees_ByNameAndTitle.IndexEntry, Employees_ByNameAndTitle>()
                            // Define a function
                            let format =
                                (Func<Employees_ByNameAndTitle.IndexEntry, string>)(p =>
                                    p.FirstName + " " + p.LastName)
                            select new
                            {
                                // Call the function from the projection
                                FullName = format(x)
                            })
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_5_async
                    var projectedResults =
                        // Use LINQ query syntax notation
                        await (from x in asyncSession
                                    .Query<Employees_ByNameAndTitle.IndexEntry, Employees_ByNameAndTitle>()
                                // Define a function
                                let format =
                                    (Func<Employees_ByNameAndTitle.IndexEntry, string>)(p =>
                                        p.FirstName + " " + p.LastName)
                                select new
                                {
                                    // Call the function from the projection
                                    FullName = format(x)
                                })
                            .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region projections_6
                    var projectedResults = 
                        // Use LINQ query syntax notation
                        (from o in session
                                .Query<Orders_ByCompanyAndShippedAt.IndexEntry, Orders_ByCompanyAndShippedAt>()
                            // Use RavenQuery.Load to load the related Company document
                            let c = RavenQuery.Load<Company>(o.Company)
                            select new
                            {
                                CompanyName = c.Name,   // info from the related Company document
                                ShippedAt = o.ShippedAt // info from the Order document
                            })
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_6_async
                    // Use LINQ query syntax notation
                    var projectedResults = 
                        await (from o in asyncSession
                                    .Query<Orders_ByCompanyAndShippedAt.IndexEntry, Orders_ByCompanyAndShippedAt>()
                            // Use RavenQuery.Load to load the related Company document
                            let c = RavenQuery.Load<Company>(o.Company)
                            select new
                            {
                                CompanyName = c.Name,   // info from the related Company document
                                ShippedAt = o.ShippedAt // info from the Order document
                            })
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region projections_7
                    var projectedResults = session
                        .Query<Employees_ByFirstNameAndBirthday.IndexEntry, Employees_ByFirstNameAndBirthday>()
                        .Select(x => new
                        {
                            DayOfBirth = x.Birthday.Day,
                            MonthOfBirth = x.Birthday.Month,
                            Age = DateTime.Today.Year - x.Birthday.Year
                        })
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_7_async
                    var projectedResults = await asyncSession
                        .Query<Employees_ByFirstNameAndBirthday.IndexEntry, Employees_ByFirstNameAndBirthday>()
                        .Select(x => new
                        {
                            DayOfBirth = x.Birthday.Day,
                            MonthOfBirth = x.Birthday.Month,
                            Age = DateTime.Today.Year - x.Birthday.Year
                        })
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_8
                    var projectedResults = session
                        .Query<Employees_ByFirstNameAndBirthday.IndexEntry, Employees_ByFirstNameAndBirthday>()
                        .Select(x => new
                        {
                            // Provide a JavaScript expression to the RavenQuery.Raw method
                            Date = RavenQuery.Raw<DateTime>("new Date(Date.parse(x.Birthday))"),
                            Name = RavenQuery.Raw(x.FirstName, "substr(0,3)")
                        })
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_8_async
                    var projectedResults = await asyncSession
                        .Query<Employees_ByFirstNameAndBirthday.IndexEntry, Employees_ByFirstNameAndBirthday>()
                        .Select(x => new
                        {
                            // Provide a JavaScript expression to the RavenQuery.Raw method
                            Date = RavenQuery.Raw<DateTime>("new Date(Date.parse(x.Birthday))"),
                            Name = RavenQuery.Raw(x.FirstName, "substr(0,3)")
                        })
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_9
                    var projectedResults = session
                        .Query<Employees_ByFirstNameAndBirthday.IndexEntry, Employees_ByFirstNameAndBirthday>()
                        .Select(x => new
                        {
                            Name = x.FirstName,
                            Metadata = RavenQuery.Metadata(x) // Get the metadata
                        })
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_9_async
                    var projectedResults = await asyncSession
                        .Query<Employees_ByFirstNameAndBirthday.IndexEntry, Employees_ByFirstNameAndBirthday>()
                        .Select(x => new
                        {
                            Name = x.FirstName,
                            Metadata = RavenQuery.Metadata(x) // Get the metadata
                        })
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_10
                    var projectedResults = session
                        .Query<Companies_ByContactDetailsAndPhone.IndexEntry, Companies_ByContactDetailsAndPhone>()
                        .Where(x => x.ContactTitle == "owner")
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
                        .Query<Companies_ByContactDetailsAndPhone.IndexEntry, Companies_ByContactDetailsAndPhone>()
                        .Where(x => x.ContactTitle == "owner")
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
                    // Query an index with DocumentQuery
                    var projectedResults = session.Advanced
                        .DocumentQuery<Products_ByNamePriceQuantityAndUnits.IndexEntry,
                            Products_ByNamePriceQuantityAndUnits>()
                         // Call 'SelectFields'
                         // Pass the projection class type
                        .SelectFields<ProductDetails>()
                        .ToList();
                    
                    // Each resulting object in the list is Not a 'Product' entity,
                    // it is an object of type 'ProductDetails'.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_11_async
                    // Query an index with DocumentQuery
                    var projectedResults = await asyncSession.Advanced
                        .AsyncDocumentQuery<Products_ByNamePriceQuantityAndUnits.IndexEntry, 
                            Products_ByNamePriceQuantityAndUnits>()
                         // Call 'SelectFields'
                         // Pass the projection class type
                        .SelectFields<ProductDetails>()
                        .ToListAsync();
                    
                    // Each resulting object in the list is Not a 'Product' entity,
                    // it is an object of type 'ProductDetails'.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region projections_12
                    // Define an array with the field names that will be projected
                    var fields = new string[] {
                        "ProductName",
                        "PricePerUnit"
                    };
                    
                    // Query an index with DocumentQuery
                    var projectedResults = session.Advanced
                        .DocumentQuery<Companies_ByContactDetailsAndPhone.IndexEntry,
                            Companies_ByContactDetailsAndPhone>()
                         // Call 'SelectFields'
                         // Pass the projection class type & the fields to be projected from it
                        .SelectFields<ProductDetails>(fields)
                        .ToList();
                    
                    // Each resulting object in the list is Not a 'Product' entity,
                    // it is an object of type 'ProductDetails' containing data ONLY for the specified fields.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_12_async
                    // Define an array with the field names that will be projected
                    var fields = new string[] {
                        "ProductName",
                        "PricePerUnit"
                    };
                    
                    // Query an index with DocumentQuery
                    var projectedResults = await asyncSession.Advanced
                        .AsyncDocumentQuery<Companies_ByContactDetailsAndPhone.IndexEntry,
                            Companies_ByContactDetailsAndPhone>()
                         // Call 'SelectFields'
                         // Pass the projection class type & the fields to be projected from it
                        .SelectFields<ProductDetails>(fields)
                        .ToListAsync();
                    
                    // Each resulting object in the list is Not a 'Product' entity,
                    // it is an object of type 'ProductDetails' containing data ONLY for the specified fields.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region projections_13_1
                    var projectedResults = session
                        .Query<Employees_ByNameAndTitleWithStoredFields.IndexEntry,
                            Employees_ByNameAndTitleWithStoredFields>()
                         // Call 'Customize'
                         // Pass the requested projection behavior to the 'Projection' method
                        .Customize(x => x.Projection(ProjectionBehavior.FromIndexOrThrow))
                         // Select the fields that will be returned by the projection
                        .Select(x => new EmployeeDetails
                        {
                            FirstName = x.FirstName,
                            Title = x.Title
                        })
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region projections_13_2
                    var projectedResults = session.Advanced
                        .DocumentQuery<Employees_ByNameAndTitleWithStoredFields.IndexEntry,
                            Employees_ByNameAndTitleWithStoredFields>()
                         // Pass the requested projection behavior to the 'SelectFields' method
                         // and specify the field that will be returned by the projection
                        .SelectFields<EmployeeDetails>(ProjectionBehavior.FromIndexOrThrow)
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region projections_13_3
                    var projectedResults = session.Advanced
                        // Define an RQL query that returns a projection
                        .RawQuery<EmployeeDetails>(
                            @"from index 'Employees/ByNameAndTitleWithStoredFields' select FirstName, Title")
                        // Pass the requested projection behavior to the 'Projection' method
                        .Projection(ProjectionBehavior.FromIndexOrThrow)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_14
                    List<Company> results = session
                        .Query<Companies_ByContactDetailsAndPhone.IndexEntry, Companies_ByContactDetailsAndPhone>()
                         // Here we filter by an IndexEntry field,
                         // The compiler recognizes 'x' as an IndexEntry type
                        .Where(x => x.ContactTitle == "owner")
                         // Here we let the compiler know that results are of type 'Company' documents
                        .OfType<Company>()
                        .ToList();
                    #endregion
                }
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region projections_14_async
                    List<Company> results = await asyncSession
                        .Query<Companies_ByContactDetailsAndPhone.IndexEntry, Companies_ByContactDetailsAndPhone>()
                         // Here we filter by an IndexEntry field,
                         // The compiler recognizes 'x' as an IndexEntry type
                        .Where(x => x.ContactTitle == "owner")
                         // Here we let the compiler know that results are of type 'Company' documents
                        .OfType<Company>()
                        .ToListAsync();
                    #endregion
                }
            }
        }
        
        private interface IFoo
        {
            #region projection_behavior syntax
            // For Query:
            IDocumentQueryCustomization Projection(ProjectionBehavior projectionBehavior);

            // For DocumentQuery:
            IDocumentQuery<TProjection> SelectFields<TProjection>(
                ProjectionBehavior projectionBehavior, params string[] fields);

            IDocumentQuery<TProjection> SelectFields<TProjection>(
                ProjectionBehavior projectionBehavior);

            // Projection behavior options:
            public enum ProjectionBehavior {
                Default,
                FromIndex,
                FromIndexOrThrow,
                FromDocument,
                FromDocumentOrThrow
            }
            #endregion
        }
    }

    #region index_1
    public class Employees_ByNameAndTitle : AbstractIndexCreationTask<Employee>
    {
        public class IndexEntry
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Title { get; set; }
        }
        
        public Employees_ByNameAndTitle()
        {
            Map = employees => from employee in employees
                
                select new IndexEntry
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Title = employee.Title
                };
        }
    }
    #endregion

    #region index_1_stored
    public class Employees_ByNameAndTitleWithStoredFields : AbstractIndexCreationTask<Employee>
    {    
        public class IndexEntry
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Title { get; set; }
        }
        
        public Employees_ByNameAndTitleWithStoredFields()
        {
            Map = employees => from employee in employees
                select new IndexEntry
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Title = employee.Title
                };
            
            // Store some fields in the index:
            Stores.Add(x => x.FirstName, FieldStorage.Yes);
            Stores.Add(x => x.LastName, FieldStorage.Yes);
        }
    }
    #endregion
    
    #region projections_class_1
    public class EmployeeDetails
    {
        public string FirstName { get; set; } 
        public string Title { get; set; }
    }
    #endregion
    
    #region index_2
    public class Orders_ByCompanyAndShipToAndLines : AbstractIndexCreationTask<Order>
    {
        public class IndexEntry
        {
            public string Company { get; set; }
            public Address ShipTo { get; set; }
            public List<OrderLine> Lines { get; set; }
        }
        
        public Orders_ByCompanyAndShipToAndLines()
        {
            Map = orders => from order in orders
                select new IndexEntry
                {
                    Company = order.Company,
                    ShipTo = order.ShipTo,
                    Lines = order.Lines
                };
        }
    }
    
    // public class Address
    // {
    //     public string Line1 { get; set; }
    //     public string Line2 { get; set; }
    //     public string City { get; set; }
    //     public string Region { get; set; }
    //     public string PostalCode { get; set; }
    //     public string Country { get; set; }
    //     public Location Location { get; set; }
    // }

    // public class OrderLine
    // {
    //     public string Product { get; set; }
    //     public string ProductName { get; set; }
    //     public decimal PricePerUnit { get; set; }
    //     public int Quantity { get; set; }
    //     public decimal Discount { get; set; }
    // }
    #endregion
    
    #region index_3
    public class Orders_ByCompanyAndShippedAt : AbstractIndexCreationTask<Order>
    {
        public class IndexEntry
        {
            public string Company { get; set; }
            public DateTime? ShippedAt { get; set; }
        }
        
        public Orders_ByCompanyAndShippedAt()
        {
            Map = orders => from order in orders
                
                select new IndexEntry
                {
                    Company = order.Company,
                    ShippedAt = order.ShippedAt
                };
        }
    }
    #endregion
    
    #region index_4
    public class Employees_ByFirstNameAndBirthday : AbstractIndexCreationTask<Employee>
    {
        public class IndexEntry
        {
            public string FirstName { get; set; }
            public DateTime Birthday { get; set; }
        }
        
        public Employees_ByFirstNameAndBirthday()
        {
            Map = employees => from employee in employees
                
                select new IndexEntry
                {
                    FirstName = employee.FirstName,
                    Birthday = employee.Birthday
                };
        }
    }
    #endregion
    
    #region index_5
    public class Companies_ByContactDetailsAndPhone : AbstractIndexCreationTask<Company>
    {
        public class IndexEntry
        {
            public string ContactName { get; set; }
            public string ContactTitle { get; set; }
            public string Phone { get; set; }
        }
        
        public Companies_ByContactDetailsAndPhone()
        {
            Map = companies => companies
                .Select(x => new IndexEntry
                {
                    ContactName = x.Contact.Name,
                    ContactTitle = x.Contact.Title,
                    Phone = x.Phone
                });
        }
    }
    #endregion

    #region projections_class_5
    public class ContactDetails
    {
        // The projection class contains field names from the index-fields
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
    }
    #endregion
    
    #region index_6
    public class Products_ByNamePriceQuantityAndUnits : AbstractIndexCreationTask<Product>
    {
        public class IndexEntry
        {
            public string ProductName { get; set; }
            public string QuantityPerUnit { get; set; }
            public decimal PricePerUnit { get; set; }
            public int UnitsInStock { get; set; }
            public int UnitsOnOrder { get; set; }
        }
        
        public Products_ByNamePriceQuantityAndUnits()
        {
            Map = products => from product in products
                
                select new IndexEntry
                {
                    ProductName = product.Name,
                    QuantityPerUnit = product.QuantityPerUnit,
                    PricePerUnit = product.PricePerUnit,
                    UnitsInStock = product.UnitsInStock,
                    UnitsOnOrder = product.UnitsOnOrder
                };
        }
    }
    #endregion
    
    #region projections_class_6
    public class ProductDetails
    {
        // The projection class contains field names from the index-fields
        public string ProductName { get; set; }
        public decimal PricePerUnit { get; set; }
        public int UnitsInStock { get; set; }
    }
    #endregion
}

