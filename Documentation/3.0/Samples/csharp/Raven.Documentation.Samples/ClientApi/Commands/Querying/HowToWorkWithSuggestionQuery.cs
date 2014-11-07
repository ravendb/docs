using System;

using Raven.Abstractions.Data;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.Commands.Querying
{
	public class HowToWorkWithSuggestionQuery
	{
		private interface IFoo
		{
			#region suggest_1
			SuggestionQueryResult Suggest(string index, SuggestionQuery suggestionQuery);
			#endregion
		}

		public HowToWorkWithSuggestionQuery()
		{
			using (var store = new DocumentStore())
			{
				#region suggest_2
				// Get suggestions for 'johne' using 'FullName' field in 'Users/ByFullName' index
				var result = store
					.DatabaseCommands
					.Suggest(
						"Users/ByFullName",
						new SuggestionQuery
							{
								Field = "FullName",
								Term = "johne",
								MaxSuggestions = 10
							});

				Console.WriteLine("Did you mean?");

				foreach (var suggestion in result.Suggestions)
				{
					Console.WriteLine("\t{0}", suggestion);
				}

				// Did you mean?
				//		john
				//		jones
				//		johnson
				#endregion
			}
		}
	}
}