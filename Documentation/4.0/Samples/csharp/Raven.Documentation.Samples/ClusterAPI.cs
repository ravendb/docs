using System.Net.Http;
using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples
{

    public class ClusterAPI
    {
        public ClusterAPI()
        {
            using (var store = new DocumentStore())
            {
                #region add_node_with_args
                store.GetRequestExecutor().HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Put, "http://<server-url>/admin/cluster/node?url=<new-node-url>&tag=<new-node-tag>&watcher=<is-watcher>&assignedCores=<assigned-cores>"));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region delete_node
                store.GetRequestExecutor().HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "http://<server-url>/admin/cluster/node?nodeTag=<node-tag>"));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region promote_node
                store.GetRequestExecutor().HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "http://<server-url>/admin/cluster/promote?nodeTag=<node-tag>"));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region demote_node
                store.GetRequestExecutor().HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "http://<server-url>/admin/cluster/demote?nodeTag=<node-tag>"));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region force_election
                store.GetRequestExecutor().HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "http://<server-url>/admin/cluster/reelect"));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region force_timeout
                store.GetRequestExecutor().HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "http://<server-url>/admin/cluster/timeout"));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region bootstrap
                store.GetRequestExecutor().HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "http://<server-url>/admin/cluster/bootstrap"));
                #endregion
            }

        }
    }
}
