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