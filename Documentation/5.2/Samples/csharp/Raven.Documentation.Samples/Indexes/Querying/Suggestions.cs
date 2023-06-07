using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries.Suggestions;
using Raven.Documentation.Samples.Orders;

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

                Indexes.Add(x => x.Name, FieldIndexing.Search);     // (optional) splitting name into multiple tokens
                Suggestion(x => x.Name);                            // configuring suggestions
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
                    var product = session
                        .Query<Product, Products_ByName>()
                        .Search(x => x.Name, "chaig")
                        .FirstOrDefault();
                    #endregion

                    #region suggestions_3
                    if (product == null)
                    {
                        Dictionary<string, SuggestionResult> suggestionResult = session
                            .Query<Product, Products_ByName>()
                            .SuggestUsing(builder => builder.ByField(x => x.Name, "chaig"))
                            .Execute();

                        Console.WriteLine("Did you mean?");

                        foreach (string suggestion in suggestionResult["Name"].Suggestions)
                        {
                            Console.WriteLine("\t{0}", suggestion);
                        }
                    }
                    #endregion

                    #region query_suggestion_over_multiple_words
                    var resultsByMultipleWords = session
                        .Query<Product, Products_ByName>()
                        .SuggestUsing(builder => builder
                            .ByField(x => x.Name, new[] { "chaig", "tof" })
                            .WithOptions(new SuggestionOptions
                            {
                                Accuracy = 0.4f,
                                PageSize = 5,
                                Distance = StringDistanceTypes.JaroWinkler,
                                SortMode = SuggestionSortMode.Popularity
                            }))
                        .Execute();

                    Console.WriteLine("Did you mean?");

                    foreach (string suggestion in resultsByMultipleWords["Name"].Suggestions)
                    {
                        Console.WriteLine("\t{0}", suggestion);
                    }
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    Product product = null;

                    #region suggestions_4
                    if (product == null)
                    {
                        Dictionary<string, SuggestionResult> suggestionResult = session.Advanced
                            .DocumentQuery<Product, Products_ByName>()
                            .SuggestUsing(builder => builder.ByField(x => x.Name, "chaig"))
                            .Execute();

                        Console.WriteLine("Did you mean?");

                        foreach (string suggestion in suggestionResult["Name"].Suggestions)
                        {
                            Console.WriteLine("\t{0}", suggestion);
                        }
                    }
                    #endregion

                    #region query_suggestion_over_multiple_words_1
                    var resultsByMultipleWords = session.Advanced
                        .DocumentQuery<Product, Products_ByName>()
                        .SuggestUsing(builder => builder
                            .ByField(x => x.Name, new[] { "chaig", "tof" })
                            .WithOptions(new SuggestionOptions
                            {
                                Accuracy = 0.4f,
                                PageSize = 5,
                                Distance = StringDistanceTypes.JaroWinkler,
                                SortMode = SuggestionSortMode.Popularity
                            }))
                        .Execute();

                    Console.WriteLine("Did you mean?");

                    foreach (string suggestion in resultsByMultipleWords["Name"].Suggestions)
                    {
                        Console.WriteLine("\t{0}", suggestion);
                    }
                    #endregion
                }
            }
        }
    }
}
