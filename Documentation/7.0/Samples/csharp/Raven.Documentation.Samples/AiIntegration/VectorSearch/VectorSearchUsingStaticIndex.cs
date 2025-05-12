using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Indexes.Vector;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Client.Documents.Queries.Vector;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.AiIntegration
{
    public class VectorSearchUsingStaticIndex
    {
        // The indexes:
        // =============
        
        #region index_1
        public class Products_ByVector_Text :
            AbstractIndexCreationTask<Product, Products_ByVector_Text.IndexEntry>
        {
            public class IndexEntry()
            {
                // This index-field will hold the embeddings that will be generated
                // from the TEXT in the documents
                public object VectorFromText { get; set; }
            }
    
            public Products_ByVector_Text()
            {
                Map = products => from product in products
                    select new IndexEntry
                    {
                        // Call 'CreateVector' to create a VECTOR FIELD.
                        // Pass the document field containing the text
                        // from which the embeddings will be generated.
                        VectorFromText = CreateVector(product.Name)
                    };

                // Customize the vector field options:
                VectorIndexes.Add(x => x.VectorFromText,
                    new VectorOptions()
                    {
                        // Define the source embedding type
                        SourceEmbeddingType = VectorEmbeddingType.Text, 
                        
                        // Define the quantization for the destination embedding
                        DestinationEmbeddingType = VectorEmbeddingType.Single,
                        
                        // Optionally, set the number of edges 
                        NumberOfEdges = 20,
                        
                        // Optionally, set the number of candidates
                        NumberOfCandidatesForIndexing = 20
                    });

                // The index MUST use the Corax search engine 
                SearchEngineType = Raven.Client.Documents.Indexes.SearchEngineType.Corax;
            }
        }
        #endregion
        
        #region index_2
        public class Products_ByVector_Text_JS : AbstractJavaScriptIndexCreationTask
        {
            public Products_ByVector_Text_JS()
            {
                Maps = new HashSet<string>()
                {
                    $@"map('Products', function (product) {{
                           return {{
                               VectorFromText: createVector(product.Name)
                           }};
                     }})"
                };
            
                Fields = new();
                Fields.Add("VectorFromText", new IndexFieldOptions()
                {
                    Vector = new VectorOptions()
                    {
                        SourceEmbeddingType = VectorEmbeddingType.Text, 
                        DestinationEmbeddingType = VectorEmbeddingType.Single,
                        NumberOfEdges = 20,
                        NumberOfCandidatesForIndexing = 20
                    }
                });
        
                SearchEngineType = Raven.Client.Documents.Indexes.SearchEngineType.Corax;
            }
        }
        #endregion
        
        #region index_4
        public class Movies_ByVector_Single :
            AbstractIndexCreationTask<Movie, Movies_ByVector_Single.IndexEntry>
        {
            public class IndexEntry()
            {
                // This index-field will hold the embeddings that will be generated
                // from the NUMERICAL content in the documents.
                public object VectorFromSingle { get; set; }
            }
    
            public Movies_ByVector_Single()
            {
                Map = movies => from movie in movies
                    select new IndexEntry
                    {
                        // Call 'CreateVector' to create a VECTOR FIELD.
                        // Pass the document field containing the array (32-bit floating-point values)
                        // from which the embeddings will be generated.
                        VectorFromSingle = CreateVector(movie.TagsEmbeddedAsSingle)
                    };

                // Customize the vector field options:
                VectorIndexes.Add(x => x.VectorFromSingle,
                    new VectorOptions()
                    {
                        // Define the source embedding type
                        SourceEmbeddingType = VectorEmbeddingType.Single,
                        
                        // Define the quantization for the destination embedding
                        DestinationEmbeddingType = VectorEmbeddingType.Single,
                        
                        // It is recommended to configure the number of dimensions
                        // which is the size of the arrays that will be indexed.
                        Dimensions = 2,
                        
                        // Optionally, set the number of edges and candidates
                        NumberOfEdges = 20,
                        NumberOfCandidatesForIndexing = 20
                    });

                // The index MUST use the Corax search engine 
                SearchEngineType = Raven.Client.Documents.Indexes.SearchEngineType.Corax;
            }
        }
        #endregion
        
        #region index_5
        public class Movies_ByVector_Single_JS : AbstractJavaScriptIndexCreationTask
        {
            public Movies_ByVector_Single_JS()
            {
                Maps = new HashSet<string>()
                {
                    $@"map('Movies', function (movie) {{
                           return {{
                               VectorFromSingle: createVector(movie.TagsEmbeddedAsSingle)
                           }};
                     }})"
                };
            
                Fields = new();
                Fields.Add("VectorFromSingle", new IndexFieldOptions()
                {
                    Vector = new VectorOptions()
                    {
                        SourceEmbeddingType = VectorEmbeddingType.Single, 
                        DestinationEmbeddingType = VectorEmbeddingType.Single,
                        Dimensions = 2,
                        NumberOfEdges = 20,
                        NumberOfCandidatesForIndexing = 20
                    }
                });
        
                SearchEngineType = Raven.Client.Documents.Indexes.SearchEngineType.Corax;
            }
        }
        #endregion
        
        #region index_7
        public class Movies_ByVector_Int8 :
            AbstractIndexCreationTask<Movie, Movies_ByVector_Int8.IndexEntry>
        {
            public class IndexEntry()
            {
                // This index-field will hold the embeddings that will be generated
                // from the NUMERICAL content in the documents.
                public object VectorFromInt8Arrays { get; set; }
            }
    
            public Movies_ByVector_Int8()
            {
                Map = movies => from movie in movies
                    select new IndexEntry
                    {
                        // Call 'CreateVector' to create a VECTOR FIELD.
                        // Pass the document field containing the arrays (8-bit integer values)
                        // from which the embeddings will be generated.
                        VectorFromInt8Arrays = CreateVector(movie.TagsEmbeddedAsInt8)
                    };

                // Customize the vector field options:
                VectorIndexes.Add(x => x.VectorFromInt8Arrays,
                    new VectorOptions()
                    {
                        // Define the source embedding type
                        SourceEmbeddingType = VectorEmbeddingType.Int8,
                        
                        // Define the quantization for the destination embedding
                        DestinationEmbeddingType = VectorEmbeddingType.Int8,
                        
                        // It is recommended to configure the number of dimensions
                        // which is the size of the arrays that will be indexed.
                        Dimensions = 2,
                        
                        // Optionally, set the number of edges and candidates
                        NumberOfEdges = 20,
                        NumberOfCandidatesForIndexing = 20
                    });

                // The index MUST use the Corax search engine 
                SearchEngineType = Raven.Client.Documents.Indexes.SearchEngineType.Corax;
            }
        }
        #endregion
        
        #region index_8
        public class Movies_ByVector_Int8_JS : AbstractJavaScriptIndexCreationTask
        {
            public Movies_ByVector_Int8_JS()
            {
                Maps = new HashSet<string>()
                {
                    $@"map('Movies', function (movie) {{
                           return {{
                               VectorFromInt8Arrays: createVector(movie.TagsEmbeddedAsInt8)
                           }};
                     }})"
                };
            
                Fields = new();
                Fields.Add("VectorFromInt8Arrays", new IndexFieldOptions()
                {
                    Vector = new VectorOptions()
                    {
                        SourceEmbeddingType = VectorEmbeddingType.Int8, 
                        DestinationEmbeddingType = VectorEmbeddingType.Int8,
                        Dimensions = 2,
                        NumberOfEdges = 20,
                        NumberOfCandidatesForIndexing = 20
                    }
                });
        
                SearchEngineType = Raven.Client.Documents.Indexes.SearchEngineType.Corax;
            }
        }
        #endregion
                
        #region index_10
        public class Products_ByMultipleFields :
            AbstractIndexCreationTask<Product, Products_ByMultipleFields.IndexEntry>
        {
            public class IndexEntry()
            {
                // An index-field for 'regular' data
                public decimal PricePerUnit { get; set; }
                
                // An index-field for 'full-text' search
                public string Name { get; set; }
                
                // An index-field for 'vector' search
                public object VectorFromText { get; set; }
            }
    
            public Products_ByMultipleFields()
            {
                Map = products => from product in products
                    select new IndexEntry
                    {
                        PricePerUnit = product.PricePerUnit,
                        Name = product.Name,
                        VectorFromText = CreateVector(product.Name)
                    };
                
                // Configure the index-field 'Name' for FTS:
                Index(x => x.Name, FieldIndexing.Search);
                
                // Note:
                // Default values will be used for the VECTOR FIELD if not customized here.
                
                // The index MUST use the Corax search engine 
                SearchEngineType = Raven.Client.Documents.Indexes.SearchEngineType.Corax;
            }
        }
        #endregion
        
        #region index_11
        public class Products_ByMultipleFields_JS : AbstractJavaScriptIndexCreationTask
        {
            public Products_ByMultipleFields_JS()
            {
                Maps = new HashSet<string>()
                {
                    $@"map('Products', function (product) {{
                           return {{
                               PricePerUnit: product.PricePerUnit,
                               Name: product.Name,
                               VectorFromText: createVector(product.Name)
                           }};
                     }})"
                };
            
                Fields = new();
                Fields.Add("Name", new IndexFieldOptions()
                {
                    Indexing = FieldIndexing.Search
                });
        
                SearchEngineType = Raven.Client.Documents.Indexes.SearchEngineType.Corax;
            }
        }
        #endregion
        
        #region index_13
        public class Categories_ByPreMadeTextEmbeddings :
            AbstractIndexCreationTask<Category, Categories_ByPreMadeTextEmbeddings.IndexEntry>
        {
            public class IndexEntry()
            {
                // This index-field will hold the text embeddings
                // that were pre-made by the Embeddings Generation Task
                public object VectorFromTextEmbeddings { get; set; }
            }
    
            public Categories_ByPreMadeTextEmbeddings()
            {
                Map = categories => from category in categories
                    select new IndexEntry
                    {
                        // Call 'LoadVector' to create a VECTOR FIELD. Pass:
                        // * The document field name to be indexed (as a string) 
                        // * The identifier of the task that generated the embeddings
                        //   for the 'Name' field
                        VectorFromTextEmbeddings = LoadVector("Name", "id-for-task-open-ai")
                    };

                VectorIndexes.Add(x => x.VectorFromTextEmbeddings,
                    new VectorOptions()
                    {
                       // Vector options can be customized
                       // in the same way as the above index example.
                    });
                
                // The index MUST use the Corax search engine 
                SearchEngineType = Raven.Client.Documents.Indexes.SearchEngineType.Corax;
            }
        }
        #endregion
        
        #region index_14
        public class Categories_ByPreMadeTextEmbeddings_JS : AbstractJavaScriptIndexCreationTask
        {
            public Categories_ByPreMadeTextEmbeddings_JS()
            {
                Maps = new HashSet<string>()
                {
                    $@"map('Categories', function (category) {{
                           return {{
                               VectorFromTextEmbeddings:
                                   loadVector('Name', 'id-for-task-open-ai')
                           }};
                     }})"
                };
            
                Fields = new();
                Fields.Add("VectorFromTextEmbeddings", new IndexFieldOptions()
                {
                    Vector = new VectorOptions()
                    {
                        // Vector options can be customized
                        // in the same way as the above index example.
                    }
                });
        
                SearchEngineType = Raven.Client.Documents.Indexes.SearchEngineType.Corax;
            }
        }
        #endregion

        public void IndexDefinitionExamples()
        {
            using (var store = new DocumentStore())
            {
                #region index_3
                var indexDefinition = new IndexDefinition
                {
                    Name = "Products/ByVector/Text",
                    
                    Maps = new HashSet<string>
                    {
                        @"
                          from product in docs.Products
                          select new 
                          {
                              VectorFromText  = CreateVector(product.Name)
                          }"
                    },
                    
                    Fields = new Dictionary<string, IndexFieldOptions>()
                    {
                        {
                            "VectorFromText",
                            new IndexFieldOptions()
                            {
                                Vector = new VectorOptions()
                                {
                                    SourceEmbeddingType = VectorEmbeddingType.Text,
                                    DestinationEmbeddingType = VectorEmbeddingType.Single,
                                    NumberOfEdges = 20,
                                    NumberOfCandidatesForIndexing = 20
                                }
                            }
                        }
                    },
                    
                    Configuration = new IndexConfiguration()
                    {
                        ["Indexing.Static.SearchEngineType"] = "Corax"
                    }
                };

                store.Maintenance.Send(new PutIndexesOperation(indexDefinition));
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region index_6
                var indexDefinition = new IndexDefinition 
                {
                    Name = "Movies/ByVector/Single",

                    Maps = new HashSet<string>
                    {
                        @"
                          from movie in docs.Movies
                          select new 
                          {
                              VectorFromSingle = CreateVector(movie.TagsEmbeddedAsSingle)
                          }"
                    },
                    
                    Fields = new Dictionary<string, IndexFieldOptions>()
                    {
                        {
                            "VectorFromSingle",
                            new IndexFieldOptions()
                            {
                                Vector = new VectorOptions()
                                {
                                    SourceEmbeddingType = VectorEmbeddingType.Single,
                                    DestinationEmbeddingType = VectorEmbeddingType.Single,
                                    Dimensions = 2,
                                    NumberOfEdges = 20,
                                    NumberOfCandidatesForIndexing = 20
                                }
                            }
                        }
                    },
                    
                    Configuration = new IndexConfiguration()
                    {
                        ["Indexing.Static.SearchEngineType"] = "Corax"
                    }
                };

                store.Maintenance.Send(new PutIndexesOperation(indexDefinition));
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region index_9
                var indexDefinition = new IndexDefinition 
                {
                    Name = "Movies/ByVector/Int8",

                    Maps = new HashSet<string>
                    {
                        @"
                          from movie in docs.Movies
                          select new 
                          {
                              VectorFromInt8Arrays = CreateVector(movie.TagsEmbeddedAsInt8)
                          }"
                    },
                    
                    Fields = new Dictionary<string, IndexFieldOptions>()
                    {
                        {
                            "VectorFromInt8Arrays",
                            new IndexFieldOptions()
                            {
                                Vector = new VectorOptions()
                                {
                                    SourceEmbeddingType = VectorEmbeddingType.Int8,
                                    DestinationEmbeddingType = VectorEmbeddingType.Int8,
                                    Dimensions = 2,
                                    NumberOfEdges = 20,
                                    NumberOfCandidatesForIndexing = 20
                                }
                            }
                        }
                    },
                    
                    Configuration = new IndexConfiguration()
                    {
                        ["Indexing.Static.SearchEngineType"] = "Corax"
                    }
                };

                store.Maintenance.Send(new PutIndexesOperation(indexDefinition));
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region index_12
                var indexDefinition = new IndexDefinition
                {
                    Name = "Products/ByMultipleFields",
                    Maps = new HashSet<string>
                    {
                        @"
                          from product in docs.Products
                          select new 
                          {
                              PricePerUnit = product.PricePerUnit,
                              Name = product.Name,
                              VectorFromText  = CreateVector(product.Name)
                          }"
                    },
                    
                    Fields = new Dictionary<string, IndexFieldOptions>()
                    {
                        {
                            "Name",
                            new IndexFieldOptions()
                            {
                                Indexing = FieldIndexing.Search
                            }
                        }
                    },
                    
                    Configuration = new IndexConfiguration()
                    {
                        ["Indexing.Static.SearchEngineType"] = "Corax"
                    }
                };

                store.Maintenance.Send(new PutIndexesOperation(indexDefinition));
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region index_15
                var indexDefinition = new IndexDefinition
                {
                    Name = "Categories/ByPreMadeTextEmbeddings",
                    Maps = new HashSet<string>
                    {
                        @"
                          from category in docs.Categories
                          select new 
                          {
                              VectorFromTextEmbeddings = LoadVector(""Name"", ""id-for-task-open-ai"")
                          }"
                    },
                    
                    Fields = new Dictionary<string, IndexFieldOptions>()
                    {
                        {
                            "VectorFromTextEmbeddings",
                            new IndexFieldOptions()
                            {
                                Vector = new VectorOptions()
                                {
                                    // Vector options can be customized
                                    // in the same way as the above index example.
                                }
                            }
                        }
                    },
                    
                    Configuration = new IndexConfiguration()
                    {
                        ["Indexing.Static.SearchEngineType"] = "Corax"
                    }
                };

                store.Maintenance.Send(new PutIndexesOperation(indexDefinition));
                #endregion
            }
        }
        
        public async Task QueryExamples()
        {
            using (var store = new DocumentStore())
            {
                // Query for TEXTUAL content
                // =========================
                
                using (var session = store.OpenSession())
                {
                    #region query_1
                    var similarProducts = session
                        .Query<Products_ByVector_Text.IndexEntry, Products_ByVector_Text>()
                         // Perform a vector search
                         // Call the 'VectorSearch' method
                        .VectorSearch(
                            field => field
                                 // Call 'WithField'
                                 // Specify the index-field in which to search for similar values
                                .WithField(x => x.VectorFromText),
                            searchTerm => searchTerm
                                 // Call 'ByText'   
                                 // Provide the term for the similarity comparison
                                .ByText("italian food"),
                            // Optionally, specify the minimum similarity value
                            minimumSimilarity: 0.82f,
                            // Optionally, specify the number candidates for querying
                            numberOfCandidates: 20,
                            // Optionally, specify whether the vector search should use the 'exact search method'
                            isExact: true)
                         // Waiting for not-stale results is not mandatory
                         // but will assure results are not stale
                        .Customize(x => x.WaitForNonStaleResults())
                        .OfType<Product>()
                        .ToList();
                        #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_1_async
                    var similarProducts = await asyncSession
                        .Query<Products_ByVector_Text.IndexEntry, Products_ByVector_Text>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromText),
                            searchTerm => searchTerm
                                .ByText("italian food"), 0.82f, 20, isExact: true)
                        .Customize(x => x.WaitForNonStaleResults())
                        .OfType<Product>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_2
                    var similarProducts = session.Advanced
                        .DocumentQuery<Products_ByVector_Text.IndexEntry, Products_ByVector_Text>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromText),
                            searchTerm => searchTerm
                                .ByText("italian food"), 0.82f, 20, isExact: true)
                        .WaitForNonStaleResults()
                        .OfType<Product>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_2_async
                    var similarProducts = await asyncSession.Advanced
                        .AsyncDocumentQuery<Products_ByVector_Text.IndexEntry, Products_ByVector_Text>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromText),
                            searchTerm => searchTerm
                                .ByText("italian food"),
                            0.82f, 20, isExact: true)
                        .WaitForNonStaleResults()
                        .OfType<Product>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_3
                    var similarProducts = session.Advanced
                        .RawQuery<Product>(@"
                            from index 'Products/ByVector/Text'
                            // Optionally, wrap the 'vector.search' query with 'exact()' to perform an exact search
                            where exact(vector.search(VectorFromText, $searchTerm, 0.82, 20))")
                        .AddParameter("searchTerm", "italian food")
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_3_async
                    var similarProducts = await asyncSession.Advanced
                        .AsyncRawQuery<Product>(@"
                            from index 'Products/ByVector/Text'
                            // Optionally, wrap the 'vector.search' query with 'exact()' to perform an exact search
                            where exact(vector.search(VectorFromText, $searchTerm, 0.82, 20))")
                        .AddParameter("searchTerm", "italian food")
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                // Query for NUMERICAL content
                // ===========================
                
                using (var session = store.OpenSession())
                {
                    #region query_4
                    var similarMovies = session
                        .Query<Movies_ByVector_Single.IndexEntry, Movies_ByVector_Single>()
                         // Perform a vector search
                         // Call the 'VectorSearch' method
                        .VectorSearch(
                            field => field
                                 // Call 'WithField'
                                 // Specify the index-field in which to search for similar values
                                .WithField(x => x.VectorFromSingle),
                            queryVector => queryVector
                                 // Call 'ByEmbedding'   
                                 // Provide the vector for the similarity comparison
                                .ByEmbedding(
                                     new RavenVector<float>(new float[] { 6.599999904632568f, 7.699999809265137f })))
                        .Customize(x => x.WaitForNonStaleResults())
                        .OfType<Movie>()
                        .ToList();
                        #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_4_async
                    var similarMovies = await asyncSession
                        .Query<Movies_ByVector_Single.IndexEntry, Movies_ByVector_Single>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromSingle),
                            queryVector => queryVector
                                .ByEmbedding(
                                    new RavenVector<float>(new float[] { 6.599999904632568f, 7.699999809265137f })))
                        .Customize(x => x.WaitForNonStaleResults())
                        .OfType<Movie>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_5
                    var similarMovies = session.Advanced
                        .DocumentQuery<Movies_ByVector_Single.IndexEntry, Movies_ByVector_Single>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromSingle),
                            queryVector => queryVector
                                .ByEmbedding(
                                    new RavenVector<float>(new float[] { 6.599999904632568f, 7.699999809265137f })))
                        .WaitForNonStaleResults()
                        .OfType<Movie>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_5_async
                    var similarMovies = await asyncSession.Advanced
                        .AsyncDocumentQuery<Movies_ByVector_Single.IndexEntry, Movies_ByVector_Single>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromSingle),
                            queryVector => queryVector
                                .ByEmbedding(
                                    new RavenVector<float>(new float[] { 6.599999904632568f, 7.699999809265137f })))
                        .WaitForNonStaleResults()
                        .OfType<Movie>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_6
                    var similarMovies = session.Advanced
                        .RawQuery<Movie>(@"
                            from index 'Movies/ByVector/Single'
                            where vector.search(VectorFromSingle, $queryVector)")
                        .AddParameter("queryVector", new RavenVector<float>(new float[]
                         {
                             6.599999904632568f, 7.699999809265137f
                         }))
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_6_async
                    var similarMovies = await asyncSession.Advanced
                        .AsyncRawQuery<Movie>(@"
                            from index 'Movies/ByVector/Single'
                            where vector.search(VectorFromSingle, $queryVector)")
                        .AddParameter("queryVector", new RavenVector<float>(new float[]
                         {
                             6.599999904632568f, 7.699999809265137f
                         }))
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_7
                    var similarMovies = session
                        .Query<Movies_ByVector_Int8.IndexEntry, Movies_ByVector_Int8>()
                         // Perform a vector search
                         // Call the 'VectorSearch' method
                        .VectorSearch(
                            field => field
                                 // Call 'WithField'
                                 // Specify the index-field in which to search for similar values
                                .WithField(x => x.VectorFromInt8Arrays),
                            queryVector => queryVector
                                 // Call 'ByEmbedding'   
                                 // Provide the vector for the similarity comparison
                                 // (Note: provide a single vector)
                                .ByEmbedding(
                                    // The provided vector MUST be in the same format as was stored in your document
                                    // Call 'VectorQuantizer.ToInt8' to transform the rawData to the Int8 format 
                                    VectorQuantizer.ToInt8(new float[] { 0.1f, 0.2f })))
                        .Customize(x => x.WaitForNonStaleResults())
                        .OfType<Movie>()
                        .ToList();
                        #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_7_async
                    var similarMovies = await asyncSession
                        .Query<Movies_ByVector_Int8.IndexEntry, Movies_ByVector_Int8>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromInt8Arrays),
                            queryVector => queryVector
                                .ByEmbedding(
                                    VectorQuantizer.ToInt8(new float[] { 0.1f, 0.2f })))
                        .Customize(x => x.WaitForNonStaleResults())
                        .OfType<Movie>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_8
                    var similarMovies = session.Advanced
                        .DocumentQuery<Movies_ByVector_Int8.IndexEntry, Movies_ByVector_Int8>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromInt8Arrays),
                            queryVector => queryVector
                                .ByEmbedding(
                                    VectorQuantizer.ToInt8(new float[] { 0.1f, 0.2f })))
                        .WaitForNonStaleResults()
                        .OfType<Movie>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_8_async
                    var similarMovies = await asyncSession.Advanced
                        .AsyncDocumentQuery<Movies_ByVector_Int8.IndexEntry, Movies_ByVector_Int8>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromInt8Arrays),
                            queryVector => queryVector
                                .ByEmbedding(
                                    VectorQuantizer.ToInt8(new float[] { 0.1f, 0.2f })))
                        .WaitForNonStaleResults()
                        .OfType<Movie>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_9
                    var similarMovies = session.Advanced
                        .RawQuery<Movie>(@"
                            from index 'Movies/ByVector/Int8'
                            where vector.search(VectorFromInt8Arrays, $queryVector)")
                        .AddParameter("queryVector", VectorQuantizer.ToInt8(new float[] { 0.1f, 0.2f }))
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_9_async
                    var similarMovies = await asyncSession.Advanced
                        .AsyncRawQuery<Movie>(@"
                            from index 'Movies/ByVector/Int8'
                            where vector.search(VectorFromInt8Arrays, $queryVector)")
                        .AddParameter("queryVector", VectorQuantizer.ToInt8(new float[] { 0.1f, 0.2f }))
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_10
                    var results = session.Advanced
                        .DocumentQuery<Products_ByMultipleFields.IndexEntry, Products_ByMultipleFields>()
                         // Perform a regular search
                        .WhereGreaterThan(x => x.PricePerUnit, 200)
                        .OrElse()
                         // Perform a full-text search
                        .Search(x => x.Name, "Alice")
                        .OrElse()
                         // Perform a vector search
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromText),
                            searchTerm => searchTerm
                                .ByText("italian food"),
                            minimumSimilarity: 0.8f)
                        .WaitForNonStaleResults()
                        .OfType<Product>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_10_async
                    var results = await asyncSession.Advanced
                        .AsyncDocumentQuery<Products_ByMultipleFields.IndexEntry, Products_ByMultipleFields>()
                        .WhereGreaterThan(x => x.PricePerUnit, 200)
                        .OrElse()
                        .Search(x => x.Name, "Alice")
                        .OrElse()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromText),
                            searchTerm => searchTerm
                                .ByText("italian food"),
                            minimumSimilarity: 0.8f)
                        .WaitForNonStaleResults()
                        .OfType<Product>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_11
                    var results = session.Advanced
                        .RawQuery<Product>(@"
                            from index 'Products/ByMultipleFields'
                            where PricePerUnit > $minPrice
                            or search(Name, $searchTerm1)
                            or vector.search(VectorFromText, $searchTerm2, 0.8)")
                        .AddParameter("minPrice", 200)
                        .AddParameter("searchTerm1", "Alice")
                        .AddParameter("searchTerm2", "italian")
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_11_async
                    var results = await asyncSession.Advanced
                        .AsyncRawQuery<Product>(@"
                            from index 'Products/ByMultipleFields'
                            where PricePerUnit > $minPrice
                            or search(Name, $searchTerm1)
                            or vector.search(VectorFromText, $searchTerm2, 0.8)")
                        .AddParameter("minPrice", 200)
                        .AddParameter("searchTerm1", "Alice")
                        .AddParameter("searchTerm2", "italian")
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                // Query for TEXTUAL content from PRE-MADE embeddings generated by tasks
                // =====================================================================
                
                using (var session = store.OpenSession())
                {
                    #region query_12
                    var similarCategories = session
                        .Query<Categories_ByPreMadeTextEmbeddings.IndexEntry, Categories_ByPreMadeTextEmbeddings>()
                        // Perform a vector search
                        // Call the 'VectorSearch' method
                        .VectorSearch(
                            field => field
                                // Call 'WithField'
                                // Specify the index-field in which to search for similar values
                                .WithField(x => x.VectorFromTextEmbeddings),
                            searchTerm => searchTerm
                                // Call 'ByText'
                                // Provide the search term for the similarity comparison
                                .ByText("candy"),
                            // Optionally, specify the minimum similarity value
                            minimumSimilarity: 0.75f,
                            // Optionally, specify the number of candidates for querying
                            numberOfCandidates: 20,
                            // Optionally, specify whether the vector search should use the 'exact search method'
                            isExact: true)
                        // Waiting for not-stale results is not mandatory
                        // but will assure results are not stale
                        .Customize(x => x.WaitForNonStaleResults())
                        .OfType<Category>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_12_async
                    var similarCategories = await asyncSession
                        .Query<Categories_ByPreMadeTextEmbeddings.IndexEntry, Categories_ByPreMadeTextEmbeddings>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromTextEmbeddings),
                            searchTerm => searchTerm
                                .ByText("candy"), 0.75f, 20, isExact: true)
                        .Customize(x => x.WaitForNonStaleResults())
                        .OfType<Category>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_13
                    var similarCategories = session.Advanced
                        .DocumentQuery<Categories_ByPreMadeTextEmbeddings.IndexEntry, Categories_ByPreMadeTextEmbeddings>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromTextEmbeddings),
                            searchTerm => searchTerm
                                .ByText("candy"), 0.75f, 20, isExact: true)
                        .WaitForNonStaleResults()
                        .OfType<Category>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_13_async
                    var similarCategories = await asyncSession.Advanced
                        .AsyncDocumentQuery<Categories_ByPreMadeTextEmbeddings.IndexEntry, Categories_ByPreMadeTextEmbeddings>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromTextEmbeddings),
                            searchTerm => searchTerm
                                .ByText("candy"),
                            0.75f, 20, isExact: true)
                        .WaitForNonStaleResults()
                        .OfType<Category>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_14
                    var similarCategories = session.Advanced
                        .RawQuery<Category>(@"
                            from index 'Categories/ByPreMadeTextEmbeddings'
                            // Optionally, wrap the 'vector.search' query with 'exact()' to perform an exact search
                            where exact(vector.search(VectorFromTextEmbeddings, $searchTerm, 0.75, 20))")
                        .AddParameter("searchTerm", "candy")
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_14_async
                    var similarCategories = await asyncSession.Advanced
                        .AsyncRawQuery<Category>(@"
                            from index 'Categories/ByPreMadeTextEmbeddings'
                            // Optionally, wrap the 'vector.search' query with 'exact()' to perform an exact search
                            where exact(vector.search(VectorFromTextEmbeddings, $searchTerm, 0.75, 20))")
                        .AddParameter("searchTerm", "candy")
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                // Query for similar documents
                // ===========================
                
                using (var session = store.OpenSession())
                {
                    #region query_15
                    var similarProducts = session
                        .Query<Products_ByVector_Text.IndexEntry, Products_ByVector_Text>()
                        // Perform a vector search
                        // Call the 'VectorSearch' method
                        .VectorSearch(
                            field => field
                                // Call 'WithField'
                                // Specify the index-field in which to search for similar values
                                .WithField(x => x.VectorFromText),
                            embedding => embedding
                                // Call 'ForDocument'
                                // Provide the document ID for which you want to find similar documents.
                                // The embedding stored in the index for the specified document
                                // will be used as the "query vector".
                                .ForDocument("Products/7-A"),
                            // Optionally, specify the minimum similarity value
                            minimumSimilarity: 0.82f)
                        .Customize(x => x.WaitForNonStaleResults())
                        .OfType<Product>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_15_async
                    var similarCategories = await asyncSession
                        .Query<Products_ByVector_Text.IndexEntry, Products_ByVector_Text>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromText),
                            embedding => embedding
                                .ForDocument("Products/7-A"),
                            minimumSimilarity: 0.82f)
                        .Customize(x => x.WaitForNonStaleResults())
                        .OfType<Category>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_16
                    var similarProducts = session.Advanced
                        .DocumentQuery<Products_ByVector_Text.IndexEntry, Products_ByVector_Text>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromText),
                            embedding => embedding
                                .ForDocument("Products/7-A"),
                            minimumSimilarity: 0.82f)
                        .WaitForNonStaleResults()
                        .OfType<Product>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_16_async
                    var similarProducts = await asyncSession.Advanced
                        .AsyncDocumentQuery<Products_ByVector_Text.IndexEntry, Products_ByVector_Text>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromText),
                            embedding => embedding
                                .ForDocument("Products/7-A"),
                            minimumSimilarity: 0.82f)
                        .WaitForNonStaleResults()
                        .OfType<Product>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_17
                    var similarProducts = session.Advanced
                        .RawQuery<Product>(@"
                            from index 'Products/ByVector/Text'
                            // Pass a document ID to the 'forDoc' method to find similar documents
                            where vector.search(VectorFromText, embedding.forDoc($documentID), 0.82)")
                        .AddParameter("$documentID", "Products/7-A")
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_17_async
                    var similarProducts = await asyncSession.Advanced
                        .AsyncRawQuery<Product>(@"
                            from index 'Products/ByVector/Text'
                            // Pass a document ID to the 'forDoc' method to find similar documents
                            where vector.search(VectorFromText, embedding.forDoc($documentID), 0.82)")
                        .AddParameter("$documentID", "Products/7-A")
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
            }
        }
    }
}
