namespace Raven.Documentation.Samples.ClientApi
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using Client.Documents;

    public class CreatingDocumentStore
    {
        public CreatingDocumentStore()
        {
            #region document_store_creation

            using (IDocumentStore store = new DocumentStore()
            {
                // Required: define the server node URLs
                Urls = new[] { "http://your_RavenDB_cluster_node_A", /*some additional nodes of this cluster*/ },

                // Change some of the conventions
                Conventions =
                {
                    MaxNumberOfRequestsPerSession = 10,
                    UseOptimisticConcurrency = true
                },

                // Define a default database
                Database = "your_database_name",

                // Define a client certificate
                Certificate = new X509Certificate2("C:\\path_to_your_pfx_file\\cert.pfx"),
            }.Initialize())
            {
                // Do your work here
            }
            #endregion
        }

        #region document_store_holder
        // The `DocumentStoreHolder` class holds a single Document Store instance.
        public class DocumentStoreHolder
        {
            // Use Lazy<IDocumentStore> to initialize the document store lazily, 
            // and ensure that it is created only once - when first accessing the public `Store` property.
            private static Lazy<IDocumentStore> store = new Lazy<IDocumentStore>(CreateStore);

            public static IDocumentStore Store => store.Value;
            
            private static IDocumentStore CreateStore()
            {
                IDocumentStore store = new DocumentStore()
                {
                    Urls = new[] { "http://your_RavenDB_cluster_node_A", /*additional nodes of your cluster*/ },
                    
                    // Make other configurations as necessary
                    Database = "your_database_name"
                }.Initialize();

                return store;
            }
        }
        #endregion
    }
}
