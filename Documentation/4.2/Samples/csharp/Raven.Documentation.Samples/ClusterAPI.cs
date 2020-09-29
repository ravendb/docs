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
        }
    }
}
