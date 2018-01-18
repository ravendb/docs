using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Indexes
{
	public class StartIndexing
	{
		private interface IFoo
		{
            /*
            #region start_1
            public StartIndexingOperation()
            #endregion
            */
        }

        public StartIndexing()
		{
			using (var store = new DocumentStore())
			{
                #region start_2
                store.Maintenance.Send(new StartIndexingOperation());
                #endregion
			}
		}
	}
}
