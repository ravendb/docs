using System.Net.Http;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.Replication;
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
                //note: by default the embedded server will use a random port.
                // if there is a need to replicate TO the embedded server, we would need to specify the port directly
                EmbeddedServer.Instance.StartServer(new ServerOptions
                {
                    ServerUrl = "http://localhost:8090"
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

        private static async Task ExternalReplication()
        {
            #region External_Replication_And_Embedded

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
                    ServerUrl = "http://localhost:8090",
                    AcceptEula = true
                });

                var embeddedServerUrl =
                    (await EmbeddedServer.Instance.GetServerUriAsync().ConfigureAwait(false)).ToString();

                // create watcher definition that will be added to existing cluster
                var externalReplicationWatcher = new ExternalReplication(
                    database: "Northwind",
                    connectionStringName: "Embedded Northwind Instance");

                //create the connection string for the embedded instance on the existing cluster
                await store.Maintenance.SendAsync(
                    new PutConnectionStringOperation<RavenConnectionString>(new RavenConnectionString
                    {
                        Name = externalReplicationWatcher.ConnectionStringName,
                        Database = externalReplicationWatcher.Database,
                        TopologyDiscoveryUrls = new[] {embeddedServerUrl} //urls to discover topology at destination
                    })).ConfigureAwait(false);

                //create External Replication task from the cluster to the embedded RavenDB instance
                await store.Maintenance.SendAsync(new UpdateExternalReplicationOperation(externalReplicationWatcher))
                    .ConfigureAwait(false);
            }

            #endregion
        }
    }
}
