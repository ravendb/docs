using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.ServerWide.Operations;

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
                // Start indexing
                store.Maintenance.Send(new StartIndexingOperation());

                // Disable the database
                store.Maintenance.Server.Send(new ToggleDatabasesStateOperation(store.Database, true));
                // Enable the database to apply new settings
                store.Maintenance.Server.Send(new ToggleDatabasesStateOperation(store.Database, false));
                #endregion
            }
		}
	}
}
