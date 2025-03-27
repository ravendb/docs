using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes.Vector;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries.Vector;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.AiIntegration
{
    public class VectorSearchUsingDynamicQuery
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                // Examples for TEXTUAL content
                // ============================
                
                using (var session = store.OpenSession())
                {
                    #region vs_1
                    var similarProducts = session.Query<Product>()
                         // Perform a vector search
                         // Call the 'VectorSearch' method
                        .VectorSearch(
                            // Call 'WithText'
                            // Specify the document field in which to search for similar values
                            field => field.WithText(x => x.Name),
                            // Call 'ByText' 
                            // Provide the term for the similarity comparison
                            searchTerm => searchTerm.ByText("italian food"),
                            // It is recommended to specify the minimum similarity level
                            0.82f,
                            // Optionally, specify the number of candidates for the search
                            20)
                         // Waiting for not-stale results is not mandatory
                         // but will assure results are not stale
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_1_async
                    var similarProducts = await asyncSession.Query<Product>()
                        .VectorSearch(
                            field => field.WithText(x => x.Name), 
                            searchTerm => searchTerm.ByText("italian food"),
                            0.82f,
                            20)
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region vs_2
                    var similarProducts = session.Advanced
                        .DocumentQuery<Product>()
                        .VectorSearch(
                            field => field.WithText(x => x.Name),
                            searchTerm => searchTerm.ByText("italian food"),
                            0.82f,
                            20)
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_2_async
                    var similarProducts = await asyncSession.Advanced
                        .AsyncDocumentQuery<Product>()
                        .VectorSearch(
                            field => field.WithText(x => x.Name),
                            searchTerm => searchTerm.ByText("italian food"),
                            0.82f,
                            20)
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region vs_3
                    var similarProducts = session.Advanced
                        .RawQuery<Product>(@"
                            from 'Products'
                            // Wrap the document field 'Name' with 'embedding.text' to indicate the source data type
                            where vector.search(embedding.text(Name), $searchTerm, 0.82, 20)")
                        .AddParameter("searchTerm", "italian food")
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_3_async
                    var similarProducts = await asyncSession.Advanced
                        .AsyncRawQuery<Product>(@"
                            from 'Products'
                            // Wrap the document field 'Name' with 'embedding.text' to indicate the source data type
                            where vector.search(embedding.text(Name), $searchTerm, 0.82, 20)")
                        .AddParameter("searchTerm", "italian food")
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                // Examples for NUMERICAL content
                // ==============================
                
                #region sample_data
                using (var session = store.OpenSession())
                {
                    var movie1 = new Movie()
                    {
                        Title = "Hidden Figures",
                        Id = "movies/1",
                    
                        // Embedded vector represented as float values
                        TagsEmbeddedAsSingle = new RavenVector<float>(new float[]
                        {
                            6.599999904632568f, 7.699999809265137f
                        }),
                        
                        // Embedded vectors encoded in Base64 format
                        TagsEmbeddedAsBase64 = new List<string>()
                        {
                            "zczMPc3MTD6amZk+", "mpmZPs3MzD4AAAA/"
                        },
                        
                        // Array of embedded vectors quantized to Int8
                        TagsEmbeddedAsInt8 = new sbyte[][]
                        {
                            // Use RavenDB's quantization methods to convert float vectors to Int8
                            VectorQuantizer.ToInt8(new float[] { 0.1f, 0.2f }),
                            VectorQuantizer.ToInt8(new float[] { 0.3f, 0.4f })
                        },
                    };
                
                    var movie2 = new Movie()
                    {
                        Title = "The Shawshank Redemption",
                        Id = "movies/2",
                    
                        TagsEmbeddedAsSingle =new RavenVector<float>(new float[]
                        {
                            8.800000190734863f, 9.899999618530273f
                        }),
                        TagsEmbeddedAsBase64 = new List<string>() {"zcxMPs3MTD9mZmY/", "zcxMPpqZmT4zMzM/"},
                        TagsEmbeddedAsInt8 = new sbyte[][]
                        {
                            VectorQuantizer.ToInt8(new float[] { 0.5f, 0.6f }),
                            VectorQuantizer.ToInt8(new float[] { 0.7f, 0.8f })
                        }
                    };
                
                    session.Store(movie1);
                    session.Store(movie2);
                    session.SaveChanges();
                }
                #endregion
                
                using (var session = store.OpenSession())
                {
                    #region vs_4
                    var similarMovies = session.Query<Movie>()
                         // Perform a vector search
                         // Call the 'VectorSearch' method
                        .VectorSearch(
                            // Call 'WithEmbedding', specify:
                            // * The source field that contains the embedding in the document
                            // * The source embedding type
                            field => field.WithEmbedding(
                                x => x.TagsEmbeddedAsSingle, VectorEmbeddingType.Single),
                            // Call 'ByEmbedding'
                            // Provide the vector for the similarity comparison
                            queryVector => queryVector.ByEmbedding(
                                new RavenVector<float>(new float[] { 6.599999904632568f, 7.699999809265137f })),
                            // It is recommended to specify the minimum similarity level
                            0.85f,
                            // Optionally, specify the number of candidates for the search
                            10)
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_4_async
                    var similarMovies = await asyncSession.Query<Movie>()
                        .VectorSearch(
                            field => field.WithEmbedding(
                                x => x.TagsEmbeddedAsSingle, VectorEmbeddingType.Single),
                            queryVector => queryVector.ByEmbedding(
                                new RavenVector<float>(new float[] { 6.599999904632568f, 7.699999809265137f })),
                            0.85f,
                            10)
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region vs_5
                    var similarMovies = session.Advanced
                        .DocumentQuery<Movie>()
                        .VectorSearch(
                            field => field.WithEmbedding(
                                x => x.TagsEmbeddedAsSingle, VectorEmbeddingType.Single),
                            queryVector => queryVector.ByEmbedding(
                                new RavenVector<float>(new float[] { 6.599999904632568f, 7.699999809265137f })),
                            0.85f,
                            10)
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_5_async
                    var similarMovies = await asyncSession.Advanced
                        .AsyncDocumentQuery<Movie>()
                        .VectorSearch(
                            field => field.WithEmbedding(
                                x => x.TagsEmbeddedAsSingle, VectorEmbeddingType.Single),
                            queryVector => queryVector.ByEmbedding(
                                new RavenVector<float>(new float[] { 6.599999904632568f, 7.699999809265137f })),
                            0.85f,
                            10)
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
               
                using (var session = store.OpenSession())
                {
                    #region vs_6
                    var similarProducts = session.Advanced
                        .RawQuery<Movie>(@"
                            from 'Movies' 
                            where vector.search(TagsEmbeddedAsSingle, $queryVector, 0.85, 10)")
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
                    #region vs_6_async
                    var similarProducts = await asyncSession.Advanced
                        .AsyncRawQuery<Movie>(@"
                            from 'Movies' 
                            where vector.search(TagsEmbeddedAsSingle, $queryVector, 0.85, 10)")
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
                    #region vs_7
                    var similarMovies = session.Query<Movie>()
                        .VectorSearch(
                            // Call 'WithEmbedding', specify:
                            // * The source field that contains the embeddings in the document
                            // * The source embedding type
                            field => field.WithEmbedding(
                                x => x.TagsEmbeddedAsInt8, VectorEmbeddingType.Int8),
                            // Call 'ByEmbedding'
                            // Provide the vector for the similarity comparison
                            // (provide a single vector from the vector list in the TagsEmbeddedAsInt8 field)
                            queryVector => queryVector.ByEmbedding(
                                // The provided vector MUST be in the same format as was stored in your document
                                // Call 'VectorQuantizer.ToInt8' to transform the raw data to the Int8 format  
                                VectorQuantizer.ToInt8(new float[] { 0.1f, 0.2f })))
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToList();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region vs_8
                    var similarMovies = session.Query<Movie>()
                        .VectorSearch(
                            // Call 'WithBase64', specify:
                            // * The source field that contains the embeddings in the document
                            // * The source embedding type
                            //   (the type from which the Base64 string was constructed)
                            field => field.WithBase64(x => x.TagsEmbeddedAsBase64, VectorEmbeddingType.Single),
                            // Call 'ByBase64'
                            // Provide the Base64 string that represents the vector to query against
                            queryVector => queryVector.ByBase64("zczMPc3MTD6amZk+"))
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToList();
                    #endregion
                }
                
                // Examples for exact search
                // =========================
                
                using (var session = store.OpenSession())
                {
                    #region vs_9
                    var similarProducts = session.Query<Product>()
                        .VectorSearch(
                            field => field.WithText(x => x.Name),
                            searchTerm => searchTerm.ByText("italian food"),
                            // Optionally, set the 'isExact' param to true to perform an Exact search
                            isExact: true)
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_9_async
                    var similarProducts = await asyncSession.Query<Product>()
                        .VectorSearch(
                            field => field.WithText(x => x.Name), 
                            searchTerm => searchTerm.ByText("italian food"),
                            isExact: true)
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region vs_10
                    var similarProducts = session.Advanced
                        .DocumentQuery<Product>()
                        .VectorSearch(
                            field => field.WithText(x => x.Name),
                            searchTerm => searchTerm.ByText("italian food"),
                            isExact: true)
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_10_async
                    var similarProducts = await asyncSession.Advanced
                        .AsyncDocumentQuery<Product>()
                        .VectorSearch(
                            field => field.WithText(x => x.Name),
                            searchTerm => searchTerm.ByText("italian food"),
                            isExact: true)
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region vs_11
                    var similarProducts = session.Advanced
                        .RawQuery<Product>(@"
                            from 'Products'
                            // Wrap the query with the 'exact()' method
                            where exact(vector.search(embedding.text(Name), $searchTerm))")
                        .AddParameter("searchTerm", "italian food")
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_11_async
                    var similarProducts = await asyncSession.Advanced
                        .AsyncRawQuery<Product>(@"
                            from 'Products'
                            // Wrap the query with the 'exact()' method
                            where exact(vector.search(embedding.text(Name), $searchTerm))")
                        .AddParameter("searchTerm", "italian food")
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                // Examples for combined search
                // ============================
                
                using (var session = store.OpenSession())
                {
                    #region vs_12
                    var similarProducts = session.Query<Product>()
                         // Perform a filtering condition:
                        .Where(x => x.PricePerUnit > 35)
                         // Perform a vector search:
                        .VectorSearch(
                            field => field.WithText(x => x.Name),
                            searchTerm => searchTerm.ByText("italian food"),
                            0.75f, 16)
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_12_async
                    var similarProducts = await asyncSession.Query<Product>()
                        .Where(x => x.PricePerUnit > 35)
                        .VectorSearch(
                            field => field.WithText(x => x.Name),
                            searchTerm => searchTerm.ByText("italian food"),
                            0.75f, 16)
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region vs_13
                    var similarProducts = session.Advanced
                        .DocumentQuery<Product>()
                        .VectorSearch(
                            field => field.WithText(x => x.Name),
                            searchTerm => searchTerm.ByText("italian food"),
                            0.75f, 16)
                        .WhereGreaterThan(x => x.PricePerUnit, 35)
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_13_async
                    var similarProducts = await asyncSession.Advanced
                        .AsyncDocumentQuery<Product>()
                        .VectorSearch(
                            field => field.WithText(x => x.Name),
                            searchTerm => searchTerm.ByText("italian food"),
                            0.75f, 16)
                        .WhereGreaterThan(x => x.PricePerUnit, 35)
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region vs_14
                    var similarProducts = session.Advanced
                        .RawQuery<Product>(@"
                            from 'Products'
                            where (PricePerUnit > $minPrice) and (vector.search(embedding.text(Name), $searchTerm, 0.75, 16))")
                        .AddParameter("minPrice", 35.0)
                        .AddParameter("searchTerm", "italian food")
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_14_async
                    var similarProducts = await asyncSession.Advanced
                        .AsyncRawQuery<Product>(@"
                            from 'Products'
                            where (PricePerUnit > $minPrice) and (vector.search(embedding.text(Name), $searchTerm, 0.75, 16))")
                        .AddParameter("minPrice", 35.0)
                        .AddParameter("searchTerm", "italian food")
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                // Examples for quantization
                // =========================
                
                // Text => Int8
                using (var session = store.OpenSession())
                {
                    #region vs_15
                    var similarProducts = session.Query<Product>()
                        .VectorSearch(
                            field => field
                                // Specify the source text field for the embeddings
                                .WithText(x => x.Name)
                                // Set the quantization type for the generated embeddings
                                .TargetQuantization(VectorEmbeddingType.Int8),
                            searchTerm => searchTerm
                                // Provide the search term for comparison
                                .ByText("italian food"))
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_15_async
                    var similarProducts = await asyncSession.Query<Product>()
                        .VectorSearch(
                            field => field
                                .WithText(x => x.Name)
                                .TargetQuantization(VectorEmbeddingType.Int8),
                            searchTerm => searchTerm
                                .ByText("italian food"))
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region vs_16
                    var similarProducts = session.Advanced
                        .DocumentQuery<Product>()
                        .VectorSearch(
                            field => field
                                .WithText(x => x.Name)
                                .TargetQuantization(VectorEmbeddingType.Int8), 
                            searchTerm => searchTerm
                                .ByText("italian food"))
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_16_async
                    var similarProducts = await asyncSession.Advanced
                        .AsyncDocumentQuery<Product>()
                        .VectorSearch(
                            field => field
                                .WithText(x => x.Name)
                                .TargetQuantization(VectorEmbeddingType.Int8),
                            searchTerm => searchTerm
                                .ByText("italian food"))
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region vs_17
                    var similarProducts = session.Advanced
                        .RawQuery<Product>(@"
                            from 'Products'
                            // Wrap the 'Name' field with 'embedding.text_i8'
                            where vector.search(embedding.text_i8(Name), $searchTerm)")
                        .AddParameter("searchTerm", "italian food")
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_17_async
                    var similarProducts = await asyncSession.Advanced
                        .AsyncRawQuery<Product>(@"
                            from 'Products'
                            // Wrap the 'Name' field with 'embedding.text_i8'
                            where vector.search(embedding.text_i8(Name), $searchTerm)")
                        .AddParameter("searchTerm", "italian food")
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                /// F32 => Binary
                using (var session = store.OpenSession())
                {
                    #region vs_18
                    var similarMovies = session.Query<Movie>()
                        .VectorSearch(
                            field => field
                                // Specify the source field and its type   
                                .WithEmbedding(x => x.TagsEmbeddedAsSingle, VectorEmbeddingType.Single)
                                // Set the quantization type for the generated embeddings
                                .TargetQuantization(VectorEmbeddingType.Binary),
                            queryVector => queryVector
                                // Provide the vector to use for comparison
                                .ByEmbedding(new RavenVector<float>(new float[]
                                {
                                    6.599999904632568f, 7.699999809265137f
                                })))
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_18_async
                    var similarMovies = await asyncSession.Query<Movie>()
                        .VectorSearch(
                            field => field
                                .WithEmbedding(x => x.TagsEmbeddedAsSingle, VectorEmbeddingType.Single)
                                .TargetQuantization(VectorEmbeddingType.Binary),
                            queryVector => queryVector
                                .ByEmbedding(new RavenVector<float>(new float[]
                                 {
                                     6.599999904632568f, 7.699999809265137f
                                 })))
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region vs_19
                    var similarProducts = session.Advanced
                        .DocumentQuery<Movie>()
                        .VectorSearch(
                            field => field
                                .WithEmbedding(x => x.TagsEmbeddedAsSingle, VectorEmbeddingType.Single)
                                .TargetQuantization(VectorEmbeddingType.Binary),
                            queryVector => queryVector
                                .ByEmbedding(new RavenVector<float>(new float[]
                                 {
                                     6.599999904632568f, 7.699999809265137f
                                 })))
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_19_async
                    var similarProducts = await asyncSession.Advanced
                        .AsyncDocumentQuery<Movie>()
                        .VectorSearch(
                            field => field
                                .WithEmbedding(x => x.TagsEmbeddedAsSingle, VectorEmbeddingType.Single)
                                .TargetQuantization(VectorEmbeddingType.Binary),
                            queryVector => queryVector
                                .ByEmbedding(new RavenVector<float>(new float[]
                                 {
                                     6.599999904632568f, 7.699999809265137f
                                 })))
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region vs_20
                    var similarMovies = session.Advanced
                        .RawQuery<Movie>(@"
                            from 'Movies'
                            // Wrap the 'TagsEmbeddedAsSingle' field with 'embedding.f32_i1'
                            where vector.search(embedding.f32_i1(TagsEmbeddedAsSingle), $queryVector)")
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
                    #region vs_20_async
                    var similarMovies = await asyncSession.Advanced
                        .AsyncRawQuery<Movie>(@"
                            from 'Movies'
                            // Wrap the 'TagsEmbeddedAsSingle' field with 'embedding.f32_i1'
                            where vector.search(embedding.f32_i1(TagsEmbeddedAsSingle), $queryVector)")
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
                    #region vs_21
                    var similarCategories = session.Query<Category>()
                        .VectorSearch(
                            field => field
                                 // Call 'WithText'
                                 // Specify the document field in which to search for similar values
                                .WithText(x => x.Name)
                                 // Call 'UsingTask'
                                 // Specify the identifier of the task that generated
                                 // the embeddings for the Name field
                                .UsingTask("id-for-task-open-ai"),
                            // Call 'ByText' 
                            // Provide the search term for the similarity comparison
                            searchTerm => searchTerm.ByText("candy"),
                            // It is recommended to specify the minimum similarity level
                            0.75f)
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_21_async
                    var similarCategories = await asyncSession.Query<Category>()
                        .VectorSearch(
                            field => field
                                .WithText(x => x.Name)
                                .UsingTask("id-for-task-open-ai"),
                            searchTerm => searchTerm.ByText("candy"),
                            0.75f)
                        .Customize(x => x.WaitForNonStaleResults())
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region vs_22
                    var similarCategories = session.Advanced
                        .DocumentQuery<Category>()
                        .VectorSearch(
                            field => field
                                .WithText(x => x.Name)
                                .UsingTask("id-for-task-open-ai"),
                            searchTerm => searchTerm.ByText("candy"),
                            0.75f)
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_22_async
                    var similarCategories = await asyncSession.Advanced
                        .AsyncDocumentQuery<Category>()
                        .VectorSearch(
                            field => field
                                .WithText(x => x.Name)
                                .UsingTask("id-for-task-open-ai"),
                            searchTerm => searchTerm.ByText("candy"),
                            0.75f)
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region vs_23
                    var similarCategories = session.Advanced
                        .RawQuery<Category>(@"
                            from 'Categories'
                            // Specify the identifier of the task that generated the embeddings inside 'ai.task'
                            where vector.search(embedding.text(Name, ai.task('id-for-task-open-ai')), $searchTerm, 0.75)")
                        .AddParameter("searchTerm", "candy")
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region vs_23_async
                    var similarCategories = await asyncSession.Advanced
                        .AsyncRawQuery<Category>(@"
                            from 'Categories'
                            // Specify the identifier of the task that generated the embeddings inside 'ai.task'
                            where vector.search(embedding.text(Name, ai.task('id-for-task-open-ai')), $searchTerm, 0.75)")
                        .AddParameter("searchTerm", "candy")
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
            }
        }
        
        private interface IFoo 
        {
            #region syntax_1
            public IRavenQueryable<T> VectorSearch<T>(
                Func<IVectorFieldFactory<T>, IVectorEmbeddingTextField> textFieldFactory,
                Action<IVectorEmbeddingTextFieldValueFactory> textValueFactory,
                float? minimumSimilarity = null,
                int? numberOfCandidates = null,
                bool isExact = false);

            public IRavenQueryable<T> VectorSearch<T>(
                Func<IVectorFieldFactory<T>, IVectorEmbeddingField> embeddingFieldFactory,
                Action<IVectorEmbeddingFieldValueFactory> embeddingValueFactory,
                float? minimumSimilarity = null,
                int? numberOfCandidates = null,
                bool isExact = false);
            
            public IRavenQueryable<T> VectorSearch<T>(
                Func<IVectorFieldFactory<T>, IVectorField> embeddingFieldFactory,
                Action<IVectorFieldValueFactory> embeddingValueFactory,
                float? minimumSimilarity = null,
                int? numberOfCandidates = null,
                bool isExact = false);
            #endregion
            
            #region syntax_2
            public interface IVectorFieldFactory<T>
            {
                // Methods for the dynamic query:
                public IVectorEmbeddingTextField WithText(string documentFieldName);
                public IVectorEmbeddingTextField WithText(Expression<Func<T, object>> propertySelector);
                public IVectorEmbeddingField WithEmbedding(string documentFieldName, 
                    VectorEmbeddingType storedEmbeddingQuantization = VectorEmbeddingType.Single);
                public IVectorEmbeddingField WithEmbedding(Expression<Func<T, object>> propertySelector,
                    VectorEmbeddingType storedEmbeddingQuantization = VectorEmbeddingType.Single);
                public IVectorEmbeddingField WithBase64(string documentFieldName,
                    VectorEmbeddingType storedEmbeddingQuantization = VectorEmbeddingType.Single);
                public IVectorEmbeddingField WithBase64(Expression<Func<T, object>> propertySelector,
                    VectorEmbeddingType storedEmbeddingQuantization = VectorEmbeddingType.Single);
                
                // Methods for querying a static index:
                public IVectorField WithField(string indexFieldName);
                public IVectorField WithField(Expression<Func<T, object>> indexPropertySelector);
            }
            #endregion
            
            #region syntax_3
            public interface IVectorEmbeddingTextField
            {
                public IVectorEmbeddingTextField TargetQuantization(
                    VectorEmbeddingType targetEmbeddingQuantization);
                
                public IVectorEmbeddingTextField UsingTask(
                    string embeddingsGenerationTaskIdentifier);
            }

            public interface IVectorEmbeddingField
            {
                public IVectorEmbeddingField TargetQuantization(
                    VectorEmbeddingType targetEmbeddingQuantization);
            }
            #endregion
            
            #region syntax_4
            public enum VectorEmbeddingType
            {
                Single,
                Int8,
                Binary,
                Text
            }
            #endregion
            
            #region syntax_5
            public interface IVectorEmbeddingTextFieldValueFactory
            {
                // Defines the queried text
                public void ByText(string text);
            }
            public interface IVectorEmbeddingFieldValueFactory
            {
                // Define the queried embedding:
                // =============================
                
                // 'embeddings' is an Enumerable containing embedding values.
                public void ByEmbedding<T>(IEnumerable<T> embedding) where T : unmanaged, INumber<T>;
                
                // 'embeddings' is an array containing embedding values.
                public void ByEmbedding<T>(T[] embedding) where T : unmanaged, INumber<T>;
                
                // Defines queried embedding in base64 format.
                // 'base64Embedding' is encoded as base64 string.
                public void ByBase64(string base64Embedding);
                
                // 'embedding` is a `RavenVector` containing embedding values.
                public void ByEmbedding<T>(RavenVector<T> embedding) where T : unmanaged, INumber<T>;
            }
            #endregion
            
            #region syntax_6
            public class RavenVector<T>()
            {
                public T[] Embedding { get; set; }
            } 
            #endregion
            
            /*
            #region syntax_7
            public static class VectorQuantizer
            {
                public static sbyte[] ToInt8(float[] rawEmbedding);
                public static byte[] ToInt1(ReadOnlySpan<float> rawEmbedding);
            }
            #endregion
            */
        }
    }
}

#region movie_class
// Sample class representing a document with various formats of numerical vectors
public class Movie
{
    public string Id { get; set; }
    public string Title { get; set; }
    
    // This field will hold numerical vector data - Not quantized
    public RavenVector<float> TagsEmbeddedAsSingle { get; set; }
    
    // This field will hold numerical vector data - Quantized to Int8
    public sbyte[][] TagsEmbeddedAsInt8 { get; set; }
    
    // This field will hold numerical vector data - Encoded in Base64 format
    public List<string> TagsEmbeddedAsBase64 { get; set; }
}
#endregion

/*
#region sample_document
{
    "Title": "Hidden Figures",
    
    "TagsEmbeddedAsSingle": {
        "@vector": [
            6.599999904632568,
            7.699999809265137
        ]
    },
    
    "TagsEmbeddedAsInt8": [
        [
            64,
            127,
            -51,
            -52,
            76,
            62
        ],
        [
            95,
            127,
            -51,
            -52,
            -52,
            62
        ]
    ],
    
    "TagsEmbeddedAsBase64": [
        "zczMPc3MTD6amZk+",
        "mpmZPs3MzD4AAAA/"
    ],
    
    "@metadata": {
        "@collection": "Movies"
    }
}
#endregion
*/

