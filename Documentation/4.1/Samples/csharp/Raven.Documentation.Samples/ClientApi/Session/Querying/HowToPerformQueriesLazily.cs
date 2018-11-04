using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Queries.Facets;
using Raven.Client.Documents.Queries.Suggestions;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToPerformQueriesLazily
    {
        private interface IFoo
        {
            #region lazy_1
            Lazy<IEnumerable<T>> Lazily<T>();

            Lazy<IEnumerable<T>> Lazily<T>(Action<IEnumerable<T>> onEval);

            Lazy<Task<IEnumerable<T>>> LazilyAsync<T>();

            Lazy<Task<IEnumerable<T>>> LazilyAsync<T>(Action<IEnumerable<T>> onEval);
            #endregion

            #region lazy_4
            Lazy<int> CountLazily<T>();

            Lazy<Task<int>> CountLazilyAsync<T>(CancellationToken token = default(CancellationToken));
            #endregion


            #region lazy_6
            Lazy<Dictionary<string, SuggestionResult>> ExecuteLazy(Action<Dictionary<string, SuggestionResult>> onEval = null);

            Lazy<Task<Dictionary<string, SuggestionResult>>> ExecuteLazyAsync(Action<Dictionary<string, SuggestionResult>> onEval = null);
            #endregion

            #region lazy_8
            Lazy<Dictionary<string, FacetResult>> ExecuteLazy(Action<Dictionary<string, FacetResult>> onEval = null);

            Lazy<Task<Dictionary<string, FacetResult>>> ExecuteLazyAsync(Action<Dictionary<string, FacetResult>> onEval = null);
            #endregion
        }

        public HowToPerformQueriesLazily()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region lazy_2
                    Lazy<IEnumerable<Employee>> employeesLazy = session
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert")
                        .Lazily();

                    IEnumerable<Employee> employees = employeesLazy.Value; // query will be executed here
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region lazy_5
                    Lazy<int> countLazy = session
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert")
                        .CountLazily();

                    int count = countLazy.Value; // query will be executed here
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region lazy_7
                    Lazy<Dictionary<string, SuggestionResult>> suggestLazy = session
                        .Query<Employee>("Employees_ByFullName")
                        .SuggestUsing(builder => builder.ByField("FullName", "johne"))
                        .ExecuteLazy();

                    Dictionary<string, SuggestionResult> suggest = suggestLazy.Value; // query will be executed here
                    List<string> suggestions = suggest["FullName"].Suggestions;
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region lazy_9
                    Lazy<Dictionary<string, FacetResult>> facetsLazy = session
                        .Query<Camera>("Camera/Costs")
                        .AggregateUsing("facets/CameraFacets")
                        .ExecuteLazy();

                    Dictionary<string, FacetResult> facets = facetsLazy.Value; // query will be executed here
                    FacetResult results = facets["Manufacturer"];
                    #endregion
                }
            }
        }

        private async Task LazilyAsync()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region lazy_3
                    Lazy<Task<IEnumerable<Employee>>> employeesLazy = session
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert")
                        .LazilyAsync();

                    IEnumerable<Employee> employees = await employeesLazy.Value; // query will be executed here
                    #endregion
                }
            }
        }
    }
}
