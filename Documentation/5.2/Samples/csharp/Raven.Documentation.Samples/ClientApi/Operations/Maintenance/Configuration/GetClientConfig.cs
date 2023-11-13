using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Configuration;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.Configuration
{
    public class GetClientConfig
    {
        public GetClientConfig()
        {
            using (var store = new DocumentStore())
            {
                #region get_config
                // Define the get client-configuration operation 
                var getClientConfigOp = new GetClientConfigurationOperation();
                    
                // Execute the operation by passing it to Maintenance.Send
                GetClientConfigurationOperation.Result result = store.Maintenance.Send(getClientConfigOp);
                
                ClientConfiguration clientConfiguration = result.Configuration;
                #endregion
            }
        }
        
        public async Task GetClientConfigAsync()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region get_config_async
                    // Define the get client-configuration operation 
                    var getClientConfigOp = new GetClientConfigurationOperation();
                
                    // Execute the operation by passing it to Maintenance.SendAsync
                    GetClientConfigurationOperation.Result config = 
                        await store.Maintenance.SendAsync(getClientConfigOp);
                
                    ClientConfiguration clientConfiguration = config.Configuration;
                    #endregion
                }
            }
        }
        
        private interface IFoo
        {
            /*
            #region syntax_1
            public GetClientConfigurationOperation()
            #endregion
            */
            
            /*
            #region syntax_2
            // Executing the operation returns the following object: 
            public class Result
            {
                // The configuration Etag
                public long Etag { get; set; }
                
                // The current client-configuration deployed on the server for the database
                public ClientConfiguration Configuration { get; set; }
            }
            #endregion
            */
        }
    }
}
