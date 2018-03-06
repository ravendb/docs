using System.Runtime.InteropServices.ComTypes;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Indexes.Querying;

namespace Raven.Documentation.Samples.ClientApi.Cluster
{
    public class HowClientApiIntegratesWithReplicationAndCluster
    {
        public void InitializeStoreWithMultipleNodes()
        {
            #region InitializationSample

            using (var store = new DocumentStore
            {
                Database = "TestDB",
                Urls = new [] { 
                                "http://[node A url]",
                                "http://[node B url]",
                                "http://[node C url]"
                              }
            })
            {
                store.Initialize();
                
                // the rest of ClientAPI code
            }

            #endregion

            using (var store = new DocumentStore
            {
                Database = "TestDB",
                Urls = new[]
                {
                    "http://[node A url]",                
                }
            })
            {
                store.Initialize();

                #region WriteAssuranceSample

                using (var session = store.OpenSession())
                {
                    var user = new User
                    {
                        Name = "John Dow"
                    };

                    session.Store(user);

                    //make sure that the comitted data is replicated to 2 nodes
                    //before returning from the SaveChanges() call.
                    session.Advanced
                        .WaitForReplicationAfterSaveChanges(replicas: 2);

                    session.SaveChanges();
                }

                #endregion
            }
        }
    }
}
