namespace Raven.Documentation.Samples.ClientApi
{
    using System;
    using Client.Documents;

    public class CreatingDocumentStore
    {
        public CreatingDocumentStore()
        {
            #region document_store_creation

            using (IDocumentStore store = new DocumentStore()
            {
                // Define the cluster nodes URLs
                Urls = new[] { "http://your_RavenDB_cluster_node_A", /*additional nodes of this cluster*/ }

                // Define a default database
                Database = "your_database_name"


            }.Initialize())
            {

            }

            #endregion
        }

        #region document_store_holder
        // The `DocumentStoreHolder` class holds a single Document Store instance.
        public class DocumentStoreHolder
        {
            // Use Lazy<IDocumentStore> to initialize the document store lazily, 
            // and ensure that it is created only once - when first accessing the public 'Store' property.
            private static Lazy<IDocumentStore> store = new Lazy<IDocumentStore>(CreateStore);

            public static IDocumentStore Store => store.Value;
            
            private static IDocumentStore CreateStore()
            {
                IDocumentStore store = new DocumentStore()
                {
                    Urls = new[] { "http://your_RavenDB_cluster_node_A", /*additional nodes of this cluster*/ },
                    Database = "your_database_name"
                }.Initialize();

                return store;
            }
        }
        #endregion
    }
}
