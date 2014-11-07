using System.Collections.Generic;

using Raven.Abstractions.Data;
using Raven.Client.Document;
using Raven.Json.Linq;

namespace Raven.Documentation.Samples.ClientApi.Commands.Querying
{
	public class HowToStreamQueryResults
	{
		private interface IFoo
		{
			#region stream_query_1
			IEnumerator<RavenJObject> StreamQuery(
				string index,
				IndexQuery query,
				out QueryHeaderInformation queryHeaderInfo);
			#endregion
		}

		public HowToStreamQueryResults()
		{
			using (var store = new DocumentStore())
			{
				#region stream_query_2
				QueryHeaderInformation queryHeaderInfo;
				var enumerator = store
					.DatabaseCommands
					.StreamQuery(
						"Orders/Totals",
						new IndexQuery
						{
							Query = "Company:companies/1"
						},
						out queryHeaderInfo);

				while (enumerator.MoveNext())
				{
					var order = enumerator.Current;
				}
				#endregion
			}
		}
	}
}