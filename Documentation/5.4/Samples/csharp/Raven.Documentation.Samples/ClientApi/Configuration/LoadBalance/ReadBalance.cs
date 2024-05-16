using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Operations.Configuration;
using Raven.Client.Http;
using Raven.Client.ServerWide.Operations.Configuration;

namespace Raven.Documentation.Samples.ClientApi.Configuration.LoadBalance
{
    public class ReadBalanceExamples
    {
        public ReadBalanceExamples()
        {
            #region ReadBalance_1
            // Initialize 'ReadBalanceBehavior' on the client:
            var documentStore = new DocumentStore
            {
                Urls = new[] { "ServerURL_1", "ServerURL_2", "..." },
                Database = "DefaultDB",
                Conventions = new DocumentConventions
                {
                    // With ReadBalanceBehavior set to: 'FastestNode':
                    // Client READ requests will address the fastest node
                    // Client WRITE requests will address the preferred node
                    ReadBalanceBehavior = ReadBalanceBehavior.FastestNode
                }
            }.Initialize();
            #endregion
            
            #region ReadBalance_2
            // Setting 'ReadBalanceBehavior' on the server by sending an operation:
            using (documentStore)
            {
                // Define the client configuration to put on the server
                var clientConfiguration = new ClientConfiguration
                {
                    // Replace 'FastestNode' (from example above) with 'RoundRobin'
                    ReadBalanceBehavior = ReadBalanceBehavior.RoundRobin
                };
                
                // Define the put configuration operation for the DEFAULT database
                var putConfigurationOp = new PutClientConfigurationOperation(clientConfiguration);
                
                // Execute the operation by passing it to Maintenance.Send
                documentStore.Maintenance.Send(putConfigurationOp);
                
                // After the operation has executed:
                // All WRITE requests will continue to address the preferred node 
                // READ requests, per session, will address a different node based on the RoundRobin logic
            }
            #endregion
            
            #region ReadBalance_3
            // Setting 'ReadBalanceBehavior' on the server by sending an operation:
            using (documentStore)
            {
                // Define the client configuration to put on the server
                var clientConfiguration = new ClientConfiguration
                {
                    // Replace 'FastestNode' (from example above) with 'RoundRobin'
                    ReadBalanceBehavior = ReadBalanceBehavior.RoundRobin
                };
                
                // Define the put configuration operation for the ALL databases
                var putConfigurationOp = new PutServerWideClientConfigurationOperation(clientConfiguration);
                
                // Execute the operation by passing it to Maintenance.Server.Send
                documentStore.Maintenance.Server.Send(putConfigurationOp);
                
                // After the operation has executed:
                // All WRITE requests will continue to address the preferred node 
                // READ requests, per session, will address a different node based on the RoundRobin logic
            }
            #endregion
        }
    }
}
