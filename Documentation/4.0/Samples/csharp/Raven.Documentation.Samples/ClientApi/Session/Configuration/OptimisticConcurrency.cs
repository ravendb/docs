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
                    session.Advanced.UseOptimisticConcurrency = true;

                    Product product = new Product { Name = "Some Name" };

                    session.Store(product, "products/999");
                    session.SaveChanges();

                    using (IDocumentSession otherSession = store.OpenSession())
                    {
                        Product otherProduct = otherSession.Load<Product>("products/999");
                        otherProduct.Name = "Other Name";

                        otherSession.SaveChanges();
                    }

                    product.Name = "Better Name";
                    session.SaveChanges(); // will throw ConcurrencyException
                }
                #endregion

                #region optimistic_concurrency_2
                store.Conventions.UseOptimisticConcurrency = true;

                using (IDocumentSession session = store.OpenSession())
                {
                    bool isSessionUsingOptimisticConcurrency = session.Advanced.UseOptimisticConcurrency; // will return true
                }
                #endregion

                #region optimistic_concurrency_3
                using (IDocumentSession session = store.OpenSession())
                {
                    session.Store(new Product { Name = "Some Name" }, id: "products/999");
                    session.SaveChanges();
                }

                using (IDocumentSession session = store.OpenSession())
                {
                    session.Advanced.UseOptimisticConcurrency = true;

                    session.Store(new Product { Name = "Some Other Name" }, changeVector: null, id: "products/999");
                    session.SaveChanges(); // will NOT throw Concurrency exception
                }
                #endregion

                #region optimistic_concurrency_4
                using (IDocumentSession session = store.OpenSession())
                {
                    session.Store(new Product { Name = "Some Name" }, id: "products/999");
                    session.SaveChanges();
                }

                using (IDocumentSession session = store.OpenSession())
                {
                    session.Advanced.UseOptimisticConcurrency = false; // default value

                    session.Store(new Product { Name = "Some Other Name" }, changeVector: string.Empty, id: "products/999");
                    session.SaveChanges(); // will throw Concurrency exception
                }
                #endregion
            }
        }
    }
}
