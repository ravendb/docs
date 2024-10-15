using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries.Facets;
using Raven.Client.Documents.Queries.Suggestions;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToPerformQueriesLazily
    {
        public async Task LazyQueries()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region lazy_1
                    // Define a lazy query
                    Lazy<IEnumerable<Employee>> lazyEmployees = session
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert")
                         // Add a call to 'Lazily'
                        .Lazily();

                    IEnumerable<Employee> employees = lazyEmployees.Value; // Query is executed here
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region lazy_2
                    // Define a lazy query
                    Lazy<Task<IEnumerable<Employee>>> lazyEmployees = asyncSession
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert")
                         // Add a call to 'LazilyAsync'
                        .LazilyAsync();

                    IEnumerable<Employee> employees = await lazyEmployees.Value; // Query is executed here
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region lazy_3
                    // Define a lazy DocumentQuery
                    Lazy<IEnumerable<Employee>> lazyEmployees = session.Advanced
                        .DocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Robert")
                         // Add a call to 'Lazily'
                        .Lazily();

                    IEnumerable<Employee> employees = lazyEmployees.Value; // DocumentQuery is executed here
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region lazy_4
                    // Define a lazy count query
                    Lazy<int> lazyCount = session
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert")
                         // Add a call to 'CountLazily'
                        .CountLazily();

                    int count = lazyCount.Value; // Query is executed here
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region lazy_5
                    // Define a lazy count query
                    Lazy<Task<int>> lazyCount = asyncSession
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert")
                         // Add a call to 'CountLazilyAsync'
                        .CountLazilyAsync();

                    int count = await lazyCount.Value; // Query is executed here
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region lazy_6
                    // Define a lazy DocumentQuery
                    Lazy<int> lazyCount = session.Advanced
                        .DocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Robert")
                         // Add a call to 'CountLazily'
                        .CountLazily();

                    int count = lazyCount.Value; // DocumentQuery is executed here
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region lazy_7
                    // Define a lazy suggestion query
                    Lazy<Dictionary<string, SuggestionResult>> lazySuggestions = session
                        .Query<Product>()
                        .SuggestUsing(builder => builder.ByField(x => x.Name, "chaig"))
                         // Add a call to 'ExecuteLazy'
                        .ExecuteLazy();

                    Dictionary<string, SuggestionResult> suggest = lazySuggestions.Value; // Query is executed here
                    List<string> suggestions = suggest["Name"].Suggestions;
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region lazy_8
                    // Define a lazy suggestion query
                    Lazy<Task<Dictionary<string, SuggestionResult>>> lazySuggestions = asyncSession
                        .Query<Employee>()
                        .SuggestUsing(builder => builder.ByField("Name", "chaig"))
                         // Add a call to 'ExecuteLazyAsync'
                        .ExecuteLazyAsync();

                    Dictionary<string, SuggestionResult> suggest = await lazySuggestions.Value; // Query is executed here
                    List<string> suggestions = suggest["Name"].Suggestions;
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region lazy_9
                    // Define a lazy DocumentQuery
                    Lazy<Dictionary<string, SuggestionResult>> lazySuggestions = session.Advanced
                        .DocumentQuery<Employee>()
                        .SuggestUsing(builder => builder.ByField("Name", "chaig"))
                         // Add a call to 'ExecuteLazy'
                        .ExecuteLazy();

                    Dictionary<string, SuggestionResult> suggest = lazySuggestions.Value; // DocumentQuery is executed here
                    List<string> suggestions = suggest["FullName"].Suggestions;
                    #endregion
                }

                #region the_facets
                // The facets definition used in the facets query:
                List<FacetBase> facetsDefinition = new List<FacetBase>
                {
                    new Facet
                    {
                        FieldName = "CategoryName",
                        DisplayFieldName = "Product Category"
                    },
                    new RangeFacet<Product>
                    {
                        Ranges =
                        {
                            product => product.PricePerUnit < 25,
                            product => product.PricePerUnit >= 25 && product.PricePerUnit < 50,
                            product => product.PricePerUnit >= 50 && product.PricePerUnit < 100,
                            product => product.PricePerUnit >= 100
                        },
                        DisplayFieldName = "Price per Unit"
                    }
                };
                #endregion
                
                using (var session = store.OpenSession())
                {
                    #region lazy_10
                    // Define a lazy facets query
                    Lazy<Dictionary<string, FacetResult>> lazyFacets = session
                        .Query<Product, Products_ByCategoryAndPrice>()
                        .AggregateBy(facetsDefinition)
                         // Add a call to 'ExecuteLazy'
                        .ExecuteLazy();

                    Dictionary<string, FacetResult> facets = lazyFacets.Value; // Query is executed here
                    
                    FacetResult categoryResults = facets["Product Category"];
                    FacetResult priceResults = facets["Price per Unit"];
                    #endregion
                }
                
                using (var asyncSession = store.OpenSession())
                {
                    #region lazy_11
                    // Define a lazy DocumentQuery
                    Lazy<Task<Dictionary<string,FacetResult>>> lazyFacets = asyncSession
                        .Query<Product, Products_ByCategoryAndPrice>()
                        .AggregateBy(facetsDefinition)
                         // Add a call to 'ExecuteLazyAsync'
                        .ExecuteLazyAsync();

                    Dictionary<string, FacetResult> facets = await lazyFacets.Value; // Query is executed here

                    FacetResult categoryResults = facets["Product Category"];
                    FacetResult priceResults = facets["Price per Unit"];
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region lazy_12
                    // Define a lazy DocumentQuery
                    Lazy<Dictionary<string, FacetResult>> lazyFacets = session.Advanced
                        .DocumentQuery<Product, Products_ByCategoryAndPrice>()
                        .AggregateBy(facetsDefinition)
                         // Add a call to 'ExecuteLazy'
                        .ExecuteLazy();

                    Dictionary<string, FacetResult> facets = lazyFacets.Value; //  DocumentQuery is executed here

                    FacetResult categoryResults = facets["Product Category"];
                    FacetResult priceResults = facets["Price per Unit"];
                    #endregion
                }
            }
        }
        
        private interface IFoo
        {
            #region syntax_1
            // Lazy query overloads:
            Lazy<IEnumerable<T>> Lazily<T>();
            Lazy<IEnumerable<T>> Lazily<T>(Action<IEnumerable<T>> onEval);

            Lazy<Task<IEnumerable<T>>> LazilyAsync<T>();
            Lazy<Task<IEnumerable<T>>> LazilyAsync<T>(Action<IEnumerable<T>> onEval);
            #endregion

            #region syntax_2
            // Lazy count query overloads:
            Lazy<int> CountLazily<T>();
            Lazy<long> LongCountLazily<T>();
            
            Lazy<Task<int>> CountLazilyAsync<T>(CancellationToken token = default(CancellationToken));
            Lazy<Task<long>> LongCountLazilyAsync<T>(CancellationToken token = default(CancellationToken));
            #endregion

            #region syntax_3
            // Lazy suggestions query overloads:
            Lazy<Dictionary<string, SuggestionResult>>
                ExecuteLazy(Action<Dictionary<string, SuggestionResult>> onEval = null);

            Lazy<Task<Dictionary<string, SuggestionResult>>>
                ExecuteLazyAsync(Action<Dictionary<string, SuggestionResult>> onEval = null,
                    CancellationToken token = default);
            #endregion

            #region syntax_4
            // Lazy facets query overloads:
            Lazy<Dictionary<string, FacetResult>>
                ExecuteLazy(Action<Dictionary<string, FacetResult>> onEval = null);
            
            Lazy<Task<Dictionary<string, FacetResult>>>
                ExecuteLazyAsync(Action<Dictionary<string, FacetResult>> onEval = null,
                    CancellationToken token = default);
            #endregion
        }
    }
    
    #region the_index
    // The index definition used in the facets query:
    public class Products_ByCategoryAndPrice :
        AbstractIndexCreationTask<Product, Products_ByCategoryAndPrice.IndexEntry>
    {
        // The IndexEntry class defines the index-fields
        public class IndexEntry
        {
            public string CategoryName { get; set; }
            public decimal PricePerUnit { get; set; }
        }
            
        public Products_ByCategoryAndPrice()
        {
            // The 'Map' function defines the content of the index-fields
            Map = products => from product in products
                select new IndexEntry
                {
                    CategoryName = LoadDocument<Category>(product.Category).Name,
                    PricePerUnit = product.PricePerUnit
                };
        }
    }
    #endregion
}
