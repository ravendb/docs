using System.Collections.Generic;
using Raven.Client.Documents.Indexes;

#region suggestions_4
using Raven.Client.Documents;
using Raven.Client.Documents.Queries.Suggestions;
#endregion

/*
#region suggestions_3
using Raven.Abstractions.Data;
using Raven.Client;
#endregion
*/

using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Migration.ClientApi.Session.Querying
{
    public class Suggestions
    {
        private class Foo
        {
            public Foo()
            {
                /*
                #region suggestions_1
                SuggestionQueryResult suggestions = session
                    .Query<Employee, Employees_ByFullName>()
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
                */
            }
        }

        public Suggestions()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region suggestions_2
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
            }
        }

        private class Employees_ByFullName : AbstractIndexCreationTask<Employee>
        {
        }
    }
}
