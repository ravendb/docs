using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    public class Metadata
    {
        #region index_1
        public class Products_ByMetadata_AccessViaValue : AbstractIndexCreationTask<Product>
        {
            public class IndexEntry
            {
                public DateTime LastModified { get; set; }
                public bool HasCounters { get; set; }
            }

            public Products_ByMetadata_AccessViaValue()
            {
                Map = products => from product in products
                    // Use 'MetadataFor' to access the metadata object
                    let metadata = MetadataFor(product)
                    
                    // Define the index fields
                    select new IndexEntry()
                    {
                        // Access metadata properties using generic method
                        LastModified = metadata.Value<DateTime>(
                            // Specify the Client API Constant corresponding to '@last-modified'
                            Raven.Client.Constants.Documents.Metadata.LastModified),
                        
                        HasCounters =  metadata.Value<object>(
                            // Specify the Client API Constant corresponding to '@counters'
                            Raven.Client.Constants.Documents.Metadata.Counters) != null
                    };
            }
        }
        #endregion
        
        #region index_2
        public class Products_ByMetadata_AccessViaIndexer : AbstractIndexCreationTask<Product>
        {
            public class IndexEntry
            {
                public DateTime LastModified { get; set; }
                public bool HasCounters { get; set; }
            }

            public Products_ByMetadata_AccessViaIndexer()
            {
                Map = products => from product in products
                    // Use 'MetadataFor' to access the metadata object
                    let metadata = MetadataFor(product)
                    
                    // Define the index fields
                    select new IndexEntry()
                    {
                        // Access metadata properties using indexer
                        LastModified =
                            // Specify the Client API Constant corresponding to '@last-modified'
                            (DateTime)metadata[Raven.Client.Constants.Documents.Metadata.LastModified],

                        HasCounters = 
                            // Specify the Client API Constant corresponding to '@counters'
                            metadata[Raven.Client.Constants.Documents.Metadata.Counters] != null
                    };
            }
        }
        #endregion
        
        #region index_3
        public class Products_ByMetadata_JS : AbstractJavaScriptIndexCreationTask
        {
            public Products_ByMetadata_JS()
            {
                Maps = new HashSet<string>
                {
                    @"map('Products', function (product) {
                        var metadata = metadataFor(product);

                        return {
                            LastModified: metadata['@last-modified'],
                            HasCounters: !!metadata['@counters']
                        };
                    })"
                };
            }
        }
        #endregion

        public Metadata()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region query_1
                    List<Product> productsWithCounters = session
                        .Query<Products_ByMetadata_AccessViaValue.IndexEntry,
                            Products_ByMetadata_AccessViaValue>()
                        .Where(x => x.HasCounters == true)
                        .OrderByDescending(x => x.LastModified)
                        .OfType<Product>()
                        .ToList();
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region query_1_async
                    List<Product> productsWithCounters = await asyncSession
                        .Query<Products_ByMetadata_AccessViaValue.IndexEntry,
                            Products_ByMetadata_AccessViaValue>()
                        .Where(x => x.HasCounters == true)
                        .OrderByDescending(x => x.LastModified)
                        .OfType<Product>()
                        .ToListAsync();
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region query_2
                    List<Product> productsWithCounters = session.Advanced.
                        DocumentQuery<Products_ByMetadata_AccessViaValue.IndexEntry,
                            Products_ByMetadata_AccessViaValue>()
                        .WhereEquals(x => x.HasCounters, true)
                        .OrderByDescending(x => x.LastModified)
                        .OfType<Product>()
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
