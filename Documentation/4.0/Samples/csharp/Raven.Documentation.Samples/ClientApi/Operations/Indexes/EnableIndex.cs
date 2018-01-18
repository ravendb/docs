using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Indexes
{
	public class EnableIndex
	{
		private interface IFoo
		{
            /*
            #region enable_1
            public EnableIndexOperation(string indexName)
            #endregion
            */
        }

        public EnableIndex()
		{
			using (var store = new DocumentStore())
			{
                #region enable_2
                store.Maintenance.Send(new EnableIndexOperation("Orders/Totals"));
                #endregion
			}
		}
	}
}
