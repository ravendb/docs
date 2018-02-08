using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Configuration;
using Raven.Client.Documents.Queries;
using Raven.Client.Http;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations.Configuration;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{
    public class ClientConfig
    {
        private interface IFoo
        {
            /*
            #region config_1_0
            public GetServerWideClientConfigurationOperation()
            #endregion
            */

            /*
            #region config_2_0
            public PutServerWideClientConfigurationOperation(ClientConfiguration configuration)
            #endregion
            */
        }


        public ClientConfig()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region config_1_2
                    ClientConfiguration clientConfiguration =
                        store.Maintenance.Server.Send(new GetServerWideClientConfigurationOperation());
                    #endregion
                }

                {
                    #region config_2_2
                    ClientConfiguration clientConfiguration = new ClientConfiguration
                    {
                        MaxNumberOfRequestsPerSession = 100,
                        ReadBalanceBehavior = ReadBalanceBehavior.FastestNode
                    };
                    
                    store.Maintenance.Server.Send(new PutServerWideClientConfigurationOperation(clientConfiguration));
                    #endregion
                }



            }
        }
    }
}
