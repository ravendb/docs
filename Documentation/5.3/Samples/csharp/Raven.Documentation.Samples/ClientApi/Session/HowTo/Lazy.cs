using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.CompareExchange;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class Lazy
    {
        public Lazy()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region lazy_Load
                    Lazy<Employee> lazyEmployee = session
                         // Add a call to Lazily 
                        .Advanced.Lazily
                         // Document will Not be loaded from the database here, no server call is made
                        .Load<Employee>("employees/1-A");

                    Employee employee = lazyEmployee.Value; // 'Load' operation is executed here
                    // The employee entity is now loaded & tracked by the session
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region lazy_LoadWithInclude
                    Lazy<Product> lazyProduct = session
                         // Add a call to Lazily 
                        .Advanced.Lazily
                         // Request to include the related Supplier document
                         // Documents will Not be loaded from the database here, no server call is made
                        .Include<Product>(x => x.SupplierId) 
                        .Load<Product>("products/1-A"); 

                    // 'Load with include' operation will be executed here
                    // Both documents will be retrieved from the database
                    Product product = lazyProduct.Value;
                    // The product entity is now loaded & tracked by the session

                    // Access the related document, no additional server call is made
                    Supplier supplier = session.Load<Supplier>(product.SupplierId);
                    // The supplier entity is now also loaded & tracked by the session
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region lazy_LoadStartingWith
                    Lazy<Dictionary<string, Employee>> lazyEmployees = session
                         // Add a call to Lazily 
                        .Advanced.Lazily
                         // Request to load entities whose ID starts with 'employees/'
                         // Documents will Not be loaded from the database here, no server call is made
                        .LoadStartingWith<Employee>("employees/");

                    var employees = lazyEmployees.Value; // 'Load' operation is executed here
                    // The employee entities are now loaded & tracked by the session
                    
                    int employeesCount = employees.Count;
                    #endregion
                }
                
                #region lazy_ConditionalLoad
                // Create document and get the change-vector:
                string changeVector; 
                using (var session = store.OpenSession())
                {
                    Employee employee = new Employee();
                    session.Store(employee, "employees/1-A");
                    session.SaveChanges();
                    // Get the tracked entity change-vector
                    changeVector = session.Advanced.GetChangeVectorFor(employee);
                }
                
                // Conditionally lazy-load the document:
                using (var session = store.OpenSession())
                {
                    var lazyEmployee = session
                         // Add a call to Lazily 
                        .Advanced.Lazily
                         // Document will Not be loaded from the database here, no server call is made
                        .ConditionalLoad<Employee>("employees/1-A", changeVector);

                    var loadedItem = lazyEmployee.Value; // 'ConditionalLoad' operation is executed here
                    Employee employee = loadedItem.Entity;
                    // If ConditionalLoad has actually fetched the document from the server (logic described above)
                    // then the employee entity is now loaded & tracked by the session
                    
                }
                #endregion

                using (var session = store.OpenSession())
                {
                    #region lazy_Query
                    // Define a lazy query:
                    Lazy<IEnumerable<Employee>> lazyEmployees = session
                        .Query<Employee>()
                        .Where( x => x.FirstName == "John")
                         // Add a call to Lazily, the query will Not be executed here 
                        .Lazily();

                    IEnumerable<Employee> employees = lazyEmployees.Value; // Query is executed here
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region lazy_Revisions
                    // Request to get a revision lazily: 
                    var lazyRevisions = session
                         // Add a call to Lazily 
                        .Advanced.Revisions.Lazily
                         // Revisions will not be fetched here, no server call is made
                        .GetFor<Employee>("employees/1-A");
                    
                         // Apply the same usage for the other 2 methods:
                         // .Get()
                         // .GetMetadataFor()

                    List<Employee> revisionsLazyResult = lazyRevisions.Value; // Getting revisions is executed here
                    #endregion
                }
                
                #region lazy_CompareExchange
                // Create compare-exchange value:
                using (var session = store.OpenSession())
                {
                    session.Advanced.ClusterTransaction
                        .CreateCompareExchangeValue(key: "someKey", value: "someValue");
                    session.SaveChanges();
                }
                
                // Get the compare-exchange value lazily: 
                using (var session = store.OpenSession())
                {
                    var lazyCmpXchg = session
                        // Add a call to Lazily 
                        .Advanced.ClusterTransaction.Lazily
                        // Compare-exchange values will not be fetched here, no server call is made
                        .GetCompareExchangeValue<string>("someKey");
                    
                        // Apply the same usage for the other method:
                        // .GetCompareExchangeValues()

                    CompareExchangeValue<string> cmpXchgValue = 
                        lazyCmpXchg.Value; // Getting compare-exchange value is executed here
                }
                #endregion

                using (var session = store.OpenSession())
                {
                    #region lazy_ExecuteAllPendingLazyOperations
                    // Define multiple lazy requests
                    Lazy<User> LazyUser1 =
                        session.Advanced.Lazily.Load<User>("users/1-A");
                    Lazy<User> LazyUser2 =
                        session.Advanced.Lazily.Load<User>("users/2-A");
                    Lazy<IEnumerable<Employee>> LazyEmployees =
                        session.Query<Employee>().Lazily();

                    // Execute all pending lazy operations
                    session.Advanced.Eagerly.ExecuteAllPendingLazyOperations();
                    
                    // All values are now available
                    User user1 = LazyUser1.Value;
                    User user2 = LazyUser2.Value;
                    IEnumerable<Employee> employees = LazyEmployees.Value;
                    #endregion
                }
            }
        }
        
        #region lazy_productClass
        public class Product
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string SupplierId { get; set; } // The related document ID
        }
        #endregion
    }
}
