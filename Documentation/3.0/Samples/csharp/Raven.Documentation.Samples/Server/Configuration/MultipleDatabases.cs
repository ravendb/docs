using Raven.Client.Document;
using Raven.Client.Extensions;

#region multiple_databases_2

// required namespace in usings
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

				using (var northwindSession = store.OpenSession("Northwind"))
				{
					// ...
				}
				#endregion
			}
		}
	}
}