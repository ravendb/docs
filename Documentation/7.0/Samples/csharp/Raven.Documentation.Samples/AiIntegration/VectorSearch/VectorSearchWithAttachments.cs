using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Indexes.Vector;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.AiIntegration
{
    public class VectorSearchWithAttachments
    {
        // The indexes:
        // ============
        
        #region index_1
        public class Companies_ByVector_FromTextAttachment :
            AbstractIndexCreationTask<Company, Companies_ByVector_FromTextAttachment.IndexEntry>
        {
            public class IndexEntry()
            {
                // This index-field will hold embeddings
                // generated from the TEXT in the attachments.
                public object VectorFromAttachment { get; set; }
            }
        
            public Companies_ByVector_FromTextAttachment()
            {
                Map = companies => from company in companies
                    
                    // Load the attachment from the document (ensure it is not null)
                    let attachment = LoadAttachment(company, "description.txt")
                    where attachment != null
                    
                    select new IndexEntry()
                    {
                        // Index the text content from the attachment in the vector field
                        VectorFromAttachment =
                            CreateVector(attachment.GetContentAsString(Encoding.UTF8))
                    };

                // Configure the vector field:
                VectorIndexes.Add(x => x.VectorFromAttachment,
                    new VectorOptions()
                    {
                        // Specify 'Text' as the source format
                        SourceEmbeddingType = VectorEmbeddingType.Text,
                        // Specify the desired destination format within the index
                        DestinationEmbeddingType = VectorEmbeddingType.Single
                    });
            
                SearchEngineType = Raven.Client.Documents.Indexes.SearchEngineType.Corax;
            }
        }
        #endregion
        
        #region Index_2
        public class Companies_ByVector_FromTextAttachment_JS :
            AbstractJavaScriptIndexCreationTask
        {
            public Companies_ByVector_FromTextAttachment_JS()
            {
                Maps = new HashSet<string>
                {
                    $@"map('Companies', function (company) {{

                           var attachment = loadAttachment(company, 'description.txt');
                           if (!attachment) return null;
 
                           return {{
                               VectorFromAttachment: createVector(attachment.getContentAsString('utf8'))
                           }};
                    }})"
                };

                Fields = new Dictionary<string, IndexFieldOptions>()
                {
                    {
                        "VectorFromAttachment",
                        new IndexFieldOptions()
                        {
                            Vector = new()
                            {
                                SourceEmbeddingType = VectorEmbeddingType.Text,
                                DestinationEmbeddingType = VectorEmbeddingType.Single
                            }
                        }
                    }
                };
            
                SearchEngineType = Raven.Client.Documents.Indexes.SearchEngineType.Corax;
            }
        }
        #endregion
        
        #region index_4
        public class Companies_ByVector_FromNumericalAttachment :
            AbstractIndexCreationTask<Company, Companies_ByVector_FromNumericalAttachment.IndexEntry>
        {
            public class IndexEntry()
            {
                // This index-field will hold embeddings
                // generated from the NUMERICAL content in the attachments.
                public object VectorFromAttachment { get; set; }
            }
    
            public Companies_ByVector_FromNumericalAttachment()
            {
                Map = companies => from company in companies
                    
                    // Load the attachment from the document (ensure it is not null)
                    let attachment = LoadAttachment(company, "vector.raw")
                    where attachment != null
                    
                    select new IndexEntry
                    {
                        // Index the attachment's content in the vector field
                        VectorFromAttachment = CreateVector(attachment.GetContentAsStream())
                    };

                // Configure the vector field:
                VectorIndexes.Add(x => x.VectorFromAttachment,
                    new VectorOptions()
                    {
                        // Define the source embedding type
                        SourceEmbeddingType = VectorEmbeddingType.Single,
                        // Define the desired destination format within the index
                        DestinationEmbeddingType = VectorEmbeddingType.Single
                    });

                SearchEngineType = Raven.Client.Documents.Indexes.SearchEngineType.Corax;
            }
        }
        #endregion
        
        #region Index_6
        public class Companies_ByVector_FromNumericalAttachment_JS :
            AbstractJavaScriptIndexCreationTask
        {
            public Companies_ByVector_FromNumericalAttachment_JS()
            {
                Maps = new HashSet<string>()
                {
                    $@"map('Companies', function (company) {{

                         var attachment = loadAttachment(company, 'vector_base64.raw');
                         if (!attachment) return null;

                         return {{
                             VectorFromAttachment: createVector(attachment.getContentAsString('utf8'))
                         }};
                     }})"
                };
            
                Fields = new();
                Fields.Add("VectorFromAttachment", new IndexFieldOptions()
                {
                    Vector = new VectorOptions()
                    {
                        SourceEmbeddingType = VectorEmbeddingType.Single, 
                        DestinationEmbeddingType = VectorEmbeddingType.Single
                    }
                });
        
                SearchEngineType = Raven.Client.Documents.Indexes.SearchEngineType.Corax;
            }
        }
        #endregion
        
        #region index_7
        public class Companies_ByVector_AllAttachments :
            AbstractIndexCreationTask<Company, Companies_ByVector_AllAttachments.IndexEntry>
        {
            public class IndexEntry()
            {
                // This index-field will hold embeddings
                // generated from the NUMERICAL content of ALL attachments.
                public object VectorFromAttachment { get; set; }
            }
    
            public Companies_ByVector_AllAttachments()
            {
                Map = companies => from company in companies
                    
                    // Load ALL attachments from the document
                    let attachments = LoadAttachments(company)
                    
                    select new IndexEntry
                    {
                        // Index the attachments content in the vector field
                        VectorFromAttachment = CreateVector(
                            attachments.Select(e => e.GetContentAsStream()))
                    };

                // Configure the vector field:
                VectorIndexes.Add(x => x.VectorFromAttachment,
                    new VectorOptions()
                    {
                        SourceEmbeddingType = VectorEmbeddingType.Single,
                        DestinationEmbeddingType = VectorEmbeddingType.Single
                    });

                SearchEngineType = Raven.Client.Documents.Indexes.SearchEngineType.Corax;
            }
        }
        #endregion
        
        // Index definitions:
        // ==================

        public void IndexDefinitionExamples()
        {
            using (var store = new DocumentStore())
            {
                #region Index_3
                var indexDefinition = new IndexDefinition
                {
                    Name = "Companies/ByVector/FromTextAttachment",
                    
                    Maps = new HashSet<string>
                    {
                        @"from company in docs.Companies

                          let attachment = LoadAttachment(company, ""description.txt"")
                          where attachment != null

                          select new 
                          {
                              VectorFromAttachment =
                                  CreateVector(attachment.GetContentAsString(Encoding.UTF8))
                          }"
                    },
                    
                    Fields = new Dictionary<string, IndexFieldOptions>()
                    {
                        {
                            "VectorFromAttachment",
                            new IndexFieldOptions()
                            {
                                Vector = new VectorOptions()
                                {
                                    SourceEmbeddingType = VectorEmbeddingType.Text,
                                    DestinationEmbeddingType = VectorEmbeddingType.Single
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
                #region Index_5
                var indexDefinition = new IndexDefinition 
                {
                    Name = "Companies/ByVector/FromNumericalAttachment",

                    Maps = new HashSet<string>
                    {
                        @"from company in docs.Companies

                          let attachment = LoadAttachment(company, ""vector.raw"")
                          where attachment != null
                    
                          select new
                          {
                              VectorFromAttachment = CreateVector(attachment.GetContentAsStream())
                          }"
                    },
                    
                    Fields = new Dictionary<string, IndexFieldOptions>()
                    {
                        {
                            "VectorFromAttachment",
                            new IndexFieldOptions()
                            {
                                Vector = new VectorOptions()
                                {
                                    SourceEmbeddingType = VectorEmbeddingType.Single,
                                    DestinationEmbeddingType = VectorEmbeddingType.Single
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
                #region Index_8
                var indexDefinition = new IndexDefinition 
                {
                    Name = "Companies/ByVector/AllAttachments",

                    Maps = new HashSet<string>
                    {
                        @"from company in docs.Companies

                          let attachments = LoadAttachments(company)
                    
                          select new
                          {
                              VectorFromAttachment =
                                  CreateVector(attachments.Select(e => e.GetContentAsStream()))
                          }"
                    },
                    
                    Fields = new Dictionary<string, IndexFieldOptions>()
                    {
                        {
                            "VectorFromAttachment",
                            new IndexFieldOptions()
                            {
                                Vector = new VectorOptions()
                                {
                                    SourceEmbeddingType = VectorEmbeddingType.Single,
                                    DestinationEmbeddingType = VectorEmbeddingType.Single,
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
        
        // Storing the attachments:
        // ========================

        public async Task StoreAttachments()
        {
            using (var store = new DocumentStore())
            {
                #region store_attachments_1
                // Prepare text as `byte[]` to be stored as attachments:
                // =====================================================
                var byteArray1 = Encoding.UTF8.GetBytes(
                    "Supplies soft drinks, fruit juices, and flavored syrups to restaurants and retailers.");
                var byteArray2 = Encoding.UTF8.GetBytes(
                    "Supplies fine dining restaurants with premium meats, cheeses, and wines across France.");
                var byteArray3 = Encoding.UTF8.GetBytes(
                    "An American grocery chain known for its fresh produce, organic foods, and local meats.");
                var byteArray4 = Encoding.UTF8.GetBytes(
                    "An Asian grocery store specializing in ingredients for Japanese and Thai cuisine.");
                var byteArray5 = Encoding.UTF8.GetBytes(
                    "A rural general store offering homemade jams, fresh-baked bread, and locally crafted gifts.");
                
                using (var session = store.OpenSession())
                {
                    // Load existing Company documents from RavenDB's sample data:
                    // ===========================================================
                    var company1 = session.Load<Company>("companies/11-A");
                    var company2 = session.Load<Company>("companies/26-A");
                    var company3 = session.Load<Company>("companies/32-A");
                    var company4 = session.Load<Company>("companies/41-A");
                    var company5 = session.Load<Company>("companies/43-A");
                    
                    // Store the attachments in the documents (using MemoryStream):
                    // ============================================================
                    session.Advanced.Attachments.Store(company1, "description.txt",
                        new MemoryStream(byteArray1), "text/plain");
                    session.Advanced.Attachments.Store(company2, "description.txt",
                        new MemoryStream(byteArray2), "text/plain");
                    session.Advanced.Attachments.Store(company3, "description.txt",
                        new MemoryStream(byteArray3), "text/plain");
                    session.Advanced.Attachments.Store(company4, "description.txt",
                        new MemoryStream(byteArray4), "text/plain");
                    session.Advanced.Attachments.Store(company5, "description.txt",
                        new MemoryStream(byteArray5), "text/plain");
                    
                    session.SaveChanges();
                }
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region store_attachments_2
                // These vectors are simple pre-computed embedding vectors with 32-bit floating-point values.
                // Note: In a real scenario, embeddings would be generated by a model.
                // ==========================================================================================
                var v1 = new float[] { 0.1f, 0.2f, 0.3f, 0.4f };
                var v2 = new float[] { 0.1f, 0.7f, 0.8f, 0.9f };
                var v3 = new float[] { 0.5f, 0.6f, 0.7f, 0.8f };
                
                // Prepare the embedding vectors as `byte[]` to be stored as attachments:
                // =====================================================================
                var byteArray1 = MemoryMarshal.Cast<float, byte>(v1).ToArray();
                var byteArray2 = MemoryMarshal.Cast<float, byte>(v2).ToArray();
                var byteArray3 = MemoryMarshal.Cast<float, byte>(v3).ToArray();
                
                using (var session = store.OpenSession())
                {
                    // Load existing Company documents from RavenDB's sample data:
                    // ===========================================================
                    var company1 = session.Load<Company>("companies/50-A");
                    var company2 = session.Load<Company>("companies/51-A");
                    var company3 = session.Load<Company>("companies/52-A");
                    
                    // Store the attachments in the documents (using MemoryStream):
                    // ============================================================
                    session.Advanced.Attachments.Store(company1, "vector.raw", new MemoryStream(byteArray1));
                    session.Advanced.Attachments.Store(company2, "vector.raw", new MemoryStream(byteArray2));
                    session.Advanced.Attachments.Store(company3, "vector.raw", new MemoryStream(byteArray3));
                    
                    session.SaveChanges();
                }
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region store_attachments_3
                // These vectors are simple pre-computed embedding vectors with 32-bit floating-point values.
                // Note: In a real scenario, embeddings would be generated by a model.
                // ==========================================================================================
                var v1 = new float[] { 0.1f, 0.2f, 0.3f, 0.4f };
                var v2 = new float[] { 0.1f, 0.7f, 0.8f, 0.9f };
                var v3 = new float[] { 0.5f, 0.6f, 0.7f, 0.8f };
                
                // Prepare the embedding vectors as a BASE64 string to be stored as attachments:
                // =============================================================================
                var base64ForV1 = Convert.ToBase64String(MemoryMarshal.Cast<float, byte>(v1));
                var base64ForV2 = Convert.ToBase64String(MemoryMarshal.Cast<float, byte>(v2));
                var base64ForV3 = Convert.ToBase64String(MemoryMarshal.Cast<float, byte>(v3));
                
                // Convert to byte[] for streaming:
                // ================================
                var byteArray1 = Encoding.UTF8.GetBytes(base64ForV1);
                var byteArray2 = Encoding.UTF8.GetBytes(base64ForV2);
                var byteArray3 = Encoding.UTF8.GetBytes(base64ForV3);
                
                using (var session = store.OpenSession())
                {
                    // Load existing Company documents from RavenDB's sample data:
                    // ===========================================================
                    var company1 = session.Load<Company>("companies/60-A");
                    var company2 = session.Load<Company>("companies/61-A");
                    var company3 = session.Load<Company>("companies/62-A");
                    
                    // Store the attachments in the documents (using MemoryStream):
                    // ============================================================
                    session.Advanced.Attachments.Store(company1, "vector_base64.raw", new MemoryStream(byteArray1));
                    session.Advanced.Attachments.Store(company2, "vector_base64.raw", new MemoryStream(byteArray2));
                    session.Advanced.Attachments.Store(company3, "vector_base64.raw", new MemoryStream(byteArray3));
                    
                    session.SaveChanges();
                }
                #endregion
            }
            
            using (var store = new DocumentStore())
            {
                #region store_attachments_4
                // These vectors are simple pre-computed embedding vectors with 32-bit floating-point values.
                // Note: In a real scenario, embeddings would be generated by a model.
                // ==========================================================================================
                var v1 = new float[] { 0.1f, 0.2f, 0.3f, 0.4f };
                var v2 = new float[] { 0.5f, 0.6f, 0.7f, 0.8f };
                
                var v3 = new float[] { -0.1f, 0.2f, -0.7f, -0.8f };
                var v4 = new float[] { 0.3f, -0.6f, 0.9f, -0.9f };
                
                // Prepare the embedding vectors as `byte[]` to be stored as attachments:
                // =====================================================================
                var byteArray1 = MemoryMarshal.Cast<float, byte>(v1).ToArray();
                var byteArray2 = MemoryMarshal.Cast<float, byte>(v2).ToArray();
                
                var byteArray3 = MemoryMarshal.Cast<float, byte>(v3).ToArray();
                var byteArray4 = MemoryMarshal.Cast<float, byte>(v4).ToArray();
                
                using (var session = store.OpenSession())
                {
                    // Load existing Company documents from RavenDB's sample data:
                    // ===========================================================
                    var company1 = session.Load<Company>("companies/70-A");
                    var company2 = session.Load<Company>("companies/71-A");
                    
                    // Store multiple attachments in the documents (using MemoryStream):
                    // =================================================================
                    
                    session.Advanced.Attachments.Store(company1, "vector1.raw", new MemoryStream(byteArray1));
                    session.Advanced.Attachments.Store(company1, "vector2.raw", new MemoryStream(byteArray2));
                    
                    session.Advanced.Attachments.Store(company2, "vector1.raw", new MemoryStream(byteArray3));
                    session.Advanced.Attachments.Store(company2, "vector2.raw", new MemoryStream(byteArray4));
                    
                    session.SaveChanges();
                }
                #endregion
            }
        }
        
        public async Task QueryExamples()
        {
            using (var store = new DocumentStore())
            {
                // Query for textual content in attachments
                // ========================================
                
                using (var session = store.OpenSession())
                {
                    #region query_1
                    var relevantCompanies = session
                        .Query<Companies_ByVector_FromTextAttachment.IndexEntry,
                            Companies_ByVector_FromTextAttachment>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromAttachment),
                            searchTerm => searchTerm
                                .ByText("chinese food"), 0.8f)
                        .Customize(x => x.WaitForNonStaleResults())
                        .OfType<Company>()
                        .ToList();
                    #endregion
                    
                    #region extract_attachment_content
                    // Extract text from the attachment of the first resulting document
                    // ================================================================
                    
                    // Retrieve the attachment stream
                    var company = relevantCompanies[0];
                    var attachmentResult =  session.Advanced.Attachments.Get(company, "description.txt");
                    var attStream = attachmentResult.Stream;
                
                    // Read the attachment content into memory and decode it as a UTF-8 string
                    var ms = new MemoryStream();
                    attStream.CopyTo(ms);
                    string attachmentText = Encoding.UTF8.GetString(ms.ToArray());
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_1_async
                    var relevantCompanies = await asyncSession
                        .Query<Companies_ByVector_FromTextAttachment.IndexEntry,
                            Companies_ByVector_FromTextAttachment>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromAttachment),
                            searchTerm => searchTerm
                                .ByText("chinese food"), 0.8f)
                        .Customize(x => x.WaitForNonStaleResults())
                        .OfType<Company>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_2
                    var relevantCompanies = session.Advanced
                        .DocumentQuery<Companies_ByVector_FromTextAttachment.IndexEntry,
                            Companies_ByVector_FromTextAttachment>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromAttachment),
                            searchTerm => searchTerm
                                .ByText("chinese food"), 0.8f)
                        .WaitForNonStaleResults()
                        .OfType<Company>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_2_async
                    var relevantCompanies = await asyncSession.Advanced
                        .AsyncDocumentQuery<Companies_ByVector_FromTextAttachment.IndexEntry,
                            Companies_ByVector_FromTextAttachment>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromAttachment),
                            searchTerm => searchTerm
                                .ByText("chinese food"), 0.8f)
                        .WaitForNonStaleResults()
                        .OfType<Company>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_3
                    var relevantCompanies = session.Advanced
                        .RawQuery<Company>(@"
                            from index 'Companies/ByVector/FromTextAttachment'
                            where vector.search(VectorFromAttachment, $searchTerm, 0.8)")
                        .AddParameter("searchTerm", "chinese food")
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_3_async
                    var relevantCompanies = await asyncSession.Advanced
                        .AsyncRawQuery<Company>(@"
                            from index 'Companies/ByVector/FromTextAttachment'
                            where vector.search(VectorFromAttachment, $searchTerm, 0.8)")
                        .AddParameter("searchTerm", "chinese food")
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                // Query for numerical content
                // ===========================
                
                using (var session = store.OpenSession())
                {
                    #region query_4
                    var similarCompanies = session
                        .Query<Companies_ByVector_FromNumericalAttachment.IndexEntry,
                            Companies_ByVector_FromNumericalAttachment>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromAttachment),
                            queryVector => queryVector
                                .ByEmbedding(new float[] { 0.1f, 0.2f, 0.3f, 0.4f }))
                        .Customize(x => x.WaitForNonStaleResults())
                        .OfType<Company>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_4_async
                    var similarCompanies = await asyncSession
                        .Query<Companies_ByVector_FromNumericalAttachment.IndexEntry,
                            Companies_ByVector_FromNumericalAttachment>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromAttachment),
                            queryVector => queryVector
                                .ByEmbedding(new float[] { 0.1f, 0.2f, 0.3f, 0.4f }))
                        .Customize(x => x.WaitForNonStaleResults())
                        .OfType<Company>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_5
                    var similarCompanies = session.Advanced
                        .DocumentQuery<Companies_ByVector_FromNumericalAttachment.IndexEntry,
                            Companies_ByVector_FromNumericalAttachment>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromAttachment),
                            queryVector => queryVector
                                .ByEmbedding(new float[] { 0.1f, 0.2f, 0.3f, 0.4f }))
                        .WaitForNonStaleResults()
                        .OfType<Company>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_5_async
                    var similarCompanies = await asyncSession.Advanced
                        .AsyncDocumentQuery<Companies_ByVector_FromNumericalAttachment.IndexEntry,
                            Companies_ByVector_FromNumericalAttachment>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromAttachment),
                            queryVector => queryVector
                                .ByEmbedding(new float[] { 0.1f, 0.2f, 0.3f, 0.4f }))
                        .WaitForNonStaleResults()
                        .OfType<Company>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_6
                    var similarCompanies = session.Advanced
                        .RawQuery<Company>(@"
                            from index 'Companies/ByVector/FromNumericalAttachment'
                            where vector.search(VectorFromAttachment, $queryVector)")
                        .AddParameter("queryVector", new float[] { 0.1f, 0.2f, 0.3f, 0.4f })
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_6_async
                    var similarCompanies = await asyncSession.Advanced
                        .AsyncRawQuery<Company>(@"
                            from index 'Companies/ByVector/FromNumericalAttachment'
                            where vector.search(VectorFromAttachment, $queryVector)")
                        .AddParameter("queryVector", new float[] { 0.1f, 0.2f, 0.3f, 0.4f })
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_7
                    var similarCompanies = session.Advanced
                        .RawQuery<Company>(@"
                            from index 'Companies/ByVector/FromNumericalAttachment/JS'
                            where vector.search(VectorFromAttachment, $queryVector)")
                        .AddParameter("queryVector", new float[] { 0.1f, 0.2f, 0.3f, 0.4f })
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_7_async
                    var similarCompanies = await asyncSession.Advanced
                        .AsyncRawQuery<Company>(@"
                            from index 'Companies/ByVector/FromNumericalAttachment/JS'
                            where vector.search(VectorFromAttachment, $queryVector)")
                        .AddParameter("queryVector", new float[] { 0.1f, 0.2f, 0.3f, 0.4f })
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_8
                    var similarCompanies = session
                        .Query<Companies_ByVector_AllAttachments.IndexEntry,
                            Companies_ByVector_AllAttachments>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromAttachment),
                            queryVector => queryVector
                                .ByEmbedding(new float[] { -0.1f, 0.2f, -0.7f, -0.8f }))
                        .Customize(x => x.WaitForNonStaleResults())
                        .OfType<Company>()
                        .ToList();
                        #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_8_async
                    var similarCompanies = await asyncSession
                        .Query<Companies_ByVector_AllAttachments.IndexEntry,
                            Companies_ByVector_AllAttachments>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromAttachment),
                            queryVector => queryVector
                                .ByEmbedding(new float[] { -0.1f, 0.2f, -0.7f, -0.8f }))
                        .Customize(x => x.WaitForNonStaleResults())
                        .OfType<Company>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_9
                    var similarCompanies = session.Advanced
                        .DocumentQuery<Companies_ByVector_AllAttachments.IndexEntry,
                            Companies_ByVector_AllAttachments>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromAttachment),
                            queryVector => queryVector
                                .ByEmbedding(new float[] { -0.1f, 0.2f, -0.7f, -0.8f }))
                        .WaitForNonStaleResults()
                        .OfType<Company>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_9_async
                    var similarCompanies = await asyncSession.Advanced
                        .AsyncDocumentQuery<Companies_ByVector_AllAttachments.IndexEntry,
                            Companies_ByVector_AllAttachments>()
                        .VectorSearch(
                            field => field
                                .WithField(x => x.VectorFromAttachment),
                            queryVector => queryVector
                                .ByEmbedding(new float[] { -0.1f, 0.2f, -0.7f, -0.8f }))
                        .WaitForNonStaleResults()
                        .OfType<Company>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_10
                    var similarCompanies = session.Advanced
                        .RawQuery<Company>(@"
                            from index 'Companies/ByVector/AllAttachments'
                            where vector.search(VectorFromAttachment, $queryVector)")
                        .AddParameter("queryVector", new float[] { 0.1f, 0.2f, -0.7f, -0.8f })
                        .WaitForNonStaleResults()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_10_async
                    var similarCompanies = await asyncSession.Advanced
                        .AsyncRawQuery<Company>(@"
                            from index 'Companies/ByVector/AllAttachments'
                            where vector.search(VectorFromAttachment, $queryVector)")
                        .AddParameter("queryVector", new float[] { 0.1f, 0.2f, -0.7f, -0.8f })
                        .WaitForNonStaleResults()
                        .ToListAsync();
                    #endregion
                }
            }
        }
    }
}
