using Raven.Client.Documents;
using Raven.Client.Http;

namespace Raven.Documentation.Samples.ClientApi.Configuration
{

	public class Replication
	{
		public Replication()
		{
			var store = new DocumentStore();

			var Conventions = store.Conventions;

            #region ReadBalanceBehavior

		    Conventions.ReadBalanceBehavior = ReadBalanceBehavior.FastestNode;

            #endregion

        }
    }
}
