using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.Configuration;
using Raven.Client.Documents.Queries;
using Raven.Client.ServerWide;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class ClientConfig
    {
        private interface IFoo
        {
            /*
            #region config_1_0
            public GetClientConfigurationOperation()
            #endregion
            */

            /*
            #region config_2_0
            public PutClientConfigurationOperation(ClientConfiguration configuration)
            #endregion
            */
        }
        private class Foo
        {
            #region config_1_1
            public class Result
            {
                public long Etag { get; set; }

                public ClientConfiguration Configuration { get; set; }
            }
            #endregion
        }



        public ClientConfig()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region config_1_2
                    GetClientConfigurationOperation.Result config = store.Maintenance.Send(new GetClientConfigurationOperation());
                    ClientConfiguration clientConfiguration = config.Configuration;
                    #endregion
                }

                {
                    ClientConfiguration clientConfiguration = null;
                    #region config_2_2
                    store.Maintenance.Send(new PutClientConfigurationOperation(clientConfiguration));
                    #endregion
                }



            }
        }
    }
}
