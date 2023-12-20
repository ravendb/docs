namespace Raven.Documentation.Samples.ClientApi
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using Client.Documents;
    using static Raven.Client.Constants;

    public class CreatingDocumentStore
    {

        #region document_store_holder
        // The `DocumentStoreHolder` class holds a single Document Store instance.
        public class DocumentStoreHolder
        {
            // Use Lazy<IDocumentStore> to initialize the document store lazily. 
            // This ensures that it is created only once - when first accessing the public `Store` property.
            private static Lazy<IDocumentStore> store = new Lazy<IDocumentStore>(CreateStore);

            public static IDocumentStore Store => store.Value;
            
            private static IDocumentStore CreateStore()
            {
                IDocumentStore store = new DocumentStore()
                {
                    // Define the cluster node URLs (required)
                    Urls = new[] { "http://your_RavenDB_cluster_node", 
                                   /*some additional nodes of this cluster*/ },

                    // Set conventions as necessary (optional)
                    Conventions =
                    {
                        MaxNumberOfRequestsPerSession = 10,
                        UseOptimisticConcurrency = true
                    },

                    // Define a default database (optional)
                    Database = "your_database_name",

                    // Define a client certificate (optional)
                    Certificate = new X509Certificate2("C:\\path_to_your_pfx_file\\cert.pfx"),

                    // Initialize the Document Store
                }.Initialize();

                // When the store is disposed of, the certificate file will be removed as well
                Store.AfterDispose += (sender, args) => Store.Certificate.Dispose();

                return store;
            }
        }
        #endregion
    }

    public class CertificateCleanup
    {
        // The `DocumentStoreHolder` class holds a single Document Store instance.
        public class DocumentStoreHolder
        {
            // Use Lazy<IDocumentStore> to initialize the document store lazily. 
            // This ensures that it is created only once - when first accessing the public `Store` property.
            private static Lazy<IDocumentStore> store = new Lazy<IDocumentStore>(CreateStore);

            public static IDocumentStore Store => store.Value;

            private static IDocumentStore CreateStore()
            {
                IDocumentStore store = new DocumentStore()
                {
                    // Define the cluster node URLs (required)
                    Urls = new[] { "http://your_RavenDB_cluster_node", 
                                   /*some additional nodes of this cluster*/ },

                    // Set conventions as necessary (optional)
                    Conventions =
                    {
                        MaxNumberOfRequestsPerSession = 10,
                        UseOptimisticConcurrency = true
                    },

                    // Define a default database (optional)
                    Database = "your_database_name",

                    // Define a client certificate (optional)
                    Certificate = new X509Certificate2("C:\\path_to_your_pfx_file\\cert.pfx"),

                    // Initialize the Document Store
                }.Initialize();

                #region certificate_cleanup
                // Use the AfterDispose store event to remove the certificate after the store is disposed of
                Store.AfterDispose += (sender, args) => Store.Certificate.Dispose();
                #endregion

                return store;
            }
        }
    }
}
