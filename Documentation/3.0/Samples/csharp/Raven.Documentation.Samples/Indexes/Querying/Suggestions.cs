using System;
using System.Linq;

using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.CodeSamples.Indexes.Querying
{
	public class Suggestions
	{
		#region user_class
		public class User
		{
			public string Id { get; set; }
			public string FullName { get; set; }
		}
		#endregion

		#region index_def
		public class Users_ByFullName : AbstractIndexCreationTask<User>
		{
			public Users_ByFullName()
			{
				Map = users => from user in users
							   select new { user.FullName };

				Indexes.Add(x => x.FullName, FieldIndexing.Analyzed);
			}
		}
		#endregion

		public Suggestions()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region query
					var query = session.Query<User, Users_ByFullName>().Where(x => x.FullName == "johne");

					var user = query.FirstOrDefault();
					#endregion

					#region query_suggestion
					if (user == null)
					{
						SuggestionQueryResult suggestionResult = query.Suggest();

						Console.WriteLine("Did you mean?");

						foreach (var suggestion in suggestionResult.Suggestions)
						{
							Console.WriteLine("\t{0}", suggestion);
						}
					}
					#endregion

					#region query_suggestion_with_options

					session.Query<User, Users_ByFullName>()
						   .Suggest(new SuggestionQuery()
						   {
							   Field = "FullName",
							   Term = "johne",
							   Accuracy = 0.4f,
							   MaxSuggestions = 5,
							   Distance = StringDistanceTypes.JaroWinkler,
							   Popularity = true,
						   });
					#endregion

					#region document_store_suggestion
					store.DatabaseCommands.Suggest("Users/ByFullName", new SuggestionQuery()
					{
						Field = "FullName",
						Term = "johne"
					});

					#endregion

					#region query_suggestion_over_multiple_words

					SuggestionQueryResult resultsByMultipleWords = session.Query<User, Users_ByFullName>()
						   .Suggest(new SuggestionQuery()
						   {
							   Field = "FullName",
							   Term = "<<johne davi>>",
							   Accuracy = 0.4f,
							   MaxSuggestions = 5,
							   Distance = StringDistanceTypes.JaroWinkler,
							   Popularity = true,
						   });

					Console.WriteLine("Did you mean?");

					foreach (var suggestion in resultsByMultipleWords.Suggestions)
					{
						Console.WriteLine("\t{0}", suggestion);
					}
					#endregion
				}
			}
		}
	}
}