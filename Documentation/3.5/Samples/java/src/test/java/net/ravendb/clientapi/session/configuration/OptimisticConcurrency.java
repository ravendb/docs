package net.ravendb.clientapi.session.configuration;

import net.ravendb.abstractions.basic.CleanCloseable;
import net.ravendb.client.IDocumentSession;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentSession;
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

      //region optimistic_concurrency_2
      store.getConventions().setDefaultUseOptimisticConcurrency(true);

      try (IDocumentSession session = store.openSession()) {
        boolean isSessionUsingOptimisticConcurrency = session.advanced().isUseOptimisticConcurrency(); // will return true
      }
      //endregion

      //region optimistic_concurrency_3
      store.getConventions().setDefaultUseOptimisticConcurrency(true);

      try (DocumentSession session = (DocumentSession) store.openSession()) {
        try (CleanCloseable _ = session.getDatabaseCommands().forceReadFromMaster()) {
          // In replicated setup where ONLY reads are load balanced (FailoverBehavior.ReadFromAllServers)
          // and optimistic concurrency checks are turned on
          // you must set 'ForceReadFromMaster' to get the appropriate ETag value for the document
          // when you want to perform document updates (writes)
          // because writes will go to the master server and ETag values between servers are not synchronized

          Product product = session.load(Product.class, "products/999");
          product.setName("New Name");

          session.saveChanges();
        }
      }
      //endregion
    }
  }
}
