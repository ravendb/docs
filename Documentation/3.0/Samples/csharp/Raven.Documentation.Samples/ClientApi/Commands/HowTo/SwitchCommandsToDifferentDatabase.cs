using Raven.Client.Connection;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.HowTo
{
	public class SwitchCommandsToDifferentDatabase
	{
		private interface IFoo
		{
			#region for_database_1
			IDatabaseCommands ForDatabase(string database);
			#endregion

			#region for_database_2
			IDatabaseCommands ForSystemDatabase();
			#endregion
		}

		public SwitchCommandsToDifferentDatabase()
		{
			using (var store = new DocumentStore())
			{
				#region for_database_3
				var commands = store.DatabaseCommands.ForDatabase("otherDatabase");
				#endregion

				#region for_database_4
				var systemCommands = store.DatabaseCommands.ForSystemDatabase();
				#endregion
			}
		}
	}
}