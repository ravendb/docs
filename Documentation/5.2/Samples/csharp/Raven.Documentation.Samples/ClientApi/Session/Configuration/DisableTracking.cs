using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
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
                    // Load a product entity, the session will track its changes
                    Product product = session.Load<Product>("products/1-A");
                    
                    // Disable tracking for the loaded product entity
                    session.Advanced.IgnoreChangesFor(product);
                    
                    // The following change will be ignored for SaveChanges
                    product.UnitsInStock += 1;
                    
                    session.SaveChanges();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region disable_tracking_1_async
                    // Load a product entity, the session will track its changes
                    Product product = await asyncSession.LoadAsync<Product>("products/1-A");
                    
                    // Disable tracking for the loaded product entity
                    asyncSession.Advanced.IgnoreChangesFor(product);
                    
                    // The following change will be ignored for SaveChanges
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
                    
                    // Calling SaveChanges will throw an exception
                    session.SaveChanges();
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
                    
                    // Calling SaveChangesAsync will throw an exception
                    asyncSession.SaveChangesAsync();
                }
                #endregion
            }

            #region disable_tracking_3
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
        }
    }
}
