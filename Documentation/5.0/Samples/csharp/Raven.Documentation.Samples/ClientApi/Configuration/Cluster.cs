using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Http;

namespace Raven.Documentation.Samples.ClientApi.Configuration
{

    public class Cluster
    {
        public Cluster()
        {
            var store = new DocumentStore()
            {
                Conventions = new DocumentConventions
                {
                    #region ReadBalanceBehavior
                    ReadBalanceBehavior = ReadBalanceBehavior.FastestNode
                    #endregion
                    ,
                    #region LoadBalanceBehavior
                    LoadBalanceBehavior = LoadBalanceBehavior.UseSessionContext
                    #endregion
                }
            };
        }
    }
}
