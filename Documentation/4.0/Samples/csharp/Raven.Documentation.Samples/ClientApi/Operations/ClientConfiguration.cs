using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.Configuration;
using Raven.Client.Documents.Queries;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class ClientConfiguration
    {
        private interface IFoo
        {
            /*
            #region config_1
            public GetClientConfigurationOperation()
            #endregion
            */
        }

        #region config_2
        public class Result
        {
            public long Etag { get; set; }

            public ClientConfiguration Configuration { get; set; }
        }
        #endregion

        public ClientConfiguration()
        {
            using (var store = new DocumentStore())
            {
                #region config_3
                GetClientConfigurationOperation.Result config = store.Maintenance.Send(new GetClientConfigurationOperation());
                Client.ServerWide.ClientConfiguration clientConfiguration = config.Configuration;
                #endregion
            }
        }
    }
}
