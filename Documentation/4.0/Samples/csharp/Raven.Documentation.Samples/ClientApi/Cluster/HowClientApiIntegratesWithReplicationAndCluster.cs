using Raven.Client.Documents;

namespace Raven.Documentation.Samples.ClientApi.Cluster
{
    public class HowClientApiIntegratesWithReplicationAndCluster
    {
        public void InitializeStoreWithMultipleNodes()
        {
            #region Sample

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
        }
    }
}
