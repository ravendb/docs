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
            #region session_options
            string Database { get; set; }

            bool NoTracking { get; set; }

            bool NoCaching { get; set; }

            RequestExecutor RequestExecutor { get; set; }

            TransactionMode TransactionMode { get; set; }
            #endregion
        }

        private interface IFoo
        {
            #region open_session_1
            // First overload
            // Open session for the default database configured in `DocumentStore.Database`
            IDocumentSession OpenSession();

            // Second overload
            // Open session for a specified database
            IDocumentSession OpenSession(string database);

            // Third overload
            // Open session and pass it a preconfigured SessionOptions object
            IDocumentSession OpenSession(SessionOptions options);
            #endregion

            #region open_session_1_1
            // First overload
            // Open session for the default database configured in `DocumentStore.Database`
            IAsyncDocumentSession OpenAsyncSession();

            // Second overload
            // Open session for a specified database
            IAsyncDocumentSession OpenAsyncSession(string database);

            // Third overload
            // Open session and pass it a preconfigured SessionOptions object
            IAsyncDocumentSession OpenAsyncSession(SessionOptions options);
            #endregion
        }

        public async Task Sample()
        {
            #region open_session_2
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

            #region open_session_2_1
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

            using (var store = new DocumentStore())
            {
                #region open_session_4
                using (IDocumentSession session = store.OpenSession())
                {
                    // code here
                }
                #endregion

                #region open_session_5
                using (IAsyncDocumentSession session = store.OpenAsyncSession())
                {
                    // async code here
                }
                #endregion

                #region open_session_tracking_1
                using (IDocumentSession session = store.OpenSession(new SessionOptions
                {
                    NoTracking = true
                }))
                {
                    Employee employee1 = session.Load<Employee>("employees/1-A");
                    Employee employee2 = session.Load<Employee>("employees/1-A");

                    // because NoTracking is set to 'true'
                    // each load will create a new Employee instance
                    Assert.NotEqual(employee1, employee2);
                }
                #endregion

                #region open_session_tracking_2
                using (IAsyncDocumentSession session = store.OpenAsyncSession(new SessionOptions
                {
                    NoTracking = true
                }))
                {
                    Employee employee1 = await session.LoadAsync<Employee>("employees/1-A");
                    Employee employee2 = await session.LoadAsync<Employee>("employees/1-A");

                    // because NoTracking is set to 'true'
                    // each load will create a new Employee instance
                    Assert.NotEqual(employee1, employee2);
                }
                #endregion

                #region open_session_caching_1
                using (IDocumentSession session = store.OpenSession(new SessionOptions
                {
                    NoCaching = true
                }))
                {
                    // code here
                }
                #endregion

                #region open_session_caching_2
                using (IAsyncDocumentSession session = store.OpenAsyncSession(new SessionOptions
                {
                    NoCaching = true
                }))
                {
                    // async code here
                }
                #endregion
            }
        }
    }
}
