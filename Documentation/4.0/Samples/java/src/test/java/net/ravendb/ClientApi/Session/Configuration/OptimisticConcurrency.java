package net.ravendb.ClientApi.Session.Configuration;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.session.IDocumentSession;

public class OptimisticConcurrency {
    private static class Product {
        private String name;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }

    public OptimisticConcurrency() {
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
                    otherProduct.setName("Other Name");

                    otherSession.saveChanges();
                }

                product.setName("Better Name");
                session.saveChanges(); //  will throw ConcurrencyException
            }
            //endregion

            //region optimistic_concurrency_2
            store.getConventions().setUseOptimisticConcurrency(true);

            try (IDocumentSession session = store.openSession()) {
                boolean isSessionUsingOptimisticConcurrency
                    = session.advanced().isUseOptimisticConcurrency(); // will return true
            }
            //endregion

            //region optimistic_concurrency_3
            try (IDocumentSession session = store.openSession()) {
                Product product = new Product();
                product.setName("Some Name");

                session.store(product, "products/999");
                session.saveChanges();
            }

            try (IDocumentSession session = store.openSession()) {
                session.advanced().setUseOptimisticConcurrency(true);

                Product product = new Product();
                product.setName("Some Other Name");

                session.store(product, null, "products/999");
                session.saveChanges(); // will NOT throw Concurrency exception
            }
            //endregion

            //region optimistic_concurrency_4
            try (IDocumentSession session = store.openSession()) {
                Product product = new Product();
                product.setName("Some Name");
                session.store(product, "products/999");
                session.saveChanges();
            }

            try (IDocumentSession session = store.openSession()) {
                session.advanced().setUseOptimisticConcurrency(false); // default value

                Product product = new Product();
                product.setName("Some Other Name");

                session.store(product, "", "products/999");
                session.saveChanges(); // will throw Concurrency exception
            }
            //endregion
        }
    }
}
