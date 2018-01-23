using Raven.Client.Documents;
using Raven.Client.Http;

namespace Raven.Documentation.Samples.ClientApi.Configuration
{

    public class Cluster
    {
        public Cluster()
        {
            var store = new DocumentStore()
            {
                Conventions =
                {
                    #region ReadBalanceBehavior
                    ReadBalanceBehavior = ReadBalanceBehavior.FastestNode
                    #endregion
                }
            };
        }
    }
}
