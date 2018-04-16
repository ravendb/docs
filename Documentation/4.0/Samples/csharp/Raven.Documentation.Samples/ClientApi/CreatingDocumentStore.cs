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
                Urls = new[] { "http://localhost:8080" }
            }.Initialize())
            {

            }

            #endregion
        }

        #region document_store_holder
        public class DocumentStoreHolder
        {
            private static Lazy<IDocumentStore> store = new Lazy<IDocumentStore>(CreateStore);

            public static IDocumentStore Store => store.Value;

            private static IDocumentStore CreateStore()
            {
                IDocumentStore store = new DocumentStore()
                {
                    Urls = new[] { "http://localhost:8080" },
                    Database = "Northwind"
                }.Initialize();

                return store;
            }
        }
        #endregion
    }
}
