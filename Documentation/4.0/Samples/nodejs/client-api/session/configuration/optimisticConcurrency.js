import { DocumentStore, DocumentConventions } from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class Product { }

async function example() {
    //region optimistic_concurrency_1
    session.advanced.useOptimisticConcurrency = true;

    const product = new Product();
    product.name = "Some Name";

    await session.store(product, "products/999");
    await session.saveChanges();

    {
        const anotherSession = store.openSession();
        const otherProduct = await anotherSession.load("products/999");
        otherProduct.name = "Other Name";

        await anotherSession.saveChanges();
    }

    product.name = "Better Name";
    await session.saveChanges(); //  will throw ConcurrencyException error
    //endregion
                
    //region optimistic_concurrency_2
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

        await session.store(product, "products/999");
        await session.saveChanges();
    }

    {
        const session = store.openSession();
        session.advanced.useOptimisticConcurrency = true;

        const product = new Product();
        product.setName("Some Other Name");

        session.store(product, null, "products/999");
        session.saveChanges(); // will NOT throw Concurrency exception
    }
    //endregion

    //region optimistic_concurrency_4
    {
        const session = store.openSession();
        const product = new Product();
        product.name = "Some Name";
        await session.store(product, "products/999");
        await session.saveChanges();
    }

    {
        session.advanced.useOptimisticConcurrency = false; // default value

        const product = new Product();
        product.setName("Some Other Name");

        session.store(product, "", "products/999");
        session.saveChanges(); // will throw Concurrency exception
    }
    //endregion
}
