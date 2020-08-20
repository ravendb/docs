using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Http;

namespace Raven.Documentation.Samples.ClientApi.Configuration
{

    public class Cluster
    {
        public Cluster()
        {
            var store1 = new DocumentStore()
            {
                #region ReadBalanceBehavior
                Conventions = new DocumentConventions
                {
                    ReadBalanceBehavior = ReadBalanceBehavior.FastestNode
                }
                #endregion
            };


            var store2 = new DocumentStore()
            {
                #region LoadBalanceBehavior
                Conventions = new DocumentConventions
                {
                    LoadBalanceBehavior = LoadBalanceBehavior.UseSessionContext
                }
                #endregion
            };
        }

    }
}
