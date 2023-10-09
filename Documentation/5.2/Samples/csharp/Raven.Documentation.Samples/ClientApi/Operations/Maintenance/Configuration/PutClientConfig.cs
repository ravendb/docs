using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Operations.Configuration;
using Raven.Client.Http;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Configuration
{
    public class PutClientConfig
    {
        public PutClientConfig()
        {
            #region put_config_1
            // You can initialize the client-configuration options when creating the Document Store:
            // (This is optional)
            var documentStore = new DocumentStore
            {
                Urls = new[] { "ServerURL_1", "ServerURL_2", "..." },
                Database = "DefaultDB",
                Conventions = new DocumentConventions
                {
                    // Initialize some client-configuration options:
                    MaxNumberOfRequestsPerSession = 100,
                    IdentityPartsSeparator = '$'
                    // ...
                }
            }.Initialize();
            #endregion
            
            #region put_config_2
            // Set (or override) the client-configuration using the put operation:
            using (documentStore)
            {
                // Define the client-configuration object
                ClientConfiguration clientConfiguration = new ClientConfiguration
                {
                    MaxNumberOfRequestsPerSession = 200,
                    ReadBalanceBehavior = ReadBalanceBehavior.FastestNode
                    // ...
                };
                    
                // Define the put client-configuration operation, pass the configuration 
                var putClientConfigOp = new PutClientConfigurationOperation(clientConfiguration);
                
                // Execute the operation by passing it to Maintenance.Send
                documentStore.Maintenance.Send(putClientConfigOp);
            }
            #endregion
        }
        
        public async Task PutClientConfigAsync()
        {
            var documentStore = new DocumentStore();
            
            #region put_config_3
            // Set (or override) the client-configuration using the put operation:
            using (documentStore)
            {
                // Define the client-configuration object
                ClientConfiguration clientConfiguration = new ClientConfiguration
                {
                    MaxNumberOfRequestsPerSession = 200,
                    ReadBalanceBehavior = ReadBalanceBehavior.FastestNode
                    // ...
                };
                    
                // Define the put client-configuration operation, pass the configuration 
                var putClientConfigOp = new PutClientConfigurationOperation(clientConfiguration);
                
                // Execute the operation by passing it to Maintenance.SendAsync
                await documentStore.Maintenance.SendAsync(putClientConfigOp);
            }
            #endregion
        }
        
        private interface IFoo
        {
            /*
            #region syntax_1
            public PutClientConfigurationOperation(ClientConfiguration configuration)
            #endregion
            */
            
            /*
            #region syntax_2
            public class ClientConfiguration
            {
                private char? _identityPartsSeparator;
                public long Etag { get; set; }
                public bool Disabled { get; set; }
                public int? MaxNumberOfRequestsPerSession { get; set; }
                public ReadBalanceBehavior? ReadBalanceBehavior { get; set; }
                public LoadBalanceBehavior? LoadBalanceBehavior { get; set; }
                public int? LoadBalancerContextSeed { get; set; }
                public char? IdentityPartsSeparator;  // can be any character except '|'
            }
            #endregion
            */
        }
    }
}
