using System.Net.Http;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.ServerWide.Commands;
using Raven.Client.ServerWide.Operations;
using Raven.Embedded;
// ReSharper disable RegionWithinTypeMemberBody

namespace Raven.Documentation.Samples.Server
{
    public class ReplicationEmbedded
    {
        public async Task Initiate_Embedded_Replication()
        {
            #region Embedded_OpenStudio

            //this starts the embedded server
            EmbeddedServer.Instance.StartServer(new ServerOptions
            {
                ServerUrl = "http://localhost:8090" //if we don't specify a port, it will have random port
            });

            //this opens Embedded RavenDB Studio in the default browser
            EmbeddedServer.Instance.OpenStudioInBrowser();

            #endregion

            #region Embedded_Replication_Setup
            //first, initialize connection with one of cluster nodes
            var ravenClusterNodeUrl = "http://localhost:8080";
            using (var store = new DocumentStore
            {
                Urls = new[] {ravenClusterNodeUrl},
                Database = "Northwind"
            })
            {
                store.Initialize();

                //first, start the embedded server
                EmbeddedServer.Instance.StartServer(new ServerOptions
                {
                    ServerUrl = "http://localhost:8090" //if we don't specify a port, it will have random port
                });

                //then, add embedded instance to existing cluster
                //(programmatically, this is done via REST)
                var embeddedServerUrl =
                    (await EmbeddedServer.Instance.GetServerUriAsync().ConfigureAwait(false)).ToString();
                var addToClusterCommandUrl = $"{ravenClusterNodeUrl}/admin/cluster/node?url={embeddedServerUrl}";
                await store.GetRequestExecutor()
                    .HttpClient
                    .SendAsync(
                        new HttpRequestMessage(
                            HttpMethod.Put,
                            addToClusterCommandUrl)).ConfigureAwait(false);

                var getTopologyCommand = new GetClusterTopologyCommand();
                var embeddedStore = EmbeddedServer.Instance.GetDocumentStore("Northwind");
                string embeddedTag;
                using (var session = embeddedStore.OpenSession())
                {
                    //fetch topology info from embedded, so we can fetch the tag assigned by the cluster
                    await embeddedStore.GetRequestExecutor()
                        .ExecuteAsync(getTopologyCommand, session.Advanced.Context)
                        .ConfigureAwait(false);
                    embeddedTag = getTopologyCommand.Result.Topology.LastNodeId;
                }

                //this sends the command to add the database "Northwind" to its database group
                await store.Maintenance
                           .Server
                           .SendAsync(
                                new AddDatabaseNodeOperation(databaseName: "Northwind",
                                                             node: embeddedTag))
                           .ConfigureAwait(false);
            }
            #endregion
        }
    }
}
