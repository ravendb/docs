import { DocumentStore, DocumentConventions } from "ravendb";

const store = new DocumentStore();
class Product { }

async function example() {
    
    //region optimistic_concurrency_1
    // Enable optimistic concurrency for this session
    const session = store.openSession();
    session.advanced.useOptimisticConcurrency = true;
    
    const product = new Product();
    product.name = "Some Name";

    // Save a document in this session
    await session.store(product, "products/999");
    await session.saveChanges();

    {
        // Modify the document 'externally' by another session 
        const anotherSession = store.openSession();
        
        const otherProduct = await anotherSession.load("products/999");
        otherProduct.name = "Other Name";
        await anotherSession.saveChanges();
    }

    // Trying to modify the document without reloading it first will throw
    product.name = "Better Name";
    await session.saveChanges(); // This will throw a ConcurrencyException
    //endregion
                
    //region optimistic_concurrency_2
    // Enable for all sessions that will be opened within this document store
    store.conventions.useOptimisticConcurrency = true;

    {
        const session = store.openSession();
        const isSessionUsingOptimisticConcurrency
            = session.advanced.useOptimisticConcurrency; // true
    }
    //endregion

    //region optimistic_concurrency_3
    {        
        const session = store.openSession();
        
        const product = new Product();
        product.name = "Some Name";

        // Store document 'products/999'
        await session.store(product, "products/999");
        await session.saveChanges();
    }
    {
        const session = store.openSession();
        
        // Enable optimistic concurrency for the session
        session.advanced.useOptimisticConcurrency = true;

        const product = new Product();
        product.name = "Some Other Name";

        // Store the same document
        // Pass 'null' as the changeVector to turn OFF optimistic concurrency for this document
        await session.store(product, "products/999", { "changeVector": null });

        // This will NOT throw a ConcurrencyException, and the document will be saved
        await session.saveChanges();
    }
    //endregion

    //region optimistic_concurrency_4
    {
        const session = store.openSession();
        
        const product = new Product();
        product.name = "Some Name";

        // Store document 'products/999'
        await session.store(product, "products/999");
        await session.saveChanges();
    }
    {
        const session = store.openSession();
        
        // Disable optimistic concurrency for the session 
        session.advanced.useOptimisticConcurrency = false; // This is also the default value

        const product = new Product();
        product.name = "Some Other Name";

        // Store the same document
        // Pass an empty string as the changeVector to turn ON optimistic concurrency for this document
        await session.store(product, "products/999", { "changeVector": "" });

        // This will throw a ConcurrencyException, and the document will NOT be saved
        await session.saveChanges();
    }
    //endregion
}
