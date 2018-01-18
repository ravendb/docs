using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
	public class Identities
	{
		private interface IFoo
		{
            /*
            #region sample_1
            public GetIdentitiesOperation()
            #endregion
            */
        }

        public Identities()
		{
			using (var store = new DocumentStore())
			{
                #region sample_2
                Dictionary<string, long> identities = store.Maintenance.Send(new GetIdentitiesOperation());
                #endregion
			}
		}
	}
}
