using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Configuration;
using Raven.Client.ServerWide.Operations.Configuration;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{
    public class GetClientConfig
    {
        public GetClientConfig()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region get_config
                    // Define the get client-configuration operation 
                    var getServerWideClientConfigOp = new GetServerWideClientConfigurationOperation();
                    
                    // Execute the operation by passing it to Maintenance.Server.Send
                    ClientConfiguration config = store.Maintenance.Server.Send(getServerWideClientConfigOp);
                    #endregion
                }
            }
        }
        
        public async Task GetClientConfigAsync()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region get_config_async
                    // Define the get client-configuration operation 
                    var getServerWideClientConfigOp = new GetServerWideClientConfigurationOperation();
                    
                    // Execute the operation by passing it to Maintenance.Server.SendAsync
                    ClientConfiguration config = 
                        await store.Maintenance.Server.SendAsync(getServerWideClientConfigOp);
                    #endregion
                }
            }
        }
        
        private interface IFoo
        {
            /*
            #region syntax
            public GetServerWideClientConfigurationOperation()
            #endregion
            */
        }
    }
}
