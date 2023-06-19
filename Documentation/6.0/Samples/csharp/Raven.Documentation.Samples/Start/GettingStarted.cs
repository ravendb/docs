using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using Xunit;

namespace Raven.Documentation.Samples.Start
{
    public class GettingStarted
    {
        public void T1()
        {
            #region start_1
            using (IDocumentStore store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080/" }, // server URL
                Database = "Northwind"   // default database
            })
            {
                store.Initialize(); // initializes document store, by connecting to server and downloading various configurations

                using (IDocumentSession session = store.OpenSession()) // opens a session that will work in context of 'DefaultDatabase'
                {
                    Employee employee = new Employee
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    };

                    session.Store(employee); // stores employee in session, assigning it to a collection `Employees`
                    string employeeId = employee.Id; // Session.Store will assign Id to employee, if it is not set

                    session.SaveChanges(); // sends all changes to server

                    // Session implements Unit of Work pattern,
                    // therefore employee instance would be the same and no server call will be made
                    Employee loadedEmployee = session.Load<Employee>(employeeId);
                    Assert.Equal(employee, loadedEmployee);
                }
            }
            #endregion
        }

        #region start_2
        /// <summary>
        /// All _ in index class names will be converted to /
        /// it means that Employees_ByFirstNameAndLastName will be Employees/ByFirstNameAndLastName
        /// when deployed to server
        /// 
        /// AbstractIndexCreationTask is a helper class that gives you strongly-typed syntax
        /// for creating indexes
        /// </summary>
        public class Employees_ByFirstNameAndLastName : AbstractIndexCreationTask<Employee>
        {
            public Employees_ByFirstNameAndLastName()
            {
                // this is a simple (Map) index LINQ-flavored mapping function
                // that enables searching of Employees by
                // FirstName, LastName (or both)
                Map = employees => from employee in employees
                                   select new
                                   {
                                       FirstName = employee.FirstName,
                                       LastName = employee.LastName
                                   };
            }
        }
        #endregion

        public void T2()
        {
            #region start_2
            using (IDocumentStore store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080/" }, // server URL
                Database = "Northwind"   // default database
            })
            {
                store.Initialize(); // initializes document store, by connecting to server and downloading various configurations

                new Employees_ByFirstNameAndLastName().Execute(store); // deploying index to server

                using (IDocumentSession session = store.OpenSession()) // opens a session that will work in context of 'DefaultDatabase'
                {
                    List<Employee> employees = session.Query<Employee, Employees_ByFirstNameAndLastName>() // returning object of type Employee, using Employees/ByFirstNameAndLastName index
                        .Where(x => x.FirstName == "Robert") // predicates (can only use fields that index defines, so FirstName, LastName or both)
                        .ToList(); // materializing query - sending to server
                }
            }
            #endregion
        }

        public void Sample()
        {
            #region client_1
            using (IDocumentStore store = new DocumentStore
            {
                Urls = new[]                        // URL to the Server,
                {                                   // or list of URLs 
                    "http://live-test.ravendb.net"  // to all Cluster Servers (Nodes)
                },
                Database = "Northwind",             // Default database that DocumentStore will interact with
                Conventions = { }                   // DocumentStore customizations
            })
            {
                store.Initialize();                 // Each DocumentStore needs to be initialized before use.
                                                    // This process establishes the connection with the Server
                                                    // and downloads various configurations
                                                    // e.g. cluster topology or client configuration
            }
            #endregion
        }

        public void Sample2()
        {
            using (var store = new DocumentStore())
            {
                #region client_2
                using (IDocumentSession session = store.OpenSession())  // Open a session for a default 'Database'
                {
                    Category category = new Category
                    {
                        Name = "Database Category"
                    };

                    session.Store(category);                            // Assign an 'Id' and collection (Categories)
                                                                        // and start tracking an entity

                    Product product = new Product
                    {
                        Name = "RavenDB Database",
                        Category = category.Id,
                        UnitsInStock = 10
                    };

                    session.Store(product);                             // Assign an 'Id' and collection (Products)
                                                                        // and start tracking an entity

                    session.SaveChanges();                              // Send to the Server
                                                                        // one request processed in one transaction
                }
                #endregion

                string productId = string.Empty;

                #region client_3
                using (IDocumentSession session = store.OpenSession())  // Open a session for a default 'Database'
                {
                    Product product = session
                        .Include<Product>(x => x.Category)              // Include Category
                        .Load(productId);                               // Load the Product and start tracking

                    Category category = session
                        .Load<Category>(product.Category);              // No remote calls,
                                                                        // Session contains this entity from .Include

                    product.Name = "RavenDB";                           // Apply changes
                    category.Name = "Database";

                    session.SaveChanges();                              // Synchronize with the Server
                                                                        // one request processed in one transaction
                }
                #endregion

                #region client_4
                using (IDocumentSession session = store.OpenSession())  // Open a session for a default 'Database'
                {
                    List<string> productNames = session
                        .Query<Product>()                               // Query for Products
                        .Where(x => x.UnitsInStock > 5)                 // Filter
                        .Skip(0).Take(10)                               // Page
                        .Select(x => x.Name)                            // Project
                        .ToList();                                      // Materialize query
                }
                #endregion
            }
        }
    }
}
