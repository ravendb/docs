using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Http;
using Raven.Documentation.Samples.Orders;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Session
{
    public class OpeningSession
    {
        private interface IFoo2
        {
            #region Session_options
            string Database { get; set; }

            bool NoTracking { get; set; }

            bool NoCaching { get; set; }

            RequestExecutor RequestExecutor { get; set; }

            TransactionMode TransactionMode { get; set; }
            #endregion
        }

        private interface IFoo
        {
            #region open_Session_1
            // Open a Session for the default database configured in `DocumentStore.Database`
            IDocumentSession OpenSession();

            // Open a Session for a specified database
            IDocumentSession OpenSession(string database);
            
            // Open a Session and pass it a preconfigured SessionOptions object
            IDocumentSession OpenSession(SessionOptions options);
            #endregion

            #region open_Session_1_1
            // Open a Session for the default database configured in `DocumentStore.Database`
            IAsyncDocumentSession OpenAsyncSession();

            // Open a Session for a specified database
            IAsyncDocumentSession OpenAsyncSession(string database);

            // Open a Session and pass it a preconfigured SessionOptions object
            IAsyncDocumentSession OpenAsyncSession(SessionOptions options);
            #endregion
        }

        public async Task Sample()
        {
            #region open_Session_2
            using (var store = new DocumentStore())
            {
                // The first overload -
                store.OpenSession();
                // - is equivalent to:
                store.OpenSession(new SessionOptions());

                // The second overload -
                store.OpenSession("your_database_name");
                // - is equivalent to:
                store.OpenSession(new SessionOptions
                {
                    Database = "your_database_name"
                });
            }
            #endregion

            #region open_Session_2_1
            using (var store = new DocumentStore())
            {
                // The first overload -
                store.OpenAsyncSession();
                // - is equivalent to:
                store.OpenAsyncSession(new SessionOptions());

                // The second overload -
                store.OpenAsyncSession("your_database_name");
                // - is equivalent to:
                store.OpenAsyncSession(new SessionOptions
                {
                    Database = "your_database_name"
                });
            }
            #endregion

            #region open_Session_3
            using (var store = new DocumentStore())
            {
                // Open a Session in synchronous operation mode for cluster-wide transactions

                SessionOptions options = new SessionOptions
                {
                    Database = "your_database_name",
                    TransactionMode = TransactionMode.ClusterWide
                };

                using (IDocumentSession Session = store.OpenSession(options))
                {
                    //   Run your business logic:
                    //   
                    //   Store documents
                    //   Load and Modify documents
                    //   Query indexes & collections 
                    //   Delete documents
                    //   ... etc.

                    Session.SaveChanges();
                }
            }
            #endregion

            #region open_Session_3_1
            using (var store = new DocumentStore())
            {
                // Open a Session in asynchronous operation mode for cluster-wide transactions

                SessionOptions options = new SessionOptions
                {
                    Database = "your_database_name",
                    TransactionMode = TransactionMode.ClusterWide
                };

                using (IAsyncDocumentSession Session = store.OpenAsyncSession(options))
                {
                    //   Run your business logic:
                    //   
                    //   Store documents
                    //   Load and Modify documents
                    //   Query indexes & collections 
                    //   Delete documents
                    //   ... etc.

                    await Session.SaveChangesAsync();
                }
            }
            #endregion

            using (var store = new DocumentStore())
            {
                #region open_Session_4
                using (IDocumentSession Session = store.OpenSession())
                {
                    // code here
                }
                #endregion

                #region open_Session_5
                using (IAsyncDocumentSession Session = store.OpenAsyncSession())
                {
                    // async code here
                }
                #endregion

                

                #region open_Session_tracking_1
                using (IDocumentSession Session = store.OpenSession(new SessionOptions
                {
                    NoTracking = true
                }))
                {
                    Employee employee1 = Session.Load<Employee>("employees/1-A");
                    Employee employee2 = Session.Load<Employee>("employees/1-A");

                    // because NoTracking is set to 'true'
                    // each load will create a new Employee instance
                    Assert.NotEqual(employee1, employee2);
                }
                #endregion

                #region open_Session_tracking_2
                using (IAsyncDocumentSession Session = store.OpenAsyncSession(new SessionOptions
                {
                    NoTracking = true
                }))
                {
                    Employee employee1 = await Session.LoadAsync<Employee>("employees/1-A");
                    Employee employee2 = await Session.LoadAsync<Employee>("employees/1-A");

                    // because NoTracking is set to 'true'
                    // each load will create a new Employee instance
                    Assert.NotEqual(employee1, employee2);
                }
                #endregion

                #region open_Session_caching_1
                using (IDocumentSession Session = store.OpenSession(new SessionOptions
                {
                    NoCaching = true
                }))
                {
                    // code here
                }
                #endregion

                #region open_Session_caching_2
                using (IAsyncDocumentSession Session = store.OpenAsyncSession(new SessionOptions
                {
                    NoCaching = true
                }))
                {
                    // async code here
                }
                #endregion
            }

            #region ignore_entity_function
            using (var store = new DocumentStore()
            {
                Conventions =
                {
                    ShouldIgnoreEntityChanges =
                        (session, entity, id) => (entity is Employee e) &&
                                                 (e.FirstName == "Bob")
                }
            }.Initialize())
            {
                using (IDocumentSession Session = store.OpenSession())
                {
                    var employee1 = new Employee() { Id = "employees/1",
                                                     FirstName = "Alice" };
                    var employee2 = new Employee() { Id = "employees/2",
                                                     FirstName = "Bob" };

                    Session.Store(employee1); // Entity is tracked
                    Session.Store(employee2); // Entity is ignored

                    Session.SaveChanges(); // Only employee1 is persisted

                    employee1.FirstName = "Bob"; // Entity is now ignored
                    employee2.FirstName = "Alice"; // Entity is now tracked

                    Session.SaveChanges(); // Only employee2 is persisted
                }
            }
            #endregion
        }
    }
}
