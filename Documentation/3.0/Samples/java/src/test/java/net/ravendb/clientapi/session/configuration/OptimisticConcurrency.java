package net.ravendb.clientapi.session.configuration;

import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.samples.northwind.Product;


public class OptimisticConcurrency {

  public OptimisticConcurrency() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region optimistic_concurrency_1
      try (IDocumentSession session = store.openSession()) {
        session.advanced().setUseOptimisticConcurrency(true);

        Product product = new Product();
        product.setName("Some Name");

        session.store(product, "products/999");
        session.saveChanges();

        try (IDocumentSession otherSession = store.openSession()) {
          Product otherProduct = otherSession.load(Product.class, "products/999");
          otherProduct.setName("Other name");

          otherSession.saveChanges();
        }

        product.setName("Better Name");
        session.saveChanges(); // throws ConcurrencyException
      }
      //endregion
    }
  }
}
