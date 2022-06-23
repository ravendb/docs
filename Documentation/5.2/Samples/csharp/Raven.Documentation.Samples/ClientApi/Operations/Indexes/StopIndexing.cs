using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.ServerWide.Operations;


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
                // Stop indexing
                store.Maintenance.Send(new StopIndexingOperation());

                // Disable the database
                store.Maintenance.Server.Send(new ToggleDatabasesStateOperation(store.Database, true));
                // Enable the database to apply new settings
                store.Maintenance.Server.Send(new ToggleDatabasesStateOperation(store.Database, false));
                #endregion
            }
		}
	}
}
