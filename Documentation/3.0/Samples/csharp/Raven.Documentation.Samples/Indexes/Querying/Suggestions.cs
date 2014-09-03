using System;
using System.Linq;

using Raven.Abstractions.Data;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
	public class Suggestions
	{
		#region suggestions_1
		public class Products_ByName : AbstractIndexCreationTask<Product>
		{
			public Products_ByName()
			{
				Map = products => from product in products
								  select new
									{
										product.Name
									};

				Indexes.Add(x => x.Name, FieldIndexing.Analyzed);	// (optional) splitting name into multiple tokens
				Suggestion(x => x.Name);				// configuring suggestions
			}
		}
		#endregion

		public Suggestions()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region suggestions_2
					var query = session
						.Query<Product, Products_ByName>()
						.Where(x => x.Name == "chaig");

					var product = query.FirstOrDefault();
					#endregion

					#region suggestions_3
					if (product == null)
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
					session
						.Query<Product, Products_ByName>()
						.Suggest(
							new SuggestionQuery
							{
								Field = "Name",
								Term = "chaig",
								Accuracy = 0.4f,
								MaxSuggestions = 5,
								Distance = StringDistanceTypes.JaroWinkler,
								Popularity = true
							});
					#endregion

					#region document_store_suggestion
					store
						.DatabaseCommands
						.Suggest(
							"Products/ByName",
							new SuggestionQuery
							{
								Field = "Name",
								Term = "chaig",
								Accuracy = 0.4f,
								MaxSuggestions = 5,
								Distance = StringDistanceTypes.JaroWinkler,
								Popularity = true
							});
					#endregion

					#region query_suggestion_over_multiple_words
					SuggestionQueryResult resultsByMultipleWords =
						session
						.Query<Product, Products_ByName>()
						.Suggest(new SuggestionQuery
						{
							Field = "Name",
							Term = "<<chaig tof>>",
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