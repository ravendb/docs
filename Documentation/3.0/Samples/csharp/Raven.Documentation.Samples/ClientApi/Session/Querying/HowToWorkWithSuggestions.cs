using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Indexes.Querying;

namespace Raven.Documentation.CodeSamples.ClientApi.Session.Querying
{
	public class HowToWorkWithSuggestions
	{
		private interface IFoo
		{
			/*
			#region suggest_1
			SuggestionQueryResult Suggest(
				this IQueryable queryable) { ... }

			SuggestionQueryResult Suggest(
				this IQueryable queryable,
				SuggestionQuery query) { ... }
			#endregion
			*/
		}

		private class Users_ByFullName : AbstractIndexCreationTask<User>
		{
		}

		public HowToWorkWithSuggestions()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region suggest_2

					var suggestions = session
						.Query<User, Users_ByFullName>()
						.Suggest(
							new SuggestionQuery
								{
									Field = "FullName",
									Term = "johne",
									Accuracy = 0.4f,
									MaxSuggestions = 5,
									Distance = StringDistanceTypes.JaroWinkler,
									Popularity = true,
								});
					#endregion
				}
			}
		}
	}
}