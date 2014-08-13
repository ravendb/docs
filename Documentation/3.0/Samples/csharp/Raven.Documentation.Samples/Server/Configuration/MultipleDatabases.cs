using Raven.Client.Document;
#region multiple_databases_2
using Raven.Client.Extensions; // required namespace in usings
#endregion

namespace Raven.Documentation.CodeSamples.Server.Configuration
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