using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.Http;

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
            // Open session for a 'default' database configured in 'DocumentStore'
            IDocumentSession OpenSession();

            // Open session for a specified database
            IDocumentSession OpenSession(string database);

            IDocumentSession OpenSession(SessionOptions options);
            #endregion

            #region open_session_1_1
            // Open session for a 'default' database configured in 'DocumentStore'
            IAsyncDocumentSession OpenAsyncSession();

            // Open session for a specified database
            IAsyncDocumentSession OpenAsyncSession(string database);

            IAsyncDocumentSession OpenAsyncSession(SessionOptions options);
            #endregion
        }

        public OpeningSession()
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
            }
        }
    }
}
