using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.Samples.ClientApi.Commands.HowTo
{
	public class GetDatabaseConfiguration
	{
		private interface IFoo
		{
			#region database_configuration_1
			RavenJObject GetDatabaseConfiguration();
			#endregion
		}

		public GetDatabaseConfiguration()
		{
			using (var store = new DocumentStore())
			{
				#region database_configuration_2
				RavenJObject configuration = store
					.DatabaseCommands
					.Admin
					.GetDatabaseConfiguration();
				#endregion
			}
		}
	}
}