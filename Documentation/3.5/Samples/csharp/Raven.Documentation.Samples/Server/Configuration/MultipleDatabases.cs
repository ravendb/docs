

#region multiple_databases_2
// required namespaces in using section
using Raven.Client;
using Raven.Client.Document;
#endregion

namespace Raven.Documentation.Samples.Server.Configuration
{
	public class MultipleDatabases
	{
		public void Sample()
		{
			using (var store = new DocumentStore())
			{
				#region multiple_databases_1
				store
					.DatabaseCommands
					.GlobalAdmin
					.EnsureDatabaseExists("Northwind");

				using (IDocumentSession northwindSession = store.OpenSession("Northwind"))
				{
					// ...
				}
				#endregion
			}
		}
	}
}