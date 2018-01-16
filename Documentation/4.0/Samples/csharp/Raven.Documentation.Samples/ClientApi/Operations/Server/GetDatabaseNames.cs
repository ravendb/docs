using Raven.Client.Documents;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{
    public class GetDatabaseNamesInterfaces
    {
        private class GetDatabaseNamesOperation
        {
            #region get_db_names_interface
            public GetDatabaseNamesOperation(int start, int pageSize)
            #endregion
            {
            }
        }
    }

    public class GetDatabaseNamesSamples
	{
		public void Foo()
		{
            using (var store = new DocumentStore())
            {
                #region get_db_names_sample
                var operation = new GetDatabaseNamesOperation(0, 25);
                string[] databaseNames = store.Maintenance.Server.Send(operation);
                #endregion
            }
		}
	}
}
