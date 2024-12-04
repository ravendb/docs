using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries.Explanation;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class IncludingExplanations
    {
        #region index_1
        public class Products_BySearchName : AbstractIndexCreationTask<Product>
        {
            public class IndexEntry
            {
                public string Name { get; set; }
            }

            public Products_BySearchName()
            {
                Map = products => from product in products
                    select new IndexEntry()
                    {
                        Name = product.Name
                    };

                // Configure the index-field 'Name' for FTS
                Indexes.Add(x => x.Name, FieldIndexing.Search);
            }
        }
        #endregion
        
        #region index_2
        // This index counts the number of units ordered per category in all Product documents
        // ===================================================================================
        
        public class NumberOfUnitsOrdered_PerCategory : 
            AbstractIndexCreationTask<Product, NumberOfUnitsOrdered_PerCategory.IndexEntry>
        {
            public class IndexEntry
            {
                public string Category { get; set; }
                public int NumberOfUnitsOrdered { get; set; }
            }
            
            public NumberOfUnitsOrdered_PerCategory()
            {
                Map = products => from product in products
                    // Load the products' category 
                    let categoryName = LoadDocument<Category>(product.Category).Name
                    
                    select new IndexEntry()
                    {
                        Category = categoryName,
                        NumberOfUnitsOrdered = product.UnitsOnOrder
                    };

                Reduce = results => from result in results
                    group result by result.Category
                    into g
                    let unitsOrdered = g.Sum(x => x.NumberOfUnitsOrdered)
                    
                    select new IndexEntry()
                    {
                        Category = g.Key,
                        NumberOfUnitsOrdered = unitsOrdered
                    };
            }
        }
        #endregion

        public async Task GetExplanations()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region inc_1
                    List<Product> products = session
                         // Query the index
                        .Query<Products_BySearchName.IndexEntry, Products_BySearchName>()
                         // Convert the IRavenQueryable to IDocumentQuery
                         // to be able to use 'IncludeExplanations'
                        .ToDocumentQuery()
                         // Call 'IncludeExplanations', provide an out param for the explanations results
                        .IncludeExplanations(out Explanations explanations)
                         // Convert back to IRavenQueryable
                         // to continue building the query using LINQ
                        .ToQueryable()
                         // Define query criteria
                         // e.g. search for docs containing Syrup -or- Lager in their Name field
                        .Search(x => x.Name, "Syrup Lager" )
                        .OfType<Product>() 
                        .ToList();
                    
                    // When running the above query on the RavenDB sample data
                    // the results contain 3 product documents.
                    
                    // To get the score details for the first document from the results
                    // call 'GetExplanations' on the resulting Explanations object as follows:
                    string[] scoreDetails = explanations.GetExplanations(products[0].Id);
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region inc_1_async
                    List<Product> products = await asyncSession
                         // Query the index
                        .Query<Products_BySearchName.IndexEntry, Products_BySearchName>()
                         // Convert the IRavenQueryable to IDocumentQuery
                         // to be able to use 'IncludeExplanations'
                         .ToAsyncDocumentQuery()
                         // Call 'IncludeExplanations', provide an out param for the explanations results
                        .IncludeExplanations(out Explanations explanations)
                         // Convert back to IRavenQueryable
                         // to continue building the query using LINQ
                        .ToQueryable()
                         // Define query criteria
                         // e.g. search for docs containing Syrup -or- Lager in their Name field
                        .Search(x => x.Name, "Syrup Lager" )
                        .OfType<Product>() 
                        .ToListAsync();
                    
                    // When running the above query on the RavenDB sample data
                    // the results contain 3 product documents.
                    
                    // To get the score details for the first document from the results
                    // call 'GetExplanations' on the resulting Explanations object as follows:
                    string[] scoreDetails = explanations.GetExplanations(products[0].Id);
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region inc_2
                    List<Product> products = session.Advanced
                         // Query the index
                        .DocumentQuery<Products_BySearchName.IndexEntry, Products_BySearchName>()
                         // Call 'IncludeExplanations', provide an out param for the explanations results
                        .IncludeExplanations(out Explanations explanations)
                         // Define query criteria
                         // e.g. search for docs containing Syrup -or- Lager in their Name field
                        .Search(x => x.Name, "Syrup Lager" )
                        .OfType<Product>() 
                        .ToList();
                    
                    // When running the above query on the RavenDB sample data
                    // the results contain 3 product documents.
                    
                    // To get the score details for the first document from the results
                    // call 'GetExplanations' on the resulting Explanations object as follows:
                    string[] scoreDetails = explanations.GetExplanations(products[0].Id);
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region inc_2_async
                    List<Product> products = await asyncSession.Advanced
                         // Query the index
                        .AsyncDocumentQuery<Products_BySearchName.IndexEntry, Products_BySearchName>()
                         // Call 'IncludeExplanations', provide an out param for the explanations results
                        .IncludeExplanations(out Explanations explanations)
                         // Define query criteria
                         // e.g. search for docs containing Syrup -or- Lager in their Name field
                        .Search(x => x.Name, "Syrup Lager" )
                        .OfType<Product>() 
                        .ToListAsync();
                    
                    // When running the above query on the RavenDB sample data
                    // the results contain 3 product documents.
                    
                    // To get the score details for the first document from the results
                    // call 'GetExplanations' on the resulting Explanations object as follows:
                    string[] scoreDetails = explanations.GetExplanations(products[0].Id);
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region inc_3
                    List<NumberOfUnitsOrdered_PerCategory.IndexEntry> results = session
                         // Query the Map-Reduce index
                        .Query<NumberOfUnitsOrdered_PerCategory.IndexEntry, 
                            NumberOfUnitsOrdered_PerCategory>()
                         // Convert the IRavenQueryable to IDocumentQuery
                         // to be able to use 'IncludeExplanations'
                        .ToDocumentQuery()
                         // Call 'IncludeExplanations', provide:
                         // * The group key for each result item
                         // * An out param for the explanations results
                        .IncludeExplanations(
                            new ExplanationOptions { GroupKey = "Category" }, 
                            out Explanations explanations)
                         // Convert back to IRavenQueryable
                         // to continue building the query using LINQ
                        .ToQueryable()
                         // Query for categories that have a total of more than a 400 units ordered
                        .Where(x => x.NumberOfUnitsOrdered > 400)
                        .ToList();
                    
                    // Get the score details for an item in the results
                    // Pass the group key (Category, in this case) to 'GetExplanations'
                    string[] scoreDetails = explanations.GetExplanations(results[0].Category);
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region inc_3_async
                    List<NumberOfUnitsOrdered_PerCategory.IndexEntry> results = await asyncSession
                         // Query the Map-Reduce index
                        .Query<NumberOfUnitsOrdered_PerCategory.IndexEntry, 
                            NumberOfUnitsOrdered_PerCategory>()
                         // Convert the IRavenQueryable to IDocumentQuery
                         // to be able to use 'IncludeExplanations'
                        .ToAsyncDocumentQuery()
                         // Call 'IncludeExplanations', provide:
                         // * The group key for each result item
                         // * An out param for the explanations results
                        .IncludeExplanations(
                            new ExplanationOptions { GroupKey = "Category" }, 
                            out Explanations explanations)
                         // Convert back to IRavenQueryable
                         // to continue building the query using LINQ
                        .ToQueryable()
                         // Query for categories that have a total of more than a 400 units ordered
                        .Where(x => x.NumberOfUnitsOrdered > 400)
                        .ToListAsync();
                    
                    // Get the score details for an item in the results
                    // Pass the group key (Category, in this case) to 'GetExplanations'
                    string[] scoreDetails = explanations.GetExplanations(results[0].Category);
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region inc_4
                    List<NumberOfUnitsOrdered_PerCategory.IndexEntry> results = session.Advanced
                         // Query the Map-Reduce index
                        .DocumentQuery<NumberOfUnitsOrdered_PerCategory.IndexEntry, 
                            NumberOfUnitsOrdered_PerCategory>()
                         // Call 'IncludeExplanations', provide:
                         // * The group key for each result item
                         // * An out param for the explanations results
                        .IncludeExplanations(
                            new ExplanationOptions { GroupKey = "Category" }, 
                            out Explanations explanations)
                         // Query for categories that have a total of more than a 400 units ordered
                        .WhereGreaterThan(x => x.NumberOfUnitsOrdered, 400)
                        .ToList();
                    
                    // Get the score details for an item in the results
                    // Pass the group key (Category, in this case) to 'GetExplanations'
                    string[] scoreDetails = explanations.GetExplanations(results[0].Category);
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region inc_4_async
                    List<NumberOfUnitsOrdered_PerCategory.IndexEntry> results = await asyncSession.Advanced
                         // Query the Map-Reduce index
                        .AsyncDocumentQuery<NumberOfUnitsOrdered_PerCategory.IndexEntry, 
                            NumberOfUnitsOrdered_PerCategory>()
                         // Call 'IncludeExplanations', provide:
                         // * The group key for each result item
                         // * An out param for the explanations results
                        .IncludeExplanations(
                            new ExplanationOptions { GroupKey = "Category" }, 
                            out Explanations explanations)
                         // Query for categories that have a total of more than a 400 units ordered
                        .WhereGreaterThan(x => x.NumberOfUnitsOrdered, 400)
                        .ToListAsync();
                    
                    // Get the score details for an item in the results
                    // Pass the group key (Category, in this case) to 'GetExplanations'
                    string[] scoreDetails = explanations.GetExplanations(results[0].Category);
                    #endregion
                }
            }
        }
        
        public interface IFoo<T>
        {
            #region syntax_1
            // Use this overload when querying a Map index
            IDocumentQuery<T> IncludeExplanations(out Explanations explanations);
            
            // Use this overload when querying a Map-Reduce index
            IDocumentQuery<T> IncludeExplanations(ExplanationOptions options, out Explanations explanations);
            #endregion

            /*
            #region syntax_2
            public class Explanations
            {
                // Returns a list with all explanations.
                // Pass the document ID of a document from the results to get its score details (Map index)
                // Pass the GroupKey of an item from the results to get its score details (Map-Reduce index)
                public string[] GetExplanations(string key);
            }
            #endregion
            */
            
            #region syntax_3
            public sealed class ExplanationOptions
            {
                // The GroupKey that was used to generate items (index-entries) in a Map-Reduce index 
                public string GroupKey { get; set; }
            }
            #endregion
        }
    }
}
