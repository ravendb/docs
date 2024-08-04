using Raven.Client.Documents;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using System.Threading.Tasks;
using Raven.Client.Documents.Operations.Replication;
using System;

namespace Raven.Documentation.Samples.Server.OngoingTasks.ExternalReplicationSamples
{

    public class ExternalReplicationExampleConfig
    {

        #region ExternalReplicationProperties
        public class ExternalReplication
        {
            public TimeSpan DelayReplicationFor { get; set; }
            public string Name { get; set; }
            public string ConnectionStringName { get; set; }
            public string MentorNode { get; set; }
        }
        #endregion

    }

    public class ExternalReplicationSample
    {

        public async Task ExtRepSimple()
        {
            #region ExternalReplication
            IDocumentStore sourceStore = null;
            IDocumentStore destinationStore = null;
            string connectionStrName = null;

            //setup connection string from sourceStore to destinationStore
            await sourceStore.Maintenance.SendAsync(
                new PutConnectionStringOperation<RavenConnectionString>(new RavenConnectionString
            {
                Database = destinationStore.Database,
                Name = connectionStrName,
                TopologyDiscoveryUrls = destinationStore.Urls
            }));

            //define external replication
            await sourceStore.Maintenance.SendAsync(
                new UpdateExternalReplicationOperation(new ExternalReplication
            {
                ConnectionStringName = connectionStrName,
                Name = "task-name",
            }));

            #endregion



        }


        public async Task RepWithDelayAndMentorNode()
        {
            IDocumentStore sourceStore = null;
            IDocumentStore destinationStore = null;
            string connectionStrName = null;

            //setup connection string from sourceStore to destinationStore
            await sourceStore.Maintenance.SendAsync(
                new PutConnectionStringOperation<RavenConnectionString>(new RavenConnectionString
            {
                Database = destinationStore.Database,
                Name = connectionStrName,
                TopologyDiscoveryUrls = destinationStore.Urls
            }));

            #region ExternalReplicationWithMentorAndDelay
            //define external replication with mentor node and delay timespan
            await sourceStore.Maintenance.SendAsync(
                new UpdateExternalReplicationOperation(new ExternalReplication
            {
                ConnectionStringName = connectionStrName,
                Name = "task-name",
                MentorNode = "B",
                DelayReplicationFor = TimeSpan.FromMinutes(30)
            }));
           
            #endregion
        }
    }
}
