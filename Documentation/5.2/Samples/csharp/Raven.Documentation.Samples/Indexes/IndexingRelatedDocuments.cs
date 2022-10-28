using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Indexes.IndexingRelatedDocuments;
using Raven.Documentation.Samples.Orders;
using Raven.Client.Documents.Linq;

namespace Raven.Documentation.Samples.Indexes
{
    namespace Syntax
    {
        public class LoadDocumentSyntax
        {
            private interface ILoadDocument
            {
                #region syntax
                T LoadDocument<T>(string relatedDocumentId);
                
                T LoadDocument<T>(string relatedDocumentId, string relatedCollectionName);

                T[] LoadDocument<T>(IEnumerable<string> relatedDocumentIds);

                T[] LoadDocument<T>(IEnumerable<string> relatedDocumentIds, string relatedCollectionName);
                #endregion
            }
        }
    }
    
    namespace IndexingRelatedDocuments
    {
        #region indexing_related_documents_1
        public class Products_ByCategoryName : AbstractIndexCreationTask<Product>
        {
            public class IndexEntry
            {
                public string CategoryName { get; set; }
            }

            public Products_ByCategoryName()
            {
                Map = products => from product in products
                    
                    // Call LoadDocument to load the related Category document
                    // The document ID to load is specified by 'product.Category'
                    let category = LoadDocument<Category>(product.Category)
                    
                    select new IndexEntry
                    {
                        // Index the Name field from the related Category document
                        CategoryName = category.Name
                    };
                        
                    // Since NoTracking was Not specified,
                    // then any change to either Products or Categories will trigger reindexing 
            }
        }
        #endregion
        
        #region indexing_related_documents_1_JS
        public class Products_ByCategoryName_JS : AbstractJavaScriptIndexCreationTask
        {
            public Products_ByCategoryName_JS()
            {
                Maps = new HashSet<string>()
                {
                    // Call method 'load' to load the related Category document
                    // The document ID to load is specified by 'product.Category'
                    // The Name field from the related Category document will be indexed
                    
                    @"map('products', function(product) {
                        let category = load(product.Category, 'Categories')
                        return {
                            CategoryName: category.Name
                        };
                    })"
                    
                    // Since noTracking was Not specified,
                    // then any change to either Products or Categories will trigger reindexing 
                };
            }
        }
        #endregion
        
        #region indexing_related_documents_3
        // The referencing document
        public class Author
        {
            public string Id { get; set; }
            public string Name { get; set; }
            
            // Referencing a list of related document IDs
            public List<string> BookIds { get; set; }
        }
        
        // The related document
        public class Book
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        #endregion

        #region indexing_related_documents_4
        public class Authors_ByBooks : AbstractIndexCreationTask<Author>
        {
            public class IndexEntry
            {
                public IEnumerable<string> BookNames { get; set; }
            }

            public Authors_ByBooks()
            {
                Map = authors => from author in authors
                    select new IndexEntry
                    {
                        // For each Book ID, call LoadDocument and index the book's name
                        BookNames = author.BookIds.Select(x => LoadDocument<Book>(x).Name)
                    };
                
                // Since NoTracking was Not specified,
                // then any change to either Authors or Books will trigger reindexing 
            }
        }
        #endregion
        
        #region indexing_related_documents_4_JS
        public class Authors_ByBooks_JS : AbstractJavaScriptIndexCreationTask
        {
            public Authors_ByBooks_JS()
            {
                Maps = new HashSet<string>()
                {
                    // For each Book ID, call 'load' and index the book's name
                    @"map('Author', function(author) {
                        return {
                            Books: author.BooksIds.map(x => load(x, 'Books').Name)
                        }
                    })"
                    
                    // Since NoTracking was Not specified,
                    // then any change to either Authors or Books will trigger reindexing 
                };
            }
        }
        #endregion
        
        #region indexing_related_documents_6
        public class Products_ByCategoryName_NoTracking : AbstractIndexCreationTask<Product>
        {
            public class IndexEntry
            {
                public string CategoryName { get; set; }
            }

            public Products_ByCategoryName_NoTracking()
            {
                Map = products => from product in products
                    
                    // Call NoTracking.LoadDocument to load the related Category document w/o tracking
                    let category = NoTracking.LoadDocument<Category>(product.Category)
                    
                    select new IndexEntry
                    {
                        // Index the Name field from the related Category document
                        CategoryName = category.Name
                    };
                        
                    // Since NoTracking is used -
                    // then only the changes to Products will trigger reindexing 
            }
        }
        #endregion
        
        #region indexing_related_documents_6_JS
        public class Products_ByCategoryName_NoTracking_JS : AbstractJavaScriptIndexCreationTask
        {
            public Products_ByCategoryName_NoTracking_JS()
            {
                Maps = new HashSet<string>()
                {
                    // Call 'noTracking.load' to load the related Category document w/o tracking
                    
                    @"map('products', function(product) {
                        let category = noTracking.load(product.Category, 'Categories')
                        return {
                            CategoryName: category.Name
                        };
                    })"
                    
                    // Since noTracking is used -
                    // then only the changes to Products will trigger reindexing
                };
            }
        }
        #endregion
    }

    public class IndexingRelatedDocumentsQueries
    {
        public async void Queries()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region indexing_related_documents_2
                    IList<Product> matchingProducts = session
                        .Query<Products_ByCategoryName.IndexEntry, Products_ByCategoryName>()
                        .Where(x => x.CategoryName == "Beverages")
                        .OfType<Product>()
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region indexing_related_documents_2_async
                    IList<Product> matchingProducts = await asyncSession
                        .Query<Products_ByCategoryName.IndexEntry, Products_ByCategoryName>()
                        .Where(x => x.CategoryName == "Beverages")
                        .OfType<Product>()
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexing_related_documents_5
                    // Get all authors that have books with title: "The Witcher"
                    IList<Author> matchingAuthors = session
                        .Query<Authors_ByBooks.IndexEntry, Authors_ByBooks>()
                        .Where(x => x.BookNames.Contains("The Witcher"))
                        .OfType<Author>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region indexing_related_documents_5_async
                    // Get all authors that have books with title: "The Witcher"
                    IList<Author> matchingAuthors = await asyncSession
                        .Query<Authors_ByBooks.IndexEntry, Authors_ByBooks>()
                        .Where(x => x.BookNames.Contains("The Witcher"))
                        .OfType<Author>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region indexing_related_documents_7
                    IList<Product> matchingProducts = session
                        .Query<Products_ByCategoryName_NoTracking.IndexEntry, Products_ByCategoryName_NoTracking>()
                        .Where(x => x.CategoryName == "Beverages")
                        .OfType<Product>()
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region indexing_related_documents_7_async
                    IList<Product> matchingProducts = await asyncSession
                        .Query<Products_ByCategoryName_NoTracking.IndexEntry, Products_ByCategoryName_NoTracking>()
                        .Where(x => x.CategoryName == "Beverages")
                        .OfType<Product>()
                        .ToListAsync();
                    #endregion
                }
            }
        }
    }
}
