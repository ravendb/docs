using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.HowTo
{
	public class GetDatabaseAndServerStatistics
	{
		private interface IFoo
		{
			#region database_statistics_1
			DatabaseStatistics GetStatistics();
			#endregion
		}

		private interface IFoo2
		{
			#region server_statistics_1
			AdminStatistics GetStatistics();
			#endregion
		}

		public GetDatabaseAndServerStatistics()
		{
			using (var store = new DocumentStore())
			{
				#region database_statistics_2
				DatabaseStatistics statistics = store.DatabaseCommands.GetStatistics();
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region server_statistics_2
				AdminStatistics serverStatistics = store
					.DatabaseCommands
					.GlobalAdmin
					.GetStatistics();
				#endregion
			}
		}
	}
}