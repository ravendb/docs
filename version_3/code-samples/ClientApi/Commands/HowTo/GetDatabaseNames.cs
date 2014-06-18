using Raven.Client.Document;

namespace Raven.Documentation.CodeSamples.ClientApi.Commands.HowTo
{
	public class GetDatabaseNames
	{
		private interface IFoo
		{
			#region get_database_names_1
			string[] GetDatabaseNames(int pageSize, int start = 0);
			#endregion
		}

		public GetDatabaseNames()
		{
			using (var store = new DocumentStore())
			{
				#region get_database_names_2
				var databaseNames = store
					.DatabaseCommands
					.GetDatabaseNames(0, 10);
				#endregion
			}
		}
	}
}