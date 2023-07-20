using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries.Suggestions;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class Suggestions
    {
        #region suggestions_index_1
        public class Products_ByName : AbstractIndexCreationTask<Product, Products_ByName.IndexEntry>
        {
            // The IndexEntry class defines the index-fields
            public class IndexEntry
            {
                public string ProductName { get; set; }
            }
            
            public Products_ByName()
            {
                // The 'Map' function defines the content of the index-fields
                Map = products => from product in products
                    select new IndexEntry
                    {
                        ProductName = product.Name
                    };

                // Configure index-field 'ProductName' for suggestions
                Suggestion(x => x.ProductName);
                
                // Optionally: set 'Search' on this field
                // This will split the field content into multiple terms allowing for a full-text search
                Indexes.Add(x => x.ProductName, FieldIndexing.Search);
            }
        }
        #endregion
        
        #region suggestions_index_2
        public class Companies_ByNameAndByContactName :
            AbstractIndexCreationTask<Company, Companies_ByNameAndByContactName.IndexEntry>
        {
            // The IndexEntry class defines the index-fields.
            public class IndexEntry
            {
                public string CompanyName { get; set; }
                public string ContactName { get; set; }
            }
            
            public Companies_ByNameAndByContactName()
            {
                // The 'Map' function defines the content of the index-fields
                Map = companies => from company in companies
                    select new IndexEntry
                    {
                        CompanyName = company.Name,
                        ContactName = company.Contact.Name
                    };

                // Configure the index-fields for suggestions
                Suggestion(x => x.CompanyName);
                Suggestion(x => x.ContactName);
                
                // Optionally: set 'Search' on the index-fields
                // This will split the fields' content into multiple terms allowing for a full-text search
                Indexes.Add(x => x.CompanyName, FieldIndexing.Search);
                Indexes.Add(x => x.ContactName, FieldIndexing.Search);
            }
        }
        #endregion

        public async Task Samples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region suggestions_2

                    // This query on index 'Products/ByName' has NO resulting documents
                    List<Product> products = session
                        .Query<Products_ByName.IndexEntry, Products_ByName>()
                        .Search(x => x.ProductName, "chokolade")
                        .OfType<Product>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region suggestions_3
                    // Query the index for suggested terms for single term:
                    // ====================================================

                    Dictionary<string, SuggestionResult> suggestions = session
                         // Query the index   
                        .Query<Products_ByName.IndexEntry, Products_ByName>()
                         // Call 'SuggestUsing'
                        .SuggestUsing(builder => builder
                             // Request to get terms from index-field 'ProductName' that are similar to 'chokolade' 
                            .ByField(x => x.ProductName, "chokolade"))
                        .Execute();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region suggestions_4
                    // Query the index for suggested terms for single term:
                    // ====================================================

                    Dictionary<string, SuggestionResult> suggestions = await asyncSession
                         // Query the index   
                        .Query<Products_ByName.IndexEntry, Products_ByName>()
                         // Call 'SuggestUsing'
                        .SuggestUsing(builder => builder
                             // Request to get terms from index-field 'ProductName' that are similar to 'chokolade' 
                            .ByField(x => x.ProductName, "chokolade"))
                        .ExecuteAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region suggestions_5
                    // Define the suggestion request for single term
                    var suggestionRequest = new SuggestionWithTerm("ProductName")
                    {
                        // Looking for terms from index-field 'ProductName' that are similar to 'chokolade'  
                        Term = "chokolade"
                    };

                    // Query the index for suggestions 
                    Dictionary<string, SuggestionResult> suggestions = session
                        .Query<Products_ByName.IndexEntry, Products_ByName>()
                         // Call 'SuggestUsing' - pass the suggestion request
                        .SuggestUsing(suggestionRequest)
                        .Execute();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggestions_6
                    // Query the index for suggested terms for single term:
                    // ====================================================

                    Dictionary<string, SuggestionResult> suggestions = session.Advanced
                         // Query the index   
                        .DocumentQuery<Products_ByName.IndexEntry, Products_ByName>()
                         // Call 'SuggestUsing'
                        .SuggestUsing(builder => builder
                             // Request to get terms from index-field 'ProductName' that are similar to 'chokolade' 
                            .ByField(x => x.ProductName, "chokolade"))
                        .Execute();
                    #endregion
                    
                    #region suggestions_7
                    // The resulting suggested terms:
                    // ==============================

                    Console.WriteLine("Suggested terms in index-field 'ProductName' that are similar to 'chokolade':");
                    foreach (string suggestedTerm in suggestions["ProductName"].Suggestions)
                    {
                        Console.WriteLine("\t{0}", suggestedTerm);
                    }

                    // Suggested terms in index-field 'ProductName' that are similar to 'chokolade':
                    //     schokolade
                    //     chocolade
                    //     chocolate
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggestions_8
                    // Query the index for suggested terms for multiple terms:
                    // =======================================================

                    Dictionary<string, SuggestionResult> suggestions = session
                         // Query the index
                        .Query<Products_ByName.IndexEntry, Products_ByName>()
                         // Call 'SuggestUsing'
                        .SuggestUsing(builder => builder
                             // Request to get terms from index-field 'ProductName' that are similar to 'chokolade' OR 'syrop' 
                            .ByField(x => x.ProductName, new[] { "chokolade", "syrop" }))
                        .Execute();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region suggestions_9
                    // Query the index for suggested terms for multiple terms:
                    // =======================================================

                    Dictionary<string, SuggestionResult> suggestions = await asyncSession
                         // Query the index   
                        .Query<Products_ByName.IndexEntry, Products_ByName>()
                         // Call 'SuggestUsing'
                        .SuggestUsing(builder => builder
                             // Request to get terms from index-field 'ProductName' that are similar to 'chokolade' OR 'syrop' 
                            .ByField(x => x.ProductName, new[] { "chokolade", "syrop" }))
                        .ExecuteAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region suggestions_10
                    // Define the suggestion request for multiple terms
                    var suggestionRequest = new SuggestionWithTerms("ProductName")
                    {
                        // Looking for terms from index-field 'ProductName' that are similar to 'chokolade' OR 'syrop'  
                        Terms = new[] { "chokolade", "syrop"}
                    };

                    // Query the index for suggestions 
                    Dictionary<string, SuggestionResult> suggestions = session
                        .Query<Products_ByName.IndexEntry, Products_ByName>()
                         // Call 'SuggestUsing' - pass the suggestion request
                        .SuggestUsing(suggestionRequest)
                        .Execute();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggestions_11
                    // Query the index for suggested terms for multiple terms:
                    // =======================================================

                    Dictionary<string, SuggestionResult> suggestions = session.Advanced
                         // Query the index   
                        .DocumentQuery<Products_ByName.IndexEntry, Products_ByName>()
                         // Call 'SuggestUsing'
                        .SuggestUsing(builder => builder
                             // Request to get terms from index-field 'ProductName' that are similar to 'chokolade' OR 'syrop'  
                            .ByField(x => x.ProductName, new[] { "chokolade", "syrop" }))
                        .Execute();
                    #endregion
                    
                    #region suggestions_12
                    // The resulting suggested terms:
                    // ==============================

                    // Suggested terms in index-field 'ProductName' that are similar to 'chokolade' OR to 'syrop':
                    //     schokolade
                    //     chocolade
                    //     chocolate
                    //     sirop
                    //     syrup
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggestions_13
                    // Query the index for suggested terms in multiple fields:
                    // =======================================================

                    Dictionary<string, SuggestionResult> suggestions = session
                         // Query the index
                        .Query<Companies_ByNameAndByContactName.IndexEntry, Companies_ByNameAndByContactName>()
                         // Call 'SuggestUsing' to get suggestions for terms that are 
                         // similar to 'chese' in first index-field (e.g. 'CompanyName') 
                        .SuggestUsing(builder => builder
                            .ByField(x => x.CompanyName, "chese" ))
                         // Call 'AndSuggestUsing' to get suggestions for terms that are 
                         // similar to 'frank' in an additional index-field (e.g. 'ContactName')
                        .AndSuggestUsing(builder => builder
                            .ByField(x => x.ContactName, "frank"))
                        .Execute();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region suggestions_14
                    // Query the index for suggested terms in multiple fields:
                    // =======================================================

                    Dictionary<string, SuggestionResult> suggestions = await asyncSession
                         // Query the index
                        .Query<Companies_ByNameAndByContactName.IndexEntry, Companies_ByNameAndByContactName>()
                         // Call 'SuggestUsing' to get suggestions for terms that are 
                         // similar to 'chese' in first index-field (e.g. 'CompanyName') 
                        .SuggestUsing(builder => builder
                            .ByField(x => x.CompanyName, "chese" ))
                         // Call 'AndSuggestUsing' to get suggestions for terms that are 
                         // similar to 'frank' in an additional index-field (e.g. 'ContactName')
                        .AndSuggestUsing(builder => builder
                            .ByField(x => x.ContactName, "frank"))
                        .ExecuteAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggestions_15
                    // Define suggestion requests for multiple fields:
                    
                    var request1 = new SuggestionWithTerm("CompanyName")
                    {
                        // Looking for terms from index-field 'CompanyName' that are similar to 'chese'  
                        Term = "chese"
                    };

                    var request2 = new SuggestionWithTerm("ContactName")
                    {
                        // Looking for terms from nested index-field 'ContactName' that are similar to 'frank'  
                        Term = "frank"
                    };

                    // Query the index for suggestions 
                    Dictionary<string, SuggestionResult> suggestions = session
                        .Query<Companies_ByNameAndByContactName.IndexEntry, Companies_ByNameAndByContactName>()
                         // Call 'SuggestUsing' - pass the suggestion request for the first index-field
                        .SuggestUsing(request1)
                         // Call 'AndSuggestUsing' - pass the suggestion request for the second index-field
                        .AndSuggestUsing(request2)
                        .Execute();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggestions_16
                    // Query the index for suggested terms in multiple fields:
                    // =======================================================

                    Dictionary<string, SuggestionResult> suggestions = session.Advanced
                         // Query the index
                        .DocumentQuery<Companies_ByNameAndByContactName.IndexEntry, Companies_ByNameAndByContactName>()
                         // Call 'SuggestUsing' to get suggestions for terms that are 
                         // similar to 'chese' in first index-field (e.g. 'CompanyName') 
                        .SuggestUsing(builder => builder
                            .ByField(x => x.CompanyName, "chese" ))
                         // Call 'AndSuggestUsing' to get suggestions for terms that are 
                         // similar to 'frank' in an additional index-field (e.g. 'ContactName')
                        .AndSuggestUsing(builder => builder
                            .ByField(x => x.ContactName, "frank"))
                        .Execute();
                    #endregion
                    
                    #region suggestions_17
                    // The resulting suggested terms:
                    // ==============================

                    // Suggested terms in index-field 'CompanyName' that is similar to 'chese':
                    //     cheese
                    //     chinese

                    // Suggested terms in index-field 'ContactName' that are similar to 'frank':
                    //     fran
                    //     franken
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggestions_18
                    // Query the index for suggested terms - customize options and display name:
                    // =========================================================================

                    Dictionary<string, SuggestionResult> suggestions = session
                         // Query the index   
                        .Query<Products_ByName.IndexEntry, Products_ByName>()
                         // Call 'SuggestUsing'
                        .SuggestUsing(builder => builder
                            .ByField(x => x.ProductName, "chokolade")
                         // Customize suggestions options
                        .WithOptions(new SuggestionOptions
                        {
                            Accuracy = 0.3f,
                            PageSize = 5,
                            Distance = StringDistanceTypes.NGram,
                            SortMode = SuggestionSortMode.Popularity
                        })
                         // Customize display name for results
                        .WithDisplayName("SomeCustomName"))
                        .Execute();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region suggestions_19
                    // Query the index for suggested terms - customize options and display name:
                    // =========================================================================

                    Dictionary<string, SuggestionResult> suggestions = await asyncSession
                         // Query the index   
                        .Query<Products_ByName.IndexEntry, Products_ByName>()
                         // Call 'SuggestUsing'
                        .SuggestUsing(builder => builder
                            .ByField(x => x.ProductName, "chokolade")
                         // Customize suggestions options
                        .WithOptions(new SuggestionOptions
                        {
                            Accuracy = 0.3f,
                            PageSize = 5,
                            Distance = StringDistanceTypes.NGram,
                            SortMode = SuggestionSortMode.Popularity
                        })
                         // Customize display name for results
                        .WithDisplayName("SomeCustomName"))
                        .ExecuteAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggestions_20
                    // Define the suggestion request
                    var suggestionRequest = new SuggestionWithTerm("ProductName")
                    {
                        // Looking for terms from index-field 'ProductName' that are similar to 'chokolade'  
                        Term = "chokolade",
                        // Customize options
                        Options = new SuggestionOptions 
                        {
                            Accuracy = 0.3f,
                            PageSize = 5,
                            Distance = StringDistanceTypes.NGram,
                            SortMode = SuggestionSortMode.Popularity
                        },
                        // Customize display name
                        DisplayField = "SomeCustomName"
                    };

                    // Query the index for suggestions 
                    Dictionary<string, SuggestionResult> suggestions = session
                        .Query<Products_ByName.IndexEntry, Products_ByName>()
                        // Call 'SuggestUsing' - pass the suggestion request
                        .SuggestUsing(suggestionRequest)
                        .Execute();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region suggestions_21
                    // Query the index for suggested terms - customize options and display name:
                    // =========================================================================

                    Dictionary<string, SuggestionResult> suggestions = session.Advanced
                         // Query the index   
                        .DocumentQuery<Products_ByName.IndexEntry, Products_ByName>()
                         // Call 'SuggestUsing'
                        .SuggestUsing(builder => builder
                            .ByField(x => x.ProductName, "chokolade")
                             // Customize suggestions options
                            .WithOptions(new SuggestionOptions
                            {
                                Accuracy = 0.3f,
                                PageSize = 5,
                                Distance = StringDistanceTypes.NGram,
                                SortMode = SuggestionSortMode.Popularity
                            })
                             // Customize display name for results
                            .WithDisplayName("SomeCustomName"))
                        .Execute();
                    #endregion
                    
                    #region suggestions_22
                    // The resulting suggested terms:
                    // ==============================

                    Console.WriteLine("Suggested terms:");
                    // Results are available under the custom name entry
                    foreach (string suggestedTerm in suggestions["SomeCustomName"].Suggestions)
                    {
                        Console.WriteLine("\t{0}", suggestedTerm);
                    }

                    // Suggested terms:
                    //     chocolade
                    //     schokolade
                    //     chocolate
                    //     chowder
                    //     marmalade
                    #endregion
                }
            }
        }
    }
}
