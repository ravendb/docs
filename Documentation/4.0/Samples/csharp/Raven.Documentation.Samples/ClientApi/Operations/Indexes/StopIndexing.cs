using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Indexes
{
	public class StopIndexing
	{
		private interface IFoo
		{
            /*
            #region stop_1
            public StopIndexingOperation()
            #endregion
            */
        }

        public StopIndexing()
		{
			using (var store = new DocumentStore())
			{
                #region stop_2
                store.Maintenance.Send(new StopIndexingOperation());
                #endregion
			}
		}
	}
}
