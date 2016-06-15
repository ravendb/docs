using Raven.Client;
using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Orders;

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
				store.Conventions.DefaultUseOptimisticConcurrency = true;

				using (IDocumentSession session = store.OpenSession())
				{
					bool isSessionUsingOptimisticConcurrency = session.Advanced.UseOptimisticConcurrency; // will return true
				}
                #endregion

                #region optimistic_concurrency_3
                store.Conventions.DefaultUseOptimisticConcurrency = true;

                using (DocumentSession session = (DocumentSession)store.OpenSession())
                using (session.DatabaseCommands.ForceReadFromMaster())
                {
                    // In replicated setup where ONLY reads are load balanced (FailoverBehavior.ReadFromAllServers)
                    // and optimistic concurrency checks are turned on
                    // you must set 'ForceReadFromMaster' to get the appropriate ETag value for the document
                    // when you want to perform document updates (writes)
                    // because writes will go to the master server and ETag values between servers are not synchronized

                    Product product = session.Load<Product>("products/999");
                    product.Name = "New Name";

                    session.SaveChanges();
                }
                #endregion
            }
		}
	}
}