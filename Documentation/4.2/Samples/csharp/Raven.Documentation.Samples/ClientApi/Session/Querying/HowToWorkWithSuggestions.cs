using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries.Suggestions;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToWorkWithSuggestions
    {
        private class Foo
        {
            #region suggest_7
            public int PageSize { get; set; } = 15;

            public StringDistanceTypes? Distance { get; set; } = StringDistanceTypes.Levenshtein;

            public float? Accuracy { get; set; } = 0.5f;

            public SuggestionSortMode SortMode { get; set; } = SuggestionSortMode.Popularity;
            #endregion
        }

        private interface IFoo<T>
        {
            #region suggest_1
            ISuggestionQuery<T> SuggestUsing<T>(SuggestionBase suggestion);

            ISuggestionQuery<T> SuggestUsing<T>(Action<ISuggestionBuilder<T>> builder);
            #endregion

            #region suggest_2
            ISuggestionOperations<T> ByField(string fieldName, string term);

            ISuggestionOperations<T> ByField(string fieldName, string[] terms);

            ISuggestionOperations<T> ByField(Expression<Func<T, object>> path, string term);

            ISuggestionOperations<T> ByField(Expression<Func<T, object>> path, string[] terms);

            ISuggestionOperations<T> WithOptions(SuggestionOptions options);
            #endregion
        }

        private class Employees_ByFullName : AbstractIndexCreationTask<Employee>
        {
        }

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region suggest_5
                    Dictionary<string, SuggestionResult> suggestions = session
                        .Query<Employee, Employees_ByFullName>()
                        .SuggestUsing(builder => builder
                            .ByField("FullName", "johne")
                            .WithOptions(new SuggestionOptions
                            {
                                Accuracy = 0.4f,
                                PageSize = 5,
                                Distance = StringDistanceTypes.JaroWinkler,
                                SortMode = SuggestionSortMode.Popularity
                            }))
                        .Execute();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region suggest_6
                    Dictionary<string, SuggestionResult> suggestions = await asyncSession
                        .Query<Employee, Employees_ByFullName>()
                        .SuggestUsing(builder => builder
                            .ByField("FullName", "johne")
                            .WithOptions(new SuggestionOptions
                            {
                                Accuracy = 0.4f,
                                PageSize = 5,
                                Distance = StringDistanceTypes.JaroWinkler,
                                SortMode = SuggestionSortMode.Popularity
                            }))
                        .ExecuteAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region suggest_8
                    Dictionary<string, SuggestionResult> suggestions = session
                        .Query<Employee, Employees_ByFullName>()
                        .SuggestUsing(new SuggestionWithTerm("FullName") { Term = "johne" })
                        .Execute();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region suggest_9
                    Dictionary<string, SuggestionResult> suggestions = await asyncSession
                        .Query<Employee, Employees_ByFullName>()
                        .SuggestUsing(new SuggestionWithTerm("FullName") { Term = "johne" })
                        .ExecuteAsync();
                    #endregion
                }
            }
        }
    }
}
