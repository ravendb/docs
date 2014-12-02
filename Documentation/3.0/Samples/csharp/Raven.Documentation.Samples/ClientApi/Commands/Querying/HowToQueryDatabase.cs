using System.Collections.Generic;

using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.Samples.ClientApi.Commands.Querying
{
	public class HowToQueryDatabase
	{
		private interface IFoo
		{
			#region query_database_1
			QueryResult Query(
				string index,
				IndexQuery query,
				string[] includes = null, 
				bool metadataOnly = false,
				bool indexEntriesOnly = false);
			#endregion
		}

		public HowToQueryDatabase()
		{
			using (var store = new DocumentStore())
			{
				QueryResult result;

				#region query_database_2
				result = store.DatabaseCommands.Query("Orders/Totals", new IndexQuery
				{
					Query = "Company:companies/1"
				});

				List<RavenJObject> users = result.Results; // documents resulting from this query - orders
				#endregion

				#region query_database_3

				result = store.DatabaseCommands.Query("Orders/Totals", new IndexQuery(),
					new[]
					{
						"Company",
						"Employee"
					});

				List<RavenJObject> referencedDocs = result.Includes; // included documents - companies and employees
				#endregion
			}
		} 
	}
}