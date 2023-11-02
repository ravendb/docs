import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getIdentities() {
    {
        //region increment_identity
        // Create a document with an identity ID:
        // ======================================

        const session = documentStore.openSession();
        const company = new Company();
        company.name = "RavenDB";
        
        // Pass a collection name that ends with a pipe '|' to create an identity ID
        await session.store(company, "companies|");

        await session.saveChanges();
        // => Document "companies/1" will be created 

        // Increment the identity value on the server:
        // ===========================================

        // Define the next identity operation
        // Pass the collection name (can be with or without a pipe)
        const nextIdentityOp = new NextIdentityForOperation("companies|");

        // Execute the operation by passing it to maintenance.send
        // The latest value will be incremented to "2"
        // and the next document created with an identity will be assigned "3"
        const incrementedValue = await store.maintenance.send(nextIdentityOp);

        // Create another document with an identity ID:
        // ============================================
        
        const company = new Company();
        company.name = "AnotherComapany";
        
        await session.store(company, "companies|");
        await session.saveChanges();
        // => Document "companies/3" will be created
        //endregion
    }
}

{
    //region syntax
    const nextIdentityOp = new NextIdentityForOperation(name);
    //endregion
}
