using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Indexes
{
	public class DisableIndex
	{
		private interface IFoo
		{
            /*
            #region disable_1
            public DisableIndexOperation(string indexName)
            #endregion
            */
        }

        public DisableIndex()
		{
			using (var store = new DocumentStore())
			{
                #region disable_2
                store.Maintenance.Send(new DisableIndexOperation("Orders/Totals"));
                // index is disabled at this point, new data won't be indexed
                // but you can still query on this index
                #endregion
			}
		}
	}
}
