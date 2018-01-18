using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Indexes
{
	public class StopIndex
	{
		private interface IFoo
		{
            /*
            #region stop_1
            public StopIndexOperation(string indexName)
            #endregion
            */
        }

        public StopIndex()
		{
			using (var store = new DocumentStore())
			{
                #region stop_2
                store.Maintenance.Send(new StopIndexOperation("Orders/Totals"));
                #endregion
			}
		}
	}
}
