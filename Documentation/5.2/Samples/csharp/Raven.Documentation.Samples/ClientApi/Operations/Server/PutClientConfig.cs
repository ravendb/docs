using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Configuration;
using Raven.Client.Http;
using Raven.Client.ServerWide.Operations.Configuration;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{
    public class PutClientConfig
    {
        public PutClientConfig()
        {
            using (var store = new DocumentStore())
            {
                #region put_config_1
                // Define the client-configuration object
                ClientConfiguration clientConfiguration = new ClientConfiguration
                {
                    MaxNumberOfRequestsPerSession = 100,
                    ReadBalanceBehavior = ReadBalanceBehavior.FastestNode
                    // ...
                };
                    
                // Define the put server-wide client-configuration operation, pass the configuration 
                var putServerWideClientConfigOp = new PutServerWideClientConfigurationOperation(clientConfiguration);
                
                // Execute the operation by passing it to Maintenance.Server.Send
                store.Maintenance.Server.Send(putServerWideClientConfigOp);
                #endregion
            }
        }
        
        public async Task PutClientConfigAsync()
        {
            var store = new DocumentStore();
            
            #region put_config_2
            // Define the client-configuration object
            ClientConfiguration clientConfiguration = new ClientConfiguration
            {
                MaxNumberOfRequestsPerSession = 100,
                ReadBalanceBehavior = ReadBalanceBehavior.FastestNode
                // ...
            };
                    
            // Define the put server-wide client-configuration operation, pass the configuration 
            var putServerWideClientConfigOp = new PutServerWideClientConfigurationOperation(clientConfiguration);
                
            // Execute the operation by passing it to Maintenance.Server.SendAsync
           store.Maintenance.Server.SendAsync(putServerWideClientConfigOp);
            #endregion
        }
        
        private interface IFoo
        {
            /*
            #region syntax_1
            public PutServerWideClientConfigurationOperation(ClientConfiguration configuration)
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
