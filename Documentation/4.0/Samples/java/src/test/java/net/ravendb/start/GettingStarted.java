package net.ravendb.start;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.conventions.DocumentConventions;
import net.ravendb.client.documents.session.IDocumentSession;

import java.util.List;

public class GettingStarted {
    public void foo() {

        //region client_1
        try (IDocumentStore store = new DocumentStore(
            new String[]{ "http://live-test.ravendb.net" },        // URL to the Server,
            // or list of URLs
            // to all Cluster Servers (Nodes)
            "Northwind")                                           // Default database that DocumentStore will interact with
        ) {

            DocumentConventions conventions = store.getConventions();  // DocumentStore customizations

            store.initialize();                                        // Each DocumentStore needs to be initialized before use.
            // This process establishes the connection with the Server
            // and downloads various configurations
            // e.g. cluster topology or client configuration
        }
        //endregion

        {
            IDocumentStore store = new DocumentStore();
            //region client_2
            try (IDocumentSession session = store.openSession()) {      // Open a session for a default 'Database'
                Category category = new Category();
                category.setName("Database Category");

                session.store(category);                            // Assign an 'Id' and collection (Categories)
                // and start tracking an entity

                Product product = new Product();
                product.setName("RavenDB Database");
                product.setCategory(category.getId());
                product.setUnitsInStock(10);

                session.store(product);                             // Assign an 'Id' and collection (Products)
                // and start tracking an entity

                session.saveChanges();                              // Send to the Server
                // one request processed in one transaction
            }
            //endregion
        }


        IDocumentStore store = null;
        String productId = null;

        //region client_3
        try (IDocumentSession session = store.openSession()) {     // Open a session for a default 'Database'
            Product product = session
                .include("category")                        // Include Category
                .load(Product.class, productId);            // Load the Product and start tracking

            Category category = session
                .load(Category.class,                       // No remote calls,
                    product.getCategory());             // Session contains this entity from .include

            product.setName("RavenDB");                         // Apply changes
            category.setName("Database");


            session.saveChanges();                              // Synchronize with the Server
            // one request processed in one transaction
        }
        //endregion

        //region client_4
        try (IDocumentSession session = store.openSession()) {      // Open a session for a default 'Database'
            List<String> productNames = session
                .query(Product.class)                       // Query for Products
                .whereGreaterThan("unitsInStock", 5)        // Filter
                .skip(0).take(10)                           // Page
                .selectFields(String.class, "name")         // Project
                .toList();                                  // Materialize query
        }
        //endregion
    }

    private static class Product {
        private String name;
        private String category;
        private int unitsInStock;

        public String getCategory() {
            return category;
        }

        public void setCategory(String category) {
            this.category = category;
        }

        public int getUnitsInStock() {
            return unitsInStock;
        }

        public void setUnitsInStock(int unitsInStock) {
            this.unitsInStock = unitsInStock;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }

    private static class Category {
        private String id;
        private String name;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }
    }
}
