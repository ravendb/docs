using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
namespace Raven.Documentation.Samples.ClientApi.Session
{
    public class OpenSession
    {
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

            #region open_Session_1_async
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
                // Define the Session's options object
                SessionOptions options = new SessionOptions
                {
                    Database = "your_database_name",
                    TransactionMode = TransactionMode.ClusterWide
                };

                // Open the Session in a Synchronous mode
                // Pass the options object to the session
                using (IDocumentSession session = store.OpenSession(options))
                {
                    //   Run your business logic:
                    //   
                    //   Store entities
                    //   Load and Modify entities
                    //   Query indexes & collections 
                    //   Delete entities
                    //   ... etc.

                    session.SaveChanges();
                    // When 'SaveChanges' returns successfully,
                    // all changes made to the entities in the session are persisted to the documents in the database
                }
            }
            #endregion

            #region open_Session_2_async
            using (var store = new DocumentStore())
            {
                // Define the Session's options object
                SessionOptions options = new SessionOptions
                {
                    Database = "your_database_name",
                    TransactionMode = TransactionMode.ClusterWide
                };

                // Open the Session in an Asynchronous mode
                // Pass the options object to the session
                using (IAsyncDocumentSession asyncSession = store.OpenAsyncSession(options))
                {
                    //   Run your business logic:
                    //   
                    //   Store entities
                    //   Load and Modify documentitiesents
                    //   Query indexes & collections 
                    //   Delete documents
                    //   ... etc.

                    await asyncSession.SaveChangesAsync();
                    // When 'SaveChanges' returns successfully,
                    // all changes made to the entities in the session are persisted to the documents in the database
                }
            }
            #endregion
        }
    }
}
