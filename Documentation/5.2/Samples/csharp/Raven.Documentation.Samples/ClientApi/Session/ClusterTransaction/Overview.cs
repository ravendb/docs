using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Raven.Documentation.Samples.ClientApi.Session.ClusterTransactions
{
    class ClusterTransactions<T>
    {
        public class DNS
        {
            public string IpAddress;
        }

        public async Task Async()
        {
            using (var store = new DocumentStore())
            {
                #region open_cluster_session_async
                using (var session = store.OpenAsyncSession(new SessionOptions
                {
                    // Set mode to be cluster-wide
                    TransactionMode = TransactionMode.ClusterWide
                    
                    // Session will be single-node when either:
                    //   * Mode is not specified
                    //   * Explicitly set TransactionMode.SingleNode
                }))
                #endregion
                {
                }
            }
        }

        public void Sync()
        {
            using (var store = new DocumentStore())
            {
                #region open_cluster_session_sync
                using (var session = store.OpenSession(new SessionOptions
                {
                    // Set mode to be cluster-wide
                    TransactionMode = TransactionMode.ClusterWide
                
                    // Session will be single-node when either:
                    //   * Mode is not specified
                    //   * Explicitly set TransactionMode.SingleNode
                }))
                #endregion
                {
                }
            }
        }
    }
}
