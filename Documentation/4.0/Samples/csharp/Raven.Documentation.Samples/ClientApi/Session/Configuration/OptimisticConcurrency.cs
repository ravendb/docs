using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Configuration
{
    public class OptimisticConcurrency
    {
        public OptimisticConcurrency()
        {
            using (var store = new DocumentStore())
            {
                #region optimistic_concurrency_1
                using (IDocumentSession session = store.OpenSession())
                {
                    // Enable optimistic concurrency for this session
                    session.Advanced.UseOptimisticConcurrency = true;

                    // Save a document in this session
                    Product product = new Product { Name = "Some Name" };
                    session.Store(product, "products/999");
                    session.SaveChanges();

                    // Modify the document 'externally' by another session 
                    using (IDocumentSession otherSession = store.OpenSession())
                    {
                        Product otherProduct = otherSession.Load<Product>("products/999");
                        otherProduct.Name = "Other Name";
                        otherSession.SaveChanges();
                    }

                    // Trying to modify the document without reloading it first will throw
                    product.Name = "Better Name";
                    session.SaveChanges(); // This will throw a ConcurrencyException
                }
                #endregion

                #region optimistic_concurrency_2
                // Enable for all sessions that will be opened within this document store
                store.Conventions.UseOptimisticConcurrency = true;

                using (IDocumentSession session = store.OpenSession())
                {
                    bool isSessionUsingOptimisticConcurrency = session.Advanced.UseOptimisticConcurrency; // will return true
                }
                #endregion

                #region optimistic_concurrency_3
                using (IDocumentSession session = store.OpenSession())
                {
                    // Store document 'products/999'
                    session.Store(new Product { Name = "Some Name" }, id: "products/999");
                    session.SaveChanges();
                }

                using (IDocumentSession session = store.OpenSession())
                {
                    // Enable optimistic concurrency for the session
                    session.Advanced.UseOptimisticConcurrency = true;

                    // Store the same document
                    // Pass 'null' as the changeVector to turn OFF optimistic concurrency for this document
                    session.Store(new Product { Name = "Some Other Name" }, changeVector: null, id: "products/999");
                    
                    // This will NOT throw a ConcurrencyException, and the document will be saved
                    session.SaveChanges();
                }
                #endregion

                #region optimistic_concurrency_4
                using (IDocumentSession session = store.OpenSession())
                {
                    // Store document 'products/999'
                    session.Store(new Product { Name = "Some Name" }, id: "products/999");
                    session.SaveChanges();
                }

                using (IDocumentSession session = store.OpenSession())
                {
                    // Disable optimistic concurrency for the session 
                    session.Advanced.UseOptimisticConcurrency = false; // This is also the default value

                    // Store the same document
                    // Pass 'string.Empty' as the changeVector to turn ON optimistic concurrency for this document
                    session.Store(new Product { Name = "Some Other Name" }, changeVector: string.Empty, id: "products/999");
                    
                    // This will throw a ConcurrencyException, and the document will NOT be saved
                    session.SaveChanges();
                }
                #endregion
            }
        }
    }
}
