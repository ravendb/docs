using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region suggest_1
                    // This dynamic query on the 'Products' collection has NO resulting documents
                    var product = session
                        .Query<Product>()
                        .Where(x => x.Name == "chaig")
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggest_2
                    // Query for suggested terms for single term:
                    // ==========================================
                    
                    Dictionary<string, SuggestionResult> suggestions = session
                         // Make a dynamic query on collection 'Products'
                        .Query<Product>()
                         // Call 'SuggestUsing'
                        .SuggestUsing(builder => builder
                             // Request to get terms from field 'Name' that are similar to 'chaig' 
                            .ByField(x => x.Name, "chaig"))
                        .Execute();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region suggest_3
                    // Query for suggested terms for single term:
                    // ==========================================
                    
                    Dictionary<string, SuggestionResult> suggestions = await asyncSession
                         // Make a dynamic query on collection 'Products'
                        .Query<Product>()
                         // Call 'SuggestUsing'
                        .SuggestUsing(builder => builder
                             // Request to get terms from field 'Name' that are similar to 'chaig'  
                            .ByField(x => x.Name, "chaig"))
                        .ExecuteAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region suggest_4
                    // Define the suggestion request for single term
                    var suggestionRequest = new SuggestionWithTerm("Name")
                    {
                        // Looking for terms from field 'Name' that are similar to term 'chaig'  
                        Term = "chaig"
                    };
                    
                    // Query for suggestions 
                    Dictionary<string, SuggestionResult> suggestions = session
                        .Query<Product>()
                         // Call 'SuggestUsing' - pass the suggestion request
                        .SuggestUsing(suggestionRequest)
                        .Execute();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggest_5
                    // Query for suggested terms for single term:
                    // ==========================================
                    
                    Dictionary<string, SuggestionResult> suggestions = session.Advanced
                         // Make a dynamic document-query on collection 'Products'
                        .DocumentQuery<Product>()
                         // Call 'SuggestUsing' 
                        .SuggestUsing(builder => builder
                             // Request to get terms from field 'Name' that are similar to 'chaig' 
                            .ByField(x => x.Name, "chaig"))
                        .Execute();
                    #endregion
                    
                    #region suggest_6
                    // The resulting suggested terms:
                    // ==============================
                    
                    Console.WriteLine("Suggested terms in field 'Name' that are similar to 'chaig':");
                    foreach (string suggestedTerm in suggestions["Name"].Suggestions)
                    {
                        Console.WriteLine("\t{0}", suggestedTerm);
                    }
                    
                    // Suggested terms in field 'Name' that are similar to 'chaig':
                    //     chai
                    //     chang
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggest_7
                    // Query for suggested terms for multiple terms:
                    // =============================================
                    
                    Dictionary<string, SuggestionResult> suggestions = session
                         // Make a dynamic query on collection 'Products'
                        .Query<Product>()
                         // Call 'SuggestUsing'
                        .SuggestUsing(builder => builder
                             // Request to get terms from field 'Name' that are similar to 'chaig' OR 'tof' 
                            .ByField(x => x.Name, new[] { "chaig", "tof" }))
                        .Execute();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region suggest_8
                    // Query for suggested terms for multiple terms:
                    // =============================================
                    
                    Dictionary<string, SuggestionResult> suggestions = await asyncSession
                         // Make a dynamic query on collection 'Products'
                        .Query<Product>()
                         // Call 'SuggestUsing' 
                        .SuggestUsing(builder => builder
                             // Request to get terms from field 'Name' that are similar to 'chaig' OR 'tof'
                            .ByField(x => x.Name, new[] { "chaig", "tof" }))
                        .ExecuteAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region suggest_9
                    // Define the suggestion request for multiple terms
                    var suggestionRequest = new SuggestionWithTerms("Name")
                    {
                        // Looking for terms from field 'Name' that are similar to terms 'chaig' OR 'tof'  
                        Terms = new[] { "chaig", "tof"}
                    };
                    
                    // Query for suggestions 
                    Dictionary<string, SuggestionResult> suggestions = session
                        .Query<Product>()
                         // Call 'SuggestUsing' - pass the suggestion request
                        .SuggestUsing(suggestionRequest)
                        .Execute();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region suggest_10
                    // Query for suggested terms for multiple terms:
                    // =============================================
                    
                    Dictionary<string, SuggestionResult> suggestions = session.Advanced
                         // Make a dynamic document-query on collection 'Products'
                        .DocumentQuery<Product>()
                         // Call 'SuggestUsing'
                        .SuggestUsing(builder => builder
                             // Request to get terms from field 'Name' that are similar to 'chaig' OR 'tof' 
                            .ByField(x => x.Name, new[] { "chaig", "tof" }))
                        .Execute();
                    #endregion
                    
                    #region suggest_11
                    // The resulting suggested terms:
                    // ==============================
                    
                    // Suggested terms in field 'Name' that are similar to 'chaig' or to 'tof':
                    //     chai
                    //     chang
                    //     tofu
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region suggest_12
                    // Query for suggested terms in multiple fields:
                    // =============================================
                    
                    Dictionary<string, SuggestionResult> suggestions = session
                         // Make a dynamic query on collection 'Companies'
                        .Query<Company>()
                         // Call 'SuggestUsing' to get suggestions for terms that are 
                         // similar to 'chaig' in first document field (e.g. 'Name') 
                        .SuggestUsing(builder => builder
                            .ByField(x => x.Name, "chop-soy china"))
                         // Call 'AndSuggestUsing' to get suggestions for terms that are 
                         // similar to 'maria larson' in an additional field (e.g. 'Contact.Name')
                        .AndSuggestUsing(builder => builder
                            .ByField(x => x.Contact.Name, "maria larson"))
                        .Execute();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region suggest_13
                    // Query for suggested terms in multiple fields:
                    // =============================================
                    
                    Dictionary<string, SuggestionResult> suggestions = await asyncSession
                         // Make a dynamic query on collection 'Companies'   
                        .Query<Company>()
                         // Call 'SuggestUsing' to get suggestions for terms that are 
                         // similar to 'chaig' in first document field (e.g. 'Name') 
                        .SuggestUsing(builder => builder
                            .ByField(x => x.Name, "chop-soy china"))
                         // Call 'AndSuggestUsing' to get suggestions for terms that are 
                         // similar to 'maria larson' in an additional field (e.g. 'Contact.Name')
                        .AndSuggestUsing(builder => builder
                            .ByField(x => x.Contact.Name, "maria larson"))
                        .ExecuteAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggest_14
                    // Define suggestion requests for multiple fields:
                    
                    var request1 = new SuggestionWithTerm("Name")
                    {
                        // Looking for terms from field 'Name' that are similar to 'chop-soy china'  
                        Term = "chop-soy china"
                    };
                    
                    var request2 = new SuggestionWithTerm("Contact.Name")
                    {
                        // Looking for terms from nested field 'Contact.Name' that are similar to 'maria larson'  
                        Term = "maria larson"
                    };
                    
                    Dictionary<string, SuggestionResult> suggestions = session
                        .Query<Company>()
                         // Call 'SuggestUsing' - pass the suggestion request for the first field
                        .SuggestUsing(request1)
                         // Call 'AndSuggestUsing' - pass the suggestion request for the second field
                        .AndSuggestUsing(request2)
                        .Execute();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggest_15
                    // Query for suggested terms in multiple fields:
                    // =============================================
                    
                    Dictionary<string, SuggestionResult> suggestions = session.Advanced
                         // Make a dynamic document-query on collection 'Companies'
                        .DocumentQuery<Company>()
                         // Call 'SuggestUsing' to get suggestions for terms that are 
                         // similar to 'chaig' in first document field (e.g. 'Name') 
                        .SuggestUsing(builder => builder
                            .ByField(x => x.Name, "chop-soy china"))
                         // Call 'AndSuggestUsing' to get suggestions for terms that are 
                         // similar to 'maria larson' in an additional field (e.g. 'Contact.Name')
                        .AndSuggestUsing(builder => builder
                            .ByField(x => x.Contact.Name, "maria larson"))
                        .Execute();
                    #endregion
                    
                    #region suggest_16
                    // The resulting suggested terms:
                    // ==============================
                    
                    // Suggested terms in field 'Name' that is similar to 'chop-soy china':
                    //     chop-suey chinese
                    
                    // Suggested terms in field 'Contact.Name' that are similar to 'maria larson':
                    //     maria larsson
                    //     marie bertrand
                    //     aria cruz
                    //     paula wilson
                    //     maria anders
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggest_17
                    // Query for suggested terms - customize options and display name:
                    // ===============================================================
                    
                    Dictionary<string, SuggestionResult> suggestions = session
                         // Make a dynamic query on collection 'Products'
                        .Query<Product>()
                         // Call 'SuggestUsing'
                        .SuggestUsing(builder => builder
                            .ByField(x => x.Name, "chaig")
                             // Customize suggestions options
                            .WithOptions(new SuggestionOptions
                            {
                                Accuracy = 0.4f,
                                PageSize = 5,
                                Distance = StringDistanceTypes.JaroWinkler,
                                SortMode = SuggestionSortMode.Popularity
                            })
                             // Customize display name for results
                            .WithDisplayName("SomeCustomName"))
                        .Execute();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region suggest_18
                    // Query for suggested terms - customize options and display name:
                    // ===============================================================

                    Dictionary<string, SuggestionResult> suggestions = await asyncSession
                         // Make a dynamic query on collection 'Products'
                        .Query<Product>()
                         // Call 'SuggestUsing'
                        .SuggestUsing(builder => builder
                            .ByField(x => x.Name, "chaig")
                             // Customize suggestions options
                            .WithOptions(new SuggestionOptions
                            {
                                Accuracy = 0.4f,
                                PageSize = 5,
                                Distance = StringDistanceTypes.JaroWinkler,
                                SortMode = SuggestionSortMode.Popularity
                            })
                             // Customize display name for results
                            .WithDisplayName("SomeCustomName"))
                        .ExecuteAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggest_19
                    // Define the suggestion request
                    var suggestionRequest = new SuggestionWithTerm("Name")
                    {
                        // Looking for terms from field 'Name' that are similar to term 'chaig'  
                        Term = "chaig",
                        // Customize options
                        Options = new SuggestionOptions 
                        {
                            Accuracy = 0.4f,
                            PageSize = 5,
                            Distance = StringDistanceTypes.JaroWinkler,
                            SortMode = SuggestionSortMode.Popularity
                        },
                        // Customize display name
                        DisplayField = "SomeCustomName"
                    };
                    
                    // Query for suggestions 
                    Dictionary<string, SuggestionResult> suggestions = session
                        .Query<Product>()
                        // Call 'SuggestUsing' - pass the suggestion request
                        .SuggestUsing(suggestionRequest)
                        .Execute();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggest_20
                    // Query for suggested terms - customize options and display name:
                    // ===============================================================
                    
                    Dictionary<string, SuggestionResult> suggestions = session.Advanced
                         // Make a dynamic query on collection 'Products'
                        .DocumentQuery<Product>()
                         // Call 'SuggestUsing'
                        .SuggestUsing(builder => builder
                            .ByField(x => x.Name, "chaig")
                             // Customize suggestions options
                            .WithOptions(new SuggestionOptions
                            {
                                Accuracy = 0.4f,
                                PageSize = 5,
                                Distance = StringDistanceTypes.JaroWinkler,
                                SortMode = SuggestionSortMode.Popularity
                            })
                             // Customize display name for results
                            .WithDisplayName("SomeCustomName"))
                        .Execute();
                    #endregion
                    
                    #region suggest_21
                    // The resulting suggested terms:
                    // ==============================
                    
                    Console.WriteLine("Suggested terms:");
                    // Results are available under the custom name entry
                    foreach (string suggestedTerm in suggestions["SomeCustomName"].Suggestions)
                    {
                        Console.WriteLine("\t{0}", suggestedTerm);
                    }
                    
                    // Suggested terms:
                    //     chai
                    //     chang
                    //     chartreuse verte
                    #endregion
                }
            }
        }

        private interface IFoo<T>
        {
            #region syntax_1
            // Overloads for requesting suggestions for term(s) in a field: 
            ISuggestionQuery<T> SuggestUsing<T>(SuggestionBase suggestion);
            ISuggestionQuery<T> SuggestUsing<T>(Action<ISuggestionBuilder<T>> builder);

            // Overloads requesting suggestions for term(s) in another field in the same query:
            ISuggestionQuery<T> AndSuggestUsing(SuggestionBase suggestion);
            ISuggestionQuery<T> AndSuggestUsing(Action<ISuggestionBuilder<T>> builder);
            #endregion

            #region syntax_2
            ISuggestionOperations<T> ByField(string fieldName, string term);
            ISuggestionOperations<T> ByField(string fieldName, string[] terms);
            ISuggestionOperations<T> ByField(Expression<Func<T, object>> path, string term);
            ISuggestionOperations<T> ByField(Expression<Func<T, object>> path, string[] terms);

            ISuggestionOperations<T> WithOptions(SuggestionOptions options);
            ISuggestionOperations<T> WithDisplayName(string displayName);
            #endregion
        }
        
        private class Foo
        {
            #region syntax_3
            public int PageSize { get; set; }
            public StringDistanceTypes? Distance { get; set; }
            public float? Accuracy { get; set; }
            public SuggestionSortMode SortMode { get; set; }
            #endregion
        }
    }
}
