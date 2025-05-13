using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Session.Loaders;
using Raven.Documentation.Samples.Orders;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Session.Configuration
{
    public class DisableTracking
    {
        private interface IgnoreChangesEntity
        {
            #region syntax_1
            void IgnoreChangesFor(object entity);
            #endregion
        }

        private abstract class IgnoreChangesConvention
        {
            #region syntax_2
            public Func<InMemoryDocumentSessionOperations, object, string, bool> ShouldIgnoreEntityChanges;
            #endregion
        }

        public async Task DisableTrackingSamples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region disable_tracking_1
                    // Load a product entity - the session will track the entity by default
                    Product product = session.Load<Product>("products/1-A");
                    
                    // Call 'IgnoreChangesFor' to instruct the session to ignore changes made to this entity
                    session.Advanced.IgnoreChangesFor(product);
                    
                    // The following change will be ignored by SaveChanges - it will not be persisted
                    product.UnitsInStock += 1;
                    
                    session.SaveChanges();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region disable_tracking_1_async
                    // Load a product entity - the session will track the entity by default
                    Product product = await asyncSession.LoadAsync<Product>("products/1-A");
                    
                    // Call 'IgnoreChangesFor' to instruct the session to ignore changes made to this entity
                    asyncSession.Advanced.IgnoreChangesFor(product);
                    
                    // The following change will be ignored by SaveChanges - it will not be persisted
                    product.UnitsInStock += 1;
                    
                    await asyncSession.SaveChangesAsync();
                    #endregion
                }
                
                #region disable_tracking_2
                using (IDocumentSession session = store.OpenSession(new SessionOptions
                {
                    // Disable tracking for all entities in the session's options
                    NoTracking = true
                }))
                {
                    // Load any entity, it will Not be tracked by the session
                    Employee employee1 = session.Load<Employee>("employees/1-A");
                    
                    // Loading again from same document will result in a new entity instance
                    Employee employee2 = session.Load<Employee>("employees/1-A");
                    
                    // Entities instances are not the same
                    Assert.NotEqual(employee1, employee2);
                }
                #endregion

                #region disable_tracking_2_async
                using (IAsyncDocumentSession asyncSession = store.OpenAsyncSession(new SessionOptions
                {
                    // Disable tracking for all entities in the session's options
                    NoTracking = true
                }))
                {
                    // Load any entity, it will Not be tracked by the session
                    Employee employee1 = await asyncSession.LoadAsync<Employee>("employees/1-A");
                    
                    // Loading again from same document will result in a new entity instance
                    Employee employee2 = await asyncSession.LoadAsync<Employee>("employees/1-A");

                    // Entities instances are not the same
                    Assert.NotEqual(employee1, employee2);
                }
                #endregion
                
                #region disable_tracking_3
                using (IDocumentSession session = store.OpenSession())
                {
                    // Define a query
                    List<Employee> employeesResults = session.Query<Employee>()
                        // Set NoTracking, all resulting entities will not be tracked
                        .Customize(x => x.NoTracking())
                        .Where(x => x.FirstName == "Robert")
                        .ToList();

                    // The following modification will not be tracked for SaveChanges
                    Employee firstEmployee = employeesResults[0];
                    firstEmployee.LastName = "NewName";
                    
                    // Change to 'firstEmployee' will not be persisted
                    session.SaveChanges();
                }
                #endregion

                #region disable_tracking_3_async
                using (IAsyncDocumentSession asyncSession = store.OpenAsyncSession())
                {
                    // Define a query
                    List<Employee> employeesResults = asyncSession.Query<Employee>()
                        // Set NoTracking, all resulting entities will not be tracked
                        .Customize(x => x.NoTracking())
                        .Where(x => x.FirstName == "Robert")
                        .ToList();

                    // The following modification will not be tracked for SaveChanges
                    Employee firstEmployee = employeesResults[0];
                    firstEmployee.LastName = "NewName";
                    
                    // Change to 'firstEmployee' will not be persisted
                    await asyncSession.SaveChangesAsync();
                }
                #endregion
                
                #region disable_tracking_3_documentQuery
                using (IDocumentSession session = store.OpenSession())
                {
                    // Define a query
                    List<Employee> employeesResults = session.Advanced.DocumentQuery<Employee>()
                        // Set NoTracking, all resulting entities will not be tracked
                        .NoTracking()
                        .Where(x => x.FirstName == "Robert")
                        .ToList();

                    // The following modification will not be tracked for SaveChanges
                    Employee firstEmployee = employeesResults[0];
                    firstEmployee.LastName = "NewName";
                    
                    // Change to 'firstEmployee' will not be persisted
                    session.SaveChanges();
                }
                #endregion
                
                #region disable_tracking_3_documentQuery_async
                using (IAsyncDocumentSession asyncSession = store.OpenAsyncSession())
                {
                    // Define a query
                    List<Employee> employeesResults = asyncSession.Advanced.AsyncDocumentQuery<Employee>()
                        // Set NoTracking, all resulting entities will not be tracked
                        .NoTracking()
                        .Where(x => x.FirstName == "Robert")
                        .ToList();

                    // The following modification will not be tracked for SaveChanges
                    Employee firstEmployee = employeesResults[0];
                    firstEmployee.LastName = "NewName";
                    
                    // Change to 'firstEmployee' will not be persisted
                    await asyncSession.SaveChangesAsync();
                }
                #endregion
            }

            #region disable_tracking_4
            using (var store = new DocumentStore()
            {
                // Define the 'ignore' convention on your document store
                Conventions =
                {
                    ShouldIgnoreEntityChanges =
                        // Define for which entities tracking should be disabled 
                        // Tracking will be disabled ONLY for entities of type Employee whose FirstName is Bob
                        (session, entity, id) => (entity is Employee e) &&
                                                 (e.FirstName == "Bob")
                }
            }.Initialize())
            {
                using (IDocumentSession session = store.OpenSession())
                {
                    var employee1 = new Employee { Id = "employees/1", FirstName = "Alice" };
                    var employee2 = new Employee { Id = "employees/2", FirstName = "Bob" };

                    session.Store(employee1);      // This entity will be tracked
                    session.Store(employee2);      // Changes to this entity will be ignored

                    session.SaveChanges();         // Only employee1 will be persisted

                    employee1.FirstName = "Bob";   // Changes to this entity will now be ignored
                    employee2.FirstName = "Alice"; // This entity will now be tracked

                    session.SaveChanges();         // Only employee2 is persisted
                }
            }
            #endregion
           
            using (var store = new DocumentStore())
            {
                #region disable_tracking_5
                using (IDocumentSession session = store.OpenSession(new SessionOptions
                {
                    // Working with a non-tracking session
                    NoTracking = true
                }))
                {
                    try
                    {
                        // Trying to include a related document when loading a document will throw:
                        Product product1 = session
                            .Include<Product>(x => x.Supplier)
                            .Load<Product>("products/1-A");
                        
                        // The same applies when using the builder syntax:
                        Product product2 = session.Load<Product>("products/1-A",
                            builder => builder.IncludeDocuments(product => product.Supplier));
                    }
                    catch (InvalidOperationException e)
                    {
                        // An InvalidOperationException is expected here
                    }
                }
                #endregion
                
                #region disable_tracking_5_async
                using (IAsyncDocumentSession asyncSession = store.OpenAsyncSession(new SessionOptions
                       {
                           // Working with a non-tracking session
                           NoTracking = true
                       }))
                {
                    try
                    {
                        // Trying to include a related document when loading a document will throw:
                        Product product = await asyncSession
                            .Include<Product>(x => x.Supplier)
                            .LoadAsync<Product>("products/1-A");
                        
                        // The same applies when using the builder syntax:
                        Product product2 = await asyncSession.LoadAsync<Product>("products/1-A",
                            builder => builder.IncludeDocuments(product => product.Supplier));
                    }
                    catch (InvalidOperationException e)
                    {
                        // An InvalidOperationException is expected here
                    }
                }
                #endregion
                
                #region disable_tracking_6
                using (IDocumentSession session = store.OpenSession(new SessionOptions
                       {
                           // Working with a non-tracking session
                           NoTracking = true
                       }))
                {
                    try
                    {
                        // Trying to include related documents in a query will throw
                        var products = session
                            .Query<Product>()
                            .Include(x => x.Supplier)
                            .ToList();
                    }
                    catch (InvalidOperationException e)
                    {
                        // An InvalidOperationException is expected here
                    }
                }
                #endregion
                
                #region disable_tracking_6_async
                using (IAsyncDocumentSession asyncSession = store.OpenAsyncSession(new SessionOptions
                       {
                           // Working with a non-tracking session
                           NoTracking = true
                       }))
                {
                    try
                    {
                        // Trying to include related documents when making a query will throw
                        var products = await asyncSession
                            .Query<Product>()
                            .Include(x => x.Supplier)
                            .ToListAsync();
                    }
                    catch (InvalidOperationException e)
                    {
                        // An InvalidOperationException is expected here
                    }
                }
                #endregion
            }
        }
    }
}
