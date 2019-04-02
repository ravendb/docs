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
            // Open session for the default database configured in `DocumentStore.Database`
            IDocumentSession OpenSession();

            // Open session for a specified database
            IDocumentSession OpenSession(string database);

            // Open session and pass it a preconfigured SessionOptions object
            IDocumentSession OpenSession(SessionOptions options);

            //The first overloaded method is equivalent to

            #endregion

            #region open_session_1_1
            // Open session for the default database configured in `DocumentStore.Database`
            IAsyncDocumentSession OpenAsyncSession();

            // Open session for a specified database
            IAsyncDocumentSession OpenAsyncSession(string database);

            // Open session and pass it a preconfigured SessionOptions object
            IAsyncDocumentSession OpenAsyncSession(SessionOptions options);
            #endregion
        }

        public async Task Sample()
        {
            string databaseName = "DB1";

            using (var store = new DocumentStore())
            {
                #region open_session_2
                store.OpenSession(new SessionOptions());
                #endregion

                #region open_session_2_1
                store.OpenAsyncSession(new SessionOptions());
                #endregion

                #region open_session_3
                store.OpenSession(new SessionOptions
                {
                    Database = databaseName
                });
                #endregion

                #region open_session_3_1
                store.OpenAsyncSession(new SessionOptions
                {
                    Database = databaseName
                });
                #endregion

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
